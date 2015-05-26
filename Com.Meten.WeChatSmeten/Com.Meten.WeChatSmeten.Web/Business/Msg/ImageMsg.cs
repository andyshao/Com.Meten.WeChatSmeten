using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using Com.Meten.WeChatSmeten.Helper;
using Com.Meten.WeChatSmeten.Web.Data;

namespace Com.Meten.WeChatSmeten.Web.Business
{
    /// <summary>
    /// 图片消息
    /// </summary>
    public class ImageMsg : BaseMsg
    {
        private readonly string _pictureUrl;
        private readonly string _mediaId;

        public ImageMsg(XmlDocument xmldoc)
            : base(xmldoc)
        {
            //<xml>
            //<ToUserName><![CDATA[toUser]]></ToUserName>
            //<FromUserName><![CDATA[fromUser]]></FromUserName>
            //<CreateTime>1348831860</CreateTime>
            //<MsgType><![CDATA[image]]></MsgType>
            //<PicUrl><![CDATA[this is a url]]></PicUrl>
            //<MediaId><![CDATA[media_id]]></MediaId>
            //<MsgId>1234567890123456</MsgId>
            //</xml>
            //ToUserName	开发者微信号
            //FromUserName	 发送方帐号（一个OpenID）
            //CreateTime	 消息创建时间 （整型）
            //MsgType	 image
            //PicUrl	 图片链接
            //MediaId	 图片消息媒体id，可以调用多媒体文件下载接口拉取数据。
            //MsgId	 消息id，64位整型
            XmlNode xmlPicUrl = xmldoc.SelectSingleNode("/xml/PicUrl");
            if (xmlPicUrl == null) throw new ArgumentNullException("PicUrl");
            XmlNode xmlMediaId = xmldoc.SelectSingleNode("/xml/MediaId");
            if (xmlMediaId == null) throw new ArgumentNullException("media_id");
            _pictureUrl = xmlPicUrl.InnerText;
            _mediaId = xmlMediaId.InnerText;
        }


        public override string ReturnXmlString()
        {
            return MsgCreateHelper.CreateImageReplyString(this, _mediaId);
        }
    }
}