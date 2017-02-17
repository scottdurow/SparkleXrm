// RibbonMenuSection.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Xrm.Sdk.Ribbon
{
    [ScriptNamespace("SparkleXrm.Sdk.Ribbon")]
    public class RibbonMenuSection
    {
        public RibbonMenuSection(string Id, string LabelText, int Sequence, RibbonDisplayMode DisplayMode)
        {
            this.Id = Id;
            this.Title = LabelText;
            this.Sequence = Sequence;
            this.DisplayMode = DisplayMode;
        }

        [ScriptName("Id")]
        public string Id;
        [ScriptName("Title")]
        public string Title;
        [ScriptName("Sequence")]
        public int Sequence;
        [ScriptName("DisplayMode")]
        public RibbonDisplayMode DisplayMode;


        public List<RibbonControl> Buttons = new List<RibbonControl>();
        public void SerialiseToRibbonXml(StringBuilder sb)
        {
            /*
             *   <MenuSection Id="dev1.ApplicationRibbon.Section11.Section" Sequence="10" DisplayMode="Menu16">
              <Controls Id="dev1.ApplicationRibbon.Section11.Section.Controls">
                <Button Id="dev1.ApplicationRibbon.Button11.Button" LabelText="$LocLabels:dev1.ApplicationRibbon.Button11.Button.LabelText" Sequence="15" />
                <Button Id="dev1.ApplicationRibbon.Button10.Button" LabelText="$LocLabels:dev1.ApplicationRibbon.Button10.Button.LabelText" Sequence="20" />
              </Controls>
            </MenuSection>*/
            
            sb.AppendLine("<MenuSection Id=\"" + XmlHelper.Encode(Id) + (Title!=null ? "\" Title=\"" + Title.ToString() : "") + "\" Sequence=\"" + Sequence.ToString() + "\" DisplayMode=\"" + DisplayMode + "\">");
            sb.AppendLine("<Controls Id=\"" + XmlHelper.Encode(Id + ".Controls") + "\">");
            foreach (RibbonControl button in Buttons)
            {
                button.SerialiseToRibbonXml(sb);
            }
            sb.AppendLine("</Controls>");
            sb.AppendLine("</MenuSection>");
        }
        public RibbonMenuSection AddButton(RibbonControl button)
        {
            // We use this because the standard script# array extension functions break the CRM code
            ArrayEx.Add(Buttons, button);
            return this;
        }

    }

    [NamedValues]
    [Imported]
    [IgnoreNamespace]
    public enum RibbonDisplayMode
    {
        [ScriptName("Menu16")]
        Menu16 = 0,
         [ScriptName("Menu32")]
        Menu32 = 1
    }
}
