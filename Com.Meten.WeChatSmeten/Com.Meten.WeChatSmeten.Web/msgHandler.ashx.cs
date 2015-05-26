using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using Com.Meten.WeChatSmeten.Helper;
using Com.Meten.WeChatSmeten.Web.Business;
using Com.Meten.WeChatSmeten.Web.Data;

namespace Com.Meten.WeChatSmeten.Web
{
    /// <summary>
    /// msgHandler 的摘要说明
    /// </summary>
    public class msgHandler : IHttpHandler
    {
        string signature = "";
        string timestamp = "";
        string nonce = "";
        string echoString = "";
        /// <summary>
        ///encrypt_type的值为raw时表示为不加密，encrypt_type的值为aes时，表示aes加密（暂时只有raw和aes两种值)，无encrypt_type参数同样表示不加密
        /// </summary>
        string encrypt_type = "";
        /// <summary>
        /// 表示对消息体的签名
        /// </summary>
        string msg_signature = "";

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                signature = context.Request.QueryString["signature"];
                timestamp = context.Request.QueryString["timestamp"];
                nonce = context.Request.QueryString["nonce"];
                echoString = context.Request.QueryString["echostr"];
                encrypt_type = context.Request.QueryString["encrypt_type"];
                msg_signature = context.Request.QueryString["msg_signature"];
                //LogHelper.WriteInfo(string.Format("signature is : {0};   ", signature));
                //LogHelper.WriteInfo(string.Format("timestamp is : {0};   ", timestamp));
                //LogHelper.WriteInfo(string.Format("nonce is : {0};   ", nonce));
                //LogHelper.WriteInfo(string.Format("encrypt_type is : {0};   ", encrypt_type));
                //LogHelper.WriteInfo(string.Format("msg_signature is : {0};   ", msg_signature));
                //验证开发者
                if (!string.IsNullOrEmpty(echoString))
                {
                    //LogHelper.WriteInfo(string.Format("echostr is : {0};将进入开发者验证环节.", echoString));
                    ConnectInterface(context);
                }
                else
                {
                    //LogHelper.WriteInfo("无echostr将进入消息处理环节.");
                    //消息处理
                    string postString = "";
                    if (context.Request.HttpMethod.ToUpper() == "POST")
                    {
                        using (Stream stream = context.Request.InputStream)
                        {
                            Byte[] postBytes = new Byte[stream.Length];
                            stream.Read(postBytes, 0, (Int32)stream.Length);
                            postString = Encoding.UTF8.GetString(postBytes);
                            //LogHelper.WriteInfo("收到微信服务器的消息为:" + postString);
                            MessageHelper20 help = new MessageHelper20();
                            string replyContent = help.ReturnMessage(encrypt_type, msg_signature, timestamp, nonce,
                                postString);
                            //LogHelper.WriteInfo("处理后返回用于回送到微信服务器的消息为:" + replyContent);
                            context.Response.Clear();
                            context.Response.ContentEncoding = Encoding.UTF8;
                            context.Response.Write(replyContent);
                            //LogHelper.WriteInfo("消息已回复");
                            context.Response.End();
                        }
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteFatal(ex.Message);
                context.Response.End();
            }
        }




        /// <summary>
        /// 验证Signature 成为开发者url测试，返回echoStr 
        /// </summary>
        public void ConnectInterface(HttpContext context)
        {
            if (string.IsNullOrEmpty(CommonData.WeixinToken))
            {
                throw new ApplicationException("无有效的token");
            }
            if (string.IsNullOrEmpty(signature))
            {
                throw new ApplicationException("缺少参数signature;无法继续验证signature");
            }
            if (string.IsNullOrEmpty(timestamp))
            {
                throw new ApplicationException("缺少参数timestamp;无法继续验证timestamp");
            }
            if (string.IsNullOrEmpty(nonce))
            {
                throw new ApplicationException("缺少参数nonce;无法继续验证nonce");
            }
            string[] ArrTmp = { CommonData.WeixinToken, timestamp, nonce };
            Array.Sort(ArrTmp); //字典排序  
            string tmpStr = string.Join("", ArrTmp);
            LogHelper.WriteInfo(string.Format("tmpStr is : {0};   ", tmpStr));
            tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            tmpStr = tmpStr.ToLower();
            //LogHelper.WriteInfo(string.Format("sha1加密后 tmpStr is : {0};   ", tmpStr));
            if (tmpStr == signature)
            {
                if (!string.IsNullOrEmpty(echoString))
                {
                    //LogHelper.WriteInfo(string.Format("验证signature通过且echoString不为空,正确输出echoString到微信服务器;echoString为 : {0};   ", echoString));
                    context.Response.Clear();
                    context.Response.Write(echoString);
                    context.Response.End();
                }
                else
                {
                    LogHelper.WriteInfo("验证signature通过;但echoString为空不能正确返回!");
                }
            }
            else
            {
                LogHelper.WriteInfo("验证signature不通过;可能不是来自微信服务器的请求!");
            }
        }



        public bool IsReusable
        {
            get { return false; }
        }
    }
}