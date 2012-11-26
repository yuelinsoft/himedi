<%@ Page Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeFile="AddMyBuyToSend.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.AddMyBuyToSend" EnableSessionState="True"  %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">



     <div class="dataarea mainwidth databody">
    <div class="title m_none td_bottom title_height"> 
      <em><img src="../images/07.gif" width="32" height="32" /></em>
      <h1 class=" title_line">添加买几送几促销活动</h1>
   </div>
    <div class="datafrom">
    <div class="formitem validator4">
                <ul>
                  <li>   <Hi:DistributorPromoteview id="addpromoteSales" runat="server">
                        <SkinTemplate> 
                        <ul>
                  <li> <span class="formitemtitle Pw_110">促销活动名称：<em>*</em></span>
                   <asp:TextBox ID="txtPromoteSalesName" runat="server" CssClass="forminput"></asp:TextBox>
                    <p id="ctl00_contentHolder_addpromoteSales_txtPromoteSalesNameTip">活动的名称，在1至60个字符之间</p>
                  </li>
                   <li> <span class="formitemtitle Pw_110">促销详细信息：</span> 
                    <Kindeditor:KindeditorControl FileCategoryJson="~/Shopadmin/FileCategoryJson.aspx" UploadFileJson="~/Shopadmin/UploadFileJson.aspx" FileManagerJson="~/Shopadmin/FileManagerJson.aspx" ID="fckDescription" runat="server" Width="550px"  Height="200px"/>
                  </li>
                  </ul>
                     </SkinTemplate>
                    </Hi:DistributorPromoteview>
                  </li>
                  <li> <span class="formitemtitle Pw_110">购买数量：<em>*</em></span>
                    <asp:TextBox ID="txtBuyQuantity" runat="server" CssClass="forminput"></asp:TextBox>
                    <p id="ctl00_contentHolder_txtBuyQuantityTip">买几送几的购买数据，即购买商品的数量(在1-10000之间</p>
                  </li>
                  <li> <span class="formitemtitle Pw_110">赠送数量：<em>*</em></span>
                     <asp:TextBox ID="txtGiveQuantity" runat="server" CssClass="forminput"></asp:TextBox>
                     <p id="ctl00_contentHolder_txtGiveQuantityTip"></p>
                  </li>
                  <li> <span class="formitemtitle Pw_110">适合的客户：<em>*</em></span>
                  <span style="float:left;"><Hi:UnderlingGradeCheckBoxList ID="chklMemberGrade" runat="server" RepeatDirection="Horizontal" /></span>
                  </li>
                  
                </ul>
                <ul class="btntf Pa_110 clear">
                 <asp:Button ID="btnNext" runat="server" Text="下一步,添加促销商品" OnClientClick="return PageIsValid();" CssClass="submit_jixu inbnt m_none"/>
      </ul>
      </div>
    </div>
</div>
  <div class="bottomarea testArea">
  </div>
  


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
     <script type="text/javascript" language="javascript">
function InitValidators()
{
initValid(new InputValidator('ctl00_contentHolder_addpromoteSales_txtPromoteSalesName', 1, 60, false, null, '促销活动的名称，在1至60个字符之间'));
initValid(new InputValidator('ctl00_contentHolder_txtBuyQuantity', 1, 10, false, '-?[0-9]\\d*', '买几送几的购买数据，即购买商品的数量(在1-10000之间)'));
appendValid(new NumberRangeValidator('ctl00_contentHolder_txtBuyQuantity', 1, 10000, '请输入从1至10000之间的购买数量'));
initValid(new InputValidator('ctl00_contentHolder_txtGiveQuantity', 1, 10, false, '-?[0-9]\\d*', '买几送几的赠送数据，即赠送商品的数量(在1-10000之间)'));
appendValid(new NumberRangeValidator('ctl00_contentHolder_txtGiveQuantity', 1, 10000, '请输入从1至10000之间的赠送数量'));
}
$(document).ready(function(){ InitValidators(); });
</script>
</asp:Content>
