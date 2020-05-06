using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkleXrm.Tasks.Config
{
    public class EarlyBoundTypeConfig : DLaB.EarlyBoundGenerator.Settings.POCO.ExtensionConfig
    {
        public string profile;
        public string entities;
        public string actions;
        public bool generateOptionsetEnums;
        public bool? generateActions;
        public string actionFilename;
        public string optionSetFilename;
        public string filename;
        public string classNamespace;
        public string serviceContextName;
        public bool oneTypePerFile;
    }
}