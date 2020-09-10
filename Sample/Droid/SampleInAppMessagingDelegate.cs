using IO.Karte.Android.Inappmessaging;

namespace SampleApp.Droid
{
    public class SampleInAppMessagingDelegate : InAppMessagingDelegate
    {
        public override void OnDismissed(string campaignId, string shortenId)
        {
            base.OnDismissed(campaignId, shortenId);
            System.Diagnostics.Debug.WriteLine("InAppMessaging OnDismissed with campaignId: " + campaignId);
        }

        public override void OnPresented(string campaignId, string shortenId)
        {
            base.OnPresented(campaignId, shortenId);
            System.Diagnostics.Debug.WriteLine("InAppMessaging OnPresented with campaignId: " + campaignId);
        }

        public override void OnWindowDismissed()
        {
            base.OnWindowDismissed();
            System.Diagnostics.Debug.WriteLine("InAppMessaging OnWindowDismissed");
        }

        public override void OnWindowPresented()
        {
            base.OnWindowPresented();
            System.Diagnostics.Debug.WriteLine("InAppMessaging OnWindowPresented");
        }

        public override bool ShouldOpenURL(Android.Net.Uri url)
        {
            System.Diagnostics.Debug.WriteLine("InAppMessaging ShouldOpenURL: " + url);
            return base.ShouldOpenURL(url);
        }
    }
}
