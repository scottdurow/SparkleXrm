using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;


namespace QUnitApi
{
    [Imported]
    [IgnoreNamespace]
    public static class QUnit
    {
        public static void Test(string name, Action<Assert> callback) { }
        /// <summary>
        /// Register a callback to fire whenever the test suite begins.
        /// </summary>
        /// <param name="callback"></param>
        public static void Begin(Action<BeginInfo> callback) { }
        /// <summary>
        /// Register a callback to fire whenever the test suite ends.
        /// </summary>
        /// <param name="callback"></param>
        public static void Done(Action<DoneInfo> callback) { }
        public static void Log(Action<LogInfo> callback) { }
        public static void Module(string name, ModuleInfo hooks) { }
    }

    [Imported]
    [IgnoreNamespace]
    public abstract class Assert
    {
        public abstract Action Async();
        public abstract void Expect(Number amount);
        public abstract void DeepEqual(object actual, object expected, string message);
        public abstract void Equal(object actual, object expected, string message);
        public abstract void NotDeepEqual(object actual, object expected, string message);
        public abstract void NotEqual(object actual, object expected, string message);
        public abstract void NotOk(object state, string message);
        public abstract void NotPropEqual(object actual, object expected, string message);
        public abstract void NotStrictEqual(object actual, object expected, string message);
        public abstract void Ok(object state, string message);
        public abstract void propEqual(object actual, object expected, string message);
        public abstract void Push(bool result, object action, object expected, string message);
        public abstract void StrictEqual(object actual, object expected, string message);
        public abstract void Throws(Action block, Func<Exception, bool> expected, string message);
    }
    [Imported]
    [IgnoreNamespace]
    public abstract class BeginInfo
    {
        public Number totalTests;
    }
    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public class ModuleInfo
    {
        /// <summary>
        /// The number of total tests in the test suite
        /// </summary>
        public Action BeforeEach;
        public Action AfterEach;
    }
    [Imported]
    [IgnoreNamespace]
    public abstract class DoneInfo
    {
        /// <summary>
        /// The number of failed assertions
        /// </summary>
        public Number Failed;
        /// <summary>
        /// The number of passed assertions
        /// </summary>
        public Number Passed;
        /// <summary>
        /// The total number of assertions
        /// </summary>
        public Number Total;
        /// <summary>
        /// The time in milliseconds it took tests to run from start to finish.
        /// </summary>
        public Number Runtime;
    }
    [Imported]
    [IgnoreNamespace]
    public abstract class LogInfo
    {
        /// <summary>
        /// The boolean result of an assertion, true means passed, false means failed.
        /// </summary>
        public bool Result;
        /// <summary>
        /// One side of a comparision assertion. Can be undefined when ok() is used.
        /// </summary>
        public object Actual;
        /// <summary>
        /// One side of a comparision assertion. Can be undefined when ok() is used.
        /// </summary>
        public object Expected;
        /// <summary>
        /// A string description provided by the assertion.
        /// </summary>
        public string Message;
        /// <summary>
        /// The associated stacktrace, either from an exception or pointing to the source of the assertion. Depends on browser support for providing stacktraces, so can be undefined.
        /// </summary>
        public string Source;
        /// <summary>
        /// The test module name of the assertion. If the assertion is not connected to any module, the property's value will be undefined.
        /// </summary>
        public string Module;
        /// <summary>
        /// The test block name of the assertion.
        /// </summary>
        public string Name;
        /// <summary>
        /// The time elapsed in milliseconds since the start of the containing QUnit.test(), including setup.
        /// </summary>
        public Number Runtime;
    }
}
