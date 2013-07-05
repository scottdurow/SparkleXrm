// EventData.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Slick
{
            /***
   * An event object for passing data to event handlers and letting them control propagation.
   * <p>This is pretty much identical to how W3C and jQuery implement events.</p>
   * @class EventData
   * @constructor
   */
    [Imported]
    public class EventData
    {



        /***
         * Stops event from propagating up the DOM tree.
         * @method stopPropagation
         */
        public void StopPropagation()
        {

        }

        /***
         * Returns whether stopPropagation was called on this event object.
         * @method isPropagationStopped
         * @return {Boolean}
         */
        public bool IsPropagationStopped()
        {
            return false;
        }

        /***
         * Prevents the rest of the handlers from being executed.
         * @method stopImmediatePropagation
         */
        public void StopImmediatePropagation()
        {

        }
    }
}
