<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeFile="UserArealDistributionStatistics.aspx.cs" Inherits="Hidistro.UI.Web.Admin.UserArealDistributionStatistics" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="optiongroup mainwidth">
		<ul>            
            <li class="optionstar"><a href="MemberRanking.aspx" class="optionnext"><span>会员消费排行</span></a></li>
			<li class="menucurrent"><a href="#"><span class="optioncenter">会员分析统计</span></a></li>
			<li><a href="UserIncreaseStatistics.aspx"><span>会员增长统计</span></a></li>
            <li><a href="BalanceStatistics.aspx" ><span>预付款统计</span></a></li>
            <li class="optionend"><a href="DrawRequestStatistics.aspx"><span>提现统计</span></a></li>
		</ul>
</div>
<div class="dataarea mainwidth">
		<!--搜索-->
	    <!--数据列表区域-->
	  <div class="datalist">
	          <UI:Grid ID="grdUserStatistics" runat="server" ShowHeader="true" DataKeyNames="RegionId" AutoGenerateColumns="false" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
                    <Columns>                                         
                            <asp:TemplateField HeaderText="所在地" HeaderStyle-CssClass="td_right td_left">
                                <ItemTemplate>
                                   <asp:Label runat="server" ID="lblReionName"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField> 
                            <asp:BoundField HeaderText="客户数量" DataField="UserCounts" HeaderStyle-CssClass="td_right td_left"/>                                                                                                                                                                                                                                                                                                   
                            <asp:TemplateField HeaderText="百分比"  HeaderStyle-CssClass="td_right td_left">
                                <ItemTemplate>
                                    <img width='<%# string.Format("{0}px", Eval("Lenth")) %>' height="15" class="votelenth"/><asp:Literal ID="lblPercentage" runat="server" text='<%# DataBinder.Eval(Container, "DataItem.Percentage", "{0:N2}") %>' />%
                                </ItemTemplate>
                            </asp:TemplateField> 
                        </Columns>
               </UI:Grid>
     
	  </div>

</div>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
