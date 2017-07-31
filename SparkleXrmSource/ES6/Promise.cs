// Promise.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ES6
{
    [Imported]
    public delegate void PromiseArgs(Action<object> resolve, Action<Exception> reject);

    [Imported]
    public delegate Promise PromiseThenArgs<T>(T value);
    [Imported]
    [IgnoreNamespace]
    public class Promise
    {
        public Promise(PromiseArgs promise)
        {

        }
        [ScriptName("then")]
        public Promise Then<T>(PromiseThenArgs<T> onFulfilled)
        {
            return null;
        }

        [ScriptName("then")]
        public Promise Then<T>(Func<T, Promise> onFulfilled)
        {
            return null;
        }
        [ScriptName("then")]
        public Promise Then2<T>(Action<T, Promise> onFulfilled, Action<Exception> onRejected)
        {
            return null;
        }
        [ScriptName("then")]
        public Promise Then3<T>(Action<T> onFulfilled)
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

}
