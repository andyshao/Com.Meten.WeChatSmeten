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
    public class CustomServiceHelper
    {
        private const string CustomCacheName = "weixincustomname";
        private static TimeSpan ts = new TimeSpan(1, 0, 0, 0);

        /// <summary>
        /// 获取所有客服账号
        /// </summary>
        /// <returns></returns>
        public static List<CustomService> GetCustomServicesList()
        {
            try
            {
                List<CustomService> resultList = new List<CustomService>();
                object obj = MemoryCacheHelper.GetExistsCache<List<CustomService>>(CustomCacheName);
                if (obj == null) //未找到缓存  ，则获取
                {
                    string result = HttpHelper.GetHtml(CommonData.GetCustomServiceListUrl);
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

                        SortedDictionary<string, List<CustomService>> objList =
                            js.Deserialize<SortedDictionary<string, List<CustomService>>>(result);
                        if (objList.ContainsKey("kf_list"))
                        {
                            resultList = objList["kf_list"];
                        }
                    }
                    else
                    {
                        throw new ApplicationException("获取分组返回的格式错误");
                    }
                    if (resultList != null && resultList.Count > 0)
                        MemoryCacheHelper.AddCache<List<CustomService>>(CustomCacheName, resultList, ts);
                }
                else
                {
                    resultList = (List<CustomService>) obj;
                }
                return resultList;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("获取客服列表错误！错误信息为：" + ex.Message);
                return new List<CustomService>();
            }
        }


        /// <summary>
        /// 创建/修改/删除客服账号
        /// </summary>
        /// <param name="kfAccount"></param>
        /// <param name="nickname"></param>
        /// <param name="password"></param>
        /// <param name="optType">1：添加，2：修改，3：删除</param>
        /// <param name="houzui"></param>
        /// <returns></returns>
        public static bool SaveServiceAccount(string kfAccount, string nickname, string password, int optType = 1,
            string houzui = "@smeten")
        {
            string logResult = "";
            try
            {
//POST 的json数据格式
// {
//     "kf_account" : "test1@test",
//     "nickname" : "客服1",
//     "password" : "pswmd5",
//}
                Dictionary<string, string> s1 = new Dictionary<string, string>();
                string url = CommonData.AddCustomServiceUrl;
                s1.Add("kf_account", kfAccount + houzui);
                s1.Add("nickname", nickname);
                s1.Add("password",
                    System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(password, "MD5"));
                if (optType == 2)
                {
                    url = CommonData.ModifyCustomServiceUrl;
                }
                if (optType == 3)
                {
                    url = CommonData.DeleteCustomServiceUrl;
                }
                JavaScriptSerializer js = new JavaScriptSerializer();
                string jssTr = js.Serialize(s1);
                string result = HttpHelper.GetHTTPPost(url, jssTr);
                SortedDictionary<string, object> sdResult = js.Deserialize<SortedDictionary<string, object>>(result);
                if (sdResult["errcode"].ToString() != "0")
                {
                    throw new Exception(string.Format("操作客服账号发生错误；错误代码：{0},描述为：{1}。", sdResult["errcode"],
                        sdResult["errmsg"]) + (sdResult["errcode"] == "-1"
                        ? "请确保当前账号没有客户接入状态后再修改！"
                        : ""));
                }
                else
                {
                    logResult = "操作成功：返回信息为：" + result;
                    //清除缓存，这样下次获取时，就会及时获取到
                    RemoveGroupFromCache();
                    LogHelper.WriteDebug(logResult);
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message);
                return false;
            }
        }


        /// <summary>
        /// 保存客服账号头像
        /// </summary>
        /// <param name="kfAccount">账号</param>
        /// <param name="vpath">图片地址</param>
        /// <returns>如果为空则表示成功，否则为失败信息</returns>
        public static string SaveServiceAccountHeadImg(string kfAccount, string vpath)
        {
            {
                try
                {
                    if (string.IsNullOrEmpty(CommonData.ModifyCustomServiceImgUrl))
                    {
                        throw new Exception("SaveServiceAccountHeadImg方法。。缺少更新客服头像的路径配置或者该配置为空，无法继续上传");
                    }
                    string wxurl = string.Format(CommonData.ModifyCustomServiceImgUrl, CommonData.AccessToken, kfAccount);
                    string filepath = HttpContext.Current.Server.MapPath("~") + vpath; //(本地服务器的地址)
                    LogHelper.FileLog.Debug("SaveServiceAccountHeadImg方法。。上传路径:" + filepath);
                    string result = HttpHelper.UploadFile(wxurl, "post", filepath);
                    LogHelper.FileLog.Debug("SaveServiceAccountHeadImg方法。。上传result:" + result);
                    return result;
                }
                catch (Exception ex)
                {
                    LogHelper.FileLog.Fatal(string.Format("SaveServiceAccountHeadImg方法。。上传文件发生错误！AT:{0};kfAccount:{1};vpath:{2};错误信息:{3}", CommonData.AccessToken,kfAccount, vpath, ex.Message));
                    return ex.Message;
                }
            }
        }


        /// <summary>
        /// 删除客服的缓存
        /// </summary>
        private static void RemoveGroupFromCache()
        {
            MemoryCacheHelper.RemoveCacheByKey(CustomCacheName);
        }
    }
}