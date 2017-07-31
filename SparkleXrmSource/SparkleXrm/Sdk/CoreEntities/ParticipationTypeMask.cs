// ParticipationTypeMask.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Xrm.Sdk
{
    [NumericValues]
    public enum ParticipationTypeMask
    {
        Sender = 1,
        To_Recipient = 2,
        CC_Recipient = 3,
        BCC_Recipient = 4,
        Required_attendee = 5,
        Optional_attendee = 6,
        Organizer = 7,
        Regarding = 8,
        Owner = 9,
        Resource = 10,
        Customer = 11
    }
}
