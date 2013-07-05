// ViewModelBase.cs
//

using KnockoutApi;

namespace SparkleXrm
{
    public class ViewModelBase
    {
        public Observable<bool> IsBusy = Knockout.Observable<bool>(false);
        public Observable<float?> IsBusyProgress = Knockout.Observable<float?>(null);
        public Observable<string> IsBusyMessage = Knockout.Observable<string>("Please Wait...");
        
    }
}
