using IO.Karte.Android.Core.Library;

namespace IO.Karte.Android
{
    internal class XamarinLibrary : Java.Lang.Object, ILibrary
    {
        public bool IsPublic => true;

        public string Name => "xamarin";

        public string Version => "0.0.1";

        public void Configure(KarteApp app) { }

        public void Unconfigure(KarteApp app) { }
    }
}
