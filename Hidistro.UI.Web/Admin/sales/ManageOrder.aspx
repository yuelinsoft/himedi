<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeFile="ManageOrder.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ManageOrder" %>
<%@ Import Namespace="Hidistro.Core"%>
<%@ Import Namespace="Hidistro.Entities.Sales"%>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>

<%@ Import Namespace="Hidistro.Membership.Context" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

<div class="optiongroup mainwidth">
		<ul>
			<li  id="anchors0"><asp:HyperLink ID="hlinkAllOrder" runat="server" ><span>所有订单</span></asp:HyperLink></li>
			<li  id="anchors1"><asp:HyperLink ID="hlinkNotPay" runat="server" Text=""><span>等待买家付款</span></asp:HyperLink></li>
			<li  id="anchors2"><asp:HyperLink ID="hlinkYetPay" runat="server" Text=""><span>等待发货</span></asp:HyperLink></li>
            <li  id="anchors3"><asp:HyperLink ID="hlinkSendGoods" runat="server" Text=""><span>已发货</span></asp:HyperLink></li>
            <li  id="anchors5"><asp:HyperLink ID="hlinkTradeFinished" runat="server" Text=""><span>成功订单</span></asp:HyperLink></li>
            <li  id="anchors4"><asp:HyperLink ID="hlinkClose" runat="server" Text=""><span>已关闭</span></asp:HyperLink></li>
            <li  id="anchors99"><asp:HyperLink ID="hlinkHistory" runat="server" Text=""><span>历史订单</span></asp:HyperLink></li>                                                                             
		</ul>
	</div>
	<!--选项卡-->

