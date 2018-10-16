<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AttributeView.ascx.cs"
    Inherits="Hidistro.UI.Web.Admin.product.ascx.AttributeView" %>
<%@ Import Namespace="Hidistro.Core" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<div class="content">
    <input type="hidden" id="hidProductTypeId" name="hidProductTypeId" value="<%=typeId %>" />
    <table class="table table-striped">
        <thead>
            <tr>
                <th style="width: 20%;">属性名称</th>
                <th style="width: 10%;">支持多选</th>
                <th>属性值</th>
                <th style="width: 10%;">排序</th>
                <th style="width: 15%;">操作</th>
            </tr>
        </thead>
        <tbody id="datashow">
        </tbody>
    </table>
    <!--S Data Template-->
    <script id="datatmpl" type="text/html">
        {{each rows as item index}}                  
                    <tr>
                        <td>
                            <input type="text" value="{{item.AttributeName}} " class="form-control forminput ipt_AttrName" style="width:70px;">
                            <a href="###" style="line-height: 32px; margin-left: 10px;" class="btn_edit" onclick="Post_EditName('{{item.AttributeId}}',this)">修改</a>
                        </td>
                        <td><a href="###" onclick="Post_Multi('{{item.AttributeId}}')">{{if item.IsMultiView}}
                         <img src="../images/allright.gif"/>
                            {{else}}
                           <img src="../images/wrong.gif"/>
                            {{/if}}
                            </a>
                        </td>
                        <td>{{each item.AttributeValues as model index}}
                             <span class="SKUValue">
                                 <span class="span1" style="margin-right: 0px;">
                                     <a href="javascript:void(0)">{{model.ValueStr}}</a>
                                 </span>
                                 <span class="span2" style="margin-right: 0px;">
                                     <a href="javascript:void(0)" onclick="Post_DeleteValue('{{model.ValueId}}');">删除</a></span>
                             </span>
                            {{/each}}
                        </td>
                        <td>
                            <input type="hidden" value="{{item.AttributeId}}" id="DisplaySequence_{{index}}" />
                            <a href="###" class="sort-fall" onclick="SetOrder('Fall', '{{item.DisplaySequence}}', '{{item.AttributeId}}', '{{index}}')" ><img src="../images/fall.gif" border="0"></a>
                            <a href="###" class="sort-rise" onclick="SetOrder('Rise', '{{item.DisplaySequence}}', '{{item.AttributeId}}', '{{index}}')"><img src="../images/rise.gif" border="0"></a>
                        </td>
                        <td>
                            <div class="operation">

                                <span class="submit_tiajia"><a href="javascript:void(0)" onclick="ShowAddValueDiv('{{item.AttributeId}}', '{{item.AttributeName}}');">添加属性值</a></span>
                                <span class="submit_bianji"><a href="EditSpecificationValues.aspx?TypeId={{item.TypeId}}&AttributeId={{item.AttributeId}}&UseAttributeImage={{item.UseAttributeImage}}">编辑</a></span>
                                <span class="submit_dalata">
                                    <a href="javascript:Post_Deletes({{item.AttributeId}})">删除</a>
                                </span>
                            </div>
                        </td>
                    </tr>
        {{/each}}
    </script>
    <!--E Data Template-->
</div>
<div class="Pg_15">
    <a name="button" id="button" class="btn btn-primary "
        onclick="ShowAddKzAtribute();">添加扩展属性</a>
</div>

<%--添加扩展属性--%>
<div id="addAttribute" style="display: none;">
    <div class="frame-content">
        <p><span class="frame-span frame-input90">属性名称：<em>*</em></span><asp:TextBox ID="txtName" CssClass="form-control forminput" runat="server"></asp:TextBox></p>
        <b id="ctl00_contentHolder_attributeView_txtNameTip">扩展属性的名称，最多15个字符，不允许包含脚本标签和\(反斜杠)。</b>
        <span class="frame-span frame-input90">是否支持多选：</span>
        <asp:CheckBox ID="chkMulti_copy" Text="支持多选" runat="server" onclick="javascript:SetMultSate(this)" Checked="true" />
        <%--<Hi:OnOff runat="server" ID="chkMulti_copy" onclick="javascript:SetMultSate(this)" SelectedValue="true" ></Hi:OnOff>--%>

        <p><span class="frame-span frame-input90">属性值：<em>*</em></span><asp:TextBox ID="txtValues" runat="server" Width="300" CssClass="form-control forminput"></asp:TextBox></p>
        <b>扩展属性的值，多个属性值可用“,”号隔开，每个值的字符数最多15个字符，不允许包含脚本标签和\(反斜杠)，系统会自动过滤</b>
    </div>
</div>

<%--添加属性值--%>
<div id="addAttributeValue" style="display: none;">
    <div class="frame-content">
        <p><span class="frame-span frame-input90">属性名称：<em>*</em></span>
            <input type="hidden" class="hid_attrid" />
            <input type="text" class="forminput ipt_attrvalue" style="width:300px;">
        </p>
        <b>多个规格值可用“,”号隔开，每个值的字符数最多15个字符，不允许包含脚本标签和\(反斜杠)，系统会自动过滤</b>
    </div>
