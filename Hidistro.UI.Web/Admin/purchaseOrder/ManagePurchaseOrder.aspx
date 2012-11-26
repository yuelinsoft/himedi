<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeFile="ManagePurchaseOrder.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ManagePurchaseOrder"  %>
<%@ Import Namespace="Hidistro.Core"%>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
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
            <li  id="anchors5"><asp:HyperLink ID="hlinkTradeFinished" runat="server" Text=""><span>成功采购单</span></asp:HyperLink></li>       
            <li id="anchors4"><asp:HyperLink ID="hlinkClose" runat="server"><span>已关闭</span></asp:HyperLink></li>
            <li id="anchors99"><asp:HyperLink ID="hlinkHistory" runat="server"><span>历史采购单</span></asp:HyperLink></li>                                                                             
		</ul>
	</div>
	<!--选项卡-->
<input type="hidden" runat="server" id="lblPurchaseOrderId" />
<div class="dataarea mainwidth">
		<!--搜索-->
		<div class="searcharea clearfix br_search">
		  <ul  class="a_none_left">
		    <li> <span>选择时间段：</span><span>
		     <UI:WebCalendar CalendarType="StartDate" ID="calendarStartDate" runat="server" cssclass="forminput" />
		      </span> <span class="Pg_1010">至</span> <span>
		       <UI:WebCalendar ID="calendarEndDate" runat="server" CalendarType="EndDate" cssclass="forminput" />
		        </span></li>
		    <li><span>分销商名称：</span><span>
		      <asp:TextBox ID="txtDistributorName" runat="server" cssclass="forminput" />
		      </span></li>
		      <li><span>商品名称：</span><span>
		      <asp:TextBox ID="txtProductName" runat="server" cssclass="forminput" />
		      </span></li>
		      <li><span>订单编号：</span><span>
		      <asp:TextBox ID="txtOrderId" runat="server" cssclass="forminput" />
		      </span></li>		      
		    <li><span>采购单编号：</span><span>
		      <asp:TextBox ID="txtPurchaseOrderId" runat="server" cssclass="forminput"></asp:TextBox> <asp:Label ID="lblStatus" runat="server" style="display:none;"></asp:Label>
		      </span></li>
		      <li><span>收货人：</span><span>
		      <asp:TextBox ID="txtShopTo" runat="server" cssclass="forminput"></asp:TextBox>
		      </span></li> 
		      <li><span>配送方式：</span><span>
		        <abbr class="formselect"><hi:ShippingModeDropDownList runat="server" AllowNull="true" ID="shippingModeDropDownList" /></abbr>
		      </span></li>
		      <li style="width:150px;"><span>打印状态：</span><span>
		        <abbr class="formselect">
		        <asp:DropDownList runat="server" ID="ddlIsPrinted" /></abbr>
		      </span></li>
		    <li>
		      <asp:Button ID="btnSearchButton" runat="server" class="searchbutton" Text="查询" />
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
            <UI:Pager runat="server" ShowTotalPages="false" ID="pager" />
           </div>
			</div>
			<!--结束-->
			 <div class="blank8 clearfix"></div>
      <div class="batchHandleArea">
        <ul>
          <li class="batchHandleButton"> <span class="signicon"></span> 
          <span class="allSelect"><a href="javascript:void(0)" onclick="SelectAll()">全选</a></span>
		  <span class="reverseSelect"><a href="javascript:void(0)" onclick=" ReverseSelect()">反选</a></span>
		  <span class="delete"><Hi:ImageLinkButton ID="lkbtnDeleteCheck" runat="server" Text="删除" IsShow="true"/></span>
		  <Hi:ImageLinkButton ID="btnBatchPrintData" runat="server" Text="批量打印快递单" DeleteMsg="将当前选中结果创建快递单打印任务列表，是否继续？" IsShow="true"/>
          <Hi:ImageLinkButton ID="btnBatchSendGoods" runat="server" Text="批量发货" DeleteMsg="将当前选中结果中筛选出已付款未发货的订单进行批量发货，是否继续？" IsShow="true"/>
		  </li>
        </ul>
      </div>
		</div>
		 <input type="hidden" id="hidPurchaseOrderId" runat="server" />
        		<!--数据列表区域-->
	  <div class="datalist">	  
	   <asp:DataList ID="dlstPurchaseOrders" runat="server" DataKeyField="PurchaseOrderId" Width="100%">
	   <HeaderTemplate>
	   <table width=" 0" border="0" cellspacing="0">
		    <tr class="table_title">
		      <td width="24%" class="td_right td_left">分销商</td>
		      <td width="20%" class="td_right td_left">收货人</td>
		      <td width="12%" class="td_right td_left">订单实收款(元)</td>
		      <td width="12%" class="td_right td_left">采购单实收款(元)</td>
		      <td width="18%" class="td_right td_left">采购状态</td>
		      <td width="12%" class="td_left td_right_fff">发货</td>
	        </tr>
	   </HeaderTemplate>
	  <ItemTemplate>	   
	   <tr class="td_bg">
		      <td><input name="CheckBoxGroup" type="checkbox" value='<%#Eval("PurchaseOrderId") %>'>采购单编号：<%#Eval("PurchaseOrderId") %></td>
		      <td>成交时间：<Hi:FormatedTimeLabel ID="lblStartTimes" Time='<%#Eval("PurchaseDate") %>' ShopTime="true" runat="server" ></Hi:FormatedTimeLabel></td>
		      <td><%# (bool)Eval("IsPrinted")?"已打印":"未打印" %></td>
		      <td>&nbsp;</td>
		      <td>&nbsp;</td>
		      <td align="right"><a href="javascript:RemarkPurchaseOrder('<%#Eval("PurchaseOrderId") %>','<%#Eval("OrderId") %>','<%#Eval("PurchaseDate") %>','<%#Eval("PurchaseTotal") %>','<%#Eval("ManagerMark") %>','<%#  Eval("ManagerRemark") %>')"><Hi:OrderRemarkImage runat="server" DataField="ManagerMark" ID="OrderRemarkImageLink" /></a></td>
	        </tr>   
	    
		    <tr>
		      <td><%#Eval("Distributorname") %> <Hi:WangWangConversations runat="server" ID="WangWangConversations"  WangWangAccounts='<%#Eval("DistributorWangwang") %>'/>  </td>
		      <td><%#Eval("ShipTo") %>&nbsp;</td>
		      <td><Hi:FormatedMoneyLabel ID="lblOrderTotal" Money='<%#Eval("OrderTotal") %>' runat="server" /></td>
		      <td><Hi:FormatedMoneyLabel ID="lblPurchaseTotal" Money='<%#Eval("PurchaseTotal") %>' runat="server" />
		        <span class="Name"><div runat="server" visible="false" id="lkbtnEditPurchaseOrder"><a href="javascript:void(0);" onclick="OpenWindow('<%# Eval("PurchaseOrderId")%>','<%# Eval("PurchaseTotal")%>')">修改采购单价格</a></div></span>
		        <a href="javascript:ClosePurchaseOrder('<%#Eval("PurchaseOrderId") %>');"><asp:Literal runat="server" ID="litClosePurchaseOrder" Visible="false" Text="关闭采购单" /></a> 
		      </td>
		      <td>
		      <table border="0" cellpadding="0" cellspacing="0" style="border:none;">
		      <tr><td style="border:none;"><Hi:PuchaseStatusLabel runat="server" ID="lblPurchaseStatus" PuchaseStatusCode='<%# Eval("PurchaseStatus") %>'  /> </td>
		      <td rowspan="2" style="border:none;"><Hi:OrderRefundStatusMark runat="server" ID="OrderRefundStatusMark" NavigateUrl='<%#"RefundPurchaseOrderDetails.aspx?PurchaseOrderId="+Eval("PurchaseOrderId" )%>' Status='<%# Eval("RefundStatus") %>' /></td></tr>
		      <tr><td style="border:none;"> <span class="Name"><Hi:PurchaseOrderDetailsHyperLink ID="lkbtnPurchaseOrderDetails" PurchaseStatusCode='<%# Eval("PurchaseStatus") %>' PurchaseOrderId ='<%# Eval("PurchaseOrderId") %>' Target="_blank" Text="查看详情" runat="server"/></span>
		        <Hi:PurchaseOrderItemUpdateHyperLink Target="_blank" runat="server" PurchaseOrderId='<%# Eval("PurchaseOrderId") %>' PurchaseStatusCode='<%# Eval("PurchaseStatus") %>' DistorUserId='<%# Eval("DistributorId") %>' Text="修改采购商品" />
		      </td></tr>
		      </table>	      
		      
	             </td>
		      <td>
		        <a href='<%# "../sales/PurchasePrintData.aspx?PurchaseOrderId="+ Eval("PurchaseOrderId") %>' target="_blank">打印快递单</a>
		        <a href='<%#" PrintPurchaseOrder.aspx?purchaseOrderId="+Eval("purchaseOrderId") %>' target="_blank">购</a>
		        <a href='<%#" PrintPurchasePackingOrder.aspx?purchaseOrderId="+Eval("purchaseOrderId") %>' target="_blank">配</a>
		        <span class="submit_faihuo"><asp:HyperLink ID="lkbtnSendGoods" runat="server" NavigateUrl='<%# "SendPurchaseOrderGoods.aspx?PurchaseOrderId="+ Eval("PurchaseOrderId") %>' Target="_blank" Text="发货" Visible="false"  ForeColor="Red"></asp:HyperLink></span>
		           <span class="Name"><Hi:ImageLinkButton ID="lkbtnPayOrder" runat="server" Text="我已线下收款" CommandArgument='<%# Eval("PurchaseOrderId") %>' CommandName="CONFIRM_PAY" OnClientClick="return ConfirmPayOrder()" Visible="false" ForeColor="Red"></Hi:ImageLinkButton>
		        <Hi:ImageLinkButton ID="lkbtnConfirmPurchaseOrder" IsShow="true" runat="server" Text="完成采购单" CommandArgument='<%# Eval("PurchaseOrderId") %>' CommandName="FINISH_TRADE"  DeleteMsg="确认要完成该采购单吗？" Visible="false" ForeColor="Red" />
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
      <div class="blank12 clearfix"></div>
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
</div>

	<div class="databottom"></div>
	
