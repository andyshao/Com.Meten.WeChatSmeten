using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Com.Meten.WeChatSmeten.Helper;
using Com.Meten.WeChatSmeten.Web.Business;
using Com.Meten.WeChatSmeten.Web.Data;

namespace Com.Meten.WeChatSmeten.Web
{
    public partial class GetInfos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblAccessT.Text = CommonData.AccessToken;
                lblIPs.Text = CommonData.WeiXinIp;
            }
            catch (Exception exception)
            {
                //LogHelper.WriteError("页面发生异常" + exception.Message);
                Response.Write("获取信息错误！详细请查看日志文件。" + exception.Message);
            }
        }
    }
}