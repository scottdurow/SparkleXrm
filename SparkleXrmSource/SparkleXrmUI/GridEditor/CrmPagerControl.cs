// CrmPagerControl.cs
//

using jQueryApi;
using Slick;
using System;
using System.Runtime.CompilerServices;

namespace SparkleXrm.GridEditor
{
    [ScriptName("Object")]
    [IgnoreNamespace]
    [Imported]
    public class NavigationState
    {
        public bool CanGotoFirst;
        public bool CanGotoLast;
        public bool CanGotoPrev;
        public bool CanGotoNext;
        public PagingInfo pagingInfo;
    }

    public class CrmPagerControl
    {


        DataViewBase _dataView;
        Grid _grid;
        jQueryObject _container;

        public CrmPagerControl(DataViewBase dataView, Grid grid, jQueryObject container)
        {
            _dataView = dataView;
            _grid = grid;
            _container = container;
            jQuery.OnDocumentReady(delegate()
            {
                init();
            });

        }
        public void init()
        {
            _dataView.OnPagingInfoChanged.Subscribe(delegate(EventData e, object pagingInfo)
            {
                updatePager((PagingInfo)pagingInfo);
            });
            _dataView.OnSelectedRowsChanged += _dataView_OnSelectedRowsChanged;
            constructPagerUI();
            updatePager(_dataView.GetPagingInfo());
        }

        void _dataView_OnSelectedRowsChanged()
        {
            updatePager(_dataView.GetPagingInfo());
        }

        public NavigationState getNavState()
        {

            bool cannotLeaveEditMode = (bool)Script.Literal("!Slick.GlobalEditorLock.commitCurrentEdit()");
            PagingInfo pagingInfo = _dataView.GetPagingInfo();
            int? lastPage = pagingInfo.TotalPages - 1;
            NavigationState info = new NavigationState();
            info.CanGotoFirst = !cannotLeaveEditMode && pagingInfo.PageSize != 0 && pagingInfo.PageNum > 0;
            info.CanGotoLast = !cannotLeaveEditMode && pagingInfo.PageSize != 0 && pagingInfo.PageNum != lastPage;
            info.CanGotoPrev = !cannotLeaveEditMode && pagingInfo.PageSize != 0 && pagingInfo.PageNum > 0;
            info.CanGotoNext = !cannotLeaveEditMode && pagingInfo.PageSize != 0 && pagingInfo.PageNum < lastPage;
            info.pagingInfo = pagingInfo;
            return info;
        }

        public void setPageSize(int? n)
        {
            Script.Literal("{0}.setRefreshHints({{isFilterUnchanged: true}})", _dataView);
            PagingInfo paging = new PagingInfo();
            paging.PageSize = n;
            _dataView.SetPagingOptions(paging);
        }

        public void gotoFirst(jQueryEvent e)
        {
            if (getNavState().CanGotoFirst)
            {
                PagingInfo paging = new PagingInfo();
                paging.PageNum = 0;
                _dataView.SetPagingOptions(paging);
            }
        }

        public void gotoLast(jQueryEvent e)
        {
            NavigationState state = getNavState();
            if (state.CanGotoLast)
            {
                PagingInfo paging = new PagingInfo();
                paging.PageNum = state.pagingInfo.TotalPages - 1;
                _dataView.SetPagingOptions(paging);
            }
        }

        public void gotoPrev(jQueryEvent e)
        {
            NavigationState state = getNavState();
            if (state.CanGotoPrev)
            {
                PagingInfo paging = new PagingInfo();
                paging.PageNum = state.pagingInfo.PageNum - 1;
                _dataView.SetPagingOptions(paging);
            }

        }

        public void gotoNext(jQueryEvent e)
        {
            NavigationState state = getNavState();
            if (state.CanGotoNext)
            {
                PagingInfo paging = new PagingInfo();
                paging.PageNum = state.pagingInfo.PageNum + 1;
                _dataView.SetPagingOptions(paging);
            }
        }

        public void constructPagerUI()
        {

            _container.Empty();

            jQueryObject pager = jQuery.FromHtml("<table cellspacing='0' cellpadding='0' class='sparkle-grid-status'><tbody><tr>" +
            "<td class='sparkle-grid-status-label'>1 - 1 of 1&nbsp;(0 selected)</td>" +
            "<td class='sparkle-grid-status-paging'>" +
                "<img src='../../sparkle_/css/images/transparent_spacer.gif' class='sparkle-grid-paging-first'>" +
                "<img src='../../sparkle_/css/images/transparent_spacer.gif' class='sparkle-grid-paging-back'>" +
                "<span class='sparkle-grid-status-paging-page'>Page 1</span>" +
                "<img src='../../sparkle_/css/images/transparent_spacer.gif' class='sparkle-grid-paging-next'>" +
                 "&nbsp;</td></tr></tbody></table>");

            jQueryObject firstButton = pager.Find(".sparkle-grid-paging-first");
            jQueryObject backButton = pager.Find(".sparkle-grid-paging-back");
            jQueryObject nextButton = pager.Find(".sparkle-grid-paging-next");
            jQueryObject label = pager.Find(".sparkle-grid-status-label");
            jQueryObject pageInfo = pager.Find(".sparkle-grid-status-paging-page");
            _container.Append(pager);
            firstButton.Click(gotoFirst);
            backButton.Click(gotoPrev);
            nextButton.Click(gotoNext);

        }

        public void updatePager(PagingInfo pagingInfo)
        {
            NavigationState state = getNavState();

            jQueryObject firstButton = _container.Find(".sparkle-grid-paging-first");
            jQueryObject backButton = _container.Find(".sparkle-grid-paging-back");
            jQueryObject nextButton = _container.Find(".sparkle-grid-paging-next");
            jQueryObject label = _container.Find(".sparkle-grid-status-label");
            jQueryObject pageInfo = _container.Find(".sparkle-grid-status-paging-page");
            jQueryObject status = _container.Find(".sparkle-grid-status-label");
            if (state.CanGotoFirst)
                firstButton.RemoveClass("disabled");
            else
                firstButton.AddClass("disabled");

            if (state.CanGotoPrev)
                backButton.RemoveClass("disabled");
            else
                backButton.AddClass("disabled");

            if (state.CanGotoNext)
                nextButton.RemoveClass("disabled");
            else
                nextButton.AddClass("disabled");
            
            status.Text(string.Format("{0} - {1} of {2} ({3} selected)", pagingInfo.FromRecord, pagingInfo.ToRecord, pagingInfo.TotalRows, _dataView.GetSelectedRows().Length.ToString()));
            pageInfo.Text(string.Format("Page {0}", pagingInfo.PageNum + 1));

        }


    }
}
