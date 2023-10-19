using System;
using System.Threading.Tasks;

using Destiny2API.Clan;

namespace Destiny2API
{
    class Program
    {
        HttpClient client = null;

        string x_api_key = "66dfbc1e62bc4016a36e239dd96550dc";

        private static void Main(string[] args) => new Program();

        public Program()
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("X-API-Key", x_api_key);
            
            ClanUtil clanUtil = new ClanUtil(client);
            ClanInfo clanInfo = clanUtil.GetClanInfo("어딜 내놔도 좀");
        }

    }
}