using System.Runtime.CompilerServices;

namespace Xrm.Sdk
{
    [ScriptNamespace("SparkleXrm.Sdk")]
    public class ColumnSet
    {
        public bool AllColumns;
        public string[] Columns;

        public ColumnSet(object columns)
        {
            if (columns.GetType() == typeof(bool))
            {
                AllColumns = (bool)columns;
            }
            else
            {
                Columns = (string[])columns;
            }
            
        }
        
    }
}