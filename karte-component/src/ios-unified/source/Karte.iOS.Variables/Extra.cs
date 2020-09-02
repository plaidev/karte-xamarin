using System.Collections.Generic;
using Foundation;
using Karte.iOS.Core;

namespace Karte.iOS.Variables
{
    public partial class KRTVariables
    {
        public static void Configure()
        {
        }

        public static void TrackVariableOpen(KRTVariable[] variables, NSDictionary values)
        {
            Track("message_open", variables, values);
        }

        public static void TrackVariableClick(KRTVariable[] variables, NSDictionary values)
        {
            Track("message_click", variables, values);
        }

        private static void Track(string message_type, KRTVariable[] variables, NSDictionary values)
        {
            if (variables == null)
            {
                return;
            }

            var sList = new List<string>();
            foreach (var item in variables)
            {
                if (item.CampaignId == null || item.ShortenId == null)
                    return;

                if (sList.Contains(item.CampaignId))
                {
                    return;
                }
                sList.Add(item.CampaignId);

                var campaignIdDict = new NSDictionary("campaign_id", item.CampaignId, "shorten_id", item.ShortenId);
                var messageDict = new NSDictionary("message", campaignIdDict);
                values = values ?? new NSDictionary();
                var mutableDict = new NSMutableDictionary(values);
                mutableDict.AddEntries(messageDict);
                KRTTracker.Track(message_type, (NSDictionary)mutableDict.Copy());
            }
        }
    }
}
