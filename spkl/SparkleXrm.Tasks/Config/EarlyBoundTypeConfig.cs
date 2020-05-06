namespace SparkleXrm.Tasks.Config
{
    public class EarlyBoundTypeConfig : DLaB.EarlyBoundGenerator.Settings.POCO.ExtensionConfig
    {
        public bool useEarlyBoundGenerator;

        public string profile;
        public string entities;
        public string[] entityCollection;
        public string actions;
        public string[] actionCollection;
        public bool generateOptionsetEnums;
        public string filename;
        public string classNamespace;
        public string serviceContextName;
        public bool oneTypePerFile;

        #region Sprkl Generation Specific

        public bool generateGlobalOptionsets;
        public bool generateStateEnums;

        #endregion Sprkl Generation Specific

        #region Early Bound Generator Specific

        
        public bool? generateActions;
        public string actionFilename;
        public string optionSetFilename;

        #endregion Early Bound Generator Specific
    }
}