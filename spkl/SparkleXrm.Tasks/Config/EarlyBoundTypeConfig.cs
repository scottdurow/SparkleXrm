using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkleXrm.Tasks.Config
{
    public class EarlyBoundTypeConfig
    {
        public string profile;
        public string entities;
        public string actions;
        public bool generateOptionsetEnums;
        public bool generateGlobalOptionsets;
        public bool generateStateEnums;
        public string filename;
        public string classNamespace;
        public string serviceContextName;
        public bool oneTypePerFile;
    }
}