// TaskIterrator.cs
//

using System;
using System.Collections.Generic;

namespace Xrm
{
    public delegate void TaskIteratorTask(Action successCallBack,Action errorCallBack);
    /// <summary>
    /// Provides an easy way of running n number of tasks with async callbacks
    /// </summary>
    public class TaskIterrator
    {
        private List<TaskIteratorTask> _tasks = new List<TaskIteratorTask>();
        private Action _errorCallBack;
        private Action _successCallBack;
        public TaskIterrator()
        {
           
        }
        public void AddTask(TaskIteratorTask task)
        {
            _tasks.Add(task);
        }
        public void Start(Action successCallBack,Action errorCallBack)
        {
            _errorCallBack= errorCallBack;
            _successCallBack = successCallBack;
            CompleteCallBack();
        }
        private void CompleteCallBack()
        {
            TaskIteratorTask nextAction = _tasks[0];
            if (nextAction != null)
            {
                _tasks.Remove(nextAction);
                nextAction(CompleteCallBack, _errorCallBack);
            }
            else
            {
                if (_successCallBack!=null)
                    _successCallBack();
            }
        }
    }
}
