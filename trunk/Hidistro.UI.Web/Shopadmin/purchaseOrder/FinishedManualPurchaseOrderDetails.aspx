<%@ Page Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeFile="FinishedManualPurchaseOrderDetails.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.FinishedManualPurchaseOrderDetails" Title="无标题页" %>
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
       <div class="StepsD">
        <ul>
          <li><strong class="fonts">第<span class="colorG">1</span>步</strong> 提交采购单</li>
          <li><strong class="fonts">第<span class="colorG">2</span>步</strong> 付款</li>
          <li><strong class="fonts">第<span class="colorG">3</span>步</strong>供应商发货</li>
          <li><strong class="fonts colorP">第4步</strong> <span class="colorO">交易完成</span></li>
        </ul>
      </div>
      <div class="State">
  <ul>
        	<li><strong class="fonts colorE">当前采购单状态：已完成</strong></li>
            <li class="Pg_8">
                <div class="display" runat="server" id="divRefundDetails" visible="false">
                当前订单已部分退款给买家,点击查看：<asp:HyperLink runat="server" ID="hlkRefundDetails" ForeColor="#4183F1" Text="退款成功详情" ></asp:HyperLink>
                    </div>
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
       <cc1:PurchaseOrder_Charges  ID="chargesList" runat="server" />
       <cc1:PurchaseOrder_ShippingAddress runat="server" ID="shippingAddress" />  
       <asp:Panel runat="server" ID="plExpress" Visible="false">
        <table width="908" border="0" cellspacing="0" cellpadding="0">
        <tr>
          <td  colspan="4"><div id="spExpressData">正在加载中....</div></td>
        </tr>
        <tr>
        <td colspan="4">
        <a href='http://www.kuaidi100.com' target='_blank' id="power" runat="server" visible="false">此物流信息由快递100提供</a>
        </td>
        </tr>
      </table>
      </asp:Panel>
  </div>
  </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript">
    window.onload = function Init() {
        var OrderId = window.location.search;
        OrderId = OrderId.substring(OrderId.indexOf("=") + 1);
        $.ajax({
            url: "ExpressData.aspx?PurchaseOrderId=" + OrderId,
            type: 'POST', dataType: 'json',
            async: true,
            success: function(resultData) {
                $('#spExpressData').html(resultData.Express);
            }
        });
    }
    </script>
</asp:Content>
