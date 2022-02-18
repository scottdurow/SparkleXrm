using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ES6
{
    [Imported]
    [IgnoreNamespace]
    public class Promise
    {
        public Promise(PromiseArgs promise)
        {
        }

        public static Promise Resolve(object value)
        {
            return null;
        }
        
        [ScriptName("then")]
        public Promise Then(Func<object, Promise> onFulfilled)
        {
            return null;
        }
        
        [ScriptName("then")]
        public Promise Then2(Action<object, Promise> onFulfilled, Action<Exception> onRejected)
        {
            return null;
        }
        
        [ScriptName("then")]
        public Promise Then3(Action<object> onFulfilled)
        {
            return null;
        }
        
        public Promise Catch(Action<Exception> onRejected)
        {
            return null;
        }
        
        public static Promise Resolve(Promise promise)
        {
            return null;
        }
        
        public static Promise Reject(Exception error)
        {
            return null;
        }
        
        public static Promise All(List<Promise> all)
        {
            return null;
        }
        
        public static Promise Race(List<Promise> reace)
        {
            return null;
        }
    }

    [Imported]
    public delegate void PromiseArgs(Action<object> resolve, Action<Exception> reject);
}
