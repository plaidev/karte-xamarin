using ObjCRuntime;

namespace Karte.iOS.Core
{
    [Native]
    public enum KRTLogLevel : long
    {
        Error = 0,
        Warn = 1,
        Info = 2,
        Debug = 3,
        Verbose = 4
    }
}
