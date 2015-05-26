using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Meten.WeChatSmeten.Entities.Class
{
    public class NewsItem
    {
//title	图文消息的标题
//thumb_media_id	图文消息的封面图片素材id（必须是永久mediaID）
//show_cover_pic	是否显示封面，0为false，即不显示，1为true，即显示
//author	作者
//digest	图文消息的摘要，仅有单图文消息才有摘要，多图文此处为空
//content	图文消息的具体内容，支持HTML标签，必须少于2万字符，小于1M，且此处会去除JS
//content_source_url	图文消息的原文地址，即点击“阅读原文”后的URL
        public string title { set; get; }
        public string thumb_media_id { set; get; }
        public string show_cover_pic { set; get; }
        public string author { set; get; }
        public string digest { set; get; }
        public string content { set; get; }
        public string content_source_url { set; get; }
    }

    public class PictureArticleItem
    {
        public List<NewsItem>  news_item { get; set; }
    }

    public class ArticleItem
    {
        public  string media_id { set; get; }
        public PictureArticleItem content { get; set; }
        public string update_time { set; get; }
    }

    /// <summary>
    /// 获取多图文返回的类
    /// </summary>
    public class NewsMaterial
    {
        public string total_count { set; get; }
        public string item_count { get; set; }
        public List<ArticleItem> item { get; set; } 
    }


//    {
//   "total_count": TOTAL_COUNT,
//   "item_count": ITEM_COUNT,
//   "item": [{
//       "media_id": MEDIA_ID,
//       "name": NAME,
//       "update_time": UPDATE_TIME
//   },
//   //可能会有多个素材
//   ]
//}
    public class MaterialItem
    {
        public string media_id { set; get; }
        public string name { get; set; }
        public string update_time { set; get; }
    }

    /// <summary>
    /// 获取素材（非多图文）返回的类
    /// </summary>
    public class Material
    {
        public string total_count { set; get; }
        public string item_count { get; set; }
        public List<MaterialItem> item { get; set; }
    }



}
