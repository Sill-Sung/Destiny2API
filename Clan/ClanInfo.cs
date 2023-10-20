using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GroupsV2.Data.Containers;
using GroupsV2.Enums;

namespace Destiny2API.Clan
{
    internal class ClanInfo
    {
        GroupV2 info;
        long clanId;
        string name;
        string callSign;
        int memberCount = 0;
        List<GroupUserInfoBase> clanMembers = null;

        public ClanInfo()
        {
            Info = new GroupV2();
            ClanMembers = new List<GroupUserInfoBase>();
        }

        public long ClanId { get => clanId; set => clanId = value; }

        public List<GroupUserInfoBase> ClanMembers { get => clanMembers; set => clanMembers = value; }
        public GroupV2 Info { get => info; set => info = value; }
        public int MemberCount { get => memberCount; set => memberCount = value; }
        public string CallSign { get => callSign; set => callSign = value; }
        public string Name { get => name; set => name = value; }
    }
}
