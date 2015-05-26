<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Menus.aspx.cs" ValidateRequest="false" Inherits="Com.Meten.WeChatSmeten.Web.Menus" %>
<%@ Import Namespace="Com.Meten.WeChatSmeten.Entities" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>自定义菜单管理</title>
    <script src="js/jquery-1.8.2.min.js" language="javascript"></script>
    <script language="javascript" type="text/javascript">
        $(function () {
            $("#<%= btn_Sure.ClientID%>").click(function () {
                var tdata = "";
                $(".topCss").each(function () {
                    //数据格式： 
                    //  两个菜单之间用 <  
                    //  菜单与子菜单之间用   >
                    //  两个子菜单之间用  *
                    //  菜单各项之间用 |
                    //name|type|tkey|turl>sname|stype|stkey|sturl*sname|stype|stkey|sturl*sname|stype|stkey|sturl*sname|stype|stkey|sturl*sname|stype|stkey|sturl
                    // <
                    // name|type|tkey|turl>sname|stype|stkey|sturl*sname|stype|stkey|sturl*sname|stype|stkey|sturl*sname|stype|stkey|sturl*sname|stype|stkey|sturl
                    var temp = "";
                    var tname = $(this).find(".txt_name").val();
                    //一级菜单名称不为空
                    if (tname != "") {
                        var ttype = $(this).find(".ddl_type option:selected").val();
                        var tkey = $(this).find(".txt_key").val();
                        var turl = $(this).find(".txt_url").val();
                        temp = tname + "|" + ttype + "|" + tkey + "|" + turl;
                        var subtemp = "";
                        $(this).find(".underCss").each(function () {
                            var subtname = $(this).find(".txt_name").val();
                            //二级菜单名称不为空
                            if (subtname != "") {
                                var subttype = $(this).find(".ddl_type option:selected").val();
                                var subtkey = $(this).find(".txt_key").val();
                                var subturl = $(this).find(".txt_url").val();
                                var isOk = true;
                                if (subttype == "<%= Convert.ToInt32( UserButtonType.view) %>") {   //view类型的菜单  url不为空
                                    if (subturl == "") {
                                        isOk = false;
                                    }
                                } else {
                                    if (subtkey == "") {
                                        isOk = false;
                                    }
                                }
                                //alert(isOk);
                                if (isOk) {
                                    if (subtemp != "")
                                        subtemp += "*";
                                    subtemp += subtname + "|" + subttype + "|" + subtkey + "|" + subturl;
                                }
                            }
                        });
                        //如果子菜单不为空，则相加子菜单；否则判断一级菜单是否合法,不合法则忽略
                        if (subtemp != "") {
                            temp = temp + ">" + subtemp;
                        } else {
                            if (ttype == "<%= Convert.ToInt32( UserButtonType.view) %>") {   //view类型的菜单  url不为空
                                if (turl == "") {
                                    temp="";
                                }
                            } else {
                                if (tkey == "") {
                                    temp = "";
                                }
                            }
                        }
                    }
                    //如果菜单不为空；则相加
                    if (temp != "") {
                        if (tdata != "")
                            tdata += "<";
                        tdata += temp;
                    }
                });
                //alert(tdata);
                if (tdata != "") {
                    $("#<%=hfFinalData.ClientID%>").val(tdata);
                    return true;
                }
                return false;
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <br>
            <h3>自定义菜单</h3>
            <asp:Repeater ID="Repeater1" runat="server" OnItemDataBound="Repeater1_ItemDataBound">
                <HeaderTemplate>
                </HeaderTemplate>
                <ItemTemplate>
                    <table cellpadding="3" cellspacing="3" style="padding: 3px;" class="topCss">
                        <tr>
                            <td colspan="4">
                                菜单名称：<asp:TextBox CssClass="txt_name" ID="txt_name" runat="server" MaxLength="16"></asp:TextBox>
                                菜单类型：<asp:DropDownList CssClass="ddl_type" ID="ddl_type" runat="server"></asp:DropDownList>
                                菜单key值：<asp:TextBox CssClass="txt_key" ID="txt_key" runat="server" MaxLength="128"></asp:TextBox>
                                菜单url值：<asp:TextBox CssClass="txt_url" ID="txt_url" runat="server" MaxLength="256"></asp:TextBox>
                            </td>
                            <td></td>
                        </tr>
                        <asp:Repeater ID="sub_Repeater1" runat="server">
                            <HeaderTemplate>
                                <tr>
                                    <th style="text-align: center;">
                                       上级
                                    </th>
                                    <th style="text-align: center;">
                                       名称
                                    </th>
                                    <th style="text-align: center;">
                                       类型
                                    </th>
                                    <th style="text-align: center;">
                                       key值
                                    </th>
                                    <th style="text-align: center;">
                                       url值
                                    </th>
                                </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr class="underCss">
                                    <td style="text-align: center;">
                                        <h3 style="display: inline;"><%# DataBinder.Eval(((RepeaterItem)Container.Parent.Parent).DataItem,"name") %></h3>
                                    </td><td style="text-align: center;">
                                        <asp:TextBox CssClass="txt_name" ID="txt_name" runat="server" MaxLength="16"></asp:TextBox>
                                    </td><td style="text-align: center;">
                                        <asp:DropDownList CssClass="ddl_type" ID="ddl_type" runat="server"></asp:DropDownList>
                                    </td><td style="text-align: center;">
                                        <asp:TextBox CssClass="txt_key" ID="txt_key" runat="server" MaxLength="128"></asp:TextBox>
                                    </td><td style="text-align: center;">
                                        <asp:TextBox CssClass="txt_url" ID="txt_url" runat="server" MaxLength="256"></asp:TextBox>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                            </FooterTemplate>
                        </asp:Repeater>
                    </table>
                </ItemTemplate>
                <FooterTemplate>
                </FooterTemplate>
            </asp:Repeater>
        </div>
        <br />
        <div>
            注：
        <br />
            1、一级菜单个数应为1~3个；二级菜单数组，个数应为1~5个；
        <br />
            2、菜单标题为必填（如不填保存时将忽略该菜单以及所有子菜单），不超过16个字节，子菜单不超过40个字节
        <br />
            3、菜单key值，非view类型时该值为必填（若缺少将在保存时忽略），用于消息接口推送，不超过128字节
        <br />
            3、菜单url值，view类型时该值为必填（若缺少将在保存时忽略），网页链接，用户点击菜单可打开链接，不超过256字节

        </div>
        <br />
        <br />
        <div style="margin-left:300px;">
            <asp:HiddenField ID="hfFinalData" runat="server" />
            <asp:Button ID="btn_Sure" runat="server" OnClick="btn_Sure_Click" Width="100px" Height="40px" Text="确定" />
            <asp:Button ID="btn_Delete" style="margin-left: 300px;" runat="server" OnClick="btn_Delete_Click" Width="100px" Height="40px" Text="删除自定义菜单" />
        </div>
        
        

        <br />
        <br />
        <div style="margin-left:300px;">
        </div>
        
        

    </form>
</body>
</html>
