<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeFile="ImportFromYfx.aspx.cs" Inherits="Hidistro.UI.Web.Admin.product.ImportFromYfx" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <link rel="stylesheet" href="../css/jquery.treeTable.css" type="text/css" media="screen" />
    <script type="text/javascript" src="../js/jquery.treeTable.js"></script>
    <script type="text/javascript" src="import.yfx.helper.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="optiongroup mainwidth">
		<ul>
			<li class="menucurrent"><a><span>从易分销数据包导入</span></a></li>
			<li><a href="ImportFromHishop.aspx"><span>从Hishop数据包导入</span></a></li>
			<li><a href="ImportFromTB.aspx"><span>从淘宝数据包导入</span></a></li>
			<li><a href="ImportFromPP.aspx"><span>从拍拍数据包导入</span></a></li>
		</ul>
</div>
<div class="dataarea mainwidth databody">

<div class="datafrom">
<div class="formitem">
            <ul>
              <li><h2 class="colorE">数据包信息</h2></li>
              <li>
                <span class="formitemtitle Pw_198">导入插件版本： </span>
                <asp:DropDownList runat="server" ID="dropImportVersions"></asp:DropDownList>
              </li>
              <li>
                <span class="formitemtitle Pw_198">选择要导入的数据包文件： </span>
                <asp:DropDownList runat="server" ID="dropFiles" AutoPostBack="true"></asp:DropDownList>
                <span>
                导入之前需要先将数据包文件上传到服务器上；<br/>
                如果上面的下拉框中没有您要导入的数据包文件，请先上传。
                </span>
              </li>
              <li> <span class="formitemtitle Pw_198"></span>
                <asp:FileUpload runat="server" ID="fileUploader" /><asp:Button runat="server" ID="btnUpload" Text="上传" OnClick="btnUpload_Click" />
                <span>
                    上传数据包须小于40M，否则可能上传失败，<br/>您还可以使用FTP工具先将数据包上传到网站的/storage/data/yfx目录以后，再重新打开此页面操作。
                </span>
                <asp:TextBox runat="server" ID="txtProductTypeXml" style="display:none;"></asp:TextBox>
                <asp:TextBox runat="server" ID="txtPTXml" style="display:none;"></asp:TextBox>
                <asp:CheckBox runat="server" ID="chkFlag" style="display:none;" />
              </li>
              </ul>
              <ul id="infoZone">
              <li>
                <span class="formitemtitle Pw_198">数据包版本：</span>
                <span class="formitemtitle"><asp:Literal runat="server" ID="lblVersion"></asp:Literal></span>
              </li>
              <li>
                <span class="formitemtitle Pw_198">商品数量：</span>
                <span class="formitemtitle"><asp:Literal runat="server" ID="lblQuantity"></asp:Literal></span>
                <asp:CheckBox runat="server" ID="chkIncludeCostPrice" style="display:none;" />
                <asp:CheckBox runat="server" ID="chkIncludeStock" style="display:none;" />
                <asp:CheckBox runat="server" ID="chkIncludeImages" style="display:none;" />
              </li>
              <li id="ptRow">
                <table id="tTypes">
                <thead>
                <tr>
                  <th>数据包中包含的商品类型</th>
                  <th style="width:30%">匹配操作</th>
                </tr>
              </thead>
              <tbody id="tbTypes">
                </tbody>
                </table>
              </li>
              </ul>
              <ul>
              <li><h2 class="colorE">导入选项</h2></li>
              <li>
                <span class="formitemtitle Pw_198">店铺分类：</span>
                <abbr class="formselect">
                    <Hi:ProductCategoriesDropDownList ID="dropCategories" runat="server" NullToDisplay="-请选择店铺分类-" />
                </abbr>
              </li>
              <li id="liCategory"> <span class="formitemtitle Pw_198">产品线：</span>
                   <abbr class="formselect">
                   <Hi:ProductLineDropDownList ID="dropProductLines" runat="server" NullToDisplay="-请选择产品线-" />
                   </abbr>
              </li>
              <li > <span class="formitemtitle Pw_198">商品品牌：</span>
                   <abbr class="formselect">
                    <Hi:BrandCategoriesDropDownList runat="server" ID="dropBrandList" NullToDisplay="--请选择品牌--" CssClass="forminput"></Hi:BrandCategoriesDropDownList>
                   </abbr>
              </li>
              <li> <span class="formitemtitle Pw_198">商品导入状态：</span> 
                <asp:RadioButton runat="server" ID="radOnSales" GroupName="SaleStatus" Checked="true"  Text="出售中"></asp:RadioButton>
                <asp:RadioButton runat="server" ID="radUnSales" GroupName="SaleStatus"  Text="下架区"></asp:RadioButton>
                <asp:RadioButton runat="server" ID="radInStock" GroupName="SaleStatus"  Text="仓库中"></asp:RadioButton>
            </li>
<%--            <li> <span class="formitemtitle Pw_198">如何判断重复商品：</span> 
                <asp:RadioButton runat="server" ID="radProductCode" GroupName="Duplicate" Checked="true"  Text="商家编码相同的商品"></asp:RadioButton>
                <asp:RadioButton runat="server" ID="radProductName" GroupName="Duplicate"  Text="商品名称相同的商品"></asp:RadioButton>
            </li>
            <li> <span class="formitemtitle Pw_198">重复商品处理选项：</span> 
                <asp:RadioButton runat="server" ID="radUpdate" GroupName="ActionOptions" Checked="true"  Text="覆盖"></asp:RadioButton>
                <asp:RadioButton runat="server" ID="radAddNew" GroupName="ActionOptions"  Text="做为新商品添加"></asp:RadioButton>
                <asp:RadioButton runat="server" ID="radSkip" GroupName="ActionOptions"  Text="跳过(什么都不做)"></asp:RadioButton>
            </li>--%>
            <li> <span class="formitemtitle Pw_198"></span> 
                <asp:CheckBox runat="server" ID="chkDeleteFiles" Text="导入完成以后删除数据包及临时文件" Checked="true" />
            </li>
            </ul>
            <ul class="btntf Pa_198">
                <asp:TextBox runat="server" ID="txtMappedTypes" style="display:none;"></asp:TextBox>
                <asp:Button ID="btnImport" runat="server" OnClientClick="return doImport();" CssClass="submit_DAqueding inbnt" Text="导 入" OnClick="btnImport_Click" />
            </ul>
            <div class="blank12 clearfix"></div>
        </div>
    </div>

</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>