</div>
<div style="display: none">
    <input type="submit" name="btnCreateValue" value="添加属性值" id="btnCreateValue" class="submit_DAqueding" />
    <asp:Button ID="btnCreate" runat="server" Text="添加扩展属性" CssClass="submit_DAqueding" />
    <asp:CheckBox ID="chkMulti" Text="支持多选" runat="server" Checked="true" />
    <input runat="server" type="hidden" id="currentAttributeId" />
</div>

<input type="hidden" name="dataurl" id="dataurl" value="/admin/product/ashx/AttributeView.ashx" />
<script src="/Utility/artTemplate.js" type="text/javascript"></script>
<script src="/Utility/QW.Repeater.js" type="text/javascript"></script>
<script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
<script src="/admin/product/scripts/AttributeView.js" type="text/javascript"></script>

<script type="text/javascript" language="javascript">

    function SetMultSate(multiobj) {
        if (multiobj.checked) {
            $("#ctl00_contentHolder_attributeView_chkMulti").attr("checked", true);
        } else {
            $("#ctl00_contentHolder_attributeView_chkMulti").attr("checked", false);
        }
    }
    //判断规格值
    String.prototype.trim = function () {
        return this.replace(/^\s+|\s+$/g, "");//删除前后空格
    }


    var formtype = "";
    function ShowAddValueDiv(id, attributename) {
        formtype = "addvalue";
        arrytext = null;
        var dlg = art.dialog({
            title: "添加" + attributename + "的属性值",
            content: "<div id='addvaluebox'></div>",
            lock: true,
            button: [{
                name: '保存',
                callback: function () {
                    var addvaluebox = $("#addvaluebox");
                    var attrid = $(".hid_attrid", addvaluebox).val();
                    var attrvalue = $(".ipt_attrvalue", addvaluebox).val();
                    if (attrvalue.length < 1) {
                        alert("请填写值");
                        return;
                    }
                    var pdata = {
                        id: attrid, typid: typeId, contents: attrvalue, action: "AddValue"
                    }
                    var loading;
                    try {
                        loading = showCommonLoading();
                    } catch (e) { }
                    $.ajax({
                        type: "post",
                        url: dataurl,
                        data: pdata,
                        dataType: "json",
                        success: function (data) {
                            try {
                                loading.close();
                            } catch (e) { }
                            if (data.success) {
                                ShowMsg(data.message, true);
                                dlg.close();
                                $('.hishop_menu_scroll', window.parent.document).css("opacity", "1");
                                databox.QWRepeater("reload");
                            } else {
                                alert(data.message);
                            }
                        },
                        error: function () {
                            try {
                                alert("未知异常");
                                dlg.close();
                                $('.hishop_menu_scroll', window.parent.document).css("opacity", "1");
                                loading.close();
                            } catch (e) { }
                        }
                    });
                    return false;
                },
                focus: true
            }],
            close: function () {
                $('.hishop_menu_scroll', window.parent.document).css("opacity", "1");
            },
            init: function () {
                var addvaluebox = $("#addvaluebox");
                addvaluebox.append($("#addAttributeValue").html());
                $(".hid_attrid", addvaluebox).val(id);
                $(".ipt_attrvalue", addvaluebox).val("");
            }
        });

        //DialogShow("添加" + attributename + "的属性值", "addskuvalue", "addAttributeValue", "btnCreateValue");
    }


    //添加扩展属性
    function ShowAddKzAtribute() {
        formtype = "add";
        arrytext = null;
        setArryText('ctl00_contentHolder_attributeView_txtName', "");
        setArryText('ctl00_contentHolder_attributeView_txtValues', "");

        DialogShow("添加扩展属性", "addattri", "addAttribute", "ctl00_contentHolder_attributeView_btnCreate");
        //$('#ctl00_contentHolder_attributeView_chkMulti_copy').iCheck();
    }


    function validatorForm() {
        if (formtype == "addvalue") {
            $("#ctl00_contentHolder_attributeView_txtValueStr").val($("#ctl00_contentHolder_attributeView_txtValueStr").val().replace(/\s/g, "").replace("\\", ""));
            if ($("#ctl00_contentHolder_attributeView_txtValueStr").val() == "") {
                alert("请输入属性值！");
                return false;
            }
            if ($("#ctl00_contentHolder_attributeView_currentAttributeId").val().replace(/\s/g, "") == "") {
                alert("请选择要添加值的属性");
                return false;
            }
        } else {
            $("#ctl00_contentHolder_attributeView_txtName").val($("#ctl00_contentHolder_attributeView_txtName").val().replace(/\s/g, "").replace("\\", ""));
            $("#ctl00_contentHolder_attributeView_txtValues").val($("#ctl00_contentHolder_attributeView_txtValues").val().replace(/\s/g, "").replace("\\", ""));
            if ($("#ctl00_contentHolder_attributeView_txtName").val() == "") {
                alert("请输入扩展属性名称");
                return false;
            }
            if ($("#ctl00_contentHolder_attributeView_txtValues").val() == "") {
                alert("请输入扩展属性值");
                return false;
            }

        }
        return true;
    }
</script>

