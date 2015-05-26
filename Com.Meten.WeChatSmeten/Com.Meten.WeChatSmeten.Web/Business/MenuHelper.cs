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
    /// <summary>
    /// 自定义菜单管理
    /// </summary>
    public class MenuHelper
    {
        private const string menuCacheName = "weixinmenuname";
        private static TimeSpan ts = new TimeSpan(1, 0, 0, 0);

        /// <summary>
        /// 获取所有的自定义菜单
        /// </summary>
        /// <returns></returns>
        public static List<UserButton> GetAllMenus()
        {
            List<UserButton> rList = new List<UserButton>();
            try
            {
                UserMenu umMenu = new UserMenu();
                object obj = MemoryCacheHelper.GetExistsCache<UserMenu>(menuCacheName);
                if (obj == null) //未找到缓存  ，则获取
                {
                    string result = HttpHelper.GetHtml(CommonData.GetAllMenusUrl);
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    SortedDictionary<string, UserMenu> sdResult =
                        js.Deserialize<SortedDictionary<string, UserMenu>>(result);
                    if (sdResult != null)
                    {
                        if (sdResult.ContainsKey("errcode")) //有错误
                        {
                            throw new ApplicationException(sdResult.ContainsKey("errcode")
                                ? "获取分组发生错误；代码为：" + sdResult["errcode"] +
                                  (sdResult.ContainsKey("errmsg") ? "；错误描述为：" + sdResult["errmsg"] : "")
                                : "");
                        }

                        if (sdResult.ContainsKey("menu"))
                        {
                            umMenu = sdResult["menu"];
                        }
                    }
                    else
                    {
                        throw new ApplicationException("获取分组返回的格式错误");
                    }
                    if (umMenu != null && umMenu.button.Count > 0)
                        MemoryCacheHelper.AddCache<UserMenu>(menuCacheName, umMenu, ts);
                }
                else
                {
                    umMenu = (UserMenu) obj;
                }
                rList = umMenu.button;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("获取自定义菜单错误！错误信息为：" + ex.Message);
            }

            while (rList.Count < 3)
            {
                rList.Add(new UserButton());
            }

            for (int i = 0; i < rList.Count; i++)
            {
                if (rList[i].sub_button == null)
                    rList[i].sub_button = new List<UserButton>();
                while (rList[i].sub_button.Count < 5)
                {
                    rList[i].sub_button.Add(new UserButton());
                }
            }
            return rList;
        }


        /// <summary>
        /// 自定义菜单的保存
        /// </summary>
        /// <param name="buttons"></param>
        /// <returns></returns>
        public static string SaveButtons(List<UserButton> buttons)
        {
            try
            {
                string url = CommonData.CreateAllMenusUrl;
                Dictionary<string, List<UserButton>> s1 = new Dictionary<string, List<UserButton>>();
                s1.Add("button", buttons);
                JavaScriptSerializer js = new JavaScriptSerializer();
                string jssTr = js.Serialize(s1).Replace("\\u0026", "&");
                string result = HttpHelper.GetHTTPPost(url, jssTr);
                SortedDictionary<string, string> sdResult = js.Deserialize<SortedDictionary<string, string>>(result);
                if (sdResult.ContainsKey("errcode"))
                {
                    if (sdResult["errcode"] != "0")
                        throw new Exception(string.Format("保存自定义菜单发生错误；错误代码：{0},描述为：{1}。", sdResult["errcode"],
                            sdResult["errmsg"]));
                    MemoryCacheHelper.RemoveCacheByKey(menuCacheName);
                    return "";
                }
                else
                {
                    throw new Exception("保存自定义菜单未返回预期数据，可能是请求路径错误或网络不通！");
                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message);
                return ex.Message;
            }
        }

        /// <summary>
        /// 删除自定义菜单
        /// </summary>
        /// <returns></returns>
        public static bool DeleteButtons()
        {
            try
            {
                string url = CommonData.DeleteAllMenusUrl;
                JavaScriptSerializer js = new JavaScriptSerializer();
                string result = HttpHelper.GetHtml(url);
                SortedDictionary<string, string> sdResult = js.Deserialize<SortedDictionary<string, string>>(result);
                if (sdResult.ContainsKey("errcode"))
                {
                    if (sdResult["errcode"] != "0")
                        throw new Exception(string.Format("删除自定义菜单发生错误；错误代码：{0},描述为：{1}。", sdResult["errcode"],
                            sdResult["errmsg"]));
                    MemoryCacheHelper.RemoveCacheByKey(menuCacheName);
                    return true;
                }
                else
                {
                    throw new Exception("删除自定义菜单未返回预期数据，可能是请求路径错误或网络不通！");
                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message);
                return false;
            }
        }

    
    }
}