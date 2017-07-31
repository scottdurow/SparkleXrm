// EntityCollection.cs
//



using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Xml;
namespace Xrm.Sdk
{
    [ScriptNamespace("SparkleXrm.Sdk")]
    public class EntityCollection
    {
        #region Constructors
        public EntityCollection(List<Entity> entities)
        {
            Entities = new DataCollectionOfEntity(entities);
        }
        #endregion

        #region Properties
       
        public DataCollectionOfEntity Entities;




        public string EntityName;
       
        

        public Entity this[int index] {
            get
            {
                return this.Entities[index];
            }
            set
            {
                this.Entities[index] = value;
            }
        }


        public string MinActiveRowVersion;
        public bool MoreRecords;
        public string PagingCookie;
        public int TotalRecordCount;
        public bool TotalRecordCountLimitExceeded;
      
        #endregion

        #region Methods
        public static string Serialise(EntityCollection value)
        {
            string valueXml = string.Empty;
          
            // Check the type
            if (value.GetType() != typeof(EntityCollection))
                throw new Exception("An attribute value of type 'EntityCollection' must contain an EntityCollection instance");
            EntityCollection arrayValue = value as EntityCollection;

            valueXml += "<a:Entities>";
            for (int i = 0; i < arrayValue.Entities.Count; i++)
            { 
                valueXml += ((Entity)arrayValue[i]).Serialise(false);
            }
            valueXml += "</a:Entities>";
            return valueXml;
        }

        public static EntityCollection DeSerialise(XmlNode node)
        {
            List<Entity> entities = new List<Entity>();
            EntityCollection collection = new EntityCollection(entities);
            collection.EntityName = XmlHelper.SelectSingleNodeValue(node, "EntityName");
            XmlNode entitiesNode = XmlHelper.SelectSingleNodeDeep(node, "Entities");
            foreach (XmlNode entityNode in entitiesNode.ChildNodes)
            {
                Entity entity = new Entity(collection.EntityName);
                entity.DeSerialise(entityNode);
                ArrayEx.Add(entities, entity);
            }
            return collection;
        }

        public static void SerialiseWebApi(EntityCollection collection, Action<object> completeCallback, Action<object> errorCallback, bool async)
        {
            List<Object> items = new List<Object>();
            DelegateItterator.CallbackItterate(delegate (int index, Action nextCallBack, ErrorCallBack errorCallBack)
                {
                    Entity.SerialiseWebApi(collection.Entities[index], delegate (object jobject)
                    {
                        items.Add(jobject);
                        nextCallBack();
                    }, errorCallback, async);
                },
            collection.Entities.Count,
            delegate ()
            {
                completeCallback(items);
            },
            delegate (Exception ex)
            {
                errorCallback((object)ex);
            });
        }

        public static EntityCollection DeserialiseWebApi(Type entityType, string logicalName, Dictionary<string, object> results)
        {
            List<Entity> entities = new List<Entity>();
            foreach (Dictionary<string, object> row in (object[])results["value"])
            {
                Entity entity = (Entity)Type.CreateInstance(entityType, logicalName);
                entity.DeSerialiseWebApi(row);
                entities.Add(entity);
            }
            EntityCollection collection = new EntityCollection(entities);
            // Get the paging cookie if there is one
            if (results["@Microsoft.Dynamics.CRM.fetchxmlpagingcookie"] != null)
            {
                collection.PagingCookie = (string)results["@Microsoft.Dynamics.CRM.fetchxmlpagingcookie"];
            }
            
            // Get the paging cookie if there is one
            if (results["@Microsoft.Dynamics.CRM.fetchxmlpagingcookie"] != null)
            {
                collection.TotalRecordCount = (int)results["@Microsoft.Dynamics.CRM.totalrecordcount"];
            }
            return collection;
        }
        #endregion
    }

}
