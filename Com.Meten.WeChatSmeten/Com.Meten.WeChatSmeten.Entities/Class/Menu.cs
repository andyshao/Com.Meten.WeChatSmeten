using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Meten.WeChatSmeten.Entities
{
    /// <summary>
    /// 按钮
    /// </summary>
    public class UserButton
    {
        public UserButtonType ubttype { set; get; }

        public string type { set; get; }

        public string name { set; get; }

        public string key { get; set; }

        public string url { get; set; }

        public List<UserButton> sub_button { get; set; }
    }

    /// <summary>
    /// 自定义菜单
    /// </summary>
    public class UserMenu
    {
        public List<UserButton> button { get; set; }
    }
}
