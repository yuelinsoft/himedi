<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeFile="PaymentTypes.aspx.cs" Inherits="Hidistro.UI.Web.Admin.PaymentTypes" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" Runat="Server">

<div class="dataarea mainwidth databody">
  <div class="title">
  <em><img src="../images/05.gif" width="32" height="32" /></em>
  <h1>支付方式列表</h1>
<span>已添加的支付方式列表</span> <span style="margin-left:20px;"><a target="_blank" href="http://act.life.alipay.com/systembiz/hishop/">立即免费申请开通支付宝接口</a></span></div>
<div class="btn"><a href="AddPaymentType.aspx" class="submit_jia">添加新支付方式</a></div>
<div class="datalist">
<asp:GridView ID="grdPaymentMode" runat="server" AutoGenerateColumns="false" ShowHeader="true" Width="100%" DataKeyNames="ModeId" GridLines="None">
                 <HeaderStyle CssClass="table_title" /> 
                   <Columns>
                        <asp:TemplateField HeaderText="支付方式名称" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                               <asp:Label ID="lblModeName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                               <asp:Literal ID="lblDisplaySequence" runat="server" Text='<%#Eval("DisplaySequence") %>' Visible="false"></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <UI:SortImageColumn HeaderText="显示顺序" HeaderStyle-CssClass="td_right td_left" />
                        <asp:TemplateField HeaderText="操作" ItemStyle-Width="20%" HeaderStyle-CssClass="td_right td_right_fff">
                            <ItemTemplate>
		                        <span class="submit_bianji"><a href='<%# "EditPaymentType.aspx?ModeId="+Eval("ModeId") %>' >编辑</a></span>
		                        <span class="submit_shanchu"><Hi:ImageLinkButton runat="server" IsShow="true" ID="Delete" Text="删除" CommandName="Delete" /></span>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

    <div class="page">
      <div class="bottomPageNumber clearfix">
        <div class="pageNumber">
         <UI:Pager runat="server" ShowTotalPages="true" ID="pager1" />
       </div>
      </div>
    </div>
  </div>
</div>
    
</asp:Content>