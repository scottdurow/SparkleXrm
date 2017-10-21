// Promise.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ES6
{

    [Imported]
    [IgnoreNamespace]
    public class Promise
    {
        public Promise(Action<object> resolve, Action reject)
        {

        }
        [ScriptName("then")]
        public Promise Then(Action resolve)
        {
            return null;
        }

        [ScriptName("then")]
        public Promise Then<T>(Action<T> resolve)
        {
            return null;
        }

        [ScriptName("then")]
        public Promise Then<T>(Func<T, Promise> resolve)
        {
            return null;
        }

        [ScriptName("then")]
        public Promise Then<T>(Func<T, Promise> resolve, Action<Exception> reject)
        {
            return null;
        }

        public Promise Catch(Action<Exception> rejected)
        {
            return null;
        }

        [ScriptName("resolve")]
        public static Promise Resolve(Promise promise)
        {
            return null;
        }

        [ScriptName("resolve")]
        public static Promise Resolve<T>(T result)
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
