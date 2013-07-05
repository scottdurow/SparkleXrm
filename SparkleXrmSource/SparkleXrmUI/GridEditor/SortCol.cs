// SortCol.cs
//

using System;
using System.Collections.Generic;

namespace SparkleXrm.GridEditor
{
    public class SortCol
    {
        public SortCol(string attributeName, bool ascending)
        {
            AttributeName = attributeName;
            Ascending = ascending;
        }
        public string AttributeName;
        public bool Ascending;

    }
}
