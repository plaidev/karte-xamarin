using System;
using UserNotifications;

namespace KarteiOSSample
{
    public class SampleUserNotificationDelegate : IUNUserNotificationCenterDelegate
    {
        public SampleUserNotificationDelegate()
        {
        }

        public IntPtr Handle => throw new NotImplementedException();

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
