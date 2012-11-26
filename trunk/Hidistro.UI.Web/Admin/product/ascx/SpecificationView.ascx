<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SpecificationView.ascx.cs" Inherits="Hidistro.UI.Web.Admin.product.ascx.SpecificationView" %>
<%@ Import Namespace="Hidistro.Core"%>
 <%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
 <%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
   
      <div class="content">
	
            <UI:Grid ID="grdSKU" runat="server" ShowHeader="true" 
                AutoGenerateColumns="false" DataKeyNames="AttributeId" 
                HeaderStyle-CssClass="table_title" GridLines="None" Width="100%" 
                onrowcommand="grdSKU_RowCommand" onrowdatabound="grdSKU_RowDataBound" 
                onrowdeleting="grdSKU_RowDeleting">
            <Columns>
                    <asp:TemplateField HeaderText="规格名称" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="15%">
                        <ItemTemplate>
                            规格名[<asp:Literal runat="server" ID="litUseAttributeImage" Text='<%# Eval("UseAttributeImage") %>' />]：
		                    <Hi:HtmlDecodeTextBox ID="txtSKUName" runat="server" Text='<%# Eval("AttributeName") %>' Width="70px" />
		                    <asp:Literal ID="lblDisplaySequence" runat="server" Text='<%#Eval("DisplaySequence") %>' Visible=false></asp:Literal>
		                    <asp:LinkButton ID="lbtnAdd" Text="修改" runat="server" CommandName="saveSKUName" CommandArgument='<%# Container.DataItemIndex  %>' />
                        </ItemTemplate>
                    </asp:TemplateField>  
                    
                      <asp:TemplateField HeaderText="规格值"  HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="45%">
                        <ItemTemplate>
                            <asp:Repeater ID="rptSKUValue" runat="server" DataSource='<%# Eval("AttributeValues") %>'>
   		                               <ItemTemplate>
   		                           <span class="SKUValue">
   		                                <span class="span1"><Hi:SKUImage ID="SKUImage1" runat="server" CssClass="a_none" ImageUrl='<%# Eval("ImageUrl")%>' ValueStr='<%# Eval("ValueStr")%>' /></span>
                                        <span class="span2"><a href="#" onclick="deleteSKUValue(this, '<%# Eval("ValueId")%>', '<%# Eval("ImageUrl")%>');">删除</a></span>
                                    </span>
   		                        </ItemTemplate>
   		                    </asp:Repeater>
                        </ItemTemplate>
                    </asp:TemplateField>   
                                   
                    <UI:SortImageColumn HeaderText="排序"  ReadOnly="true" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="7%"/>
                     <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="25%">
                         <ItemStyle CssClass="spanD spanN" />
                         <ItemTemplate>
	                         <span class="submit_tiajia"><a href="#" onclick="ShowAddSKUValueDiv('<%# Eval("AttributeId") %>','<%# Eval("UseAttributeImage") %>', '<%# Eval("AttributeName") %>');">添加规格值</a></span>	                         	                        
 	                         <span class="submit_bianji"><asp:HyperLink  ID="lkbViewAttribute" runat="server" Text="编辑" NavigateUrl='<%# Globals.GetAdminAbsolutePath(string.Format("product/EditSpecificationValues.aspx?TypeId={0}&AttributeId={1}&UseAttributeImage={2}",Eval("TypeId"),Eval("AttributeId"),Eval("UseAttributeImage")))%>' ></asp:HyperLink></span> 
 	                         <span class="submit_dalata"><Hi:ImageLinkButton runat="server" ID="lbtnDelete" CommandName="delete" IsShow="true" DeleteMsg="当前操作将彻底删该除规格及下属的所有规格值，确定吗？" Text="删除" /></span>

                         </ItemTemplate>
                     </asp:TemplateField> 
                                     
            </Columns>
        </UI:Grid>
      </div>
        
        <div class="Pg_15">
          <input type="button" onclick="DivWindowOpen(480,280,'addSKU');" name="button" id="button" value="添加新规格" class="submit_bnt3"/>
        </div>
        
        <div class="Pop_up" id="addSKU" style="display:none;">
  <h1>添加新的规格 </h1>
    <div class="img_datala"><img src="../images/icon_dalata.gif" width="38" height="20" /></div>
    <div class="formitem">
    <ul>
              <li> 
                <span class="formitemtitle Pw_100">规格名称：<em >*</em></span>
                <asp:TextBox ID="txtName" CssClass="forminput" runat="server"></asp:TextBox>
                <p class="Pa_100" id="ctl00_contentHolder_specificationView_txtNameTip">规格名称长度在1至30个字符之间。</p>
              </li>
              <li> <span class="formitemtitle Pw_100">显示类型：</span>
                <Hi:UseAttributeImageRadioButtonList runat="server" ID="radIsImage" style="display:inline;" />
              </li>
        </ul>
        <ul class="up Pa_100">
       <asp:Button ID="btnCreate" runat="server" CssClass="submit_DAqueding" Text="确 定" 
                OnClientClick="return PageIsValid();" onclick="btnCreate_Click"/>
      </ul>
  </div>  
