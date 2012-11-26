<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Login" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <Hi:HeadContainer ID="HeadContainer1" runat="server" />
    <Hi:PageTitle ID="PageTitle1" runat="server" />
<link rel="stylesheet" href="css/newlogin.css" type="text/css" media="screen" />
</head>
<body>
<form id="form1" runat="server">
<asp:Panel ID="Panel1" runat="server" DefaultButton="btnAdminLogin">
<div class="login">
  <div class="logo"></div>
  <div class="loginput">
    <div class="icon"></div>
    <div class="imgbg">
    <span class="topimg"></span>
    <span class="cenimg"></span>
    <span class="botimg"></span>
    </div>
    <div class="inputbg">
      <table width="247" border="0" cellspacing="0" cellpadding="0">
        <tr>
          <td colspan="3"><img src="images/login_09.gif" width="247" height="38" /></td>
        </tr>
        <tr>
          <td width="62" nowrap="nowrap">用户名：</td>
          <td colspan="2">
          <asp:TextBox ID="txtAdminName" CssClass="inputcs" Width="180" runat="server"></asp:TextBox>
         </td>
        </tr>
        <tr>
          <td>密  &nbsp;&nbsp;码：</td>
          <td colspan="2">
          <asp:TextBox ID="txtAdminPassWord"  CssClass="inputcs" runat="server" TextMode="Password" Width="180" />
          </td>
        </tr>
        <tr>
          <td>验证码：</td>
          <td>
          <asp:TextBox ID="txtCode" runat="server" CssClass="inputcs" Width="90"></asp:TextBox>
         </td>
          <td style="width:100%;padding-left:10px;text-align:left;"><a href="javascript:refreshCode();"><img id="imgVerifyCode" src='<%= Globals.ApplicationPath + "/VerifyCodeImage.aspx" %>' style="border-style:none" /></a></td>
        </tr>
        <tr>
          <td>&nbsp;</td>
          <td colspan="2">
          <asp:Button ID="btnAdminLogin" runat="server" Text="登录" OnClick="btnAdminLogin_Click" CssClass="btn" 
                 />
          </td>
        </tr>
        <tr>
          <td>&nbsp;</td>
          <td colspan="2">
          <Hi:SmallStatusMessage ID="lblStatus" runat="server" Visible="False" Width="260px" />
          </td>
        </tr>
      </table>
    </div>
  </div>
</div>
</asp:Panel>
</form>
<script language="javascript" type="text/javascript">
                function refreshCode() {
                    var img = document.getElementById("imgVerifyCode");
                    if (img != null) {
                        var currentDate = new Date();
                        img.src = '<%= Globals.ApplicationPath + "/VerifyCodeImage.aspx?t=" %>' + currentDate.getTime();
       }
  }
</script>
</body>
</html>