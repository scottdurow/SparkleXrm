// ActivityPointer.cs
//

using System;
using System.Collections.Generic;
using KnockoutApi;

namespace TimeSheet.Client.ViewModel
{
    public class ObservableActivityPointer
    {
        public Observable<string> Id = Knockout.Observable<string>();
        public Observable<string> Subject = Knockout.Observable<string>();
    }
}
