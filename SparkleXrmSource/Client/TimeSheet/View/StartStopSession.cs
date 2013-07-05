using jQueryApi;
using KnockoutApi;
using TimeSheet.Client.ViewModel;
using Xrm.Sdk;
using jQueryApi.UI.Widgets;
using SparkleXrm;

namespace Client.TimeSheet.View
{
    public class StartStopSession : ViewBase
    {
        public static void Init()
        {
            StartStopSessionViewModel vm = new StartStopSessionViewModel(null, null);
            RegisterViewModel(vm);

        }
    }
}
