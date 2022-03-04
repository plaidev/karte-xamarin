
using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Webkit;
using Android.Widget;
using AndroidX.AppCompat.App;
using Org.Json;

using IO.Karte.Android;
using IO.Karte.Android.Core.Usersync;
using IO.Karte.Android.Inappmessaging;
using IO.Karte.Android.Tracking;
using IO.Karte.Android.Variables;

namespace SampleApp.Droid
{
    [Activity(Label = "MainActivity", MainLauncher = true)]
    [IntentFilter(new string[] {Intent.ActionView}, DataScheme = "krt-YOUR_APP_KEY")]
    public partial class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_main);
            setButtons();
            InAppMessaging.Delegate = new SampleInAppMessagingDelegate();
        }

        protected override void OnResume()
        {
            base.OnResume();

            Tracker.View("signup", "会員登録");

            System.Diagnostics.Debug.WriteLine("VisitorId: " + KarteApp.VisitorId);
            System.Diagnostics.Debug.WriteLine("IsOptOut: " + KarteApp.IsOptOut);
        }

        private void SetButtonsDelegate(int ResId, Action<Button> action)
        {
            Button button = FindViewById<Button>(ResId);
            button.Click += delegate
            {
                action(button);
            };
        }

        private void setButtons()
        {
            SetButtonsDelegate(Resource.Id.buttonIdentify, (button) =>
            {
                Tracker.Identify("test-user", new Dictionary<string, object>() {
                    {"name", "sample-name"}
                });
            });
            SetButtonsDelegate(Resource.Id.buttonView, (button) =>
            {
                Tracker.View("banner");
            });
            SetButtonsDelegate(Resource.Id.buttonTrack, (button) =>
            {
                Tracker.Track("favorite", new Dictionary<string, object>() {
                    { "id", "P00003" },
                    { "name", "ミネラルウォーター（500ml）" },
                    { "price", "100" }
                });
                Tracker.Track("buy", new Dictionary<string, object>() {
                    {"name", "sample-name"}
                }, (isSuccess) =>
                {
                    System.Diagnostics.Debug.WriteLine("TrackingTask isSuccess: " + isSuccess);
                });
            });
            SetButtonsDelegate(Resource.Id.buttonPush, (button) =>
            {
                Tracker.View("push_text", "push_text", new Dictionary<string, object>() {
                    {"name", "sample-name"}
                });
            });
            SetButtonsDelegate(Resource.Id.buttonUserSync, (button) =>
            {
                UserSyncTest();
            });
            SetButtonsDelegate(Resource.Id.buttonOptOut, (button) =>
            {
                KarteApp.OptOut();
            });
            SetButtonsDelegate(Resource.Id.buttonOptIn, (button) =>
            {
                KarteApp.OptIn();
            });
            SetButtonsDelegate(Resource.Id.buttonRenewVisitorId, (button) =>
            {
                KarteApp.RenewVisitorId();
            });
            SetButtonsDelegate(Resource.Id.buttonIsPresenting, (button) =>
            {
                System.Diagnostics.Debug.WriteLine("IsPresenting: " + InAppMessaging.IsPresenting);
            });
            SetButtonsDelegate(Resource.Id.buttonDismiss, (button) =>
            {
                InAppMessaging.Dismiss();
            });
            SetButtonsDelegate(Resource.Id.buttonSuppress, (button) =>
            {
                InAppMessaging.Suppress();
            });
            SetButtonsDelegate(Resource.Id.buttonUnsuppress, (button) =>
            {
                InAppMessaging.Unsuppress();
            });
            SetButtonsDelegate(Resource.Id.buttonFetch, (button) =>
            {
                FetchVariables();
            });
        }

        private void UserSyncTest()
        {
            var uri = Android.Net.Uri.Parse("https://karte.io");
            var appendingQueryParameterUrl1 = UserSync.AppendUserSyncQueryParameter(uri);
            System.Diagnostics.Debug.WriteLine("AppendingQueryParameterWithURL: " + appendingQueryParameterUrl1);

            var appendingQueryParameterUrl2 = UserSync.AppendUserSyncQueryParameter(uri.ToString());
            System.Diagnostics.Debug.WriteLine("AppendingQueryParameterWithURLString: " + appendingQueryParameterUrl2);

            var webView = new WebView(this);
            webView.Settings.JavaScriptEnabled = true;
            webView.SetWebViewClient(new MyWebViewClient(view =>
            {
                UserSync.SetUserSyncScript(view);
                webView.EvaluateJavascript("(function() { return window.__karte_ntvsync; })();", new ValueCallback((value) =>
                {
                    System.Diagnostics.Debug.WriteLine("WebView UserScripts: " + value);
                }));
            }));
            webView.LoadUrl("https://karte.io");
        }

        private void FetchVariables()
        {

            Variables.Fetch((isSuccess) =>
            {
                System.Diagnostics.Debug.WriteLine("FetchWithCompletion: " + isSuccess);
                Variable variable = Variables.Get("string_var1");
                Variable unknownVariable = Variables.Get("unknown_variable");
                System.Diagnostics.Debug.WriteLine("Unknown Variable: " + unknownVariable.GetString("default value"));

                Variable arrayVariable = Variables.Get("array");
                var array = arrayVariable.GetJSONArray(new JSONArray());
                System.Diagnostics.Debug.WriteLine("JSONArray variable: " + string.Join(",", array));

                Variable objectVariable = Variables.Get("object");
                var dictionary = objectVariable.GetJSONObject(new JSONObject());
                System.Diagnostics.Debug.WriteLine("JSONObject variable: " + dictionary.ToString());

                Variable[] variables = new Variable[] { variable };
                Variables.TrackOpen(variables, new Dictionary<string, object>() { { "test-key", "test-value" } });
                Variables.TrackClick(variables, new Dictionary<string, object>() { { "test-key", "test-value" } });
                var nulDict = new Dictionary<string, object>() { { "test-key", "test-value" } };
                nulDict = null;
                Variables.TrackOpen(variables, nulDict);
                Variables.TrackOpen(new Variable[] { }, new Dictionary<string, object>() { { "test-key", "test-value" } });
                Variables.TrackOpen(new Variable[] { }, values: null);
                //Variables.TrackOpen(null, new Dictionary<string, object>() { { "test-key", "test-value" } });
                //Variables.TrackOpen(null, values: null);
                //Variables.TrackOpen(null, values: null);
            });
        }
    }

    class MyWebViewClient : WebViewClient
    {
        Action<WebView> action;
        public MyWebViewClient(Action<WebView> action)
        {
            this.action = action;
        }
        public override void OnPageStarted(WebView view, string url, Bitmap favicon)
        {
            base.OnPageStarted(view, url, favicon);
            action(view);
        }
    }

    class ValueCallback : Java.Lang.Object, IValueCallback
    {
        Action<string> action;

        public ValueCallback(Action<string> action)
        {
            this.action = action;
        }
        public void OnReceiveValue(Java.Lang.Object value)
        {
            action(value.ToString());
        }
    };
}
