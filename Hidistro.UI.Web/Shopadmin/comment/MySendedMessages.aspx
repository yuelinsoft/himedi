﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeFile="MySendedMessages.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.MySendedMessages" %>
<%@ Import Namespace="Hidistro.Core"%>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

<div class="optiongroup mainwidth">
	<ul>
        <li class="optionstar"><a href="MyReceivedMessages.aspx" class="optionnext"><span>收件箱</span></a></li>
        <li class="menucurrent"><a href="javascript:void(0);"><span class="optioncenter">发件箱</span></a></li>
        <li><a href="SendMyMessage.aspx"><span>发送站内信</span></a></li>
      </ul>
</div>
	<div class="dataarea mainwidth">
    <div class="toptitle"> <em><img src="../images/07.gif" width="32" height="32" /></em> <span class="title_height">你可以查看删除你发送给会员的站内消息.</span> </div>
    <!--搜索-->
    <div class="functionHandleArea m_none">
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
          <li class="batchHandleButton"> <span class="signicon"></span>
           <span class="allSelect"><a onclick="CheckClickAll()" href="javascript:void(0)">全选</a></span>
            <span class="reverseSelect"><a onclick="CheckReverse()" href="javascript:void(0)">反选</a></span> 
            <span class="delete"><Hi:ImageLinkButton ID="btnDeleteSelect" CssClass="submit66" IsShow="true" Height="25px" runat="server" Text="删除" /></span></li>
        </ul>
      </div>
     
    </div>
    <!--数据列表区域-->
    <div class="datalist">
        <UI:Grid ID="messagesList" runat="server" ShowHeader="true" AutoGenerateColumns="false" DataKeyNames="SendMessageId" GridLines="None" Width="100%" HeaderStyle-CssClass="table_title">
           <Columns>                 
                <UI:CheckBoxColumn HeaderStyle-CssClass="table_title"/>                
                <asp:TemplateField HeaderText="标题" HeaderStyle-CssClass="td_right td_left">
                 <itemstyle  CssClass="Name" />
                    <ItemTemplate>
                        <asp:Literal ID="litTitle" runat="server" Text='<%#Eval("Title")%>'></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="收件人" HeaderStyle-CssClass="td_right td_left">
                <itemstyle  CssClass="Name" />
                    <ItemTemplate>
                       <a href='<%# Globals.ApplicationPath+string.Format("/shopadmin/Underling/ManageUnderlings.aspx?Username={0}",Eval("Addressee")) %>'><asp:Literal ID="litAddressee" runat="server" Text='<%# Eval("Addressee")%>' /></a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="内容" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="40%">
                    <ItemTemplate>
                       <asp:Label ID="litPublishContent" runat="server" Text='<%#Eval("PublishContent")%>' CssClass="line" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="时间" HeaderStyle-CssClass="td_right td_left">
                    <ItemTemplate>
                        <Hi:FormatedTimeLabel ID="litDateTime" ShopTime="true" runat="server" Time='<%#Eval("PublishDate") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                    <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="80px">
                        <ItemStyle CssClass="spanD spanN" />
                        <ItemTemplate>
                            <Hi:ImageLinkButton ID="lkbtnDelete" runat="server" IsShow="true"  Text="删除" CommandName="Delete"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                            
            </Columns>
        </UI:Grid>       
    <div class="blank12 clearfix"></div>
    </div>
     <div class="bottomBatchHandleArea clearfix">
			<div class="batchHandleArea">
				<ul>
					<li class="batchHandleButton">
					<span class="bottomSignicon"></span>
					<span class="allSelect"><a onclick="CheckClickAll()" href="javascript:void(0)">全选</a></span>
					<span class="reverseSelect"><a onclick="CheckReverse()" href="javascript:void(0)">反选</a></span>
                    <span class="delete"><Hi:ImageLinkButton ID="btnDeleteSelect1"  IsShow="true" Height="25px" runat="server" Text="删除" /></span></li>
				</ul>
			</div>
	  </div>
    <!--数据列表底部功能区域-->
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

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
