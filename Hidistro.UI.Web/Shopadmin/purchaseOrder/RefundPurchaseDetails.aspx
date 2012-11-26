<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeFile="RefundPurchaseDetails.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.RefundPurchaseDetails" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
 <div class="dataarea mainwidth td_top_ccc title_height">
      <div class="toptitle"><em><img src="../images/+003.gif" width="32" height="32" /></em>
        <h1 class="title_height">采购单退款详情</h1>
       </div>
      <div class="Emall">
        <ul>
          <li><strong class="colorA">当前采购单编号:  <asp:Literal runat="server" ID="litPurchaseOrderId" /></strong></li>
        </ul>
      </div>
      
      <div class="areaform td_bg2">
<ul>
              <li> <span class="formitemtitle Pw_198">退款时间：</span><Hi:FormatedTimeLabel ID="lblRefundDate" runat="server" /> </li>
              <li> <span class="formitemtitle Pw_198">退款状态：</span>退款成功</li>
              <li> <span class="formitemtitle Pw_198">已退款的金额(元)：</span><strong class="colorG fonts"><Hi:FormatedMoneyLabel runat="server" ID="lblRefundAmount" /></strong></li>
              <li> <span class="formitemtitle Pw_198">供货商实收金额(元)：</span><strong class="colorB fonts"><Hi:FormatedMoneyLabel runat="server" ID="lblPaymentAmount" /></strong></li>
              <li> <span class="formitemtitle Pw_198">退款说明：</span><asp:Literal runat="server" ID="litRefundRemark" /></li>
              <li > <span class="formitemtitle Pw_198"></span><strong class="colorA">提示：</strong><abbr class="colorE">退款成功直接返还到买家的预付款帐户中</abbr></li>
    </ul>
</div>
     
</div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
