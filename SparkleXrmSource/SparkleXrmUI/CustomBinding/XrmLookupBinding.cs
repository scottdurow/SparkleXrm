// CrmLookupBinding.cs
//

using jQueryApi;
using jQueryApi.UI;
using jQueryApi.UI.Widgets;
using KnockoutApi;
using SparkleXrm.GridEditor;
using System;
using System.Collections.Generic;
using Xrm.Sdk;
using Xrm.Sdk.Metadata;

namespace SparkleXrm.CustomBinding
{
    public class XrmLookupBinding : BindingHandler
    {
        static XrmLookupBinding()
        {
            if ((string)Script.Literal("typeof(ko)") != "undefined")
            {
                Knockout.BindingHandlers["lookup"] = new XrmLookupBinding();
            }
        }

        public override void Init(System.Html.Element element, Func<object> valueAccessor, Func<System.Collections.Dictionary> allBindingsAccessor, object viewModel, object context)
        {
            XrmLookupEditorButton footerButton = (XrmLookupEditorButton)allBindingsAccessor()["footerButton"];
            bool showFooter = (bool)allBindingsAccessor()["showFooter"];
            jQueryObject container = jQuery.FromElement(element);
            jQueryObject hiddenInputValue = container.Find(".sparkle-input-lookup-hidden-part");
            jQueryObject inputField = container.Find(".sparkle-input-lookup-part");
            jQueryObject selectButton = container.Find(".sparkle-input-lookup-button-part");
            EntityReference _value = new EntityReference(null, null, null);
            AutoCompleteOptions options = new AutoCompleteOptions();
            options.MinLength = 100000; // Don't enable type down - use search button (or return)
            options.Delay = 0;
            options.Position = new Dictionary<string, object>("collision", "fit");
            bool justSelected = false;
            int totalRecordsReturned = 0;
            Action<AutoCompleteItem, bool> setValue = delegate(AutoCompleteItem item, bool setFocus)
            {
                if (_value == null) _value = new EntityReference(null, null, null);
                string value = item.Label;
                inputField.Value(value);
                hiddenInputValue.Value(value);
                _value.Id = ((Guid)item.Value);
                _value.Name = item.Label;
                _value.LogicalName = (string)item.Data;
                justSelected = true;
                TrySetObservable(valueAccessor, inputField, _value, setFocus);
            };

            // Set the value when selected
            options.Select = delegate(jQueryEvent e, AutoCompleteSelectEvent uiEvent)
            {
                // Note we assume that the binding has added an array of string items
                AutoCompleteItem item = (AutoCompleteItem)uiEvent.Item;
                string data = ((string)item.Data);
                if (data == "footerlink" || data == null)
                {
                    footerButton.OnClick(item);
                    e.PreventDefault();
                    e.StopImmediatePropagation();
                    Script.Literal("return false;");
                }
                else
                {
                    setValue(item, true);
                    Script.Literal("return false;");
                }
            };

            options.Open = delegate(jQueryEvent e, jQueryObject o)
            {

                if (showFooter && totalRecordsReturned > 0)
                {
                    WidgetObject menu = (WidgetObject)Script.Literal("{0}.autocomplete({1})", inputField, "widget");
                    XrmLookupEditor.AddFooter(menu, totalRecordsReturned);
                }
            };

            options.Close = delegate(jQueryEvent e, jQueryObject o)
            {

                WidgetObject menu = (WidgetObject)Script.Literal("{0}.autocomplete({1})", inputField, "widget");
                jQueryObject footer = menu.Next();
                if (footer.Length > 0 || footer.HasClass("sparkle-menu-footer"))
                {
                    footer.Hide();
                }
            };
            // Get the query command
            Action<string, Action<EntityCollection>> queryCommand = (Action<string, Action<EntityCollection>>)((object)allBindingsAccessor()["queryCommand"]);
            string nameAttribute = ((string)allBindingsAccessor()["nameAttribute"]);
            string idAttribute = ((string)allBindingsAccessor()["idAttribute"]);
            string typeCodeAttribute = ((string)allBindingsAccessor()["typeCodeAttribute"]);
            string[] columnAttributes = null;
            // If there multiple names, add them to the columnAttributes
            string[] columns = nameAttribute.Split(",");

            if (columns.Length > 1)
            {
                columnAttributes = columns;
                nameAttribute = columnAttributes[0];
            }

            // wire up source to CRM search
            Action<AutoCompleteRequest, Action<AutoCompleteItem[]>> queryDelegate = delegate(AutoCompleteRequest request, Action<AutoCompleteItem[]> response)
            {
                Action<EntityCollection> queryCallBack = delegate(EntityCollection fetchResult)
                    {
                        int recordsFound = fetchResult.Entities.Count;
                        bool noRecordsFound = recordsFound == 0;
                        AutoCompleteItem[] results = new AutoCompleteItem[recordsFound + (footerButton != null ? 1 : 0) + (noRecordsFound ? 1 : 0)];

                        for (int i = 0; i < recordsFound; i++)
                        {
                            results[i] = new AutoCompleteItem();
                            results[i].Label = (string)fetchResult.Entities[i].GetAttributeValue(nameAttribute);
                            results[i].Value = fetchResult.Entities[i].GetAttributeValue(idAttribute);
                            results[i].Data = fetchResult.Entities[i].LogicalName;
                            GetExtraColumns(columnAttributes, fetchResult, results, i);

                            string typeCodeName = fetchResult.Entities[i].LogicalName;
                            // Get the type code from the name to find the icon
                            if (!string.IsNullOrEmpty(typeCodeAttribute))
                            {
                                typeCodeName = fetchResult.Entities[i].GetAttributeValue(typeCodeAttribute).ToString();
                            }

                            results[i].Image = MetadataCache.GetSmallIconUrl(typeCodeName);
                        }

                        if (fetchResult.TotalRecordCount > fetchResult.Entities.Count)
                        {
                            totalRecordsReturned = fetchResult.TotalRecordCount;
                        }
                        else
                        {
                            totalRecordsReturned = fetchResult.Entities.Count;
                        }
                        int itemsCount = recordsFound;
                        if (noRecordsFound)
                        {
                            AutoCompleteItem noRecordsItem = new AutoCompleteItem();
                            noRecordsItem.Label = SparkleResourceStrings.NoRecordsFound;
                            results[itemsCount] = noRecordsItem;
                            itemsCount++;
                        }
                        if (footerButton != null)
                        {
                            // Add the add new
                            AutoCompleteItem addNewLink = new AutoCompleteItem();
                            addNewLink.Label = footerButton.Label;
                            addNewLink.Image = footerButton.Image;
                            addNewLink.ColumnValues = null;
                            addNewLink.Data = "footerlink";
                            results[itemsCount] = addNewLink;
                        }
                        response(results);

                        // Disable it now so typing doesn't trigger a search
                        AutoCompleteOptions disableOption = new AutoCompleteOptions();
                        disableOption.MinLength = 100000;
                        inputField.Plugin<AutoCompleteObject>().AutoComplete(disableOption);
                    };

                // Call the function with the correct 'this' context
                Script.Literal("{0}.call({1}.$parent,{2},{3})", queryCommand, context, request.Term, queryCallBack);
            };

            options.Source = queryDelegate;
            options.Focus = delegate(jQueryEvent e, AutoCompleteFocusEvent uiEvent)
            {
                // Prevent the value being updated in the text box we scroll through the results
                Script.Literal("return false;");
            };
            inputField = inputField.Plugin<AutoCompleteObject>().AutoComplete(options);

            // Set render template
            ((RenderItemDelegate)Script.Literal("{0}.data('ui-autocomplete')", inputField))._renderItem = delegate(object ul, AutoCompleteItem item)
            {
                if (item.Data == null)
                {
                    return (object)jQuery.Select("<li class='ui-state-disabled'>" + item.Label + "</li>").AppendTo((jQueryObject)ul);
                }

                string html = "<a class='sparkle-menu-item'><span class='sparkle-menu-item-img'>";
                if (item.Image != null)
                {
                    html += @"<img src='" + item.Image + "'/>";
                }
                html += @"</span><span class='sparkle-menu-item-label'>" + item.Label + "</span><br>";

                if (item.ColumnValues != null && item.ColumnValues.Length > 0)
                {
                    foreach (string value in item.ColumnValues)
                    {
                        html += "<span class='sparkle-menu-item-moreinfo'>" + value + "</span>";
                    }
                }
                html += "</a>";
                return (object)jQuery.Select("<li>").Append(html).AppendTo((jQueryObject)ul);

            };

            // Add the click binding to show the drop down
            selectButton.Click(delegate(jQueryEvent e)
            {
                inputField.Value(hiddenInputValue.GetValue());
                AutoCompleteOptions enableOption = new AutoCompleteOptions();
                enableOption.MinLength = 0;
                inputField.Focus();
                inputField.Plugin<AutoCompleteObject>().AutoComplete(enableOption);
                inputField.Plugin<AutoCompleteObject>().AutoComplete(AutoCompleteMethod.Search);
            });

            // handle the field changing
            inputField.FocusIn(delegate(jQueryEvent e)
            {
                hiddenInputValue.Value("");
            });

            // handle the field changing
            inputField.Change(delegate(jQueryEvent e)
            {
                hiddenInputValue.Value(inputField.GetValue());
                string inputValue = hiddenInputValue.GetValue();
                if (inputValue != _value.Name)
                {

                    // The name is different from the name of the lookup reference
                    // search to see if we can auto resolve it

                    TrySetObservable(valueAccessor, inputField, null, false);
                    AutoCompleteRequest lookup = new AutoCompleteRequest();
                    lookup.Term = inputValue;
                    Action<AutoCompleteItem[]> lookupResults = delegate(AutoCompleteItem[] results)
                    {
                        int selectableItems = 0;
                        // If there is only one, then auto-set
                        if (results != null)
                        {
                            foreach (AutoCompleteItem item in results)
                            {
                                if (isItemSelectable(item))
                                {
                                    selectableItems++;
                                }
                                if (selectableItems > 2)
                                    break;
                            }
                        }

                        if (selectableItems == 1)
                        {
                            // There is only a single value so set it now
                            setValue(results[0], false);
                        }
                        else
                        {
                            inputField.Value(String.Empty);
                        }
                    };

                    queryDelegate(lookup, lookupResults);
                }



            });

            Action disposeCallBack = delegate()
            {
                if ((bool)Script.Literal("$({0}).data('ui-autocomplete')!=undefined", inputField))
                {
                    Script.Literal("$({0}).autocomplete(\"destroy\")", inputField);
                }
            };

            //handle disposal (if KO removes by the template binding)
            Script.Literal("ko.utils.domNodeDisposal.addDisposeCallback({0}, {1})", element, (object)disposeCallBack);
            Knockout.BindingHandlers["validationCore"].Init(element, valueAccessor, allBindingsAccessor, null, null);

            // Bind return to searching 
            inputField.Keydown(delegate(jQueryEvent e)
            {
                if (e.Which == 13 && !justSelected) // Return pressed - but we want to do a search not move to the next cell
                {
                    selectButton.Click();
                }
                else if (e.Which == 13)
                {
                    return;
                }
                switch (e.Which)
                {
                    case 13: // Return
                    case 38: // Up - don't navigate - but use the dropdown to select search results
                    case 40: // Down - don't navigate - but use the dropdown to select search results
                        e.PreventDefault();
                        e.StopPropagation();
                        break;
                }
                justSelected = false;
            });

            //Script.Literal("return { controlsDescendantBindings: true };");
        }
        private static bool isItemSelectable(AutoCompleteItem item)
        {
            string data = (string)item.Data;
            return data != null && data != "footerlink";
        }
        /// <summary>
        /// Extracts additional column display values from the search result
        /// The name attribute can now be a column separated list of attribute logical names
        /// </summary>
        /// <param name="columnAttributes"></param>
        /// <param name="fetchResult"></param>
        /// <param name="results"></param>
        /// <param name="i"></param>
        internal static void GetExtraColumns(string[] columnAttributes, EntityCollection fetchResult, AutoCompleteItem[] results, int i)
        {
            if (columnAttributes != null)
            {
                List<string> columnValues = new List<string>();
                bool first = true;
                // Get the additional column attributes to display
                foreach (string attribute in columnAttributes)
                {
                    if (first)
                    {
                        first = false;
                        continue;
                    }
                    string value = "";
                    Entity record = fetchResult.Entities[i];

                    if (record.FormattedValues.ContainsKey(attribute + "name"))
                    {
                        value = record.FormattedValues[attribute + "name"];
                    }
                    else
                    {
                        object attributeValue = record.GetAttributeValue(attribute);
                        if (attributeValue != null)
                        {
                            switch (attributeValue.GetType().Name)
                            {
                                case "EntityReference":
                                    value = ((EntityReference)attributeValue).Name;
                                    break;
                                default:
                                    value = attributeValue.ToString();
                                    break;
                            }
                        }
                    }
                    if (value != null && value.Length > 0)
                    {
                        columnValues.Add(value);
                    }

                }
                results[i].ColumnValues = (string[])columnValues;
            }
        }

        private static void TrySetObservable(Func<object> valueAccessor, jQueryObject inputField, EntityReference value, bool setFocus)
        {
            Observable<EntityReference> observable = (Observable<EntityReference>)valueAccessor();
            bool isValid = true;
            observable.SetValue(value);

            if (((string)Script.Literal("typeof({0}.isValid)", observable)) != "undefined")
            {
                isValid = ((IValidatedObservable)observable).IsValid() == true;
            }

            if (isValid && setFocus)
            {
                // Ensure the field is reinitialised
                inputField.Blur();
                inputField.Focus();
            }
        }

        public override void Update(System.Html.Element element, Func<object> valueAccessor, Func<System.Collections.Dictionary> allBindingsAccessor, object viewModel, object context)
        {
            jQueryObject container = jQuery.FromElement(element);
            jQueryObject inputField = container.Find(".sparkle-input-lookup-part");
            EntityReference value = (EntityReference)KnockoutUtils.UnwrapObservable(valueAccessor());

            string displayName = "";
            if (value != null) displayName = value.Name;
            inputField.Value(displayName);
        }
    }
}
