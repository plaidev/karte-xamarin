﻿using System;
using Foundation;
using ObjCRuntime;
using UIKit;
using WebKit;

namespace Karte.iOS.Core
{
    // @interface KRTConfigurationXamarin : NSObject
    [BaseType(typeof(NSObject), Name = "KRTConfigurationXamarin")]
    interface KRTConfiguration
    {
        // @property (readonly, nonatomic, strong, class) KRTConfigurationXamarin * _Nonnull defaultConfiguration;
        [Static]
        [Export("defaultConfiguration", ArgumentSemantic.Strong)]
        KRTConfiguration DefaultConfiguration { get; }

        // @property (nonatomic) BOOL isDryRun;
        [Export("isDryRun")]
        bool IsDryRun { get; set; }

        // @property (nonatomic) BOOL isOptOut;
        [Export("isOptOut")]
        bool IsOptOut { get; set; }

        [Wrap("WeakIdfaDelegate")]
        [NullAllowed]
        KRTIDFADelegate IdfaDelegate { get; set; }

        // @property (nonatomic, weak) id<KRTIDFADelegateXamarin> _Nullable idfaDelegate;
        [NullAllowed, Export("idfaDelegate", ArgumentSemantic.Weak)]
        NSObject WeakIdfaDelegate { get; set; }
    }

    // @interface KRTXamarin : NSObject
    [BaseType(typeof(NSObject), Name = "KRTXamarin")]
    interface KRTApp
    {
        // @property (readonly, copy, nonatomic, class) NSString * _Nonnull visitorId;
        [Static]
        [Export("visitorId")]
        string VisitorId { get; }

        // @property (readonly, nonatomic, strong, class) KRTConfigurationXamarin * _Nonnull configuration;
        [Static]
        [Export("configuration", ArgumentSemantic.Strong)]
        KRTConfiguration Configuration { get; }

        // @property (readonly, nonatomic, class) BOOL isOptOut;
        [Static]
        [Export("isOptOut")]
        bool IsOptOut { get; }

        // +(void)setupWithAppKey:(NSString * _Nonnull)appKey;
        [Static]
        [Export("setupWithAppKey:")]
        void SetupWithAppKey(string appKey);

        // +(void)setupWithAppKey:(NSString * _Nonnull)appKey configuration:(KRTConfigurationXamarin * _Nonnull)configuration;
        [Static]
        [Export("setupWithAppKey:configuration:")]
        void SetupWithAppKey(string appKey, KRTConfiguration configuration);

        //+(void) setLogLevelWithLogLevel:(enum LogLevel)logLevel;
        [Static]
        [Export("setLogLevelWithLogLevel:")]
        void SetLogLevel(KRTLogLevel logLevel);

        //+(void) setLogEnabled:(BOOL) isEnabled;
        [Static]
        [Export("setLogEnabled:")]
        void SetLogEnabled(bool isEnabled);

        // +(void)optIn;
        [Static]
        [Export("optIn")]
        void OptIn();

        // +(void)optOut;
        [Static]
        [Export("optOut")]
        void OptOut();

        // +(void)renewVisitorId;
        [Static]
        [Export("renewVisitorId")]
        void RenewVisitorId();

        // +(BOOL)application:(UIApplication * _Nonnull)app openURL:(NSURL * _Nonnull)url;
        [Static]
        [Export("application:openURL:")]
        bool Application(UIApplication app, NSUrl url);
    }

    // @protocol KRTIDFADelegateXamarin
    [Protocol, Model(AutoGeneratedName = true)]
    [BaseType(typeof(NSObject), Name = "KRTIDFADelegateXamarin")]
    interface KRTIDFADelegate
    {
        // @required @property (readonly, copy, nonatomic) NSString * _Nullable advertisingIdentifierString;
        [Abstract]
        [NullAllowed, Export("advertisingIdentifierString")]
        string AdvertisingIdentifierString { get; }

        // @required @property (readonly, nonatomic) BOOL isAdvertisingTrackingEnabled;
        [Abstract]
        [Export("isAdvertisingTrackingEnabled")]
        bool IsAdvertisingTrackingEnabled { get; }
    }

    // @interface KRTTrackerXamarin : NSObject
    [BaseType(typeof(NSObject), Name = "KRTTrackerXamarin")]
    [DisableDefaultCtor]
    interface KRTTracker
    {
        // -(instancetype _Nonnull)initWithView:(UIView * _Nullable)view __attribute__((objc_designated_initializer));
        [Export("initWithView:")]
        [DesignatedInitializer]
        IntPtr Constructor([NullAllowed] UIView view);

        // +(KRTTrackingTaskXamarin * _Nonnull)track:(NSString * _Nonnull)name;
        [Static]
        [Export("track:")]
        KRTTrackingTask Track(string name);

