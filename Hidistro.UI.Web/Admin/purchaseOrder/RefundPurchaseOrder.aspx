<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeFile="RefundPurchaseOrder.aspx.cs" Inherits="Hidistro.UI.Web.Admin.RefundPurchaseOrder"  %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

<div class="dataarea mainwidth td_top_ccc title_height">
	  <div class="toptitle"> <em><img src="../images/05.gif" width="32" height="32" /></em>
	    <h1 class="title_height">采购单退款</h1>
      </div>
	  <div class="Emall" style="height:25px;">
	    <ul>
	      <li class="colorA">执行退款操作后,退款项将退回到分销商的预付款帐户中。</li>
        </ul>
      </div>
	  <div class="Emal">
	    <ul>
	      <li>分销商：<asp:Literal runat="server" ID="litDistributorName" />  </li>
	      <li>订单号：<asp:Literal runat="server" ID="litOrderId" /></li>
	      <li>采购单编号：<asp:Literal runat="server" ID="litPurchaseOrderId" /></li>
	      <li>成交时间：<Hi:FormatedTimeLabel runat="server" ID="lblOrderDate" /> </li>
        </ul>
      </div>
	  <div class="areaform td_bg2 validator1">
	    <ul>
	      <li> <span class="formitemtitle Pw_198">采购单状态：</span><abbr class="colorG fonts"><Hi:PuchaseStatusLabel runat="server" ID="lblPurchaseStatus"  /> </abbr></li>
	      <li> <span class="formitemtitle Pw_198">采购单实收款(元)：</span><strong class="colorA fonts"><Hi:FormatedMoneyLabel runat="server" ID="lblTotalPrice" /></strong></li>
	      <li> <span class="formitemtitle Pw_198">需要退款的金额：</span>
	        <strong class="colorA fonts"><Hi:FormatedMoneyLabel runat="server" ID="lblRefundTotal" /><asp:TextBox runat="server" ID="txtRefundTotal" /></strong>
	        <span id="txtRefundTotalTip" runat="server" />
	        <p class="Pa_198 colorD"><asp:Literal runat="server" ID="litRefundComment" /></p>
          </li>
	      <li> <span class="formitemtitle Pw_198">退款说明：</span>
	        <asp:TextBox TextMode="MultiLine" runat="server" ID="txtRefundRemark" width="400" height="100"></asp:TextBox>
          </li>
        </ul>
      </div>
	  <div class="btn Pa_198 Pg_20">
	    <asp:Button runat="server" ID="BtnRefund" class="submit_DAqueding"  Text="确认退款" />
	   </div>
</div>
	<div class="bottomarea testArea">
    <!--顶部logo区域-->
  </div>
  
  

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
 <script type="text/javascript" language="javascript">
     function InitValidators() {
         if (document.getElementById("ctl00_contentHolder_txtRefundTotal").style.display == "") {
             initValid(new InputValidator('ctl00_contentHolder_txtRefundTotal', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '需要退款的金额不能为空,只能是数值,且不能超过2位小数'))
             appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtRefundTotal', 0.01, 10000000.00, '满足金额范围必须在0.01-10000000之间'));
         }
     }
$(document).ready(function(){ InitValidators(); });
</script>
</asp:Content>