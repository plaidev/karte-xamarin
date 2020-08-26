
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using IO.Karte.Android;
using IO.Karte.Android.Core.Logger;
using IO.Karte.Android.Variables;

namespace SampleApp.Droid
{
    [Application]
    public class SampleApp : Application
    {
        public SampleApp(IntPtr javaReference, JniHandleOwnership transfer)
        : base(javaReference, transfer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
            KarteApp.SetLogLevel(LogLevel.Debug);
            KarteApp.PrintAssemblies();
            KarteApp.Setup(this, "aiu6r2orS3YMXJYsT9mp3ilBvXIN5iD7");
            KarteApp.RegisterJObject("io/karte/android/inappmessaging/InAppMessaging");
            KarteApp.RegisterJObject("io/karte/android/variables/internal/VariablesService");
            Variables.Fetch();
        }
    }
}
