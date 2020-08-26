using AdSupport;
using Karte.iOS.Core;

namespace KarteiOSSample
{
    public class SampleIDFADelegate : KRTIDFADelegate
    {
        public override string AdvertisingIdentifierString => ASIdentifierManager.SharedManager.AdvertisingIdentifier.AsString();


        public override bool IsAdvertisingTrackingEnabled => ASIdentifierManager.SharedManager.IsAdvertisingTrackingEnabled;
    }
}
