using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

using GroupsV2.Data.Containers;
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

            JObject jo_detail = content_json.Response.detail;

            ClanInfo clanInfo = new ClanInfo();

            clanInfo.Info = Newtonsoft.Json.JsonConvert.DeserializeObject<GroupV2>(jo_detail.ToString());
            clanInfo.ClanId = clanInfo.Info.GroupId;
            clanInfo.MemberCount = clanInfo.Info.MemberCount;
            clanInfo.Name = clanInfo.Info.Name;

            return clanInfo;
        }

        public List<GroupUserInfoBase> GetClanMembersInfo(long clanId)
        {
            List <GroupUserInfoBase> clanMembers = new List <GroupUserInfoBase>();

            string url = $"http://www.bungie.net/platform/GroupV2/{clanId}/Members/";

            HttpResponseMessage response = client.GetAsync(url).Result;
            string content = response.Content.ReadAsStringAsync().Result;
            dynamic content_json = Newtonsoft.Json.JsonConvert.DeserializeObject(content);

            JArray results = content_json.Response.results;

            foreach (JObject result in results)
            {
                GroupUserInfoBase clanMember = new GroupUserInfoBase();

                clanMember = Newtonsoft.Json.JsonConvert.DeserializeObject<GroupUserInfoBase>(result.ToString());

                clanMembers.Add(clanMember);
            }

            return clanMembers;
        }
    }
}