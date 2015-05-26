<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OAuthLogin.aspx.cs" Inherits="Com.Meten.WeChatSmeten.Web.OAuthLogin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<meta name="viewport" content="width=device-width,height=device-height,inital-scale=1.0,maximum-scale=1.0,user-scalable=no;" />
    <title>点击使用微信登录</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:HyperLink ID="hlToWeixin" runat="server" ImageUrl="~/img/icon48_wx_button.png"></asp:HyperLink>
    </div>
    </form>
</body>
</html>
