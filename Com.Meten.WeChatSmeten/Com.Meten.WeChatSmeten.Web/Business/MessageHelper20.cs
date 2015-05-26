using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;
using Com.Meten.WeChatSmeten.Entities;
using Com.Meten.WeChatSmeten.Helper;
using Com.Meten.WeChatSmeten.TencentCrypt;
using Com.Meten.WeChatSmeten.Web.Data;

namespace Com.Meten.WeChatSmeten.Web.Business
{
    public class MessageHelper20
    {
        private static WXBizMsgCrypt wxcpt = null;

        /// <summary>
        /// 返回消息，包含解密、加密
        /// </summary>
        /// <param name="encrypt_type">类型</param>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <param name="postString"></param>
        /// <returns></returns>
        public string ReturnMessage(string encrypt_type, string msg_signature, string timestamp, string nonce,
            string postString)
        {

            try
            {
                string resultStr = "";
                if (wxcpt == null)
                    wxcpt = new WXBizMsgCrypt(CommonData.WeixinToken, CommonData.EncodingAESKey, CommonData.AppID);
                int flag = 0;
                if (encrypt_type.ToLower() == "aes")
                {
                    //解密
                    flag = wxcpt.DecryptMsg(msg_signature, timestamp, nonce, postString, ref postString);
                    if (flag != 0)
                        throw new Exception("接收时解密消息发生错误，错误码为" + flag);
                    //LogHelper.WriteInfo(string.Format("接收时解密后的消息为：{0}",postString));
                    //end 解密
                }

                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(new System.IO.MemoryStream(System.Text.Encoding.GetEncoding("UTF-8").GetBytes(postString)));
                BaseMsg imsg = new BaseMsg(xmldoc);
                //LogHelper.WriteInfo("类型为：" + imsg.MsgType);
                switch (imsg.MsgType)
                {
                    case MessageType.events:
                        imsg = new EventsMsg(xmldoc);
                        break;
                    case MessageType.text:
                        imsg = new TextMsg(xmldoc);
                        break;
                    case MessageType.image:
                        imsg=new ImageMsg(xmldoc);
                        break;
                    case MessageType.voice:
                        imsg = new VoiceMsg(xmldoc);
                        break;
                    case MessageType.video:
                        imsg = new VideoMsg(xmldoc);
                        break;
                    case MessageType.location:
                        imsg=new LocationMsg(xmldoc);
                        break;
                    case MessageType.link:
                        imsg=new LinkMsg(xmldoc);
                        break;
                    default:
                        break;
                }
                resultStr = imsg.ReturnXmlString();
                if (string.IsNullOrEmpty(resultStr))
                    return "";
                LogHelper.WriteInfo(string.Format("ReturnMessage得到的消息为（加密前）：{0}", resultStr)); 
                if (encrypt_type.ToLower() == "aes")
                {
                    //加密
                    //LogHelper.WriteInfo(string.Format("回复时加密前的消息为：{0}", resultStr));
                    flag = wxcpt.EncryptMsg(resultStr, timestamp, nonce, ref resultStr);
                    if (flag != 0)
                        throw new Exception("回复时加密消息发生错误，错误码为" + flag);
                    //end 加密
                }
                LogHelper.WriteInfo(string.Format("ReturnMessage得到的消息为（加密后）：{0}", resultStr));
                return resultStr;
            }
            catch (Exception exception)
            {
                LogHelper.WriteFatal("处理消息出现错误；错误信息为：" + exception.Message);
                return "";
            }
        }
    }
}