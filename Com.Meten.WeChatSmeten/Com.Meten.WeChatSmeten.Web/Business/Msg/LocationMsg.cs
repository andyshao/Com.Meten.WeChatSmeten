using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using Com.Meten.WeChatSmeten.Entities;
using Com.Meten.WeChatSmeten.Helper;
using Com.Meten.WeChatSmeten.Web.Data;

namespace Com.Meten.WeChatSmeten.Web.Business
{
    /// <summary>
    /// 地理位置消息
    /// </summary>
    public class LocationMsg : BaseMsg
    {
        private readonly string _locationX = "";
        private readonly string _locationY = "";
        private readonly string _scale = "";
        private readonly string _label = "";

        public LocationMsg(XmlDocument xmldoc) : base(xmldoc)
        {
            //<xml>
            //<ToUserName><![CDATA[toUser]]></ToUserName>
            //<FromUserName><![CDATA[fromUser]]></FromUserName>
            //<CreateTime>1351776360</CreateTime>
            //<MsgType><![CDATA[location]]></MsgType>
            //<Location_X>23.134521</Location_X>
            //<Location_Y>113.358803</Location_Y>
            //<Scale>20</Scale>
            //<Label><![CDATA[位置信息]]></Label>
            //<MsgId>1234567890123456</MsgId>
            //</xml> 
            //参数	描述
            //ToUserName	开发者微信号
            //FromUserName	 发送方帐号（一个OpenID）
            //CreateTime	 消息创建时间 （整型）
            //MsgType	 location
            //Location_X	 地理位置维度
            //Location_Y	 地理位置经度
            //Scale	 地图缩放大小
            //Label	 地理位置信息
            //MsgId	 消息id，64位整型
            XmlNode xmlLocationX = xmldoc.SelectSingleNode("/xml/Location_X");
            if (xmlLocationX == null) throw new ArgumentNullException("Location_X");
            XmlNode xmlLocationY = xmldoc.SelectSingleNode("/xml/Location_Y");
            if (xmlLocationY == null) throw new ArgumentNullException("Location_Y");
            XmlNode xmlScale = xmldoc.SelectSingleNode("/xml/Scale");
            if (xmlScale == null) throw new ArgumentNullException("Scale");
            XmlNode xmlLabel = xmldoc.SelectSingleNode("/xml/Label");
            if (xmlLabel == null) throw new ArgumentNullException("Label");
            _locationX = xmlLocationX.InnerText;
            _locationY = xmlLocationY.InnerText;
            _scale = xmlScale.InnerText;
            _label = xmlLabel.InnerText;
        }

        /// <summary>
        /// 被动回复
        /// </summary>
        /// <returns></returns>
        public override string ReturnXmlString()
        {
            string replyContent = "";
            //LogHelper.WriteInfo(string.Format("地理消息；fromUserName:{0}({1});经度:{2};纬度:{3};比例:{4};位置标识:{5};MsgID:{6};", FromUserName, FromUser.nickname, _locationX, _locationY, _scale, _label, MsgId));
            replyContent = string.Format(CommonData.MessageText, FromUserName, ToUserName, CreateTime,string.Format("尊敬的{0}用户；您发送了地理位置信息已经收到。经度:{1};纬度:{2};比例:{3};位置标识:{4}",FromUser.nickname, _locationX, _locationY,_scale, _label));
            return replyContent;
        }
    }
}