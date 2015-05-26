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
    public partial class OAuthLoginReturn : System.Web.UI.Page
    {
        protected OAuthAccessToken at
        {
            get
            {
                if (ViewState["OAuthAccessToken"] == null)
                {
                    return null;
                }
                return (OAuthAccessToken) ViewState["OAuthAccessToken"];
            }
            set { ViewState["OAuthAccessToken"] = value; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            string code = Request.QueryString["code"];
            string state = Request.QueryString["state"];
            Response.Write(string.Format("授权返回成功.<br/>Code is :{0}<br/>state is :{1} <br/>", code, state));
            Image1.Visible = false;
            at = OAuthHelper.GetAccessTokenByCode(code);

            if (at != null)
            {
                Response.Write(string.Format(@"<br/><br/><br/><br/>获取AccessToken返回信息如下<br/>access_token:{0} <br/>expires_in:{1}秒 <br/>openid:{2}<br/>refresh_token:{3}<br/>Scope:{4}<br/>", at.access_token, at.expires_in, at.openid, at.refresh_token, at.scope));
                if (OAuthHelper.VerifyAccessToken(at))
                {
                    Response.Write(@"<br/><br/><br/><br/>AccessToken有效");
                    User u = OAuthHelper.GetUserInfoByAccessToken(at);
                    if (u != null)
                    {
                        u.privilege=new List<UserPrivilege>();
                        u.privilege.Add(UserPrivilege.chinaunicom);
                        u.privilege.Add(UserPrivilege.chinaMoblie);
                        Response.Write(string.Format(@"<br/><br/><br/><br/>根据AccessToken获取用户的信息如下：<br/><br/>openid(用户的唯一标识)：{0}<br/><br/>nickname(用户昵称)：{1}<br/><br/>sex(性别1男2女)：{2}<br/><br/>province(所在省)：{3}<br/><br/>city(所在城市)：{4}<br/><br/>country(所在国家)：{5}<br/><br/>headimgurl(头像)：{6}<br/><br/>privilege(用户特权标识)：{7}<br/><br/>unionid(用户unionid)：{8}<br/><br/>", u.openid, u.nickname, u.sex, u.province, u.city, u.country, u.headimgurl, string.Join(",", u.privilege), u.unionid));
                        Image1.Visible = true;
                        Image1.ImageUrl = u.headimgurl;
                        Image1.Width = 64;
                        Image1.Height = 64;
                    }
                    else
                    {
                        Response.Write(@"<br/><br/><br/><br/>根据AccessToken获取用户信息失败；详情请查看日志文件。<br/><br/>");
                    }
                    at = OAuthHelper.RefreshAccessToken(at);
                    if (at != null)
                    {
                        Response.Write(string.Format(@"<br/><br/><br/><br/>刷新AccessToken成功；返回信息如下<br/>access_token:{0} <br/>expires_in:{1}秒 <br/>openid:{2}<br/>refresh_token:{3}<br/>Scope:{4}<br/>", at.access_token, at.expires_in, at.openid, at.refresh_token, at.scope));
                    }
                    else
                    {
                        Response.Write(@"<br/><br/><br/><br/>刷新AccessToken失败；详情请查看日志文件。<br/><br/>");
                    }
                }
                else
                {
                    Response.Write(@"<br/><br/><br/><br/>AccessToken无效；无法获取个人信息。<br/><br/>");
                }
            }
        }
    }
}