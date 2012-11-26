﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Order_ItemsList.ascx.cs" Inherits="Hidistro.UI.Web.Admin.Order_ItemsList" %>
<%@ Import Namespace="Hidistro.Core"%>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>


    <h1>商品列表</h1>    
    <asp:DataList ID="dlstOrderItems" runat="server"  Width="100%" >
         <HeaderTemplate>
      <table width="200" border="0" cellspacing="0">
	    <tr class="table_title">
	      <td colspan="2" class="td_right td_left">商品名称</td>
	      <td width="12%" class="td_right td_left">商品单价(元) </td>
	      <td width="10%" class="td_right td_left">购买数量 </td>
	      <td width="12%" class="td_right td_left">发货数量 </td>
	      <td width="12%" class="td_left td_right_fff">总价(元) </td>
        </tr>
        </HeaderTemplate>
        <ItemTemplate>        
          <tr>
	      <td width="7%"><a href='<%#"../../ProductDetails.aspx?ProductId="+Eval("ProductId") %>' target="_blank">
                                <Hi:ListImage ID="HiImage2"  runat="server" DataField="ThumbnailsUrl" /></a> </td>
	      <td width="32%"><span class="Name"><a href='<%#"../../ProductDetails.aspx?ProductId="+Eval("ProductId") %>' target="_blank">
	        <%# Eval("ItemDescription")%></a></span> <span class="colorC">货号：<asp:Literal runat="server" ID="litCode" Text='<%#Eval("sku") %>' />
	        <asp:Literal ID="litSKUContent" runat="server" Text='<%# Eval("SKUContent") %>'></asp:Literal></span>
	      <span><a href='<%# Globals.ApplicationPath+"/FavourableDetails.aspx?activityId="+Eval("PurchaseGiftId") %>' target="_blank"><%# Eval("PurchaseGiftName")%></a>
                            <a href='<%# Globals.ApplicationPath+"/FavourableDetails.aspx?activityId="+Eval("WholesaleDiscountId") %>' target="_blank"><%# Eval("WholesaleDiscountName")%></a></span></td>
	      <td><Hi:FormatedMoneyLabel ID="lblItemListPrice" runat="server" Money='<%# Eval("ItemAdjustedPrice") %>' /></td>
	      <td>×<asp:Literal runat="server" ID="litQuantity" Text='<%#Eval("Quantity") %>' /></td>
	      <td>×<asp:Literal runat="server" ID="litShipmentQuantity" Text='<%#Eval("ShipmentQuantity") %>' /></td>
	      <td><strong class="colorG"><Hi:FormatedMoneyLabel ID="FormatedMoneyLabelForAdmin2"  runat="server"  Money='<%# (decimal)Eval("ItemAdjustedPrice")*(int)Eval("Quantity") %>'/></strong></td>
        </tr>        
        </ItemTemplate>
        <FooterTemplate>
      </table>
      </FooterTemplate>
      </asp:DataList>
      
	  <div class="Price">
	    <table width="200" border="0" cellspacing="0">
	       <tr class="bg">
	      <td class="Pg_top td_none" width="88%" align="right" >商品金额（元）：</td>
	      <td class="Pg_top td_none" width="12%" ><strong class="fonts colorG"><Hi:FormatedMoneyLabel ID="litGoodsAmount" runat="server" /></strong></td>
        </tr>
	    <tr class="bg">
	      <td class="Pg_bot" align="right">商品总重量（克）：</td>
	      <td class="Pg_bot" ><strong class="fonts "><asp:Literal ID="lblWeight" runat="server" /></strong></td>
        </tr>
        </table>
	  </div>
	  
	  <h1><asp:Label ID="lblOrderGifts" runat="server" Text="礼品列表"></asp:Label> </h1>
	  <asp:DataList ID="grdOrderGift" runat="server" DataKeyField="GiftId" Width="100%" >
         <HeaderTemplate>
      <table width="200" border="0" cellspacing="0">
        <tr class="table_title">
            <td width="170" class="td_right td_left">礼品名称</td>
            <td width="30" class="td_right td_left">数量 </td>
          </tr>
        </HeaderTemplate>
        <ItemTemplate>
        <tr>
            <td ><Hi:HiImage ID="HiImage1" AutoResize="true" Width="60" Height="60" runat="server" DataField="ThumbnailsUrl" /> <span><asp:Literal ID="giftName" runat="server" Text='<%# Eval("GiftName") %>'></asp:Literal></span> </td>
            
            <td>×<asp:Literal ID="litQuantity" runat="server" Text='<%# Eval("Quantity") %>'  ></asp:Literal></td>
            
        </tr>
        </ItemTemplate>
        <FooterTemplate>
      </table>
      </FooterTemplate>
      </asp:DataList>





