using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

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

        /// <summary>
        /// 클랜 이름으로 클랜 정보 및 클랜원 목록 조회
        /// </summary>
        /// <param name="name">클랜 이름</param>
        /// <returns></returns>
        public ClanInfo GetClanInfo(string name)
        {
            ClanInfo clanInfo = GetClanInfoByName(name);

            clanInfo.ClanMembers = GetClanMembersInfo(clanInfo.ClanId);

            return clanInfo;
        }

        /// <summary>
        /// 클랜 이름으로 클랜 정보만 조회
        /// </summary>
        /// <param name="name">클랜 이름</param>
        /// <returns></returns>
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

        /// <summary>
        /// 클랜 ID로 클랜원 목록만 조회
        /// </summary>
        /// <param name="clanId">클랜 ID</param>
        /// <returns></returns>
        public List<GroupUserInfoBase> GetClanMembersInfo(long clanId)
        {
            Destiny2API.Destiny.DestinyUtil destinyUtil = new Destiny2API.Destiny.DestinyUtil(client);

            List <GroupUserInfoBase> clanMembers = new List <GroupUserInfoBase>();

            string url = $"http://www.bungie.net/platform/GroupV2/{clanId}/Members/";

            HttpResponseMessage response = client.GetAsync(url).Result;
            string content = response.Content.ReadAsStringAsync().Result;
            dynamic content_json = Newtonsoft.Json.JsonConvert.DeserializeObject(content);

            JArray results = content_json.Response.results;

            Parallel.ForEach(results, x =>
            {
                GroupUserInfoBase clanMember = new GroupUserInfoBase();

                clanMember = Newtonsoft.Json.JsonConvert.DeserializeObject<GroupUserInfoBase>(x.ToString());

                clanMember.DestinyProfile = destinyUtil.GetDestinyUserProfile(clanMember.DestinyUserInfo.MembershipId);

                clanMembers.Add(clanMember);
            });

            return clanMembers;
        }

        /// <summary>
        /// 클랜 내 장기 미접속 클랜원 목록
        /// </summary>
        /// <param name="name">클랜 이름</param>
        /// <param name="days">기준 일자</param>
        public void GetLongTermUnplayedMembers(string name, int days)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            stopwatch.Start();

            DateTime nowDate = DateTime.Now;
            DateTime compareDate = nowDate.AddDays(days * (-1));

            ClanInfo clanInfo = GetClanInfo(name);
            if (clanInfo == null)
            {
                Console.WriteLine("Not Found Clan Info");
                return;
            }

            List<GroupUserInfoBase> list = clanInfo.ClanMembers
                                                    .Where(x => x.DestinyProfile.Profiles != null)
                                                    .Where(x => x.DestinyProfile.Profiles.MaxBy(x => x.DateLastPlayed).DateLastPlayed.Date < compareDate.Date)
                                                    .OrderBy(x=> x.DestinyProfile.Profiles.MaxBy(y=>y.DateLastPlayed).DateLastPlayed).ToList();

            stopwatch.Stop();
            Console.WriteLine($"[ 현재 일자 : {nowDate.Date.ToString("yyyy-MM-dd")} / 기준 일자 : {compareDate.Date.ToString("yyyy-MM-dd")} (3주 기준) ]");
            Console.WriteLine($"( 연산 소요 시간 : {stopwatch.Elapsed} )\n");

            int index = 0;

            foreach (GroupUserInfoBase clanMember in list)
            {
                index++;

                string bungieName = clanMember.DestinyUserInfo.BungieGlobalDisplayName;
                DateTime lastTime = clanMember.DestinyProfile.Profiles.MaxBy(x => x.DateLastPlayed).DateLastPlayed.Date;
                double elapsedDays = (compareDate.Date - lastTime.Date).TotalDays;
                
                Console.WriteLine("[{0:D2}] {1, -40} : {2} (미접 일수 : {3})", index, bungieName, lastTime.ToString("yyyy-MM-dd"), elapsedDays);
            }
        }
    }
}