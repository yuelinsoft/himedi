<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeFile="ManageShippingTemplates.aspx.cs" Inherits="Hidistro.UI.Web.Admin.sales.ManageShippingTemplates" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth td_top_ccc">
           <div class="functionHandleArea clearfix">
			<!--分页功能-->
		   <div class="pageHandleArea">
				<ul>
					<li><a href="AddShippingTemplate.aspx" class="submit_jia">添加新配送模板</a></li>
				</ul>
			</div>
			<!--结束-->
	  </div>
	<div class="datalist">
	<div>
	 <UI:Grid ID="grdShippingTemplates" runat="server" ShowHeader="true" AutoGenerateColumns="false" DataKeyNames="TemplateId" GridLines="None" Width="100%" HeaderStyle-CssClass="border_background">
          <HeaderStyle CssClass="table_title" />
            <Columns>   
                    <asp:TemplateField HeaderText="方式名称"  HeaderStyle-CssClass="td_right">
                        <ItemTemplate>
                           <asp:Label ID="lblShippingModesName" Text='<%#Eval("TemplateName") %>' runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="首重" DataField="Weight" HeaderStyle-CssClass="td_right" />
                    <asp:TemplateField HeaderText="起步价" HeaderStyle-CssClass="td_right">
                        <ItemTemplate>
                            <Hi:FormatedMoneyLabel ID="FormatedMoneyLabelForAdmin1" runat="server" Money='<%# Eval("Price") %>'></Hi:FormatedMoneyLabel>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="加重"  DataField="AddWeight" HeaderStyle-CssClass="td_right"/>
                    <asp:TemplateField HeaderText="加价" HeaderStyle-CssClass="td_right">
                        <ItemTemplate>
                            <Hi:FormatedMoneyLabel ID="FormatedMoneyLabelForAdmin2" runat="server" Money='<%# Eval("AddPrice") %>'></Hi:FormatedMoneyLabel>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="border_top border_bottom">
                        <ItemStyle CssClass="spanD spanN" />
                           <ItemTemplate>
	                           <span class="submit_bianji"><a href='<%# "EditShippingTemplate.aspx?TemplateId="+Eval("TemplateId")%>' class="SmallCommonTextButton">编辑</a></span>
	                           <span class="submit_shanchu"><Hi:ImageLinkButton runat="server" ID="Delete" CommandArgument='<%# Eval("TemplateId") %>' CommandName="DEL_Template" IsShow="true" CssClass="SmallCommonTextButton" Text="删除"/></span>
                           </ItemTemplate>
                    </asp:TemplateField>
                                                         
            </Columns>
        </UI:Grid>
</div>
<div class="blank12 clearfix"></div>
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
</div>
</asp:Content>
