using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Meten.WeChatSmeten.Entities
{

    /// <summary>
    /// 图文消息的文章项
    /// </summary>
    public class Article
    {
        public string Title { set; get; }
        public string Description { set; get; }
        public string PicUrl { set; get; }
        public string Url { set; get; }
    }
}
