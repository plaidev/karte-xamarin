using System;
using Foundation;
using Karte.iOS.InAppMessaging;

namespace KarteiOSSample
{
    public class SampleInAppMessagingDelegate : KRTInAppMessagingDelegate
    {
        public override bool InAppMessaging(KRTInAppMessaging inAppMessaging, NSUrl url)
        {
            return true;
        }

        public override void InAppMessagingIsDismissed(KRTInAppMessaging inAppMessaging, string campaignId, string shortenId)
        {
            System.Diagnostics.Debug.WriteLine("InAppMessagingIsDismissed with campaignId: " + campaignId);
        }
    }
}