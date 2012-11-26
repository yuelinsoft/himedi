<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeFile="EditMyCountDown.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.EditMyCountDown" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" src="groupbuy.helper.js"></script>
<script type="text/javascript" language="javascript">
      function InitValidators() {

          initValid(new InputValidator('ctl00_contentHolder_txtPrice',1, 10, false, '([0-9]\\d*(\\.\\d{1,2})?)', '数据类型错误，限时抢购价格只能输入数值,且不能超过2位小数'))
          appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtPrice',0.01,9999999, '输入的数值超出了系统表示范围')); 

         
      }
      $(document).ready(function() { InitValidators(); });       
  </script>     
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
<div class="areacolumn clearfix">
<div class="columnleft clearfix"><ul><li><a href="MyCountDowns.aspx"><span>限时抢购管理</span></a></li></ul></div>
      <div class="columnright">
          <div class="title title_height">
            <em><img src="../images/06.gif" width="32" height="32" /></em>
            <h1 class="title_line">修改限时抢购活动</h1>
          </div>          
          
      <div class="formitem validator5" style="padding-left:15px;">      
		<ul style="background:#FFFADF;border:1px solid #ccc;color:#f60;padding-top:15px;height:40px;">
				<table border="0" cellspacing="5" cellpadding="0" style="width:60%;" class=float">
                  <tr>
                    <td><span class="formitemtitle Pw_100">商品名称：</span></td>
                    <td><asp:TextBox ID="txtSearchText" runat="server" CssClass="forminput" /></td>
                    <td><abbr class="formselect">
						 <Hi:DistributorProductCategoriesDropDownList ID="dropCategories" runat="server" NullToDisplay="--请选择店铺分类--" />
					     </abbr></td>
                    <td ><span class="formitemtitle Pw_100" style="white-space:nowrap;">商家编码：</span></td>
                    <td><asp:TextBox ID="txtSKU" Width="110" runat="server" CssClass="forminput" /></td>
                    <td><input type="button" id="btnSearch" value="查询" onclick="ResetGroupBuyProducts()" class="searchbutton"/></td>
                  </tr>
                </table>
		 </ul>
        <ul>
        <li></li>
        <li><span class="formitemtitle Pw_140">限时抢购商品：</span>
			<abbr class="formselect">
						<Hi:DistributorGroupBuyProductDropDownList ID="dropGroupBuyProduct" runat="server" />
					</abbr>
					<p id="P1">限时抢购商品</p>
			</li>
          <li> <span class="formitemtitle Pw_140">结束日期：<em >*</em></span>
          <UI:WebCalendar runat="server" CssClass="forminput" ID="calendarEndDate" />
          <p id="P2">当达到结束日期时，活动会结束。</p>
          </li>
          <li><span class="formitemtitle Pw_140">限时抢购价格：<em >*</em></span>
            <asp:TextBox ID="txtPrice" runat="server" CssClass="forminput"></asp:TextBox>
           <p id="ctl00_contentHolder_txtPriceTip">限时抢购价格,不能为空。</p>
          </li>
         <li>
	       <span class="formitemtitle Pw_140">活动说明：</span>
		     <asp:TextBox ID="txtContent" runat="server" TextMode="MultiLine" Columns="50" Rows="5" CssClass="forminput"></asp:TextBox>
	            </li>
	          <li></li>        
      </ul>
      <ul class="btn Pa_100 clear">
      <li>
         <asp:Button ID="btnUpdateCountDown" runat="server" Text="修改" OnClientClick="return PageIsValid();"  CssClass="submit_DAqueding" OnClick="btnUpdateGroupBuy_Click"  />
         </li>
        </ul>
      </div>

      
  </div>
  </div>

</asp:Content>
