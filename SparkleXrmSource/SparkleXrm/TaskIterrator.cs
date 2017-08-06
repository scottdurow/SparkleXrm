// TaskIterrator.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Xrm
{
    [ScriptNamespace("SparkleXrm")]
    public delegate void TaskIteratorTask(Action successCallBack,Action errorCallBack, Dictionary<string, object> state);

    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public class QueuedTaskItteratorTask
    {
        public TaskIteratorTask Task;
        public Dictionary<string, object> State;
            
    }
    /// <summary>
    /// Provides an easy way of running n number of tasks with async callbacks
    /// </summary>
    [ScriptNamespace("SparkleXrm")]
    public class TaskIterrator
    {
        private List<QueuedTaskItteratorTask> _tasks = new List<QueuedTaskItteratorTask>();
        private Action _errorCallBack;
        private Action _successCallBack;
        public TaskIterrator()
        {
           
        }
        public void AddTask(TaskIteratorTask task, Dictionary<string, object> state)
        {
            QueuedTaskItteratorTask queued = new QueuedTaskItteratorTask();
            queued.Task = task;
            queued.State = state;
            _tasks.Add(queued);
        }
        public void Start(Action successCallBack,Action errorCallBack)
        {
            _errorCallBack= errorCallBack;
            _successCallBack = successCallBack;
            CompleteCallBack();
        }
        private void CompleteCallBack()
        {
           
            QueuedTaskItteratorTask nextAction = _tasks[0];
            if (nextAction != null)
            {
                _tasks.Remove(nextAction);
                nextAction.Task(CompleteCallBack, _errorCallBack, nextAction.State);
            }
            else
            {
                if (_successCallBack!=null)
                    _successCallBack();
            }
        }
    }
}