</div>
    
        <div class="Pop_up" id="addSKUValue" style="display:none;">
  <h1>添加&quot;<span id="spAttributeName"></span>&quot;的规格值 </h1>
    <div class="img_datala"><img src="../images/icon_dalata.gif" width="38" height="20" /></div>
  <div class="mianform">
        <ul id="valueStr">
          <li>
             <span class="formitemtitle Pw_100">规格值名：<em >*</em></span>
            <asp:TextBox ID="txtValueStr" CssClass="forminput" Width="300" runat="server"  onkeydown="javascript:this.value=this.value.replace('，',',')" />  
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
          <asp:Button ID="btnCreateValue" runat="server" Text="确 定"  
                CssClass="submit_DAqueding"  OnClientClick="return isFlagValue();" 
                onclick="btnCreateValue_Click"/>
        </ul>
  </div>
</div>

        <input runat="server" type="hidden" id="currentAttributeId" />


        <script type="text/javascript" language="javascript">
     //判断规格值
      function isFlagValue()
      {
          if (document.getElementById("valueStr").style.display == "block") {
              var skuValue = document.getElementById("ctl00_contentHolder_specificationView_txtValueStr").value.replace(/(^\s*)|(\s*$)/g,"");
              if (skuValue.length < 1 ) {
                  alert("请输入规格值");
                  return false;
              }
          }
          else {
              if (document.getElementById("ctl00_contentHolder_specificationView_fileUpload").value.length < 1) {
                  alert("请浏览图片");
                  return false;
              }
              var skuDec = document.getElementById("ctl00_contentHolder_specificationView_txtValueDec").value;
              if (skuDec.length < 1 || skuDec.length > 20) {
                  alert("请输入图片描述并控制在20个字符以内");
                  return false;
              }
          }
       return true;
     } 
  
     function ShowAddSKUValueDiv(attributeId, useAttributeImage, attributename) 
     {
        $("#spAttributeName").html(attributename);
        if(useAttributeImage == "True")
        {            
            document.getElementById("valueStr").style.display = "none";
            document.getElementById("valueImage").style.display = "block";            
        }
        else
        {
            document.getElementById("valueImage").style.display = "none";
            document.getElementById("valueStr").style.display = "block";
        }
            
        $("#ctl00_contentHolder_specificationView_currentAttributeId").val(attributeId);
        
        DivWindowOpen(480,250,'addSKUValue');
    }


    function deleteSKUValue(obj, valueId, imageUrl) {
        $.ajax({
            url: "AddSpecification.aspx",
            type: 'post', dataType: 'json', timeout: 10000,
            data: { ValueId: valueId, ImageUrl: imageUrl, isCallback: "true" },
            async: false,
            success: function(data) {
                if (data.Status == "true") 
                {
                    /*var _parentElement = obj.parentNode.parentNode;
                    if (_parentElement) {
                        var children = _parentElement.childNodes;
                        for (i = 0; i < children.length; i++) {
                            _parentElement.removeChild(children[i]);
                        }
                    }*/
                   location.reload();
                }
                else {
                    ShowMsg("此规格值有商品在使用，删除失败", false);
                }
            }
        });
    }
    
    function PreviewImg(imgFile)
    {        
        var newPreview = document.getElementById("newPreview");
        newPreview.filters.item("DXImageTransform.Microsoft.AlphaImageLoader").src = imgFile.value;
        newPreview.style.width = "28px";
        newPreview.style.height = "26px";
    }

</script>
