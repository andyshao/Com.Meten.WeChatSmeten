using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Com.Meten.WeChatSmeten.Entities;
using Com.Meten.WeChatSmeten.Web.Business;

namespace Com.Meten.WeChatSmeten.Web
{
    public partial class Menus : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindAllMenus();
            }
        }

        protected void btn_Sure_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hfFinalData.Value.Trim()))
            {
                //数据格式： 
                //  两个菜单之间用 <  
                //  菜单与子菜单之间用   >
                //  两个子菜单之间用  *
                //  菜单各项之间用 |
                //name|type|tkey|turl>sname|stype|stkey|sturl*sname|stype|stkey|sturl*sname|stype|stkey|sturl*sname|stype|stkey|sturl*sname|stype|stkey|sturl
                // <
                // name|type|tkey|turl>sname|stype|stkey|sturl*sname|stype|stkey|sturl*sname|stype|stkey|sturl*sname|stype|stkey|sturl*sname|stype|stkey|sturl
                List<UserButton> ubs = new List<UserButton>();
                string[] menus = hfFinalData.Value.Trim().Split('<'); //分割成多个菜单
                foreach (string s in menus)
                {
                    string[] onemenu = s.Split('>'); //分割主菜单与子菜单。  放在 0 1 下标的数组
                    if (onemenu.Length > 0)
                    {
                        //处理主菜单
                        UserButton ub = new UserButton();
                        string[] menuitems = onemenu[0].Split('|'); //分割主菜单各项
                        if (menuitems.Length > 3)
                        {
                            ub.name = menuitems[0];
                            ub.ubttype = (UserButtonType) Convert.ToInt32(menuitems[1]);
                            ub.type = ((UserButtonType) Convert.ToInt32(menuitems[1])).ToString();
                            ub.key = menuitems[2];
                            ub.url = menuitems[3];
                        }

                        //处理子菜单
                        if (onemenu.Length == 2 && (!string.IsNullOrEmpty(onemenu[1].Trim())))
                        {
                            List<UserButton> sububs = new List<UserButton>();
                            string[] submenus = onemenu[1].Trim().Split('*'); //分割子菜单
                            foreach (string submenu in submenus)
                            {
                                if (string.IsNullOrEmpty(submenu)) continue;
                                UserButton subub = new UserButton();
                                string[] submenusItems = submenu.Split('|'); //分割子菜单各项。。
                                if (submenusItems.Length > 3)
                                {
                                    subub.name = submenusItems[0];
                                    subub.ubttype = (UserButtonType) Convert.ToInt32(submenusItems[1]);
                                    subub.type = ((UserButtonType) Convert.ToInt32(submenusItems[1])).ToString();
                                    subub.key = submenusItems[2];
                                    subub.url = submenusItems[3];
                                    sububs.Add(subub);
                                }
                            }
                            if (sububs != null && sububs.Count > 0)
                            {
                                ub.sub_button = sububs;
                            }
                        }
                        ubs.Add(ub);
                    }
                }
                if (ubs.Count > 0)
                {
                    string result = MenuHelper.SaveButtons(ubs);
                    if (string.IsNullOrEmpty(result))
                    {
                        result = "保存成功！";
                        BindAllMenus();
                    }
                    Response.Write(result);
                }

            }
            else
            {
                Response.Write("请至少保留一个菜单！");
            }
        }

        protected void BindAllMenus()
        {
            List<UserButton> ubs = MenuHelper.GetAllMenus();
            this.Repeater1.DataSource = ubs;
            this.Repeater1.DataBind();
        }

        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header || e.Item.ItemType == ListItemType.Footer)
                return;
            UserButton ub = (UserButton) e.Item.DataItem;
            if (e.Item.FindControl("txt_name") != null)
            {
                TextBox tbName = (TextBox) e.Item.FindControl("txt_name");
                tbName.Text = ub.name;
            }
            if (e.Item.FindControl("ddl_type") != null)
            {
                DropDownList ddlType = (DropDownList) e.Item.FindControl("ddl_type");
                BaseDataHelper.BindEnumToDropDownList(ddlType, typeof (UserButtonType), ub.type);
            }

            if (e.Item.FindControl("txt_key") != null)
            {
                TextBox tbKey = (TextBox) e.Item.FindControl("txt_key");
                tbKey.Text = ub.key;
            }

            if (e.Item.FindControl("txt_url") != null)
            {
                TextBox tbUrl = (TextBox) e.Item.FindControl("txt_url");
                tbUrl.Text = ub.url;
            }


            if (e.Item.FindControl("sub_Repeater1") != null)
            {
                Repeater subRepeater = (Repeater) e.Item.FindControl("sub_Repeater1");
                subRepeater.ItemDataBound += subRepeater_ItemDataBound;
                subRepeater.DataSource = ub.sub_button;
                subRepeater.DataBind();
            }
        }

        private void subRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header || e.Item.ItemType == ListItemType.Footer)
                return;
            UserButton ub = (UserButton) e.Item.DataItem;
            if (e.Item.FindControl("txt_name") != null)
            {
                TextBox tbName = (TextBox) e.Item.FindControl("txt_name");
                tbName.Text = ub.name;
            }
            if (e.Item.FindControl("ddl_type") != null)
            {
                DropDownList ddlType = (DropDownList) e.Item.FindControl("ddl_type");
                BaseDataHelper.BindEnumToDropDownList(ddlType, typeof (UserButtonType), ub.type);
            }

            if (e.Item.FindControl("txt_key") != null)
            {
                TextBox tbKey = (TextBox) e.Item.FindControl("txt_key");
                tbKey.Text = ub.key;
            }

            if (e.Item.FindControl("txt_url") != null)
            {
                TextBox tbUrl = (TextBox) e.Item.FindControl("txt_url");
                tbUrl.Text = ub.url;
            }
        }

        protected void btn_Delete_Click(object sender, EventArgs e)
        {
            string result = "删除失败！";
            if (MenuHelper.DeleteButtons())
            {
                BindAllMenus();
                result = "删除成功！";
            }
            Response.Write(result);
        }
    }
}