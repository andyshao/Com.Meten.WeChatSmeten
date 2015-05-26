<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Users.aspx.cs" Inherits="Com.Meten.WeChatSmeten.Web.Users" %>
<%@ Import Namespace="System.Activities.Statements" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>用户列表</title>
    <script src="js/jquery-1.8.2.min.js" language="javascript"></script>
    <script language="javascript" type="text/javascript">
        $(function() {
            $("#<%= Button1.ClientID%>").click(function () {
                var selID = "";
                $(".checkcss input[type='checkbox']").each(function () {
                    if ($(this).attr("checked")) {
                        if (selID != "")
                            selID += ",";
                        selID += $(this).parent().next().val();
                    }
                });
                //alert(selID);
                if (selID == "") {
                    alert("请至少选择一个用户");
                    return false;
                } else {
                    $("#<%=hfChecked.ClientID %>").val(selID);
                    return true;
                }
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <P>翻页的信息放在这里：<%= PageInfo %></P>
        <p>总数：<%=CountAll %>；当前获取的数量：<%= CoubtNow %></p>
        <br>
        <br>
        所有分组
        <asp:DropDownList ID="DropDownList1" runat="server"></asp:DropDownList>
        <asp:Button ID="Button1" runat="server"  Text="将所选用户移动到该分组" OnClick="Button1_Click" />
                <asp:HiddenField runat="server" ID="hfChecked" />
        <br>
        <br>
                <asp:Repeater ID="Repeater1" runat="server"  OnItemCommand="Repeater1_ItemCommand" OnItemDataBound="Repeater1_ItemDataBound">
            <HeaderTemplate></HeaderTemplate>
            <ItemTemplate>
                用户OPENID：<a href="UserInfo.aspx?open_id=<%# Container.DataItem.ToString() %>" target="_blank" title="点击查看该用户详细信息"><%# Container.DataItem.ToString() %></a>
                <asp:CheckBox runat="server" ID="check" CssClass="checkcss" />
                <asp:HiddenField runat="server" ID="hvalue" />
                <br/>
            </ItemTemplate>
            <FooterTemplate></FooterTemplate>
        </asp:Repeater>
        
    </div>
    </form>
</body>
</html>
