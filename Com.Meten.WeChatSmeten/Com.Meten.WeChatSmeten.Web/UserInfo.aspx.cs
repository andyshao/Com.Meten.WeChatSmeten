using System;
using System.Collections.Generic;
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
    public partial class UserInfo : System.Web.UI.Page
    {
        protected UserGroup UGroup
        {
            set { ViewState[" WeiXinDIYGroup"] = value; }
            get
            {
                if (ViewState[" WeiXinDIYGroup"] != null)
                {
                    return (UserGroup)ViewState[" WeiXinDIYGroup"];
                }
                return null;
            }
        }

        protected string open_id
        {
            set { ViewState[" open_id"] = value; }
            get
            {
                if (ViewState[" open_id"] != null)
                {
                    return ViewState[" open_id"].ToString();
                }
                return null;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                GetUserInfo();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string id = DropDownList1.SelectedValue;
            if (id == UGroup.Id)
            {
                Response.Write("用户已经在该组<br/><br/><br/><br/>");
            }
            else
            {
                if (UserHelper.MoveUserGroup(open_id, id))
                {
                    Response.Write("移动成功！<br/><br/><br/><br/>");
                }
                else
                {
                    Response.Write("移动失败，详情请查看日志！<br/><br/><br/><br/>");
                }
            }
            GetUserInfo();
        }

        private void GetUserInfo()
        {
            try
            {
                User u = UserHelper.GetUserInfo(Request.QueryString["open_id"]);
                if (u == null)
                {
                    throw new Exception();
                }
                string result = "";
                if (u.subscribe == "1")
                {
                    UGroup = UserHelper.GetUserGroup(open_id);
                    result = string.Format(@"用户是否订阅该公众号：{0}；<br/>openid：{1}；<br/>
用户的昵称：{2}；<br/>性别：{3}；<br/>所在城市(国家-省-城市)：{4}-{5}-{6}；<br/>所使用的语言：{7}；<br/>头像地址：{8}；<br/>关注时间：{9}；<br/>unionid：{10}；<br/>所在分组：{11}。",
                        u.subscribe == "1" ? "是" : "否", u.openid, u.nickname,
                        u.sex == "1" ? "男" : (u.sex == "2" ? "女" : "未知"), u.country, u.province, u.city, u.language,
                        u.headimgurl,
                        TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)).Add(new TimeSpan(Convert.ToInt64(u.subscribe_time + "0000000"))).ToString("yyyy年MM月dd日 HH时mm分ss秒"), string.IsNullOrEmpty(u.unionid) ? "无" : u.unionid,
                        UGroup != null ? UGroup.Name : "");
                    headimg.ImageUrl = u.headimgurl;

                    if (!IsPostBack)
                    {
                        List<UserGroup> listGroups = UserGroupHepler.GetGroups();
                        DropDownList1.DataSource = listGroups;
                        DropDownList1.DataTextField = "Name";
                        DropDownList1.DataValueField = "Id";
                        DropDownList1.DataBind();
                        if (UGroup != null)
                            DropDownList1.SelectedValue = UGroup.Id;
                    }
                }
                else
                {
                    result = "用户未关注，无法获取信息";
                    headimg.Visible = false;
                }
                LogHelper.WriteInfo(result);
                Response.Write(result);
            }
            catch (Exception)
            {
                Response.Write("未能获取到用户信息，详情请查看日志文件！");
            }
        }

        protected void btn_SetRemark_Click(object sender, EventArgs e)
        {
            string Sremark = remark.Text.Trim();

            if (string.IsNullOrEmpty(Sremark))
            {
                Response.Write("请输入备注名称<br/><br/><br/><br/>");
            }
            else
            {
                if (UserHelper.SetRemark(open_id, Sremark))
                {
                    Response.Write("设置备注成功！<br/><br/><br/><br/>");
                }
                else
                {
                    Response.Write("设置备注失败，详情请查看日志！<br/><br/><br/><br/>");
                }
            }
            GetUserInfo();
        }
    }
}