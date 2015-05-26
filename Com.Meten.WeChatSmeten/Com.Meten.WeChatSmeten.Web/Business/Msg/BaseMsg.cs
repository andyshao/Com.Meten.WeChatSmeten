using System;
using System.Xml;
using Com.Meten.WeChatSmeten.Entities;
using Com.Meten.WeChatSmeten.Helper;

namespace Com.Meten.WeChatSmeten.Web.Business
{
    public  class BaseMsg 
    {
        private readonly string _toUserName;
        private readonly string _fromUserName;
        private readonly string _createTime;
        private readonly string _msgType;
        private readonly string _msgId;
        private readonly User _user;
        /// <summary>
        /// 接收者
        /// </summary>
        public string ToUserName
        {
            get { return _toUserName; }
        }

        /// <summary>
        /// 发送者
        /// </summary>
        public string FromUserName
        {
            get { return _fromUserName; }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime
        {
            get { return _createTime; }
        }

        /// <summary>
        /// 消息类型
        /// </summary>
        public MessageType MsgType
        {
            get
            {
                MessageType mt = MessageType.text;
                foreach (int i in Enum.GetValues(typeof (MessageType)))
                {
                    if (Enum.GetName(typeof (MessageType), i) == _msgType)
                    {
                        mt= (MessageType)Convert.ToInt32(i.ToString());
                        break;
                    }
                }
                return mt;
            }
        }

        /// <summary>
        /// 消息ID
        /// </summary>
        public string MsgId
        {
            get { return _msgId; }
        }

        public User FromUser
        {
            get { return _user; }
            
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="node"></param>
        public BaseMsg(XmlDocument xmldoc)
        {
            XmlNode xmlToUserName = xmldoc.SelectSingleNode("/xml/ToUserName");
            if (xmlToUserName == null) throw new ArgumentNullException("toUserName");
            XmlNode xmlFromUserName = xmldoc.SelectSingleNode("/xml/FromUserName");
            if (xmlFromUserName == null) throw new ArgumentNullException("fromUserName");
            XmlNode xmlMsgType = xmldoc.SelectSingleNode("/xml/MsgType");
            if (xmlMsgType == null) throw new ArgumentNullException("MsgType");
            XmlNode xmlCreateTime = xmldoc.SelectSingleNode("/xml/CreateTime");
            if (xmlCreateTime == null) throw new ArgumentNullException("CreateTime");
            XmlNode xmlMsgId = xmldoc.SelectSingleNode("/xml/MsgId");
            _toUserName = xmlToUserName.InnerText;
            _fromUserName = xmlFromUserName.InnerText;
            _createTime = xmlCreateTime.InnerText;
            _msgType = xmlMsgType.InnerText;
            if (xmlMsgId != null)
                _msgId = xmlMsgId.InnerText;
            if (_msgType == "event") _msgType = "events";
            _user = UserHelper.GetUserInfo(FromUserName);
        }


        #region  需要被覆盖以实现多态的函数
        /// <summary>
        /// 被动回复消息返回串。。。
        /// </summary>
        /// <returns></returns>
        public virtual string ReturnXmlString()
        {
            LogHelper.WriteDebug("basemsg的ReturnXmlString方法被调用");
            return "";
        }
        #endregion
    }
}
