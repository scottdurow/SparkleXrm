// BlockUI.cs
//

using jQueryApi;
using System.Runtime.CompilerServices;

namespace SparkleXrm.jQueryPlugins
{
    [Imported]
    [IgnoreNamespace]
    public class jQueryBlockUI : jQueryObject
    {
        private jQueryBlockUI() { }

        [ScriptName("block")]
        public void Block(jQueryBlockUIOptions options) { }

        [ScriptName("unblock")]
        public void Unblock() { }

    }
    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public class jQueryBlockUIOptions
    {
        public string Message;
        public int FadeIn;
        public int FadeOut;
        public int BaseZ;

        public bool IgnoreIfBlocked;
        public bool ShowOverlay;
        public BlockUICSS Css;
        public OverlayCSS OverlayCSS;
    }

    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public class OverlayCSS
    {
        public string BackgroundColor;
        public string Opacity;
    }
    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public class BlockUICSS
    {
        public int padding;//        0, 
        public int margin;//         0, 
        public string width;//         '30%', 
        public string height;//
        public string top;//         '40%', 
        public string left;//         '35%', 
        public string textAlign;//      'center', 
        public string color;//         '#000', 
        public string border;//         '3px solid #aaa', 
        public string backgroundColor;//'#fff', 
        public string cursor;//        'wait' 
    }
}
