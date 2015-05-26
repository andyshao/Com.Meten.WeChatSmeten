using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls.Adapters;
using Com.Meten.WeChatSmeten.Entities;
using Com.Meten.WeChatSmeten.Entities.Class;
using Com.Meten.WeChatSmeten.Helper;
using Com.Meten.WeChatSmeten.Web.Data;
using Common.Logging;

namespace Com.Meten.WeChatSmeten.Web.Business
{
    public class MediaHelper
    {
        /// <summary>
        /// 上传多媒体文件,返回 MediaId
        /// </summary>
        /// <param name="mType"></param>
        /// <param name="vpath"></param>
        /// <returns></returns>
        public static string UploadMedia(MediaType mType, string vpath)
        {
            try
            {
                if (string.IsNullOrEmpty(CommonData.UploadMediaFileUrl))
                {
                    throw new Exception("UploadMedia方法。。缺少上传多媒体文件路径配置或者该配置为空，无法继续上传");
                }
                string wxurl = string.Format(CommonData.UploadMediaFileUrl,CommonData.AccessToken, mType.ToString());
                string filepath = HttpContext.Current.Server.MapPath("~") + vpath; //(本地服务器的地址)
                LogHelper.FileLog.Debug("UploadMedia方法。。上传路径:" + filepath);
                string result = HttpHelper.UploadFile(wxurl, "post", filepath);
                LogHelper.FileLog.Debug("UploadMedia方法。。上传result:" + result);
                return result;
            }
            catch (Exception ex)
            {
                LogHelper.FileLog.Fatal(string.Format("UploadMedia方法。。上传文件发生错误！AT:{0};Type:{1};vpath:{2};错误信息:{3}", CommonData.AccessToken,
                    mType, vpath, ex.Message));
                throw ex;
            }
        }
        /// <summary>
        /// 下载多媒体文件
        /// </summary>
        /// <param name="media_id"></param>
        /// <param name="fileExt">后缀</param>
        /// <returns></returns>
        public static string DownLoadMedia(string media_id, string fileExt)
        {
            try
            {
                //WebClient webClient = new WebClient();
                if (string.IsNullOrEmpty(CommonData.DownloadMediaFileUrl))
                {
                    throw new Exception("DownLoadMedia方法。。缺少下载多媒体文件路径配置或者该配置为空，无法继续下载");
                }
                string wxurl = string.Format(CommonData.DownloadMediaFileUrl, CommonData.AccessToken, media_id);
                string downloadPath = HttpContext.Current.Server.MapPath("~") + CommonData.DownloadFilePath;
                if (!Directory.Exists(downloadPath))
                    Directory.CreateDirectory(downloadPath);
                //webClient.DownloadFile(wxurl, downloadPath + media_id + fileExt.ToLower());
                HttpHelper.DownloadFile(wxurl, downloadPath + media_id + fileExt.ToLower());
                LogHelper.FileLog.Debug(string.Format("DownLoadMedia方法。。下载文件成功，保存地址为:{0}", downloadPath + media_id + fileExt));
                return CommonData.DownloadFilePath + media_id + fileExt;
            }
            catch (Exception ex)
            {
                LogHelper.FileLog.Fatal(string.Format("DownLoadMedia方法。。下载文件失败，错误信息:{0}" + ex.Message));
                throw ex;
            }
        }


