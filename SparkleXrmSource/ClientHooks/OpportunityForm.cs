// OpportunityForm.cs
//

using System;
using System.Collections.Generic;
using Xrm;

namespace ClientHooks
{
    public class OpportunityForm
    {
        public static void Onload()
        {
            
            // Add On Process Stage change
            Page.Data.Process.AddOnStageChange(delegate(ExecutionContext context){
                Script.Literal("debugger");
            });

            // Add On Process Stage change
            Page.Data.Process.AddOnStageSelected(delegate(ExecutionContext context)
            {
                Script.Literal("debugger");
            });

            Script.Literal("debugger");
            // Get Current Process
            Process process = Page.Data.Process.GetActiveProcess();

            Stage stage = Page.Data.Process.GetActiveStage();

            // Get Stages
            ClientCollection<Stage> stages = process.GetStages();

            if (stages.GetLength() > 0)
            {
                // Get Steps
                Stage stage0 = stages.Get(0);
                ClientCollection<Step> steps = stage0.GetSteps();
            }

            // Show/Hide Process
            Page.Ui.Process.SetVisible(true);
        }
    }
}