<div class="dataarea mainwidth">
		<!--搜索-->
		<div class="searcharea clearfix br_search">
		  <ul>
		    <li> <span>选择时间段：</span><span>
		      <UI:WebCalendar CalendarType="StartDate" ID="calendarStartDate" runat="server" cssclass="forminput" />
		      </span> <span class="Pg_1010">至</span> <span>
		        <UI:WebCalendar ID="calendarEndDate" runat="server" CalendarType="EndDate" cssclass="forminput" />
		        </span></li>
		    <li><span>会员名：</span><span>
		      <asp:TextBox ID="txtUserName" runat="server" cssclass="forminput" />
		    </span></li>
		    <li><span>订单编号：</span><span>
		      <asp:TextBox ID="txtOrderId" runat="server" cssclass="forminput" /><asp:Label ID="lblStatus" runat="server" style="display:none;"></asp:Label>
		      </span></li>		     
		      <li><span>商品名称：</span><span>
		      <asp:TextBox ID="txtProductName" runat="server" cssclass="forminput" />
		      </span></li>
		      <li><span>收货人：</span><span>
		      <asp:TextBox ID="txtShopTo" runat="server" cssclass="forminput"></asp:TextBox>
		      </span></li>
		      <li><span>打印状态：</span><span>
		        <abbr class="formselect">  <asp:DropDownList runat="server" ID="ddlIsPrinted" /></abbr>
		      </span></li>
		      <li>&nbsp;</li>
		      	      <li><span>配送方式：</span><span>
		        <abbr class="formselect"><hi:ShippingModeDropDownList runat="server" AllowNull="true" ID="shippingModeDropDownList" /></abbr>
		      </span></li>
		      <li style="width:450px;">
		      <abbr>  <Hi:RegionSelector runat="server" ID="dropRegion" /></abbr>
		      </li>
		    <li>
		      <asp:Button ID="btnSearchButton" runat="server" class="searchbutton" OnClick="btnSearchButton_Click" Text="查询" />
	        </li>
	      </ul>
  </div>
		<!--结束-->
         <div class="functionHandleArea clearfix m_none">
			<!--分页功能-->
			<div class="pageHandleArea">
				<ul>
					<li class="paginalNum"><span>每页显示数量：</span><UI:PageSize runat="server" ID="hrefPageSize" /></li>
				</ul>
			</div>
			<div class="pageNumber">
				<div class="pagination">
                    <UI:Pager runat="server" ShowTotalPages="false" ID="pager1" />
                </div>
			</div>
			<!--结束-->

  <div class="blank8 clearfix"></div>
      <div class="batchHandleArea">
        <ul>
          <li class="batchHandleButton"> <span class="signicon"></span> 
          <span class="allSelect"><a href="javascript:void(0)" onclick="SelectAll()">全选</a></span>
		  <span class="reverseSelect"><a href="javascript:void(0)" onclick=" ReverseSelect()">反选</a></span>
		  <span class="delete"><Hi:ImageLinkButton ID="lkbtnDeleteCheck" runat="server" Text="删除" OnClick="lkbtnDeleteCheck_Click" IsShow="true"/></span>
		  <Hi:ImageLinkButton ID="btnBatchPrintData" runat="server" Text="批量打印快递单" OnClick="btnPrintOrder_Click" DeleteMsg="将当前选中结果创建快递单打印任务列表，是否继续？" IsShow="true"/>
          <Hi:ImageLinkButton ID="btnBatchSendGoods" runat="server" Text="批量发货" OnClick="btnSendGoods_Click" DeleteMsg="将当前选中结果中筛选出已付款未发货的订单进行批量发货，是否继续？" IsShow="true"/>
		</li>
        </ul>
      </div>  </div>
		<input type="hidden" id="hidOrderId" runat="server" />
		<!--数据列表区域-->
	  <div class="datalist">
	  <asp:DataList ID="dlstOrders" runat="server" DataKeyField="OrderId" OnItemDataBound="dlstOrders_ItemDataBound" OnItemCommand="dlstOrders_ItemCommand" Width="100%">
	  
	  <HeaderTemplate>
      <table width="0" border="0" cellspacing="0">
		    <tr class="table_title">
		      <td width="24%" class="td_right td_left">会员名</td>
		      <td width="20%" class="td_right td_left">收货人</td>
		      <td width="18%" class="td_right td_left">支付方式</td>
		      <td width="12%" class="td_right td_left">订单实收款(元)</td>
		      <td width="12%" class="td_right td_left">订单状态</td>
		      <td width="12%" class="td_left td_right_fff">发货</td>
	        </tr>
	    </HeaderTemplate>
         <ItemTemplate>
	   
	   <tr class="td_bg">
		      <td><input name="CheckBoxGroup" type="checkbox" value='<%#Eval("OrderId") %>' />订单编号：<%#Eval("OrderId") %><asp:Literal ID="group" runat="server" Text='<%# Convert.ToInt32(Eval("GroupBuyId"))>0?"(团)":"" %>' /></td>
		      <td>成交时间：<Hi:FormatedTimeLabel ID="lblStartTimes" Time='<%#Eval("OrderDate") %>' ShopTime="true" runat="server" ></Hi:FormatedTimeLabel></td>
		      <td><%# (bool)Eval("IsPrinted")?"已打印":"未打印" %></td>
		      <td>&nbsp;</td>
		      <td>&nbsp;</td>
		      <td align="right">		       
		        <a href="javascript:RemarkOrder('<%#Eval("OrderId") %>','<%#Eval("OrderDate") %>','<%#Eval("OrderTotal") %>','<%#Eval("ManagerMark") %>','<%#Eval("ManagerRemark") %>');" ><Hi:OrderRemarkImage runat="server" DataField="ManagerMark" ID="OrderRemarkImageLink" /></a>
		      </td>
	        </tr>
		    <tr>
		      <td><asp:HyperLink runat="server" target="_blank" NavigateUrl='<%# Globals.GetAdminAbsolutePath(string.Format("/member/MemberDetails.aspx?userId={0}", Eval("UserId")))%>' Enabled='<%# Eval("UserId").ToString()!="1100" %>' ><%#Eval("UserName")%></asp:HyperLink>
		       <Hi:WangWangConversations runat="server" ID="WangWangConversations"  WangWangAccounts='<%#Eval("Wangwang") %>'/> </td>
		      <td><%#Eval("ShipTo") %>&nbsp;</td>
		      <td><%#Eval("PaymentType") %></td>
		      <td><Hi:FormatedMoneyLabel ID="lblOrderTotal" Money='<%#Eval("OrderTotal") %>' runat="server" />
		        <span class="Name"><asp:HyperLink ID="lkbtnEditPrice" runat="server" NavigateUrl='<%# "EditOrder.aspx?OrderId="+ Eval("OrderId") %>'  Target="_blank" Text="修改价格" Visible="false" ForeColor="Blue"></asp:HyperLink></span>
		        <a href="javascript:CloseOrder('<%#Eval("OrderId") %>');"><asp:Literal runat="server" ID="litCloseOrder" Visible="false" Text="关闭订单" /></a> 
		       </td>
		      <td><Hi:OrderStatusLabel ID="lblOrderStatus" OrderStatusCode='<%# Eval("OrderStatus") %>' runat="server" /> 
               <span class="Name"><Hi:OrderDetailsHyperLink ID="lkbtnOrderDetails" OrderStatusCode='<%# Eval("OrderStatus") %>' OrderId='<%# Eval("OrderId") %>' Target="_blank" Text="详情" runat="server" ForeColor="BlueViolet"/></span>
                <span class="Name"><Hi:ImageLinkButton ID="lkbtnPayOrder" runat="server" Text="我已线下收款" CommandArgument='<%# Eval("OrderId") %>' CommandName="CONFIRM_PAY" OnClientClick="return ConfirmPayOrder()" Visible="false" ForeColor="Red"></Hi:ImageLinkButton>
                <Hi:OrderRefundStatusMark runat="server" ID="hpkOrderRefund" NavigateUrl='<%# "RefundOrderDetails.aspx?OrderId="+ Eval("OrderId") %>' Target="_blank" Status ='<%# Eval("RefundStatus") %>'></Hi:OrderRefundStatusMark></span></td>
		      <td>
		        <a href='<%# "PrintData.aspx?OrderId="+ Eval("OrderId") %>' target="_blank">打印快递单</a>
		        <a href='<%#" PrintOrder.aspx?OrderId="+Eval("OrderId") %>' target="_blank">购</a>
		        <a href='<%#" PrintPackingOrder.aspx?OrderId="+Eval("OrderId") %>' target="_blank">配</a>
		        <span class="submit_faihuo"> <asp:HyperLink ID="lkbtnSendGoods" runat="server" NavigateUrl='<%# "SendOrderGoods.aspx?OrderId="+ Eval("OrderId") %>' Target="_blank" Text="发货" Visible="false"  ForeColor="Red"></asp:HyperLink></span>
		        <Hi:ImageLinkButton ID="lkbtnConfirmOrder" IsShow="true" runat="server" Text="完成订单" CommandArgument='<%# Eval("OrderId") %>' CommandName="FINISH_TRADE"  DeleteMsg="确认要完成该订单吗？" Visible="false" ForeColor="Red" />
		     </td>
	        </tr>
	   </ItemTemplate>
	   <FooterTemplate>
	   </table>
	   </FooterTemplate>  
        </asp:DataList>
        <div class="instantstat clearfix" id="divSendOrders">
				注：订单状态列中有“退”字代表该订单退过款；有“(团)”字的代表团购订单
			</div>
      <div class="blank5 clearfix"></div>
      
  </div>
  
    <!--数据列表底部功能区域-->
	  <div class="page">
	 <div class="page">
	  <div class="bottomPageNumber clearfix">
			<div class="pageNumber">
				<div class="pagination">
            <UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
               </div>

			</div>
		</div>
      </div>
      </div>

