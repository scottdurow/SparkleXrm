using ES6;
using System.Runtime.CompilerServices;

namespace Xrm
{
    [Imported]
    public class Navigation
    {
        public NavigationItems Items;
        public Promise OpenForm(EntityFormOptions formOptions)
        {
            return null;
        }
    }

    [Imported]
    public class EntityFormOptions
    {
        public string EntityName;
        public string EntityId;
    }
}
