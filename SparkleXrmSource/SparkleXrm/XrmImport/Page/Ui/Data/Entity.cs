using System.Runtime.CompilerServices;

namespace Xrm
{
    [Imported]
    public class XrmEntity
    {
        public Attributes Attributes;

       
        public void AddOnSave(AddRemoveOnSaveHandler function)
        {
        }

        public void RemoveOnSave(AddRemoveOnSaveHandler function)
        {
        }

        public string GetDataXml()
        {
            return null;
        }

        public string GetEntityName()
        {
            return null;
        }

        public string GetId()
        {
            return null;
        }

        public bool GetIsDirty()
        {
            return false;
        }

        public void Save()
        {
        }

        public void Save(string option)
        {
        }
      
    }
}
