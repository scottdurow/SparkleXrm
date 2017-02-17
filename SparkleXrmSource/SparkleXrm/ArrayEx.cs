using System;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.CompilerServices;

namespace Xrm
{
    /// <summary>
    /// This class is here because we can't use the mscorlib Array Extensions in Ribbon Commands/Form JS because they conflict with the CRM scripts
    /// </summary> 
    [ScriptNamespace("SparkleXrm")]
    public class ArrayEx
    {
        public static void Add(object list, object item)
        {
            Script.Literal("{0}[{0}.length]={1}", list, item);

        }
        public static IEnumerator GetEnumerator(object list)
        {
            return (IEnumerator)Script.Literal("new ss.ArrayEnumerator({0})",list);
        }
        public static string Join(Array list, string delimeter)
        {
            string result = "";
            for (int i = 0; i < list.Length; i++)
            {
                if (i > 0)
                    result += delimeter;
                result += list[i];
            }
            return result;
        }
    }
}
