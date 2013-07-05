// Class1.cs
//

using System;
using System.Runtime.CompilerServices;

namespace Xrm.ComponentModel
{
    public delegate void PropertyChangedEventHandler(object sender, PropertyChangedEventArgs e);

    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public class PropertyChangedEventArgs : EventArgs
    {
        // Summary:
        //     Initializes a new instance of the System.ComponentModel.PropertyChangedEventArgs
        //     class.
        //
        // Parameters:
        //   propertyName:
        //     The name of the property that changed.
        public PropertyChangedEventArgs()
        {

        }

        // Summary:
        //     Gets the name of the property that changed.
        //
        // Returns:
        //     The name of the property that changed; System.String.Empty or null if all
        //     of the properties have changed.
        public string PropertyName;
    }

    public interface INotifyPropertyChanged
    {
        // Summary:
        //     Occurs when a property value changes.
        event PropertyChangedEventHandler PropertyChanged;

        void RaisePropertyChanged(string propertyName);
    }
}
