<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeFile="RefundOrderDetails.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.RefundOrderDetails" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">


<div class="dataarea mainwidth td_top_ccc title_height">
	  <div class="toptitle"> <em><img src="../images/05.gif" width="32" height="32" /></em>
	    <h1 class="title_height">退款成功</h1>
      </div>
	  <div class="Emal">
	    <ul>
	      <li>订单号：<asp:Literal runat="server" ID="litOrderId" /></li>
	      <li>成交时间：<Hi:FormatedTimeLabel runat="server" ID="lblOrderDate" /></li>
	      <li>订单实付款(元)：<strong class="colorE"><Hi:FormatedMoneyLabel runat="server" ID="lblTotalPrice" />  </strong></li>
        </ul>
      </div>
	  <div class="areaform td_bg2">
	    <ul>
	      <li> <span class="formitemtitle Pw_198">退款时间：</span><Hi:FormatedTimeLabel ID="lblRefundDate" runat="server" /> </li>
	      <li> <span class="formitemtitle Pw_198">退款状态：</span>退款成功</li>
	      <li> <span class="formitemtitle Pw_198">退款给买家的金额(元)：</span><strong class="colorG fonts"><Hi:FormatedMoneyLabel runat="server" ID="lblRefundAmount" /></strong></li>
	      <li> <span class="formitemtitle Pw_198">退款方式：</span><label id="lblRefundMode" runat="server"></label></li>
	      <li> <span class="formitemtitle Pw_198">退款说明：</span><asp:Literal runat="server" ID="litRefundRemark" /></li>
	      <li> <span class="formitemtitle Pw_198"></span><strong class="colorA">提示：</strong><abbr class="colorE"><label id="lblDescription" runat="server"></label></abbr></li>
        </ul>
      </div>
	 
</div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