<div class="bottomarea testArea">
  <!--顶部logo区域-->
</div>


 
 <!--修改价格-->
<div class="Pop_up" id="EditPurchaseOrder" style="display:none;">
  <h1>修改价格 </h1>
    <div class="img_datala"><img src="../images/icon_dalata.gif" width="38" height="20" /></div>
    <div class="mianform">
    <ul>
              <li> <span class="formitemtitle Pw_128">采购单原实收款：</span>
                   <strong class="colorA fonts"><asp:Label ID="lblPurchaseOrderAmount" Text="22"  runat="server"/></strong> 元 
              </li>
              <li> <span class="formitemtitle Pw_128">涨价或折扣：<em>*</em></span>
                  <asp:TextBox ID="txtPurchaseOrderDiscount" runat="server" cssclass="forminput" onblur="ChargeAmount()" /> 负数代表折扣，正数代表涨价
              </li>
              <li> <span class="formitemtitle Pw_128">分销商实付：</span>
                    <asp:Label ID="lblPurchaseOrderAmount1" Text="22" runat="server" /><span>+</span>
                    <asp:Label ID="lblPurchaseOrderAmount2" Text="22" runat="server" /><span>=</span>
                    <strong class="colorA fonts"><asp:Label ID="lblPurchaseOrderAmount3" Text="22"  runat="server" /></strong>元
              </li>
              <li> <span class="formitemtitle Pw_128">分销商实付：</span>
                  采购单原实收款+涨价或折扣
              </li>
        </ul>
        <ul class="up Pa_128">
          <asp:Button ID="btnEditOrder"  runat="server"  Text="确定" CssClass="submit_DAqueding"   />  
       </ul>
      </div>
