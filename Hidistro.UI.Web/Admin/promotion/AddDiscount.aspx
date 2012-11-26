<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeFile="AddDiscount.aspx.cs" Inherits="Hidistro.UI.Web.Admin.AddDiscount" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" Runat="Server">



<div class="dataarea mainwidth databody">
    <div class="title m_none td_bottom title_height"> 
      <em><img src="../images/07.gif" width="32" height="32" /></em>
      <h1 class=" title_line">添加满额打折促销活动</h1>
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
                <li><span class="formitemtitle Pw_110">促销详细信息：</span> 
                    <Kindeditor:KindeditorControl ID="fckDescription" runat="server" Width="550px"  Height="200px"/>
                  </li>
                  </ul>
                     </SkinTemplate>
                    </Hi:PromoteView>
                    
                    </li>
                    
                  <li> <span class="formitemtitle Pw_110">满足金额：<em>*</em></span>
                    <asp:TextBox ID="txtAmount" runat="server" CssClass="forminput"></asp:TextBox>
                    <p id="ctl00_contentHolder_txtAmountTip">满足金额只能是数值，不能超过10000000，且不能超过2位小数</p>
                  </li>
                  <li> <span class="formitemtitle Pw_110">打折方式：</span>
                     <Hi:DiscountValueTypeRadioButtonList id="radioValueType" onclick="DiscountType(this);" runat="server" style="display:inline;"></Hi:DiscountValueTypeRadioButtonList>
                  </li>
                  <li id="discountValue"> <span class="formitemtitle Pw_110">优惠金额：<em>*</em></span>
                   <asp:TextBox ID="txtDiscountValue" runat="server" CssClass="forminput"></asp:TextBox>
                   <p id="ctl00_contentHolder_txtDiscountValueTip">优惠金额只能是数值，1-10000000</p>
                 </li>
                 
                 <li id="discountRate"> <span class="formitemtitle Pw_110">折扣率：<em>*</em></span>
                     <asp:TextBox ID="txtDiscountRate" runat="server" CssClass="forminput"></asp:TextBox>%
                     <p id="ctl00_contentHolder_txtDiscountRateTip">折扣率只能是数值</p>
                 </li>
                 
                  <li> <span class="formitemtitle Pw_110">适合的客户：<em>*</em></span>
                   <span style="float:left;"><Hi:MemberGradeCheckBoxList ID="chklMemberGrade" runat="server" RepeatDirection="Horizontal" /></span>
                  </li>
                 
                </ul>
                <ul class="btntf Pa_110 clear">
                 <asp:Button ID="btnAddDiscount" OnClientClick="return PageIsValid();" runat="server" Text="添加"  CssClass="submit_DAqueding inbnt" style="float:left"></asp:Button>
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

            function DiscountType(obj) {

                var radioValueType = document.getElementById(obj.id);
                var liDiscountValue = document.getElementById("discountValue");
                var liDiscountRate = document.getElementById("discountRate");

                var radioValueTypes = radioValueType.getElementsByTagName('input');
                var checkedValue=null;
                for (var i = 0; i < radioValueTypes.length; i++) {
                    if (radioValueTypes[i].type == "radio") {
                        if (radioValueTypes[i].checked) {
                            checkedValue = radioValueTypes[i].value;
                            break;
                        }
                    }
                }
               
                if (checkedValue == "0") {
                    liDiscountValue.style.display = "block";
                    liDiscountRate.style.display = "none";
                }
                if (checkedValue == "1") {
                    liDiscountValue.style.display = "none";
                    liDiscountRate.style.display = "block";
                }                                
                
      }
function InitValidators()
{
    initValid(new InputValidator('ctl00_contentHolder_addpromoteSales_txtPromoteSalesName', 1, 60, false, null, '促销活动的名称，在1至60个字符之间'))
    initValid(new InputValidator('ctl00_contentHolder_txtAmount', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '满足金额只能是数值，不能超过10000000，且不能超过2位小数'))
    appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtAmount', 0.01, 10000000, '满足金额只能是数值，不能超过10000000，且不能超过2位小数'));
initValid(new InputValidator('ctl00_contentHolder_txtDiscountValue', 1, 10, true, '-?[0-9]\\d*', '优惠金额只能是正整数'))
appendValid(new NumberRangeValidator('ctl00_contentHolder_txtDiscountValue', 1, 10000000, '优惠金额只能是数值，1-10000000'));
initValid(new InputValidator('ctl00_contentHolder_txtDiscountRate', 1, 10, true, '-?[0-9]\\d*', '折扣率只能是数值'))
appendValid(new NumberRangeValidator('ctl00_contentHolder_txtDiscountRate', 1, 100, '折扣率只能是数值，1-100'));

}
$(document).ready(function() { InitValidators(); DiscountType(ctl00_contentHolder_radioValueType); });
</script>

</asp:Content>

