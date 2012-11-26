<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeFile="ManageMyManualPurchaseOrder.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.ManageMyManualPurchaseOrder" %>
<%@ Import Namespace="Hidistro.Core"%>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server"> 
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
		      <UI:WebCalendar CalendarType="StartDate" ID="calendarStartDate" runat="server" CssClass="forminput" />
		      </span> <span class="Pg_1010">至</span> <span>
		        <UI:WebCalendar ID="calendarEndDate" runat="server" CalendarType="EndDate" CssClass="forminput" />
		        </span></li>
		    <li><span>收货人名称：</span><span>
		      <asp:TextBox runat="server" ID="txtShipTo" CssClass="forminput" />
		      </span></li>
		      </ul>
		      <div class="blank12 clearfix"></div>
		      <ul>
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
<input type="hidden" id="hidPurchaseOrderId" runat="server" />
		
		<!--数据列表区域-->
	  <div class="datalist">
	  <div>
	   <asp:DataList ID="dlstPurchaseOrders" runat="server" DataKeyField="PurchaseOrderId" Width="100%">
	   <HeaderTemplate>
	   <table width=" 0" border="0" cellspacing="0">
		     <tr class="table_title">
		      <td width="10%" class="td_right td_left">收货人</td>
		      <td width="20%" class="td_right td_left">采购单编号</td>
		      <td width="15%" class="td_right td_left">采购单实收款(元)</td>
		      <td colspan="2" class="td_right td_left">采购状态</td>
		      <td width="20%" class="td_right td_left">成交时间</td>
	        </tr>
	   </HeaderTemplate>
	  <ItemTemplate>
	   <tr>
		      <td><%#Eval("ShipTo") %> </td>
		      <td><%#Eval("PurchaseOrderId") %>
		        <asp:Literal runat="server" ID="litTbOrderDetailLink" /></td>
		      <td><Hi:FormatedMoneyLabel ID="lblPurchaseTotal" Money='<%#Eval("PurchaseTotal") %>' runat="server" /></td>
		      <td width="15%">
		              <table border="0" style="border:none;" width="100%" cellpadding="0" cellspacing="0">
		              <tr><td style="border:none;"> <span class="colorE"> <Hi:PuchaseStatusLabel runat="server" ID="lblPurchaseStatus" PuchaseStatusCode='<%# Eval("PurchaseStatus") %>' Font-Bold="true"  /></span></td>
		              <td style="border:none;" rowspan="2" align="left"><Hi:OrderRefundStatusMark runat="server" ID="OrderRefundStatusMark" NavigateUrl='<%#"RefundPurchaseDetails.aspx?PurchaseOrderId="+Eval("PurchaseOrderId" )%>' Status='<%# Eval("RefundStatus") %>' /></td></tr>
		               <tr><td style="border:none;"><span class="Name float"><Hi:DistributorManualPurchaseOrderDetailsHyperLink ID="lkbtnPurchaseOrderDetails" PurchaseStatusCode='<%# Eval("PurchaseStatus") %>' PurchaseOrderId ='<%# Eval("PurchaseOrderId") %>' Target="_blank" Text="查看详情"  runat="server" /></span>
		                    <span><Hi:DistributorChangePurchaseOrderItemsHyperLink ID="lkbtnUpdatePurchaseOrder" PurchaseStatusCode='<%# Eval("PurchaseStatus") %>' PurchaseOrderId ='<%# Eval("PurchaseOrderId") %>' Target="_blank" Text="修改"  runat="server" /></span>
		               </td></tr>
		              </table>		     
	          </td>
		      <td width="20%"><span class="submit_faihuo"><asp:HyperLink ID="lkbtnPay" runat="server" NavigateUrl='<%# "Pay.aspx?PurchaseOrderId="+ Eval("PurchaseOrderId") %>'  Target="_blank" Text="付款" Visible="false"/></span>
		        <span class="submit_tongyi" ><div runat="server" visible="false"  id="lkbtnClosePurchaseOrder"><a href="javascript:void(0);" onclick="ShowCloseDiv('<%#Eval("PurchaseOrderId") %>')" id="btnClose">取消采购</a></div></span> &nbsp;
		        <Hi:ImageLinkButton ID="lkbtnConfirmPurchaseOrder" IsShow="true" runat="server" Text="确认采购单" CommandArgument='<%# Eval("PurchaseOrderId") %>' CommandName="FINISH_TRADE"  DeleteMsg="确认要完成该采购单吗？" Visible="false" ForeColor="Red" />
		     </td>
		      <td><Hi:FormatedTimeLabel ID="lblPurchaseDate" Time='<%#Eval("PurchaseDate") %>' ShopTime="true" runat="server" ></Hi:FormatedTimeLabel></td>
	        </tr>	 
	   </ItemTemplate>
	   <FooterTemplate>
	   </table>
	   </FooterTemplate>
	 </asp:DataList>
      <div class="instantstat clearfix" id="divSendOrders">
				注：采购单状态列中有“退”字代表该采购单退过款
		</div>
	</div>
</div>
<div class="blank5 clearfix"></div>
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
