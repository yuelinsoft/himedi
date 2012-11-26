<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeFile="ClosedOrderDetails.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.ClosedOrderDetails" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="cc1" TagName="Order_ItemsList" Src="~/Shopadmin/Ascx/Order_ItemsList.ascx" %>
<%@ Register TagPrefix="cc1" TagName="Order_ChargesList" Src="~/Shopadmin/Ascx/Order_ChargesList.ascx" %>
<%@ Register TagPrefix="cc1" TagName="Order_ShippingAddress" Src="~/Shopadmin/Ascx/Order_ShippingAddress.ascx" %>
<%@ Register TagPrefix="cc1" TagName="Order_UserInfo" Src="~/Shopadmin/Ascx/Order_UserInfo.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">


<div class="dataarea mainwidth databody">
    <div class="title title_height m_none td_bottom"> 
      <em><img src="../images/05.gif" width="32" height="32" /></em>
      <h1 class="title_line">订单详情</h1>
</div>
    <div class="Purchase">
      <div class="StepsD">
        <ul>
        	<li><strong class="fonts">第<span class="colorG">1</span>步</strong> 买家已下单</li>
            <li><strong class="fonts">第<span class="colorG">2</span>步</strong> 买家付款</li>
            <li><strong class="fonts">第<span class="colorG">3</span>步</strong> 发货</li>
            <li><strong class="fonts colorP">第4步</strong> <span class="colorO">交易完成</span></li>                                         
        </ul>
      </div>
      <div class="State">
        <ul>
        	<li><strong class="fonts colorE">当前订单状态：已关闭</strong></li>
        	<li>关闭原因：<asp:Literal runat="server" ID="litCloseReason" /></li>
            <li id="divRefundDetails" runat="server" visible="false">当前订单已全额退款给买家,点击查看： <abbr><asp:HyperLink runat="server" ID="hlkRefundDetails" Text="退款成功详情" Target="_blank"></asp:HyperLink></abbr></li>
            <li class="Pg_8">
                <span class="submit_btnbianji"><a href="javascript:DivWindowOpen(550, 350, 'RemarkOrder')">备注</a></span>
           </li>                      
        </ul>
      </div>
    </div>
	<div class="blank12 clearfix"></div>
	<div class="Purchase">
	  <div class="State">
       <cc1:Order_UserInfo runat="server" ID="userInfo" />
	  </div>
    </div>
  <div class="blank12 clearfix"></div>
	<div class="list">
     <cc1:Order_ItemsList  runat="server" ID="itemsList" />
	 <cc1:Order_ChargesList  ID="chargesList" runat="server" />
<cc1:Order_ShippingAddress runat="server" ID="shippingAddress" />
  </div>
  </div>

 <div class="Pop_up" id="RemarkOrder"  style="display:none;">
  <h1>编辑备注信息 </h1>
  <div class="img_datala"><img src="../images/icon_dalata.gif" width="38" height="20" /></div>
  <div class="mianform">
    <ul>
              <li><span class="formitemtitle Pw_128">订单编号：</span><asp:Literal ID="spanOrderId" runat="server" /></li>
             
       <li><span class="formitemtitle Pw_128">成交时间：</span><Hi:FormatedTimeLabel runat="server" ID="lblorderDateForRemark" /></li>
              <li><span class="formitemtitle Pw_128">订单实收款(元)：</span><strong class="colorA"><Hi:FormatedMoneyLabel ID="lblorderTotalForRemark" runat="server" /></strong></li>
              <li><span class="formitemtitle Pw_128">标志：</span>
                <span><Hi:OrderRemarkImageRadioButtonList runat="server" ID="orderRemarkImageForRemark" /></span>
                </li>
              <li><span class="formitemtitle Pw_128">备忘录：</span>
          <asp:TextBox ID="txtRemark" TextMode="MultiLine" runat="server" Width="300" Height="50" />
              </li>
        </ul>
         <ul class="up Pa_100">
         <asp:Button runat="server" ID="btnRemark" Text="确定" CssClass="submit_DAqueding"/>
       </ul>
  </div> 
</div>




</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
   
</asp:Content>

