<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PurchaseOrder_Charges.ascx.cs" Inherits="Hidistro.UI.Web.Admin.PurchaseOrder_Charges" %>

<h1>采购单实收款结算</h1>
        <div class="Settlement">
        <table width="200" border="0" cellspacing="0">
          <tr>
            <td width="15%" align="right">运费(元)：</td>
            <td width="12%"><asp:Literal ID="litFreight" runat="server" /> (<asp:Literal ID="lblModeName" runat="server" />)</td>
            <td width="73%"><span class="Name"><asp:LinkButton runat="server" Visible="false" ID="lkBtnEditshipingMode" OnClientClick="DivWindowOpen(400,200,'setShippingMode');return false;" Text="修改配送方式" /></span></td>
          </tr>
          <tr>
            <td align="right">采购单选项费用(元)：</td>
            <td colspan="2"><asp:Literal ID="litOptionPrice" runat="server" /><small class="colorE"> <asp:Literal ID="litOderItem" runat="server" /></small></td>
          </tr>
          <tr>
            <td align="right">涨价或折扣(元)： </td>
            <td colspan="2" class="colorB"><asp:Literal ID="litDiscount" runat="server" /></td>
          </tr>
          <tr class="bg">
            <td align="right" class="colorG">采购单实收款(元)：</td>
            <td colspan="2"> <strong class="colorG fonts"><asp:Literal ID="litTotalPrice" runat="server" /></strong></td>
          </tr>
        </table>
    </div>
    
