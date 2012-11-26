<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.Default" CodeFile="Default.aspx.cs" MasterPageFile="~/Admin/Admin.Master" %>
<%@ Import Namespace="Hidistro.Core"%>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
	  <div class="datalist">
	  <div class="businessinfo">
		<div class="leftbusiness">
			<div class="welcomemessage clearfix">
			<ul>
				<li class="managername"><span><asp:Literal runat="server" ID="ltrAdminName"></asp:Literal> 欢迎回来.</span>
					<span class="message"></span><asp:HyperLink ID="hpkMessages" runat="server" ></asp:HyperLink>
					<span class="zixun"></span><asp:HyperLink ID="hpkZiXun" runat="server" ></asp:HyperLink>
					<span class="liuyan"></span><asp:HyperLink ID="hpkLiuYan" runat="server" ></asp:HyperLink>
				</li>
				<li class="lastlogintime">上次登录：<Hi:FormatedTimeLabel ID="lblTime" runat="server"></Hi:FormatedTimeLabel></li>
			</ul>
			</div>
			<div class="orderlist clearfix">
				<div class="optiontitle">
					<ul>
						<li id="fen"><span><a href="Default.aspx?Status=0" style="color:BlueViolet;">分销商采购单</a></span></li>
						<li id="hui"><span><a href="Default.aspx?Status=1" style="color:BlueViolet;">会员订单</a></span></li>
					</ul>
				</div>
				<input type="hidden" runat="server" id="hidStatus" />
				<div class="orderdata" id="divPurchaseOrders">
				
				
				 <UI:Grid ID="grdPurchaseOrders" runat="server" SortOrderBy="PurchaseDate" SortOrder="DESC" AutoGenerateColumns="False" DataKeyNames="PurchaseOrderId" HeaderStyle-CssClass="border_background"  GridLines="None" AllowSorting="true" Width="100%" ShowHeader="false">
                    <Columns>
                                                
                        <asp:TemplateField HeaderText="成交时间" SortExpression="PurchaseDate" HeaderStyle-CssClass="datetime">
                            <itemtemplate>
                               <Hi:FormatedTimeLabel ID="lblStartTimes" Time='<%#Eval("PurchaseDate") %>' ShopTime="true" runat="server" ></Hi:FormatedTimeLabel>
                            </itemtemplate>
                        </asp:TemplateField>                        
                        
                        <asp:TemplateField HeaderText="采购单信息" HeaderStyle-CssClass="info">
                         <itemtemplate>
  <Hi:PurchaseOrderDetailsHyperLink ID="lkbtnPurchaseOrderDetail" PurchaseStatusCode='<%# Eval("PurchaseStatus") %>' PurchaseOrderId ='<%# Eval("PurchaseOrderId") %>' Target="_blank"  runat="server" ForeColor="BlueViolet" >

                               <asp:Label ID="lblName" Text='<%#Eval("Distributorname") %>' runat="server" ></asp:Label>
                               <asp:Label ID="lblDistroStatus" runat="server"></asp:Label>
                           </Hi:PurchaseOrderDetailsHyperLink>
                            </itemtemplate>
                        </asp:TemplateField>
                        
                         <asp:TemplateField HeaderText="采购单状态" HeaderStyle-CssClass="estate">
                            <itemtemplate>
                               <Hi:PuchaseStatusLabel runat="server" ID="lblPurchaseStatus" PuchaseStatusCode='<%# Eval("PurchaseStatus") %>'  />
                               <Hi:OrderRefundStatusMark runat="server" ID="OrderRefundStatusMark" NavigateUrl='<%# Globals.ApplicationPath + "/Admin/purchaseOrder/RefundPurchaseOrderDetails.aspx?PurchaseOrderId="+Eval("PurchaseOrderId" )%>' Status='<%# Eval("RefundStatus") %>' />
                            </itemtemplate>
                        </asp:TemplateField>
                                             
                        <asp:TemplateField HeaderText="采购单操作" HeaderStyle-CssClass="handle">
                            <itemtemplate>
                              <Hi:PurchaseOrderDetailsHyperLink ID="lkbtnPurchaseOrderDetails" PurchaseStatusCode='<%# Eval("PurchaseStatus") %>' PurchaseOrderId ='<%# Eval("PurchaseOrderId") %>' Target="_blank" Text="查看详情" runat="server" ForeColor="BlueViolet" /><br />
	                           <asp:HyperLink ID="lkbtnSendGoods" runat="server" NavigateUrl='<%# Globals.ApplicationPath + "/Admin/purchaseOrder/SendPurchaseOrderGoods.aspx?PurchaseOrderId="+ Eval("PurchaseOrderId") %>' Target="_blank" Text="发货" Visible="false"  ForeColor="Red"></asp:HyperLink>
                              <div runat="server" visible="false" id="lkbtnEditPurchaseOrder"><a href="javascript:void(0);" onclick="OpenWindow('<%# Eval("PurchaseOrderId")%>','<%# Eval("PurchaseTotal")%>')">修改采购单价格</a></div>                           </ItemTemplate>
                        </asp:TemplateField>
                      
                    </Columns>
                    <EmptyDataTemplate><span class="empty">最近记录为空</span></EmptyDataTemplate>
                  </UI:Grid>
				</div>
				
				
				 <input type="hidden" id="lblPurchaseOrderId" runat="server"  />
   <div class="Pop_up" id="EditPurchaseOrder" style="display:none;">
  <h1>修改价格 </h1>
    <div class="img_datala"><img src="./images/icon_dalata.gif" width="38" height="20" /></div>
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

				<div class="orderdata" id="divOrders">
					  <UI:Grid ID="grdOrders" runat="server" SortOrderBy="OrderDate" SortOrder="Desc" AutoGenerateColumns="False" DataKeyNames="OrderId" HeaderStyle-CssClass="border_background"  GridLines="None" ShowHeader="false" AllowSorting="true" Width="100%">
                    <Columns>
                     
                        <asp:TemplateField HeaderText="下单时间" SortExpression="OrderDate" HeaderStyle-CssClass="datetime" >
                            <itemtemplate>
                               <Hi:FormatedTimeLabel ID="lblStartTimes" Time='<%#Eval("OrderDate") %>' ShopTime="true" runat="server" ></Hi:FormatedTimeLabel>
                            </itemtemplate>
                        </asp:TemplateField>                        
                         <asp:TemplateField HeaderText="订单号" SortExpression="OrderId" HeaderStyle-CssClass="info" >
                         <itemtemplate>     
                            <asp:Label ID="lblPurchaseOrderID" Text='<%#Eval("OrderId") %>' runat="server" ></asp:Label><asp:Literal ID="group" runat="server" Text='<%# Convert.ToInt32(Eval("GroupBuyId"))>0?"(团)":"" %>' />
                         </itemtemplate> 
                         </asp:TemplateField>                        
                       
                        <asp:TemplateField HeaderText="订单状态" HeaderStyle-CssClass="estate" >
                            <itemtemplate>
                               <Hi:OrderStatusLabel ID="lblOrderStatus" OrderStatusCode='<%# Eval("OrderStatus") %>' runat="server" /> <br />
                               <Hi:OrderRefundStatusMark runat="server" ID="hpkOrderRefund" NavigateUrl='<%#Globals.ApplicationPath+( "/Admin/sales/RefundOrderDetails.aspx?OrderId="+ Eval("OrderId")) %>' Target="_blank" Status ='<%# Eval("RefundStatus") %>'></Hi:OrderRefundStatusMark>
                                                           </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderStyle-CssClass="handle">
                            <itemtemplate>
                               <Hi:OrderDetailsHyperLink ID="lkbtnOrderDetails" OrderStatusCode='<%# Eval("OrderStatus") %>' OrderId='<%# Eval("OrderId") %>' Target="_blank" Text="详情" runat="server" ForeColor="BlueViolet"/><br />
	                           <asp:HyperLink ID="lkbtnEditPrice" runat="server" NavigateUrl='<%# Globals.ApplicationPath +  "/Admin/sales/EditOrder.aspx?OrderId="+ Eval("OrderId") %>'  Target="_blank" Text="修改价格" Visible="false" ForeColor="Blue"></asp:HyperLink>
	                           <asp:HyperLink ID="lkbtnSendGoods" runat="server" NavigateUrl='<%# Globals.ApplicationPath +  "/Admin/sales/SendOrderGoods.aspx?OrderId="+ Eval("OrderId") %>' Target="_blank" Text="发货" Visible="false" ForeColor="Red"></asp:HyperLink>
                            </itemtemplate>
                        </asp:TemplateField>
                    </Columns>
                     <EmptyDataTemplate><span class="empty">最近记录为空</span></EmptyDataTemplate>
                  </UI:Grid>            
				</div>
			
			</div>
			
			
			<div class="instantstat clearfix" id="divSendOrders">
				注：这里显示最近的 <asp:Label ID="lblOrderNumbers" runat="server"></asp:Label> 笔订单，有“退”字代表该订单退过款，您共有<asp:HyperLink ID="hpksendOrder" runat="server"  Target="_blank"  ForeColor="Blue"></asp:HyperLink>笔订单待发货。<asp:HyperLink ID="allorders" runat="server"   Target="_blank" Text="查看所有会员订单"  ForeColor="Blue"></asp:HyperLink>
	                           
			</div>
			
			<div class="instantstat clearfix" id="divSendPurchseOrders">
				注：这里显示最近的<asp:Label ID="lblPurchaseOrderNumbers" runat="server"></asp:Label>笔采购单，有“退”字代表该采购单退过款，您共有<asp:HyperLink ID="hpksendPurchaseOrder" runat="server"   Target="_blank"   ForeColor="Blue"></asp:HyperLink>笔采购单待发货。<asp:HyperLink ID="allPurchaseOrder" runat="server"   Target="_blank" Text="查看所有分销商采购单"  ForeColor="Blue"></asp:HyperLink>
			</div>
		</div>
		<div class="rightbusiness">
			<div class="statistics">
				<h3>统计</h3>
				<ul>
					<li><span class="explain">今日订单金额(元)：</span><span class="statresult"><Hi:ClassShowOnDataLitl runat="server"  DefaultText="￥0.00" Id="lblTodayOrderAmout" /></span></li>
					<li><span class="explain">今日销售利润(元)：</span><span class="statresult"><Hi:ClassShowOnDataLitl runat="server"  DefaultText="￥0.00" Id="lblTodaySalesProfile" /></span></li>
					<li><span class="explain">今日新增会员：</span><span class="statresult"><Hi:ClassShowOnDataLitl runat="server"  DefaultText="0" Id="ltrTodayAddMemberNumber" /></span></li>
					<li><span class="explain">今日新增分销商：</span><span class="statresult"><Hi:ClassShowOnDataLitl runat="server"  DefaultText="0" Id="ltrTodayAddDistroNumber" /></span></li>
					<li><span class="explain">库存报警商品：</span><span class="statresult"><Hi:ClassShowOnDataLitl runat="server"  DefaultText="" Id="lblProductCountTotal" /></span></li>
					<li><span class="explain">待发货采购单：</span><span class="statresult"><Hi:ClassShowOnDataLitl runat="server"  DefaultText="0" Id="ltrWaitSendPurchaseOrdersNumber" /></span></li>
					<li><span class="explain">待发货订单：</span><span class="statresult"><Hi:ClassShowOnDataLitl runat="server"  DefaultText="0" Id="ltrWaitSendOrdersNumber" /></span></li>
					<li><span class="explain">会员预付款总额(元)：</span><span class="statresult"><Hi:ClassShowOnDataLitl runat="server"  DefaultText="￥0.00" Id="lblMembersBalanceTotal" /></span></li>
					<li><span class="explain">分销商预付款总额(元)：</span><span class="statresult"><Hi:ClassShowOnDataLitl runat="server"  DefaultText="￥0.00" Id="lblDistrosBalanceTotal" /></span></li>
					<li><span class="explain">预付款总额(元)：</span><span class="statresult"><Hi:ClassShowOnDataLitl runat="server"  DefaultText="￥0.00" Id="lblAllBalanceTotal" /></span></li>

				</ul>
			</div>

		</div>
	  </div>
	  </div>

	  <div class="blank12 clearfix"></div>	  
	  <script type="text/javascript">
	      $(document).ready(function() {
	          if ($("#ctl00_contentHolder_hidStatus").val() == "1") {
	                orders();   
	          }
	          else {
	              purchaseOrders();
	          }
	      });
	      
	      
	      function orders() {
	          document.getElementById("divOrders").style.display = "block";
	          document.getElementById("divPurchaseOrders").style.display = "none";
	          document.getElementById("divSendOrders").style.display = "block";
	          document.getElementById("divSendPurchseOrders").style.display = "none";
	          document.getElementById("fen").className = "";
	          document.getElementById("hui").className = "menucurrent";
	      }


	      function purchaseOrders() {
	          document.getElementById("divOrders").style.display = "none";
	          document.getElementById("divPurchaseOrders").style.display = "block";
	          document.getElementById("divSendOrders").style.display = "none";
	          document.getElementById("divSendPurchseOrders").style.display = "block";
	          document.getElementById("fen").className = "menucurrent";
	          document.getElementById("hui").className = "";
	      }
	      
	      function OpenWindow(PurchaseOrderId, PurchaseTotal) {
	          $("#ctl00_contentHolder_lblPurchaseOrderId").val(PurchaseOrderId);
	          $("#ctl00_contentHolder_lblPurchaseOrderAmount").html(PurchaseTotal);
	          $("#ctl00_contentHolder_lblPurchaseOrderAmount1").html(PurchaseTotal);
	          $("#ctl00_contentHolder_lblPurchaseOrderAmount3").html(PurchaseTotal);
	          $("#ctl00_contentHolder_lblPurchaseOrderAmount2").html("0.00");	         
	         
	         DivWindowOpen(560,300,'EditPurchaseOrder');
	      }


	      function ChargeAmount() {
	          var reg = /^\-?([1-9]\d*|0)(\.\d+)?$/;
	          if ((document.getElementById("ctl00_contentHolder_txtPurchaseOrderDiscount").value != "") && reg.test(document.getElementById("ctl00_contentHolder_txtPurchaseOrderDiscount").value)) {
	              document.getElementById("ctl00_contentHolder_lblPurchaseOrderAmount2").innerHTML = document.getElementById("ctl00_contentHolder_txtPurchaseOrderDiscount").value;
	              var amount1 = parseFloat(document.getElementById("ctl00_contentHolder_lblPurchaseOrderAmount").innerHTML);
	              var amount2 = parseFloat(document.getElementById("ctl00_contentHolder_lblPurchaseOrderAmount2").innerHTML);

	              var amount3 = amount1 + amount2;
	              document.getElementById("ctl00_contentHolder_lblPurchaseOrderAmount3").innerHTML = amount3;
	          }
	      }	      
	  </script>	 
	  
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server"></asp:Content>

