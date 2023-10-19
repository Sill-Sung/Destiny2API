using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GroupsV2.Container;
using GroupsV2.Enums;

namespace Destiny2API.Clan
{
    internal class ClanInfo
    {
        GroupV2 info;
        int clanId;
        string name;
        string callSign;
        int memberCount = 0;
        List<GroupUserInfo> clanMembers = null;

        public ClanInfo()
        {
            Info = new GroupV2();
            ClanMembers = new List<GroupUserInfo>();
        }

        public int ClanId { get => clanId; set => clanId = value; }

        public List<GroupUserInfo> ClanMembers { get => clanMembers; set => clanMembers = value; }
        public GroupV2 Info { get => info; set => info = value; }
    }
}
