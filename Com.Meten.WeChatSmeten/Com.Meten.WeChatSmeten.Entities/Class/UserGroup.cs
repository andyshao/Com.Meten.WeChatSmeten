using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Meten.WeChatSmeten.Entities
{
    [Serializable]
    public class UserGroup
    {
        public string Id { set; get; }
        public string Name { set; get; }
        public string Count { set; get; }

        public UserGroup()
        {
        }

        public UserGroup(string id, string name, string count)
        {
            this.Id = id;
            this.Name = name;
            this.Count = count;
        }
    }
}
