<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.SiteContent" CodeFile="SiteContent.aspx.cs" MasterPageFile="~/Admin/Admin.Master" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth databody">
      <div class="title title_height m_none td_bottom"> <em><img src="../images/01.gif" width="32" height="32" /></em>
        <h1 class="title_line">店铺基本设置</h1>
      </div>
      <div class="datafrom">
        <div class="formitem validator1">
          <ul>
            <li><span class="formitemtitle Pw_198">店铺名称：<em >*</em></span>
              <asp:TextBox ID="txtSiteName" CssClass="forminput formwidth" runat="server"  />
              <p id="txtSiteNameTip" runat="server">店铺名称为必填项，长度限制在60字符以内</p>
            </li>
            <li><span class="formitemtitle Pw_198">店铺标志：</span>              
              <asp:FileUpload ID="fileUpload" runat="server" CssClass="forminput" />
              <asp:Button ID="btnUpoad" runat="server" Text="上传" CssClass="submit_queding" OnClick="btnUpoad_Click" />              
                <table width="300" border="0" cellspacing="0">
                  <tr>
                    <td valign="middle" style="padding-top:10px;"><Hi:HiImage ID="imgLogo" runat="server" Width="180" Height="40" />&nbsp;<Hi:ImageLinkButton ID="btnDeleteLogo"  runat="server" Text="删除" OnClick="btnDeleteLogo_Click" IsShow="true" /></td>
                  </tr>
                  <tr><td align="left"></td></tr>
                </table>
            </li>
            <li style="padding-top:5px;"><span class="formitemtitle Pw_198">网店域名：<em >*</em></span>
              <span class="float">http://</span><asp:TextBox ID="txtDomainName" CssClass="forminput" runat="server"></asp:TextBox>
              <p id="txtDomainNameTip" runat="server">店铺域名必须控制在128个字符以内</p>
            </li>
            <li> <span class="formitemtitle Pw_198">是否开启伪静态：</span>
              <Hi:YesNoRadioButtonList ID="radEnableHtmRewrite" runat="server" RepeatLayout="Flow" />              
            </li>    
            <li><span class="formitemtitle Pw_198">自定义页尾：</span>
              <span style="display:block; float:left;width:550px;height:300px;overflow:hidden;"><Kindeditor:KindeditorControl ID="fkFooter" runat="server" Width="550px" Height="300px"/></span>
            </li>
            
            
            
            <h2 class="clear">商品设置</h2>
            <li><span class="formitemtitle Pw_198">商品价格精确到小数点后位数：</span>
              <Hi:DecimalLengthDropDownList ID="dropBitNumber" CssClass="forminput" runat="server" />
            </li>
            <li><span class="formitemtitle Pw_198">默认商品图片：</span>
                 <div class="Pa_198 Pg_8"><Hi:ImageUploader runat="server" ID="uploader1" /></div>              
            </li>
            <li><span class="formitemtitle Pw_198">几元一积分：<em >*</em></span>
              <asp:TextBox Id="txtProductPointSet" runat="server" CssClass="forminput"></asp:TextBox>
              <p id="txtProductPointSetTip" runat="server">几元一积分不能为空,必须在0.1-10000000之间</p>
            </li>
            <li><span class="formitemtitle Pw_198">“您的价”重命名为：</span>
                <asp:TextBox Id="txtNamePrice" runat="server" CssClass="forminput"></asp:TextBox>
                <span id="txtNamePriceTip" runat="server"></span>
            </li>
            <h2 class="clear">订单设置</h2>
            <li><span class="formitemtitle Pw_198">显示几天内订单数：<em >*</em></span>
              <asp:TextBox ID="txtShowDays" runat="server" CssClass="forminput" />
              <p id="txtShowDaysTip" runat="server">前台发货查询中显示最近几天内的订单项</p>
            </li>
            <li><span class="formitemtitle Pw_198">过期几天自动关闭订单：<em >*</em></span>
              <asp:TextBox ID="txtCloseOrderDays" runat="server" CssClass="forminput" />
              <p id="txtCloseOrderDaysTip" runat="server">下单后过期几天系统自动关闭未付款订单</p>
            </li>
             <li><span class="formitemtitle Pw_198">过期几天自动关闭采购单：<em >*</em></span>
              <asp:TextBox ID="txtClosePurchaseOrderDays" runat="server" CssClass="forminput" />
              <p id="txtClosePurchaseOrderDaysTip" runat="server">采购后过期几天系统自动关闭未付款的采购单</p>
            </li>
             <li><span class="formitemtitle Pw_198">发货几天自动完成订单：<em >*</em></span>
              <asp:TextBox ID="txtFinishOrderDays" runat="server" CssClass="forminput" />
              <p id="txtFinishOrderDaysTip" runat="server">发货几天后，系统自动把订单改成已完成状态</p>
            </li>
            <li><span class="formitemtitle Pw_198">发货几天自动完成采购单：<em >*</em></span>
              <asp:TextBox ID="txtFinishPurchaseOrderDays" runat="server" CssClass="forminput" />
              <p id="txtFinishPurchaseOrderDaysTip" runat="server">发货几天后，系统自动把采购单改成已完成状态</p>
            </li>
            <h2 class="clear">会员设置</h2>
            <li><span class="formitemtitle Pw_198">是否开启主站会员零售：</span>
            <Hi:YesNoRadioButtonList ID="radiIsOpenSiteSale" runat="server" RepeatLayout="Flow" />  
            </li>
            <h2 class="clear">促销设置</h2>  
              <li> <span class="formitemtitle Pw_198">是否显示团购标签：</span>
              <Hi:YesNoRadioButtonList ID="radiIsShowGroupBuy" runat="server" RepeatLayout="Flow" />              
            </li>    
            <li> <span class="formitemtitle Pw_198">是否显示限时抢购标签：</span>
                <Hi:YesNoRadioButtonList ID="radiIsShowCountDown" runat="server" RepeatLayout="Flow" />              
            </li>   
            <li> <span class="formitemtitle Pw_198">是否显示积分商城标签：</span>
                <Hi:YesNoRadioButtonList ID="radiIsShowOnlineGift" runat="server" RepeatLayout="Flow" />      
            </li>     
            <h2 class="clear">SEO设置</h2>            
            <li> <span class="formitemtitle Pw_198">简单介绍：</span>
              <asp:TextBox ID="txtSiteDescription" CssClass="forminput formwidth" runat="server"></asp:TextBox>
              <span id="txtSiteDescriptionTip" runat="server"></span>
            </li>
            <li> <span class="formitemtitle Pw_198">店铺描述：</span>
              <asp:TextBox ID="txtSearchMetaDescription" runat="server" CssClass="forminput formwidth"></asp:TextBox>
              <span id="txtSearchMetaDescriptionTip" runat="server"></span>
            </li>
            <li><span class="formitemtitle Pw_198">搜索关键字：</span>
              <asp:TextBox ID="txtSearchMetaKeywords" CssClass="forminput formwidth" runat="server" />
              <span id="txtSearchMetaKeywordsTip" runat="server"></span>
            </li>
              <h2 class="clear">在线客服设置</h2>
            <li> <span class="formitemtitle Pw_198">在线客服：</span>
                <span style="display:block; float:left;width:550px;height:300px;overflow:hidden;"><Kindeditor:KindeditorControl ID="fcOnLineServer" runat="server" Width="550px" ImportLib="false" Height="300px"/></span>
            </li>
            
            <h2 class="clear">网站版权信息</h2>
            <li>
            <span class="formitemtitle Pw_198">作者(author)：</span>
            <asp:TextBox ID="txtAuthor" CssClass="forminput formwidth" Text="Hishop development team" runat="server" />
            <span id="txtAuthorTip" runat="server"></span>
            </li>
                        <li>
            <span class="formitemtitle Pw_198">生成(GENERATOR)：</span>
            <asp:TextBox ID="txtGenerator" CssClass="forminput formwidth" Text="易分销 2.0" runat="server" />
            <span id="txtGeneratorTip" runat="server"></span>
            </li>
          </ul>
          <div style="clear:both"></div>
           <ul class="btntf Pa_198">
            <asp:Button ID="btnOK" runat="server" Text="提 交" OnClick="btnOK_Click" CssClass="submit_DAqueding inbnt" OnClientClick="return PageIsValid();" />
	       </ul>
        </div>
      </div>
