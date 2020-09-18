using System;
using System.Linq;
using UIKit;
using System.Drawing;
using Foundation;
using WebKit;

using Karte.iOS.Core;
using Karte.iOS.InAppMessaging;
using Karte.iOS.Variables;
using Karte.iOS.Notification;

namespace KarteiOSSample
{
    public partial class ViewController : UIViewController, IWKNavigationDelegate
    {
        SampleIDFADelegate idfaDelegate;
        SampleInAppMessagingDelegate iamDelegate;

        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            
            //For iam delegate testing
            iamDelegate = new SampleInAppMessagingDelegate();
            KRTInAppMessaging.Shared.Delegate = iamDelegate;

            //For idfa testing
            idfaDelegate = new SampleIDFADelegate();
            var configuration = new KRTConfiguration();
            configuration.IdfaDelegate = this.idfaDelegate;
            configuration.IsDryRun = false;

            KRTApp.SetLogLevel(KRTLogLevel.Verbose);
            KRTApp.SetupWithAppKey("APP_KEY", configuration);
            KRTInAppMessaging.Configure();
            KRTNotification.Configure();
            KRTVariables.Configure();
            KRTTracker.View("chat", "chat");


            System.Diagnostics.Debug.WriteLine("AdvertisingIdentifierString: " + idfaDelegate.AdvertisingIdentifierString);
            System.Diagnostics.Debug.WriteLine("VisitorId: " + KRTApp.VisitorId);
            System.Diagnostics.Debug.WriteLine("IsOptOut: " + KRTApp.IsOptOut);

            string[] btnTitles = new string[]
            {
                "Identify",
                "View",
                "Track",
                "ViewForScene",
                "Push",
                "UserSync",
                "OptOut",
                "OptIn",
                "RenewVisitorId",
                "IsPresenting",
                "Dismiss",
                "Suppress",
                "Unsuppress",
                "Fetch Variables"
            };
            AddButtons(btnTitles);
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        public void AddButtons(string[] titles)
        {
            foreach (var item in titles.Select((v, i) => new { v, i }))
            {
                var button = new UIButton(UIButtonType.RoundedRect)
                {
                    Bounds = new RectangleF(0, 0, 200, 30),
                    Center = new PointF((float)View.Bounds.Width / 2, 80 + 40 * item.i),
                };
                button.SetTitle(item.v, UIControlState.Normal);
                button.TouchUpInside += (sender, args) =>
                {
                    switch (item.v)
                    {
                        case "Identify":
                            KRTTracker.Identify(new NSDictionary("name", "sample-name"));
                            break;
                        case "View":
                            KRTTracker.View("banner");
                            break;
                        case "Track":
                            KRTTracker.Track("favorite", new NSDictionary(
                                "id", "P00003",
                                "name", "ミネラルウォーター（500ml）",
                                "price", "100"
                            ));
                            var task = KRTTracker.Track("buy", new NSDictionary("name", "sample-name"));
                            task.Completion = (isSuccess) =>
                            {
                                System.Diagnostics.Debug.WriteLine("TrackingTask isSuccess: " + isSuccess);
                            };
                            break;
                        case "ViewForScene":
                            TrackFromTrackerInstance();
                            break;
                        case "Push":
                            KRTTracker.View("push_text", "push_text", new NSDictionary("name", "sample-name"));
                            break;
                        case "UserSync":
                            var nSUrl = new NSUrl(new NSString("https://karte.io"));
                            var appendingQueryParameterUrl1 = KRTUserSync.AppendingQueryParameterWithURL(nSUrl);
                            System.Diagnostics.Debug.WriteLine("AppendingQueryParameterWithURL: " + appendingQueryParameterUrl1);

                            var appendingQueryParameterUrl2 = KRTUserSync.AppendingQueryParameterWithURLString(nSUrl.AbsoluteString);
                            System.Diagnostics.Debug.WriteLine("AppendingQueryParameterWithURLString: " + appendingQueryParameterUrl2);

                            var webView = new WKWebView(CoreGraphics.CGRect.Empty, new WKWebViewConfiguration());
                            KRTUserSync.SetUserSyncScriptWithWebView(webView);
                            var url = new NSUrl("https://karte.io");
                            if (url != null)
                            {
                                webView.LoadRequest(new NSUrlRequest(url));
                            }
                            System.Diagnostics.Debug.WriteLine("WebView UserScripts: " + webView.Configuration.UserContentController.UserScripts.First().Source);
                            break;
                        case "OptOut":
                            KRTApp.OptOut();
                            break;
                        case "OptIn":
                            KRTApp.OptIn();
                            break;
                        case "RenewVisitorId":
                            KRTApp.RenewVisitorId();
                            break;
                        case "IsPresenting":
                            System.Diagnostics.Debug.WriteLine("IsPresenting: " + KRTInAppMessaging.Shared.IsPresenting);
                            break;
                        case "Dismiss":
                            KRTInAppMessaging.Shared.Dismiss();
                            break;
                        case "Suppress":
                            KRTInAppMessaging.Shared.Suppress();
                            break;
                        case "Unsuppress":
                            KRTInAppMessaging.Shared.Unsuppress();
                            break;
                        case "Fetch Variables":
                            FetchVariable();
                            break;
                    }
                };
                View.AddSubview(button);
            }
        }

        public void TrackFromTrackerInstance()
        {
            var tracker = new KRTTracker(this.View);
            tracker.ViewForScene("popup");
        }

        public void FetchVariable()
        {
            KRTVariables.FetchWithCompletion(delegate (bool isSuccess)
            {
                System.Diagnostics.Debug.WriteLine("FetchWithCompletion: " + isSuccess);
                KRTVariable variable = KRTVariables.VariableForKey("string_var1");
                KRTVariable unknownVariable = KRTVariables.VariableForKey("unknown_variable");
                System.Diagnostics.Debug.WriteLine("Unknown Variable: " + unknownVariable.StringWithDefaultValue("default value"));

                KRTVariable arrayVariable = KRTVariables.VariableForKey("array");
                var array = arrayVariable.Array;
                System.Diagnostics.Debug.WriteLine("Array variable: " + string.Join(",", array));

                KRTVariable objectVariable = KRTVariables.VariableForKey("object");
                var dictionary = objectVariable.Dictionary;
                System.Diagnostics.Debug.WriteLine("Dictionary variable: " + dictionary.ToString());

                KRTVariable[] variables = new KRTVariable[] { variable };
                KRTVariables.TrackVariableOpen(variables, new NSDictionary("test-key", "test-value"));
                KRTVariables.TrackVariableClick(variables, new NSDictionary("test-key", "test-value"));
                var nulDict = new NSDictionary("test-key", "test-value");
                nulDict = null; 
                KRTVariables.TrackVariableOpen(variables, nulDict);
            });
        }
    }
}