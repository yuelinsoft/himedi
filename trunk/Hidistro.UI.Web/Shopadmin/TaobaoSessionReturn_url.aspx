<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TaobaoSessionReturn_url.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.TaobaoSessionReturn_url" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
    <style>
    .clearfix{clear:both;}
    </style>
</head>
<body>

    <form id="form1" runat="server">
     <div  style="background-color:#FFFFC6;width:630px;height:250px; margin:200px auto;border:1px solid #FFFFC6">
       <div class="clearfix"></div>
        <div style="margin:90px auto;width:500px;font-weight:700; font-size:15px;">
            <div style="float:left; background:url(../Admin/images/ico.gif) no-repeat left top; width:50px;height:50px;"></div>
            <asp:Label runat="server" ID="lblMsg" Text="正在转到第三方登录页面，请稍候..."></asp:Label>
            <span style="margin:5px 5px;font-size:12px;font-weight:normal;float:left;">　<asp:Literal ID="litlink" runat="server"></asp:Literal></span>
        </div>
        <div class="clearfix"></div>
    </div>
    
    
    <div>
        
    </div>
    </form>
</body>
</html>
