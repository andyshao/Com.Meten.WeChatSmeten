using System;
using System.Web.UI.WebControls;
using System.Xml;
using Com.Meten.WeChatSmeten.Entities;
using Com.Meten.WeChatSmeten.Helper;
using Com.Meten.WeChatSmeten.TencentCrypt;
using Com.Meten.WeChatSmeten.Web.Data;

namespace Com.Meten.WeChatSmeten.Web.Business
{
    /// <summary>
    /// 接受/发送消息帮助类
    /// </summary>
    public class MessageHelper
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
            resultStr = ReturnMessage(postString);
            //LogHelper.WriteInfo(string.Format("ReturnMessage得到的消息为：{0}", resultStr)); 
            if (encrypt_type.ToLower() == "aes")
            {
                //加密
                //LogHelper.WriteInfo(string.Format("回复时加密前的消息为：{0}", resultStr));
                flag = wxcpt.EncryptMsg(resultStr, timestamp, nonce, ref resultStr);
                if (flag != 0)
                    throw new Exception("回复时加密消息发生错误，错误码为" + flag);
                //end 加密
            }
            return resultStr;
        }


        /// <summary>
        ///返回消息 
        /// </summary>
        /// <param name="postStr"></param>
        /// <returns></returns>
        public string ReturnMessage(string postStr)
        {
            try
            {
                string replyContent = "";
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(new System.IO.MemoryStream(System.Text.Encoding.GetEncoding("UTF-8").GetBytes(postStr)));
                XmlNode MsgType = xmldoc.SelectSingleNode("/xml/MsgType");
                if (MsgType.InnerText.ToLower() != "event")
                    LogHelper.WriteInfo(string.Format("ReturnMessage函数记录.收到微信的信息为：{0}.", xmldoc.OuterXml));
                if (MsgType != null)
                {
                    switch (MsgType.InnerText.ToLower())
                    {
                        case "event": //事件
                            replyContent = EventHandle(xmldoc);
                            break;
                        case "text": //文本
                            replyContent = TextHandle(xmldoc);
                            break;
                        case "voice": //语音
                            replyContent = VoiceHandle(xmldoc);
                            break;
                        case "image": //图片
                            replyContent = PictureHandle(xmldoc);
                            break;
                        case "video": //视频
                            replyContent = VideoHandle(xmldoc);
                            break;
                        case "location": //地理位置
                            replyContent = LocationHandle(xmldoc);
                            break;
                        case "link": //链接消息
                            replyContent = LinkHandle(xmldoc);
                            break;
                        default:
                            break;
                    }
                }
                if (MsgType.InnerText.ToLower() != "event")
                    LogHelper.WriteInfo(string.Format("ReturnMessage函数记录.回复为：{0}.", replyContent));
                return replyContent;
            }
            catch (Exception exception)
            {
                LogHelper.WriteFatal("处理消息出现错误");
                LogHelper.WriteFatal(exception);
                return "";
            }
        }

        /// <summary>
        /// 事件（包含自定义菜单接收到的点击等）
        /// </summary>
        /// <param name="xmldoc"></param>
        /// <returns></returns>
        private string EventHandle(XmlDocument xmldoc)
        {
            string replyContent = "";
            XmlNode Event = xmldoc.SelectSingleNode("/xml/Event");
            XmlNode EventKey = null;
            XmlNode ToUserName = xmldoc.SelectSingleNode("/xml/ToUserName");
            XmlNode FromUserName = xmldoc.SelectSingleNode("/xml/FromUserName");
            XmlNode CreateTime = xmldoc.SelectSingleNode("/xml/CreateTime");
            XmlNode ScanType = null;
            XmlNode ScanResult = null;
            XmlNode PictureCount = null;
            XmlNodeList PicMd5Sums =null;
            if (Event != null)
            {
                string replayText = "";
                //菜单单击事件
                switch (Event.InnerText.ToUpper())
                {
                    //单击
                    case "CLICK":
                        EventKey = xmldoc.SelectSingleNode("/xml/EventKey");
                        replayText = string.Format("您单击了{0}菜单", EventKey == null ? "" : EventKey.InnerText);
                        switch (EventKey.InnerText.ToUpper())
                        {
                            //todo  单击按钮来自哪里？？？   新增菜单则在这里增加case即可。。
                            case "key1":
                                break;
                            case "key2":
                                break;
                            default:
                                //LogHelper.WriteInfo("CASE CLICK" + Event.InnerText.ToUpper() + "；Reply:" + replayText);
                                replayText = string.Format("收到单击菜单，key值为：{0}。", EventKey.InnerText);
                                break;
                        }
                        break;
                    //跳转
                    case "VIEW":
                        EventKey = xmldoc.SelectSingleNode("/xml/EventKey");
                        replayText = string.Format("您单击了跳转链接，跳转链接地址为：{0}", EventKey == null ? "" : EventKey.InnerText);
                        break;
                    //扫码推事件的事件推送
                    case "SCANCODE_PUSH":
                        EventKey = xmldoc.SelectSingleNode("/xml/EventKey");
                         ScanType = xmldoc.SelectSingleNode("/xml/ScanCodeInfo/ScanType");
                         ScanResult = xmldoc.SelectSingleNode("/xml/ScanCodeInfo/ScanResult");
                        replayText = string.Format("您单击了扫码菜单，对应菜单的key值为：{0}；扫描的条码类型：{1},条码信息:{2}", EventKey == null ? "" : EventKey.InnerText, ScanType == null ? "" : ScanType.InnerText, ScanResult == null ? "" : ScanResult.InnerText);
                        break;
                    //扫码推事件且弹出“消息接收中”提示框的事件推送
                    case "SCANCODE_WAITMSG":
                        EventKey = xmldoc.SelectSingleNode("/xml/EventKey");
                         ScanType = xmldoc.SelectSingleNode("/xml/ScanCodeInfo/ScanType");
                         ScanResult = xmldoc.SelectSingleNode("/xml/ScanCodeInfo/ScanResult");
                         replayText = string.Format("您单击了扫码且弹出“消息接收中”菜单，对应菜单的key值为：{0}；扫描的条码类型：{1},条码信息:{2}", EventKey == null ? "" : EventKey.InnerText, ScanType == null ? "" : ScanType.InnerText, ScanResult == null ? "" : ScanResult.InnerText);
                        break;
                    //弹出系统拍照发图的事件推送
                    case "PIC_SYSPHOTO":
                    //弹出拍照或者相册发图的事件推送
                    case "PIC_PHOTO_OR_ALBUM":
                    //弹出微信相册发图器的事件推送
                    case "PIC_WEIXIN":
                        string eventName = "系统拍照发图";
                        if (Event.InnerText.ToUpper() == "PIC_PHOTO_OR_ALBUM")
                            eventName = "拍照或者相册发图";
                        if (Event.InnerText.ToUpper() == "PIC_WEIXIN")
                            eventName = "微信相册发图器";
                        EventKey = xmldoc.SelectSingleNode("/xml/EventKey");
                        PictureCount = xmldoc.SelectSingleNode("/xml/SendPicsInfo/Count");
                        PicMd5Sums = xmldoc.SelectNodes("/xml/SendPicsInfo/PicList/item/PicMd5Sum");
                        replayText = string.Format("您单击了发图功能菜单{0}；并发送了{1}张图片。您的发图入口为:{2}。他们的MD5值如下：",
                            EventKey == null ? "" : EventKey.InnerText,
                            PictureCount == null ? "" : PictureCount.InnerText, eventName);
                        foreach (XmlNode picMd5Sum in PicMd5Sums)
                        {
                            replayText += picMd5Sum.InnerText + "；";
                        }
                        break;
                    //发送地理位置
                    case "LOCATION_SELECT":
                        EventKey = xmldoc.SelectSingleNode("/xml/EventKey");
                        XmlNode Location_X = xmldoc.SelectSingleNode("/xml/SendLocationInfo/Location_X");
                        XmlNode Location_Y = xmldoc.SelectSingleNode("/xml/SendLocationInfo/Location_Y");
                        XmlNode Scale = xmldoc.SelectSingleNode("/xml/SendLocationInfo/Scale");
                        XmlNode Label = xmldoc.SelectSingleNode("/xml/SendLocationInfo/Label");
                        XmlNode Poiname = xmldoc.SelectSingleNode("/xml/SendLocationInfo/Poiname");
                        replayText = string.Format("您单击了发送地理位置的{0}菜单；您发送的地理位置信息为：经度{1},维度{2},比例{3},具体位置:{4},朋友圈POI信息{5}", EventKey == null ? "" : EventKey.InnerText, Location_X == null ? "" : Location_X.InnerText, Location_Y == null ? "" : Location_Y.InnerText, Scale == null ? "" : Scale.InnerText, Label == null ? "" : Label.InnerText, Poiname == null ? "" : Poiname.InnerText);
                        break;
                        //关注
                    case "SUBSCRIBE":
                        replayText = "欢迎关注";
                        break;
                        //取消关注
                    case "UNSUBSCRIBE":
                        replayText = "取消关注";
                        break;
                        //扫描
                    case "SCAN":
                        EventKey = xmldoc.SelectSingleNode("/xml/EventKey"); //事件KEY值，是一个32位无符号整数，即创建二维码时的二维码scene_id
                        //扫描带参数二维码进来时带有该参数，该值用来请求维信获取二维码图片
                        XmlNode Ticket = xmldoc.SelectSingleNode("/xml/Ticket");
                        replayText = "扫描进来；Ticket为：" + Ticket.InnerText + "scene_id为：" + EventKey.Value;
                        break;
                        //上报用户地理位置信息
                    case "LOCATION":
                        //Latitude	 地理位置纬度
                        //Longitude	 地理位置经度
                        //Precision	 地理位置精度
                        XmlNode Latitude = xmldoc.SelectSingleNode("/xml/Latitude");
                        XmlNode Longitude = xmldoc.SelectSingleNode("/xml/Longitude");
                        XmlNode Precision = xmldoc.SelectSingleNode("/xml/Precision");

                        replayText = string.Format("您的地理位置信息；精度：{0}、维度：{1}、精度：{2}", Latitude.InnerText,
                            Longitude.InnerText, Precision.InnerText);
                        //上报地理位置不回复
                        return "";
                        break;
                    default:
                        break;
                }
                //LogHelper.WriteInfo("事件：" + Event.InnerText.ToUpper() + "；Reply:" + replayText);
                if (!string.IsNullOrEmpty(replayText))
                {
                    replyContent = string.Format(CommonData.MessageText,
                        FromUserName == null ? "" : FromUserName.InnerText,
                        ToUserName == null ? "" : ToUserName.InnerText,
                        CreateTime == null ? DateTime.Now.Ticks.ToString() : CreateTime.InnerText,
                        replayText);
                }
            }
            return replyContent;
        }

        /// <summary>
        /// 接收文本消息
        /// </summary>
        /// <param name="xmldoc"></param>
        /// <returns></returns>
        private string TextHandle(XmlDocument xmldoc)
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
            string replyContent = "";
            XmlNode toUserName = xmldoc.SelectSingleNode("/xml/ToUserName");
            XmlNode fromUserName = xmldoc.SelectSingleNode("/xml/FromUserName");
            XmlNode content = xmldoc.SelectSingleNode("/xml/Content");
            if (content != null)
            {
                if (content.InnerText.Trim() == "0") //回复0，转人工
                {
                    replyContent = string.Format(CommonData.Message_ToServiceCustom,
                        fromUserName.InnerText,
                        toUserName.InnerText,
                        DateTime.Now.Ticks);
                }
                else
                {
                    replyContent = string.Format(CommonData.MessageText,
                        fromUserName.InnerText,
                        toUserName.InnerText,
                        DateTime.Now.Ticks,
                        string.Format("您说的“{0}”；我已经收到，暂时无法处理;回复0转人工。", content.InnerText));
                    //"美联英语内部服务欢迎您，您输入的内容为：" + content.InnerText);
                }
            }
            return replyContent;
        }

        /// <summary>
        /// 接收语音消息
        /// </summary>
        /// <param name="xmldoc"></param>
        /// <returns></returns>
        private string VoiceHandle(XmlDocument xmldoc)
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
            string replyContent = "";
            XmlNode toUserName = xmldoc.SelectSingleNode("/xml/ToUserName");
            XmlNode fromUserName = xmldoc.SelectSingleNode("/xml/FromUserName");
            XmlNode media_id = xmldoc.SelectSingleNode("/xml/MediaId");
            XmlNode Format = xmldoc.SelectSingleNode("/xml/Format");
            XmlNode Recognition = xmldoc.SelectSingleNode("/xml/Recognition");
            XmlNode MsgId = xmldoc.SelectSingleNode("/xml/MsgId");
            if (media_id != null)
            {
                //LogHelper.WriteInfo(string.Format("语音消息；fromUserName:{0};media_id:{1};Format(格式):{2};Recognition(翻译):{3};MsgID:{4};",fromUserName.InnerText, media_id.InnerText, Format.InnerText, Recognition.InnerText,MsgId.InnerText));
                replyContent = string.Format(CommonData.MessageVoice,fromUserName.InnerText,toUserName.InnerText,DateTime.Now.Ticks,media_id.InnerText);
            }
            return replyContent;
        }

        /// <summary>
        /// 接受图片消息
        /// </summary>
        /// <param name="xmldoc"></param>
        /// <returns></returns>
        private string PictureHandle(XmlDocument xmldoc)
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
            string replyContent = "";
            XmlNode toUserName = xmldoc.SelectSingleNode("/xml/ToUserName");
            XmlNode fromUserName = xmldoc.SelectSingleNode("/xml/FromUserName");
            XmlNode PicUrl = xmldoc.SelectSingleNode("/xml/PicUrl");
            XmlNode media_id = xmldoc.SelectSingleNode("/xml/MediaId");
            XmlNode MsgId = xmldoc.SelectSingleNode("/xml/MsgId");
            if (media_id != null)
            {
                //LogHelper.WriteInfo(string.Format("图片消息；fromUserName:{0};media_id:{1};PicUrl:{2};MsgID:{3};",        fromUserName.InnerText, media_id.InnerText, PicUrl.InnerText,MsgId.InnerText));
                replyContent = string.Format(CommonData.MessagePicture,fromUserName.InnerText,toUserName.InnerText,DateTime.Now.Ticks,media_id.InnerText);
            }
            return replyContent;
        }

        /// <summary>
        /// 接受视频消息
        /// </summary>
        /// <param name="xmldoc"></param>
        /// <returns></returns>
        private string VideoHandle(XmlDocument xmldoc)
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
            string replyContent = "";
            XmlNode toUserName = xmldoc.SelectSingleNode("/xml/ToUserName");
            XmlNode fromUserName = xmldoc.SelectSingleNode("/xml/FromUserName");
            XmlNode media_id = xmldoc.SelectSingleNode("/xml/MediaId");
            XmlNode ThumbMediaId = xmldoc.SelectSingleNode("/xml/ThumbMediaId");
            XmlNode MsgId = xmldoc.SelectSingleNode("/xml/MsgId");
            //User u = UserHelper.GetUserInfo(fromUserName.InnerText);
            if (media_id != null)
            {
                //LogHelper.WriteInfo(string.Format("视频消息；fromUserName:{0};media_id:{1};ThumbMediaId:{2};MsgID:{3};",fromUserName.InnerText, media_id.InnerText, ThumbMediaId.InnerText, MsgId.InnerText));
                //replyContent = string.Format(CommonData.MessageVideo, fromUserName.InnerText, toUserName.InnerText, DateTime.Now.Ticks, media_id.InnerText, string.Format("{0}发来的视频。", u.nickname), string.Format("{0}发来的视频；视频消息缩略图的媒体id为{1}。", u.nickname, ThumbMediaId.InnerText));
                replyContent = string.Format(CommonData.MessageText,fromUserName.InnerText,toUserName.InnerText,DateTime.Now.Ticks,"您所发的视频已经收到，暂时无法处理;回复0转人工。");
            }
            return replyContent;
        }


        /// <summary>
        /// 处理地理位置消息
        /// </summary>
        /// <param name="xmldoc"></param>
        /// <returns></returns>
        private string LocationHandle(XmlDocument xmldoc)
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
            string replyContent = "";
            XmlNode toUserName = xmldoc.SelectSingleNode("/xml/ToUserName");
            XmlNode fromUserName = xmldoc.SelectSingleNode("/xml/FromUserName");
            XmlNode Location_X = xmldoc.SelectSingleNode("/xml/Location_X");
            XmlNode Location_Y = xmldoc.SelectSingleNode("/xml/Location_Y");
            XmlNode Scale = xmldoc.SelectSingleNode("/xml/Scale");
            XmlNode Label = xmldoc.SelectSingleNode("/xml/Label");
            XmlNode MsgId = xmldoc.SelectSingleNode("/xml/MsgId");
            User u = UserHelper.GetUserInfo(fromUserName.InnerText);
            if (Location_X != null)
            {
                //LogHelper.WriteInfo(string.Format("地理消息；fromUserName:{0};经度:{1};纬度:{2};比例:{3};位置标识:{4};MsgID:{5};",fromUserName.InnerText, Location_X.InnerText, Location_Y.InnerText,Scale.InnerText,Label.InnerText, MsgId.InnerText));
                replyContent = string.Format(CommonData.MessageText,fromUserName.InnerText,toUserName.InnerText,DateTime.Now.Ticks,string.Format("您发送了地理位置信息已经收到。经度:{0};纬度:{1};比例:{2};位置标识:{3}", Location_X.InnerText, Location_Y.InnerText, Scale.InnerText, Label.InnerText));
            }
            return replyContent;
        }


        /// <summary>
        /// 处理链接信息
        /// </summary>
        /// <param name="xmldoc"></param>
        /// <returns></returns>
        private string LinkHandle(XmlDocument xmldoc)
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
            return "";

        }




        /// <summary>
        /// 转向客服，并指定客服
        /// </summary>
        /// <param name="xmldoc"></param>
        /// <param name="toCustomService"></param>
        /// <returns></returns>
        private string GoCustomDetail(XmlDocument xmldoc,string toCustomService)
        {
            string replyContent = "";
            XmlNode toUserName = xmldoc.SelectSingleNode("/xml/ToUserName");
            XmlNode fromUserName = xmldoc.SelectSingleNode("/xml/FromUserName");
            XmlNode content = xmldoc.SelectSingleNode("/xml/Content");
            if (content != null)
            {
                replyContent = string.Format(CommonData.Message_ToServiceCustonDetail, fromUserName.InnerText,
                    toUserName.InnerText, DateTime.Now.Ticks, toCustomService);
            }
            return replyContent;
        }
    }
}