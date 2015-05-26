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
    public partial class Users : System.Web.UI.Page
    {
        protected string PageInfo = "";

        protected int CoubtNow
        {
            set { ViewState["CoubtNow"] = value; }
            get
            {
                return ViewState["CoubtNow"] == null
                    ? 0
                    : Convert.ToInt32(ViewState["CoubtNow"].ToString());
            }
        }

        protected int CountAll 
        {
            set { ViewState["CountAll"] = value; }
            get { return ViewState["CountAll"] == null ? 0 : Convert.ToInt32(ViewState["CountAll"].ToString()); }
        }

        private string next_openid
        {
            set { ViewState["next_openid"] = value; }
            get { return ViewState["next_openid"] == null ? "" : ViewState["next_openid"].ToString(); }
        }



        protected void Page_Load(object sender, EventArgs e)
        {
            if(IsPostBack)
                return;
            GetUser();
        }



        protected void Button1_Click(object sender, EventArgs e)
        {
            string id = DropDownList1.SelectedValue;

            string openids = hfChecked.Value;
            if (string.IsNullOrEmpty(openids))
            {
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "error", "<script>alert('请至少选择一个用户。');</script>");
                return;
            }
            List<string> opeList = openids.Split(',').ToList();
            opeList.RemoveAll(d => d == "");
            if (UserHelper.MoveUserGroup(opeList, id))
            {
                Response.Write("移动成功！<br/><br/><br/><br/>");
            }
            else
            {
                Response.Write("移动失败，详情请查看日志！<br/><br/><br/><br/>");
            }
        }


        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            //throw new NotImplementedException();
        }

        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header || e.Item.ItemType == ListItemType.Footer)
                return;
            if (e.Item.FindControl("hvalue") != null)
            {
                HiddenField hf = (HiddenField) e.Item.FindControl("hvalue");
                hf.Value = e.Item.DataItem.ToString();
            }
        }


        private void GetUser()
        {
            string url = string.Format(CommonData.GetUserListUrl, CommonData.AccessToken, next_openid);
            string result = HttpHelper.GetHtml(url);
            LogHelper.WriteInfo(result);
            LogHelper.WriteInfo("i");
            LogHelper.WriteWarn("w");
            LogHelper.WriteDebug("d");
            LogHelper.WriteFatal("F");
            LogHelper.WriteError("e");
            LogHelper.FileLog.Debug("f");
            LogHelper.FileLog.Error("f");
            LogHelper.FileLog.Fatal("f");
            LogHelper.FileLog.Info("f");
            LogHelper.FileLog.Warn("f");
            JavaScriptSerializer js = new JavaScriptSerializer();
            SortedDictionary<string, object> sdResult =
                js.Deserialize<SortedDictionary<string, object>>(result);
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
                    CountAll = Convert.ToInt32(sdResult["total"]);
                    CoubtNow = Convert.ToInt32(sdResult["count"]);

                    //PageInfo
                    if (sdResult["total"].ToString() == sdResult["count"].ToString()) //只有一页。关注用户少于10000
                    {
                        PageInfo = "关注用户未超过10000，无分页。";
                    }
                    else
                    {
                        //大于1页
                        PageInfo = "具体的分页信息";
                    }
                    Dictionary<string, object> dobj = (Dictionary<string, object>)sdResult["data"];
                    object[] aa = (object[])(dobj["openid"]);
                    Repeater1.DataSource = aa;
                    Repeater1.DataBind();
                }
            }
            List<UserGroup> listGroups = UserGroupHepler.GetGroups();
            DropDownList1.DataSource = listGroups;
            DropDownList1.DataTextField = "Name";
            DropDownList1.DataValueField = "Id";
            DropDownList1.DataBind();
        }
    
    }
}