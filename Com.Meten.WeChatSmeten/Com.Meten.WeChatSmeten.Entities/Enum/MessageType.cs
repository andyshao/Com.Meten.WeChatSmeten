using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Meten.WeChatSmeten.Entities
{
    /// <summary>
    /// 消息类型
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// 文本
        /// </summary>
        text,

        /// <summary>
        /// 图片
        /// </summary>
        image,

        /// <summary>
        /// 语音
        /// </summary>
        voice,

        /// <summary>
        /// 视频
        /// </summary>
        video,

        /// <summary>
        /// 地理位置
        /// </summary>
        location,

        /// <summary>
        /// 链接
        /// </summary>
        link,
        /// <summary>
        /// 事件
        /// </summary>
        events,
        /// <summary>
        /// 音乐
        /// </summary>
        music,

        /// <summary>
        /// 图文
        /// </summary>
        news
    }
}
