<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeFile="SetProductSubject.aspx.cs" Inherits="Hidistro.UI.Web.Admin.SetProductSubject" %>
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
                        <li><asp:HyperLink runat="server" ID="hLinkSubjects"><span>商品栏目</span></asp:HyperLink></li>
                  </ul>
    </div>
      <div class="columnright">
          <div class="title title_height">
            <em><img src="../images/01.gif" width="32" height="32" /></em>
            <h1>设置商品栏目</h1>
            <span>对前台首页“Common_GoodsList_Default”商品展示标签参数的相关设置</span>
          </div>
      <div class="formitem validator2">
        <ul>
          <li><span class="formitemtitle Pw_100">栏目名称：<em >*</em></span>
            <asp:TextBox ID="txtSubjectName" runat="server" CssClass="forminput" />
            <p id="txtSubjectNameTip" runat="server">用于在前台商品栏目版块显示的名称,不能为空，限制在30个字符以内</p>
          </li>
          <li><span class="formitemtitle Pw_100">栏目图片：</span>
                <asp:FileUpload ID="fileUpload" runat="server" CssClass="forminput" />           
               <asp:Button ID="btnUpoad" runat="server" Text="上传" CssClass="submit_queding" />     
          </li>
              <li> <span class="formitemtitle Pw_100"></span>
           <table width="300" border="0" cellspacing="0">
                  <tr>
                    <td valign="middle" style="padding-top:10px;"><Hi:HiImage ID="imgLogo" runat="server" Width="180" Height="40" />&nbsp;<Hi:ImageLinkButton ID="btnDeleteLogo"  runat="server" Text="删除" IsShow="true" /></td>
                  </tr>
                  <tr><td align="left"></td></tr>
                </table>
           </li>
          <li> <span class="formitemtitle Pw_100">关联标签：</span>
            <asp:ListBox ID="radProductTags" runat="server">
                <asp:ListItem Text="所有" Value="0"></asp:ListItem>
                <asp:ListItem Text="热卖商品" Value="1"></asp:ListItem>
                <asp:ListItem Text="推荐商品" Value="3"></asp:ListItem>
                <asp:ListItem Text="新品上架" Value="4"></asp:ListItem>
                <asp:ListItem Text="特价商品" Value="2"></asp:ListItem>
             </asp:ListBox>
             </li>
             <p>限制栏目版块只显示选中<a target="_blank" href="../product/SubjectProducts.aspx">标签下的商品</a>，所有表示不限制</p>
             <li> <span class="formitemtitle Pw_100">关联店铺分类：</span>
                <Hi:ProductCategoriesListBox runat="server" ID="listProductCategories" SelectionMode="Multiple" />
                       </li>
             <p>限制栏目版块只显示选中分类下的商品（可以选择多个分类），不选表示不限制</p>
             <li><span class="formitemtitle Pw_100">价格区间：</span>
             <span class="float"><table width="200" border="0" cellspacing="0">
                          <tr>
                            <td width="45">
                                <asp:TextBox runat="server" ID="txtMinPrice" CssClass="forminput" Width="100" />
                            </td>
                            <td width="55" align="center"><strong>-</strong></td>
                            <td width="48">
                               <asp:TextBox runat="server" ID="txtMaxPrice" CssClass="forminput" Width="100" />
                            </td>
                          </tr>                          
                        </table>
                    </span>
            <p>限制栏目版块只显示价格区间内的商品，不填表示不限制</p>
          </li>
          <li><span class="formitemtitle Pw_100">关键字：</span>
            <asp:TextBox ID="txtKeywords" runat="server" CssClass="forminput" />
            <p>限制栏目版块只显示符合输入关键字的商品，不填表示不限制，输入多个关键字用空格隔开</p>
          </li>
          <li><span class="formitemtitle Pw_100">显示数量：<em >*</em></span>
            <asp:TextBox ID="txtMaxNum" runat="server" CssClass="forminput" />
            <p>限制栏目版块商品的显示数量</p>
          </li>
      </ul>
      <ul class="btn Pa_100">
        <asp:Button ID="btnSaveSubject" runat="server" OnClientClick="return PageIsValid();" Text="保 存"  CssClass="submit_DAqueding" />
        </ul>
      </div>

      </div>
  </div>
  <div class="databottom"></div>
<div class="bottomarea testArea">
  <!--顶部logo区域-->
  </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" language="javascript">
    function InitValidators() {
        initValid(new InputValidator('ctl00_contentHolder_txtSubjectName', 1, 30, false, null, '商品栏目名称不能为空,长度在1-30之间'));
        initValid(new InputValidator('ctl00_contentHolder_txtMaxNum', 1, 10, false, null, '显示商品数据不能为空,长度在1-10之间'));
        appendValid(new NumberRangeValidator('ctl00_contentHolder_txtMaxNum', 0, 1000, '前台一个商品栏目的商品显示数据控制在1000以内'));
    }
    $(document).ready(function() { InitValidators(); });
        </script>
</asp:Content>
