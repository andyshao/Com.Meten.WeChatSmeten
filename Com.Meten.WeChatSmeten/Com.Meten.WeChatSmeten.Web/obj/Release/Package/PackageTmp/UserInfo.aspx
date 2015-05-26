<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserInfo.aspx.cs" Inherits="Com.Meten.WeChatSmeten.Web.UserInfo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Image ID="headimg" runat="server"   />
        <br>
        <br>
        <br>
        所有分组
        <asp:DropDownList ID="DropDownList1" runat="server"></asp:DropDownList>
        <asp:Button ID="Button1" runat="server"  Text="移动到该分组" OnClick="Button1_Click" />
        <br>
        <br>
        设置该用户备注
        <asp:TextBox runat="server" ID="remark" ></asp:TextBox>
        <asp:Button ID="btn_SetRemark" runat="server"  Text="确认设置该备注" OnClick="btn_SetRemark_Click" />
    </div>
    </form>
</body>
</html>
