using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using Com.Meten.WeChatSmeten.Web.Data;

namespace Com.Meten.WeChatSmeten.Web.Business
{
    /// <summary>
    /// 链接消息
    /// </summary>
    public class LinkMsg : BaseMsg
    {
        private readonly string _title;
        private readonly string _description;
        private readonly string _url;

        public LinkMsg(XmlDocument xmldoc) : base(xmldoc)
        {
            //<xml>
            //<ToUserName><![CDATA[toUser]]></ToUserName>
            //<FromUserName><![CDATA[fromUser]]></FromUserName>
            //<CreateTime>1351776360</CreateTime>
            //<MsgType><![CDATA[link]]></MsgType>
            //<Title><![CDATA[公众平台官网链接]]></Title>
            //<Description><![CDATA[公众平台官网链接]]></Description>
            //<Url><![CDATA[url]]></Url>
            //<MsgId>1234567890123456</MsgId>
            //</xml> 
            //参数	描述
            //ToUserName	 接收方微信号
            //FromUserName	 发送方微信号，若为普通用户，则是一个OpenID
            //CreateTime	 消息创建时间
            //MsgType	 消息类型，link
            //Title	 消息标题
            //Description	 消息描述
            //Url	 消息链接
            //MsgId	 消息id，64位整型
            XmlNode xmlTitle = xmldoc.SelectSingleNode("/xml/Title");
            if (xmlTitle == null) throw new ArgumentNullException("xmlTitle");
            XmlNode xmlDescription = xmldoc.SelectSingleNode("/xml/Description");
            if (xmlDescription == null) throw new ArgumentNullException("xmlDescription");
            XmlNode xmlUrl = xmldoc.SelectSingleNode("/xml/Url");
            if (xmlUrl == null) throw new ArgumentNullException("xmlUrl");
            _title = xmlTitle.InnerText;
            _description = xmlDescription.InnerText;
            _url = xmlUrl.InnerText;
        }

        /// <summary>
        /// 被动回复返回消息
        /// </summary>
        /// <returns></returns>
        public override string ReturnXmlString()
        {
            return string.Format(CommonData.MessageText, FromUserName, ToUserName, CreateTime, string.Format("{0}：您好。您所发送的链接已经收到，链接地址为：{1}；标题为：{2}；描述为：{3}", FromUser.nickname, _url, _title, _description));
        }
    }
}