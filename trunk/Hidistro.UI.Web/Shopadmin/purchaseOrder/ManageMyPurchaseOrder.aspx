<%@ Page Language="C#" MasterPageFile="~/ShopAdmin/ShopAdmin.Master" AutoEventWireup="true" CodeFile="ManageMyPurchaseOrder.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.ManageMyPurchaseOrder"  %>
<%@ Import Namespace="Hidistro.Core"%>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>

<%@ Import Namespace="Hidistro.Membership.Context" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

<!--选项卡-->
	<div class="optiongroup mainwidth">
	  <ul>
			<li id="anchors0"><asp:HyperLink ID="hlinkAllOrder" runat="server"><span>所有采购单</span></asp:HyperLink></li>
			<li id="anchors1"><asp:HyperLink ID="hlinkNotPay" runat="server"><span>等待付款</span></asp:HyperLink></li>
			<li id="anchors2"><asp:HyperLink ID="hlinkYetPay" runat="server"><span>等待发货</span></asp:HyperLink></li>
            <li id="anchors3"><asp:HyperLink ID="hlinkSendGoods" runat="server"><span>已发货</span></asp:HyperLink></li>    
            <li id="anchors5"><asp:HyperLink ID="hlinkFinish" runat="server"><span>成功采购单</span></asp:HyperLink></li>                     
            <li id="anchors4"><asp:HyperLink ID="hlinkClose" runat="server"><span>已关闭</span></asp:HyperLink></li>
            <li id="anchors99"><asp:HyperLink ID="hlinkHistory" runat="server"><span>历史采购单</span></asp:HyperLink></li>                                                                             
		</ul>
</div>
	<!--选项卡-->

<div class="dataarea mainwidth">
		<!--搜索-->
		<div class="searcharea clearfix">
		  <ul>
		    <li> <span>选择时间段：</span><span>
		      <UI:WebCalendar CalendarType="StartDate" ID="calendarStartDate" runat="server" cssclass="forminput" />
		      </span> <span class="Pg_1010">至</span> <span>
		       <UI:WebCalendar ID="calendarEndDate" runat="server" CalendarType="EndDate" cssclass="forminput" />
	        </span></li>
   
      <li><span>商品名称：</span><span>
		      <asp:TextBox ID="txtProductName" runat="server" cssclass="forminput" />
      </span></li>
      </ul>
		  <div class="blank12 clearfix"></div>
		  <ul>
              <li><span>订单编号：</span><span>
		      <asp:TextBox ID="txtOrderId" runat="server" cssclass="forminput" />
		      </span></li>
		    <li><span>采购单编号：</span><span>
		     <asp:TextBox ID="txtPurchaseOrderId" runat="server" cssclass="forminput"></asp:TextBox> <asp:Label ID="lblStatus" runat="server" style="display:none;"></asp:Label>
		      </span></li>
		    <li>
		      <asp:Button ID="btnSearchButton" runat="server" class="searchbutton" Text="查询" />
	        </li>
	      </ul>
     </div>
		<!--结束-->
            
            <div class="functionHandleArea clearfix">
			<!--分页功能-->
			<div class="pageHandleArea">
				<ul>
					<li class="paginalNum"><span>每页显示数量：</span><UI:PageSize runat="server" ID="hrefPageSize" /></li>
				</ul>
			</div>
			<div class="pageNumber">
				<div class="pagination">
            <UI:Pager runat="server" ShowTotalPages="false" ID="pager" />
           </div>
			</div>
			<!--结束-->
