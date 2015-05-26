using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Com.Meten.WeChatSmeten.Entities;
using Com.Meten.WeChatSmeten.Web.Data;

namespace Com.Meten.WeChatSmeten.Web
{
    public partial class OAuthLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            hlToWeixin.NavigateUrl = CommonData.GetOAuthByScopeType(ScopeType.snsapi_userinfo);
        }
    }
}