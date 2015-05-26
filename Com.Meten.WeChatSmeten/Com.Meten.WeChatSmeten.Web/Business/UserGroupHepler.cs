using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using Com.Meten.WeChatSmeten.Entities;
using Com.Meten.WeChatSmeten.Helper;
using Com.Meten.WeChatSmeten.Web.Data;

namespace Com.Meten.WeChatSmeten.Web.Business
{
    public class UserGroupHepler
    {
        private const string groupCacheName = "weixingroupname";
        private static TimeSpan ts = new TimeSpan(1, 0, 0, 0);

        /// <summary>
        /// 获取微信分组(这里会存到缓存，缓存过期后才去微信服务器获取)
        /// </summary>
        /// <returns></returns>
        public static List<UserGroup> GetGroups()
        {
            try
            {
                List<UserGroup> resultList = new List<UserGroup>();
                object obj = MemoryCacheHelper.GetExistsCache<List<UserGroup>>(groupCacheName);
                if (obj == null) //未找到缓存  ，则获取
                {
                    string result = HttpHelper.GetHtml(CommonData.GetGroupListUrl);
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    SortedDictionary<string, object> sdResult =
                        js.Deserialize<SortedDictionary<string, object>>(result);
                    if (sdResult != null)
                    {
                        if (sdResult.ContainsKey("errcode")) //有错误
                        {
                            throw new ApplicationException(sdResult.ContainsKey("errcode")
                                ? "获取分组发生错误；代码为：" + sdResult["errcode"] +
                                  (sdResult.ContainsKey("errmsg") ? "；错误描述为：" + sdResult["errmsg"] : "")
                                : "");
                        }

                        SortedDictionary<string, List<UserGroup>> objList =
                            js.Deserialize<SortedDictionary<string, List<UserGroup>>>(result);
                        if (objList.ContainsKey("groups"))
                        {
                            resultList = objList["groups"];
                        }
                    }
                    else
                    {
                        throw new ApplicationException("获取分组返回的格式错误");
                    }
                    if (resultList != null && resultList.Count > 0)
                        MemoryCacheHelper.AddCache<List<UserGroup>>(groupCacheName, resultList, ts);
                }
                else
                {
                    resultList = (List<UserGroup>) obj;
                }
                return resultList;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("获取分组错误！错误信息为：" + ex.Message);
                return new List<UserGroup>();
            }
        }


        /// <summary>
        /// 创建或修改分组 
        /// </summary>
        /// <param name="GroupName"></param>
        /// <param name="GroupID">为0，则为新建</param>
        /// <returns>对应分组的ID；为空，则表示失败！</returns>
        public static string SaveGroup(string GroupName, string GroupID = "0")
        {
            string logResult = "";
            try
            {
                string url = CommonData.CreateGroupUrl;
                Dictionary<string, string> s1 = new Dictionary<string, string>();
                s1.Add("name", GroupName);
                if (GroupID != "0")
                {
                    s1.Add("id", GroupID);
                    url = CommonData.ModifyGroupUrl;
                }
                Dictionary<string, Dictionary<string, string>> pars =
                    new Dictionary<string, Dictionary<string, string>>();
                pars.Add("group", s1);
                JavaScriptSerializer js = new JavaScriptSerializer();
                string jssTr = js.Serialize(pars);
                string result = HttpHelper.GetHTTPPost(url, jssTr);

                SortedDictionary<string, object> sdResult = js.Deserialize<SortedDictionary<string, object>>(result);
                string reResult = "";
                if (GroupID == "0")
                {
                    if (sdResult.ContainsKey("errcode"))
                    {
                        throw new Exception(string.Format("创建分组发生错误；错误代码：{0},描述为：{1}。", sdResult["errcode"],
                            sdResult["errmsg"]));
                    }

                    logResult = "创建成功：返回信息为：" + result;
                    SortedDictionary<string, SortedDictionary<string, string>> objList =js.Deserialize<SortedDictionary<string, SortedDictionary<string, string>>>(result);
                    reResult = objList["group"]["id"];
                }
                else
                {
                    if (sdResult["errcode"].ToString() != "0")
                    {
                        throw new Exception(string.Format("修改分组发生错误；错误代码：{0},描述为：{1}。", sdResult["errcode"],
                            sdResult["errmsg"]));
                    }
                    logResult = "修改成功：返回信息为：" + result;
                    reResult = GroupID;
                }
                //清除缓存，这样下次获取时，就会及时获取到
                RemoveGroupFromCache();
                LogHelper.WriteDebug(logResult);
                return reResult;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message);
                return "";
            }
        }


        /// <summary>
        /// 删除分组的缓存
        /// </summary>
        public static void RemoveGroupFromCache()
        {
            MemoryCacheHelper.RemoveCacheByKey(groupCacheName);
        }

    
    }
}