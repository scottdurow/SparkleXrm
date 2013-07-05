// RibbonMenu.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Xrm.Sdk.Ribbon
{
    /*
     * <Menu Id="dev1.ApplicationRibbon.Sessions.Button.Menu">
            <MenuSection Id="dev1.ApplicationRibbon.Section11.Section" Sequence="10" DisplayMode="Menu16">
              <Controls Id="dev1.ApplicationRibbon.Section11.Section.Controls">
                <Button Id="dev1.ApplicationRibbon.Button11.Button" LabelText="$LocLabels:dev1.ApplicationRibbon.Button11.Button.LabelText" Sequence="15" />
                <Button Id="dev1.ApplicationRibbon.Button10.Button" LabelText="$LocLabels:dev1.ApplicationRibbon.Button10.Button.LabelText" Sequence="20" />
              </Controls>
            </MenuSection>
          </Menu>
*/
    public class RibbonMenu
    {
        public RibbonMenu(string Id)
        {
            this.Id = Id;
        }
        [ScriptName("Id")]
        public string Id;
        public List<RibbonMenuSection> Sections = new List<RibbonMenuSection>();

        public string SerialiseToRibbonXml()
        {
            /*
             *   <MenuSection Id="dev1.ApplicationRibbon.Section11.Section" Sequence="10" DisplayMode="Menu16">
              <Controls Id="dev1.ApplicationRibbon.Section11.Section.Controls">
                <Button Id="dev1.ApplicationRibbon.Button11.Button" LabelText="$LocLabels:dev1.ApplicationRibbon.Button11.Button.LabelText" Sequence="15" />
                <Button Id="dev1.ApplicationRibbon.Button10.Button" LabelText="$LocLabels:dev1.ApplicationRibbon.Button10.Button.LabelText" Sequence="20" />
              </Controls>
            </MenuSection>*/
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<Menu Id=\"" + Id + "\">");
            foreach (RibbonMenuSection section in Sections)
            {
                section.SerialiseToRibbonXml(sb);
            }
            sb.AppendLine("</Menu>");
            return sb.ToString();
        }
        public RibbonMenu AddSection(RibbonMenuSection section)
        {
            // We use this because the standard script# array extension functions break the CRM code
            ArrayEx.Add(Sections, section);
            return this;
        }
    }
}
