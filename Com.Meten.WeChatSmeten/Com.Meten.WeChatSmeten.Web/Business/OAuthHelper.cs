using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using Com.Meten.WeChatSmeten.Entities;
using Com.Meten.WeChatSmeten.Helper;
using Com.Meten.WeChatSmeten.Web.Data;

namespace Com.Meten.WeChatSmeten.Web.Business
{
    public class OAuthHelper
    {
        /// <summary>
        /// 根据code获取access token
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static OAuthAccessToken GetAccessTokenByCode(string code)
        {
            try
            {
                string result = HttpHelper.GetHtml(CommonData.GetOauth2AccessTokenUrl(code));
                JavaScriptSerializer js = new JavaScriptSerializer();
                SortedDictionary<string, string> sdResult =
                    js.Deserialize<SortedDictionary<string, string>>(result);
                if (sdResult.ContainsKey("errcode")) //有错误
                {
                    throw new ApplicationException(sdResult.ContainsKey("errcode")
                        ? "获取AccessTonke发生错误；代码为：" + sdResult["errcode"] +
                          (sdResult.ContainsKey("errmsg") ? "；错误描述为：" + sdResult["errmsg"] : "")
                        : "");
                }
                else
                {
                    OAuthAccessToken at=js.Deserialize<OAuthAccessToken>(result);
                     at.access_tokenStartTime = DateTime.Now;
                    return at;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message);
                return new OAuthAccessToken();
            }
        }


        /// <summary>
        /// 根据AccrssTonke获取用户信息
        /// </summary>
        /// <param name="at"></param>
        /// <returns></returns>
        public static User GetUserInfoByAccessToken(OAuthAccessToken at)
        {
            try
            {
                string url = string.Format(CommonData.GetUserInfoByAccessTokenUrl, at.access_token, at.openid);
                LogHelper.WriteFatal(string.Format("获取用户的URL地址:{0}", url));
                string result = HttpHelper.GetHtml(url);
                JavaScriptSerializer js = new JavaScriptSerializer();
                SortedDictionary<string, object> sdResult =
                    js.Deserialize<SortedDictionary<string, object>>(result);
                if (sdResult.ContainsKey("errcode")) //有错误
                {
                    throw new ApplicationException(sdResult.ContainsKey("errcode")
                        ? "根据AccessTonke获取用户信息发生错误；代码为：" + sdResult["errcode"] +
                          (sdResult.ContainsKey("errmsg") ? "；错误描述为：" + sdResult["errmsg"] : "")
                        : "");
                }
                else
                {
                    return js.Deserialize<User>(result);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteFatal(ex.Message);
                return null;
            }
        }

        public static OAuthAccessToken RefreshAccessToken(OAuthAccessToken at)
        {
            try
            {
                if (at == null)
                    throw new Exception("AccessToken为空，无法刷新");
                //不足10分钟时刷新
                //if ((DateTime.Now - at.access_tokenStartTime).Minutes - (Convert.ToInt32(at.expires_in) / 60) < 10)
                if(true)
                {
                    string result = HttpHelper.GetHtml(string.Format(CommonData.GetOauth2RefreshAccessTokenUrl, CommonData.AppID, at.refresh_token));
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    SortedDictionary<string, string> sdResult =
                        js.Deserialize<SortedDictionary<string, string>>(result);
                    if (sdResult.ContainsKey("errcode")) //有错误
                    {
                        throw new ApplicationException(sdResult.ContainsKey("errcode") ? "获取AccessTonke发生错误；代码为：" + sdResult["errcode"] + (sdResult.ContainsKey("errmsg") ? "；错误描述为：" + sdResult["errmsg"] : "") : "");
                    }
                    else
                    {
                        at = js.Deserialize<OAuthAccessToken>(result);
                        at.access_tokenStartTime = DateTime.Now;
                        return at;
                    }
                }
                else
                {
                    return at;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteFatal(ex.Message);
                return null;
            }
        }


        public static bool VerifyAccessToken(OAuthAccessToken at)
        {
            try
            {
                if (at == null)
                    throw new Exception("AccessToken为空，无法验证");
                string url = string.Format(CommonData.VerifyOauth2AccessTokenUrl, at.access_token, at.openid);
                string result = HttpHelper.GetHtml(url);
                JavaScriptSerializer js = new JavaScriptSerializer();
                SortedDictionary<string, string> sdResult =
                    js.Deserialize<SortedDictionary<string, string>>(result);
                if (sdResult.ContainsKey("errcode")) //有错误
                {
                    if (sdResult["errcode"] == "0")
                    {
                        return true;
                    }
                    throw new ApplicationException("AccessTonke不正确；代码为：" + sdResult["errcode"] + "；描述为：" + sdResult["errmsg"] + "；AccessToken：" + at.access_token + "；OpenID：" + at.openid);
                }
                else
                {
                    throw new Exception("验证accesstoken时返回信息与微信官网描述不一致，请检查所填写的地址是否正确");
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteFatal(ex.Message);
                return false;
            }
        }
    }
}