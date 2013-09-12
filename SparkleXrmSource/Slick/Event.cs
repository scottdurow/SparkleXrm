// Event.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Slick
{
    [Imported]
    public class Event
    {
        public void Notify(object args,EventData e, object scope)
        {

        }
        public void Subscribe(Action<EventData,object> callback)
        {

        }
        public void Unsubscribe(Action<EventData,object> callback)
        {

        }
    }
}
