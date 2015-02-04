
using System.Collections.Generic;
using System.Runtime.CompilerServices;
namespace Xrm
{
    [Imported]
    public abstract class Process
    {
        /// <summary>
        /// Returns the unique identifier of the process
        /// </summary>
        /// <returns></returns>
        public abstract string GetId();

        /// <summary>
        /// Returns the name of the process
        /// </summary>
        /// <returns></returns>
        public abstract string GetName();

        /// <summary>
        /// Returns an collection of stages in the process
        /// </summary>
        /// <returns></returns>
        public abstract ClientCollectionStage GetStages();

        /// <summary>
        /// Returns true if the process is rendered, false if not
        /// If the form used has been upgraded from a previous version of Microsoft Dynamics CRM and has not been upgraded to use new forms, the business process flow control cannot be rendered
        /// </summary>
        /// <returns></returns>
        public abstract bool IsRendered();
    }
}
