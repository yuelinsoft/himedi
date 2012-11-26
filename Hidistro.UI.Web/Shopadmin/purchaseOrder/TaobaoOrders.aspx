<%@ Page Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeFile="TaobaoOrders.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.TaobaoOrders" Title="无标题页" %>
<%@ Import Namespace="Hidistro.Entities.Sales"%>
<%@ Import Namespace="Hidistro.Core"%>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
<div runat="server" id="content1" class="dataarea mainwidth td_top_ccc">		<!--搜索-->
        		<div class="toptitle">
		  <em><img src="../images/03.gif" width="32" height="32" /></em>
		  
          <h1 class="title_height"><strong>淘宝订单生成采购单</strong></h1>
          <span class="title_height"><font color="red">1.若选择在此处将淘宝订单生成采购单，请不要再次使用好易抓抓取淘宝订单来提交采购单，以免相同淘宝订单重复生成采购单。<br />2.若淘宝订单中同时包含分销站发布到淘宝的商品和非分销站商品，则该淘宝订单生成采购单时，采购单中只包含分销站发布到淘宝的商品。</font></span> 
		</div>
		<div class="searcharea clearfix br_search">
		  <ul>
		    <li> <span>选择时间段：</span><span>
		      <UI:WebCalendar CalendarType="StartDate" ID="calendarStartDate" runat="server" cssclass="forminput" />
		      </span> <span class="Pg_1010">至</span> <span>
		        <UI:WebCalendar ID="calendarEndDate" runat="server" CalendarType="EndDate" cssclass="forminput" />
		        </span></li>
		    <li><span>买家昵称：</span><span>
		      <asp:TextBox ID="txtBuyerName" runat="server" cssclass="forminput" />
		    </span></li>
		    
		    <li>
		      <asp:Button ID="btnSearch" runat="server" class="searchbutton" Text="查询" />
	        </li>
	      </ul>
  </div>
		<!--结束-->

      <div class="functionHandleArea clearfix m_none">
          <span>配送方式：</span>
            <hi:ShippingModeDropDownList runat="server" AllowNull="true" ID="shippingModeDropDownList" NullToDisplay="--请选择配送方式--" />  
        <span style="color:#888; margin-left:10px;">(请为选中的淘宝订单选择配送方式，以便生成采购单时的运费计算。)</span>
      </div>

         <div class="functionHandleArea clearfix m_none">
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
		   <span class="purchaseSelect"><a href="javascript:void(0)" onclick="ConvertPurchaseOrder()">将选中订单生成采购单</a></span> 
		</li>
        </ul>
      </div>
      <div class="filterClass">
        </div>  
      </div>
		<!--数据列表区域-->
	  <div class="datalist">
	  <div>
	  <asp:Repeater ID="rptTrades" runat="server">
        <ItemTemplate>   
            <table>
	        <tr class="td_bg">
		          <td>
		            <Hi:ConvertStatusLabel runat="server" />
		             订单编号：<%#Eval("tid")%>
		          </td>
		          <td colspan="3">成交时间：<%#Eval("created") %></td>
		     </tr>
		     <tr>
		        <td style="width:50%;">
		            <asp:Repeater ID="dlstOrders" runat="server" DataSource='<%#Eval("orders") %>'>
		             <ItemTemplate>   
		              <table>
		                  <tr>
		                      <td style="width:20%;">
		                            <a target="_blank" href='<%# "http://item.taobao.com/item.htm?id=" + Eval("NumIid") %>'><img src='<%#Eval("PicPath") %>'width="78" height="62"/></a>
		                       </td>
		                       <td style="width:50%;">
		                           <a target="_blank" href='<%# "http://item.taobao.com/item.htm?id=" + Eval("NumIid") %>'> <%#Eval("title")%> </a>
		                           <div><%#Eval("OuterSkuId")%></div>
		                           <div><%#Eval("SkuPropertiesName")%></div>
		                       </td>
		                       <td style="width:20%;">
		                            <%#Eval("price")%> 
		                      </td>
		                      <td style="width:10%;"><%#Eval("num")%> </td>
		                </tr>
		            </table>
		            </ItemTemplate>
		            </asp:Repeater>
		        </td>
		        <td style="width:20%;">
		            <%#Eval("BuyerNick")%><br />
		             <%#Eval("ReceiverName")%>
		        </td>
		        <td style="width:15%;">
		            <%#Eval("status")%><br />
		            <a target="_blank" href='<%# "http://trade.taobao.com/trade/detail/trade_item_detail.htm?bizOrderId=" + Eval("tid") %>'>详情</a>
		         </td>
		        <td style="width:15%;">
		            <%#Eval("payment")%><br />
		            (含<%#Eval("ShippingType")%>：<%#Eval("PostFee")%>)
		        </td>
	        </tr>
	        </table>
	    </ItemTemplate>
      </asp:Repeater>
      <div class="blank5 clearfix"></div>
  </div>
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
<div runat="server" id="content12" visible="false" class="dataarea mainwidth td_top_ccc">
    <asp:Literal ID="txtMsg" runat="server" />
</div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">

    <script type="text/javascript" >       
    function ConvertPurchaseOrder(){
        var shippingModeId = $("#ctl00_contentHolder_shippingModeDropDownList").val();
        if(shippingModeId == ""){
            alert("请选择一个配送方式");
            return "";
        }
        var orderIds = GetOrderId();
        if(orderIds.length > 0)
            window.open("TaobaoOrderConvertPurchaseOrder.aspx?OrderIds=" + orderIds + "&shippingModeId=" + shippingModeId);
    }    
    
    function GetOrderId(){
        var v_str = "";

        $("input[type='checkbox'][name='CheckBoxGroup']:checked").each(function(rowIndex, rowItem){
            v_str += $(rowItem).attr("value") + ",";
        });
        
        if(v_str.length == 0){
            alert("请选择将要转换的订单");
            return "";
        }
        return v_str.substring(0, v_str.length - 1);        
    }
</script>
</asp:Content>
