<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.DistributorRequestInstruction" CodeFile="DistributorRequestInstruction.aspx.cs" MasterPageFile="~/Admin/Admin.Master" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

<div class="optiongroup mainwidth">
		<ul>
            <li class="optionstar"><a href="DistributorRequests.aspx" class="optionnext"><span>招募分销商</span></a></li>
            <li class="menucurrent"><a href="DistributorRequestInstruction.aspx" class="optioncurrentend"><span class="optioncenter">招募说明</span></a></li>
		</ul>
	</div>
	<!--选项卡-->
	<div class="dataarea mainwidth">
    <div class="blank5 clearfix"></div>
    <div class="areaform">
		<ul>
        	<li><span class="formitemtitle Pw_140">招募说明：</span><span><Kindeditor:KindeditorControl ID="fkFooter" runat="server" Width="742"  Height="298" /></span></li>
            <li><span class="formitemtitle Pw_140">分销商注册协议：<em>*</em></span><span><Kindeditor:KindeditorControl ID="fkProtocols" runat="server" Width="742"  ImportLib="false" Height="298"/></span></li>
        </ul>
	</div>
    <div class="btn Pa_140">
      <asp:Button ID="btnOK" runat="server" Text="保 存" CssClass="submit_DAqueding" />
</div>
</div>
<div class="databottom"></div>	
<div class="bottomarea testArea">
  <!--顶部logo区域-->
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>

