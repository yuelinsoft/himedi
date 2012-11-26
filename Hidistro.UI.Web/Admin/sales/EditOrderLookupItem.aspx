<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeFile="EditOrderLookupItem.aspx.cs" Inherits="Hidistro.UI.Web.Admin.EditOrderLookupItem" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="areacolumn clearfix">
		<div class="columnleft clearfix">
                  <ul>
                       <li><a href="OrderLookupLists.aspx"><span>订单可选项列表</span></a></li>
                  </ul>
</div>
      <div class="columnright">
          <div class="title">
            <em><img src="../images/05.gif" width="32" height="32" /></em>
            <h1>编辑订单可选项</h1>
            <span>订单可选项是顾客在下订单时可以额外选择的一些项目，您可以自定义这些项目供顾客选择，比如：是否需要发票等。</span>
</div>
          <div class="formtab Pg_45">
                   <ul>
                      <li><a href='<%="EditOrderLookup.aspx?LookupListId=" +Page.Request.QueryString["LookupListId"] %>'>基本信息</a></li>                                      
                      <li class="visited">可选项内容</li>
            </ul>
          </div>
        <div class="content">
            <asp:GridView ID="grdOrderLookupItems" runat="server" AutoGenerateColumns="false" DataKeyNames="LookupItemId" GridLines="None" Width="100%" HeaderStyle-CssClass="table_title">
                <Columns>
                    <asp:TemplateField HeaderText="属性值名称" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <asp:Label ID="lblLookupList" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="附加金额" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblPercentageAppendMoney"></asp:Label>
                            <Hi:FormatedMoneyLabel ID="lblAppendMoney" runat="server" Money='<%# Bind("AppendMoney") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="用户信息标题" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblUserInputTitle" Text='<%# Eval("UserInputTitle")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField> 
                    <asp:TemplateField HeaderText="备注" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblRemark" Text='<%# Eval("Remark")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="td_left td_right_fff">
                           <ItemTemplate>
                            <span class="submit_shanchu"><Hi:ImageLinkButton runat="server" ID="Delete" IsShow="true" Text="删除" CommandName="Delete" /></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
          </div>
        <div class="Pg_15">
             <input name="button" type="button" value="添加可选项内容" onclick="DivWindowOpen(500,400,'DivAddOrderLookupItem');" class="submit_bnt2"/>
          </div>
          <div class="Pg_15">
           <asp:Button Text="保 存" runat="server" ID="lkOver" class="submit_DAqueding "/>
          </div>
      </div>
        
  </div>
<div class="databottom">
  <div class="databottom_bg"></div>
</div>
<div class="bottomarea testArea">
  <!--顶部logo区域-->
</div>

<div class="Pop_up" id="DivAddOrderLookupItem" style="display:none; overflow:hidden;">
  <h1>添加可选项内容</h1>
    <div class="img_datala"><img src="../images/icon_dalata.gif" width="38" height="20" /></div>
    <div class="mianform">
    <ul>
              <li> <span class="formitemtitle Pw_128">可选项内容名称：<em>*</em></span>
              <asp:TextBox ID="txtName" runat="server" CssClass="forminput"></asp:TextBox></li>
              <li> <span class="formitemtitle Pw_128">用户填写项：</span>
              <Hi:YesNoRadioButtonList ID="radlUserInput" onclick="ShowTitle()" runat="server" SelectedValue="false" style="display:inline" />
              </li>
              <li runat="server" id="liUserInputTitle" style="display:none"> 
	                <span>用户填写信息的标题：</span>
                    <asp:TextBox ID="txtUserInputTitle" runat="server" CssClass="forminput"></asp:TextBox>
	             </li>
	             <li> <span class="formitemtitle Pw_128">附加金额计算方式：</span>
              <Hi:CalculateModeRadioButtonList ID="radlCalculateMode" onclick="ShowAppendText()" runat="server" SelectedValue="1" RepeatDirection="Horizontal" style="display:inline" />
              </li>
              <li><span class="formitemtitle Pw_128">附加金额：</span>
              <span>
                  <table width="250" border="0" align="left" cellspacing="0" class="Pg_20 colorQ">
                    <tr id="trFixedrate"  style="display:block;">
                      <td width="103" align="left">固定金额</td>
                      <td width="95" align="left"> <asp:TextBox ID="txtAppendMoney" runat="server" CssClass="forminput" Text="0"></asp:TextBox></td>
                      <td width="21" align="left">元</td>
                    </tr>
                    <tr id="trPercentage"  style="display:none;">
                      <td align="left" width="100px">购物车金额百分比</td>
                      <td align="left"><asp:TextBox ID="txtPercentage" runat="server" CssClass="forminput" Text="0" Width="130"></asp:TextBox></td>
                      <td align="left">%</td>
                    </tr>
                  </table>
                 </span> 
              </li>
                   <li>
		             <span class="formitemtitle Pw_128">备注：</span>
		             <asp:TextBox ID="txtRemark" runat="server" Width="300" Height="60" TextMode="MultiLine"></asp:TextBox>
		         </li>
      </ul>
       <ul class="up Pa_128 clearfix">
       <asp:Button ID="btnCreate" OnClientClick="return PageIsValid();" Text="添 加" CssClass="submit_DAqueding" runat="server"/>
  </ul>
  </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" language="javascript">
    function ShowTitle() {
        var radlUserInput = document.getElementById("ctl00_contentHolder_radlUserInput_1");
        var liUserInputTitle = document.getElementById("ctl00_contentHolder_liUserInputTitle");
        var txtUserInputTitle = document.getElementById("ctl00_contentHolder_txtUserInputTitle");
        if (radlUserInput.checked)
         {
            liUserInputTitle.style.display = "none";
            txtUserInputTitle.value = "";
        }
        else {
            liUserInputTitle.style.display = "block";
        }
    }

    function ShowAppendText() {
        var radlCalculateMode = document.getElementById("ctl00_contentHolder_radlCalculateMode_0");
        var trFixedrate = document.getElementById("trFixedrate");
        var trPercentage = document.getElementById("trPercentage");

        if (radlCalculateMode.checked) {
            trFixedrate.style.display = "block";
            trPercentage.style.display = "none";
        }
        else {
            trFixedrate.style.display = "none";
            trPercentage.style.display = "block";
        }
    } 
                
</script>
</asp:Content>
