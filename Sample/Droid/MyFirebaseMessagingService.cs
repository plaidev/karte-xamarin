using Android.App;
using Android.Content;
using Android.Util;
using Firebase.Messaging;
using IO.Karte.Android.Notifications;

namespace SampleApp.Droid
{
    [Service(Label = "MyFirebaseMessagingService")]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {
        const string TAG = "MyFirebaseMessagingService";

        public override void OnNewToken(string token)
        {
            base.OnNewToken(token);
            Log.Debug(TAG, "Refreshed token: " + token);
            Notifications.RegisterFCMToken(token);
        }

        public override void OnMessageReceived(RemoteMessage remoteMessage)
        {
            base.OnMessageReceived(remoteMessage);
            Log.Debug(TAG, "OnMessageReceived: " + remoteMessage);
            MessageHandler.HandleMessage(this, remoteMessage);
        }
    }
}
