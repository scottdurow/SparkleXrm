// RibbonFlyoutAnchor.cs
//

using System;
using System.Collections.Generic;

namespace Xrm.Sdk.Ribbon
{
    public class RibbonFlyoutAnchor : RibbonControl
    {
        public RibbonFlyoutAnchor(string Id, int Sequence,string LabelText,string Command,string Image16, string Image32) : base(Id,Sequence,LabelText,Command,Image16,Image32)
        {
         
        }
        //<FlyoutAnchor Command="dev1.ApplicationRibbon.PopulateQuickNav.Command" Id="sparkle.ApplicationRibbon.{!EntityLogicalName}.Button3.Button" Image16by16="$webresource:dev1_/images/QuickNavigation.png" PopulateDynamically="true" PopulateQueryCommand="dev1.ApplicationRibbon.PopulateQuickNav.Command" Sequence="0" TemplateAlias="o1">
        public RibbonMenu Menu;

        public override void SerialiseToRibbonXml(StringBuilder sb)
        {
            sb.AppendLine("<FlyoutAnchor Id=\"" + XmlHelper.Encode(Id) + "\" LabelText=\"" + XmlHelper.Encode(LabelText) + "\" Sequence=\"" + Sequence.ToString() + "\" Command=\"" + XmlHelper.Encode(Command) + "\"" + ((Image32by32 != null) ? (" Image32by32=\"" + XmlHelper.Encode(Image32by32) + "\"") : "") + ((Image16by16 != null) ? (" Image16by16=\"" + XmlHelper.Encode(Image16by16) + "\"") : "") + " PopulateDynamically=\"false\">");
            sb.AppendLine(Menu.SerialiseToRibbonXml());
            sb.AppendLine("</FlyoutAnchor>");
        }
    }
}
