
using System.Collections.Generic;
using System.Runtime.CompilerServices;
namespace Xrm
{
    [IgnoreNamespace]
    [Imported]
    [NamedValues]
    public enum ProcesStage
    {

        [ScriptName("active")]
        Active,
        [ScriptName("inactive")]
        Inactive
    
    }

    [Imported]
    public class Stage
    {
        /// <summary>
        /// Returns an object with a getValue method which will return the integer value of the business process flow category. 
        /// </summary>
        /// <returns></returns>
        public Category GetCategory()
        {
            return null;
        }
        /// <summary>
        /// Returns the logical name of the entity associated with the stage.
        /// </summary>
        /// <returns></returns>
        public string GetEntityName()
        {
            return null;
        }
        /// <summary>
        /// Returns the unique identifier of the stage
        /// </summary>
        /// <returns></returns>
        public string GetId()
        {
            return null;
        }

        /// <summary>
        /// Returns the name of the stage
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return null;
        }

        /// <summary>
        /// Returns the status of the stage
        /// </summary>
        /// <returns></returns>
        public ProcesStage GetStatus()
        {
            return ProcesStage.Active;
        }

        public ClientCollection<Step> GetSteps()
        {
            return null;
        }
    }
}
