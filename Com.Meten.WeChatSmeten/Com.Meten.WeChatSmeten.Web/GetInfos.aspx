<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetInfos.aspx.cs" Inherits="Com.Meten.WeChatSmeten.Web.GetInfos" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>AccessToken与微信服务器ip地址</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        Access Token： <br /><asp:Label ID="lblAccessT" runat="server" Text=""></asp:Label>
        
        <br />
        <br />
        <br />
        微信IP地址列表： <br /><asp:Label ID="lblIPs" runat="server" Text=""></asp:Label>
    </div>
    </form>
</body>
</html>
