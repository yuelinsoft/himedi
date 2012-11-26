<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeFile="EditExpressTemplate.aspx.cs" Inherits="Hidistro.UI.Web.Admin.EditExpressTemplate" Title="无标题页" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
<script type="text/javascript" src="../js/ExpressFlex.js" ></script>
<style type="text/css">.tdline{	border-right:1px #ccc solid;}</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="areacolumn clearfix">
		<table width="897" id="controls" height="56" border="0" cellpadding="0" cellspacing="0" style="text-align:center; border:1px #CCC solid;  background-color:#E7E7E7; font-size:12px; padding:0px;
margin:0px 0px 5px 0px;">
  <tr>
    <td height="24" colspan="2" class="tdline">单据名称</td>
    <td colspan="2" class="tdline">单据尺寸</td>
    <td colspan="2" class="tdline">单据背景图</td>
    <td colspan="2" class="tdline">单据打印项</td>
    <td width="73" class="tdline"><form name="form0" style=" padding:8px 0px 0px 0px">
      <label>
       <select 
			    name="fsize" 
				onChange="fontsize();return false;"
				size="1">
				<option value="10">大小
			    <option value="10">10
			    <option value="12">12
			    <option value="14">14
			    <option value="18">18
				<option value="20">20
				<option value="24">24
				<option value="30">30
				<option value="36">36
			</select>
      </label>
    </form></td>
    <td width="146"><form name="form1" style=" padding:8px 0px 0px 0px">
      <label><select 
			    name="ffamily" 
				onChange="fontfamily();return false;"
				size="1">
				<option value="Arial">字体
			    <option value="Times New Roman">Times New Roman
				<option value="Arial">Arial
				<option value="Courier New">Courier New
			</select></label>
    </form></td>
  </tr>
  <tr>
    <td width="108" height="30"><form name="form2">
      <label><input name="emsname" type="text" id="emsname" size="12"/></label>
    </form></td>
    <td width="42" class="tdline"><form name="form3">
      <label><button name="btndata" onClick="getData();" type="button">保存
			</button></label>
    </form></td>
    <td width="112"><form name="form4">
      <label>宽:<input name="swidth" type="text" id="widths" size="4" value="228" onchange="setfsize()"/>mm</label>
    </form></td>
    <td width="120" class="tdline"><form name="form5">
      <label>*高:<input name="sheight" type="text" id="heights" size="4" value="127" onchange="setfsize()" />mm</label>
    </form></td>
    <td width="49"><form name="form6">
      <label><button name="btnclick"
			onClick ="clickbtn();return false;">上传
			</button></label>
    </form></td>
    <td width="48" class="tdline"><form name="form7">
      <label><button name="btnimage"
			onClick ="imagebtn();return false;">删除
			</button></label>
    </form></td>
    <td width="125"><form name="form8">
      <label>
        <select 
			    name="item" 
				onChange="addbtn(i);tt();return false;"
				size="1">
				<option value="收货人-姓名">添加打印项
			    <option value="收货人-姓名">收货人-姓名
				<option value="收货人-地址">收货人-地址
				<option value="收货人-电话">收货人-电话
				<option value="收货人-邮编">收货人-邮编	
				<option value="收货人-手机">收货人-手机	
				<option value="收货人-地区1级">收货人-地区1级
				<option value="收货人-地区2级">收货人-地区2级
				<option value="收货人-地区3级">收货人-地区3级	
				
				<option value="发货人-姓名">发货人-姓名
				<option value="发货人-地区1级">发货人-地区1级
				<option value="发货人-地区2级">发货人-地区2级
				<option value="发货人-地区3级">发货人-地区3级										
				<option value="发货人-地址">发货人-地址
				<option value="发货人-电话">发货人-电话
				<option value="发货人-邮编">发货人-邮编
				<option value="发货人-手机">发货人-手机
				
				<option value="订单-订单号">订单-订单号
				<option value="订单-总金额">订单-总金额
				<option value="订单-物品总重量">订单-物品总重量
				<option value="订单-备注">订单-备注
				<option value="订单-详情">订单-详情
				<option value="订单-送货时间">订单-送货时间
				<option value="网店名称">网店名称
				<option value="√">对号-√
				<option value="自定义内容">自定义内容
			</select></label>
    </form>
    </td>
    <td width="54" class="tdline"><form name="form9">
      <label><button name="btnitem" onClick="delData()"  type="button">删除
			</button></label>
    </form></td>
    <td class="tdline"><form name="form10">
      <label><select 
			    name="style" 
				onChange="showstyle();return false;"
				size="1">
				<option value="normal">样式
			    <option value="italic">斜体
				<option value="normal">普通
				<option value="bold">粗体
			</select></label>
    </form></td>
    <td><form name="form11">
      <label><select 
			    name="bottomDropDown" 
				onchange="updateBottom();return false;"
				size="1">
      <option value="left">对齐方式 </option>
      <option value="left">居左 </option>
      <option value="center">居中 </option>
      <option value="right">居右 </option>
    </select></label>
    </form></td>
  </tr>
</table>
		<div id="writeroot"></div>
		
		<div id="flashoutput">
			<noscript>
				<object id="flexApp" classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=8,5,0,0" height="600" width="900">
				<param name="flashvars" value="bridgeName=example"/>
				<param name="src" value="../../Storage/master/flex/EditExpressTemplate.swf"/>
				<embed name="flexApp" pluginspage="http://www.macromedia.com/go/getflashplayer" src="../../Storage/master/flex/EditExpressTemplate.swf" height="600" width="900" flashvars="bridgeName=example"/>
				</object>
			</noscript>
			<script language="javascript" charset="utf-8">
				document.write('<object id="flexApp" classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=8,5,0,0" height="600" width="900">');
				document.write('<param name="flashvars" value="bridgeName=example"/>');
				document.write('<param name="src" value="../../Storage/master/flex/EditExpressTemplate.swf"/>');
				document.write('<embed name="flexApp" pluginspage="http://www.macromedia.com/go/getflashplayer" src="../../Storage/master/flex/EditExpressTemplate.swf" height="600" width="900" flashvars="bridgeName=example"/>');
				document.write('</object>');
			</script>
		</div>
  </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript">
	var i=0;  
	function tt()
	{
		i++;
		//document.getElementById("Button2").value=i;
	}
	function InitExpressName()
	{
	    var url = location.href;
	    $("#emsname").val(decodeURI(url.split("=")[2].split("&")[0]));
	}
	$(document).ready(function(){ InitExpressName(); });
</script>
</asp:Content>
