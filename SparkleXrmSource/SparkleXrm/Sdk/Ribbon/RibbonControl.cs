// RibbonControl.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Xrm.Sdk.Ribbon
{
    public class RibbonControl
    {
        public RibbonControl(string Id, int Sequence, string LabelText, string Command, string Image16, string Image32)
        {
            this.Id = Id;
            this.Sequence = Sequence;
            this.LabelText = LabelText;
            this.Command = Command;
            this.Image16by16 = Image16;
            this.Image32by32 = Image32;
        }
        [ScriptName("Id")]
        public string Id;
        [ScriptName("LabelText")]
        public string LabelText;
        [ScriptName("Sequence")]
        public int Sequence;
        [ScriptName("Command")]
        public string Command;
        [ScriptName("Image16by16")]
        public string Image16by16;
        [ScriptName("Image32by32")]
        public string Image32by32;

        public virtual void SerialiseToRibbonXml(StringBuilder sb)
        {

        }
    }
}