        // +(KRTTrackingTaskXamarin * _Nonnull)track:(NSString * _Nonnull)name values:(NSDictionary * _Nonnull)values;
        [Static]
        [Export("track:values:")]
        KRTTrackingTask Track(string name, NSDictionary values);

        // +(KRTTrackingTaskXamarin * _Nonnull)identify:(NSDictionary * _Nonnull)values;
        [Static]
        [Export("identify:")]
        KRTTrackingTask Identify(NSDictionary values);

        // +(KRTTrackingTask * _Nonnull)attribute:(NSDictionary<NSString *,id> * _Nonnull)values;
        [Static]
        [Export("attribute:")]
        KRTTrackingTask Attribute(NSDictionary values);

        // +(KRTTrackingTaskXamarin * _Nonnull)view:(NSString * _Nonnull)viewName;
        [Static]
        [Export("view:")]
        KRTTrackingTask View(string viewName);

        // +(KRTTrackingTaskXamarin * _Nonnull)view:(NSString * _Nonnull)viewName title:(NSString * _Nullable)title;
        [Static]
        [Export("view:title:")]
        KRTTrackingTask View(string viewName, [NullAllowed] string title);

        // +(KRTTrackingTaskXamarin * _Nonnull)view:(NSString * _Nonnull)viewName title:(NSString * _Nullable)title values:(NSDictionary * _Nonnull)values;
        [Static]
        [Export("view:title:values:")]
        KRTTrackingTask View(string viewName, [NullAllowed] string title, NSDictionary values);

        // -(KRTTrackingTaskXamarin * _Nonnull)trackForScene:(NSString * _Nonnull)name;
        [Export("trackForScene:")]
        KRTTrackingTask TrackForScene(string name);

        // -(KRTTrackingTaskXamarin * _Nonnull)trackForScene:(NSString * _Nonnull)name values:(NSDictionary * _Nonnull)values;
        [Export("trackForScene:values:")]
        KRTTrackingTask TrackForScene(string name, NSDictionary values);

        // -(KRTTrackingTaskXamarin * _Nonnull)identifyForScene:(NSDictionary * _Nonnull)values;
        [Export("identifyForScene:")]
        KRTTrackingTask IdentifyForScene(NSDictionary values);

        // -(KRTTrackingTaskXamarin * _Nonnull)viewForScene:(NSString * _Nonnull)viewName;
        [Export("viewForScene:")]
        KRTTrackingTask ViewForScene(string viewName);

        // -(KRTTrackingTaskXamarin * _Nonnull)viewForScene:(NSString * _Nonnull)viewName title:(NSString * _Nullable)title;
        [Export("viewForScene:title:")]
        KRTTrackingTask ViewForScene(string viewName, [NullAllowed] string title);

        // -(KRTTrackingTaskXamarin * _Nonnull)viewForScene:(NSString * _Nonnull)viewName title:(NSString * _Nullable)title values:(NSDictionary * _Nonnull)values;
        [Export("viewForScene:title:values:")]
        KRTTrackingTask ViewForScene(string viewName, [NullAllowed] string title, NSDictionary values);
    }

    // @interface KRTTrackingTaskXamarin : NSObject
    [BaseType(typeof(NSObject), Name = "KRTTrackingTaskXamarin")]
    [DisableDefaultCtor]
    interface KRTTrackingTask
    {
        // @property (copy, nonatomic) void (^ _Nullable)(BOOL) completion;
        [NullAllowed, Export("completion", ArgumentSemantic.Copy)]
        Action<bool> Completion { get; set; }
    }

    // @interface KRTUserSyncXamarin : NSObject
    [BaseType(typeof(NSObject), Name = "KRTUserSyncXamarin")]
    interface KRTUserSync
    {
        // +(NSString * _Nonnull)appendingQueryParameterWithURLString:(NSString * _Nonnull)urlString __attribute__((warn_unused_result("")));
        [Static]
        [Export("appendingQueryParameterWithURLString:")]
        string AppendingQueryParameterWithURLString(string urlString);

        // +(NSURL * _Nonnull)appendingQueryParameterWithURL:(NSURL * _Nonnull)url __attribute__((warn_unused_result("")));
        [Static]
        [Export("appendingQueryParameterWithURL:")]
        NSUrl AppendingQueryParameterWithURL(NSUrl url);

        // +(void)setUserSyncScriptWithWebView:(WKWebView * _Nonnull)webView;
        [Static]
        [Export("setUserSyncScriptWithWebView:")]
        void SetUserSyncScriptWithWebView(WKWebView webView);

        // +(NSString * _Nullable)getUserSyncScript __attribute__((warn_unused_result("")));
        [Static]
        [NullAllowed, Export("getUserSyncScript")]
        string UserSyncScript { get; }
    }
}