<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeFile="EditSpecificationValues.aspx.cs" Inherits="Hidistro.UI.Web.Admin.EditSpecificationValues" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
 <%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript">

    function UpdateAttributeValue(ValueId, ValueStr, ImageUrl, useAttributeImage) {

        if (useAttributeImage == "True") {
            document.getElementById("valueStr1").style.display = "none";
            document.getElementById("valueImage1").style.display = "block";
            
        }
        else {
            document.getElementById("valueImage1").style.display = "none";
            document.getElementById("valueStr1").style.display = "block";
        }

        $("#ctl00_contentHolder_txtValueStr1").val(ValueStr);
        $("#ctl00_contentHolder_txtValueDec1").val(ValueStr);
        

        $("#ctl00_contentHolder_currentAttributeId").val(ValueId);

        DivWindowOpen(480, 250, 'updateAttributeValue');

    }
     

     function ShowAddSKUValueDiv(attributeId, useAttributeImage) {
         if (useAttributeImage == "True") {
             document.getElementById("valueStr").style.display = "none";
             document.getElementById("valueImage").style.display = "block";
         }
         else {
             document.getElementById("valueImage").style.display = "none";
             document.getElementById("valueStr").style.display = "block";
         }

         $("#ctl00_contentHolder_currentAttributeId").val(attributeId);

         DivWindowOpen(480, 250, 'addAttributeValue');
     }
     
     String.prototype.trim = function(){
return this.replace(/^\s+|\s+$/g, "");//删除前后空格
}

     function isFlagValue() {
         if (document.getElementById("valueStr").style.display == "block") {
             var skuValue = document.getElementById("ctl00_contentHolder_txtValueStr").value;
             if (attributeValue.trim().length < 1) {
                 alert("请输入规格值");
                 return false;
             }
         }
         else {
             if (document.getElementById("ctl00_contentHolder_fileUpload").value.length < 1) {
                 alert("请浏览图片");
                 return false;
             }
             var skuDec = document.getElementById("ctl00_contentHolder_txtValueDec").value;
             if (skuDec.length < 1 || skuDec.length > 20) {
                 alert("请输入图片描述并控制在20个字符以内");
                 return false;
             }
         }
         return true;
     }

     function isFlagValue1() {
         if (document.getElementById("valueStr1").style.display == "block") {
             var skuValue = document.getElementById("ctl00_contentHolder_txtValueStr1").value;
             if (skuValue.length < 1 || skuValue.length > 15) {
                 alert("请输入规格值并控制在15个字符以内");
                 return false;
             }
         }
         else {
//             if (document.getElementById("ctl00_contentHolder_fileUpload1").value.length < 1) {
//                 alert("请浏览图片");
//                 return false;
//             }
             var skuDec = document.getElementById("ctl00_contentHolder_txtValueDec1").value;
             if (skuDec.length < 1 || skuDec.length > 20) {
                 alert("请输入图片描述并控制在20个字符以内");
                 return false;
             }
         }
         return true;
     }


     function PreviewImg(imgFile) {
         var newPreview = document.getElementById("newPreview");
         newPreview.filters.item("DXImageTransform.Microsoft.AlphaImageLoader").src = imgFile.value;
         newPreview.style.width = "28px";
         newPreview.style.height = "26px";
     }


     function PreviewImg1(imgFile) {
         var newPreview = document.getElementById("newPreview1");
         newPreview.filters.item("DXImageTransform.Microsoft.AlphaImageLoader").src = imgFile.value;
         newPreview.style.width = "28px";
         newPreview.style.height = "26px";
     }
     
     </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
