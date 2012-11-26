<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeFile="SiteRequest.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.SiteRequest" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth td_top_ccc">
<!--搜索-->
	  <!--结束-->
	  <div class="toptitle"> <em><img src="../images/03.gif" width="32" height="32" /></em>
	    <h1>申请开通分销子站</h1>
        <span>您可申请开通分销站点,并绑定独立域名（如www.myshop.com），您的客户就可以通过此独立域名访问您的网店</span>
      </div>
	  <div class="Site">
<ul>
  <li class="lispan"><span class="colorB fonts">域名备案：</span>根据国家信息产业部文件通知，只有备案通过的域名才可以网上开店。所以请先将您需要绑定的独立域名完成备案。</li>
      </ul>
      </div>
	  <div class="Site">
	    <ul>
	      <li class="lispan"><span class="colorB fonts">域名解析：</span>在域名通过备案后，您需要进入您的域名管理后台，将域名的cname指向到下面的IP地址：<asp:Literal runat="server" ID="litServerIp"/></li>
        </ul>
      </div>
	  <div class="Site ">
	    <ul>
	      <li class="lispan">在完成上面两步后，您可以开始绑定独立域名。您可以为网店绑定两个独立域名，例如www.shop.com和shop.com</li>
        </ul>
      </div>
	  <!--数据列表区域-->
	  <div class="datalist">
	    <table width="200" border="0" cellspacing="0">
	      <tr class="table_title">
	        <td colspan="4" class="td_right td_right_fff">分销站点域名</td>
          </tr>
	      <tr>
	        <td width="10%">第一个域名：</td>
	        <td width="40%"><asp:TextBox runat="server" ID="txtFirstSiteUrl" class="forminput input" /></td>
	        <td width="10%" align="right">备案号：</td>
	        <td width="40%"><asp:TextBox runat="server" ID="txtFirstRecordCode" class="forminput input" /></td>
          </tr>
          <tr>
	        <td width="10%">&nbsp;</td>
	        <td width="40%"><p runat="server" id="txtFirstSiteUrlTip">第一个域名不能为空,限制在30个字符以内,必须为正确格式</p></td>
	        <td width="10%" align="right">&nbsp;</td>
	        <td width="40%"><p runat="server" id="txtFirstRecordCodeTip">第一个域名备案号不能为空,长度限制在20个字符以内</p></td>
          </tr>
	      <tr>
	        <td>第二个域名：</td>
	        <td><asp:TextBox runat="server" ID="txtSencondSiteUrl" class="forminput input" /></td>
	        <td align="right">备案号：</td>
	        <td><asp:TextBox runat="server" ID="txtSecondRecordCode" class="forminput input" /></td>
          </tr>
          <tr>
	        <td width="10%">&nbsp;</td>
	        <td width="40%"><p runat="server" id="txtSencondSiteUrlTip">第二个域名长度限制在30个字符以内,必须为有效格式</p></td>
	        <td width="10%" align="right">&nbsp;</td>
	        <td width="40%"><p runat="server" id="txtSecondRecordCodeTip">第二个域名备案号长度限制在20个字符以内</p></td>
          </tr>
        </table>
	  </div>
	  <div class="Pg_15" style="text-align:center;"><asp:Button ID="btnAddRequest" runat="server" OnClientClick="return PageIsValid();" Text="提交申请"  CssClass="submit_DAqueding" /></div>
	  <!--数据列表底部功能区域-->
</div>
<div class="databottom"></div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" language="javascript">
    function InitValidators() {
        initValid(new InputValidator('ctl00_contentHolder_txtFirstSiteUrl', 1, 30, false, '^[0-9a-zA-Z]([0-9a-zA-Z-\.]+)$', '第一个域名必填,限制在30个字符以内,必须为有效格式'))
        initValid(new InputValidator('ctl00_contentHolder_txtFirstRecordCode', 1, 20, false, null, '第一个域名备案号必填,限制在20个字符以内'))
        initValid(new InputValidator('ctl00_contentHolder_txtSencondSite', 0, 30, true, '^[0-9a-zA-Z]([0-9a-zA-Z-\.]+)$', '第二个域名长度限制在30个字符以内,必须为有效格式'))
        initValid(new InputValidator('ctl00_contentHolder_txtSecondRecordCode', 0, 20, true, null, '第二个域名备案号长度限制在20个字符以内'))
    }
    $(document).ready(function() { InitValidators(); });
</script>
</asp:Content>
