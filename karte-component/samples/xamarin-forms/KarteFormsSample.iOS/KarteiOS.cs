using System;
using KarteFormsSample.iOS;
using Xamarin.Forms;
using Karte.iOS.Core;

[assembly: Dependency(typeof(KarteiOS))]

namespace KarteFormsSample.iOS
{
    public class KarteiOS : IKarte
    {
        public KarteiOS()
        {
        }

        public void Setup(string appkey)
        {
            KRTApp.SetupWithAppKey(appkey, KRTConfiguration.DefaultConfiguration);            
        }

        public void View(string title)
        {
            KRTTracker.View(title);
        }

        
    }
}
