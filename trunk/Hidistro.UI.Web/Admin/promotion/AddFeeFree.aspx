<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeFile="AddFeeFree.aspx.cs" Inherits="Hidistro.UI.Web.Admin.AddFeeFree" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" Runat="Server">
    
    <div class="dataarea mainwidth databody">
    <div class="title m_none td_bottom"> 
      <em><img src="../images/07.gif" width="32" height="32" /></em>
      <h1>添加满额免费用促销活动</h1>
      <span>满额免费用是针对订单总金额的一种优惠方式，顾客当次订单总金额达到此活动的满足金额以后，就可以免除指定项目的费用</span>
   </div>
    <div class="datafrom">
    <div class="formitem validator4">
                <ul>
                
                <li>
                  <Hi:PromoteView id="addpromoteSales" runat="server">
                        <SkinTemplate> 
                        <ul>
                  <li> <span class="formitemtitle Pw_110">促销活动名称：<em>*</em></span>
                  <asp:TextBox ID="txtPromoteSalesName" runat="server" CssClass="forminput"></asp:TextBox>
                    <p id="ctl00_contentHolder_addpromoteSales_txtPromoteSalesNameTip">活动的名称，在1至60个字符之间</p>
                  </li>
                  
                  <li> <span class="formitemtitle Pw_110">促销详细信息：</span> 
                   <Kindeditor:KindeditorControl ID="fckDescription" runat="server" Width="550px"  Height="200px"/>
                   </li>
                  </ul>
                     </SkinTemplate>
                    </Hi:PromoteView>
                  </li>                  
                  
                  <li> <span class="formitemtitle Pw_110">满足金额：<em>*</em></span>
                    <asp:TextBox ID="txtAmount" runat="server" CssClass="forminput"></asp:TextBox>
                   <p id="ctl00_contentHolder_txtAmountTip"></p>
                  </li>
                  <li> <span class="formitemtitle Pw_110">适合的客户：<em>*</em></span>
                   <span style="float:left;"><Hi:MemberGradeCheckBoxList ID="chklMemberGrade" runat="server" RepeatDirection="Horizontal" /></span>
                  </li>
                  <li> <span class="formitemtitle Pw_110">订单免费项目：<em>*</em></span>
             	    <asp:CheckBox ID="chkShipChargeFee" runat="server" />运费
                    <asp:CheckBox ID="chkPackingChargeFree" runat="server" />订单可选项产生的费用
                    <asp:CheckBox ID="chkServiceChargeFree" runat="server" />支付手续费 </li>
                  
                </ul>
                <ul class="btntf Pa_110 clear">
                <asp:Button ID="btnAddFeeFree" OnClientClick="return PageIsValid();" runat="server" Text="添加"  CssClass="submit_DAqueding inbnt" ></asp:Button>
      </ul>
      </div>
    </div>
</div>
  <div class="bottomarea testArea">
    <!--顶部logo区域-->
  </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" language="javascript">
function InitValidators()
{
initValid(new InputValidator('ctl00_contentHolder_addpromoteSales_txtPromoteSalesName', 1, 60, false, null,  '促销活动的名称，在1至60个字符之间'))
initValid(new InputValidator('ctl00_contentHolder_txtAmount', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '满足金额只能是数值，且不能超过2位小数'))
appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtAmount', 0.01, 10000000, '满足金额只能是数值，不能超过10000000，且不能超过2位小数'));
}
$(document).ready(function(){ InitValidators(); });
</script>
</asp:Content>
