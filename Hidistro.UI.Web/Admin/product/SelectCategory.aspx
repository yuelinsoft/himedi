<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeFile="SelectCategory.aspx.cs" Inherits="Hidistro.UI.Web.Admin.SelectCategory" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <link id="cssLink" rel="stylesheet" href="../css/style.css" type="text/css" media="screen" />
    <style type="text/css">
        .results_pos{position:relative;overflow:hidden;background:red;float:left;width:891px;background:#e5f0ff;border:1px solid #c7deff;border-left:0;height:298px;}
        .results_ol{position:absolute;display:block;overflow:hidden;width:2250px;clear:both;left:0px;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth td_top_ccc">
<div class="advanceSearchArea clearfix"></div>
<div class="toptitle"> <em><img src="../images/03.gif" width="32" height="32" /></em>
  <h1 class="title_height">选择店铺分类</h1>
</div>
  <div class="search_results">
  <!--
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
	        <tr>
	          <td width="24%" align="right" class="fonts">类目搜索：</td>
	          <td width="46%"><input name="input" type="text" class="forminput" size="70" /></td>
	          <td width="30%"><input type="button" value="搜索" class="searchbutton" /></td>
            </tr>
	        <tr>
	          <td align="right">&nbsp;</td>
	          <td width="46%" align="left" class="colorR">请输入商品名称或类目属性名称，快速找到正确类目，数码产品输入型号即可</td>
	          <td>&nbsp;</td>
            </tr>
    </table>
    -->
  </div>

      
    <div class="results">
        <div class="results_main">
            <div class="results_left">
                <label>
                    <input type="button" name="button2" id="button2" value="" class="search_left" />
                </label>
            </div>
            <div class="results_pos">
                <ol class="results_ol">
                </ol>
            </div>
            <div class="results_right">
                <label>
                    <input type="button" name="button2" id="button2" value="" class="search_right" />
                </label>
            </div>
        </div>
    </div>
    <div class="results_img"></div>
    <div class="results_bottom">
        <span class="spanE">你当前选择的是：</span>
        <span id="fullName"></span>
    </div>
 </div>   
 <div class="databottom"></div>
	<div class="bntto">
	  <input type="submit" name="button2" id="btnNext" value="下一步" class="submit_DAqueding"/>
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" src="publish.helper.js"></script>
</asp:Content>