</div>          
      
<!--编辑备注信息-->
   <div class="Pop_up" id="RemarkPurchaseOrder"  style="display:none;">
  <h1>编辑备注信息 </h1>
  <div class="img_datala"><img src="../images/icon_dalata.gif" width="38" height="20" /></div>
  <div class="mianform">
    <ul>
              <li><span class="formitemtitle Pw_128">订单编号：</span><span id="spanOrderId" runat="server" /></li>
              <li><span class="formitemtitle Pw_128">采购单编号：</span><span id="spanpurcharseOrderId" runat="server" /></li>
       <li><span class="formitemtitle Pw_128">成交时间：</span><span runat="server" ID="lblpurchaseDateForRemark" /></li>
              <li><span class="formitemtitle Pw_128">采购单实收款(元)：</span><strong class="colorA"><Hi:FormatedMoneyLabel ID="lblpurchaseTotalForRemark" runat="server" /></strong></li>
              <li><span class="formitemtitle Pw_128">标志：</span>
                <span><Hi:OrderRemarkImageRadioButtonList runat="server" ID="orderRemarkImageForRemark" /></span>
                </li>
              <li><span class="formitemtitle Pw_128">备忘录：</span>
          <span><asp:TextBox ID="txtRemark" TextMode="MultiLine" runat="server" Width="300" Height="50" /></span>
              </li>
        </ul>
         <ul class="up Pa_128 clear">
         <asp:Button runat="server" ID="btnRemark" Text="确定" CssClass="submit_DAqueding"/>
       </ul>
  </div> 
</div>