        /// <summary>
        /// 获取素材列表文件
        /// </summary>
        /// <returns></returns>
        public static string GetMaterialList(MediaType mType,int pageNow=1,int pageSize=20)
        {
            try
            {
                if (string.IsNullOrEmpty(CommonData.GetMaterialListUrl))
                {
                    throw new Exception("GetMaterialList方法。。缺少下载多媒体文件路径配置或者该配置为空，无法继续下载");
                }
                string wxurl = string.Format(CommonData.GetMaterialListUrl, CommonData.AccessToken);


                Dictionary<string, object> pa = new Dictionary<string, object>();
                pa.Add("type", mType.ToString().ToLower());
                pa.Add("offset", (pageNow-1)*pageSize);
                pa.Add("count", pageSize);
                JavaScriptSerializer js = new JavaScriptSerializer();
                string jssTr = js.Serialize(pa);
                string result = HttpHelper.GetHTTPPost(wxurl, jssTr);
   SortedDictionary<string, object> sdResult =
                        js.Deserialize<SortedDictionary<string, object>>(result);
                if (sdResult != null)
                {
                    if (sdResult.ContainsKey("errcode")) //有错误
                    {
                        throw new ApplicationException(sdResult.ContainsKey("errcode")
                            ? "获取素材列表发生错误；代码为：" + sdResult["errcode"] +
                              (sdResult.ContainsKey("errmsg") ? "；错误描述为：" + sdResult["errmsg"] : "")
                            : "");
                    }
                    if (mType == MediaType.News) //多媒体素材
                    {
                        NewsMaterial nm =js.Deserialize<NewsMaterial>(result);
                        if (nm != null)
                        {
                            LogHelper.WriteInfo(string.Format("total_count:{0};item_count:{1}",nm.total_count,nm.item_count));
                            //foreach (ArticleItem item in nm.item)
                            //{
                            //    GetMaterialDetail(item.media_id, mType, "");
                            //}
                        }
                    }
                    else
                    {
                        Material material = js.Deserialize<Material>(result);
                        if (material != null)
                        {
                            LogHelper.WriteInfo(string.Format("total_count:{0};item_count:{1}", material.total_count, material.item_count));
                            foreach (MaterialItem item in material.item)
                            {
                                GetMaterialDetail(item.media_id, mType);
                            }
                        }
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                LogHelper.FileLog.Fatal(string.Format("DownLoadMedia方法。。下载文件失败，错误信息:{0}" + ex.Message));
                throw ex;
            }
        }

        public static string GetMaterialDetail(string media_id, MediaType mType)
        {
            try
            {
                string fileExt = ".jpg";
                switch (mType)
                {
                    case MediaType.Image:
                        break;
                    case MediaType.News:
                        break;
                    case MediaType.Thumb:
                        break;
                    case MediaType.Video:
                        fileExt = ".mp4";
                        break;
                    case MediaType.Voice:
                        fileExt = ".mp3";
                        break;
                }
                if (string.IsNullOrEmpty(CommonData.GetMaterialDetailUrl))
                {
                    throw new Exception("GetMaterialDetail方法。。缺少下载多媒体文件路径配置或者该配置为空，无法继续下载");
                }
                string wxurl = string.Format(CommonData.GetMaterialDetailUrl, CommonData.AccessToken);


                Dictionary<string, object> pa = new Dictionary<string, object>();
                pa.Add("media_id", media_id);
                JavaScriptSerializer js = new JavaScriptSerializer();
                string jssTr = js.Serialize(pa);

                if (mType == MediaType.News) //多媒体素材
                {
                    string result = HttpHelper.GetHTTPPost(wxurl, jssTr);
                    SortedDictionary<string, object> sdResult =
                        js.Deserialize<SortedDictionary<string, object>>(result);
                    if (sdResult != null)
                    {
                        if (sdResult.ContainsKey("errcode")) //有错误
                        {
                            throw new ApplicationException(sdResult.ContainsKey("errcode")
                                ? "获取素材详情发生错误；代码为：" + sdResult["errcode"] +
                                  (sdResult.ContainsKey("errmsg") ? "；错误描述为：" + sdResult["errmsg"] : "")
                                : "");
                        }
                    }
                    PictureArticleItem nm = js.Deserialize<PictureArticleItem>(result);
                    if (nm != null)
                    {
                        LogHelper.WriteInfo(string.Format("count:{0}", nm.news_item.Count));
                    }
                }
                else
                {
                    string downloadPath = HttpContext.Current.Server.MapPath("~") + CommonData.DownloadFilePath + media_id + fileExt;
                    if (!Directory.Exists(HttpContext.Current.Server.MapPath("~") + CommonData.DownloadFilePath))
                    {
                        Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~") + CommonData.DownloadFilePath);
                    }
                    HttpHelper.DownloadFilePost(wxurl, downloadPath, jssTr);
                    return CommonData.DownloadFilePath + media_id + fileExt;
                }
                return "";
            }
            catch (Exception ex)
            {
                LogHelper.FileLog.Fatal(string.Format("GetMaterialDetail方法。。获取素材文件失败，错误信息:{0}" + ex.Message));
                throw ex;
            }
        }
    }
}