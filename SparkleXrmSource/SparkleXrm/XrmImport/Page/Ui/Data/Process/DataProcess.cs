
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Xrm
{
    [IgnoreNamespace]
    [Imported]
    [NamedValues]
    public enum MoveStepResult
    {
        [ScriptName("success")]
        Success,
        [ScriptName("crossEntity")]
        CrossEntity,
        [ScriptName("end")]
        End,
        [ScriptName("beginning")]
        Beginning,
        [ScriptName("invalid")]
        Invalid
    }


    [IgnoreNamespace]
    [Imported]
    [NamedValues]
    public enum SetActiveProcessResult
    {
        [ScriptName("success")]
        Success,
        [ScriptName("invalid")]
        Invalid
    }

    [IgnoreNamespace]
    [Imported]
    [NamedValues]
    public enum SetActiveStageResult
    {
        [ScriptName("crossEntity")]
        CrossEntity,
        [ScriptName("invalid")]
        Invalid,
        [ScriptName("unreachable")]
        Unreachable 
    
    }

    [Imported]
    public abstract class DataProcess
    {
        /// <summary>
        /// Returns a Process object representing the active process.
        /// </summary>
        public abstract Process GetActiveProcess();
        
        /// <summary>
        /// Set a Process as the active process.
        /// </summary>
        /// <param name="processId"> The Id of the process to make the active process.</param>
        /// <param name="callbackFunction">A function to call when the operation is complete. This callback function is passed one of the following string values to indicate whether the operation succeeded.</param>
        public abstract void SetActiveProcess(string processId, Action<SetActiveProcessResult> callbackFunction);
        
        /// <summary>
        /// Returns a Stage object representing the active stage
        /// </summary>
        /// <returns></returns>
        public abstract Stage GetActiveStage();
        
        /// <summary>
        /// Set a completed stage for the current entity as the active stage. 
        /// </summary>
        /// <remarks>Only a completed stage for the current entity can be set using this method.</remarks>
        /// <param name="stageId">The ID of the completed stage for the current entity to make the active stage. </param>
        /// <param name="callbackFunction">An optional function to call when the operation is complete. The callback function will be passed a string value of “success” if the operation completes successfully</param>
        public abstract void SetActiveStage(string stageId, Action<SetActiveStageResult> callbackFunction);
        
        /// <summary>
        /// Use this method to get a collection of stages currently in the active path with methods to interact with the stages displayed in the business process flow control.
        /// The active path represents stages currently rendered in the process control based on the branching rules and current data in the record.
        /// </summary>
        /// <returns></returns>
        public abstract List<Stage> GetActivePath();
       
        /// <summary>
        /// Use this method to asynchronously retrieve the enabled business process flows that the user can switch to for an entity.
        /// </summary>
        /// <param name="callbackFunction">The callback function must accept a parameter that contains an object with dictionary properties where the name of the property is the Id of the business process flow and the value of the property is the name of the business process flow.
        /// The enabled processes are filtered according to the user’s privileges. The list of enabled processes is the same ones a user can see in the UI if they want to change the process manually.</param>
        public abstract void GetEnabledProcesses(Action<Dictionary<string, string>> callbackFunction);

        /// <summary>
        /// Use this to add a function as an event handler for the OnStageChange event so that it will be called when the business process flow stage changes.
        /// </summary>
        /// <param name="handler">The function will be added to the bottom of the event handler pipeline. The execution context is automatically set to be the first parameter passed to the event handler.
        /// You should use a reference to a named function rather than an anonymous function if you may later want to remove the event handler.</param>
        public abstract void AddOnStageChange(AddRemoveOnChangeHandler handler);

        /// <summary>
        /// Use this to remove a function as an event handler for the OnStageChange event.
        /// </summary>
        /// <param name="handler">If an anonymous function is set using the addOnStageChange method it cannot be removed using this method.</param>
        public abstract void RemoveOnStageChange(AddRemoveOnChangeHandler handler);

        /// <summary>
        /// Use this to add a function as an event handler for the OnStageSelected event so that it will be called when a business process flow stage is selected.
        /// </summary>
        /// <param name="handler">The function will be added to the bottom of the event handler pipeline. The execution context is automatically set to be the first parameter passed to the event handler. See Execution context (client-side reference) for more information.</param>
        public abstract void AddOnStageSelected(AddRemoveOnChangeHandler handler);

        /// <summary>
        /// Use this to remove a function as an event handler for the OnStageSelected event.
        /// </summary>
        /// <param name="handler">If an anonymous function is set using the addOnStageSelected method it cannot be removed using this method.</param>
        public abstract void RemoveOnStageSelected(AddRemoveOnChangeHandler handler);

        /// <summary>
        /// Progresses to the next stage.
        /// </summary>
        /// <param name="callbackFunction">An optional function to call when the operation is complete. This callback function is passed one of the following string values to indicate whether the operation succeeded.</param>
        public abstract void MoveNext(Action<MoveStepResult> callbackFunction);

        /// <summary>
        /// Moves to the previous stage.
        /// </summary>
        /// <param name="callbackFunction"> An optional function to call when the operation is complete. This callback function is passed one of the following string values to indicate whether the operation succeeded.</param>
        public abstract void MovePrevious(Action<MoveStepResult> callbackFunction);

    }
}
