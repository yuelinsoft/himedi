<%@ Page Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeFile="TaobaoSetting.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.TaobaoSetting" Title="无标题页" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" language="javascript">
    function InitValidators()
    {
        initValid(new InputValidator('ctl00_contentHolder_txtTopkey', 8, 8, true, null, '淘宝Appkey不能为空，为8位数字ID'))
        initValid(new InputValidator('ctl00_contentHolder_txtTopSecret', 32, 32, true, null, '淘宝AppSecret格式不正确,为32位字符'))
    }
    $(document).ready(function() { InitValidators(); });
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth databody">
      <div class="title td_bottom"> <em><img src="images/01.gif" width="32" height="32" /></em>
        <h1 class="title_line" style="line-height:38px;">淘宝同步设置</h1>
         <div class="clear"></div>
        <span > 首先进入<a href="http://my.open.taobao.com/app/app_list.htm" target="_blank">淘宝开放平台的应用列表从</a>选择一个应用并读取其App Key和App Secret
        <br />如果您还没有创建任何应用，则<a href="http://my.open.taobao.com/common/applyIsv.htm" target="_blank">创建应用</a>。创建应用后即可获得App Key和App Secret</span>
        <br /> <span style="color:Red;"> 注意在选择要绑定的应用回调页面URL一定要写成：<asp:Literal runat="server" ID="litReturnUrl" /></span>
      </div>
     
      <div class="datafrom">
      <asp:Literal runat="server" ID="litshowmsg"></asp:Literal>
        <div class="formitem validator1" id="settaobao" runat="server">
          <ul>
            <li><span class="formitemtitle Pw_198">淘宝App key：<em >*</em></span>
              <asp:TextBox ID="txtTopkey" CssClass="forminput" runat="server"  />
              <p id="txtTopkeyTip" runat="server">淘宝Appkey不能为空，为8位数字ID</p>
            </li>
            <li><span class="formitemtitle Pw_198">淘宝App Secret：<em >*</em></span>
              <asp:TextBox ID="txtTopSecret" CssClass="forminput" Width="300px" runat="server"  />
              <p id="txtTopSecretTip" runat="server">淘宝AppSecret不能为空,为32位字符</p>
            </li>
            </ul>
           <ul class="btntf Pa_198 clearfix">
		    <asp:Button ID="btnOK" runat="server" Text="绑定淘宝" CssClass="submit_DAqueding inbnt" OnClick="btnOK_Click" OnClientClick="return PageIsValid();" />
			</ul>
        </div>
      </div>
           <div class="clear"></div>
</div>

</asp:Content>
