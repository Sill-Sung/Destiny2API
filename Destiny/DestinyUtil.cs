using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bungie.Enums;
using Destiny.Data.Containers;
using Newtonsoft.Json.Linq;

namespace Destiny2API.Destiny
{
    internal class DestinyUtil
    {
        HttpClient client = null;

        public DestinyUtil(HttpClient client)
        {
            this.client = client;
        }

        public DestinyLinkedProfilesResponse GetDestinyUserProfile(long membershipId)
        {
            return GetDestinyUserProfile(BungieMembershipType.All, membershipId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="membershipId">
        /// The ID of the membership whose linked Destiny accounts you want returned.
        /// Make sure your membership ID matches its Membership Type: don't pass us a PSN membership ID and the XBox membership type, it's not going to work!
        /// </param>
        /// <param name="membershipType">
        /// The type for the membership whose linked Destiny accounts you want returned.
        /// The types of membership the Accounts system supports.
        /// This is the external facing enum used in place of the internal-only Bungie.SharedDefinitions.MembershipType.
        /// </param>
        public DestinyLinkedProfilesResponse GetDestinyUserProfile(BungieMembershipType membershipType, long membershipId)
        {
            string url = $"http://www.bungie.net/platform/Destiny2/{membershipType}/Profile/{membershipId}/LinkedProfiles/";

            HttpResponseMessage response = client.GetAsync(url).Result;
            string content = response.Content.ReadAsStringAsync().Result;
            dynamic content_json = Newtonsoft.Json.JsonConvert.DeserializeObject(content);

            JObject jo_result = content_json.Response;

            DestinyLinkedProfilesResponse profilesResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<DestinyLinkedProfilesResponse>(jo_result.ToString());

            return profilesResponse;
        }
    }
}
