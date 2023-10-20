using System;
using System.Threading.Tasks;

using Destiny2API.Clan;
using Destiny2API.Destiny;

using Bungie.Enums;
using Destiny.Data.Containers;
using User.Data.Containers;

namespace Destiny2API
{
    class Program
    {
        HttpClient client = new HttpClient();
        ClanInfo clanInfo = new ClanInfo();     // Clan : 어딜 내놔도 좀

        string x_api_key = "66dfbc1e62bc4016a36e239dd96550dc";

        private static void Main(string[] args) => new Program().SetClanInfo();

        public Program()
        {
            client.DefaultRequestHeaders.Add("X-API-Key", x_api_key);
        }

        public void SetClanInfo()
        {
            ClanUtil clanUtil = new ClanUtil(client);

            clanUtil.GetLongTermUnplayedMembers("어딜 내놔도 좀", 21);
        }
    }
}