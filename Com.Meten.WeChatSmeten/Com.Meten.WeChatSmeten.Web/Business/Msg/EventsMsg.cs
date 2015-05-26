using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;
using Com.Meten.WeChatSmeten.Helper;
using Com.Meten.WeChatSmeten.Web.Data;

namespace Com.Meten.WeChatSmeten.Web.Business
{
    /// <summary>
    /// 事件消息
    /// </summary>
    public class EventsMsg:BaseMsg
    {
        private readonly string  _event;
        private readonly string  _eventKey;
        
        #region 扫条码时应有的字段
        private readonly string  _scanType;
        private readonly string  _scanResult;
        #endregion

        #region 发图时应有的字段
        private readonly string _pictureCount;
        private readonly List<string> _picMd5Sums = null;
        #endregion

        #region 菜单选择地理位置专用
        private readonly string _locationX;
        private readonly string _locationY;
        private readonly string _scale;
        private readonly string _label;
        private readonly string _poiname;
        #endregion

        #region 扫描专用
        /// <summary>
        ///  //扫描带参数二维码进来时带有该参数，该值用来请求微信获取二维码图片
        /// </summary>
        private readonly string _ticket;
        #endregion

        #region 自动上报地理位置专用
        /// <summary>
        /// 地理位置纬度
        /// </summary>
        private readonly string _latitude;
        /// <summary>
        /// 地理位置经度
        /// </summary>
        private readonly string _longitude;
        /// <summary>
        /// 地理位置精度
        /// </summary>
        private readonly string _precision;
        #endregion

        public EventsMsg(XmlDocument xmldoc) : base(xmldoc)
        {
            XmlNode xmlEvent = xmldoc.SelectSingleNode("/xml/Event");
            if (xmlEvent == null)
                throw new ArgumentNullException("Event");
            _event = xmlEvent.InnerText;
            XmlNode xmlEventKey = xmldoc.SelectSingleNode("/xml/EventKey");
            if (xmlEventKey != null)
            {
                _eventKey = xmlEventKey.InnerText;
            }
            XmlNode xmlScanType = xmldoc.SelectSingleNode("/xml/ScanCodeInfo/ScanType");
            if (xmlScanType != null)
            {
                _scanType = xmlScanType.InnerText;
            }

            XmlNode xmlScanResult = xmldoc.SelectSingleNode("/xml/ScanCodeInfo/ScanResult");
            if (xmlScanResult != null)
            {
                _scanResult = xmlScanResult.InnerText;
            }

            XmlNode xmlPictureCount = xmldoc.SelectSingleNode("/xml/SendPicsInfo/Count");
            if (xmlPictureCount != null)
            {
                _pictureCount = xmlPictureCount.InnerText;
            }
            XmlNodeList xmlPicMd5Sums = xmldoc.SelectNodes("/xml/SendPicsInfo/PicList/item/PicMd5Sum");
            if (xmlPicMd5Sums != null)
            {
                foreach (XmlNode picMd5Sum in xmlPicMd5Sums)
                {
                    _picMd5Sums.Add(picMd5Sum.InnerText);
                }
            }

            XmlNode xmlLocationX = xmldoc.SelectSingleNode("/xml/SendLocationInfo/Location_X");
            if (xmlLocationX != null)
            {
                _locationX = xmlLocationX.InnerText;
            }
            XmlNode xmlLocationY = xmldoc.SelectSingleNode("/xml/SendLocationInfo/Location_Y");
            if (xmlLocationY != null)
            {
                _locationY = xmlLocationY.InnerText;
            }

            XmlNode xmlScale = xmldoc.SelectSingleNode("/xml/SendLocationInfo/Scale");
            if (xmlScale != null)
            {
                _scale = xmlScale.InnerText;
            }

            XmlNode xmlLabel = xmldoc.SelectSingleNode("/xml/SendLocationInfo/Label");
            if (xmlLabel != null)
            {
                _label = xmlLabel.InnerText;
            }
            XmlNode xmlPoiname = xmldoc.SelectSingleNode("/xml/SendLocationInfo/Poiname");
            if (xmlPoiname != null)
            {
                _poiname = xmlPoiname.InnerText;
            }

            XmlNode Ticket = xmldoc.SelectSingleNode("/xml/Ticket");
            if (Ticket != null)
            {
                _ticket = Ticket.InnerText;
            }
            XmlNode Latitude = xmldoc.SelectSingleNode("/xml/Latitude");
            if (Latitude != null)
            {
                _latitude = Latitude.InnerText;
            }

            XmlNode Longitude = xmldoc.SelectSingleNode("/xml/Longitude");
            if (Longitude != null)
            {
                _longitude = Longitude.InnerText;
            }
            XmlNode Precision = xmldoc.SelectSingleNode("/xml/Precision");
            if (Precision != null)
            {
                _precision = Precision.InnerText;
            }
        }


