<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeFile="BrandCategories.aspx.cs" Inherits="Hidistro.UI.Web.Admin.BrandCategories" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" Runat="Server">
<div class="optiongroup mainwidth">
	<ul>
		<li class="menucurrent"><a><span>品牌管理</span></a></li>
		<li class="optionend"><a href="SetBrandCategoryTemplate.aspx"><span>品牌模板设置</span></a></li>
	</ul>
</div>
<div class="blank12 clearfix"></div>
<div class="dataarea mainwidth databody">
  <div class="title">
  <em><img src="../images/03.gif" width="32" height="32" /></em>
  <h1>品牌管理</h1>
  <span>管理商品所属的各个品牌，如果在上架商品时给商品指定了品牌分类，则商品可以按品牌分类浏览</span></div>
  	
		<!-- 添加按钮-->
   <div class="btn"><a href="AddBrandCategory.aspx" class="submit_jia100">添加新品牌</a></div>
   <div class="batchHandleArea" style="margin-left:10px;">
		<ul>
			<li class="batchHandleButton">
			<span class="signicon"></span>
			<span class="allOrder"><asp:LinkButton OnClientClick="return CheckOrderNumber()" ID="btnorder" runat="server">批量保存排序</asp:LinkButton> </span>
			</li>
		</ul>
</div>
   <br />
   
    
<!--结束-->
		<!--数据列表区域-->
	<div class="datalist">
	<UI:Grid ID="grdBrandCategriesList" runat="server" AutoGenerateColumns="false" ShowHeader="true" DataKeyNames="BrandId" GridLines="None" Width="100%" HeaderStyle-CssClass="table_title">
              <Columns>
                  <asp:TemplateField HeaderText="品牌分类编号" ItemStyle-Width="10%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                           <asp:Literal ID="litBrandId" Text='<%#Eval("BrandId") %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="品牌Logo" ItemStyle-Width="14%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <a id="A1" href='<%# Eval("CompanyUrl") %>' runat="server" target="_blank"><Hi:HiImage ID="HiImage1" runat="server" DataField="Logo"  CssClass="Img100_30"/></a>
                        </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="品牌名称" ItemStyle-Width="14%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <asp:Literal ID="litName" runat="server" Text='<%# Bind("BrandName") %>'></asp:Literal>
                        </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="URL重写" ItemStyle-Width="14%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>&nbsp;
                            <asp:Literal ID="litRwriteName" runat="server" Text='<%# Bind("RewriteName") %>'></asp:Literal>
                        </ItemTemplate>
                  </asp:TemplateField>
                  <UI:SortImageColumn HeaderText="排序" ReadOnly="true" ItemStyle-Width="14%" HeaderStyle-CssClass="td_right td_left"/>
                    <asp:TemplateField HeaderText="显示顺序" ItemStyle-Width="70px"  HeaderStyle-CssClass="td_right td_left">
                   <ItemTemplate>
                      <input id="Text1" type="text" runat="server" value='<%# Eval("DisplaySequence") %>' style="width:60px;" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" />
                   </ItemTemplate>
                   </asp:TemplateField>
                  <asp:TemplateField HeaderText="操作" HeaderStyle-Width="20%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                             <span class="submit_bianji"><asp:HyperLink ID="lkEdit" runat="server" Text="编辑" NavigateUrl='<%# "EditBrandCategory.aspx?brandId="+Eval("BrandId")%>' /></span> 
                             <span class="submit_shanchu"><Hi:ImageLinkButton runat="server" ID="lkbtnDelete" CommandName="Delete" IsShow="true" Text="删除" /></span>
                        </ItemTemplate>
                  </asp:TemplateField>
              </Columns>
            </UI:Grid>
	</div>
</div>

<script>
function CheckOrderNumber(){
    var reg=/^[0-9]*[1-9][0-9]*$/;
    tag=true;
    $(".datalist input[type='text']").each(function(index,item){
        if(!reg.test($(this).val().replace(/\s/g,""))){
           alert("排序值不允许为非负数！");
           tag=false; 
        }
    });
    return tag;
}
</script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" Runat="Server">
       
</asp:Content>
