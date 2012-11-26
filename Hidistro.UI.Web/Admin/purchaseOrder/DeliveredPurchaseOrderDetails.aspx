<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeFile="DeliveredPurchaseOrderDetails.aspx.cs" Inherits="Hidistro.UI.Web.Admin.DeliveredPurchaseOrderDetails"  %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="cc1" TagName="PurchaseOrder_Items" Src="~/Admin/Ascx/PurchaseOrder_Items.ascx" %>
<%@ Register TagPrefix="cc1" TagName="PurchaseOrder_Charges" Src="~/Admin/Ascx/PurchaseOrder_Charges.ascx" %>
<%@ Register TagPrefix="cc1" TagName="PurchaseOrder_ShippingAddress" Src="~/Admin/Ascx/PurchaseOrder_ShippingAddress.ascx" %>
<%@ Register TagPrefix="cc1" TagName="PurchaseOrder_DistributorInfo" Src="~/Admin/Ascx/PurchaseOrder_DistributorInfo.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

<div class="dataarea mainwidth databody">
    <div class="title title_height m_none td_bottom"> 
      <em><img src="../images/02.gif" width="32" height="32" /></em>
      <h1 class="title_line">采购单详情</h1>
  </div>
    <div class="Purchase">
      <div class="StepsC">
        <ul>
          <li><strong class="fonts">第<span class="colorG">1</span>步</strong> 提交或生成采购单</li>
          <li><strong class="fonts">第<span class="colorG">2</span>步</strong> 付款</li>
          <li><strong class="fonts colorP">第3步</strong> <span class="colorO">供应商发货</span></li>
          <li><strong class="fonts">第<span class="colorG">4</span>步</strong> 交易完成 </li>
        </ul>
      </div>
      <div class="State">
        <ul>
        	<li><strong class="fonts colorE">当前采购单状态：供应商已发货</strong></li>
             <li runat="server" id="divRefundDetails" visible="false">当前采购单已部分退款给买家,点击查看：<asp:HyperLink runat="server" ID="hlkRefundDetails" ForeColor="#4183F1" Text="退款成功详情" ></asp:HyperLink>
            </li>            
            <li runat="server" id="divRefund">如果需要给分销商部分退款.请点击 <abbr><asp:HyperLink runat="server" ID="hlkRefund" ForeColor="#4183F1" >部分退款给分销商</asp:HyperLink></abbr></li>
            <li class="Pg_8">
             <span class="submit_btnbianji"><a href="javascript:DivWindowOpen(560,420,'RemarkPurchaseOrder');">备注</a></span>
           </li>                     
        </ul>
      </div>
    </div>
	<div class="blank12 clearfix"></div>
	<div class="Purchase">
	  <cc1:PurchaseOrder_DistributorInfo runat="server" ID="userInfo" />
    </div>
  <div class="blank12 clearfix"></div>
	<div class="list">
    <cc1:PurchaseOrder_Items runat="server" ID="itemsList" />
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
  <div class="bottomarea testArea">
    <!--顶部logo区域-->
  </div>  

<!--编辑备注信息-->
  <div class="Pop_up" id="RemarkPurchaseOrder"  style="display:none;">
  <h1>编辑备注信息 </h1>
  <div class="img_datala"><img src="../images/icon_dalata.gif" width="38" height="20" /></div>
  <div class="mianform">
    <ul>
              <li><span class="formitemtitle Pw_128">订单编号：</span><asp:Literal ID="spanOrderId" runat="server" /></li>
              <li><span class="formitemtitle Pw_128">采购单编号：</span><asp:Literal ID="spanpurcharseOrderId" runat="server" /></li>
       <li><span class="formitemtitle Pw_128">成交时间：</span><Hi:FormatedTimeLabel runat="server" ID="lblpurchaseDateForRemark" /></li>
              <li><span class="formitemtitle Pw_128">采购单实收款(元)：</span><strong class="colorA"><Hi:FormatedMoneyLabel ID="lblpurchaseTotalForRemark" runat="server" /></strong></li>
              <li><span class="formitemtitle Pw_128">标志：</span>
                <span><Hi:OrderRemarkImageRadioButtonList runat="server" ID="orderRemarkImageForRemark" /></span>
                </li>
              <li><span class="formitemtitle Pw_128">备忘录：</span>
         <span> <asp:TextBox ID="txtRemark" TextMode="MultiLine" runat="server" Width="300" Height="50" /></span>
         </li>
        </ul>
         <ul class="up Pa_128 clear">
         <asp:Button runat="server" ID="btnRemark" Text="确定" CssClass="submit_DAqueding"/>
       </ul>
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

