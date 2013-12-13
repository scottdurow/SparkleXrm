// EntityCollection.cs
//



using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
namespace Xrm.Sdk
{
    public class EntityCollection
    {
        #region Constructors
        public EntityCollection(List<Entity> entities)
        {
            _entities = new DataCollectionOfEntity(entities);
        }
        #endregion

        #region Properties
        private DataCollectionOfEntity _entities;

        public DataCollectionOfEntity Entities
        {
            get { return _entities; }
            set { _entities = value; }
        }

        private string _entityName;

        public string EntityName
        {
            get { return _entityName; }
            set { _entityName = value; }
        }
        

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

        private string _minActiveRowVersion;

        public string MinActiveRowVersion
        {
            get { return _minActiveRowVersion; }
            set { _minActiveRowVersion = value; }
        }


        private bool _moreRecords;

        public bool MoreRecords
        {
            get { return _moreRecords; }
            set { _moreRecords = value; }
        }


        private string _pagingCookie;

        public string PagingCookie
        {
            get { return _pagingCookie; }
            set { _pagingCookie = value; }
        }

        private int _totalRecordCount;

        public int TotalRecordCount
        {
            get { return _totalRecordCount; }
            set { _totalRecordCount = value; }
        }


        private bool _totalRecordCountLimitExceeded;

        public bool TotalRecordCountLimitExceeded
        {
            get { return _totalRecordCountLimitExceeded; }
            set { _totalRecordCountLimitExceeded = value; }
        }
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
            for (int i = 0; i < arrayValue._entities.Count; i++)
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
        #endregion
    }

}