</div>
	
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" language="javascript">
    function InitValidators() {
        initValid(new InputValidator('ctl00_contentHolder_txtSiteName', 1, 60, false, null, '店铺名称为必填项，长度限制在60字符以内'))
        initValid(new InputValidator('ctl00_contentHolder_txtDomainName', 1,128, false, '^[0-9a-zA-Z]([0-9a-zA-Z-\.]+)$', '店铺域名必须控制在128个字符以内'))
        initValid(new InputValidator('ctl00_contentHolder_txtSiteDescription', 0, 60, true, null, '简单介绍TITLE的长度限制在60字符以内'))
        initValid(new InputValidator('ctl00_contentHolder_txtSearchMetaDescription', 0, 100, true, null, '店铺描述META_DESCRIPTION的长度限制在100字符以内'))
        initValid(new InputValidator('ctl00_contentHolder_txtSearchMetaKeywords', 0, 100, true, null, '搜索关键字META_KEYWORDS的长度限制在100字符以内，多个关键字之间用,号分开'))
        initValid(new InputValidator('ctl00_contentHolder_txtTQCode', 0, 4000, true, null, '请在这里填入您获取的网页客服代码'))
        initValid(new SelectValidator('ctl00_contentHolder_dropBitNumber', false, '设置商品的价格经过运算后数值精确到小数点后几位，超出将四舍五入。'))
        initValid(new InputValidator('ctl00_contentHolder_txtProductPointSet', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '几元一积分不能为空,必须在0.1-10000000之间'))
        appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtProductPointSet', 0.1, 10000000, '几元一积分必须在0.1-10000000之间'));
        initValid(new InputValidator('ctl00_contentHolder_txtNamePrice', 1, 10, true, null, '设置前台商品的“您的价”重命名为其他名称，如“批发价”'))
        initValid(new InputValidator('ctl00_contentHolder_txtShowDays', 1, 10, false, '-?[0-9]\\d*', '设置前台发货查询中显示最近几天内的已发货订单'))
        appendValid(new NumberRangeValidator('ctl00_contentHolder_txtShowDays', 0, 90, '设置前台发货查询中显示最近几天内的已发货订单'));
        
        initValid(new InputValidator('ctl00_contentHolder_txtCloseOrderDays', 1, 10, false, '-?[0-9]\\d*', '下单后过期几天系统自动关闭未付款订单'))
        appendValid(new NumberRangeValidator('ctl00_contentHolder_txtCloseOrderDays', 0, 90, '下单后过期几天系统自动关闭未付款订单'));
        
        initValid(new InputValidator('ctl00_contentHolder_txtClosePurchaseOrderDays', 1, 10, false, '-?[0-9]\\d*', '采购后过期几天系统自动关闭未付款的采购单'))
        appendValid(new NumberRangeValidator('ctl00_contentHolder_txtClosePurchaseOrderDays', 0, 90, '采购后过期几天系统自动关闭未付款的采购单'));
        
        initValid(new InputValidator('ctl00_contentHolder_txtFinishOrderDays', 1, 10, false, '-?[0-9]\\d*', '发货几天后，系统自动把订单改成已完成状态'))
        appendValid(new NumberRangeValidator('ctl00_contentHolder_txtFinishOrderDays', 0, 90, '发货几天后，系统自动把订单改成已完成状态'));
        
        initValid(new InputValidator('ctl00_contentHolder_txtFinishPurchaseOrderDays', 1, 10, false, '-?[0-9]\\d*', '发货几天后，系统自动把采购单改成已完成状态'))
        appendValid(new NumberRangeValidator('ctl00_contentHolder_txtFinishPurchaseOrderDays', 0, 90, '发货几天后，系统自动把采购单改成已完成状态'));
    }
    $(document).ready(function() { InitValidators(); });
</script>
</asp:Content>

