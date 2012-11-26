<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchasePrintData.aspx.cs" Inherits="Hidistro.UI.Web.Admin.PurchasePrintData" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
<meta http-equiv="content-type" content="text/html; charset=UTF-8" />
<meta http-equiv="content-language" content="zh-CN" />
<title>打印快递单 - 采购单</title>
 <Hi:Style ID="Style1" Href="/admin/css/css.css" runat="server" Media="screen" />
    <Hi:Style ID="Style4" Href="/admin/css/windows.css" runat="server" Media="screen" />    
 <Hi:Script ID="Script1" runat="server" Src="/utility/jquery-1.3.2.min.js"/>
<Hi:Script ID="Script4" runat="server" Src="/utility/windows.js"/>
</head>
<body>
<span id="SpanVildateMsg"></span>
<form id="form1" runat="server">
<div class="dataarea mainwidth databody">
   <div class="title  m_none td_bottom" style="padding:5px 0px 5px 12px"> 
      <table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td> <em><img src="../images/04.gif" width="32" height="32" /></em></td>
        <td width="100%"><h1>收货人信息</h1></td>
      </tr>
    </table> 
  </div> 
    <div class="datafrom" style="padding-bottom:1px;">
      <table width="100%" border="0" cellspacing="1" cellpadding="0" class="PrintDataTable">
  <tr>
    <th><span>收货人姓名：<em>*</em></span></th>
    <td><asp:TextBox runat="server" ID="txtShipTo" CssClass="forminput" /></td>
    <th class="leftb"><span>E-Mail：</span></th>
    <td><asp:TextBox runat="server" ID="txtEmail"  CssClass="forminput" /></td>
  </tr>
  <tr>
    <th><span>省区：<em>*</em></span></th>
    <td colspan="3"><Hi:RegionSelector ProvinceTitle="" runat="server" id="dropRegions" /></td>
    </tr>
  <tr>
    <th width="15%"><span>详细地址：<em>*</em></span></th>
    <td width="35%"><asp:TextBox runat="server" ID="txtAddress"  CssClass="forminput" Width="280"/></td>
    <th width="15%" class="leftb"><span>邮　编：</span></th>
    <td width="35%"><asp:TextBox runat="server" ID="txtZipcode"  CssClass="forminput" /></td>
  </tr>
  <tr>
    <th><span>联系电话：</span></th>
    <td><asp:TextBox runat="server" ID="txtTelphone"  CssClass="forminput"/></td>
    <th class="leftb"><span>手　机：</span></th>
    <td><asp:TextBox runat="server" ID="txtCellphone" CssClass="forminput"/></td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td colspan="3">
      <a class="linkSub float">
       <asp:Button ID="btnUpdateAddrdss" runat="server"  OnClientClick="return DoValid();"  Text="修改采购单地址"/>
      </a>
      <span class="fonts colorB">(您可以将编辑过的收货人信息更新到采购单)</span>
    </td>
    </tr>
</table>
    </div>
  <div class="title  m_none td_bottom" style="padding:5px 0px 5px 12px"> 
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td><em><img src="../images/04.gif" width="32" height="32" /></em></td>
        <td width="100%"><h1>发货人信息</h1></td>
      </tr>
    </table>   
  </div> 
  <div class="datafrom" style="padding-bottom:1px;">
   <asp:Panel runat="server" ID="pnlSender">
   <table width="100%" border="0" cellspacing="1" cellpadding="0" class="PrintDataTable">
   <tr>
    <th><span>发货点选择：</span></th>
    <td colspan="3"><Hi:ShippersDropDownList runat="server" IncludeDistributor="true" ID="ddlShoperTag" /></td>
    </tr>
  <tr>
    <th width="15%"><span>发件人姓名：</span></th>
    <td width="35%"><asp:Literal runat="server" ID="litShipperName" /></td>
    <th width="15%" class="leftb"><span>地区：</span></th>
    <td width="35%"><asp:Literal runat="server" ID="litRegion" /></td>
  </tr>
  <tr>
    <th><span>邮　编：</span></th>
    <td><asp:Literal runat="server" ID="litShipperZipcode" /></td>
    <th class="leftb">详细地址：<span></span></th>
    <td><asp:Literal runat="server" ID="litShipperAddress" /></td>
  </tr>
  <tr>
    <th><span>手　机：</span></th>
    <td><asp:Literal runat="server" ID="litShipperCellphone" /></td>
    <th class="leftb"><span>联系电话：</span></th>
    <td><asp:Literal runat="server" ID="litShipperTelphone" /></td>
  </tr>
