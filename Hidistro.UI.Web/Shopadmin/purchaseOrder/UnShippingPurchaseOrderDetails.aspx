<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeFile="UnShippingPurchaseOrderDetails.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.UnShippingPurchaseOrderDetails" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="cc1" TagName="PurchaseOrder_Items" Src="~/Shopadmin/Ascx/PurchaseOrder_Items.ascx" %>
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
          <li><strong class="fonts">第<span class="colorG">1</span>步</strong> 生成采购单</li>
          <li><strong class="fonts colorP">第2步</strong> <span class="colorO">付款</span></li>
          <li><strong class="fonts">第<span class="colorG">3</span>步</strong> 供应商发货</li>
           <li><strong class="fonts">第<span class="colorG">4</span>步</strong>辅助发货</li>
          <li><strong class="fonts">第<span class="colorG">5</span>步</strong> 交易完成 </li>
        </ul>
      </div>
       <div class="State">
        <ul>
        	<li><strong class="fonts colorE">当前采购单状态：分销商已付款，等待供应商发货</strong></li>
            <li>您已发付款给供应商，等待供应商发货</li>
                                
        </ul>
      </div>
    </div>
	<div class="blank12 clearfix"></div>
	<div class="Purchase">
	  <div class="State">
	    <h1>采购单详情</h1>
	    <table width="200" border="0" cellspacing="0">
	      <tr>
	        <td width="10%" align="right">订单编号：</td>
	        <td width="20%"><asp:HyperLink runat="server" ID="hlkOrder" /></td>
	        <td width="10%" align="right">采购单编号：</td>
	        <td width="28%"><asp:Literal ID="litPurchaseOrderId" runat="server"></asp:Literal></td>	        
          </tr>
	     
        </table>
      </div>
  </div>
  <div class="blank12 clearfix"></div>
	<div class="list">
       <cc1:PurchaseOrder_Items runat="server" ID="itemsList" />
       <cc1:PurchaseOrder_Charges  ID="chargesList" runat="server" />
       <cc1:PurchaseOrder_ShippingAddress runat="server" ID="shippingAddress" />  
  </div>
  </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>

