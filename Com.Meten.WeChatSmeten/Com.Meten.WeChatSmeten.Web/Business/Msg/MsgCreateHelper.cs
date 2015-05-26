using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Com.Meten.WeChatSmeten.Entities;
using Com.Meten.WeChatSmeten.Web.Data;

namespace Com.Meten.WeChatSmeten.Web.Business
{
    /// <summary>
    /// 被动回复时 。。。消息体的生成类
    /// </summary>
    public class MsgCreateHelper
    {
        /// <summary>
        /// 创建回复文本
        /// </summary>
        /// <returns></returns>
        public static string CreateTextReplyString(BaseMsg bmMsg, string replayContent)
        {
            return string.IsNullOrEmpty(replayContent)? "" : string.Format(CommonData.MessageText, bmMsg.FromUserName, bmMsg.ToUserName, bmMsg.CreateTime,
                    replayContent);
        }

        /// <summary>
        /// 创建图片回复
        /// </summary>
        /// <param name="bmMsg"></param>
        /// <param name="_mediaId"></param>
        /// <returns></returns>
        public static string CreateImageReplyString(BaseMsg bmMsg, string _mediaId)
        {
            string replyContent = "";
            if (!string.IsNullOrEmpty(_mediaId))
            {
                return string.Format(CommonData.MessagePicture, bmMsg.FromUserName, bmMsg.ToUserName, bmMsg.CreateTime, _mediaId);
            }
            return replyContent;
        }

        /// <summary>
        /// 创建语音回复
        /// </summary>
        /// <param name="bmMsg"></param>
        /// <param name="_mediaId"></param>
        /// <returns></returns>
        public static string CreateVoiceReplyString(BaseMsg bmMsg, string _mediaId)
        {
            string replyContent = "";
            if (!string.IsNullOrEmpty(_mediaId))
            {
                return string.Format(CommonData.MessageVoice, bmMsg.FromUserName, bmMsg.ToUserName, bmMsg.CreateTime, _mediaId);
            }
            return replyContent;
        }
        /// <summary>
        /// 创建视频回复
        /// </summary>
        /// <param name="bmMsg"></param>
        /// <param name="_mediaId"></param>
        /// <param name="_title"></param>
        /// <param name="_description"></param>
        /// <returns></returns>
        public static string CreateVideoReplyString(BaseMsg bmMsg, string _mediaId, string _title, string _description)
        {
            string replyContent = "";
            if (!string.IsNullOrEmpty(_mediaId))
            {
                return string.Format(CommonData.MessageVideo, bmMsg.FromUserName, bmMsg.ToUserName, bmMsg.CreateTime, _mediaId, _title, _description);
            }
            return replyContent;
        }
        /// <summary>
        /// 创建音乐回复
        /// </summary>
        /// <param name="bmMsg"></param>
        /// <param name="_title">标题</param>
        /// <param name="_description">描述</param>
        /// <param name="musicUrl">音乐链接</param>
        /// <param name="hMusicUrl">高质量的音乐链接</param>
        /// <param name="thumbMediaId">缩略图</param>
        /// <returns></returns>
        public static string CreateMusicReplyString(BaseMsg bmMsg, string _title, string _description, string musicUrl, string hMusicUrl, string thumbMediaId)
        {

            return string.Format(CommonData.MessageMusic, bmMsg.FromUserName, bmMsg.ToUserName, bmMsg.CreateTime, _title, _description, musicUrl, hMusicUrl, thumbMediaId);
        }


        /// <summary>
        /// 回复图文消息
        /// </summary>
        /// <param name="bmMsg"></param>
        /// <param name="listArticles"></param>
        /// <returns></returns>
        public static string CreateNewsReplyString(BaseMsg bmMsg, List<Article> listArticles)
        {
            string stringItems = "";
            foreach (Article article in listArticles)
            {
                stringItems += string.Format(CommonData.MessageNewsItem, article.Title, article.Description, article.PicUrl, article.Url);
            }
            return string.Format(CommonData.MessageNewsMain, bmMsg.FromUserName, bmMsg.ToUserName, bmMsg.CreateTime, listArticles.Count, stringItems);
        }



    }
}