</div>

		
		<!--数据列表区域-->
		<div class="datalist">
		<div>
		<input type="hidden" id="hidPurchaseOrderId" runat="server" />
		<asp:DataList ID="dlstPurchaseOrders" runat="server" DataKeyField="PurchaseOrderId" Width="100%">
	   <HeaderTemplate>
	   <table width=" 0" border="0" cellspacing="0">
		     <tr class="table_title">
		      <td width="24%" class="td_right td_left">收货人</td>
		      <td width="25%" class="td_right td_left">订单实收款(元)</td>
		      <td width="22%" class="td_right td_left">采购单实收款(元)</td>
		      <td width="18%" class="td_right td_left">采购状态</td>
		      <td width="12%" class="td_left td_right_fff">操作</td>
	        </tr>
	   </HeaderTemplate>
	  <ItemTemplate>
	    <tr class="td_bg">
		      <td>订单编号：<%#Eval("OrderId") %></td>
		      <td>采购单编号：<%#Eval("PurchaseOrderId") %></td>
		      <td>成交时间：<Hi:FormatedTimeLabel ID="lblStartTimes" Time='<%#Eval("PurchaseDate") %>' ShopTime="true" runat="server" ></Hi:FormatedTimeLabel></td>
		      <td>&nbsp;</td>
		      <td align="right">&nbsp;</td>
	        </tr>
		    <tr>
		      <td><%# Eval("ShipTo") %></td>
		      <td><Hi:FormatedMoneyLabel ID="lblOrderTotal" Money='<%#Eval("OrderTotal") %>' runat="server" /></td>
		      <td> <Hi:FormatedMoneyLabel ID="lblPurchaseTotal" Money='<%#Eval("PurchaseTotal") %>' runat="server" /></td>
		      <td>
		       <table border="0" style="border:none;" width="100%" cellpadding="0" cellspacing="0">
		              <tr><td style="border:none;"><Hi:PuchaseStatusLabel runat="server" ID="lblPurchaseStatus" PuchaseStatusCode='<%# Eval("PurchaseStatus") %>' Font-Bold="true"  /></td>
		              <td style="border:none;" rowspan="2" align="left"><Hi:OrderRefundStatusMark runat="server" ID="OrderRefundStatusMark" NavigateUrl='<%#"RefundPurchaseDetails.aspx?PurchaseOrderId="+Eval("PurchaseOrderId" )%>' Status='<%# Eval("RefundStatus") %>' /></td></tr>
		               <tr><td style="border:none;">
		                <span class="Name float"><Hi:DistributorPurchaseOrderDetailsHyperLink ID="lkbtnPurchaseOrderDetails" PurchaseStatusCode='<%# Eval("PurchaseStatus") %>' PurchaseOrderId ='<%# Eval("PurchaseOrderId") %>' Target="_blank" Text="查看详情"  runat="server" /></span>
		                <span><Hi:DistributorChangePurchaseOrderItemsHyperLink ID="lkbtnUpdatePurchaseOrder" PurchaseStatusCode='<%# Eval("PurchaseStatus") %>' PurchaseOrderId ='<%# Eval("PurchaseOrderId") %>' Target="_blank" Text="修改"  runat="server" /></span>
		               </td></tr>
		              </table>		      
		    
		      </td>
		      <td>
		       <span class="submit_faihuo"><asp:HyperLink ID="lkbtnPay" runat="server" NavigateUrl='<%# "Pay.aspx?PurchaseOrderId="+ Eval("PurchaseOrderId") %>'  Target="_blank" Text="付款" Visible="false"></asp:HyperLink></span>
		        <span class="submit_tongyi"><asp:HyperLink ID="lkbtnSendGoods" runat="server" NavigateUrl='<%#Globals.ApplicationPath +  "/Shopadmin/sales/SendMyGoods.aspx?OrderId="+ Eval("OrderId") %>' Target="_blank" Text="辅助发货" Visible="false"></asp:HyperLink> </span>
		        <div runat="server" visible="false" id="lkBtnCancelPurchaseOrder"><span class="submit_tongyi"><a href="javascript:ShowCloseDiv('<%# Eval("PurchaseOrderId")%>');">取消采购</a></span></div>&nbsp;
		        <Hi:ImageLinkButton ID="lkbtnConfirmPurchaseOrder" IsShow="true" runat="server" Text="确认采购单" CommandArgument='<%# Eval("PurchaseOrderId") %>' CommandName="FINISH_TRADE"  DeleteMsg="确认要完成该采购单吗？" Visible="false" ForeColor="Red" />
		      </td>
	        </tr>
	   </ItemTemplate>
	   <FooterTemplate>
	   </table>
	   </FooterTemplate>
	 </asp:DataList>
	 <div class="instantstat clearfix" id="divSendOrders">
				注：采购单状态列中有“退”字代表该采购单退过款
			</div>
		  <div class="blank5 clearfix"></div>
  </div>
  </div>
  
  <!--数据列表底部功能区域-->
	  <div class="page">
	  <div class="bottomPageNumber clearfix">
			<div class="pageNumber">
				<div class="pagination">
            <UI:Pager runat="server" ShowTotalPages="true" ID="pager1" />
           </div>
			</div>
		</div>
      </div>

	</div>
	<div class="databottom"></div>
		

	 
	 <!--关闭采购单-->
<div class="Pop_up" id="ClosePurchaseOrder" style="display:none;">
  <h1>关闭采购单 </h1>
  <div class="img_datala"><img src="../images/icon_dalata.gif" width="38" height="20" /></div>
  <div class="mianform fonts colorA borbac"><strong>取消采购单?请选择取消采购单的理由：</strong></div>
  <div class="mianform ">
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
     function showOrderState() {
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

     $(document).ready(function() { showOrderState(); });

     function ValidationCloseReason() {
         var reason = document.getElementById("ctl00_contentHolder_ddlCloseReason").value;
         if (reason == "请选择取消的理由") {
             alert("请选择取消的理由");
             return false;
         }

         return true;
     }

     function ShowCloseDiv(purchaseOrderId) {

         $("#ctl00_contentHolder_hidPurchaseOrderId").val(purchaseOrderId);

         DivWindowOpen(550, 200, 'ClosePurchaseOrder');
     }

     </script>
</asp:Content>
