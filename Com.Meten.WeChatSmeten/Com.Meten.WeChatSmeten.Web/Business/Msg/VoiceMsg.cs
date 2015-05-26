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
    /// 语音消息
    /// </summary>
    public class VoiceMsg : BaseMsg
    {
        private readonly string _format;
        private readonly string _recognition;
        private readonly string _mediaId;
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="xmldoc"></param>
        public VoiceMsg(XmlDocument xmldoc) : base(xmldoc)
        {
//<xml>
//<ToUserName><![CDATA[toUser]]></ToUserName>
//<FromUserName><![CDATA[fromUser]]></FromUserName>
//<CreateTime>1357290913</CreateTime>
//<MsgType><![CDATA[voice]]></MsgType>
//<MediaId><![CDATA[media_id]]></MediaId>
//<Format><![CDATA[Format]]></Format>
//<Recognition><![CDATA[腾讯微信团队]]></Recognition>
//<MsgId>1234567890123456</MsgId>
//</xml>
//参数	            描述
//ToUserName	开发者微信号
//FromUserName	 发送方帐号（一个OpenID）
//CreateTime	 消息创建时间 （整型）
//MsgType	 语音为voice
//MediaID	 语音消息媒体id，可以调用多媒体文件下载接口拉取该媒体
//Format	 语音格式：amr
//Recognition	 语音识别结果，UTF8编码
//MsgID	 消息id，64位整型
            XmlNode xmlMediaId = xmldoc.SelectSingleNode("/xml/MediaId");
            if (xmlMediaId == null) throw new ArgumentNullException("xmlMediaId");
            XmlNode xmlFormat = xmldoc.SelectSingleNode("/xml/Format");
            if (xmlFormat == null) throw new ArgumentNullException("xmlFormat");
            XmlNode xmlRecognition = xmldoc.SelectSingleNode("/xml/Recognition");
            if (xmlRecognition != null)
            {
                _recognition = xmlRecognition.InnerText;
            }
            else
            {
                //throw new ArgumentNullException("xmlRecognition");
            }
            _format = xmlFormat.InnerText;
            _mediaId = xmlMediaId.InnerText;
        }

        public override string ReturnXmlString()
        {
            //LogHelper.WriteInfo(string.Format("语音消息；fromUserName:{0};media_id:{1};Format(格式):{2};Recognition(翻译):{3};MsgID:{4};",FromUserName, _mediaId, _format, _recognition, MsgId));
            //return string.Format(CommonData.MessageVoice, FromUserName, ToUserName, CreateTime, _mediaId);
            return MsgCreateHelper.CreateImageReplyString(this, _mediaId);
        }
    }
}








         