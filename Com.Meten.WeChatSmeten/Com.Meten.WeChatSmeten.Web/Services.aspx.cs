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
    public partial class Services : System.Web.UI.Page
    {
        protected readonly string ext = "@smeten";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;
            BindCustomService();
        }

        private void BindCustomService()
        {
            List<CustomService> list = CustomServiceHelper.GetCustomServicesList();
            Repeater1.DataSource = list;
            Repeater1.DataBind();
        }

        protected void btn_Sure_Click(object sender, EventArgs e)
        {
            try
            {
                string account = this.txt_account.Text;
                string nickname = this.txt_nick.Text;
                string password = this.txt_password.Text;
                if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(nickname) || (IsAdd.Value != "3" && string.IsNullOrEmpty(password)))
                {
                    Response.Write("请同时输入账号、昵称以及密码");
                    return;
                }

                if (CustomServiceHelper.SaveServiceAccount(account, nickname, password, Convert.ToInt32(IsAdd.Value),ext))
                {
                    string headResult = "";
                    //判断是否有图片上传。。。。如果有，则更新
                    if (headPicture.HasFile)
                    {
                        string fileName = headPicture.PostedFile.FileName;
                        double fileSize = (double)headPicture.PostedFile.ContentLength / (1024 * 1024);
                        string vpath = CommonData.UploadFilePath + DateTime.Now.ToString("yyyyMMddHHmmssfff") + fileName;
                        string filepath = Server.MapPath("~") + vpath;
                        LogHelper.FileLog.Info(filepath);
                        if (!Directory.Exists(filepath.Substring(0, filepath.LastIndexOf("\\"))))
                            Directory.CreateDirectory(filepath.Substring(0, filepath.LastIndexOf("\\")));
                        string file_ext = fileName.Substring(fileName.LastIndexOf(".")).ToUpper(); //后缀名
                        if (file_ext != ".JPG")
                        {
                            headResult = "头像更新失败，请上传少于2M且格式为JPG的图片文件";
                        }
                        else
                        {
                            if (fileSize > 2) //小于2M
                            {
                                headResult = "头像更新失败，请上传少于2M且格式为JPG的图片文件";
                            }
                        }
                        if (string.IsNullOrEmpty(headResult))
                        {
                            //保存
                            headPicture.PostedFile.SaveAs(filepath);
                            //传到 微信服务器上
                            headResult = CustomServiceHelper.SaveServiceAccountHeadImg(account + ext, vpath);
                        }

                        if (string.IsNullOrEmpty(headResult))
                        {
                            headResult = "头像更新成功";
                        }
                        else
                        {
                            headResult = "头像更新失败；" + headResult;
                        }
                        Response.Write("操作成功！" + headResult);
                    }
                    else
                    {
                        Response.Write("操作成功！");
                        return;
                    }
                }
                else
                {
                    Response.Write("操作失败，详细信息请查看日志！");
                }
            }
            catch (Exception ex)
            {
                Response.Write("操作发生异常！" + ex.Message);
            }
            BindCustomService();
        }
    }
}