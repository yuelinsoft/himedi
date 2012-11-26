<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PurchaseOrder_ShippingAddress.ascx.cs" Inherits="Hidistro.UI.Web.Admin.PurchaseOrder_ShippingAddress" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<h1>物流信息</h1>
        <div class="Settlement">
        <table width="200" border="0" cellspacing="0">
         <tr id="tr_company" runat="server" visible="false">
            <td align="right">物流公司：</td>
            <td colspan="2"><asp:Literal ID="litCompanyName" runat="server" /></td>
          </tr>
          <tr>
            <td width="15%" align="right">收货地址：</td>
            <td width="60%"><asp:Literal ID="lblShipAddress" runat="server" /></td>
            <td width="25%"><span class="Name"><asp:LinkButton runat="server" ID="lkBtnEditShippingAddress" Text="修改收货地址" OnClientClick="DivWindowOpen(600,400,'dlgShipTo');return false;" Visible="false"></asp:LinkButton></span></td>
          </tr>
          <tr>
            <td align="right">配送方式：</td>
            <td colspan="2"><asp:Literal ID="litModeName" runat="server" /></td>
          </tr>
          <tr>
            <td align="right">成交时间：</td>
            <td colspan="2"><Hi:FormatedTimeLabel ID="lblPurchaseDate" runat="server" /></td>
          </tr>
           <tr>
            <td align="right" nowrap="nowrap">买家留言：</td>
            <td colspan="2" ><asp:Label ID="litRemark"  runat="server" style="word-wrap: break-word; word-break: break-all;"/></td>
          </tr>
        </table>
        </div>
