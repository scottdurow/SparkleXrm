
using ES6;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Xrm.Sdk
{
    [ScriptNamespace("SparkleXrm.Sdk")]
    public class XrmService
    {
       
        public static Promise Create(Entity contact)
        {
            return new Promise(delegate (Action<object> resolve, Action<Exception> reject)
            {               
                OrganizationServiceProxy.BeginCreate(contact, delegate (object state)
                {
                    try
                    {
                        resolve(OrganizationServiceProxy.EndCreate(state));
                    }
                    catch (Exception ex)
                    {
                        reject(ex);
                    }
                });
              
            });
        }

        public static Promise Update(Entity contact)
        {
            return new Promise(delegate (Action<object> resolve, Action<Exception> reject)
            {
                OrganizationServiceProxy.BeginUpdate(contact, delegate (object state)
                {
                    try
                    {
                        OrganizationServiceProxy.EndUpdate(state);
                        resolve(null);
                    }
                    catch (Exception ex)
                    {
                        reject(ex);
                    }
                });
               
            });
        }

        public static Promise Delete_(string entityName, Guid id)
        {
            return new Promise(delegate (Action<object> resolve, Action<Exception> reject)
            {
                OrganizationServiceProxy.BeginDelete(entityName, id, delegate(object state)
                {
                    try
                    {
                        OrganizationServiceProxy.EndDelete(state);
                        resolve(null);
                    }
                    catch (Exception ex)
                    {
                        reject(ex);
                    }
                });
                
            });
        }
    }
}
