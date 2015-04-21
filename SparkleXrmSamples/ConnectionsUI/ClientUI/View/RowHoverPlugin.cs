// RowHoverPlugin1.cs
//

using jQueryApi;
using Slick;
using System;
using System.Collections.Generic;
using System.Html;
using Xrm.Sdk;

namespace ClientUI.View.GridPlugins
{
    public class RowHoverPlugin : IPlugin
    {
        public RowHoverPlugin(string containerDivId)
        {
            _containerId = containerDivId;
        }

        private Grid _grid;
        private jQueryObject _hoverButtons;
        private string _containerId;
        private bool _mouseOut;
        public void Destroy()
        {
            _hoverButtons.Remove();
        }

        public void Init(Grid grid)
        {
            _grid = grid;
            _hoverButtons = jQuery.Select("#" + _containerId);
            _hoverButtons.MouseEnter(delegate(jQueryEvent e)
            {
                // Stop the mouse out when hovering over the button
                _mouseOut = false;
            });
            jQuery.Select("#grid").Find(".slick-viewport").Append(_hoverButtons); ;


            ((Event)Script.Literal("{0}.onMouseEnter", _grid)).Subscribe(HandleMouseEnter);
            ((Event)Script.Literal("{0}.onMouseLeave", _grid)).Subscribe(HandleMouseLeave);

        }
        public void HandleMouseLeave(EventData e, object item)
        {
            _mouseOut = true;
            Window.SetTimeout(delegate()
            {
                if (_mouseOut)
                    _hoverButtons.FadeOut();


            }, 500);
        }
        public void HandleMouseEnter(EventData e, object item)
        {
            OnCellChangedEventData cell = (OnCellChangedEventData)(object)_grid.GetCellFromEvent(e);
            if (cell != null)
            {
                _mouseOut = false;
                Entity entityRow = (Entity)_grid.GetDataItem(cell.Row);
                if (entityRow != null)
                {
                    Script.Literal("{0}.getGridPosition()", _grid);
                    int viewPortRight = _grid.GetViewport().RightPx;
                    int viewPortLeft = _grid.GetViewport().LeftPx;

                    jQueryObject node = jQuery.Select((string)Script.Literal("{0}.getCellNode({1},{2})", _grid, cell.Row, cell.Cell));
                    int buttonsWidth = _hoverButtons.GetWidth();
                    int x = node.Parent().GetWidth();
                    if (viewPortRight < x+buttonsWidth)
                        x = viewPortRight - buttonsWidth ;
                    int y = 0;

                    node.Parent().Append(_hoverButtons);
                    _hoverButtons.CSS("left", x.ToString() + "px");
                    _hoverButtons.CSS("top", y.ToString() + "px");
                    _hoverButtons.CSS("display", "block");
                    _hoverButtons.Attribute("rowId", entityRow.Id.ToString());
                }
            }
        }
    }
}
