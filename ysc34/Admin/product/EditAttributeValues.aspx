<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="EditAttributeValues.aspx.cs" Inherits="Hidistro.UI.Web.Admin.EditAttributeValues" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">

        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a href="javascript:void">编辑扩展属性</a></li>
                <li><a href='<%= Hidistro.Core.Globals.GetAdminAbsolutePath("/product/ProductTypes.aspx")%>'>商品类型</a></li>
                <li><a href='<%= Hidistro.Core.Globals.GetAdminAbsolutePath("/product/EditProductType.aspx?typeId=" + Page.Request.QueryString["typeId"])%>'>基本设置</a></li>
                <li><a href='<%= Hidistro.Core.Globals.GetAdminAbsolutePath("/product/EditAttribute.aspx?typeId=" + Page.Request.QueryString["typeId"])%>'>扩展属性</a></li>
                <li><a href='<%= Hidistro.Core.Globals.GetAdminAbsolutePath("/product/EditSpecification.aspx?typeId=" + Page.Request.QueryString["typeId"])%>'>规 格</a></li>
            </ul>
        </div>
        <!--结束-->
        <!--数据列表区域-->
        <div class="datalist clearfix">
            <div class="batchHandleButton mb_20">
                <a name="button" id="button1" href="javascript:void" class="btn btn-primary" onclick="AddAttributeValue();">添加属性值</a>
            </div>
            <table class="table table-striped">
                <thead>
                    <tr>
                        <th>属性值</th>
                        <th width="15%">排序</th>
                        <th style="width: 12%;">操作</th>
                    </tr>
                </thead>
                <tbody id="datashow">
                </tbody>
            </table>
            <!--S Data Template-->
            <script id="datatmpl" type="text/html">
                {{each rows as item index}}
                    <tr>
                        <td>{{item.ValueStr}}</td>

                        <td>
                            <div class="sortbox" val-id="{{item.ValueId}}" val-preid="" val-nextid="" val-sort="{{item.DisplaySequence}}"></div>
                        <td>
                            <div class="operation">
                                <span class="submit_dalata">
                                    <a href="javascript:Post_Deletes({{item.ValueId}})">删除</a>
                                </span>
                                <span><a href="javascript:UpdateAttributeValue('{{item.ValueId}}','{{item.ValueStr}}');">修改</a></span>
                            </div>
                        </td>
                    </tr>
                {{/each}}
            </script>
            <!--E Data Template-->
        </div>

    </div>



    <%--添加属性值--%>
    <div id="addAttributeValue" style="display: none;">
        <div class="frame-content">
            <p>
                <span class="fl" style="line-height: 32px;"><em>*</em>属性值：</span>
                <asp:TextBox ID="txtValue" runat="server" Width="300" ClientIDMode="Static" CssClass="form-control forminput"></asp:TextBox>
            </p>
            <b>扩展属性的值，字符数最多15个字符，不允许包含脚本标签和\(反斜杠)，系统会自动过滤</b>
        </div>
    </div>

    <%--修改属性值--%>
    <div id="updateAttributeValue" style="display: none;">
        <div class="frame-content">
            <p>
                <span class="fl" style="line-height: 32px;"><em>*</em>属性值：</span>
                <asp:TextBox ID="txtOldValue" runat="server" Width="300" ClientIDMode="Static" CssClass="form-control forminput"></asp:TextBox>
            </p>
            <b>扩展属性的值，字符数最多15个字符，不允许包含脚本标签和\(反斜杠)，系统会自动过滤</b>
        </div>
        <div class="clear"></div>
    </div>
    <div style="display: none">

        <input type="text" id="btnCreate" class="submit_DAqueding" onclick="AddValue()" />
        <input type="text" id="btnUpdate" class="submit_DAqueding" onclick="EditValue()" />

        <input type="hidden" id="hidvalueId" />
        <input type="hidden" id="hidvalue" />
    </div>

    <input type="hidden" name="dataurl" id="dataurl" value="/admin/product/ashx/EditAttributeValues.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/QW.Repeater.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/admin/product/scripts/EditAttributeValues.js" type="text/javascript"></script>
    <script>
        var formtype = "";
        function AddAttributeValue() {
            formtype = "add";
            arrytext = null;
            setArryText('txtValue', "");

            DialogShow('添加属性值名称', 'attributevalue', 'addAttributeValue', 'btnCreate');
        }


        function UpdateAttributeValue(ValueId, ValueStr) {
            formtype = "edite";
            arrytext = null;
            setArryText('hidvalueId', ValueId);
            setArryText('txtOldValue', ValueStr);
            setArryText('hidvalue', ValueStr);

            DialogShow('修改扩展属性值', 'attributevalue', 'updateAttributeValue', 'btnUpdate');

        }

        function validatorForm() {
            if (formtype == "add") {
                var _v = $("#txtValue").val();
                if (_v == "") {
                    alert("请输入属性值名称");
                    return false;
                }
                if (_v.length > 15) {
                    alert("属性值必须小于15个字符，不能为空,且不能包含脚本标签、HTML标签、XML标签以及\\+");
                    return false;
                }
                return true;
            }
            else {
                var oldValue = $("#txtOldValue").val();
                if (oldValue == "") {
                    alert("扩展属性值不允许为空");
                    return false;
                }
                if (oldValue.length > 15) {
                    alert("属性值必须小于15个字符，不能为空,且不能包含脚本标签、HTML标签、XML标签以及\\+");
                    return false;
                }
                return true;
            }
        }
    </script>
</asp:Content>
