<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.ProductCatalog" CodeFile="ProductCatalog.aspx.cs" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
 
  <div class="dataarea mainwidth td_top_ccc">
  <div class="toptitle">
  <em><img src="../images/03.gif" width="32" height="32" /></em>
  <h1 class="title_height">查看产品线的商品目录</h1>
</div>
		<!--搜索-->
		<div class="searcharea clearfix br_search">
			<ul>
				<li style="margin-left:43px;"><span>商品名称：</span><span>
				  <asp:TextBox ID="txtProductName" runat="server" CssClass="forminput" Width="110px" />
			  </span></li>
				<li style="margin-left:30px;"><span>店铺分类：</span>
					<abbr class="formselect">
						<Hi:ProductCategoriesDropDownList ID="dropCategories" runat="server" CssClass="forminput" />
					</abbr>
				</li>
				<li style="margin-left:30px;"><span>商家编码：</span><span>
				  <asp:TextBox ID="txtSku" runat="server" CssClass="forminput"/>
			  </span></li>
			  </ul>
			  <ul>
			  <li style="margin-left:30px;"><span>所属分销商：</span><span>
				  <Hi:DistributorDropDownList runat="server" ID="ddlDistributor" CssClass="forminput" />
			  </span></li>
			  <li style="margin-left:30px;"><span>产品线：</span><span>
				  <Hi:ProductLineDropDownList ID="dropProductLine" NullToDisplay="--请选择--"  runat="server" CssClass="forminput" />
			  </span></li>
			  <li style="margin-left:30px;"><span>品牌：</span><span>
				  <Hi:BrandCategoriesDropDownList runat="server" ID="dropBrandList" NullToDisplay="--请选择--" CssClass="forminput"></Hi:BrandCategoriesDropDownList>
			  </span></li>
			
				<li style="margin-left:30px;"><asp:Button ID="btnSearchButton" runat="server" class="searchbutton" Text="查询" /></li>
			</ul>
	</div>
		<div class="advanceSearchArea clearfix">
			<!--预留显示高级搜索项区域-->
			
	    </div>
		<!--结束-->
	

		
		
		
          <div class="functionHandleArea m_none">
		  <!--分页功能-->
		  <div style="width:100%">
		      
		        <ul>
		          <li class="paginalNum"><span>每页显示数量：</span><UI:PageSize runat="server" ID="hrefPageSize" /></li>
	            </ul>
	            <div class="pageNumber" style="float:right;">
		        <div class="pagination">
                  <UI:Pager runat="server" ShowTotalPages="false" ID="pager" />
                </div>
             </div>
	      
	          <div class="clearfix"></div>
		  
        </div>
		  <!--结束-->
		  <div class="blank8 clearfix"></div>
	

		      <div class="batchHandleArea">
		        <ul>
		          <li class="batchHandleButton">
		          <span class="signicon"></span>
                  <span class="allSelect"><a href="javascript:void(0);" onclick="CheckClickAll()">全选</a></span>
						    <span class="reverseSelect"><a href="javascript:void(0);" onclick="CheckReverse()">反选</a></span>
						    <span class="delete"><Hi:ImageLinkButton ID="lkbtnDeleteCheck1" IsShow="true"  runat="server" Text="删除" DeleteMsg="确定要把商品移入回收站吗？" /></span>
						    <span class="releaseGoods"><Hi:ImageLinkButton ID="btnPenetration1"　IsShow="true" DeleteMsg="选中商品铺货后，分销商将可以下载这些商品进行销售，确定铺货吗？" runat="server" Text="铺货" /></span>
						    <span class="quashGoods"><Hi:ImageLinkButton ID="btnCancle1" IsShow="true" DeleteMsg="选中商品撤销铺货后，将删除分销商已下载的这些商品，确定撤销吗？" runat="server" Text="撤销铺货" /></span> 
						    </li>
	            </ul>
	          </div>
		      <div class="filterClass"> <span><b>铺货状态：</b></span> <span class="formselect">
		        <Hi:PenetrationStatusDropDownList ID="droppenetrationStatus" runat="server" />
		        </span> </div>
	  </div>
		<!--数据列表区域-->
		<div class="datalist">
		 <UI:Grid runat="server" ID="grdProducts" Width="100%" AllowSorting="true" ShowOrderIcons="true" GridLines="None" DataKeyNames="ProductId"
                    SortOrder="Desc" SortOrderBy="DisplaySequence" AutoGenerateColumns="false" HeaderStyle-CssClass="table_title">
                    <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                    <Columns>
                        <UI:CheckBoxField HeadWidth="35" HeaderStyle-CssClass="td_right td_left" />
                        <asp:BoundField HeaderText="排序" DataField="DisplaySequence" ItemStyle-Width="45px" HeaderStyle-CssClass="td_right td_left" />
                        
                        <asp:TemplateField ItemStyle-Width="280px" HeaderText="商品" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>                              
                                 <table border="0" cellpadding="0" cellspacing="0" width="100%" style="border:none;">
                                 <tr>
                                 <td style="border:none;" rowspan="2"><a href='<%#"../../ProductDetails.aspx?productId="+Eval("ProductId")%>' target="_blank">
                                        <Hi:ListImage ID="HiImage1"  runat="server" DataField="ThumbnailUrl40"/>      
                                 </a> </td>
                                 <td style="border:none;"><span class="Name"><a href='<%#"../../ProductDetails.aspx?productId="+Eval("ProductId")%>' target="_blank"><%# Eval("ProductName") %></a></span>
                                 </td>
                                 </tr>
                                 <tr>                                 
                                 <td class="colorC" style="border:none;"> 商家编码：<%# Eval("ProductCode") %></td></tr>
                                 </table>
                         </itemtemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="分销商最低零售价"  ItemStyle-Width="150" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                             <asp:Literal ID="lblSmall" Text='<%#decimal.Round(decimal.Parse((Eval("LowestSalePrice")).ToString()), 2) %>' runat="server" />
                          </itemtemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="一口价(元)"  ItemStyle-Width="100" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                             <Hi:FormatedMoneyLabel ID="lblsalesPrice" Money='<%#Eval("SalePrice") %>' runat="server" />
                            </itemtemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="库存"  ItemStyle-Width="100" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                             <asp:Literal ID="litStock" runat ="server" Text='<%#Eval("Stock") %>'></asp:Literal>
                            </itemtemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="铺货状态"  ItemStyle-Width="100" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                              <span class='<%#Eval("PenetrationStatus").ToString() =="1"?"colorB":"colorA"%>'><asp:Literal ID="litPenetrationStatus" runat ="server" Text='<%#Eval("PenetrationStatus").ToString() =="1"?"已铺货":"未铺货"%>'></asp:Literal></span>
                              <span class="submit_quanxuan"><Hi:ImageLinkButton ID="btnPenetration" runat="server"  DeleteMsg='<%#Eval("PenetrationStatus").ToString() =="1"?"商品取消铺货后，将删除分销商下载的此件商品，确定取消吗？":"商品铺货后，分销商将可以下载此件商品进行销售，确定铺货吗？"%>' IsShow="true"  Text='<%#Eval("PenetrationStatus").ToString() =="1"?"取消":"铺货"%>' CommandName="Penetration" CommandArgument='<%#Eval("ProductId") %>'  /> </span>
                            </itemtemplate>
                        </asp:TemplateField>
                        
                         <asp:TemplateField HeaderText="操作"  ItemStyle-Width="200" HeaderStyle-CssClass="td_left td_right_fff">
                            <itemtemplate>                
                        <span class="submit_bianji"><a href='<%#"EditProduct.aspx?productId="+Eval("ProductId")%>' target="_blank">编辑</a></span>
                        <span class="submit_shanchu"><Hi:ImageLinkButton  runat="server" IsShow="true" ID="Delete" Text="删除" CommandName="Delete" DeleteMsg="确定要把商品移入回收站吗？" ></Hi:ImageLinkButton></span>
                            </itemtemplate>
                        </asp:TemplateField>
                    </Columns>
                </UI:Grid>
		 
      </div>
		<!--数据列表底部功能区域-->
		
		       		<div class="blank12 clearfix"></div>
	  <div  class=" br_search" style=" border-bottom:1px #ddd solid;">
       	<div class="searcharea clearfix">
        	<ul>
				<li><span><b>移动商品到产品线：</b></span>
					<span class="formselect">
						 <Hi:ProductLineDropDownList ID="dropProductLine2" NullToDisplay="--请选择--"  runat="server" CssClass="forminput" />
					</span>
				</li>
                <li>
                    <asp:Button ID="btnOK" runat="server" Text="确定" CssClass="submit_DAqueding" OnClientClick="return ChangeProductLine();" />
                </li>
			</ul>
          </div>
          <div class="colorD ">批量转移商品的产品线，在转移以前请先选择要转移的商品。</div>
		</div>
		 <div class="blank12 clearfix"></div>
  <div class="bottomBatchHandleArea clearfix">
			<div class="batchHandleArea">
				<ul>
					<li class="batchHandleButton">
						<span class="bottomSignicon"></span>
						<span class="allSelect"><a href="javascript:void(0);" onclick="CheckClickAll()">全选</a></span>
						<span class="reverseSelect"><a href="javascript:void(0);" onclick="CheckReverse()">反选</a></span>
						<span class="delete"><Hi:ImageLinkButton ID="lkbtnDeleteCheck" IsShow="true"  runat="server" Text="删除" /></span>
						<span class="releaseGoods"><Hi:ImageLinkButton ID="btnPenetration" runat="server"　IsShow="true" DeleteMsg="选中商品铺货后，分销商将可以下载这些商品进行销售，确定铺货吗？" Text="铺货" /></span>
						<span class="quashGoods"><Hi:ImageLinkButton ID="btnCancle" IsShow="true" DeleteMsg="选中商品撤销铺货后，将删除分销商已下载的这些商品，确定撤销吗？" runat="server" Text="撤销铺货" /></span>
					</li>
				</ul>
			</div>
		</div>
		<div class="bottomPageNumber clearfix">
			<div class="pageNumber">
				 <div class="pagination">
                     <UI:Pager runat="server" ShowTotalPages="true" ID="pager1" />
                 </div>
			</div>
		</div>



	</div>
  <div class="databottom"></div>
<div class="bottomarea testArea">
  <!--顶部logo区域-->
  </div>
  
 
  <script>
  function ChangeProductLine(){
        tag=false;
        for(var a=0;a<$(".datalist input[type='checkbox']").size();a++){
             if($(".datalist input[type='checkbox']").eq(a).attr("checked")) {
                tag=true;
                break;
             }      
        }
        
        if(tag==false)
        {
            alert("请先选择要转移的产品！");
        }
        return tag;
  }
  </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
