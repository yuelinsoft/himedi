﻿<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.ManageCategories" CodeFile="ManageCategories.aspx.cs" MasterPageFile="~/Admin/Admin.Master" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server"> 
<div class="optiongroup mainwidth">
	<ul>
		<li class="menucurrent"><a><span>店铺分类管理</span></a></li>
		<li class="optionend"><a href="SetCategoryTemplate.aspx"><span>分类模板设置</span></a></li>
	</ul>
</div>
<div class="blank12 clearfix"></div>
<div class="dataarea mainwidth databody">
  <div class="title">
  <em><img src="../images/03.gif" width="32" height="32" /></em>
  <h1>店铺分类管理</h1>
  <span>管理店铺的店铺分类.您可以添加、编辑、删除店铺分类</span></div>  
		<!-- 添加按钮-->
   <div class="btn"><a href="AddCategory.aspx" class="submit_jia">添加新店铺分类</a></div>
<!--结束-->
		<!--数据列表区域-->
  <div class="datalist">
  <UI:Grid ID="grdTopCategries" DataKeyNames="CategoryId" runat="server" ShowHeader="true" AutoGenerateColumns="false" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
                    <Columns> 
                    <asp:TemplateField HeaderText="分类名称" HeaderStyle-Width="40%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                           <span class="icon" categoryid='<%# Eval("CategoryId") %>' parentid='<%# Eval("ParentCategoryId") %>' id="spShowImage" runat="server"><img src="../images/jian.gif" width="24" height="24"  /></span>
                            <span class="Name" id="spCategoryName"><a href='<%# "ProductOnSales.aspx?CategoryId="+Eval("CategoryId")%>' target="_blank"><asp:Literal ID="lblCategoryName" runat="server" /></a></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="URL重写名称" HeaderStyle-Width="30%" DataField="RewriteName" HeaderStyle-CssClass="td_right td_left"/>
                    <UI:SortImageColumn HeaderText="排序" ItemStyle-Width="60px" ReadOnly="true" HeaderStyle-CssClass="td_right td_left"/>
                     <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="td_left td_right_fff">
                         <ItemTemplate>
                            <span class="Name icon Pg_10"><asp:HyperLink ID="HyperLink1" runat="server" Text="转移商品" NavigateUrl='<%# "DisplaceCategory.aspx?CategoryId="+Eval("CategoryId")%>'></asp:HyperLink></span>
                            <span class="submit_bianji"><asp:HyperLink ID="lkEdit" runat="server" Text="编辑" NavigateUrl='<%# "EditCategory.aspx?CategoryId="+Eval("CategoryId")%>'></asp:HyperLink> </span>
                            <span class="submit_shanchu"><Hi:ImageLinkButton runat="server" IsShow="true" ID="Delete" Text="删除" CommandName="Delete"  DeleteMsg="删除分类会级联删除其所有子分类，确定要删除选择的分类吗？" /></span>
                         </ItemTemplate>
                     </asp:TemplateField>                     
                    </Columns>
                </UI:Grid>
  </div>
 <div class="shou">
  <span><img src="../images/jia.gif" width="24" height="24" /></span>
  <span><a id="openAll" style="cursor:pointer;">全部展开</a></span>
  <span><img src="../images/jian.gif" width="24" height="24" /></span>
  <span><a id="closeAll" style="cursor:pointer;">全部收缩</a></span>               
 </div>
</div>
<script type="text/javascript">
$(document).ready(function()
{    
	$(".datalist div table tr").each(function (index, domEle){
		if(index!=0){
		   $(this).mouseover(function(){
			 $(this).addClass("currentcolor");}).mouseout(function(){$(this).removeClass("currentcolor")});
		 }
	 })	
		//全部隐藏
		$("#closeAll").bind("click",function(){
			 $(".datalist div table tr").each(function (index, domEle){
				if(index!=0)
				{
				   var optionTag=$(this).html();
				    if(optionTag.indexOf("parentid=\"0\"")<0)
					   {
						   $(domEle).hide();
						   $(".datalist div table tr td span img").attr("src","../images/jia.gif");
					  }
				 }
			 })										 
		});	
		
		//全部展开
		$("#openAll").bind("click",function(){
			 $(".datalist div table tr").each(function (index, domEle){
				if(index!=0)
				{
					 $(domEle).show();
					 $(".datalist div table tr td span img").attr("src","../images/jian.gif");
				 }
			 })										 
		});	
}); 
      $(".datalist div table tr td span img").each(function(index,imgObj){
	     //为第一级的时候添加点击事件效果
			 $(imgObj).click(function()
			 {
				if($(imgObj).attr("src")=="../images/jian.gif")
				 {
					 var currentTrNode=$(imgObj).parents("tr");
                        currentTrNode = currentTrNode.next();
						var optionHTML;
						while(true)
						{
							optionHTML=currentTrNode.html();
							if(typeof(optionHTML)!="string"){break;}
  							  if(optionHTML.indexOf("parentid=\"0\"")<0)
								 {
									currentTrNode.hide();
									currentTrNode = currentTrNode.next();
								 }
								 else{break;}
						}			
				  //把img src设加可开打状态
				   $(imgObj).attr("src","../images/jia.gif");
				 }
				else
				 {
					 var currentTrNode=$(imgObj).parents("tr");
                        currentTrNode = currentTrNode.next();
						var optionHTML;
						while(true)
						{
							optionHTML=currentTrNode.html();
							if(typeof(optionHTML)!="string"){break;}
  							  if(optionHTML.indexOf("parentid=\"0\"")<0)
								 {
									currentTrNode.show();
									currentTrNode = currentTrNode.next();
								 }
								 else{break;}
						}	
						
				   $(imgObj).attr("src","../images/jian.gif"); 
				 }
	         }); 
	     })

</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server"></asp:Content>

