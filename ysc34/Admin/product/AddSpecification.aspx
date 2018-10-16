<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddSpecification.aspx.cs" Inherits="Hidistro.UI.Web.Admin.product.AddSpecification" %>

<%@ Import Namespace="Hidistro.Core" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <style type="text/css">
        #newPreview {
            FILTER: progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod=scale);
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="ProductTypes.aspx">商品类型</a></li>
                <li class="hover"><a href="javascript:void">添加新的商品类型</a></li>
            </ul>
        </div>
        <div class="columnright">
            <ul class="step_p_f">
                <li class="step_p_f_active_after">第一步：添加类型名称</li>
                <li class="step_p_f_active_after">第二步：添加扩展属性</li>
                <li class="step_p_f_active">第三步：添加规格</li>
            </ul>
            <div class="content datalist">
                <table class="table table-striped">
                    <thead>
                        <tr>
                            <th style="width: 20%;">规格名称</th>
                            <th>规格值</th>
                            <th style="width: 15%;">可上传规格图</th>
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
                        <td>{{item.AttributeName}}              
                  <a href="javascript:EditSkuDiv('{{item.AttributeName}}','{{item.UseAttributeImage.toString()}}','{{item.AttributeId}}');">修改</a>
                        </td>
                        <td>{{each item.AttributeValues as model index}}
                             <span class="SKUValue">
                                 <span class="span1" style="margin-right: 0px;">
                                     <a href="javascript:void(0)">{{model.ValueStr}}</a>
                                 </span>
                                 <span class="span2" style="margin-right: 0px;">
                                     <a href="javascript:void(0)" onclick="deleteSKUValue(this, '{{model.ValueId}}');">删除</a></span>
                             </span>
                            {{/each}}
                        </td>
                        <td>{{if  item.UseAttributeImage}}
                         <img src="../images/allright.gif" onclick="IsUplod('{{item.AttributeId}}','true')" />
                            {{else}}
                           <img src="../images/wrong.gif" onclick="IsUplod('{{item.AttributeId}}','false')" />
                            {{/if}}
                        </td>
                        <td>
                            <input type="hidden" value="{{item.AttributeId}}" id="DisplaySequence_{{index}}" />
                            <input type="image" onclick=" SetOrder('Fall', '{{item.DisplaySequence}}', '{{item.AttributeId}}', '{{index}}')" src="../images/fall.gif" style="border-width: 0px;">
                            <input type="image" onclick=" SetOrder('Rise', '{{item.DisplaySequence}}', '{{item.AttributeId}}', '{{index}}')" src="../images/rise.gif" style="border-width: 0px;"></td>
                        <td>
                            <div class="operation">

                                <span class="submit_tiajia"><a href="javascript:void(0)" onclick="ShowAddSKUValueDiv('{{item.AttributeId}}', '{{item.AttributeName}}');">添加规格值</a></span>
                                <span class="submit_bianji"><a href="EditSpecificationValues.aspx?TypeId={{item.TypeId}}&AttributeId={{item.AttributeId}}&UseAttributeImage={{item.UseAttributeImage}}">编辑</a></span>
                                <span class="submit_dalata">
                                    <%-- <Hi:ImageLinkButton runat="server" ID="lbtnDelete" CommandName="delete" IsShow="true" DeleteMsg="当前操作将彻底删该除规格及下属的所有规格值，确定吗？" Text="删除" /></span>--%>
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
                <%-- <input type="button" onclick="AddSkuDiv()" name="button" id="button" value="添加新规格" class="submit_bnt3"/>--%>
                <a name="button" id="button" class="btn btn-primary "
                    onclick="AddSkuDiv();">添加新规格</a>
            </div>


            <%--添加新的规格--%>
            <div id="addSKU" style="display: none;">
                <div class="frame-content">
                    <p><span class="frame-span frame-input90"><em>*</em>规格名称：</span><asp:TextBox ID="txtName" ClientIDMode="Static" CssClass="forminput" runat="server"></asp:TextBox></p>
                    <b id="ctl00_contentHolder_specificationView_txtNameTip">规格名称长度在1至30个字符之间，不允许包含脚本标签和\(反斜杠)，系统会自动过滤</b>
                    <span class="frame-span frame-input90">可上传规格图：</span>
                    <input type="checkbox" id="chkIsImg" />
                </div>
            </div>




            <div style="display: none">
             <input type="button" id="btnCreate" class="submit_DAqueding" onclick="AddSku();" />
                <input type="button" id="btnEdit" class="submit_DAqueding" onclick="EditSku();" />
            </div>

            <input type="hidden" id="hidEditId" />
            <div class="Pg_15">
                <asp:Button ID="btnFilish" runat="server" CssClass="btn btn-primary" Text="完成" />
            </div>
        </div>
    </div>
    <input type="hidden" name="dataurl" id="dataurl" value="/admin/product/ashx/AddSpecification.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/QW.Repeater.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/admin/product/scripts/AddSpecification.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" language="javascript">
        function InitValidators() {
            initValid(new InputValidator('txtName', 1, 30, false, null, '规格名称长度在1至30个字符之间'));
        }
        $(document).ready(function () { InitValidators(); });
    </script>
    <script type="text/javascript" language="javascript">
        var formtype = "";
        //判断规格值


        //添加新规格
        function AddSkuDiv() {
            //addSKU
            $("#hidEditId").val('');
            formtype = "addsku";
            DialogShow("添加新规格", "addskuwin", "addSKU", "btnCreate");
        }
        //修改规格
        function EditSkuDiv(name, isImage, attId) {
            DialogShow("修改规格", "editskuwin", "addSKU", "btnEdit");
            $("#txtName").val(name);
            if (isImage == "true") {
                $("#chkIsImg").prop("checked", true);
            }
            else {
                $("#chkIsImg").prop("checked", false);
            }
            $("#hidEditId").val(attId);
        }
        //添加规格值
        function ShowAddSKUValueDiv(attributeId, attributename) {
            var pathurl = "product/SkuValue.aspx?action=add&callback=CloseDialogAndReloadData&attributeId=" + attributeId;
            var title = "添加" + attributename + "的规格值";
            DialogFrame(pathurl, title, 540, 280);
        }



        function validatorForm() {
            arrytext = null;
            var skuname = $("#txtName").val().replace(/\s/g, "");
            skuname = skuname.replace(/<[^>].*?>/g, "");
            skuname = skuname.replace(/\，/g, ",");
            // skuname = skuname.replace(/[\\|\/]/g, "");
            skuname = skuname.replace(/[\\]/g, "");
            if (skuname == "") {
                alert("请输入规格名称");
                return false;
            }
            if (skuname.length < 1 || skuname.length > 30) {
                alert("规格名称长度在1至30个字符之间");
                return false;
            }
            var isImage = false;
            var editId = $("#hidEditId").val();
            $("[name='hidIsImage']").each(function (i, obj) {
                var s = $(obj).val();

                var skuId = $(obj).attr("skuId");
                if (s == "True" && skuId != editId) {
                    isImage = true;
                }
            });
            if (isImage && ($("#chkIsImg").is(":checked") == true || $("#specificationView_chkIsImg").is(":checked") == "checked")) {
                alert("已有其他规格可上传图片，最多只有一个规格允许上传商品规格图");
                return false;
            }
           if ($("#chkIsImg").is(":checked") == true || $("#chkIsImg").is(":checked") == "checked") {
                setArryText('chkIsImg', "true");
                $("#chkIsImg").val(true);
            } else {
                $("#chkIsImg").val(false);
            }
            setArryText('txtName', skuname);
            return true;
        }

        function deleteSKUValue(obj, valueId) {
            $.ajax({
                url: "AddSpecification",
                type: 'post', dataType: 'json', timeout: 10000,
                data: { ValueId: valueId, isCallback: "true" },
                async: false,
                success: function (data) {
                    if (data.Status == "true") {
                        databox.QWRepeater("reload");
                    }
                    else {
                        ShowMsg("此规格值有商品在使用，删除失败", false);
                    }
                }
            });
        }
    </script>
</asp:Content>


