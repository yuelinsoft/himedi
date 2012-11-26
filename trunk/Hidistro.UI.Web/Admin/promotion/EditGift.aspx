<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeFile="EditGift.aspx.cs" Inherits="Hidistro.UI.Web.Admin.EditGift" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" Runat="Server">
  <div class="dataarea mainwidth databody">
      <div class="title title_height m_none td_bottom"> <em><img src="../images/06.gif" width="32" height="32" /></em>
        <h1 class="title_line">编辑礼品</h1>
      </div>
      <div class="datafrom">
        <div class="formitem validator2">
          <ul>
            <li><span class="formitemtitle Pw_100">礼品名称：<em >*</em></span>
              <asp:TextBox ID="txtGiftName" runat="server" CssClass="forminput"></asp:TextBox>　<input type="checkbox" id="ckdown" runat="server" />是否允许下载
              <p id="ctl00_contentHolder_txtGiftNameTip">礼品名称，在1至60个字符之间</p>
            </li>
            <li class="m_none"><span class="formitemtitle Pw_100">礼品图片：</span>
                      <span class="Add_Goods">
                             <Hi:ImageUploader runat="server" UploadType="Gift" ID="uploader1" />
                        </span>
                        
            </li>
            <li class="clearfix"><span class="formitemtitle Pw_100">计量单位：</span>
             <asp:TextBox ID="txtUnit" runat="server" CssClass="forminput"></asp:TextBox>
            <p id="ctl00_contentHolder_txtUnitTip"></p>
            </li>
            <li> <span class="formitemtitle Pw_100">成本价：</span>
               <asp:TextBox ID="txtCostPrice" runat="server" CssClass="forminput"></asp:TextBox>
            <p id="ctl00_contentHolder_txtCostPriceTip"></p>
            </li>
            <li><span class="formitemtitle Pw_100">采购价：<em >*</em></span>
                 <asp:TextBox ID="txtPurchasePrice" runat="server" CssClass="forminput"></asp:TextBox>
             <p id="ctl00_contentHolder_txtPurchasePriceTip"></p>
            </li>
            <li> <span class="formitemtitle Pw_100">市场参考价：</span>
               <asp:TextBox ID="txtMarketPrice" runat="server" CssClass="forminput"></asp:TextBox>
             <p id="ctl00_contentHolder_txtMarketPriceTip">市场参考价只能是数值，且不能超过2位小数</p>
            </li>
            <li>
            <span class="formitemtitle Pw_100">兑换需积分：<em >*</em></span>
            <asp:TextBox ID="txtNeedPoint" runat="server" Text="0" CssClass="forminput"></asp:TextBox>
            <p id="ctl00_contentHolder_txtNeedPointTip">兑换所需积分只能是数字，必须大于等于O,0表示不能兑换</p>
	      </li>     
            <h2>礼品描述</h2>            
            <li> <span class="formitemtitle Pw_100">简单介绍：</span>
             <asp:TextBox ID="txtShortDescription" runat="server" TextMode="MultiLine" Width="250px" Height="70px"></asp:TextBox>
             <p id="ctl00_contentHolder_txtShortDescriptionTip"></p>
            </li>
            <li> <span class="formitemtitle Pw_100">详细信息：</span><Kindeditor:KindeditorControl ID="fcDescription" runat="server" Width="85%"  Height="200px"/>           </li>
            <h2>SEO设置</h2>
            <li><span class="formitemtitle Pw_100">详细页标题：</span>
                <asp:TextBox ID="txtGiftTitle" runat="server" CssClass="forminput"></asp:TextBox>
                <p id="ctl00_contentHolder_txtGiftTitleTip"></p>
            </li>
            <li> <span class="formitemtitle Pw_100">详细页关键字：</span>
               <asp:TextBox ID="txtTitleKeywords" runat="server" CssClass="forminput"></asp:TextBox>
               <p id="ctl00_contentHolder_txtTitleKeywordsTip"></p>
            </li>
            <li> <span class="formitemtitle Pw_100">详细页描述：</span>
               <asp:TextBox ID="txtTitleDescription" runat="server" CssClass="forminput"></asp:TextBox>
               <p id="ctl00_contentHolder_txtTitleDescriptionTip"></p>
            </li>
          </ul>
           <ul class="btntf Pa_100">
		     <asp:Button ID="btnUpdate" runat="server" Text="保 存" OnClientClick="return PageIsValid();"  CssClass="submit_DAqueding inbnt"  />
            
		  </ul>
</div>
      </div>
</div>



</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" language="javascript">
function InitValidators()
{
initValid(new InputValidator('ctl00_contentHolder_txtGiftName', 1, 60, false, null,  '礼品的名称，在1至60个字符之间'))
initValid(new InputValidator('ctl00_contentHolder_txtUnit', 1, 10, true, null,  '计量单位，在1至10个字符之间'))
initValid(new InputValidator('ctl00_contentHolder_txtCostPrice', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)',  '成本价只能是数值，且不能超过2位小数'))
appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtCostPrice', 0.01, 10000000, '成本价只能是数值，不能超过10000000，且不能超过2位小数'));
initValid(new InputValidator('ctl00_contentHolder_txtPurchasePrice', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', ' 采购价只能是数值，不能为空，且不能超过2位小数'))
appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtPurchasePrice', 0.01, 10000000, ' 采购价只能是数值，不能超过10000000，且不能超过2位小数'));
initValid(new InputValidator('ctl00_contentHolder_txtMarketPrice', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '市场参考价只能是数值，且不能超过2位小数'))
appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtMarketPrice', 0.01, 10000000, '市场参考价只能是数值，不能超过10000000，且不能超过2位小数'));
initValid(new InputValidator('ctl00_contentHolder_txtNeedPoint', 1, 10, false, '-?[0-9]\\d*', '兑换所需积分只能是数字，必须大于等O,0表示不能兑换'))
appendValid(new NumberRangeValidator('ctl00_contentHolder_txtNeedPoint', 0, 10000, '兑换所需积分不能为空，大小0-10000之间'));
initValid(new InputValidator('ctl00_contentHolder_txtShortDescription', 1, 300, true, null,  '简单介绍，在1至300个字符之间'))
initValid(new InputValidator('ctl00_contentHolder_txtGiftTitle', 1, 60, true, null,  '详细页标题，在1至60个字符之间'))
initValid(new InputValidator('ctl00_contentHolder_txtTitleKeywords', 1, 100, true, null,  '详细页关键字，在1至100个字符之间'))
initValid(new InputValidator('ctl00_contentHolder_txtTitleDescription', 1, 100, true, null,  '详细页描述，在1至100个字符之间'))

}
$(document).ready(function(){ InitValidators(); });
</script>
</asp:Content>
 
