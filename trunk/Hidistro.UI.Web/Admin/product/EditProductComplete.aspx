<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeFile="EditProductComplete.aspx.cs" Inherits="Hidistro.UI.Web.Admin.EditProductComplete" Title="无标题页" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="areacolumn clearfix">
		
      <div class="columnright">
          <div class="title">
            <em><img src="../images/03.gif" width="32" height="32" /></em>
            <h1>编辑商品成功</h1>
            <span>商品编辑成功，您还可以进行以下操作：</span>
</div>
          <div class="formitem">
          <span class="msg">商品编辑成功！</span>
         </div>
          <div class="Pg_15 Pg_45 fonts"><span class="float">你可以</span>
            <asp:HyperLink ID="hlinkProductDetails" runat="server" Target="_blank" Text="查看" />商品
        </div>
		  <div class="Pg_15 Pg_45 fonts">您可以随时到  <span class="Name"><a href="ProductOnSales.aspx">出售中的商品</a> 或 <a href="ProductInStock.aspx">仓库里的商品</a> </span>继续编辑其它商品。</div>
		  <div class="Pg_15 Pg_45 fonts"><asp:Button runat="server" ID="btnSave" Text="关 闭" OnClientClick="javascript:window.close();" CssClass="submit_DAqueding inbnt" /></div>
      </div>
        
  </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
