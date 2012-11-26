<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeFile="Roles.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Roles" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

<div class="optiongroup mainwidth">
	  <ul>
	    <li class="menucurrent"><a href="Roles.aspx"><span>部门管理</span></a></li>
	    <li><a href="Managers.aspx"><span>管理员管理</span></a></li>
	    <li class="optionend"><a href="ManageLogs.aspx"><span>操作日志</span></a></li>
      </ul>
</div>
	<div class="dataarea mainwidth">
    <div class="toptitle"> <em><img src="../images/01.gif" width="32" height="32" /></em>
      <span class="title_height">如果需要为店铺添加多个管理员，则可以在划分部门以后，将管理员加入到各自的部门 </span>
    </div>
    <!--搜索-->
    <div class="functionHandleArea m_none">
      <!--分页功能-->
      <div class="pageHandleArea">
        <ul>
          <li><a class="submit_jia" href="javascript:DivWindowOpen(600,280,'AddRole')">添加新部门</a></li>
        </ul>
      </div>
      <!--结束-->
    <input runat="server" type="hidden" id="txtRoleId" />
    <input runat="server" type="hidden" id="txtRoleName" />
    </div>
    <!--数据列表区域-->
    <div class="datalist">
    
    <UI:Grid ID="grdGroupList" runat="server" AutoGenerateColumns="false" ShowHeader="true" DataKeyNames="RoleId"  GridLines="None" HeaderStyle-CssClass="table_title" Width="100%">    
                       <Columns>
                           
                            <asp:TemplateField HeaderText="部门名称" HeaderStyle-CssClass="td_right td_left">
                               <ItemTemplate>
		                              <asp:Label ID="lblRoleName" Text='<%#Eval("Name")%>' runat="server" />
                               </ItemTemplate>                                                                                    
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="职能说明" ItemStyle-Width="50%" HeaderStyle-CssClass="td_right td_left">
                                  <ItemTemplate>
                                    <div style="word-break:break-all;" ><asp:Literal ID="lblRoleDesc" Text='<%#Eval("Description") %>' runat="server"></asp:Literal></div>
                                  </ItemTemplate>                                 
                             </asp:TemplateField>
                             <asp:TemplateField HeaderText="操作" ItemStyle-Width="35%" HeaderStyle-CssClass="td_left td_right_fff">
                                 <ItemTemplate>
			                          <span class="submit_tiajia"><a href='<%# Globals.GetAdminAbsolutePath("/store/RolePermissions.aspx?roleId=" + Eval("RoleID"))%> ' >部门权限</a></span>
			                          <span class="submit_bianji"><a href="#" onclick="ShowEditDiv('<%# Eval("RoleId")%>','<%# Eval("Name")%>','<%#  Eval("Description")%>');">编辑</a></span>
			                          <span class="submit_shanchu"><Hi:ImageLinkButton  runat="server" IsShow="true" ID="DeleteImageLinkButton1" CommandName="Delete" Text="删除"/></span>
                                 </ItemTemplate>                               
                             </asp:TemplateField>  
                      </Columns>
                </UI:Grid>
                
     
      <div class="blank5 clearfix"></div>
    </div>
    <!--数据列表底部功能区域-->
    
    </div>
  <div class="bottomarea testArea">
    <!--顶部logo区域-->
  </div>
  
    <!--添加部门-->
    <div class="Pop_up" id="AddRole" style=" display:none;">
  <h1>添加部门 </h1>
  <div class="img_datala"><a href="#" onclick="CloseDiv('AddRole');"><img src="../images/icon_dalata.gif" width="38" height="20" /></a></div>
  <div class="mianform validator2">
<ul>
              <li> <span class="formitemtitle Pw_100">部门名称：<em >*</em></span>
                <asp:TextBox ID="txtAddRoleName" runat="server" CssClass="forminput"></asp:TextBox>
                 <p id="ctl00_contentHolder_txtAddRoleNameTip">部门名称不能为空,长度限制在60个字符以内</p>
      </li>
              <li><span class="formitemtitle Pw_100">职能说明：</span>
                <asp:TextBox ID="txtRoleDesc" runat="server" CssClass="forminput"></asp:TextBox>
                <p id="ctl00_contentHolder_txtRoleDescTip">说明部门具备哪些职能，长度限制在100个字符以内</p>
              </li>
              
        </ul>
        <ul class="up Pa_100 clear">
      <asp:Button ID="btnSubmitRoles" runat="server" Text="添 加"  OnClientClick="return PageIsValid()"  CssClass="submit_DAqueding"/> 
  </ul>
  </div>
</div>

<!--编辑部门-->
<div class="Pop_up" id="EditRole" style=" display:none;">
  <h1>编辑部门 </h1>
  <div class="img_datala"><a href="#" onclick="CloseDiv('EditRole');"><img src="../images/icon_dalata.gif" width="38" height="20" /></a></div>
  <div class="mianform validator2">
<ul>
              <li> <span class="formitemtitle Pw_100">部门名称：<em >*</em></span>
                <asp:TextBox ID="txtEditRoleName" runat="server" CssClass="forminput"></asp:TextBox>
                <p id="ctl00_contentHolder_txtEditRoleNameTip">部门名称不能为空,长度限制在60个字符以内</p>
      </li>
              <li><span class="formitemtitle Pw_100">职能说明：</span>
                <asp:TextBox ID="txtEditRoleDesc" runat="server" CssClass="forminput"></asp:TextBox>
                 <p id="ctl00_contentHolder_txtEditRoleDescTip">说明部门具备哪些职能，长度限制在100个字符以内</p>
              </li>
              
        </ul>
        <ul class="up Pa_100 clear">
      <asp:Button ID="btnEditRoles" runat="server" Text="确 定"  OnClientClick="return PageIsValid()"  CssClass="submit_DAqueding"/> 
  </ul>
  </div>
</div>



</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" language="javascript">
    function InitValidators() {
        initValid(new InputValidator('ctl00_contentHolder_txtAddRoleName', 1, 60, true, null, '部门名称不能为空,长度限制在60个字符以内'))
        initValid(new InputValidator('ctl00_contentHolder_txtRoleDesc', 0, 100, true, null, '说明部门具备哪些职能，职能说明的长度限制在100个字符以内'))
        initValid(new InputValidator('ctl00_contentHolder_txtEditRoleName', 1, 60, true, null, '部门名称不能为空,长度限制在60个字符以内'))
        initValid(new InputValidator('ctl00_contentHolder_txtEditRoleDescc', 0, 100, true, null, '说明部门具备哪些职能，职能说明的长度限制在100个字符以内'))    
    }       
   $(document).ready(function() { InitValidators(); });

       

        function ShowEditDiv(roleId, name, description) {

            $("#ctl00_contentHolder_txtRoleId").val(roleId);
            $("#ctl00_contentHolder_txtRoleName").val(name);
            $("#ctl00_contentHolder_txtEditRoleName").val(name);
            $("#ctl00_contentHolder_txtEditRoleDesc").val(description);
            DivWindowOpen(550,280,'EditRole');
         
        }
</script>
</asp:Content>