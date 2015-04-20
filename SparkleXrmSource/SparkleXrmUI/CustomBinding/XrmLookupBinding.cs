// CrmLookupBinding.cs
//

using jQueryApi;
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
                //ValidationApi.MakeBindingHandlerValidatable("lookup");
            }
        }
     
        public override void Init(System.Html.Element element, Func<object> valueAccessor, Func<System.Collections.Dictionary> allBindingsAccessor, object viewModel, object context)
        {
            jQueryObject container = jQuery.FromElement(element);
            jQueryObject inputField = container.Find(".sparkle-input-lookup-part");
            jQueryObject selectButton = container.Find(".sparkle-input-lookup-button-part");
            EntityReference _value = new EntityReference(null, null, null);
            AutoCompleteOptions options = new AutoCompleteOptions();
            options.MinLength = 100000; // Don't enable type down - use search button (or return)
            options.Delay = 0;
            options.Position = new Dictionary<string, object>("collision", "fit");
            
            bool justSelected = false;

            // Set the value when selected
            options.Select = delegate(jQueryEvent e, AutoCompleteSelectEvent uiEvent)
            {

                // Note we assume that the binding has added an array of string items
                AutoCompleteItem item = (AutoCompleteItem)uiEvent.Item;
                if (_value == null) _value = new EntityReference(null, null, null);
                string value = item.Label;
                inputField.Value(value);
                _value.Id = ((Guid)item.Value);
                _value.Name = item.Label;
                _value.LogicalName = (string)item.Data;
                justSelected = true;
                TrySetObservable(valueAccessor, inputField, _value);
                Script.Literal("return false;");

            };

            
            // Get the query command
            Action<string, Action<EntityCollection>> queryCommand = (Action<string, Action<EntityCollection>>)((object)allBindingsAccessor()["queryCommand"]);
            string nameAttribute = ((string)allBindingsAccessor()["nameAttribute"]);
            string idAttribute = ((string)allBindingsAccessor()["idAttribute"]);
            string typeCodeAttribute = ((string)allBindingsAccessor()["typeCodeAttribute"]);

            // wire up source to CRM search
            Action<AutoCompleteRequest, Action<AutoCompleteItem[]>> queryDelegate = delegate(AutoCompleteRequest request, Action<AutoCompleteItem[]> response)
            {
                Action<EntityCollection> queryCallBack = delegate(EntityCollection fetchResult)
                    {
                        AutoCompleteItem[] results = new AutoCompleteItem[fetchResult.Entities.Count];

                        for (int i = 0; i < fetchResult.Entities.Count; i++)
                        {
                            results[i] = new AutoCompleteItem();
                            results[i].Label = (string)fetchResult.Entities[i].GetAttributeValue(nameAttribute);
                            results[i].Value = fetchResult.Entities[i].GetAttributeValue(idAttribute);
                            results[i].Data = fetchResult.Entities[i].LogicalName;
                            string typeCodeName = fetchResult.Entities[i].LogicalName;
                            // Get the type code from the name to find the icon
                            if (!string.IsNullOrEmpty(typeCodeAttribute))
                            {
                                typeCodeName = fetchResult.Entities[i].GetAttributeValue(typeCodeAttribute).ToString();
                            }

                            results[i].Image = MetadataCache.GetSmallIconUrl(typeCodeName);
                        }


                        response(results);

                        // Disable it now so typing doesn't trigger a search
                        AutoCompleteOptions disableOption = new AutoCompleteOptions();
                        disableOption.MinLength = 100000;
                        inputField.Plugin<AutoCompleteObject>().AutoComplete(disableOption);
                    };

                // Call the function with the correct 'this' context
                Script.Literal("{0}.call({1}.$parent,{2},{3})",queryCommand, context, request.Term, queryCallBack);
                   

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

                return (object)jQuery.Select("<li>").Append("<a class='sparkle-menu-item'><span class='sparkle-menu-item-img'><img src='" + item.Image + "'/></span><span class='sparkle-menu-item-label'>" + item.Label + "</span></a>").AppendTo((jQueryObject)ul);

            };

            // Add the click binding to show the drop down
            selectButton.Click(delegate(jQueryEvent e)
            {
                AutoCompleteOptions enableOption = new AutoCompleteOptions();
                enableOption.MinLength = 0;
                inputField.Focus();
                inputField.Plugin<AutoCompleteObject>().AutoComplete(enableOption);

                inputField.Plugin<AutoCompleteObject>().AutoComplete(AutoCompleteMethod.Search);
            });

            // handle the field changing
            inputField.Change(delegate(jQueryEvent e)
            {
                if (inputField.GetValue() != _value.Name)
                    TrySetObservable(valueAccessor, inputField, null);
            });

            Action disposeCallBack = delegate()
            {
                if ((bool)Script.Literal("$({0}).data('ui-autocomplete')!=undefined", inputField))
                {
                    //inputField.Plugin<AutoCompleteObject>().AutoComplete(AutoCompleteMethod.Destroy);
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

        private static void TrySetObservable(Func<object> valueAccessor, jQueryObject inputField, EntityReference value)
        {

            Observable<EntityReference> observable = (Observable<EntityReference>)valueAccessor();
            bool isValid = true;
            observable.SetValue(value);

            if (((string)Script.Literal("typeof({0}.isValid)", observable)) != "undefined")
            {
                isValid = ((IValidatedObservable)observable).IsValid() == true;
            }

            if (isValid)
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
