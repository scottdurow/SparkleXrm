// ContactCardView.cs
//

using Client.ContactEditor.Model;
using Client.InlineSubGrids.ViewModels;
using jQueryApi;
using SparkleXrm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Html;
using System.Runtime.CompilerServices;
using Xrm;

namespace Client.InlineSubGrids.Views
{
    public class ContactCardView
    {
        static FreeWallPlugin wall;
        public static void init()
        {
            ContactCardViewModel vm = new ContactCardViewModel();

            wall = new FreeWallPlugin("#freewall");
            FreeWallOptions options = new FreeWallOptions();
            options.selector = ".brick";
            options.animate = true;
            options.cellW = 150;
            options.cellH = "auto";
            options.onResize = delegate()
            {
                wall.fitWidth();
            };

            wall.reset(options);
            
            // Data Bind the View Model
            ViewBase.RegisterViewModel(vm);

        }
        public static void OnAfterRender(Element[] rendered)
        {
            // Layout grid everytime an image is loaded
            jQuery.FromElements(rendered).Find("img").Load(delegate(jQueryEvent e)
            {
                wall.fitWidth();
            });
        }
    }

    [Imported]
    [IgnoreNamespace]
    [ScriptName("freewall")]
    public class FreeWallPlugin
    {
        public jQueryObject container;
        public FreeWallPlugin(string selector)
        {

        }
        public void reset(FreeWallOptions options)
        {

        }
        public void fitWidth()
        {

        }
    }
    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public class FreeWallOptions
    {
        public string selector;
        public bool animate;
        public object cellW;
        public object cellH;
        public Action onResize;
    }
}
