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
                
            });

            // Add On Process Stage change
            Page.Data.Process.AddOnStageSelected(delegate(ExecutionContext context)
            {
               
            });

           
            // Get Current Process
            Process process = Page.Data.Process.GetActiveProcess();

            Stage stage = Page.Data.Process.GetActiveStage();

            // Get Stages
            ClientCollectionStage stages = process.GetStages();

            if (stages.GetLength() > 0)
            {
                // Get Steps
                Stage stage0 = stages.Get(0);
                ClientCollectionStep steps = stage0.GetSteps();
            }

            // Show/Hide Process
            Page.Ui.Process.SetVisible(true);
        }
    }
}
