<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PurchaseOrder_DistributorInfo.ascx.cs" Inherits="Hidistro.UI.Web.Admin.PurchaseOrder_DistributorInfo" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<div class="State">
 <h1>采购单详情</h1>
	    <table width="200" border="0" cellspacing="0">
              <tr>
                <td width="10%" align="right">订单编号：</td>
                <td width="20%"><asp:Literal ID="litOrderId" runat="server"></asp:Literal></td>
                <td width="10%" align="right">采购单编号：</td>
                <td width="28%"><asp:Literal ID="LitPurchaseOrderId" runat="server"></asp:Literal></td>
                <td width="10%" align="right">分销商用户名：</td>
                <td width="20%"><asp:Literal ID="distributorName" runat="server"></asp:Literal></td>
              </tr>
              <tr>
                <td align="right">真实姓名：</td>
                <td><asp:Literal ID="distributorRealName" runat="server"></asp:Literal></td>
                <td align="right">电子邮件：</td>
                <td><a  runat="server" id="email"><asp:Literal ID ="lkbtnEmail" runat ="server" ></asp:Literal></a>&nbsp;<asp:HyperLink ID="lkbtnSendMessage" runat="server" Text="发送站内信"></asp:HyperLink> </td>
                <td align="right"> 联系电话：</td>
                <td><asp:Literal ID="distributorTel" runat="server"></asp:Literal></td>
              </tr>
        </table>
</div>

