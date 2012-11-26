<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.ExpressComputerpes" CodeFile="ExpressComputerpes.aspx.cs" MasterPageFile="~/Admin/Admin.Master" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth td_top_ccc">
   <div class="functionHandleArea clearfix">
			<!--分页功能-->
			   <div class="pageHandleArea">
				<ul><li><a href="#" onclick="javascript:ShowAddSKUValueDiv('添加','','')" class="submit_jia">添加物流公司</a></li></ul>
			</div>
			<table><tr><td>公司名称：<asp:TextBox ID="txtcompany" runat="server"></asp:TextBox></td><td>英文名称：<asp:TextBox ID="txtenglish" runat="server"></asp:TextBox></td><td>  <asp:Button ID="btnSearchButton" runat="server" class="searchbutton" Text="查询" /></td><td></td></tr></table>
			
			<!--结束-->
	  </div>
	<!--数据列表区域-->
	<div class="datalist">
	<div>
	 <UI:Grid ID="grdExpresscomputors" runat="server" ShowHeader="true" DataKeyNames="Name" AutoGenerateColumns="false" GridLines="None" Width="100%" HeaderStyle-CssClass="border_background">
          <HeaderStyle CssClass="table_title" />
            <Columns>   
                    <asp:TemplateField HeaderText="物流公司"  HeaderStyle-CssClass="td_right">
                        <ItemTemplate><%#Eval("Name") %></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="英文名" HeaderStyle-CssClass="td_right">
                        <ItemTemplate><%# Eval("English")%></ItemTemplate>
                    </asp:TemplateField>
                 
                    <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="border_top border_bottom">
                        <ItemStyle CssClass="spanD spanN" />
                           <ItemTemplate>
	                           <span class="submit_bianji"><a href="#" onclick="ShowAddSKUValueDiv('编辑','<%# Eval("Name") %>','<%# Eval("English") %>')" class="SmallCommonTextButton">编辑</a></span>
	                           <span class="submit_shanchu"><Hi:ImageLinkButton runat="server" ID="Delete" CommandName="Del" CommandArgument=<%# Eval("Name") %>   IsShow="true" CssClass="SmallCommonTextButton" Text="删除"/></span>
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
<!--数据列表底部功能区域-->
</div>  
<div class="Pop_up" id="divexpresscomputers" style="display: none;">
<input type="hidden" id="hdcomputers" runat="server" />
    <h1>
    
       <span id="spExpressCmp"> 添加&quot;&quot;信息</span>
    </h1>
    <div class="img_datala">
        <img src="../images/icon_dalata.gif" width="38" height="20" /></div>
    <div class="mianform">
        <ul id="valueStr">
            <li><span class="formitemtitle Pw_110">公司名称：<em>*</em></span>
                <asp:TextBox ID="txtCmpName" CssClass="forminput" Width="250" runat="server" />
            </li>
            <li><span class="formitemtitle Pw_110">英文名称：<em>*</em></span>
            <asp:TextBox ID="txtEngCmpName" CssClass="forminput" Width="250" runat="server"></asp:TextBox>
            </li>
        </ul>
        <ul class="up Pa_100">
            <asp:Button ID="btnCreateValue" runat="server" Text="确 定" CssClass="submit_DAqueding"
                OnClientClick="return isFlagValue();" />
        </ul>
    </div>
</div>
<script>
function ShowAddSKUValueDiv(opers,strname,strenglish){
    $("#ctl00_contentHolder_hdcomputers").val(strname);
    if(strname!=""){
         $("#spExpressCmp").html(opers+'&quot;'+strname+'&quot;信息');
    }else{
         $("#spExpressCmp").html(opers+'物流公司信息');
    }
   
    $("#ctl00_contentHolder_txtCmpName").val(strname);
    $("#ctl00_contentHolder_txtEngCmpName").val(strenglish);
    DivWindowOpen(480,200,'divexpresscomputers');
}

function isFlagValue(){
    
}
</script> 
</asp:Content>
