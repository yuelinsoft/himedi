<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeFile="ProductOnSales.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ProductOnSales" %>
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
			<li class="menucurrent"><a><span>出售中的商品</span></a></li>
			<li><a href="ProductUnSales.aspx"><span>下架区的商品</span></a></li>
			<li><a href="ProductInStock.aspx"><span>仓库中的商品</span></a></li>
			<li><a href="ProductUnclassified.aspx"><span>未分类商品</span></a></li>
			<li><a href="ProductAlert.aspx"><span>库存报警商品</span></a></li>
			<li class="optionend"><a href="ProductOnDeleted.aspx"><span>回收站中的商品</span></a></li>
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
                        <span class="delete"><Hi:ImageLinkButton ID="btnDelete" runat="server" Text="删除" IsShow="true" DeleteMsg="确定要把商品移入回收站吗？" /></span>
                        <span class="submit_btnxiajia"><a href="javascript:void(0)" onclick="javascript:PenetrationStatus();">下架</a></span>
                        <span class="submit_btnxiajia"><a href="javascript:void(0)" onclick="javascript:StockStatus();">入库</a></span>
                        <span class=""><a href="javascript:void(0)" onclick="EditBaseInfo()">调整基本信息</a></span>  
                        <span class=""><a href="javascript:void(0)" onclick="EditStocks()">调整库存</a></span>                              
                        <span class=""><a href="javascript:void(0)" onclick="EditMemberPrices()">调整会员零售价</a></span>    
                        <span class=""><a href="javascript:void(0)" onclick="EditDistributorPrices()">调整分销商采购价</a></span>                 
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
                        <Hi:MoneyColumnForAdmin HeaderText=" 市场价" ItemStyle-Width="80"  DataField="MarketPrice" HeaderStyle-CssClass="td_right td_left" />
                        <Hi:MoneyColumnForAdmin HeaderText="成本价" ItemStyle-Width="80"  DataField="CostPrice" HeaderStyle-CssClass="td_right td_left"  />        
                        <Hi:MoneyColumnForAdmin HeaderText="一口价" ItemStyle-Width="80"  DataField="SalePrice" HeaderStyle-CssClass="td_right td_left"  />   
                        <Hi:MoneyColumnForAdmin HeaderText="采购价" ItemStyle-Width="80"  DataField="PurchasePrice" HeaderStyle-CssClass="td_right td_left"  />                        
                        <asp:TemplateField HeaderText="操作" ItemStyle-Width="19%" HeaderStyle-CssClass=" td_left td_right_fff">
                            <ItemTemplate>
                                <span class="submit_bianji"><a href="<%#"EditProduct.aspx?productId="+Eval("ProductId")%>" target="_blank">编辑</a></span>
                                 <span class="submit_bianji"><a href="<%#"EditReleteProducts.aspx?productId="+Eval("ProductId")%>" target="_blank">相关商品</a></span>
			                  <span class="submit_shanchu"><Hi:ImageLinkButton ID="btnDelete" CommandName="Delete" runat="server" Text="删除" IsShow="true" DeleteMsg="确定要把商品移入回收站吗？" /></span>
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
	
	<div class="Pop_up" id="UnSaleProduct" style="display: none;">
    <h1>
        下架商品
    </h1>
    <div class="img_datala">
        <img src="../images/icon_dalata.gif" width="38" height="20" /></div>
    <div class="mianform" style="text-align:center;">
    <p><span class="formitemtitle Pw_100" style="float:left;">同时撤销铺货：</span>
                <asp:CheckBox ID="chkDeleteImage" Text="撤销铺货" Checked="true" runat="server" /></p>
                <p style="color:Red; width:100%">（当选择撤销铺货时，所有子站关于此商品信息以及促销活动信息都将被删除。）</p>
                <p> <asp:Button ID="btnOK" runat="server" Text="确定" CssClass="submit_DAqueding" OnClientClick="javascript:if($('#ctl00_contentHolder_chkDeleteImage').attr('checked')){$('#ctl00_contentHolder_hdPenetrationStatus').val('1')}" /></p>
    </div>
</div>
	<div class="Pop_up" id="InStockProduct" style="display: none;">
    <h1>
        入库商品
    </h1>
    <div class="img_datala">
        <img src="../images/icon_dalata.gif" width="38" height="20" /></div>
    <div class="mianform" style="text-align:center;">
    <p><span class="formitemtitle Pw_100" style="float:left;">同时撤销铺货：</span>
                <asp:CheckBox ID="chkInstock" Text="撤销铺货" Checked="true" runat="server" /></p>
                <p style="color:Red; width:100%">（当选择撤销铺货时，所有子站关于此商品信息以及促销活动信息都将被删除。）</p>
                <p> <asp:Button ID="btnStockPentrationStauts" runat="server" Text="确定" CssClass="submit_DAqueding" OnClientClick="javascript:if($('#ctl00_contentHolder_chkInstock').attr('checked')){$('#ctl00_contentHolder_hdPenetrationStatus').val('1')}else{$('#ctl00_contentHolder_hdPenetrationStatus').val('0')}" /></p>
    </div>
</div>

<input type="hidden" id="hdPenetrationStatus" value="0" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" >
    function EditStocks(){    
        var productIds = GetProductId();
        if(productIds.length > 0)
            window.open("EditStocks.aspx?ProductIds=" + productIds);
    }
    
    function EditBaseInfo(){
        var productIds = GetProductId();
        if(productIds.length > 0)
            window.open("EditBaseInfo.aspx?ProductIds=" + productIds);
    }     
    
    function EditMemberPrices(){
        var productIds = GetProductId();
        if(productIds.length > 0)
            window.open("EditMemberPrices.aspx?ProductIds=" + productIds);
    }    
    
    function EditDistributorPrices(){
        var productIds = GetProductId();
        if(productIds.length > 0)
            window.open("EditDistributorPrices.aspx?ProductIds=" + productIds);
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
    
    
    //下架
    function PenetrationStatus(){
        DivWindowOpen(250,200,'UnSaleProduct');
    }
    
    //入库提现
    function StockStatus(){
        DivWindowOpen(250,200,'InStockProduct');
    }
</script>
</asp:Content>