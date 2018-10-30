// Promise.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ES6
{
    [Imported]
    [IgnoreNamespace]
    [ScriptName("Promise")]
    public class Promise<TResolveTo>
    {
        public Promise(Action<Action<TResolveTo>, Action<Exception>> args)
        {

        }
        public Promise(Action<Action<TResolveTo>> args)
        {

        }


        [ScriptName("then")]
        public Promise Then(Action<TResolveTo> resolve)
        {
            return null;
        }

        [ScriptName("then")]
        public Promise Then(Func<TResolveTo, Promise> resolve)
        {
            return null;
        }

        [ScriptName("then")]
        public Promise<TResolve> Then<TResolve>(Func<TResolveTo, TResolve> resolve)
        {
            return null;
        }
    }

    [Imported]
    [IgnoreNamespace]
    public class Promise
    {
        public Promise(Action<Action<object>, Action<Exception>> args)
        {

        }
        public Promise(Action<Action<object>> args)
        {

        }
        public Promise(Action<Action, Action<Exception>> args)
        {

        }
        public Promise(Action<Action> args)
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

        //[ScriptName("then")]
        //public Promise Then<T,TResolve>(Func<T, TResolve> resolve)
        //{
        //    return null;
        //}
        [ScriptName("then")]
        public Promise<TResolve> Then<T, TResolve>(Func<T, TResolve> resolve)
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
