<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeFile="MyAdvPositions.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.MyAdvPositions" %>
<%@ Import Namespace="Hidistro.Core"%>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth td_top_ccc">
  <div class="toptitle">
  <em><img src="../images/01.gif" width="32" height="32" /></em>
  <h1 class=" title_height">“<asp:Literal runat="server" ID="litThemName"/>”模板的广告位管理</h1>
</div>
		<!--搜索-->
		<div class="searcharea clearfix br_search">
			<ul>
				<li>
                <asp:HyperLink ID="hlinkAddAdv" CssClass="submit_jia100" Text="添加广告位" runat="server" />
              </li>
		  </ul>
</div>
          <div class="functionHandleArea m_none">
		  <!--分页功能-->
		  
		  <!--结束-->
		  <div class="blank8 clearfix"></div>
		  <div class="batchHandleArea">
		    <ul>
		      <li class="batchHandleButton">
              <span class="signicon"></span> <span class="allSelect"><a href="javascript:void(0);" onclick="CheckClickAll()">全选</a></span> 
              <span class="reverseSelect"><a href="javascript:void(0);" onclick="CheckReverse()">反选</a></span> 
              <span class="delete"><Hi:ImageLinkButton ID="lkbtnDeleteCheck" runat="server" Text="删除" IsShow="true" /></span></li>
	        </ul>
	      </div>
      </div>
		<!--数据列表区域-->
		<div class="datalist">
		 <UI:Grid ID="grdAdvPosition" runat="server" AutoGenerateColumns="false" ShowHeader="true" DataKeyNames="AdvPositionName" GridLines="None" Width="100%" HeaderStyle-CssClass="table_title" >                                                        
                        <Columns>   
                            <UI:CheckBoxColumn ReadOnly="true" HeadWidth="30" HeaderStyle-CssClass="td_right td_left"/>                             
                            <asp:TemplateField HeaderText="广告位名称" HeaderStyle-CssClass="td_right td_left"> 
                                <ItemTemplate>
		                            <asp:Label ID="lblAdvPositionName" runat="server" Text='<%#Bind("AdvPositionName") %>'></asp:Label>
                                </ItemTemplate>                                                         
                            </asp:TemplateField>  
                            <asp:TemplateField HeaderText="广告位标签" HeaderStyle-CssClass="td_right td_left">
                                <ItemTemplate>
                                    <asp:Label ID="lblAdv" runat="server" ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="操作" ItemStyle-Width="20%" HeaderStyle-CssClass="td_left td_right_fff">
                                <ItemStyle CssClass="spanD spanN" />
                                 <ItemTemplate>
	                                <span class="submit_bianji"><asp:HyperLink ID="lkbEdit" runat="server" Text="编辑" ></asp:HyperLink></span>
	                                <span class="submit_shanchu"><Hi:ImageLinkButton ID="lkDelete" IsShow="true" Text="删除" CommandName="Delete" runat="server" /></span>
                                 </ItemTemplate>
                             </asp:TemplateField>
                        </Columns>
                </UI:Grid>
		  
		  <div class="blank12 clearfix"></div>
		  <div class="bottomBatchHandleArea clearfix">
			<div class="batchHandleArea">
				<ul>
					<li class="batchHandleButton">
						<span class="bottomSignicon"></span>
						  <span class="allSelect"><a href="javascript:void(0);" onclick="CheckClickAll()">全选</a></span> 
              <span class="reverseSelect"><a href="javascript:void(0);" onclick="CheckReverse()">反选</a></span> 
              <span class="delete"><Hi:ImageLinkButton ID="ImageLinkButton1" runat="server" Text="删除" IsShow="true" /></span></li>
				</ul>
			</div>
		</div>
</div>
		<!--数据列表底部功能区域-->
  
		


	</div>
  <div class="databottom"></div>
<div class="bottomarea testArea">
  <!--顶部logo区域-->
  </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
