using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Com.Meten.WeChatSmeten.Entities;
using Com.Meten.WeChatSmeten.Helper;
using Com.Meten.WeChatSmeten.Web.Business;
using Com.Meten.WeChatSmeten.Web.Data;

namespace Com.Meten.WeChatSmeten.Web
{
    public partial class medias : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }



        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (FileUpload1.HasFile)
                {
                    ListItem li = DropDownList1.SelectedItem;
                    //<asp:ListItem Value="image">image（图片）JPG图片 max 1M</asp:ListItem>
                    //<asp:ListItem Value="voice">voice（音乐）AMR或MP3音频 max 2M</asp:ListItem>
                    //<asp:ListItem Value="video">video（视频）MP4视频 max 10M</asp:ListItem>
                    //<asp:ListItem Value="thumb">thumb（缩略图）JPG图片 max 64K</asp:ListItem>

                    string fileName = FileUpload1.PostedFile.FileName;
                    double fileSize = (double) FileUpload1.PostedFile.ContentLength/(1024*1024);
                    string uploadResult = "";
                    string vpath = CommonData.UploadFilePath + DateTime.Now.ToString("yyyyMMddHHmmssfff") + fileName;
                    string filepath = Server.MapPath("~") + vpath;
                    LogHelper.FileLog.Info(filepath);
                    if (!Directory.Exists(filepath.Substring(0, filepath.LastIndexOf("\\"))))
                        Directory.CreateDirectory(filepath.Substring(0, filepath.LastIndexOf("\\")));
                    MediaType mt = MediaType.Image;
                    string ext = fileName.Substring(fileName.LastIndexOf(".")).ToUpper(); //后缀名
                    switch (li.Value.ToUpper())
                    {
                        case "IMAGE":
                            if (!(fileSize < 1)) //小于1M
                            {
                                throw new Exception("文件大小超过1M,微信服务器允许上传该类型最大的大小限制为1M。");
                            }
                            if (ext != ".JPG")
                            {
                                throw new Exception("请上传JPG的图片文件,微信服务器仅支持JPG格式的图片。" + ext);
                            }
                            break;
                        case "VOICE":
                            mt = MediaType.Voice;
                            if (!(fileSize < 2)) //小于2M
                            {
                                throw new Exception("文件大小超过2M,微信服务器允许上传该类型最大的大小限制为2M。");
                            }
                            if (ext != ".MP3" && ext != ".ARM")
                            {
                                throw new Exception("请上传mp3或者arm的音频文件,微信服务器仅支持mp3与arm格式的音频文件。" + ext);
                            }
                            break;
                        case "VIDEO":
                            mt = MediaType.Video;
                            if (!(fileSize < 10)) //小于10M
                            {
                                throw new Exception("文件大小超过10M,微信服务器允许上传该类型最大的大小限制为10M。");
                            }
                            if (ext != ".MP4")
                            {
                                throw new Exception("请上传mp4的视频文件,微信服务器仅支持mp4的视频文件。" + ext);
                            }
                            break;
                        case "THUMB":
                            //
                            mt = MediaType.Thumb;
                            fileSize = (double) FileUpload1.PostedFile.ContentLength/1024;
                            if (!(fileSize < 64))
                            {
                                throw new Exception("文件大小超过64Kb,微信服务器允许上传该类型最大的大小限制为64Kb。");
                            }
                            if (ext != ".JPG")
                            {
                                throw new Exception("请上传JPG的图片文件,微信服务器仅支持JPG格式的图片。" + ext);
                            }
                            break;
                    }

                    //保存
                    FileUpload1.PostedFile.SaveAs(filepath);
                    //传到 微信服务器上
                    uploadResult = MediaHelper.UploadMedia(mt, vpath);

                    JavaScriptSerializer js = new JavaScriptSerializer();
                    SortedDictionary<string, string> sdResult =
                        js.Deserialize<SortedDictionary<string, string>>(uploadResult);
                    if (sdResult != null)
                    {
                        if (sdResult.ContainsKey("errcode")) //有错误
                        {
                            string emsg = sdResult.ContainsKey("errcode")
                                ? "错误代码为：" + sdResult["errcode"] +
                                  (sdResult.ContainsKey("errmsg") ? "；错误描述为：" + sdResult["errmsg"] : "")
                                : "";
                            LogHelper.FileLog.Error(emsg);
                            throw new ApplicationException(emsg);
                        }
                        else
                        {
                            //上传成功
                            string media_id = sdResult["media_id"];
                            string str = "上传成功，ID为：" + media_id;
                            LogHelper.FileLog.Debug(str);
                            Response.Write(str + "<br/>");

                            if (li.Value.ToUpper() == "VIDEO")
                            {
                                str = "视频类型的多媒体不支持下载。。";
                            }
                            else
                            {
                                //上传后立马下载。。。。
                                string dloadPath = MediaHelper.DownLoadMedia(media_id, ext.ToLower());
                                str = "下载成功，下载后保存地址为：" + dloadPath;
                            }
                            LogHelper.FileLog.Debug(str);
                            Response.Write(str + "<br/>");
                        }
                    }
                    else
                    {
                        throw new Exception("微信服务器未返回相关信息，上传失败！路径为：" + filepath);
                    }
                }
                else
                {
                    Response.Write("请选择文件");
                    return;
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
                LogHelper.FileLog.Fatal(ex);
            }
        }
    }
}