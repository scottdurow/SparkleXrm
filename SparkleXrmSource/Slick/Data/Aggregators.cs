// Aggregators.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Slick.Data.Aggregators
{
    [Imported]
    public class Aggregator
    {
    }

    [Imported]
    public class Avg : Aggregator
    {
        public Avg(string fieldName)
        {
        }
    }
    [Imported]
    public class Min : Aggregator
    {
        public Min(string fieldName)
        {
        }
    }
    [Imported]
    public class Max : Aggregator
    {
        public Max(string fieldName)
        {
        }
    }
    [Imported]
    public class Sum : Aggregator
    {
        public Sum(string fieldName)
        {
        }
    }
}