</table>
</asp:Panel>
<asp:Panel runat="server" ID="pnlEmptySender">
    <span><a href="AddShipper.aspx">您还没有添加发货人信息，请先点击这里添加发货人信息!</a></span>
</asp:Panel>
    </div>    
   <div class="title  m_none td_bottom" style="padding:5px 0px 5px 12px"> 
      <table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td><em><img height="32" width="32" src="../images/10.gif"></em></td>
        <td width="100%"><h1>选择快递单模板</h1></td>
      </tr>
    </table> 
  </div> 
  <div class="datafrom" style="padding-bottom:1px;">
    <asp:Panel runat="server" ID="pnlTemplates">
    <table width="100%" border="0" cellspacing="1" cellpadding="0" class="PrintDataTable">
   <tr>
    <th width="15%"><span>客户所选配送方式：</span></th>
    <td colspan="3"><asp:Literal runat="server" ID="litModeName" /></td>
    </tr>
   <tr>
    <th width="15%"><span>选择模版：<em>*</em></span></th>
    <td colspan="3">
     <div class="selectTem">
      <ul class="ul1">
     <asp:DropDownList runat="server" ID="ddlTemplates"></asp:DropDownList>
     </ul>
     <ul class="ul2">   
     <asp:Button runat="server" ID="btnPrint" CssClass="printSub" OnClientClick="return DoPrint();" />
     </ul>
     </div>
      </td>
    </tr>
    </table>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlEmptyTemplates">
        <span><a href="AddExpressTemplate.aspx">您还没有添加快递单模板，请先点击这里添加快递单模板!</a></span>
    </asp:Panel>
  </div>
</div>
    </form>
    <script language="javascript" type="text/javascript">
        function DoValid() {
            // 检查收货人姓名
            var v = $("#txtShipTo").val();
            var len = v.length;
            var exp = new RegExp("^[\u4e00-\u9fa5a-zA-Z]+[\u4e00-\u9fa5_a-zA-Z0-9]*$", "i");

            if (len < 2 || len > 20 || !exp.test(v)) {
                alert("请填写收货人姓名，以汉字或字母开头，长度在2-20个字符之间");
                return false;
            }

            // 检查电子邮件
            v = $("#txtEmail").val();
            len = v.length;
            exp = new RegExp("[\\w-]+(\\.[\\w-]+)*@[\\w-]+(\.[\\w-]+)+", "i");
            if ((len > 0) && (len < 3 || len > 200 || !exp.test(v))) {
                alert("请输入有效的电子邮件地址，电子邮件地址的长度在200个字符以内");
                return false;
            }

            // 检查省区选择
            if ($("#regionSelectorValue").val() == "") {
                alert("请选择收货人所在的地区");
                return false;
            }

            // 检查详细地址
            v = $("#txtAddress").val();
            len = v.length;
            if (len < 1 || len > 200) {
                alert("详细地址不能为空，长度限制在200个字符以内");
                return false;
            }


         

            // 检查电话
            v = $("#txtTelphone").val();
            len = v.length;
            if ((len > 0) && (len < 3 || len > 20)) {
                alert("收货人的电话号码(区号-电话号码-分机)，限制在3-20字符");
                return false;
            }

            // 检查手机
            v = $("#txtCellphone").val();
            len = v.length;
            if ((len > 0) && (len < 3 || len > 20)) {
                alert("收货人的手机号码，限制在3-20字符");
                return false;
            }

            // 电话和手机二者必填一
            if ($("#txtTelphone").val().length == 0 && $("#txtCellphone").val().length == 0) {
                alert("电话号码和手机号码必须填写其中一项");
                return false;
            }

            return true;
        }

        function DoPrint() {
            if (DoValid()) {
                if ($("#ddlTemplates").val().length == 0) {
                    alert("请选择一个快递单模板");
                    return false;
                }
                return true;
            }
            return false;
        }
 </script>
</body>
</html>