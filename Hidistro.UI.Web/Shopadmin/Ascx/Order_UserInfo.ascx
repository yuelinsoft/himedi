<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Order_UserInfo.ascx.cs" Inherits="Hidistro.UI.Web.Shopadmin.Order_UserInfo" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
  
<div class="State">
      <h1>订单详情</h1>
	    <table width="200" border="0" cellspacing="0">
              <tr>
                <td width="10%" align="right">订单编号：</td>
                <td width="20%"><asp:Label ID="lblOrderId" runat ="server" ></asp:Label></td>
                <td width="10%" align="right">联系电话：</td>
                <td width="30%"><asp:Literal ID="UserTel" runat="server"></asp:Literal></td>
                <td width="10%" align="right">会员名：</td>
                <td width="20%"><asp:Literal ID="UserName" runat="server"></asp:Literal></td>
              </tr>
              <tr>
                <td align="right">真实姓名：</td>
                <td><asp:Literal ID="UserRealName" runat="server"></asp:Literal></td>
                <td align="right">电子邮件：</td>
                <td><a  runat="server" id="email"><asp:Literal ID="UserEmail" runat="server"></asp:Literal></a> <asp:HyperLink ID="lkbtnMessage" runat ="server" Text="发送站内信" ></asp:HyperLink></td>
                <td align="right"> <asp:Label ID="lblpayTimeTitle" Text="付款时间：" Visible="false" runat="server" ></asp:Label></td>
                <td><Hi:FormatedTimeLabel ID="lblPayTime"  ShopTime="true" runat="server"  ></Hi:FormatedTimeLabel> </td>
              </tr>
              <tr>      
              <td align="right"><asp:Label ID="lblSendGoodTime"  Visible="false" runat="server" Text="发货时间：" /></td>
              <td><Hi:FormatedTimeLabel ID="lblOrderSendGoodsTime" runat="server" Visible="false"></Hi:FormatedTimeLabel></td>
              <td align="right"><asp:Label ID="lblFinishTime" Text="完成时间：" Visible="false" runat="server" /></td>
              <td><Hi:FormatedTimeLabel ID="lblOrderOverTime" runat="server" Visible="false"></Hi:FormatedTimeLabel></td>
              <td >&nbsp;</td>
              <td>&nbsp;</td>
              </tr>
        </table>
	  </div>