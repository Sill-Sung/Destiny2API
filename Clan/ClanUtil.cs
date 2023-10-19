using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GroupsV2.Container;
using GroupsV2.Enums;

namespace Destiny2API.Clan
{
    internal class ClanUtil
    {
        HttpClient client;

        public ClanUtil(HttpClient client)
        {
            this.client = client;
        }

        public ClanInfo GetClanInfo(string name)
        {
            ClanInfo clanInfo = GetClanInfoByName(name);

            clanInfo.ClanMembers = GetClanMembersInfo(clanInfo.ClanId);

            return clanInfo;
        }

        public ClanInfo GetClanInfoByName(string name)
        {
            string url = $"http://www.bungie.net/platform/GroupV2/Name/{name}/{GroupType.Clan}/";

            HttpResponseMessage response = client.GetAsync(url).Result;
            string content = response.Content.ReadAsStringAsync().Result;
            dynamic content_json = Newtonsoft.Json.JsonConvert.DeserializeObject(content);


            ClanInfo clanInfo = new ClanInfo();

            clanInfo.Info.GroupId = content_json.Response.detail.groupId;
            clanInfo.Info.Name = content_json.Response.detail.name;
            clanInfo.Info.GroupType = content_json.Response.detail.groupType;



            return clanInfo;
        }

        public List<GroupUserInfo> GetClanMembersInfo(int clanId)
        {
            List < GroupUserInfo > clanMembers = new List < GroupUserInfo >();

            string url = $"http://www.bungie.net/platform/GroupV2/{clanId}/Members/";

            HttpResponseMessage response = client.GetAsync(url).Result;
            string content = response.Content.ReadAsStringAsync().Result;
            dynamic content_json = Newtonsoft.Json.JsonConvert.DeserializeObject(content);

            int totalResults = content_json.Response.totalResults;

            for (int i = 0; i < totalResults; i++)
            {
                GroupUserInfo clanMember = new GroupUserInfo();

                clanMembers.Add(clanMember);
            }

            return clanMembers;
        }
    }
}
