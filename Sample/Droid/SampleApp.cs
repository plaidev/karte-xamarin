using System;

using Android.App;
using Android.Gms.Common;
using Android.Runtime;
using Firebase.Iid;
using IO.Karte.Android;
using IO.Karte.Android.Core.Config;
using IO.Karte.Android.Core.Logger;
using IO.Karte.Android.Variables;

namespace SampleApp.Droid
{
    [Application]
    public class SampleApp : Application
    {
        private const string AppKey = "YOUR_APP_KEY";

        public SampleApp(IntPtr javaReference, JniHandleOwnership transfer)
        : base(javaReference, transfer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
            KarteApp.SetLogLevel(LogLevel.Debug);
            Logger.D("KarteSampleApp", "logging test.");
            KarteApp.PrintAssemblies();
            Config config = new Config.Builder().InvokeIsDryRun(true).Build();
            KarteApp.Setup(this, AppKey);
            KarteApp.Register("io/karte/android/inappmessaging/InAppMessaging");
            KarteApp.Register("io/karte/android/variables/internal/VariablesService");
            KarteApp.Register("io/karte/android/notifications/Notifications");
            Variables.Fetch();


            IsPlayServicesAvailable();
            System.Diagnostics.Debug.WriteLine("InstanceID token: " + FirebaseInstanceId.Instance.Token);
        }

        public bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                    System.Diagnostics.Debug.WriteLine("" + GoogleApiAvailability.Instance.GetErrorString(resultCode));
                else
                {
                    System.Diagnostics.Debug.WriteLine("This device is not supported");
                }
                return false;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Google Play Services is available.");
                return true;
            }
        }
    }
}
