<%@ Page Language="C#" MasterPageFile="~/Shopadmin/Shopadmin.Master" AutoEventWireup="true" CodeFile="MyProductUnSales.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.MyProductUnSales" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="blank12 clearfix"></div>
<div class="optiongroup mainwidth">
		<ul>
			<li class="optionstar"><a href="MyProductOnSales.aspx" class="optionnext"><span>出售中的商品</span></a></li>
            <li class="menucurrent"><a ><span class="optioncenter">下架区的商品</span></a></li>
            <li><a href="MyProductInStock.aspx"><span>仓库中的商品</span></a></li>
			<li><a href="MyProductUnclassified.aspx"><span>未分类商品</span></a></li>
			<li class="optionend"><a href="MyProductAlert.aspx"><span>库存报警商品</span></a></li>
		</ul>
	</div>
	<!--选项卡-->

	<div class="dataarea mainwidth">
		<!--搜索-->
		<div class="searcharea clearfix">
			<ul>
				<li><span>商品名称：</span><span><asp:TextBox ID="txtSearchText" runat="server" CssClass="forminput"  /></span></li>
				<li>
					<abbr class="formselect">
						<Hi:DistributorProductCategoriesDropDownList ID="dropCategories" runat="server" NullToDisplay="--请选择店铺分类--" />
					</abbr>
				</li>
                <li><span>商家编码：</span><span> <asp:TextBox ID="txtSKU" Width="110" runat="server" CssClass="forminput" /></span></li>
				<li><asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="searchbutton"/></li>
			</ul>
		</div>
		<!--结束-->
         <div class="functionHandleArea clearfix">
			<!--分页功能-->
			<div class="pageHandleArea">
				<ul>
					<li class="paginalNum"><span>每页显示数量：</span><UI:PageSize runat="server" ID="hrefPageSize" /></li>
				</ul>
			</div>
			<div class="pageNumber">
				<div class="pagination">
                <UI:Pager runat="server" ShowTotalPages="false" ID="pager1" />
            </div>
			</div>
			<!--结束-->

			<div class="blank8 clearfix"></div>
			<div class="batchHandleArea">
				<ul>
					<li class="batchHandleButton">
					<span class="signicon"></span>
					<span class="allSelect"><a href="javascript:void(0)" onclick="SelectAll()">全选</a></span>
					<span class="reverseSelect"><a href="javascript:void(0)" onclick="ReverseSelect()">反选</a></span>
                    <span class="delete"><Hi:ImageLinkButton ID="btnDelete" runat="server" Text="删除" IsShow="true"  /></span>
                    <span class="submit_btnshangjia"><asp:LinkButton runat="server" ID="btnUpShelf" Text="上架" /></span>
                    <span class="submit_btnxiajia"><asp:LinkButton runat="server" ID="btnInStock" Text="入库"  /></span>
                     <span class=""><a href="javascript:void(0)" onclick="EditMemberPrices()">调整会员零售价</a></span>  
                     <span class=""><a href="javascript:void(0)" onclick="ShowEditDiv()">修改商品名称</a></span>  
                    </li>
				</ul>
			</div>
		</div>
		
		<!--数据列表区域-->
	  <div class="datalist">
	    <UI:Grid runat="server" ID="grdProducts" Width="100%" AllowSorting="false" ShowOrderIcons="true" GridLines="None" DataKeyNames="ProductId"
                    SortOrder="Desc" SortOrderBy="DisplaySequence" AutoGenerateColumns="false" HeaderStyle-CssClass="table_title">
                     <Columns>
                        <asp:TemplateField ItemStyle-Width="30px" HeaderText="选择" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                                <input name="CheckBoxGroup" type="checkbox" value='<%#Eval("ProductId") %>' />
                            </itemtemplate>
                        </asp:TemplateField>    
                        <asp:BoundField HeaderText="排序" DataField="DisplaySequence" ItemStyle-Width="45px" HeaderStyle-CssClass="td_right td_left" />
                        <asp:TemplateField ItemStyle-Width="300px" HeaderText="商品" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                            <div style="float:left; margin-right:10px;">
                        <Hi:DistributorProductDetailsLink ID="ProductDetailsLink2" runat="server" unSale="true"  ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>' ImageLink="true">
                                <Hi:ListImage ID="HiImage1"  runat="server" DataField="ThumbnailUrl40"/>      
                                </Hi:DistributorProductDetailsLink>
                                 </div>
                                 <div style="float:left;">
                                 <span class="Name"><Hi:DistributorProductDetailsLink ID="DistributorProductDetailsLink1" runat="server" unSale="true"  ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>'></Hi:DistributorProductDetailsLink></span>
                                  <span class="colorC">商家编码：<%# Eval("ProductCode")%></span>
                                 </div>
                         </itemtemplate>
                        </asp:TemplateField>
                                 <asp:TemplateField HeaderText="最低零售价(元)" HeaderStyle-Width="135px" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <%#Eval("LowestSalePrice", "{0:F2}")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                         <Hi:MoneyColumnForAdmin HeaderText=" 一口价"
                            ItemStyle-Width="80" DataField="SalePrice"  HeaderStyle-CssClass="td_right td_left"  />
                        <Hi:MoneyColumnForAdmin HeaderText="采购价"
                            ItemStyle-Width="80" DataField="PurchasePrice" HeaderStyle-CssClass="td_right td_left"  />                    
                         <asp:TemplateField HeaderText="差价" ItemStyle-Width="80" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                           
                             <Hi:FormatedMoneyLabel ID="FormatedMoneyLabel1" Money='<%# Convert.ToDecimal(Eval("SalePrice")) -  Convert.ToDecimal(Eval("PurchasePrice"))%>' runat="server"></Hi:FormatedMoneyLabel>
                          </itemtemplate>
                        </asp:TemplateField>  
                        <asp:TemplateField HeaderText="库存数量" SortExpression="Stock" ItemStyle-Width="80" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                             <asp:Label ID="lblStock" runat="server" Text='<%# Eval("Stock") %>' Width="25"></asp:Label>
                          </itemtemplate>
                        </asp:TemplateField>                                                        
                        <asp:TemplateField HeaderText="操作" ItemStyle-Width="15%" HeaderStyle-CssClass=" td_left td_right_fff">
                            <ItemTemplate>
                                <span class="submit_bianji"><a target="_blank" href='<%#"EditMyProduct.aspx?productId="+Eval("ProductId")%>'>编辑</a></span>
			                  <span class="submit_shanchu"><Hi:ImageLinkButton ID="btnDelete" CommandName="Delete" runat="server" Text="删除" IsShow="true"  /></span>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </UI:Grid>
		  <div class="blank12 clearfix"></div>
      </div>
        <div class="page">
		<div class="bottomPageNumber clearfix">
			<div class="pageNumber">
			<div class="pagination">
				<UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
				</div>
			</div>
		</div>
      </div>

	</div>

