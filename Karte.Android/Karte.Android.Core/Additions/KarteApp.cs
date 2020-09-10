using System;
using Android.Util;
using Android.Runtime;
using Android.Content;
using Config = IO.Karte.Android.Core.Config.Config;

namespace IO.Karte.Android
{
    public partial class KarteApp
    {
        public static void PrintAssemblies()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                Log.Debug("KarteApp", $"print assembly: {assembly.GetName()}");
            }
        }

        public static void Register(String libraryClassName)
        {
            IntPtr target_class_ref = JNIEnv.FindClass(libraryClassName);
            IntPtr constructor_id = JNIEnv.GetMethodID(target_class_ref, "<init>", "()V");
            IntPtr refInstance = JNIEnv.NewObject(target_class_ref, constructor_id);

            IntPtr app_class_ref = JNIEnv.FindClass("io/karte/android/KarteApp");
            IntPtr register_id = JNIEnv.GetStaticMethodID(app_class_ref, "register", "(Lio/karte/android/core/library/Library;)V");
            JNIEnv.CallStaticVoidMethod(app_class_ref, register_id, new JValue(refInstance));

            IntPtr self_id = JNIEnv.GetStaticFieldID(app_class_ref, "self", "Lio/karte/android/KarteApp;");
            IntPtr self = JNIEnv.GetStaticObjectField(app_class_ref, self_id);

            IntPtr configure_id = JNIEnv.GetMethodID(target_class_ref, "configure", "(Lio/karte/android/KarteApp;)V");
            JNIEnv.CallVoidMethod(refInstance, configure_id, new JValue(self));
        }

        public static void Setup(Context context, string appKey, Config config = null)
        {
            if (config == null)
            {
                SetupInternal(context, appKey);
            }
            else
            {
                SetupInternal(context, appKey, config);
            }
        }
    }
}
