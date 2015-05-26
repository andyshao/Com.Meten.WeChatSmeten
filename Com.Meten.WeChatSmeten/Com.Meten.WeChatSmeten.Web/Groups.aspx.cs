using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
    public partial class Groups : System.Web.UI.Page
    {
        protected int CountAll = 0;

        private const string systemGroup = "0,1,2";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadGroups();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string result = "";
            try
            {
                string gname = this.TextBox1.Text.Trim();
                if (string.IsNullOrEmpty(gname))
                {
                    Response.Write("请输入分组名称");
                    return;
                }

                string id = UserGroupHepler.SaveGroup(gname);
                if (id == "0")
                {
                    result = "创建失败，详细信息请查看日志文件。";
                }
                else
                {
                    result = "创建成功；返回ID为：" + id;
                    LoadGroups();
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                LogHelper.WriteError(ex);
            }
            Response.Write(result);
        }

        private void LoadGroups()
        {
            List<Com.Meten.WeChatSmeten.Entities.UserGroup> listGroups = UserGroupHepler.GetGroups();
            CountAll = listGroups.Count;
            Repeater1.DataSource = listGroups;
            Repeater1.DataBind();
        }

        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "ChangName")
            {
                string id = e.CommandArgument.ToString();
                string result = "";
                try
                {
                    if (e.Item.FindControl("txtchangName") != null && e.Item.FindControl("oldName") != null)
                    {
                        TextBox tb = (TextBox) e.Item.FindControl("txtchangName");
                        if (tb != null && string.IsNullOrEmpty(tb.Text.Trim()))
                        {
                            throw new Exception("请输入分组名称！");
                        }
                        HiddenField hf = (HiddenField) e.Item.FindControl("oldName");
                        string newName = tb.Text.Trim();
                        string oldName = hf.Value.Trim();
                        if (newName == oldName)
                        {
                            throw new Exception("分组名称不能与当前名称相同！");
                        }
                        id = UserGroupHepler.SaveGroup(newName,id);
                        if (id == "0")
                        {
                            result = "修改失败，详细信息请查看日志文件。";
                        }
                        else
                        {
                            result = "修改成功";
                            LoadGroups();
                        }
                    }
                    else
                    {
                        throw new Exception("未找到相关控件");
                    }
                }
                catch (Exception ex)
                {
                    result = ex.Message;
                    LogHelper.WriteError(ex);
                }
                Response.Write(result);
            }
        }

        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header || e.Item.ItemType == ListItemType.Footer)
                return;
            if (e.Item.FindControl("btnChangeName") != null)
            {
                Button bt = (Button) e.Item.FindControl("btnChangeName");
                bt.CommandArgument = ((UserGroup) e.Item.DataItem).Id;
            }

            if (e.Item.FindControl("oldName") != null)
            {
                HiddenField hf = (HiddenField) e.Item.FindControl("oldName");
                hf.Value = ((UserGroup)e.Item.DataItem).Name;
            }
            
            //系统分组 不能修改
            if (systemGroup.Split(',').Contains(((UserGroup)e.Item.DataItem).Id))
            {
                if (e.Item.FindControl("btnChangeName") != null)
                {
                    Button bt = (Button) e.Item.FindControl("btnChangeName");
                    bt.Visible = false;
                }

                if (e.Item.FindControl("txtchangName") != null)
                {
                    TextBox tb = (TextBox) e.Item.FindControl("txtchangName");
                    tb.Visible = false;
                }
            }
        }
    }
}