<div class="Pop_up" id="EditProductNames"  style=" display:none;">
      <h1>批量修改商品名称 </h1>
      <div class="img_datala"><img src="../images/icon_dalata.gif" width="38" height="20" /></div>
      <div class="mianform validator2">
        <ul>
            <li> 
                增加前缀 <asp:TextBox ID="txtPrefix" runat="server" Width="80px"  MaxLength="20"/> 
                增加后缀 <asp:TextBox ID="txtSuffix" runat="server" Width="80px"  MaxLength="20"/>
                <asp:Button ID="btnAddOK" runat="server" Text="确定"  CssClass="searchbutton"/> 
            </li>
            <li> 
                查找字符串 <asp:TextBox ID="txtOleWord" runat="server" Width="80px" /> 替换成 <asp:TextBox ID="txtNewWord" runat="server" Width="80px" />
                <asp:Button ID="btnReplaceOK" runat="server" Text="确定"  CssClass="searchbutton"/> 
            </li>
        </ul>
      </div>
</div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" >       
    function EditMemberPrices(){
        var productIds = GetProductId();
        if(productIds.length > 0)
            window.open("EditMyMemberPrices.aspx?ProductIds=" + productIds);
    }    
    
    function GetProductId(){
        var v_str = "";

        $("input[type='checkbox'][name='CheckBoxGroup']:checked").each(function(rowIndex, rowItem){
            v_str += $(rowItem).attr("value") + ",";
        });
        
        if(v_str.length == 0){
            alert("请选择商品");
            return "";
        }
        return v_str.substring(0, v_str.length - 1);        
    }
    
    function ShowEditDiv(id, categoryId, keywords) {
        var productIds = GetProductId();
        if(productIds.length > 0)
            DivWindowOpen(450, 180, 'EditProductNames');

    }
</script>
</asp:Content>