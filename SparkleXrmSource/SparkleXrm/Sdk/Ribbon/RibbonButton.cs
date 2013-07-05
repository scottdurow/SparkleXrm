// RibbonButton.cs
//

using System;
using System.Runtime.CompilerServices;

namespace Xrm.Sdk.Ribbon
{
    public class RibbonButton
    {
        public RibbonButton(string Id, int Sequence,string LabelText,string Command,string Image16, string Image32)
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
        public void SerialiseToRibbonXml(StringBuilder sb)
        {
            /*
             *   <MenuSection Id="dev1.ApplicationRibbon.Section11.Section" Sequence="10" DisplayMode="Menu16">
              <Controls Id="dev1.ApplicationRibbon.Section11.Section.Controls">
                <Button Id="dev1.ApplicationRibbon.Button11.Button" LabelText="$LocLabels:dev1.ApplicationRibbon.Button11.Button.LabelText" Sequence="15" />
                <Button Id="dev1.ApplicationRibbon.Button10.Button" LabelText="$LocLabels:dev1.ApplicationRibbon.Button10.Button.LabelText" Sequence="20" />
              </Controls>
            </MenuSection>*/

            sb.AppendLine("<Button Id=\"" + XmlHelper.Encode(Id) + "\" LabelText=\"" + XmlHelper.Encode(LabelText) + "\" Sequence=\"" + Sequence.ToString() + "\" Command=\"" + XmlHelper.Encode(Command) + "\"" + ((Image32by32!=null) ? (" Image32by32=\"" + XmlHelper.Encode(Image32by32) + "\"") : "") +  ((Image16by16!=null) ? (" Image16by16=\"" + XmlHelper.Encode(Image16by16) + "\"") : "") + " />");
        }
    }
}
