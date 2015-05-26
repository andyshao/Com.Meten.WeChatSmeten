using System;
using System.Collections.Generic;
using System.Xml;
using Com.Meten.WeChatSmeten.Entities;
using Com.Meten.WeChatSmeten.Helper;
using Com.Meten.WeChatSmeten.Web.Data;

namespace Com.Meten.WeChatSmeten.Web.Business
{
    /// <summary>
    /// 文本消息
    /// </summary>
    public class TextMsg : BaseMsg
    {
        private readonly string _content;
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="xmldoc"></param>
        public TextMsg(XmlDocument xmldoc)
            : base(xmldoc)
        {
            //<xml>
            //<ToUserName><![CDATA[toUser]]></ToUserName>
            //<FromUserName><![CDATA[fromUser]]></FromUserName> 
            //<CreateTime>1348831860</CreateTime>
            //<MsgType><![CDATA[text]]></MsgType>
            //<Content><![CDATA[this is a test]]></Content>
            //<MsgId>1234567890123456</MsgId>
            //</xml>
            //参数	描述
            //ToUserName	开发者微信号
            //FromUserName	 发送方帐号（一个OpenID）
            //CreateTime	 消息创建时间 （整型）
            //MsgType	 text
            //Content	 文本消息内容
            //MsgId	 消息id，64位整型
            XmlNode xmlContent = xmldoc.SelectSingleNode("/xml/Content");
            if (xmlContent == null) throw new ArgumentNullException("Content");
            _content = xmlContent.InnerText;
        }

        /// <summary>
        /// 被动回复
        /// </summary>
        /// <returns></returns>
        public override string ReturnXmlString()
        {
            //LogHelper.WriteDebug("textmsg方法被调用content:" + _content);
            if (string.IsNullOrEmpty(_content)) return "";

            if (_content == "0") //回复0，转人工
            {
                return string.Format(CommonData.Message_ToServiceCustom, FromUserName, ToUserName);
            }
            if (_content.StartsWith("音乐 "))
            {
                return MsgCreateHelper.CreateMusicReplyString(this, "一路上有你", "张学友 一路上有你", "http://wechat-test.meteni.com/music/一路上有你.mp3", "http://wechat-test.meteni.com/music/一路上有你.mp3", "Rvmj0ZAN_Frq02BuFlHK4Q7Jobk_JNEo_uc0B7OrwESrtz3-OEnUwiF6Z5it4PO7");
            }

            if (_content.StartsWith("图文 "))
            {
                //回复图文格式
                List<Article> listArticles=new List<Article>();
                Article a=new Article();
                a.Url =@"http://mp.weixin.qq.com/s?__biz=MzA3NTQxMjQyMA==&mid=203846970&idx=2&sn=8c2f94c2666c998995c8414c5e5add99&scene=1&key=8ea74966bf01cfb64584d24d0533d93d751edd1b35af2fb7a17fe2bdf487b407f9ac6fe4f2482c1ff70895d5f854f6c5&ascene=1&uin=MTg3OTIzMDU%3D&devicetype=Windows+7&version=6100071e&pass_ticket=3d6j%2B4Oxam6VZFGxH2iwQj7xhUGwTEwjiLl5QhiHTFM%3D";
                a.Description = "【3分钟教你在餐厅book a table！】生活英语——来自美联英语微杂志。";
                a.Title = "【3分钟教你在餐厅book a table！】";
                a.PicUrl = "http://wechat-test.meteni.com/img/456089862766251222.jpg";
                listArticles.Add(a);

                return MsgCreateHelper.CreateNewsReplyString(this,listArticles);
            }
            return MsgCreateHelper.CreateTextReplyString(this, "美联英语内部服务欢迎您，您输入的内容为：" + _content);
        }

    }
}
