using System;
using ObjCRuntime;

namespace Karte.iOS.Variables
{
    [Native]
    public enum KRTLastFetchStatus : long
    {
        NofetchYet = 0,
        Success = 1,
        Failure = 2
    }
}

