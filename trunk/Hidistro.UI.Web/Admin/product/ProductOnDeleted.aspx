<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeFile="ProductOnDeleted.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ProductOnDeleted" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
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
			<li class="optionstar"><a href="ProductOnSales.aspx"><span>出售中的商品</span></a></li>
			<li><a href="ProductUnSales.aspx"><span>下架区的商品</span></a></li>
			<li><a href="ProductInStock.aspx"><span>仓库中的商品</span></a></li>
			<li><a href="ProductUnclassified.aspx"><span>未分类商品</span></a></li>
			<li><a href="ProductAlert.aspx"><span>库存报警商品</span></a></li>
			<li class="menucurrent"><a class="optioncurrentend"><span class="optioncenter">回收站中的商品</span></a></li>
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
						<Hi:ProductCategoriesDropDownList ID="dropCategories" runat="server" NullToDisplay="--请选择店铺分类--" />
					</abbr>
				</li>
				<li>
					<abbr class="formselect">
						<Hi:ProductLineDropDownList ID="dropLines" runat="server" NullToDisplay="--请选择产品线--" />
					</abbr>
				</li>
				   <li>
					<abbr class="formselect">
						<Hi:BrandCategoriesDropDownList runat="server" ID="dropBrandList" NullToDisplay="--请选择商品品牌--" CssClass="forminput"></Hi:BrandCategoriesDropDownList>
					</abbr>
				</li>
                <li><span>商家编码：</span><span> <asp:TextBox ID="txtSKU" Width="110" runat="server" CssClass="forminput" /></span></li>
				</ul>
		</div>
		<div class="searcharea clearfix">
		    <ul>
		        <li><span>添加时间：</span></li>
		        <li>
		            <UI:WebCalendar CalendarType="StartDate" ID="calendarStartDate" runat="server" cssclass="forminput" />
		            <span class="Pg_1010">至</span>
		            <UI:WebCalendar ID="calendarEndDate" runat="server" CalendarType="EndDate" cssclass="forminput" />
		        </li>
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
                        <span><a href="javascript:void(0)" onclick="deleteProducts();">彻底删除</a></span>
                        <span><asp:LinkButton runat="server" ID="btnUpShelf" Text="还原到出售中"  /></span>    
                        <span><asp:LinkButton runat="server" ID="btnOffShelf" Text="还原到下架区"  /></span>    
                        <span><asp:LinkButton runat="server" ID="btnInStock" Text="还原到仓库里"  /></span>               
                    </li>
				</ul>
			</div>
		</div>		
		<!--数据列表区域-->
	  <div class="datalist">
	    <UI:Grid runat="server" ID="grdProducts" Width="100%" AllowSorting="true" ShowOrderIcons="true" GridLines="None" DataKeyNames="ProductId"
                    SortOrder="Desc"  AutoGenerateColumns="false" HeaderStyle-CssClass="table_title">
                    <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="30px" HeaderText="选择" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                                <input name="CheckBoxGroup" type="checkbox" value='<%#Eval("ProductId") %>' />
                            </itemtemplate>
                        </asp:TemplateField>                               
                        <asp:BoundField HeaderText="排序" DataField="DisplaySequence" ItemStyle-Width="40px" HeaderStyle-CssClass="td_right td_left" />
                        <asp:TemplateField ItemStyle-Width="35%" HeaderText="商品" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                            <div style="float:left; margin-right:10px;">
                                <a href='<%#"../../ProductDetails.aspx?productId="+Eval("ProductId")%>' target="_blank">
                                        <Hi:ListImage ID="ListImage1"  runat="server" DataField="ThumbnailUrl40"/>      
                                 </a> 
                                 </div>
                                 <div style="float:left;">
                                 <span class="Name"> <a href='<%#"../../ProductDetails.aspx?productId="+Eval("ProductId")%>' target="_blank"><%# Eval("ProductName") %></a></span>
                                  <span class="colorC">商家编码：<%# Eval("ProductCode") %></span>
                                 </div>
                         </itemtemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="库存"  ItemStyle-Width="100" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                             <asp:Label ID="lblStock" runat="server" Text='<%# Eval("Stock") %>' Width="25"></asp:Label>
                          </itemtemplate>
                        </asp:TemplateField>
                        <Hi:MoneyColumnForAdmin HeaderText=" 市场价" ItemStyle-Width="80"  DataField="MarketPrice" HeaderStyle-CssClass="td_right td_left"  />
                        <Hi:MoneyColumnForAdmin HeaderText="成本价" ItemStyle-Width="80"  DataField="CostPrice" HeaderStyle-CssClass="td_right td_left"  />        
                        <Hi:MoneyColumnForAdmin HeaderText="一口价" ItemStyle-Width="80"  DataField="SalePrice" HeaderStyle-CssClass="td_right td_left"  />   
                        <Hi:MoneyColumnForAdmin HeaderText="采购价" ItemStyle-Width="80"  DataField="PurchasePrice" HeaderStyle-CssClass="td_right td_left"  />                        
                        <asp:TemplateField HeaderText="操作" ItemStyle-Width="12%" HeaderStyle-CssClass=" td_left td_right_fff">
                            <ItemTemplate>
			                  <span class="submit_shanchu"><a href="javascript:void(0)" onclick="deleteProduct('<%# Eval("ProductId") %>');">彻底删除</a></span>
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


<div class="Pop_up" id="deleteProduct" style="display: none;">
    <h1>
        彻底删除商品
    </h1>
    <div class="img_datala">
        <img src="../images/icon_dalata.gif" width="38" height="20" /></div>
    <div class="mianform" style="text-align:center;">
    <p><span class="formitemtitle Pw_100" style="float:left;">是否删除图片：</span>
                <asp:CheckBox ID="chkDeleteImage" Text="删除图片" Checked="true" runat="server" /></p>
                <p> <asp:Button ID="btnOK" runat="server" Text="确定" CssClass="submit_DAqueding" /></p>
       <%-- <ul>
            <li><span class="formitemtitle Pw_100">是否删除图片：</span>
                <asp:CheckBox ID="chkDeleteImage" Text="删除图片" Checked="true" runat="server" />
            </li>           
        </ul>
        <ul class="up Pa_100">
            <asp:Button ID="btnOK" runat="server" Text="确定" CssClass="submit_DAqueding" />
        </ul>--%>
    </div>
</div>
<input runat="server" type="hidden" id="currentProductId" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript">
    function deleteProduct(productId){
        $("#ctl00_contentHolder_currentProductId").val(productId);
        DivWindowOpen(250,140,'deleteProduct');
    }
    function deleteProducts(){
        var v_str = "";

        $("input[type='checkbox'][name='CheckBoxGroup']:checked").each(function(rowIndex, rowItem){
            v_str += $(rowItem).attr("value") + ",";
        });
        
        if(v_str.length == 0){
            alert("请选择商品");
            return false;
        }
        $("#ctl00_contentHolder_currentProductId").val(v_str.substring(0, v_str.length - 1));
        DivWindowOpen(250,140,'deleteProduct');        
    }
</script>
</asp:Content>