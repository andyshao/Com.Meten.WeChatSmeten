<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="medias.aspx.cs" Inherits="Com.Meten.WeChatSmeten.Web.medias" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>多媒体上传于下载测试</title>
</head>
<body>
    <form id="form1" runat="server">
        <table style="width: 100%;">
          <%--  <tr>
                <td colspan="2">
                    http请求方式: POST/FORM
http://file.api.weixin.qq.com/cgi-bin/media/upload?access_token=ACCESS_TOKEN&type=TYPE
调用示例（使用curl命令，用FORM表单方式上传一个多媒体文件）：
curl -F media=@test.jpg "http://file.api.weixin.qq.com/cgi-bin/media/upload?access_token=ACCESS_TOKEN&type=TYPE"
                    <br/>
                    输入参数：
                    <br/>
                    access_token	 是	 调用接口凭证
                    <br/>
                    type	 是	 媒体文件类型，分别有图片（image）、语音（voice）、视频（video）和缩略图（thumb）
                    <br/>
                    media	 是	 form-data中媒体文件标识，有filename、filelength、content-type等信息
                    <br/>
                    返回参数：
                    <br/>
type	 媒体文件类型，分别有图片（image）、语音（voice）、视频（video）和缩略图（thumb，主要用于视频与音乐格式的缩略图）
                    <br/>
media_id	 媒体文件上传后，获取时的唯一标识
                    <br/>
created_at	 媒体文件上传时间戳
                    <br/>
                    注意事项
                    <br/>
上传的多媒体文件有格式和大小限制，如下：

图片（image）: 1M，支持JPG格式
语音（voice）：2M，播放长度不超过60s，支持AMR\MP3格式
视频（video）：10MB，支持MP4格式
缩略图（thumb）：64KB，支持JPG格式
</td>
            </tr>--%>
            <tr>
                <td>&nbsp;Type:</td>
                <td>&nbsp;
                    <asp:DropDownList ID="DropDownList1" runat="server">
                        <asp:ListItem Value="image">image（图片）</asp:ListItem>
                        <asp:ListItem Value="voice">voice（音乐）</asp:ListItem>
                        <asp:ListItem Value="video">video（视频）</asp:ListItem>
                        <asp:ListItem Value="thumb">thumb（缩略图）</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>&nbsp;Media</td>
                <td>&nbsp;<asp:FileUpload ID="FileUpload1" runat="server" AllowMultiple="False" /></td>
            </tr>
            <tr><td >&nbsp;</td>
                <td > <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="确定" /> </td>
            </tr>
        </table>
    </form>
</body>
</html>