<div class="Pop_up" id="closePurchaseOrder" style="display:none;">
  <h1>关闭采购单 </h1>
  <div class="img_datala"><img src="../images/icon_dalata.gif" width="38" height="20" /></div>
  <div class="mianform fonts colorA borbac"><strong>关闭交易?请您确认已经通知分销商,并已达成一致意见,您单方面关闭交易,将可能导致交易纠纷</strong></div>
  <div class="mianform ">
    <ul>
      <li><span class="formitemtitle Pw_160">关闭该采购单的理由：</span> <abbr class="formselect">
        <Hi:ClosePurchaseOrderReasonDropDownList runat="server" ID="ddlCloseReason" />
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
     function ConfirmPayOrder() {
         return confirm("如果分销商已经通过其他途径支付了采购单款项，您可以使用此操作修改采购单状态\n\n此操作成功完成以后，采购单的当前状态将变为已付款状态，确认分销商已付款？");
     }
     
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
         if ($("#ctl00_contentHolder_txtPurchaseOrderDiscount").val("")) {
             $("#ctl00_contentHolder_lblPurchaseOrderAmount2").html("0.00");
         }
         initValid(new InputValidator('ctl00_contentHolder_txtPurchaseOrderDiscount', 1, 10, true, '(0|^-?(0+(\\.[0-9]{1,2}))|^-?[1-9]\\d*(\\.\\d{1,2})?)', '折扣只能是数值，且不能超过2位小数'))
         appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtPurchaseOrderDiscount', -10000000, 10000000, '折扣只能是数值，不能超过10000000，且不能超过2位小数'));
     }

         $(document).ready(function() { showOrderState(); });


         function OpenWindow(PurchaseOrderId, PurchaseTotal) {
             $("#ctl00_contentHolder_lblPurchaseOrderId").val(PurchaseOrderId);
             $("#ctl00_contentHolder_lblPurchaseOrderAmount").html(PurchaseTotal);
             $("#ctl00_contentHolder_lblPurchaseOrderAmount1").html(PurchaseTotal);
             $("#ctl00_contentHolder_lblPurchaseOrderAmount3").html(PurchaseTotal);
             $("#ctl00_contentHolder_lblPurchaseOrderAmount2").html("0.00");
             
             DivWindowOpen(520,300,'EditPurchaseOrder');
         }

         function RemarkPurchaseOrder(purchaseOrderId, orderId, purchaseDate, purchaseTotal, managerMark, managerRemark) {
             $("#ctl00_contentHolder_spanOrderId").html(orderId);
             $("#ctl00_contentHolder_hidPurchaseOrderId").val(purchaseOrderId);
             $("#ctl00_contentHolder_spanpurcharseOrderId").html(purchaseOrderId);
             $("#ctl00_contentHolder_lblpurchaseDateForRemark").html(purchaseDate);
             $("#ctl00_contentHolder_lblpurchaseTotalForRemark").html(purchaseTotal);
             $("#ctl00_contentHolder_txtRemark").val(managerRemark);

             for (var i = 0; i < 6; i++) {
                 if (document.getElementById("ctl00_contentHolder_orderRemarkImageForRemark_" + i).value == managerMark)
                     document.getElementById("ctl00_contentHolder_orderRemarkImageForRemark_" + i).checked = true;
                 else
                     document.getElementById("ctl00_contentHolder_orderRemarkImageForRemark_" + i).checked = false;
             }
          DivWindowOpen(520,400,'RemarkPurchaseOrder');
         }

         function ChargeAmount() {
                     var reg = /^\-?([1-9]\d*|0)(\.\d+)?$/;
                     if (($("#ctl00_contentHolder_txtPurchaseOrderDiscount").val() != "") && reg.test($("#ctl00_contentHolder_txtPurchaseOrderDiscount").val())) {
                         $("#ctl00_contentHolder_lblPurchaseOrderAmount2").html($("#ctl00_contentHolder_txtPurchaseOrderDiscount").val());
                         var amount1 = parseFloat($("#ctl00_contentHolder_lblPurchaseOrderAmount").html());
                         var amount2 = parseFloat($("#ctl00_contentHolder_lblPurchaseOrderAmount2").html());

                         var amount3 = amount1 + amount2;
                         $("#ctl00_contentHolder_lblPurchaseOrderAmount3").html(amount3);
                     }
                 }

        function ClosePurchaseOrder(purchaseOrderId)
         {
              $("#ctl00_contentHolder_hidPurchaseOrderId").val(purchaseOrderId);
              DivWindowOpen(560,250,'closePurchaseOrder');
         }
         
         function ValidationCloseReason() {
             var reason = document.getElementById("ctl00_contentHolder_ddlCloseReason").value;
             if (reason == "请选择退回的理由") {
                 alert("请选择退回的理由");
                 return false;
             }

             return true;
         }   
     </script>
</asp:Content>
