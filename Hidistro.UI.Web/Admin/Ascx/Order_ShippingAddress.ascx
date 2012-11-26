<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Order_ShippingAddress.ascx.cs" Inherits="Hidistro.UI.Web.Admin.Order_ShippingAddress" %>
   
<h1>物流信息</h1>
        <div class="Settlement">
        <table width="100%" border="0" cellspacing="0">
         <tr id="tr_company" runat="server" visible="false">
            <td align="right">物流公司：</td>
            <td colspan="2" width="85%"><asp:Literal ID="litCompanyName" runat="server" /></td>
          </tr>
          <tr>
            <td width="15%" align="right">收货地址：</td>
            <td width="60%"><asp:Literal ID="lblShipAddress" runat="server" /></td>
            <td width="25%"><span class="Name"><asp:LinkButton runat="server" ID="lkBtnEditShippingAddress" Text="修改收货地址" OnClientClick="javascript:DivWindowOpen(560,400,'dlgShipTo');return false;" Visible="false"></asp:LinkButton></span></td>
          </tr>
          <tr>
            <td align="right">配送方式：</td>
            <td colspan="2" width="85%"><asp:Literal ID="litModeName" runat="server" /></td>
          </tr>
          <tr>
            <td align="right" nowrap="nowrap">买家留言：</td>
            <td colspan="2" ><asp:Label ID="litRemark"  runat="server" style="word-wrap: break-word; word-break: break-all;"/></td>
          </tr>
        </table>
        </div>
