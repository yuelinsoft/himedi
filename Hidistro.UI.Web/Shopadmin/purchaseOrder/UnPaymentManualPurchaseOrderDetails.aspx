<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeFile="UnPaymentManualPurchaseOrderDetails.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.UnPaymentManualPurchaseOrderDetails" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="cc1" TagName="ManualPurchaseOrder_Items" Src="~/Shopadmin/Ascx/ManualPurchaseOrder_Items.ascx" %>
<%@ Register TagPrefix="cc1" TagName="PurchaseOrder_Charges" Src="~/Shopadmin/Ascx/PurchaseOrder_Charges.ascx" %>
<%@ Register TagPrefix="cc1" TagName="PurchaseOrder_ShippingAddress" Src="~/Shopadmin/Ascx/PurchaseOrder_ShippingAddress.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth databody">
    <div class="title title_height m_none td_bottom"><em><img src="../images/+003.gif" width="32" height="32" /></em>
      <h1 class="title_line">采购单详情</h1>
</div>
    <div class="Purchase">
      <div class="StepsB">
        <ul>
        	<li><strong class="fonts">第<span class="colorG">1</span>步</strong>提交采购单</li>
        	<li><strong class="fonts colorP">第2步</strong> <span class="colorO">付款</span></li>
            <li><strong class="fonts">第<span class="colorG">3</span>步</strong>供应商发货</li>
            <li><strong class="fonts">第<span class="colorG">4</span>步</strong>交易完成</li>                                                     
        </ul>
      </div>
      <div class="State">
        <ul>
        	<li><strong class="fonts colorE">当前采购单状态：等待付款</strong></li>
            <li class="Pg_8">
                <span class="submit_faihuo"><asp:HyperLink runat="server" ID="lkbtnPay" Text="付款" /></span>
                <span class="submit_btnguanbi"><a id="lkbtnClosePurchaseOrder" href="javascript:DivWindowOpen(400, 220, 'ClosePurchaseOrder')">取消采购</a></span>
           </li>                     
        </ul>
      </div>
    </div>
	<div class="blank12 clearfix"></div>
	<div class="Purchase">
	  <div class="State">
	   <table width="200" border="0" cellspacing="0">
              <tr>
                <td width="11%"><h1>采购单详情</h1></td>
                <td width="10%" align="right">采购单编号：</td>
                <td width="79%"><asp:Literal ID="litPurchaseOrderId" runat="server"></asp:Literal></td>
              </tr>
        </table>
	    
      </div>
  </div>
  <div class="blank12 clearfix"></div>
	<div class="list">
       <cc1:ManualPurchaseOrder_Items runat="server" ID="itemsList" />
       <div><asp:HyperLink runat="server" ID="hlkOrderGifts" Text="添加礼品" /></div>
       <cc1:PurchaseOrder_Charges  ID="chargesList" runat="server" />
       <cc1:PurchaseOrder_ShippingAddress runat="server" ID="shippingAddress" />  
  </div>
  </div>
  
  
   <!--关闭采购单-->
<div class="Pop_up" id="ClosePurchaseOrder" style="display:none;">
  <h1>关闭采购单 </h1>
  <div class="img_datala"><img src="../images/icon_dalata.gif" width="38" height="20" /></div>
  <div class="mianform fonts colorA borbac"><strong>取消采购单?请选择取消采购单的理由：</strong></div>
  <div class="mianform">
    <ul>
      <li><span class="formitemtitle Pw_160">取消该采购单的理由：</span> <abbr class="formselect">
        <Hi:DistributorClosePurchaseOrderReasonDropDownList runat="server" ID="ddlCloseReason" />
      </abbr> </li>
    </ul>
    <ul class="up Pa_160">
      <asp:Button ID="btnClosePurchaseOrder"  runat="server" CssClass="submit_DAqueding" OnClientClick="return ValidationCloseReason()" Text="确 定"   />
    </ul>
  </div>
</div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript">
 function ValidationCloseReason() {
                     var reason = document.getElementById("ctl00_contentHolder_ddlCloseReason").value;
                     if (reason == "请选择取消的理由") {
                         alert("请选择取消的理由");
                         return false;
                     }

                     return true;
                 }                 

</script>
</asp:Content>
