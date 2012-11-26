<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TaobaoOrderConvertPurchaseOrder.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.TaobaoOrderConvertPurchaseOrder" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
       <div  style="background-color:#FFFFC6;width:630px;height:250px; margin:200px auto;border:1px solid #FFFFC6">
        <div style="margin:90px auto;width:500px;font-weight:700; font-size:15px;">
            <div style="float:left; background:url(../Admin/images/ico.gif) no-repeat left -50px; width:50px;height:50px;"></div>
            <asp:Literal ID="litmsg" runat="server" Text=""></asp:Literal> 
            <span style="margin:5px 5px;font-size:12px;font-weight:normal;float:left;">您可以　<a href="taobaoorders.aspx">继续生成淘宝订单</a></span>
        </div>
    </div>
    </form>
</body>
</html>
