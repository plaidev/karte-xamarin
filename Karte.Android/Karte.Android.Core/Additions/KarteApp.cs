using System;
using System.IO;
using Android.Runtime;
using Android.Content;
using Config = IO.Karte.Android.Core.Config.Config;
using IO.Karte.Android.Core.Logger;
using System.Collections.Generic;

namespace IO.Karte.Android
{
    public partial class KarteApp
    {
        private static readonly string TAG = "KarteApp";

        private static void PrintAssemblies()
        {
            try
            {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (var assembly in assemblies)
                {
                    Logger.D(TAG, $"print assembly: {assembly.GetName()}");
                    if (!assembly.GetName().Name.Contains("Karte")) continue;
                    var names = assembly.GetManifestResourceNames();
                    foreach (var name in names)
                    {
                        Logger.D(TAG, $"print resources: {name}");
                        if (name.EndsWith("io.karte.android.core.library.Library"))
                        {
                            using (Stream stream = assembly.GetManifestResourceStream(name))
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                string line;
                                while ((line = reader.ReadLine()) != null)
                                {
                                    Logger.D(TAG, line);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.E(TAG, $"error occured. {ex}");
            }
        }

        private static IEnumerable<string> SearchKarteLibraries()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var names = assembly.GetManifestResourceNames();
                foreach (var name in names)
                {
                    if (name.EndsWith("io.karte.android.core.library.Library"))
                    {
                        using (Stream stream = assembly.GetManifestResourceStream(name))
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string line;
                            while ((line = reader.ReadLine()) != null)
                            {
                                yield return line.Replace(".", "/");
                            }
                        }
                    }
                }
            }
        }

        private static void Register(String libraryClassName)
        {
            Logger.D(TAG, $"Register library: {libraryClassName}");
            IntPtr target_class_ref = JNIEnv.FindClass(libraryClassName);
            IntPtr constructor_id = JNIEnv.GetMethodID(target_class_ref, "<init>", "()V");
            IntPtr refInstance = JNIEnv.NewObject(target_class_ref, constructor_id);

            IntPtr app_class_ref = JNIEnv.FindClass("io/karte/android/KarteApp");
            IntPtr register_id = JNIEnv.GetStaticMethodID(app_class_ref, "register", "(Lio/karte/android/core/library/Library;)V");
            JNIEnv.CallStaticVoidMethod(app_class_ref, register_id, new JValue(refInstance));
        }

        public static void Setup(Context context, string appKey, Config config = null)
        {
            try
            {
                Logger.D(TAG, "search karte libraries");
                foreach (var library in SearchKarteLibraries())
                {
                    Register(library);
                }
                Logger.D(TAG, "add xamarin library");
                Register(library: new XamarinLibrary());
            }
            catch (Exception ex)
            {
                Logger.E(TAG, $"error occured. {ex}");
            }

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
