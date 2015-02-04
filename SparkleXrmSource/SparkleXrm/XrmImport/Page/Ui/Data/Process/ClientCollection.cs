using System;
using System.Runtime.CompilerServices;

namespace Xrm
{
    public delegate bool GetItemHandlerStage(Stage control, int index);
    public delegate bool GetItemHandlerStep(Step control, int index);
    [Imported]
    public abstract class ClientCollectionStage
    {
        /// <summary>
        /// Applies the action contained in a delegate function
        /// </summary>
        /// <param name="function">Delegate function with parameters for attribute and index</param>
        public abstract void ForEach(GetItemHandlerStage function);
        

        /// <summary>
        /// Get all the objects in the collection 
        /// </summary>
        /// <returns>Array of T</returns>
        public abstract Stage[] Get();
        

        /// <summary>
        /// Get one object from the collection 
        /// </summary>
        /// <param name="name">The object where the name matches the argument</param>
        public abstract Stage Get(string name);
        

        /// <summary>
        /// Get one object from the collection
        /// </summary>
        /// <param name="position">The object where the index matches the number</param>
        public abstract Stage Get(int position);
        

        /// <summary>
        /// Get any objects that cause the delegate function to return true
        /// </summary>
        public abstract Stage[] Get(GetItemHandlerStage function);
       

        /// <summary>
        /// Get the number of items in the collection
        /// </summary>
        public abstract int GetLength();
        
    }


    [Imported]
    public abstract class ClientCollectionStep
    {
        /// <summary>
        /// Applies the action contained in a delegate function
        /// </summary>
        /// <param name="function">Delegate function with parameters for attribute and index</param>
        public abstract void ForEach(GetItemHandlerStep function);


        /// <summary>
        /// Get all the objects in the collection 
        /// </summary>
        /// <returns>Array of T</returns>
        public abstract Step[] Get();


        /// <summary>
        /// Get one object from the collection 
        /// </summary>
        /// <param name="name">The object where the name matches the argument</param>
        public abstract Step Get(string name);


        /// <summary>
        /// Get one object from the collection
        /// </summary>
        /// <param name="position">The object where the index matches the number</param>
        public abstract Step Get(int position);


        /// <summary>
        /// Get any objects that cause the delegate function to return true
        /// </summary>
        public abstract Step[] Get(GetItemHandlerStep function);


        /// <summary>
        /// Get the number of items in the collection
        /// </summary>
        public abstract int GetLength();

    }
}
