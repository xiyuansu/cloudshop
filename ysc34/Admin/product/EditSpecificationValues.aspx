<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="EditSpecificationValues.aspx.cs" Inherits="Hidistro.UI.Web.Admin.EditSpecificationValues" %>


<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript">
        //编辑规格值
        function UpdateAttributeValue(ValueId, ValueStr) {
            var pathurl = "product/SkuValue.aspx?callback=SuccessAndCloseReload&action=update&valueId=" + ValueId;
            var title = "修改规格值";
            DialogFrame(pathurl, title, 540, 280);
        }

        //添加新规格值
        function ShowAddSKUValueDiv(attributeId) {
            var pathurl = "product/SkuValue.aspx?callback=SuccessAndCloseReload&action=add&attributeId=" + attributeId;
            var title = "添加规格值";
            DialogFrame(pathurl, title, 540, 280);
        }

        String.prototype.trim = function () {
            return this.replace(/^\s+|\s+$/g, "");//删除前后空格
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">

        <div class="columnright">
            <!-- 添加按钮-->
            <div class="batchHandleArea mb_20">
                <a href="javascript:ShowAddSKUValueDiv( '<%=Page.Request.QueryString["AttributeId"]%>');" class="btn btn-primary">添加新规格值</a>
            </div>

            <!--结束-->
            <!--数据列表区域-->
            <div class="datalist clearfix">

                <div>
                    <table class="table table-striped">
                        <thead>
                            <tr>
                                <th>属性值</th>
                                <th>排序</th>
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
        </div>


        <input runat="server" type="hidden" id="currentAttributeId" clientidmode="static" />

    </div>



    <input type="hidden" name="dataurl" id="dataurl" value="/admin/product/ashx/EditSpecificationValues.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/QW.Repeater.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/admin/product/scripts/EditSpecificationValues.js" type="text/javascript"></script>
</asp:Content>