        public override string ReturnXmlString()
        {
            string replyContent = "";
            if (string.IsNullOrEmpty(_event)) return replyContent;
            string replayText = "";
            LogHelper.WriteInfo("事件：" + _event + "。");
            switch (_event.ToUpper())
            {
                    //菜单单击事件
                case "CLICK":
                    replayText = string.Format("您单击了{0}菜单", _eventKey);
                    switch (_eventKey.ToUpper())
                    {
                            //todo  单击按钮来自哪里？？？   新增菜单则在这里增加case即可。。
                        case "key1":
                            break;
                        case "key2":
                            break;
                        case "key3":
                            break;
                            //.....
                            //....
                        default:
                            //LogHelper.WriteInfo("CASE CLICK" + Event.InnerText.ToUpper() + "；Reply:" + replayText);
                            replayText = string.Format("收到单击菜单，key值为：{0}。", _eventKey);
                            break;
                    }
                    break;
                    //跳转
                case "VIEW":
                    replayText = string.Format("您单击了跳转链接，跳转链接地址为：{0}", _eventKey);
                    break;
                    //扫码推事件的事件推送
                case "SCANCODE_PUSH":
                    //replayText = string.Format("您单击了扫码推事件的菜单，对应菜单的key值为：{0}；扫描的条码类型：{1},条码信息:{2}", _eventKey, _scanType, _scanResult);
                    replayText = string.Format("扫码推事件的菜单，key值为：{0}；扫描的条码类型：{1},条码信息:{2}", _eventKey, _scanType, _scanResult);
                    break;
                    //扫码推事件且弹出“消息接收中”提示框的事件推送
                case "SCANCODE_WAITMSG":
                    replayText = string.Format("扫码推事件且弹出“消息接收中”提示框的菜单，key值为：{0}；扫描的条码类型：{1},条码信息:{2}", _eventKey, _scanType, _scanResult);
                    break;
                    //弹出系统拍照发图的事件推送
                case "PIC_SYSPHOTO":
                    //弹出拍照或者相册发图的事件推送
                case "PIC_PHOTO_OR_ALBUM":
                    //弹出微信相册发图器的事件推送
                case "PIC_WEIXIN":
                    string eventName = "系统拍照发图";
                    if (_event.ToUpper() == "PIC_PHOTO_OR_ALBUM")
                        eventName = "拍照或者相册发图";
                    if (_event.ToUpper() == "PIC_WEIXIN")
                        eventName = "微信相册发图器";
                    replayText = string.Format("您单击了发图功能菜单{0}；并发送了{1}张图片。您的发图入口为:{2}。他们的MD5值如下：{3}。", _eventKey, _pictureCount, eventName, string.Join(",", _picMd5Sums));
                    break;
                    //发送地理位置
                case "LOCATION_SELECT":
                    replayText = string.Format("您单击了发送地理位置的{0}菜单；您发送的地理位置信息为：经度{1},维度{2},比例{3},具体位置:{4},朋友圈POI信息{5}",  _eventKey,_locationX,_locationY,_scale,_label,_poiname);
                    break;
                    //关注
                case "SUBSCRIBE":
                    replayText = string.Format("尊敬的{0}用户；欢迎关注！",UserHelper.GetUserInfo(FromUserName).nickname);
                    break;
                    //取消关注
                case "UNSUBSCRIBE":
                    replayText = string.Format("用户{0}取消关注！", UserHelper.GetUserInfo(FromUserName).nickname);
                    //replayText = "取消关注";
                    break;
                    //扫描
                case "SCAN":
                    //事件KEY值(_eventKey)，是一个32位无符号整数，即创建二维码时的二维码scene_id
                    //扫描带参数二维码进来时带有该参数，该值用来请求微信获取二维码图片
                    replayText = string.Format("扫描进来；Ticket为：{0}；scene_id为：{1}。",_ticket,_eventKey);
                    break;
                    //上报用户地理位置信息
                case "LOCATION":
                    replayText = string.Format("您的地理位置信息；精度：{0}、维度：{1}、精度：{2}。",_latitude,_longitude,_precision);
                    //上报地理位置不回复
                    return "";
                    break;
                default:
                    break;
            }
            LogHelper.WriteInfo("事件：" + _event + "；Reply:" + replayText);
            replyContent = MsgCreateHelper.CreateTextReplyString(this, replayText);
            return replyContent;
        }
    }
}