using System;
using Firebase.CloudMessaging;
using Foundation;
using UIKit;
using UserNotifications;
using Karte.iOS.Core;
using Karte.iOS.Notification;

namespace KarteiOSSample
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : UIResponder, IUIApplicationDelegate, IMessagingDelegate, IUNUserNotificationCenterDelegate
    {
        [Export("window")]
        public UIWindow Window { get; set; }

        [Export("application:didFinishLaunchingWithOptions:")]
        public bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            Firebase.Core.App.Configure();

            RegisterRemoteNotification();

            Messaging.SharedInstance.ShouldEstablishDirectChannel = true;

            // Override point for customization after application launch.
            // If not required for your application you can safely delete this method
            return true;
        }

        // UISceneSession Lifecycle

        [Export("application:configurationForConnectingSceneSession:options:")]
        public UISceneConfiguration GetConfiguration(UIApplication application, UISceneSession connectingSceneSession, UISceneConnectionOptions options)
        {
            // Called when a new scene session is being created.
            // Use this method to select a configuration to create the new scene with.
            return UISceneConfiguration.Create("Default Configuration", connectingSceneSession.Role);
        }

        [Export("application:didDiscardSceneSessions:")]
        public void DidDiscardSceneSessions(UIApplication application, NSSet<UISceneSession> sceneSessions)
        {
            // Called when the user discards a scene session.
            // If any sessions were discarded while the application was not running, this will be called shortly after `FinishedLaunching`.
            // Use this method to release any resources that were specific to the discarded scenes, as they will not return.
        }

        [Export("application:openURL:options:")]
        public bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            return KRTApp.Application(app, url);
        }

        public void RegisterRemoteNotification()
        {
            // Register your app for remote notifications.
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;
                UNUserNotificationCenter.Current.RequestAuthorization(authOptions, (granted, error) => {
                    System.Diagnostics.Debug.WriteLine("granted: " + granted);
                });

                Messaging.SharedInstance.Delegate = this;

                // For iOS 10 display notification (sent via APNS)
                UNUserNotificationCenter.Current.Delegate = this;
            }
            else
            {
                // iOS 9 or before
                var allNotificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;
                var settings = UIUserNotificationSettings.GetSettingsForTypes(allNotificationTypes, null);
                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            }

            UIApplication.SharedApplication.RegisterForRemoteNotifications();
        }

        [Export("application:didReceiveRemoteNotification:fetchCompletionHandler:")]
        public void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {

            switch (application.ApplicationState)
            {
                case UIApplicationState.Active:
                case UIApplicationState.Inactive:
                    // KARTE経由のプッシュ通知であるか判定
                    var notification = new KRTNotification(userInfo);
                    if (notification != null)
                    {
                        // KARTE経由のプッシュ通知
                        notification.HandleNotification();
                    }
                    else
                    {
                        // KARTE以外のシステムから送信されたプッシュ通知   
                    }
                    break;
                case UIApplicationState.Background:
                    break;
            }

            completionHandler(UIBackgroundFetchResult.NewData);
        }

        [Export("messaging:didRefreshRegistrationToken:")]
        public void DidRefreshRegistrationToken(Messaging messaging, string fcmToken)
        {
            System.Diagnostics.Debug.WriteLine($"FCM Token: {fcmToken}");
        }

        [Export("messaging:didReceiveRegistrationToken:")]
        public void DidReceiveRegistrationToken(Messaging messaging, string fcmToken)
        {
            // Monitor token generation: To be notified whenever the token is updated.
            System.Diagnostics.Debug.WriteLine($"Received token: {fcmToken}");
            KRTNotification.RegisterFCMToken(fcmToken);
            // Note: This callback is fired at each app startup and whenever a new token is generated.
        }

        [Export("userNotificationCenter:didReceiveNotificationResponse:withCompletionHandler:")]
        public void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
        {
            var userInfo = response.Notification.Request.Content.UserInfo;
            var notification = new KRTNotification(userInfo);

            // KARTE経由のプッシュ通知であるか判定
            if (notification != null)
            {

                System.Diagnostics.Debug.WriteLine($"notification.url: {notification.Url}");

                // KARTE経由のプッシュ通知
                notification.HandleNotification();
            }
            else
            {
                // KARTE以外のシステムから送信されたプッシュ通知   
            }
            completionHandler();
        }


        [Export("userNotificationCenter:willPresentNotification:withCompletionHandler:")]
        public void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            completionHandler(UNNotificationPresentationOptions.Alert | UNNotificationPresentationOptions.Sound);
        }
    }
}

