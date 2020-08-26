using System;
using Firebase.CloudMessaging;
using Foundation;

namespace KarteiOSSample
{
    public class SampleFirebaseMessagingDelegate : MessagingDelegate
    {
        public SampleFirebaseMessagingDelegate()
        {
        }

        [Export("messaging:didReceiveRegistrationToken:")]
        public void DidReceiveRegistrationTokenFunc(Messaging messaging, string fcmToken)
        {
            Console.WriteLine($"Firebase registration token: {fcmToken}");

            // TODO: If necessary send token to application server.
            // Note: This callback is fired at each app startup and whenever a new token is generated.
        }
    }
}
