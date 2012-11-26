<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AttributeView.ascx.cs"
    Inherits="Hidistro.UI.Web.Admin.product.ascx.AttributeView" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<div class="content">
    <UI:Grid ID="grdAttribute" runat="server" ShowHeader="true" AutoGenerateColumns="false" DataKeyNames="AttributeId" HeaderStyle-CssClass="table_title" GridLines="None"
        Width="100%">
        <Columns>
            <asp:TemplateField HeaderText="属性名称" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="15%">
                <ItemTemplate>
                    <Hi:HtmlDecodeTextBox ID="txtAttributeName" runat="server" Text='<%# Eval("AttributeName") %>'
                        Width="70px"></Hi:HtmlDecodeTextBox>
                    <asp:Literal ID="lblDisplaySequence" runat="server" Text='<%#Eval("DisplaySequence") %>'
                        Visible="false"></asp:Literal>
                    <asp:LinkButton ID="lbtnSave" Text="修改" runat="server" CommandName="saveAttributeName" />
                </ItemTemplate>
            </asp:TemplateField>
            <UI:YesNoImageColumn DataField="IsMultiView"  ItemStyle-Width="9%" HeaderText="支持多选" HeaderStyle-CssClass="td_right td_left" />
            <asp:TemplateField HeaderText="属性值" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="45%">
                <ItemTemplate>
                    <asp:Repeater ID="rptSKUValue" runat="server" DataSource='<%# Eval("AttributeValues") %>'>
                        <ItemTemplate>
                            <span class="SKUValue"><span class="span1">
                                <asp:HyperLink ID="HyperLink1" runat="server"><%# Eval("ValueStr")%></asp:HyperLink></span>
                                <span class="span2"><a href="javascript:deleteAttributeValue(this,'<%# Eval("ValueId")%>');">
                                    删除</a></span> </span>
                        </ItemTemplate>
                    </asp:Repeater>
                </ItemTemplate>
            </asp:TemplateField>
            <UI:SortImageColumn HeaderText="排序" ReadOnly="true" HeaderStyle-CssClass="td_right td_left"
                ItemStyle-Width="7%" />
            <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="25%">
                <ItemStyle CssClass="spanD spanN" />
                <ItemTemplate>
                    <span class="submit_tiajia"><a href="#" onclick="ShowAddSKUValueDiv('<%# Eval("AttributeId") %>','<%# Eval("AttributeName") %>');">
                        添加属性值</a></span> <span class="submit_bianji">
                            <asp:HyperLink ID="lkbViewAttribute" runat="server" Text="编辑" NavigateUrl='<%#Globals.GetAdminAbsolutePath(string.Format("product/EditAttributeValues.aspx?TypeId={0}&AttributeId={1}",Eval("TypeId"),Eval("AttributeId")))%>'></asp:HyperLink></span>
                    <span class="submit_shanchu">
                        <Hi:ImageLinkButton ID="lkbDelete" CssClass="SmallCommonTextButton" runat="server"
                            IsShow="true" CommandName="Delete" Text="删除" /></span>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </UI:Grid>
</div>
<div class="Pg_15">
    <input type="button" name="button" id="button" value="添加扩展属性" class="submit_bnt3 "
        onclick="DivWindowOpen(500,350,'addAttribute');" />
