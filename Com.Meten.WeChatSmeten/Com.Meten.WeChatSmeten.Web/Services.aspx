<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Services.aspx.cs" Inherits="Com.Meten.WeChatSmeten.Web.Services" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>客服人员</title>
    <style type="text/css">
        .auto-style1 { width: 94px; }
    </style>
    <script src="js/jquery-1.8.2.min.js" language="javascript"></script>
    <script type="text/javascript" language="javascript">
        $(function() {
            $("#Button1").hide();

            $("#Button1").click(function() {
                $("#<%= IsAdd.ClientID%>").val("1");
                $(this).hide();
                $("#<%=txt_account.ClientID %>").removeAttr("readonly");
                $("#<%=txt_nick.ClientID %>").removeAttr("readonly");
                // $("#<%=txt_password.ClientID %>").removeAttr("readonly");
                $("#<%=txt_account.ClientID %>").val("");
                $("#<%=txt_nick.ClientID %>").val("");
                $("#optTitle").html("添加");
            });


            $("#<%= btn_Sure.ClientID%>").click(function () {
                var filepath = $("#<%=headPicture.ClientID%>").val();
                if (filepath != "") {
                    var ext = filepath.substr(filepath.indexOf("."));
                    //alert($("#<%=headPicture.ClientID%>").val());
                    //alert(ext);
                    if (ext.toLocaleLowerCase(ext) != ".jpg") {
                        alert("请选择jpg格式的图片文件作为头像！");
                        return false;
                    }
                }
                if ($("#<%= IsAdd.ClientID%>").val() == "3") {//是删除操作
                    if ($("#<%=txt_account.ClientID %>") == "" || $("#<%=txt_nick.ClientID %>").val() == "" ) {
                        alert("请同时输入账号、昵称");
                        return false;
                    }
                }else{
                if ($("#<%=txt_account.ClientID %>") == "" || $("#<%=txt_nick.ClientID %>").val() == "" || $("#<%=txt_password.ClientID %>").val() == "") {
                    alert("请同时输入账号、昵称以及密码");
                    return false;
                }
                }
            });
        });

        function UpdateCustomService(kf_account, opttype) {
            //alert($("#" + kf_account + " td:first").html());
            var allaccount = $("#" + kf_account + " td:first").html();
            var nick = $("#" + kf_account + " td:first").next().html();
            //alert("账号：" + allaccount + "  昵称： " + nick);
            $("#<%=txt_account.ClientID %>").val(kf_account);
            $("#<%=txt_nick.ClientID %>").val(nick);
            $("#<%= IsAdd.ClientID%>").val(opttype);
            if (opttype == "2") {
                $("#Button1").val("退出修改");
                $("#<%=txt_account.ClientID %>").attr("readonly", "readonly");
                $("#<%=txt_nick.ClientID %>").removeAttr("readonly");
                //$("#<%=txt_password.ClientID %>").removeAttr("readonly");
                $("#optTitle").html("修改");
            }
            else {
                $("#Button1").val("退出删除");
                $("#<%=txt_account.ClientID %>").attr("readonly", "readonly");
                $("#<%=txt_nick.ClientID %>").attr("readonly", "readonly");
                //$("#<%=txt_password.ClientID %>").attr("readonly", "readonly");
                $("#optTitle").html("删除");
            }
            $("#Button1").show();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <br>
            <br>
            <h3>所有客服人员</h3>
            <asp:Repeater ID="Repeater1" runat="server">
                <HeaderTemplate>
                    <table cellpadding="3" cellspacing="3" style="padding: 3px;" >
                        <tr>
                            <th>客服账号</th>
                            <th>客服昵称</th>
                            <th>客服工号</th>
                            <th>客服昵称(nickname)</th>
                            <th>登录密码</th>
                            <th>头像</th>
                            <th>修改</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr id="<%#Eval("kf_account").ToString().Substring(0,Eval("kf_account").ToString().IndexOf("@")) %>">
                            <td><%#Eval("kf_account") %></td>
                            <td><%#Eval("kf_nick") %></td>
                            <td><%#Eval("kf_id") %></td>
                            <td><%#Eval("nickname") %></td>
                            <td><%#Eval("password") %></td>
                            <td><%#Eval("media") %></td>
                            <td>
                                <a href="javascript:void(0);" onclick='UpdateCustomService("<%#Eval("kf_account").ToString().Substring(0,Eval("kf_account").ToString().IndexOf("@")) %>",2);'>修改</a>
                               
                                <a href="javascript:void(0);" onclick='UpdateCustomService("<%#Eval("kf_account").ToString().Substring(0,Eval("kf_account").ToString().IndexOf("@")) %>",3);'>删除</a>
                            </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
        
        <br/>
        <br/>
        <br/>
        <br/>
        <br/>
        <div style="">
            <table cellpadding="3" cellspacing="3" style=" width: 713px; border-color:cadetblue;">
                <tr style="text-align: center;">
                    <td colspan="2"><h3><span id="optTitle">添加</span>客服人员</h3></td>
                </tr>
                <tr>
                    <td class="auto-style1">客服账号：</td>
                    <td>
                        <asp:TextBox ID="txt_account" runat="server" Width="227px"></asp:TextBox>
                        <%=ext %>(仅允许英文与数字)
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">客服昵称：</td>
                    <td>
                        <asp:TextBox ID="txt_nick" runat="server" MaxLength="12" Width="226px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">客服密码：</td>
                    <td>
                        <asp:TextBox ID="txt_password" runat="server" MaxLength="16" TextMode="Password" Width="225px"></asp:TextBox>
                        6到16位
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">客服头像：</td>
                    <td>
                        <asp:FileUpload ID="headPicture" runat="server" />(仅允许JPG格式的图片)
                    </td>
                </tr>
                <tr >
                    <td class="auto-style1"></td>
                    <td >
                        <input id="Button1" type="button" value="退出修改" />
                        <asp:Button ID="btn_Sure" runat="server" OnClick="btn_Sure_Click" Text="确定" />
                        <asp:HiddenField ID="IsAdd" Value="1" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
