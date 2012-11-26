<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeFile="AddProductTypeFinish.aspx.cs" Inherits="Hidistro.UI.Web.Admin.product.AddProductTypeFinish" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

	<div class="areacolumn clearfix">
		<div class="columnleft clearfix">
                  <ul>
                        <li><a href="ProductTypes.aspx"><span>商品类型管理</span></a></li>
                  </ul>
</div>
      <div class="columnright">
          <div class="title">
            <em><img src="../images/03.gif" width="32" height="32" /></em>
            <h1>添加新的商品类型</h1>
            <span>商品类型是一系属性的组合，可以用来向顾客展示某些商品具有的特有的属性，一个商品类型下可添加多种属性.一种是供客户查看的扩展属性,如图书类型商品的作者，出版社等，一种是供客户可选的规格,如服装类型商品的颜色、尺码。</span>
</div>
          <div class="formitem">
          <span class="msg">商品类型添加成功！</span>
         </div>
          <div class="Pg_15 Pg_45 fonts"><a href="AddProductType.aspx" class="submit_jia" >添加商品类型</a>
        </div>
          <div class="Pg_15 Pg_45 fonts">您可以随时到  <span class="Name"><a href="ProductTypes.aspx">商品类型</a> </span>中进行修改 </div>
      </div>
        
  </div>
	
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
