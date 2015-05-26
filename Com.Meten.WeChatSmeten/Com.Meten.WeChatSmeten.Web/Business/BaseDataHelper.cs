using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Com.Meten.WeChatSmeten.Entities;

namespace Com.Meten.WeChatSmeten.Web.Business
{
    public class BaseDataHelper
    {
        /// <summary>
        /// 枚举转换成IList
        /// </summary>
        /// <returns></returns>
        public static IList ListTypeForEnum(Type enumType)
        {
            ArrayList list = new ArrayList();
            foreach (int i in Enum.GetValues(enumType))
            {
                ListItem listitem = new ListItem(Enum.GetName(enumType, i), i.ToString());
                list.Add(listitem);
            }
            return list;
        }

        /// <summary>
        /// 绑定某个枚举到下拉列表
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="enumType"></param>
        /// <param name="defaultSel"></param>
        public static void BindEnumToDropDownList(DropDownList ddl, Type enumType, string defaultSel = "")
        {
            ddl.DataSource = BaseDataHelper.ListTypeForEnum(enumType);
            ddl.DataValueField = "value";
            ddl.DataTextField = "text";
            ddl.DataBind();
            if (string.IsNullOrEmpty(defaultSel)) return;
            //ddl.SelectedValue = defaultSel;
            //ddl.SelectedValue = ddl.Items.Cast<ListItem>().Where(item => item.Text == defaultSel).FirstOrDefault().Value;
            foreach (ListItem item in ddl.Items.Cast<ListItem>().Where(item => item.Text == defaultSel))
            {
                ddl.SelectedValue = item.Value;
                break;
            }
        }
    }
}