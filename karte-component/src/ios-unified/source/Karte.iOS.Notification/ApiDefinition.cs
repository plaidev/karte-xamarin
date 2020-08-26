using System;

using ObjCRuntime;
using Foundation;

namespace Karte.iOS.Notification
{
	// @interface KRTRemoteNotification : NSObject
	[BaseType(typeof(NSObject), Name = "KRTRemoteNotification")]
	[DisableDefaultCtor]
	interface KRTNotification
	{
		// @property (readonly, copy, nonatomic) NSURL * _Nullable url;
		[NullAllowed, Export("url", ArgumentSemantic.Copy)]
		NSUrl Url { get; }

		// -(instancetype _Nullable)initWithUserInfo:(NSDictionary * _Nonnull)userInfo __attribute__((objc_designated_initializer));
		[Export("initWithUserInfo:")]
		[DesignatedInitializer]
		IntPtr Constructor(NSDictionary userInfo);

		// -(BOOL)handle;
		[Export("handle")]
		bool HandleNotification();
    }
}

