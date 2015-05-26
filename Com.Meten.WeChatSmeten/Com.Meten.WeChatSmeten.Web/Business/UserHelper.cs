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
    public class UserHelper
    {
        /// <summary>
        /// 获取用户所在分组
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public static UserGroup GetUserGroup(string openid)
        {
            string logResult = "";
            try
            {
                string url = CommonData.GetUserGroupUrl;
                Dictionary<string, string> pa = new Dictionary<string, string>();
                pa.Add("openid", openid);
                JavaScriptSerializer js = new JavaScriptSerializer();
                string jssTr = js.Serialize(pa);
                string result = HttpHelper.GetHTTPPost(url, jssTr);

                SortedDictionary<string, string> sdResult = js.Deserialize<SortedDictionary<string, string>>(result);
                string reResult = "";
                if (sdResult.ContainsKey("errcode"))
                {
                    throw new Exception(string.Format("获取用户分组发生错误；错误代码：{0},描述为：{1}。", sdResult["errcode"],
                        sdResult["errmsg"]));
                }
                logResult = "获取成功：返回信息为：" + result;
                if (sdResult.ContainsKey("groupid"))
                {
                    string group = sdResult["groupid"];
                    return UserGroupHepler.GetGroups().Where(d => d.Id == @group).FirstOrDefault();
                }
                logResult += ".缺少groupid信息，返回的格式不合法";
                LogHelper.WriteError(logResult);
                return null;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex);
                return null;
            }
        }

        /// <summary>
        /// 移动用户分组
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="to_groupid"></param>
        /// <returns></returns>
        public static bool MoveUserGroup(string openid, string to_groupid)
        {
            string logResult = "";
            try
            {
                string url = CommonData.MoveUserGroupUrl;
                Dictionary<string, string> pa = new Dictionary<string, string>();
                pa.Add("openid", openid);
                pa.Add("to_groupid", to_groupid);
                JavaScriptSerializer js = new JavaScriptSerializer();
                string jssTr = js.Serialize(pa);
                string result = HttpHelper.GetHTTPPost(url, jssTr);
                SortedDictionary<string, string> sdResult = js.Deserialize<SortedDictionary<string, string>>(result);
                //正常时的返回JSON数据包示例：{"errcode": 0, "errmsg": "ok"}
                //错误时的JSON数据包示例（该示例为AppID无效错误）：{"errcode":40013,"errmsg":"invalid appid"}
                string reResult = "";
                if (sdResult.ContainsKey("errcode"))
                {
                    if (sdResult["errcode"] == "0") //无误。。
                    {
                        logResult = "移动用户分组成功：返回信息为：" + result;
                        //移动后清楚分组缓存
                        UserGroupHepler.RemoveGroupFromCache();
                    }
                    else
                    {
                        throw new Exception(string.Format("移动用户分组发生错误；错误代码：{0},描述为：{1}。", sdResult["errcode"],
                     sdResult["errmsg"]));
                    }
                }
                else
                {
                    throw new Exception("返回数据与微信公布的接口不符，请查看配置文件。");
                }
                LogHelper.WriteDebug(logResult);
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex);
                return false;
            }
        }


        /// <summary>
        /// 移动用户分组(批量)
        /// </summary>
        /// <param name="openidList"></param>
        /// <param name="to_groupid"></param>
        /// <returns></returns>
        public static bool MoveUserGroup(List<string> openidList, string to_groupid)
        {
            string logResult = "";
            try
            {
                string url = CommonData.MoveUserGroupBatchUrl;
                Dictionary<string, object> pa = new Dictionary<string, object>();
                pa.Add("openid_list", openidList);
                pa.Add("to_groupid", to_groupid);
                JavaScriptSerializer js = new JavaScriptSerializer();
                string jssTr = js.Serialize(pa);
                string result = HttpHelper.GetHTTPPost(url, jssTr);
                SortedDictionary<string, string> sdResult = js.Deserialize<SortedDictionary<string, string>>(result);
                //正常时的返回JSON数据包示例：{"errcode": 0, "errmsg": "ok"}
                //错误时的JSON数据包示例（该示例为AppID无效错误）：{"errcode":40013,"errmsg":"invalid appid"}
                string reResult = "";
                if (sdResult.ContainsKey("errcode"))
                {
                    if (sdResult["errcode"] == "0") //无误。。
                    {
                        logResult = "移动用户分组成功：返回信息为：" + result;
                        //移动后清楚分组缓存
                        UserGroupHepler.RemoveGroupFromCache();
                    }
                    else
                    {
                        throw new Exception(string.Format("移动用户分组发生错误；错误代码：{0},描述为：{1}。", sdResult["errcode"],sdResult["errmsg"]));
                    }
                }
                else
                {
                    throw new Exception("返回数据与微信公布的接口不符，请查看配置文件。");
                }
                LogHelper.WriteDebug(logResult);
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex);
                return false;
            }
        }

        /// <summary>
        /// 设置用户备注名
        /// </summary>
        /// <param name="open_id"></param>
        /// <param name="remarkName"></param>
        /// <returns></returns>
        public static bool SetRemark(string open_id, string remarkName)
        {
            string logResult = "";
            try
            {
                string url = CommonData.SetUserRemarkUrl;
                Dictionary<string, object> pa = new Dictionary<string, object>();
                pa.Add("openid", open_id);
                pa.Add("remark", remarkName);
                JavaScriptSerializer js = new JavaScriptSerializer();
                string jssTr = js.Serialize(pa);
                string result = HttpHelper.GetHTTPPost(url, jssTr);
                SortedDictionary<string, string> sdResult = js.Deserialize<SortedDictionary<string, string>>(result);
                //正常时的返回JSON数据包示例：{"errcode": 0, "errmsg": "ok"}
                //错误时的JSON数据包示例（该示例为AppID无效错误）：{"errcode":40013,"errmsg":"invalid appid"}
                string reResult = "";
                if (sdResult.ContainsKey("errcode"))
                {
                    if (sdResult["errcode"] == "0") //无误。。
                    {
                        logResult = "设置成功：返回信息为：" + result;
                    }
                    else
                    {
                        throw new Exception(string.Format("设置用户昵称发生错误；错误代码：{0},描述为：{1}。", sdResult["errcode"], sdResult["errmsg"]));
                    }
                }
                else
                {
                    throw new Exception("返回数据与微信公布的接口不符，请查看配置文件。");
                }
                LogHelper.WriteDebug(logResult);
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex);
                return false;
            }
        }


        /// <summary>
        /// 根据open_id获取用户信息
        /// </summary>
        /// <param name="open_id"></param>
        /// <returns></returns>
        public static User GetUserInfo(string open_id)
        {
            try
            {
                if (string.IsNullOrEmpty(open_id))
                {
                    throw new Exception("缺少open_id参数");
                }
                string url = string.Format(CommonData.GetUserInfoUrl, CommonData.AccessToken, open_id, "zh_CN");
                string result = HttpHelper.GetHtml(url);
                JavaScriptSerializer js = new JavaScriptSerializer();
                SortedDictionary<string, string> sdResult = js.Deserialize<SortedDictionary<string, string>>(result);
                if (sdResult.ContainsKey("errcode")) //有错误
                {
                    throw new ApplicationException(sdResult.ContainsKey("errcode")? "错误代码为：" + sdResult["errcode"] +(sdResult.ContainsKey("errmsg") ? "；错误描述为：" + sdResult["errmsg"] : "")           : "");
                }
                else
                {
                    return js.Deserialize<User>(result);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("获取用户信息发生错误；错误详情为" + ex.Message);
                return null;
            }
        }
    }
}