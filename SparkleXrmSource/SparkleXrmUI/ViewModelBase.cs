// ViewModelBase.cs
//

using KnockoutApi;
using System;

namespace SparkleXrm
{
    public class ViewModelBase
    {
        public Observable<bool> IsBusy = Knockout.Observable<bool>(false);
        public Observable<float?> IsBusyProgress = Knockout.Observable<float?>(null);
        public Observable<string> IsBusyMessage = Knockout.Observable<string>("Please Wait...");

        /// <summary>
        /// Subscribe to this event to be notified about the VM needing to commit all edits. 
        /// Set CancelEventArgs.Cancel=true if the view cannot commit at this time (Eg. validation errors in grids)
        /// </summary>
        public event Action<ViewModelBase,CancelEventArgs> OnCommitEdit;

        /// <summary>
        /// Used to notify any UI controls that they must commit
        /// because the VM needs to perform an action
        /// </summary>
        /// <returns>True if all commits were succesful</returns>
        public bool CommitEdit()
        {
            if (OnCommitEdit != null)
            {
                CancelEventArgs args = new CancelEventArgs();
                OnCommitEdit(this,args);
                return !args.Cancel;
            }
            return true;
        }


    }
}
