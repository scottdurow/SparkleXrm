// BindingInterceptor.cs
//

using System;
using System.Collections;
using System.Html;
using System.Runtime.CompilerServices;

namespace SparkleXrm.CustomBinding
{
    [Imported]
    [ScriptName("Object")]
    [IgnoreNamespace]
    public class ComputedObservable
    {
        public Func<object> Read;
        public Action<object> Write;
        public object Owner;
        public Element DisposeWhenNodeIsRemoved;
    }

    [IgnoreNamespace]
    [Imported]
    [ScriptName("ko")]
    public static class KnockoutEx
    {
        public static object Computed(ComputedObservable observableDelegate) { return null; }
        //
        // Summary:
        //     Set up bindings on a single node without binding any of its descendents.
        //
        // Parameters:
        //   node:
        //     The node to bind to.
        //
        //   bindings:
        //     An optional dictionary of bindings, pass null to let Knockout gather them
        //     from the element.

        public static void ApplyBindingsToNode(Element node, Dictionary bindings) { }
    }
}
