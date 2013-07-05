using System.Runtime.CompilerServices;

namespace Slick
{

    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public interface IDataProvider
    {
        
       int GetLength();
       object GetItem(int index);
       ItemMetaData GetItemMetadata(int i);
      

    }
}
