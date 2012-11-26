<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeFile="ShowSiteRequestStatus.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.ShowSiteRequestStatus" %>
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
      <li runat="server" id="liFail" visible="false"><span class="colorA fonts"><strong>您提交的申请没有通过审核! </strong></span> <strong class="Pg_45">拒绝原因：</strong><span class="colorB"><asp:Literal runat="server" ID="litRefuseReason"></asp:Literal></span></li>
      <li class="lispan" runat="server" id="liWait" visible="false"><span class="colorA fonts"><strong>请等待审核...</strong></span></li>
      <li class="lispan" runat="server" id="liSuccess" visible="false"><span class="colorA fonts"><strong>站点申请成功</strong></span></li>
      </ul>
      </div>
	  <!--数据列表区域-->
	  <div class="datalist Pa_15">
	    <table width="200" border="0" cellspacing="0">
	      <tr class="table_title">
	        <td colspan="4" class="td_right td_right_fff">您提交的域名及备案信息如下：</td>
          </tr>
	      <tr>
	        <td width="10%">第一个域名：</td>
	        <td width="40%"><asp:Literal runat="server" ID="litFirstUrl" /></td>
	        <td width="10%" align="right">备案号：</td>
	        <td width="40%"><asp:Literal runat="server" ID="litFirstCode" /></td>
          </tr>
	      <tr>
	        <td>第二个域名：</td>
	        <td><asp:Literal runat="server" ID="litSecondUrl" />&nbsp;</td>
	        <td align="right">备案号：</td>
	        <td><asp:Literal runat="server" ID="litSecondCode" />&nbsp;</td>
          </tr>
        </table>
	  </div>
	  <div runat="server" id="divRequestAgain" visible="false">
	  <div class="datalist">
	    <table width="200" border="0" cellspacing="0">
	    <tr class="table_title">
	        <td colspan="4" class="td_right td_right_fff"><strong class="colorA">您可以重新提交：</strong></td>
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
	  <div class="Pg_15" style="text-align:center;"><asp:Button ID="btnRequestAgain" runat="server" OnClientClick="return PageIsValid();" Text="提 交"  CssClass="submit_DAqueding" /></div>
	  </div>
	  <!--数据列表底部功能区域-->
</div>
<div class="databottom"></div>
<%--<div class="areacolumn clearfix">
      <div class="columnleft2">
        <div class="columnleftmenu2 clearfix">
          <ul>
            <li class="itempitchon"><span>申请开通分销站点</span></li>
            <li><a href="SiteUrlDetails.aspx"><span>域名管理</span></a></li>
            <li><a href="ManageMyThemes.aspx"><span>模板管理</span></a></li>
          </ul>
        </div>
        <div class="columnleftbottom2 clearfix"></div>
      </div>
      <div class="columnright">
    <asp:Panel runat="server" ID="PnRequestFailed" Visible="false">
    <div class="backg">
	  <div class="backg_bg">
	    <table width="100%" border="0" cellspacing="0" cellpadding="0">
	      <tr>
	        <td width="16%" rowspan="4" align="center" valign="middle"><img src="../images/icon_03.gif" width="74" height="71" /></td>	              
          </tr>         
          <tr>
           <td align="left" class="spanR">尊敬的：<span class="formnote Arial spanS "><asp:Literal ID="litUserName" runat="server"></asp:Literal></span></td>
          </tr>
          <tr>
           <td align="center" class="spanR">很抱歉,您开通分销站点的申请被管理员拒绝了.</td>
           </tr>
           <tr>
           <td align="center"><span style="color:Silver;">拒绝原因:<asp:Literal runat="server" ID="litRefuseReason"></asp:Literal></span> &nbsp;&nbsp;您可以:<asp:LinkButton ID="btnRequestAgain" runat="server" ForeColor="blue" Text="重新申请开通分销站点"></asp:LinkButton></td>
           </tr>
        </table>
         </div>
      </div>

</asp:Panel>

<asp:Panel runat="server" ID="PnRequestOnDeling" Visible="false">
<div class="areaform clearfix">
    <div class="backg">
	  <div class="backg_bg">
	    <table width="100%" border="0" cellspacing="0" cellpadding="0">
	      <tr>
	        <td width="16%" rowspan="3" align="center" valign="middle"><img src="../images/Ts_03.gif" width="74" height="71" /></td>	              
          </tr>         
          <tr>
           <td align="left" class="spanR">尊敬的：<span class="formnote Arial spanS "><asp:Literal ID="litUserName2" runat="server"></asp:Literal></span></td>
          </tr>
          <tr>
           <td align="center" class="spanR">您的分销站点开通申请已提交,请等待管理员审核.</td>
           </tr>           
        </table>
         </div>
      </div>
	</div>
</asp:Panel>

   <asp:Panel runat="server" ID="PnRequestSeccessed" Visible="false">
<div class="areaform clearfix">
    <div class="backg">
	  <div class="backg_bg">
	    <table width="100%" border="0" cellspacing="0" cellpadding="0">
	      <tr>
	        <td width="16%" rowspan="4" align="center" valign="middle"><img src="../images/tick_64.png" width="74" height="71" /></td>	              
          </tr>         
          <tr>
           <td align="left" class="spanR">尊敬的：<span class="formnote Arial spanS "><asp:Literal ID="litUserName3" runat="server"></asp:Literal></span></td>
          </tr>
          <tr>
           <td align="center" class="spanR">您的分销站点已开通!</td>
           </tr>
           <tr>
           <td align="center">您可以:<asp:HyperLink ID="lkToSiteUrlDetails" runat="server" NavigateUrl="~/Shopadmin/store/SiteUrlDetails.aspx" ForeColor="blue" Text="查看分销站点的域名绑定情况"></asp:HyperLink> &nbsp; &nbsp;或是:<asp:HyperLink ID="lkToManageMyThemes" runat="server" NavigateUrl="~/Shopadmin/store/ManageMyThemes.aspx" ForeColor="blue" Text="管理分销站点模板"></asp:HyperLink></td>
           </tr>
        </table>
         </div>
      </div>
	</div>
	
</asp:Panel>
</div>
</div>--%>
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
