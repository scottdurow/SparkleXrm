using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ES6
{
    [IgnoreNamespace]
    [Imported]
    public class Promise
    {
        public Promise(PromiseArgs promise) { }

        public static Promise All(List<Promise> all) { return null;  }
        public static Promise Race(List<Promise> reace) { return null; }
        public static Promise Reject(Exception error) { return null; }
        public static Promise Resolve(object value) { return null; }
        public static Promise Resolve(Promise promise) { return null; }
        public Promise Catch(Action<Exception> onRejected) { return null; }
        [ScriptName("then")]
        public Promise Then<T>(Func<T, Promise> onFulfilled) { return null; }
        [ScriptName("then")]
        public Promise Then2<T>(Action<T, Promise> onFulfilled, Action<Exception> onRejected) { return null; }
        [ScriptName("then")]
        public Promise Then3<T>(Action<T> onFulfilled) { return null; }
    }

    [Imported]
    public class PromiseArgs //: MulticastDelegate
    {
        public PromiseArgs(object @object, IntPtr method) { }

        public virtual void Invoke(Action<object> resolve, Action<Exception> reject) { }
    }

}