<div class="areacolumn clearfix">
	<div class="columnleft clearfix">
      <ul>
            <li><a href='<%="EditSpecification.aspx?TypeId="+Page.Request.QueryString["TypeId"] %>'><span>规格管理</span></a></li>
      </ul>
    </div>
      <div class="columnright">
	 
    <div class="title"> <em><img src="../images/03.gif" width="32" height="32" /></em>
      <h1>编辑规格值 </h1>
    <span>编辑规格值</span></div>
    <!-- 添加按钮-->
    <div class="btn">
      <table width="500" border="0" cellspacing="0">
        <tr>
          <td width="200"><abbr class="formselect">
            <input type="button" name="button" id="button1" value="添加规格值" class="submit_bnt3 " onclick="ShowAddSKUValueDiv( '<%=Page.Request.QueryString["AttributeId"]%>','<%=Page.Request.QueryString["UseAttributeImage"]%>');"/>
          </abbr></td>
          <td>&nbsp;</td>
        </tr>
      </table>
    </div>
    <input type="hidden" id="hidvalueId" runat="server" />
      <input type="hidden" id="hidvalue" runat="server" />
     
    <!--结束-->
    <!--数据列表区域-->
    <div class="content">
    
     <div>
	              <UI:Grid ID="grdAttributeValues" runat="server" SortOrderBy="DisplaySequence" SortOrder="desc" ShowHeader="true" AutoGenerateColumns="false" DataKeyNames="ValueId" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
            <Columns>
                    <asp:TemplateField HeaderText="规格值" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="40%">
                        <ItemTemplate>
		                    <Hi:SKUImage ID="SKUImage1" runat="server" CssClass="a_none" ImageUrl='<%# Eval("ImageUrl")%>' ValueStr='<%# Eval("ValueStr")%>' />
		                      <asp:Literal ID="lblDisplaySequence" runat="server" Text='<%#Eval("DisplaySequence") %>' Visible=false></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>                    
                    <UI:SortImageColumn HeaderText="排序"  ReadOnly="true" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="15%"/>
                     <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="td_left td_right_fff" ItemStyle-Width="20%">
                         <ItemStyle CssClass="spanD spanN" />
                         <ItemTemplate>
	                         <span class="submit_shanchu">
	                         <asp:LinkButton ID="btnAdd" Text="删除" runat="server"   CommandName="dele" CommandArgument='<%#Eval("ImageUrl") %>' />   
	                        
	                         </span>
	                         <span> <a href="javascript:UpdateAttributeValue('<%#Eval("ValueId") %>','<%#Eval("ValueStr") %>','<%# Eval("ImageUrl")%>','<%=Page.Request.QueryString["UseAttributeImage"]%>');" >修改</a></span>
                         </ItemTemplate>
                     </asp:TemplateField> 
                                     
            </Columns>
        </UI:Grid>
	  </div>
	  </div>
	 </div>
        
<input runat="server" type="hidden" id="currentAttributeId" />
<div class="Pop_up" id="addAttributeValue" style="display:none;">
  <h1>添加规格值 </h1>
    <div class="img_datala"><img src="../images/icon_dalata.gif" width="38" height="20" /></div>
  <div class="mianform">
        <ul id="valueStr">
          <li>
             <span class="formitemtitle Pw_100">规格值名：<em >*</em></span>
            <asp:TextBox ID="txtValueStr" CssClass="forminput" Width="300" runat="server"  onkeydown="javascript:this.value=this.value.replace('，',',')"  />  
            <p class="Pa_100">多个规格值可用“,”号隔开，每个值的字符数最多15个字符</p>     
          </li>         
        </ul>       
      
      <ul id="valueImage">
              <li> <span class="formitemtitle Pw_100">图片地址：<em >*</em></span>
                <table width="400" border="0" align="left" cellspacing="0">
                  <tr>
                    <td width="322"><asp:FileUpload ID="fileUpload"  CssClass="input_longest" runat="server" Width="250px" onchange="PreviewImg(this)" /></td>
                    <td width="74"><div id="newPreview"></div></td>
                  </tr>
                </table>
                <p class="Pa_100 clearfix">仅接受jpg、gif、png、格式的图片</p>
            </li>
              <li><span class="formitemtitle Pw_100">图片描述：<em >*</em></span>
                <asp:TextBox ID="txtValueDec" CssClass="forminput" runat="server" />
                <p class="Pa_100">1到20个字符！</p>
            </li>
        </ul>
        <ul class="up Pa_100">
          <asp:Button ID="btnCreateValue" runat="server" Text="确 定"  CssClass="submit_DAqueding"  OnClientClick="return isFlagValue();"/>
        </ul>
  </div>
</div>

<div class="Pop_up" id="updateAttributeValue" style="display:none;">
  <h1>修改规格值 </h1>
    <div class="img_datala"><img src="../images/icon_dalata.gif" width="38" height="20" /></div>
  <div class="mianform">
        <ul id="valueStr1">
          <li>
             <span class="formitemtitle Pw_100">规格值名：<em >*</em></span>
            <asp:TextBox ID="txtValueStr1" CssClass="forminput" runat="server" />  
            <p class="Pa_100">规格值1到15个字符。</p>     
          </li>         
        </ul>       
      
      <ul id="valueImage1">
              <li> <span class="formitemtitle Pw_100">图片地址：<em >*</em></span>
                <table width="400" border="0" align="left" cellspacing="0">
                  <tr>
                    <td width="322"><asp:FileUpload ID="fileUpload1"  CssClass="input_longest" runat="server" Width="250px" onchange="PreviewImg1(this)" /></td>
                    <td width="74"><div id="newPreview1"></div></td>
                  </tr>
                </table>
                <p class="Pa_100 clearfix">仅接受jpg、gif、png、格式的图片</p>
            </li>
              <li><span class="formitemtitle Pw_100">图片描述：<em >*</em></span>
                <asp:TextBox ID="txtValueDec1" CssClass="forminput" runat="server" />
                <p class="Pa_100">1到20个字符！</p>
            </li>
        </ul>
        <ul class="up Pa_100">
          <asp:Button ID="btnUpdate" runat="server" Text="确 定"  CssClass="submit_DAqueding"  OnClientClick="return isFlagValue1();"/>
        </ul>
  </div>
</div>

</div>
</asp:Content>