</div>


<div class="Pop_up" id="RemarkOrder"  style="display:none;">
  <h1>编辑备注信息 </h1>
  <div class="img_datala"><img src="../images/icon_dalata.gif" width="38" height="20" /></div>
  <div class="mianform">
    <ul>
              <li><span class="formitemtitle Pw_128">订单编号：</span><span id="spanOrderId" runat="server" /></li>
       <li><span class="formitemtitle Pw_128">成交时间：</span><span runat="server" ID="lblOrderDateForRemark" /></li>
              <li><span class="formitemtitle Pw_128">订单实收款(元)：</span><strong class="colorA"><Hi:FormatedMoneyLabel ID="lblOrderTotalForRemark" runat="server" /></strong></li>
              <li><span class="formitemtitle Pw_128">标志：</span>
                <span><Hi:OrderRemarkImageRadioButtonList runat="server" ID="orderRemarkImageForRemark" /></span>
                </li>
              <li><span class="formitemtitle Pw_128">备忘录：</span>
          <asp:TextBox ID="txtRemark" TextMode="MultiLine" runat="server" Width="300" Height="50" />
              </li>
        </ul>
         <ul class="up Pa_100">
         <asp:Button runat="server" ID="btnRemark" Text="确定" OnClick="btnRemark_Click" CssClass="submit_DAqueding"/>
       </ul>
  </div>
 
