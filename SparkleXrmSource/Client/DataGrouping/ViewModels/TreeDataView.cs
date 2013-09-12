// TreeDataView.cs
//

using Slick;
using SparkleXrm.GridEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xrm;
using Xrm.Sdk;

namespace Client.DataGrouping.ViewModels
{
    public class TreeDataView : DataViewBase, IGroupedDataView
    {
        private bool _suspend = false;
        private int? _refreshRowsAfter;
        private Dictionary<string, bool> _groupStates = new Dictionary<string, bool>();
        private Dictionary<string, Group> groups = new Dictionary<string, Group>();
        public List<object> _items = new List<object>();
        public List<object> _rows = new List<object>();
        private SortCol _sortColumn;
        public TreeDataView()
        {
            
        }
        public void BeginUpdate()
        {
            _suspend = true;
        }
        public void EndUpdate()
        {
            _suspend = false;
            Refresh();
        }
        public override void Sort(SortColData sorting)
        {

            _sortColumn = new SortCol(sorting.SortCol.Field, sorting.SortAsc);

            Refresh();
        }
        public override void AddItem(object item)
        {
            _items.Add(item);
            Refresh();
        }
        public override void InsertItem(int insertBefore, object item)
        {
            _items.Insert(insertBefore, item);
            Refresh();
        }
        public override void RemoveItem(object id)
        {
            _items.Remove(id);
            Refresh();
        }
        public override object GetItem(int index)
        {
            return _rows[index];
        }
        public override int GetLength()
        {
            return _rows.Count;
        }
        public override ItemMetaData GetItemMetadata(int i)
        {
           
            return base.GetItemMetadata(i);
        }

        public void ExpandGroup(string groupingKey)
        {
            if (_groupStates.ContainsKey(groupingKey))
            {
                _groupStates[groupingKey] = !_groupStates[groupingKey];
            }
            _refreshRowsAfter = groups[groupingKey].Count - 1;
           Refresh();
        }

        public void CollapseGroup(string groupingKey)
        {
            if (_groupStates.ContainsKey(groupingKey))
            {
                _groupStates[groupingKey] = !_groupStates[groupingKey];
            }
            _refreshRowsAfter = groups[groupingKey].Count-1;
           Refresh();
        }

        public override void Refresh()
        {
            if (_suspend)
                return;

            // Flattern groups
            List<object> rows = new List<object>();
            FlatternGroups(rows, null,null, 0);
            _rows = rows;
         
            // Get the rows to invalidate
            OnRowsChangedEventArgs args = new OnRowsChangedEventArgs();
            args.Rows = new List<int>();
            int? startDiffRow = _refreshRowsAfter!=null ?_refreshRowsAfter : 0;

            for (int i = startDiffRow.Value; i < rows.Count; i++)
            {
                args.Rows.Add(i);
            }
           
            this.OnRowsChanged.Notify(args, null, null);
           


        }

        private void FlatternGroups(List<object> rows, GroupedItem parent,Group parentGroup, int? parentLevel)
        {
            List<object> items;
            int? level = parentLevel==null ? 0 : parentLevel.Value+1;
            if (parent!=null)
            {
                items = parent.ChildItems;
            }
            else
            {
                items = _items;
            }

            List<object> rowsToAdd = new List<object>();
           
            List<object> sortedItems = new List<object>(items);
            for (int i = 0; i < items.Count; i++)
            {
                sortedItems[i] = items[i];
            }
            if (_sortColumn != null)
                SortBy(_sortColumn, sortedItems);

            foreach (object item in sortedItems)
            {
                GroupedItem groupedItem = (GroupedItem)item;
              
                // Is the item a group?
                if ((string)Script.Literal("typeof({0})", groupedItem.ChildItems) != "undefined")
                {
                    // Is the group expanded or collapsed?

                    // Add group
                    string groupingKey = (parent!=null ? parent.Id : "") + "|" + groupedItem.Id;
                    Group group;
                    if (groups.ContainsKey(groupingKey))
                    {
                        group = groups[groupingKey];
                    }
                    else
                    {
                        group = new Group();
                        groups[groupingKey] = group;
                    }
                    group.Title = groupedItem.Title.ToString();
                    group.Rows = new List<object>();
                    group.GroupingKey = groupingKey;
                   
                    bool collapsed = false;

                    if (_groupStates.ContainsKey(groupingKey))
                    {
                        collapsed = _groupStates[groupingKey];
                    }
                    else
                    {
                        _groupStates[groupingKey] = collapsed;
                    }

                    group.Level = level.Value;
                    rowsToAdd.Add(group);
                    group.Count = rows.Count + rowsToAdd.Count;
                    group.Collapsed = collapsed;

                    if (!collapsed)
                    {
                        // Add child rows
                        FlatternGroups(rowsToAdd, groupedItem, group, level);
                        
                    }

                }
                else
                {
                    // Just add the item
                    rowsToAdd.Add(item);
                   

                }
            }

           
            rows.AddRange(rowsToAdd);

        }
        private void SortBy(SortCol col,List<object> data)
        {
            // From SlickGrid : an extra reversal for descending sort keeps the sort stable
            // (assuming a stable native sort implementation, which isn't true in some cases)
            if (col.Ascending == false)
            {
                data.Reverse();
            }
            data.Sort(delegate(object a, object b)
            {
         
                object l = ((Dictionary<string,object>)a)[col.AttributeName];
                object r = ((Dictionary<string, object>)b)[col.AttributeName];
                decimal result = 0;

                string typeName = "";
                if (l != null)
                    typeName = l.GetType().Name;
                else if (r != null)
                    typeName = r.GetType().Name;

                if (l != r)
                {
                    switch (typeName.ToLowerCase())
                    {
                        case "string":
                            l = l != null ? ((string)l).ToLowerCase() : null;
                            r = r != null ? ((string)r).ToLowerCase() : null;
                            if ((bool)Script.Literal("{0}<{1}", l, r))
                                result = -1;
                            else
                                result = 1;
                            break;
                        case "date":
                            if ((bool)Script.Literal("{0}<{1}", l, r))
                                result = -1;
                            else
                                result = 1;
                            break;
                        case "number":
                            decimal ln = l != null ? ((decimal)l) : 0;
                            decimal rn = r != null ? ((decimal)r) : 0;
                            result = (ln - rn);
                            break;
                        case "money":
                            decimal lm = l != null ? ((Money)l).Value : 0;
                            decimal rm = r != null ? ((Money)r).Value : 0;
                            result = (lm - rm);
                            break;
                        case "optionsetvalue":
                            int? lo = l != null ? ((OptionSetValue)l).Value : 0;
                            lo = lo != null ? lo : 0;
                            int? ro = r != null ? ((OptionSetValue)r).Value : 0;
                            ro = ro != null ? ro : 0;
                            result = (decimal)(lo - ro);
                            break;
                        case "entityreference":
                            string le = (l != null) && (((EntityReference)l).Name != null) ? ((EntityReference)l).Name : "";
                            string re = r != null && (((EntityReference)r).Name != null) ? ((EntityReference)r).Name : "";
                            if ((bool)Script.Literal("{0}<{1}", le, re))
                                result = -1;
                            else
                                result = 1;
                            break;

                    }
                }
                return (int)result;
            });

            if (col.Ascending == false)
            {
                data.Reverse();
            }

        }
    }

    [Imported]
    [ScriptName("Object")]
    [IgnoreNamespace]
    public class GroupedItem
    {
        public object Id;
        public object Title;
        public List<object> ChildItems;
    }
}
