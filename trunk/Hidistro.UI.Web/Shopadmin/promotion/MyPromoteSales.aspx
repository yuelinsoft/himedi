<%@ Page Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeFile="MyPromoteSales.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.MyPromoteSales"  %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth databody">
    <div class="title"> <em><img src="../images/06.gif" width="32" height="32" /></em>
      <h1>店铺促销活动 </h1>
    <span>店铺当前促销活动管理，您可以添加新的促销活动或管理当前的促销活动</span></div>
    <!-- 添加按钮-->
    <div class="btn">
      <table width="500" border="0" cellspacing="0">
        <tr>
          <td width="177"><abbr class="formselect">
            <Hi:PromoteTypeDropDownList ID="dropPromoteTypes" runat="server"></Hi:PromoteTypeDropDownList>
          </abbr></td>
          <td width="200"><abbr class="formselect">
            <asp:LinkButton runat="server" ID="btnAddPromote" Text="添加新的促销活动" CssClass="submit_jia"  ></asp:LinkButton >
          </abbr></td>
          <td>&nbsp;</td>
        </tr>
      </table>
    </div>
    <!--结束-->
    <!--数据列表区域-->
    <div class="datalist">
    
     <div>
	            <UI:Grid ID="grdPromoteSales" runat="server" AutoGenerateColumns="false"  DataKeyNames="ActivityId" Width="100%" GridLines="None" HeaderStyle-CssClass="table_title" >
                        <Columns>
                           
                            <asp:TemplateField HeaderText="活动名称" SortExpression="Name" HeaderStyle-CssClass="td_right td_left">
                            <itemstyle  CssClass="Name" />
                              <ItemTemplate>
                               <asp:Label ID="lblPromteSalesName" Text='<%#Eval("Name") %>' runat="server"></asp:Label>
                               <asp:Literal ID="ltrPromotionInfo" runat="server"></asp:Literal><br /><asp:HyperLink ID="hpkPromotionProduct" runat ="server" Text="查看促销商品" Visible="false"></asp:HyperLink>
                              </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="活动类型" SortExpression="PromoteType" HeaderStyle-CssClass="td_right td_left">
                              <ItemTemplate>
                                <asp:Label ID="lblPromoteType" Text='<%#Eval("PromoteType") %>' style="display:none" runat="server"></asp:Label>
                                <asp:Label ID="lblPromoteTypeName" runat="server"></asp:Label>
                              </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="适合的会员等级" SortExpression="Name" HeaderStyle-CssClass="td_right td_left">
                              <ItemTemplate>
                               <asp:Label ID="lblmemberGrades" Text="" runat="server"></asp:Label>
                              </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="td_left td_right_fff">
                                <itemstyle width="150px"  />
                                <itemtemplate>  
                                    <span class="submit_shanchu"><Hi:ImageLinkButton ID="lkbDelete" runat="server"  IsShow="true" CommandName="Delete" Text="删除"></Hi:ImageLinkButton></span> 
                                      <span class="submit_chakan"><asp:HyperLink ID="hpkPromotion" runat ="server" Text="查看" ></asp:HyperLink></span>
                                </itemtemplate>
                            </asp:TemplateField>
                        </Columns>                    
                    </UI:Grid >
	  </div>
	  </div>
	  


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
