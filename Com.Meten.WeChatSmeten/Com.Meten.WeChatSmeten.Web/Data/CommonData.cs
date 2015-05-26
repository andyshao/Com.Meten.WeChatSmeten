using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using Com.Meten.WeChatSmeten.Entities;
using Com.Meten.WeChatSmeten.Helper;

namespace Com.Meten.WeChatSmeten.Web.Data
{
    public class CommonData
    {
        /// <summary>
        /// 上传到微信文件的源路径
        /// </summary>
        public static string UploadFilePath
        {
            get { return string.Format("Upload\\{0}\\", DateTime.Now.ToString("yyyyMMdd")); }
        }

        /// <summary>
        /// 从微信下载多媒体文件的保存路径
        /// </summary>
        public static string DownloadFilePath
        {
            get { return string.Format("Download\\{0}\\", DateTime.Now.ToString("yyyyMMdd")); }
        } 

        /// <summary>
        /// 微信公众号填写的TOKEN
        /// </summary>
        public static string WeixinToken
        {
            get
            {
                try
                {
                    return ReadCacheIfNullWriteBeforeWrite("WeixinToken");
                }
                catch (Exception ex)
                {
                    LogHelper.WriteFatal(ex.Message);
                    throw ex;
                }
            }
        }


        /// <summary>
        /// 微信WeiXinAppID
        /// </summary>
        public static string AppID
        {
            get
            {
                try
                {
                    return ReadCacheIfNullWriteBeforeWrite("WeiXinAppID");
                }
                catch (Exception ex)
                {
                    LogHelper.WriteFatal(ex.Message);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// APPID对应的密钥
        /// </summary>
        public static string AppSecret
        {
            get
            {
                try
                {
                    return ReadCacheIfNullWriteBeforeWrite("WeiXinAppSecret");
                }
                catch (Exception ex)
                {
                    LogHelper.WriteFatal(ex.Message);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 消息加密密钥
        /// </summary>
        public static string EncodingAESKey
        {
            get
            {
                try
                {
                    return ReadCacheIfNullWriteBeforeWrite("sEncodingAESKey");
                }
                catch (Exception ex)
                {
                    LogHelper.WriteFatal(ex.Message);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// OAuth回调地址；应与微信公众号开发设置里面所配置的一致。。
        /// </summary>
        public static string Oauth2RedirectUrl
        {
            get
            {
                try
                {
                    return ReadCacheIfNullWriteBeforeWrite("Oauth2RedirectUrl");
                }
                catch (Exception ex)
                {
                    LogHelper.WriteFatal(ex.Message);
                    throw ex;
                }
            }
        }





        /// <summary>
        /// 微信回复时普通文本消息
        /// </summary>
        public static string MessageText
        {
            get
            {
                try
                {
                    return ReadCacheIfNullWriteBeforeWrite("Message_Text");
                }
                catch (Exception ex)
                {
                    LogHelper.WriteFatal(ex.Message);
                    throw ex;
                }
            }
        }


        /// <summary>
        /// 微信回复时图片消息
        /// </summary>
        public static string MessagePicture
        {
            get
            {
                try
                {
                    return ReadCacheIfNullWriteBeforeWrite("Message_Picture");
                }
                catch (Exception ex)
                {
                    LogHelper.WriteFatal(ex.Message);
                    throw ex;
                }
            }
        }


        /// <summary>
        /// 微信回复时语音消息
        /// </summary>
        public static string MessageVoice
        {
            get
            {
                try
                {
                    return ReadCacheIfNullWriteBeforeWrite("Message_Voice");
                }
                catch (Exception ex)
                {
                    LogHelper.WriteFatal(ex.Message);
                    throw ex;
                }
            }
        }



        /// <summary>
        /// 微信回复时视频消息
        /// </summary>
        public static string MessageVideo
        {
            get
            {
                try
                {
                    return ReadCacheIfNullWriteBeforeWrite("Message_Video");
                }
                catch (Exception ex)
                {
                    LogHelper.WriteFatal(ex.Message);
                    throw ex;
                }
            }
        }



        /// <summary>
        /// 微信回复时视频消息
        /// </summary>
        public static string MessageMusic
        {
            get
            {
                try
                {
                    return ReadCacheIfNullWriteBeforeWrite("Message_Music");
                }
                catch (Exception ex)
                {
                    LogHelper.WriteFatal(ex.Message);
                    throw ex;
                }
            }
        }


        /// <summary>
        /// 微信回复时转到多客服的消息
        /// </summary>
        public static string Message_ToServiceCustom
        {
            get
            {
                try
                {
                    return ReadCacheIfNullWriteBeforeWrite("Message_ToServiceCustom");
                }
                catch (Exception ex)
                {
                    LogHelper.WriteFatal(ex.Message);
                    throw ex;
                }
            }
        }


        /// <summary>
        /// 微信回复时转到多客服且指定客服回复的消息
        /// </summary>
        public static string Message_ToServiceCustonDetail
        {
            get
            {
                try
                {
                    return ReadCacheIfNullWriteBeforeWrite("Message_ToServiceCustonDetail");
                }
                catch (Exception ex)
                {
                    LogHelper.WriteFatal(ex.Message);
                    throw ex;
                }
            }
        }

        /// <summary>
        ///  获取AccessToken
        /// </summary>
        public static string AccessToken
        {
            get
            {
                try
                {
                    object obj = MemoryCacheHelper.GetExistsCache<string>("access_token");
                    string at = "";
                    if (obj != null) at = obj.ToString();
                    if (string.IsNullOrEmpty(at))
                    {
                        string turl = ReadCacheIfNullWriteBeforeWrite("GetAccessTokenUrl");

                        if (string.IsNullOrEmpty(turl))
                        {
                            throw new Exception("未找到有效的获取Access Token的URL地址，请检查配置文件");
                        }
                        turl = string.Format(turl, AppID, AppSecret);
                        LogHelper.WriteDebug("缓存里面不存在AccessToken,获取AccessToken的地址:" + turl);
                        string result = HttpHelper.GetHtml(turl);
                        if (!string.IsNullOrEmpty(result))
                        {
                            JavaScriptSerializer js = new JavaScriptSerializer();
                            SortedDictionary<string, string> sdResult =
                                js.Deserialize<SortedDictionary<string, string>>(result);
                            if (sdResult != null)
                            {
                                if (sdResult.ContainsKey("errcode")) //有错误
                                {
                                    throw new ApplicationException(sdResult.ContainsKey("errcode")
                                        ? "错误代码为："+sdResult["errcode"]+(sdResult.ContainsKey("errmsg")? "；错误描述为："+sdResult["errmsg"]: "")
                                        : "");
                                }
                                else
                                {
                                    at = sdResult["access_token"];
                                    int s = Convert.ToInt32(sdResult["expires_in"]);
                                    //默认两小时
                                    MemoryCacheHelper.AddCache<string>("access_token", at, new TimeSpan(0, s, 0, 0));
                                }
                            }
                            else
                            {
                                throw new Exception("微信服务器未返回相关信息！");
                            }
                        }
                    }
                    return at;
                }
                catch (Exception exception)
                {
                    LogHelper.WriteFatal("获取access_token发生错误," + exception.Message);
                    throw exception;
                }
            }
        }
 
        /// <summary>
        /// WeiXinIp  放在缓存。24小时刷新一次
        /// </summary>
        public static string WeiXinIp
        {
            get
            {
                try
                {
                    object obj = MemoryCacheHelper.GetExistsCache<List<string>>("WeiXinIp");
                    List<string> ipList = new List<string>();
                    if (obj != null) ipList = (List<string>)obj;
                    if (ipList.Count==0)  //不存在
                    {
                        string turl = string.Format(ReadCacheIfNullWriteBeforeWrite("GetWeiXinIpUrl"), AccessToken);
                        if (string.IsNullOrEmpty(turl))
                        {
                            throw new Exception("未找到有效的获取IP地址的URL地址，请检查配置文件");
                        }
                        LogHelper.WriteDebug("缓存里面不存在ip列表,获取IP列表的地址:" + turl);
                        string result = HttpHelper.GetHtml(turl);
                        if (!string.IsNullOrEmpty(result))
                        {
                            JavaScriptSerializer js = new JavaScriptSerializer();
                            SortedDictionary<string, List<string>> sdResult =
                                js.Deserialize<SortedDictionary<string, List<string>>>(result);
                            if (sdResult != null)
                            {
                                if (sdResult.ContainsKey("errcode")) //有错误
                                {
                                    throw new ApplicationException(sdResult.ContainsKey("errcode")
                                        ? "错误代码为：" + sdResult["errcode"] + (sdResult.ContainsKey("errmsg") ? "；错误描述为：" + sdResult["errmsg"] : "")
                                        : "");
                                }
                                else
                                {
                                    ipList = sdResult["ip_list"];
                                    //默认两小时
                                    MemoryCacheHelper.AddCache<List<string>>("WeiXinIp", ipList, new TimeSpan(1, 0, 0, 0));
                                }
                            }
                            else
                            {
                                throw new Exception("微信服务器未返回相关信息！");
                            }
                        }
                    }
                    string rootResult = ipList.Aggregate("", (current, s) => current + (s + "、"));
                    return rootResult.Substring(0, rootResult.Length - 1);
                }
                catch (Exception exception)
                {
                    LogHelper.WriteFatal("获取IP地址发生错误,错误信息为:" + exception.Message);
                    throw exception;
                }
            }
        }

        /// <summary>
        /// 上传多媒体文件的地址
        /// </summary>
        public static string UploadMediaFileUrl
        {
            get
            {
                try
                {
                    return ReadCacheIfNullWriteBeforeWrite("UploadMediaFileUrl");
                }
                catch (Exception ex)
                {
                    LogHelper.WriteFatal(ex.Message);
                    throw ex;
                }
            }
        }
        /// <summary>
        /// 下载多媒体文件的地址
        /// </summary>
        public static string DownloadMediaFileUrl
        {
            get
            {
                try
                {
                    return ReadCacheIfNullWriteBeforeWrite("DownloadMediaFileUrl");
                }
                catch (Exception ex)
                {
                    LogHelper.WriteFatal(ex.Message);
                    throw ex;
                }
            }
        }
        /// <summary>
        /// 获取素材列表地址
        /// </summary>
        public static string GetMaterialListUrl
        {
            get
            {
                try
                {
                    return ReadCacheIfNullWriteBeforeWrite("GetMaterialListUrl");
                }
                catch (Exception ex)
                {
                    LogHelper.WriteFatal(ex.Message);
                    throw ex;
                }
            }
        }
        /// <summary>
        /// 获取永久素材地址
        /// </summary>
        public static string GetMaterialDetailUrl
        {
            get
            {
                try
                {
                    return ReadCacheIfNullWriteBeforeWrite("GetMaterialDetailUrl");
                }
                catch (Exception ex)
                {
                    LogHelper.WriteFatal(ex.Message);
                    throw ex;
                }
            }
        }
        /// <summary>
        /// 获取所有自定义分组的地址
        /// </summary>
        public static string GetGroupListUrl
        {
            get
            {
                try
                {
                    return string.Format(ReadCacheIfNullWriteBeforeWrite("GetGroupListUrl"),AccessToken);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteFatal(ex.Message);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 创建分组的地址
        /// </summary>
        public static string CreateGroupUrl
        {
            get
            {
                try
                {
                    return string.Format(ReadCacheIfNullWriteBeforeWrite("CreateGroupUrl"), AccessToken);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteFatal(ex.Message);
                    throw ex;
                }
            }
        }


        /// <summary>
        /// 修改分组的地址
        /// </summary>
        public static string ModifyGroupUrl
        {
            get
            {
                try
                {
                    return string.Format(ReadCacheIfNullWriteBeforeWrite("ModifyGroupUrl"), AccessToken);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteFatal(ex.Message);
                    throw ex;
                }
            }
        }

        


        /// <summary>
        /// 获取关注用户列表的地址
        /// </summary>
        public static string GetUserListUrl
        {
            get
            {
                try
                {
                    return ReadCacheIfNullWriteBeforeWrite("GetUserListUrl");
                }
                catch (Exception ex)
                {
                    LogHelper.WriteFatal(ex.Message);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 获取用户基本信息
        /// </summary>
        public static string GetUserInfoUrl
        {
            get
            {
                try
                {
                    return ReadCacheIfNullWriteBeforeWrite("GetUserInfoUrl");
                }
                catch (Exception ex)
                {
                    LogHelper.WriteFatal(ex.Message);
                    throw ex;
                }
            }
        }

        
        /// <summary>
        /// 获取用户所在分组
        /// </summary>
        public static string GetUserGroupUrl
        {
            get
            {
                try
                {
                    return string.Format(ReadCacheIfNullWriteBeforeWrite("GetUserGroupUrl"), AccessToken);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteFatal(ex.Message);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 移动用户分组
        /// </summary>
        public static string MoveUserGroupUrl
        {
            get
            {
                try
                {
                    return string.Format(ReadCacheIfNullWriteBeforeWrite("MoveUserGroupUrl"), AccessToken);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteFatal(ex.Message);
                    throw ex;
                }
            }
        }
        /// <summary>
        /// 移动用户分组（批量）
        /// </summary>
        public static string MoveUserGroupBatchUrl
        {
            get
            {
                try
                {
                    return string.Format(ReadCacheIfNullWriteBeforeWrite("MoveUserGroupBatchUrl"), AccessToken);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteFatal(ex.Message);
                    throw ex;
                }
            }
        }
        /// <summary>
        /// 设置用户备注
        /// </summary>
        public static string SetUserRemarkUrl
        {
            get
            {
                try
                {
                    return string.Format(ReadCacheIfNullWriteBeforeWrite("SetUserRemarkUrl"), AccessToken);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteFatal(ex.Message);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 获取所有客服账号
        /// </summary>
        public static string GetCustomServiceListUrl
        {
            get
            {
                try
                {
                    return string.Format(ReadCacheIfNullWriteBeforeWrite("GetCustomServiceListUrl"), AccessToken);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteFatal(ex.Message);
                    throw ex;
                }
            }
        } 
        
        /// <summary>
        /// 添加客服账号
        /// </summary>
        public static string AddCustomServiceUrl
        {
            get
            {
                try
                {
                    return string.Format(ReadCacheIfNullWriteBeforeWrite("AddCustomServiceUrl"), AccessToken);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteFatal(ex.Message);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 添加客服账号
        /// </summary>
        public static string ModifyCustomServiceUrl
        {
            get
            {
                try
                {
                    return string.Format(ReadCacheIfNullWriteBeforeWrite("ModifyCustomServiceUrl"), AccessToken);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteFatal(ex.Message);
                    throw ex;
                }
            }
        }
        /// <summary>
        /// 删除客服账号
        /// </summary>
        public static string DeleteCustomServiceUrl
        {
            get
            {
                try
                {
                    return string.Format(ReadCacheIfNullWriteBeforeWrite("DeleteCustomServiceUrl"), AccessToken);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteFatal(ex.Message);
                    throw ex;
                }
            }
        }
        /// <summary>
        /// 修改客服头像
        /// </summary>
        public static string ModifyCustomServiceImgUrl
        {
            get
            {
                try
                {
                    return ReadCacheIfNullWriteBeforeWrite("ModifyCustomServiceImgUrl");
                }
                catch (Exception ex)
                {
                    LogHelper.WriteFatal(ex.Message);
                    throw ex;
                }
            }
        }
        
        /// <summary>
        /// 获取所有的自定义菜单
        /// </summary>
        public static string GetAllMenusUrl
        {
            get
            {
                try
                {
                    return string.Format(ReadCacheIfNullWriteBeforeWrite("GetAllMenusUrl"), AccessToken);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteFatal(ex.Message);
                    throw ex;
                }
            }
        }
        
        /// <summary>
        /// 保存所有的自定义菜单
        /// </summary>
        public static string CreateAllMenusUrl
        {
            get
            {
                try
                {
                    return string.Format(ReadCacheIfNullWriteBeforeWrite("CreateAllMenusUrl"), AccessToken);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteFatal(ex.Message);
                    throw ex;
                }
            }
        } 
        /// <summary>
        /// 删除所有的自定义菜单
        /// </summary>
        public static string DeleteAllMenusUrl
        {
            get
            {
                try
                {
                    return string.Format(ReadCacheIfNullWriteBeforeWrite("DeleteAllMenusUrl"), AccessToken);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteFatal(ex.Message);
                    throw ex;
                }
            }
        }
        /// <summary>
        /// 引导用户授权的地址
        /// </summary>
        /// <param name="st"></param>
        /// <returns></returns>
        public static string GetOAuthByScopeType(ScopeType st)
        {
            try
            {
                string url= string.Format(ReadCacheIfNullWriteBeforeWrite("GoToOauth2Url"), AppID, Oauth2RedirectUrl, st.ToString());
                //LogHelper.WriteInfo(url);
                return url;
            }
            catch (Exception ex)
            {
                LogHelper.WriteFatal(ex.Message);
                throw ex;
            }
        }
        
        /// <summary>
        /// 得到获取AccessToken的地址
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string GetOauth2AccessTokenUrl(string code)
        {
            try
            {
                string url = string.Format(ReadCacheIfNullWriteBeforeWrite("GetOauth2AccessTokenUrl"), AppID, AppSecret, code);
                LogHelper.WriteInfo("获取access_token地址的URL:" + url);
                return url;
            }
            catch (Exception ex)
            {
                LogHelper.WriteFatal(ex.Message);
                throw ex;
            }
        }
        
        /// <summary>
        /// 用户授权后获取at的地址
        /// </summary>
        /// <param name="st"></param>
        /// <returns></returns>
        public static string GetUserInfoByAccessTokenUrl
        {
            get{
            try
            {
                return ReadCacheIfNullWriteBeforeWrite("GetUserInfoByAccessTokenUrl");
            }
            catch (Exception ex)
            {
                LogHelper.WriteFatal(ex.Message);
                throw ex;
            }
            }
        }

        
        /// <summary>
        /// 得到刷新AccessToken的地址
        /// </summary>
        public static string GetOauth2RefreshAccessTokenUrl
        {
            get
            {
                try
                {
                    return ReadCacheIfNullWriteBeforeWrite("GetOauth2RefreshAccessTokenUrl");
                }
                catch (Exception ex)
                {
                    LogHelper.WriteFatal(ex.Message);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 得到验证AccessToken的地址
        /// </summary>
        public static string VerifyOauth2AccessTokenUrl
        {
            get
            {
                try
                {
                    return ReadCacheIfNullWriteBeforeWrite("VerifyOauth2AccessTokenUrl");
                }
                catch (Exception ex)
                {
                    LogHelper.WriteFatal(ex.Message);
                    throw ex;
                }
            }
        }

        

        /// <summary>
        /// 图文消息主体
        /// </summary>
        public static string MessageNewsMain
        {
            get
            {
                try
                {
                    return ReadCacheIfNullWriteBeforeWrite("Message_News_Main");
                }
                catch (Exception ex)
                {
                    LogHelper.WriteFatal(ex.Message);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 图文消息项
        /// </summary>
        public static string MessageNewsItem
        {
            get
            {
                try
                {
                    return ReadCacheIfNullWriteBeforeWrite("Message_News_Item");
                }
                catch (Exception ex)
                {
                    LogHelper.WriteFatal(ex.Message);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 获取缓存内容，如果没有则先添加再获取
        /// </summary>
        /// <param name="cacheName"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        private static string ReadCacheIfNullWriteBeforeWrite(string cacheName, TimeSpan? ts = null)
        {
            object obj = MemoryCacheHelper.GetExistsCache<string>(cacheName);
            if (obj == null || string.IsNullOrEmpty(obj.ToString())) //未找到缓存  ，则从配置文件获取
            {
                string temp = string.IsNullOrEmpty(WebConfigurationManager.AppSettings[cacheName])
                    ? ""
                    : WebConfigurationManager.AppSettings[cacheName];
                if (string.IsNullOrEmpty(temp))
                {
                    throw new Exception(string.Format("未找到{0}配置节点或该节点为空", cacheName));
                }

                if (ts.HasValue)
                {
                    MemoryCacheHelper.AddCache<string>(cacheName, temp, ts.Value);
                }
                else
                {
                    MemoryCacheHelper.AddCache<string>(cacheName, temp);
                }
                return ReadCacheIfNullWriteBeforeWrite(cacheName, ts);
            }
            else
            {
                return obj.ToString();
            }
        }


    }
}