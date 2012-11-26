<%@ Page Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeFile="ToTaobaoProducts.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.product.ToTaobaoProducts" Title="无标题页" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div runat="server" id="content1" class="dataarea mainwidth td_top_ccc">
		<!--搜索-->
		<div class="searcharea clearfix">
			<ul>
				<li><span>商品名称：</span><span><asp:TextBox ID="txtSearchText" runat="server" CssClass="forminput"  /></span></li>
				<li>
					<abbr class="formselect">
						<Hi:DistributorProductCategoriesDropDownList ID="dropCategories" runat="server" NullToDisplay="--请选择店铺分类--" />
					</abbr>
				</li>
				<li>
					<abbr class="formselect">
						<Hi:ProductLineDropDownList ID="dropLines" runat="server" NullToDisplay="--请选择产品线--" />
					</abbr>
				</li>
                <li><span>商家编码：</span><span> <asp:TextBox ID="txtSKU" Width="110" runat="server" CssClass="forminput" /></span></li>
			</ul>
		</div>
		<div class="searcharea clearfix">
		    <ul>
		        <li><span>添加时间：</span></li>
		        <li>
		            <UI:WebCalendar CalendarType="StartDate" ID="calendarStartDate" runat="server" cssclass="forminput" />至
		            <UI:WebCalendar ID="calendarEndDate" runat="server" CalendarType="EndDate" cssclass="forminput" />
		        </li>
		        <li><asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="searchbutton" OnClick="btnSearch_Click" /></li>
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
                        <span class="purchaseSelect"><a href="javascript:void(0)" onclick="Publish()">将选中商品发布到淘宝</a></span>                      
                    </li>
				</ul>
			</div>
			<div class="filterClass"> <span><b>发布状态：</b></span> <span class="formselect">
		        <Hi:PublishDropDownList ID="dropPublishStatus" runat="server" OnSelectedIndexChanged="dropPublishStatus_SelectedIndexChanged" />
		        </span> </div>
		<div class="blank8 clearfix"></div>
		    <div class="batchHandleMsg">
		        供应商已经更新商品淘宝数据，您有<asp:Label ID="lbsum" runat="server" Text="0"></asp:Label>
                        件商品需要更新
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
                        <asp:TemplateField ItemStyle-Width="42%" HeaderText="商品" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                            <div style="float:left; margin-right:10px;">
            
                                        <Hi:ListImage ID="ListImage1"  runat="server" DataField="ThumbnailUrl40"/>      

                                 </div>
                                 <div style="float:left;">
                                 <span class="Name"> <%# Eval("ProductName") %></span>
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
                        <Hi:MoneyColumnForAdmin HeaderText="一口价" ItemStyle-Width="80"  DataField="SalePrice" HeaderStyle-CssClass="td_right td_left"  />   
                        <Hi:MoneyColumnForAdmin HeaderText="采购价" ItemStyle-Width="80"  DataField="PurchasePrice" HeaderStyle-CssClass="td_right td_left"  />                        
                        <asp:TemplateField HeaderText="发布状态" ItemStyle-Width="10%" HeaderStyle-CssClass=" td_left td_right_fff">
                            <ItemTemplate>
                                <%#SetPublishStatus(Eval("IsPublish"))%>
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
	<div runat="server" id="content12" visible="false" class="dataarea mainwidth td_top_ccc">
    <asp:Literal ID="txtMsg" runat="server" />
</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">

    <script type="text/javascript" >       
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
    
    function Publish() {
        var productIds = GetProductId();
        if(productIds.length>0){
             window.open("PublishProductToTaobao.aspx?productIds=" + productIds);
        }
    }
</script>
</asp:Content>