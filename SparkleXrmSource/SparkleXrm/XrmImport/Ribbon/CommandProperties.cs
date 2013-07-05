using System.Runtime.CompilerServices;

namespace Xrm.XrmImport.Ribbon
{
    [Imported]
    [ScriptName("Object")]
    [IgnoreNamespace]
    public class CommandProperties
    {
        [ScriptName("PopulationXML")]
        public string PopulationXML;
        [ScriptName("SourceControlId")]
        public string SourceControlId;

    }
  
}
