<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="Groups.aspx.cs" Inherits="Com.Meten.WeChatSmeten.Web.Groups" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>自定义用户分组</title>
    <style type="text/css">
        .auto-style1 { height: 19px; }
        .auto-style2 { width: 202px; }
        .auto-style3 { width: 231px; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <p>总数：<%=CountAll %></p>
            <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand" OnItemDataBound="Repeater1_ItemDataBound">
                <HeaderTemplate>
                    <table>
                        <tr>
                            <th colspan="3">群组信息：</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>ID:&nbsp;&nbsp;<%# Eval("id") %>&nbsp;&nbsp;&nbsp;&nbsp;
                    名称:&nbsp;&nbsp;<%# Eval("name") %>&nbsp;&nbsp;&nbsp;&nbsp;
                    组内人数:&nbsp;&nbsp;<%# Eval("count") %><br />
                        </td>
                        <td>
                            <asp:TextBox ID="txtchangName" runat="server"></asp:TextBox>
                            <asp:HiddenField ID="oldName" runat="server" />
                        </td>
                        <td>
                            <asp:Button ID="btnChangeName" runat="server" CommandName="ChangName" Text="改名" />
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    <table>
                </FooterTemplate>
            </asp:Repeater>
            <br />
            <br />
            <br />
            <br />
            <table style="width: 100%;">
                <tr>
                    <td class="auto-style1" colspan="2">创建新分组</td>
                    <td class="auto-style1"></td>
                </tr>
                <tr>
                    <td class="auto-style2">分组名称：</td>
                    <td class="auto-style3">
                        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style2" colspan="2">
                        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="确定" />
                    </td>

                    <td>&nbsp;</td>
                </tr>
            </table>
            <br />
        </div>

    </form>
</body>
</html>
