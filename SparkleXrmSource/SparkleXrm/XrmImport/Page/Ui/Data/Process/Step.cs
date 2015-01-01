
using System.Runtime.CompilerServices;
namespace Xrm
{
    [Imported]
    public abstract class Step
    {
        /// <summary>
        /// Returns the logical name of the attribute associated to the step.
        /// Some steps don’t contain an attribute value. 
        /// </summary>
        /// <returns></returns>
        public abstract string GetAttribute();

        /// <summary>
        /// Returns the name of the step.
        /// </summary>
        /// <returns></returns>
        public abstract string GetName();

        /// <summary>
        /// Returns whether the step is required in the business process flow.
        /// </summary>
        /// <remarks>Returns true if the step is marked as required in the Business Process Flow editor; otherwise, false. There is no connection between this value and the values you can change in the Xrm.Page.data.entity attribute RequiredLevel methods.</remarks>
        /// <returns></returns>
        public abstract bool IsRequired();
    }
}
