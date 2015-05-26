using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Meten.WeChatSmeten.Entities
{
    /// <summary>
    /// 客服
    /// </summary>
    public class CustomService
    {
        /// <summary>
        /// 完整客服账号，格式为：账号前缀@公众号微信号
        /// </summary>
        public string kf_account { set; get; }
        /// <summary>
        /// 客服昵称
        /// </summary>
        public string kf_nick { set; get; }
        /// <summary>
        /// 客服工号
        /// </summary>
        public string kf_id { set; get; }
        /// <summary>
        /// 客服昵称，最长6个汉字或12个英文字符
        /// </summary>
        public string nickname { set; get; }
        /// <summary>
        /// 客服昵称，最长6个汉字或12个英文字符
        /// </summary>
        public string password { set; get; }
        /// <summary>
        /// 客服账号登录密码，格式为密码明文的32位加密MD5值。该密码仅用于在公众平台官网的多客服功能中使用，若不使用多客服功能，则不必设置密码
        /// </summary>
        public string Password { set; get; }
        /// <summary>
        /// 该参数仅在设置客服头像时出现，是form-data中媒体文件标识，有filename、filelength、content-type等信息
        /// </summary>
        public string media { set; get; }
    }
}