</div>
<div class="Pop_up" id="addAttribute" style="display: none;">
    <h1>
        添加扩展属性
    </h1>
    <div class="img_datala">
        <img src="../images/icon_dalata.gif" width="38" height="20" /></div>
    <div class="mianform ">
    <table style="width:100%;font-size:14px;">
    <tr>
        <td width="30%" style="text-align:right; vertical-align:top;">属性名称：<em>*</em></td><td> <asp:TextBox ID="txtName" CssClass="forminput" runat="server"></asp:TextBox>
                <p id="ctl00_contentHolder_attributeView_txtNameTip">
                    扩展属性的名称，最多15个字符。</p>
        </td>
    </tr>
    <tr>
        <td style="text-align:right;vertical-align:top;">是否支持多选：</td><td><asp:CheckBox ID="chkMulti" Text="支持多选"  runat="server" />
                <p id="P1">
                    有些属性是可以选择多个属性值的，如“适合人群”，就可能既适合老年人也适合中年人。</p>
        </td>
    </tr>
    <tr>
        <td style="text-align:right; vertical-align:top;">属性值：</td><td><asp:TextBox ID="txtValues" runat="server" Width="300" CssClass="forminput"  onkeydown="javascript:this.value=this.value.replace('，',',')" ></asp:TextBox>
                <p>
                    扩展属性的值，多个属性值可用“,”号隔开，每个值的字符数最多15个字符。</p>
        </td>
    </tr>
    <tr><td colspan="2" style="text-align:center;"><asp:Button ID="btnCreate" runat="server" Text="添加" OnClientClick="return PageIsValid();"
                CssClass="submit_DAqueding" />
                </td></tr>
    </table>
       <%-- <ul>
            <li><span class="formitemtitle Pw_100">属性名称：<em>*</em></span>
               
            </li>
            <li><span class="formitemtitle Pw_100">是否支持多选：</span>
               
            </li>
            <li><span class="formitemtitle Pw_100">属性值：</span>
               
            </li>
        </ul>--%>
   <%--     <ul class="up Pa_100">
            
        </ul>--%>
    </div>
</div>
<div class="Pop_up" id="addAttributeValue" style="display: none;">
    <h1>
        添加&quot;<span id="spAttributeName"></span>&quot;的属性值
    </h1>
    <div class="img_datala">
        <img src="../images/icon_dalata.gif" width="38" height="20" /></div>
    <div class="mianform">
        <ul id="valueStr">
            <li><span class="formitemtitle Pw_100">属性值名：<em>*</em></span>
                <asp:TextBox ID="txtValueStr" CssClass="forminput" Width="300" runat="server" onkeydown="javascript:this.value=this.value.replace('，',',')" />
                <p class="Pa_100">
                    多个规格值可用“,”号隔开，每个值的字符数最多15个字符</p>
            </li>
        </ul>
        <ul class="up Pa_100">
            <asp:Button ID="btnCreateValue" runat="server" Text="确 定" CssClass="submit_DAqueding"
                OnClientClick="return isFlagValue();" />
        </ul>
    </div>
</div>
<input runat="server" type="hidden" id="currentAttributeId" />

<script type="text/javascript" language="javascript">
//判断规格值
String.prototype.trim = function(){
return this.replace(/^\s+|\s+$/g, "");//删除前后空格
}

      function isFlagValue()
      {
              var attributeValue = document.getElementById("ctl00_contentHolder_attributeView_txtValueStr").value;

              if (attributeValue.trim().length < 1 )
              {
                  alert("请输入属性值");
                  return false;
          }        
       return true;
     } 

function ShowAddSKUValueDiv(attributeId,attributename) 
     {
        $("#spAttributeName").html(attributename);
        $("#ctl00_contentHolder_attributeView_currentAttributeId").val(attributeId);
        DivWindowOpen(480,250,'addAttributeValue');
    }
    
        function deleteAttributeValue(obj, valueId) {
        $.ajax({
            url: "AddSpecification.aspx",
            type: 'post', dataType: 'json', timeout: 10000,
            data: { ValueId: valueId,isCallback: "true" },
            async: false,
            success: function(data)
            {
                if (data.Status == "true")
                {
                  /* var _parentElement = obj.parentNode.parentNode;
                    if (_parentElement)
                    {
                       var children = _parentElement.childNodes;
                       for (i = 0; i < children.length; i++) {
                           _parentElement.removeChild(children[i]);
                       }
                       alert("111");
                    }*/
                    location.reload();                    
                }
                else {
                    ShowMsg("此属性值有商品在使用，删除失败", false);
                }
            }
        });
    }
</script>

