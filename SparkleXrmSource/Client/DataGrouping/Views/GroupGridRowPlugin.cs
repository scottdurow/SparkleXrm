// GroupGridRowPlugin.cs
//

using Slick;
using System;
using System.Collections.Generic;

namespace Client.DataGrouping.Views
{
    public class GroupGridRowPlugin : IPlugin
    {
        private Grid _grid;
        public void Init(Grid grid) {
            
            _grid = grid;
            _grid.OnClick.Subscribe(HandleGridClick);
            //_grid.OnKeyDown.Subscribe(handleGridKeyDown);
            
        }

        public void Destroy() {
            
            if (_grid!=null) {
                _grid.OnClick.Unsubscribe(HandleGridClick);
                //_grid.OnKeyDown.Unsubscribe(handleGridKeyDown);
            }
        }

        public void HandleGridClick(EventData e, object a) {
            CellSelection args = (CellSelection)a;

            object item = _grid.GetDataItem(args.Row.Value);
            if (item.GetType()==typeof(Group))
            {
                IGroupedDataView dataView = (IGroupedDataView)_grid.GetData();
                Group group = (Group)item;
                if (group.Collapsed)
                {
                   dataView.ExpandGroup(group.GroupingKey);
                }
                else
                {
                    dataView.CollapseGroup(group.GroupingKey);
                }
                e.StopImmediatePropagation();
                e.PreventDefault();
            }
           
        }

    }
}
