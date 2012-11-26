<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeFile="ArticleSubject.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ArticleSubject" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
 <div class="dataarea mainwidth td_top_ccc">
  <div class="toptitle">
  <em><img src="../images/01.gif" width="32" height="32" /></em>
  <h1 class=" title_height">“<a href="ManageThemes.aspx"><asp:Literal runat="server" ID="litThemName"/></a>”模板的文章栏目</h1>
  <span>模板中，“Common_ArticleList”标签使用列表(列表中的行数代表此标签的使用次数，栏目编号是此标签在模板中设置的参数)</span>
</div>
		<!--数据列表区域-->
		<div class="datalist">
		 <UI:Grid ID="grdSubjects" runat="server" AutoGenerateColumns="false" ShowHeader="true" DataKeyNames="SubjectId" GridLines="None" Width="100%" HeaderStyle-CssClass="table_title" >                                                        
                        <Columns>   
                            <asp:TemplateField HeaderText="栏目编号" HeaderStyle-CssClass="td_right td_left"> 
                                <ItemTemplate>
		                            <asp:Label ID="lblSubjectId" runat="server" Text='<%#Bind("SubjectId") %>'></asp:Label>
                                </ItemTemplate>                                                         
                            </asp:TemplateField>  
                            <asp:TemplateField HeaderText="栏目名称" HeaderStyle-CssClass="td_right td_left"> 
                                <ItemTemplate>
		                            <asp:Label ID="lblSubjectName" runat="server" Text='<%#Bind("SubjectName") %>'></asp:Label>
                                </ItemTemplate>                                                         
                            </asp:TemplateField>                              
                             <asp:TemplateField HeaderText="关联分类" HeaderStyle-CssClass="td_right td_left">
                                <ItemTemplate>
                                    <asp:Label ID="lblCategoryName" runat="server" Text='<%#Bind("CategoryName") %>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>                           
                            <asp:TemplateField HeaderText="关键字" HeaderStyle-CssClass="td_right td_left">
                                <ItemTemplate>
                                    <asp:Label ID="lblKeywords" runat="server" Text='<%#Bind("Keywords") %>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="显示数量" HeaderStyle-CssClass="td_right td_left">
                                <ItemTemplate>
                                    <asp:Label ID="lblMaxNum" runat="server" Text='<%#Bind("MaxNum") %>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="操作" ItemStyle-Width="10%" HeaderStyle-CssClass="td_left td_right_fff">
                                <ItemStyle CssClass="spanD spanN" />
                                 <ItemTemplate>
	                                <span class="submit_bianji"><asp:HyperLink ID="lkbEdit" runat="server" Text="设置" ></asp:HyperLink></span>
                                 </ItemTemplate>
                             </asp:TemplateField>
                        </Columns>
                </UI:Grid>
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