</div>

<div class="Pop_up" id="closeOrder" style="display:none;">
  <h1>关闭订单 </h1>
  <div class="img_datala"><img src="../images/icon_dalata.gif" width="38" height="20" /></div>
  <div class="mianform fonts colorA borbac">关闭交易?请您确认已经通知买家,并已达成一致意见,您单方面关闭交易,将可能导致交易纠纷</strong></div>
  <div class="mianform ">
    <ul>
      <li><span class="formitemtitle Pw_160">关闭该订单的理由：</span> <abbr class="formselect">
        <Hi:CloseTranReasonDropDownList runat="server" ID="ddlCloseReason" />
      </abbr> </li>
    </ul>
    <ul class="up Pa_160">
      <asp:Button ID="btnCloseOrder"  runat="server" CssClass="submit_DAqueding" OnClick="btnCloseOrder_Click" OnClientClick="return ValidationCloseReason()" Text="确 定"   />
    </ul>
  </div>
</div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
 <script type="text/javascript">
     function ConfirmPayOrder() {
         return confirm("如果客户已经通过其他途径支付了订单款项，您可以使用此操作修改订单状态\n\n此操作成功完成以后，订单的当前状态将变为已付款状态，确认客户已付款？");
     }

     function ShowOrderState() {
         var status;
         if (navigator.appName.indexOf("Explorer") > -1) {

             status = document.getElementById("ctl00_contentHolder_lblStatus").innerText;

         } else {

             status = document.getElementById("ctl00_contentHolder_lblStatus").textContent;

         }
         if (status != "0") {
             document.getElementById("anchors0").className = 'optionstar';
         }
         if (status != "99") {
             document.getElementById("anchors99").className = 'optionend';
         }
         document.getElementById("anchors" + status).className = 'menucurrent';
     }

     $(document).ready(function() { ShowOrderState(); });

     function RemarkOrder(OrderId, OrderDate, OrderTotal, managerMark, managerRemark) {
         $("#ctl00_contentHolder_spanOrderId").html(OrderId);
         $("#ctl00_contentHolder_hidOrderId").val(OrderId);
         $("#ctl00_contentHolder_lblOrderDateForRemark").html(OrderDate);
         $("#ctl00_contentHolder_lblOrderTotalForRemark").html(OrderTotal);
         $("#ctl00_contentHolder_txtRemark").val(managerRemark);

         for (var i = 0; i < 5; i++) {
             if (document.getElementById("ctl00_contentHolder_orderRemarkImageForRemark_" + i).value == managerMark)
                 document.getElementById("ctl00_contentHolder_orderRemarkImageForRemark_" + i).checked = true;
             else
                     document.getElementById("ctl00_contentHolder_orderRemarkImageForRemark_" + i).checked = false;
         }
         
         DivWindowOpen(500,350,'RemarkOrder');
     }
     
     function CloseOrder(orderId)
     {
          $("#ctl00_contentHolder_hidOrderId").val(orderId);
          DivWindowOpen(560,250,'closeOrder');
     }
     
     function ValidationCloseReason() {
             var reason = document.getElementById("ctl00_contentHolder_ddlCloseReason").value;
             if (reason == "请选择关闭的理由") {
                 alert("请选择关闭的理由");
                 return false;
             }

             return true;
         }
     </script>
</asp:Content>
