using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using Com.Meten.WeChatSmeten.Helper;
using Com.Meten.WeChatSmeten.Web.Data;

namespace Com.Meten.WeChatSmeten.Web.Business
{
    public class VideoMsg : BaseMsg
    {

        private readonly string _mediaId = "";
        private readonly string _thumbMediaId = "";


        public VideoMsg(XmlDocument xmldoc) : base(xmldoc)
        {
            //<xml>
            //<ToUserName><![CDATA[toUser]]></ToUserName>
            //<FromUserName><![CDATA[fromUser]]></FromUserName>
            //<CreateTime>1357290913</CreateTime>
            //<MsgType><![CDATA[video]]></MsgType>
            //<MediaId><![CDATA[media_id]]></MediaId>
            //<ThumbMediaId><![CDATA[thumb_media_id]]></ThumbMediaId>
            //<MsgId>1234567890123456</MsgId>
            //</xml>
            //ToUserName	开发者微信号
            //FromUserName	 发送方帐号（一个OpenID）
            //CreateTime	 消息创建时间 （整型）
            //MsgType	 视频为video
            //MediaId	 视频消息媒体id，可以调用多媒体文件下载接口拉取数据。
            //ThumbMediaId	 视频消息缩略图的媒体id，可以调用多媒体文件下载接口拉取数据。
            //MsgId	 消息id，64位整型
            XmlNode xmlMediaId = xmldoc.SelectSingleNode("/xml/MediaId");
            if (xmlMediaId == null) throw new ArgumentNullException("Media_id");
            XmlNode xmlThumbMediaId = xmldoc.SelectSingleNode("/xml/ThumbMediaId");
            if (xmlThumbMediaId == null) throw new ArgumentNullException("ThumbMediaId");
            _mediaId = xmlMediaId.InnerText;
            _thumbMediaId = xmlThumbMediaId.InnerText;
        }

        /// <summary>
        /// 被动回复
        /// </summary>
        /// <returns></returns>
        public override string ReturnXmlString()
        {
            LogHelper.WriteInfo(string.Format("收到用户{0}(昵称{1})发的视频消息：media_id:{2};ThumbMediaId:{3};MsgID:{4};", FromUserName, FromUser.nickname, _mediaId, _thumbMediaId, MsgId));
#warning 回复视频信息接口没调通，根据官方文档来做仍有问题。。 
            
            //return  string.Format(CommonData.MessageVideo,FromUserName,ToUserName,CreateTime,_mediaId, string.Format("{0}发来的视频。",FromUser.nickname), string.Format("{0}发来的视频；视频消息缩略图的媒体id为{1}。",FromUser.nickname,_thumbMediaId));
            return MsgCreateHelper.CreateVideoReplyString(this, _mediaId, string.Format("{0}发来的视频。", FromUser.nickname), string.Format("{0}发来的视频；视频消息缩略图的媒体id为{1}。", FromUser.nickname, _thumbMediaId));

            //return string.Format(CommonData.MessageText, FromUserName, ToUserName, CreateTime, "您所发的视频已经收到，暂时无法处理;回复0转人工。");
            //return MsgCreateHelper.CreateTextReplyString(this, "您所发的视频已经收到，暂时无法处理;回复0转人工。");
        }
    }
}