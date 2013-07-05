using System;
using System.Collections.Generic;

namespace Xrm
{
    public delegate void ErrorCallBack(Exception ex);
    public delegate void ItteratorAction(int index, Action nextCallBack, ErrorCallBack errorCallBack);

    public class DelegateItterator
    {
        public static void CallbackItterate(ItteratorAction action, int numberOfTimes, Action completeCallBack, ErrorCallBack errorCallBack)
        {
            CallbackItterateAction(action, 0, numberOfTimes, completeCallBack, errorCallBack);
        }

        private static void CallbackItterateAction(ItteratorAction action, int index, int numberOfTimes, Action completeCallBack, ErrorCallBack errorCallBack)
        {
            if (index < numberOfTimes)
            {
                try
                {
                    action(index, delegate() {
                        index++;
                         CallbackItterateAction(action, index, numberOfTimes, completeCallBack, errorCallBack);
                    },
                    delegate(Exception ex){
                       // Error callback
                        errorCallBack(ex);
                    });
                    
                }
                catch (Exception ex)
                {
                    errorCallBack(ex);
                }
            }
            else
                completeCallBack();
        }
    }
}
