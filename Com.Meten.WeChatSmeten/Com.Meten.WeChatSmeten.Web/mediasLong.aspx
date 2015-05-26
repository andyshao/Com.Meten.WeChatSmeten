<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mediasLong.aspx.cs" Inherits="Com.Meten.WeChatSmeten.Web.mediasLong" %>
<%@ Import Namespace="Com.Meten.WeChatSmeten.Entities" %>
<%@ Import Namespace="Com.Meten.WeChatSmeten.Web.Business" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>长期素材</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        
        <%= MediaHelper.GetMaterialList(MediaType.Video,1,1) %>
    </div>
    </form>
</body>
</html>
