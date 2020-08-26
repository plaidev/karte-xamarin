using System;
using System.Runtime.InteropServices;
using Foundation;
using ObjCRuntime;

namespace Karte.iOS.Notification
{
    public partial class KRTNotification
    {
        public static void Configure()
        {
        }

        [DllImport(ObjCRuntime.Constants.ObjectiveCLibrary, EntryPoint = "objc_msgSend")]
        static extern void void_objc_msgSend_intptr(IntPtr target, IntPtr selector, IntPtr ptr);

        public static void RegisterFCMToken(string token)
        {
            if (token == null)
                return;

            NSString nsstr = new NSString(token);
            var selector = new Selector("registerFCMToken:");
            IntPtr krtAppHandle = ObjCRuntime.Class.GetHandle("KRTApp");
            void_objc_msgSend_intptr(krtAppHandle, selector.Handle, nsstr.Handle);
        }

        public static void TrackClickWithUserInfo(NSDictionary userInfo)
        {
            if (userInfo == null)
                return;

            var selector = new Selector("trackClickWithUserInfo:");
            IntPtr krtTrackerHandle = ObjCRuntime.Class.GetHandle("KRTTracker");
            void_objc_msgSend_intptr(krtTrackerHandle, selector.Handle, userInfo.Handle);
        }
    }
}
