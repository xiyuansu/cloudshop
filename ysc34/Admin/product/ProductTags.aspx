<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ProductTags.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ProductTags" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="Server">
    <div class="dataarea mainwidth databody">
        <div class="datalist clearfix">
            <div class="searcharea a_none mb_0">
                <ul>
                    <li class="pull-right mr0">
                        <a class="btn btn-primary" href="javascript:ShowTags(0,'');">添加商品标签</a>
                    </li>
                </ul>
            </div>
            <table class="table table-striped">
                <thead>
                    <tr>
                        <th>标签名称</th>
                        <th style="width: 36%;">操作</th>
                    </tr>
                </thead>
                <tbody id="datashow">
                </tbody>
            </table>
            <!--S Data Template-->
            <script id="datatmpl" type="text/html">
                {{each rows as item index}}
                  
                    <tr>
                        <td>{{item.TagName}}</td>
                        <td>
                            <div class="operation">
                                <span><a href="javascript:void(0);" onclick="ShowTags({{item.TagID}},'{{item.TagName}}')">编辑</a></span>
                                <span class="submit_dalata">
                                    <a href="javascript:Post_Deletes({{item.TagID}})">删除</a>
                                </span>
                            </div>
                        </td>
                    </tr>
                {{/each}}
            </script>
            <!--E Data Template-->
        </div>
    </div>



    <div id="editcontent" style="display: none">
        <div class="frame-content">
            <input type="hidden" class="tagid"/>
            <p>
                <span class="frame-span frame-input90"><em>*</em>&nbsp;标签名称：</span>
                <input type="text" maxlength="20" class="forminput form-control tagname"/>
            </p>
        </div>
    </div>
    <script type="text/javascript" language="javascript">
        var formtype = "";
        function ShowTags(id, tagname) {
            var title = "添加标签";
            if (id > 0) {
                title = "编辑标签";
            }
            $('.hishop_menu_scroll', window.parent.document).css("opacity", "0.3");
            var dlg = art.dialog({
                title: title,
                content: "<div id='editbox'></div>",
                lock: true,
                button: [{
                    name: '保存',
                    callback: function () {
                        var editbox = $("#editbox");
                        var tagid = $(".tagid", editbox).val();
                        var tagname = $(".tagname", editbox).val();
                        if (tagname.length < 1) {
                            alert("请填写标签名称");
                            return;
                        }
                        if (tagname.length > 20) {
                            alert("标签名称最长不可以超过20字符");
                            return;
                        }
                        var pdata = {
                            id: tagid, tagname: tagname, action: "Edit"
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
                    var editbox = $("#editbox");
                    editbox.append($("#editcontent").html());
                    $(".tagid", editbox).val(id);
                    $(".tagname", editbox).val(tagname);
                }
            });
        }
    </script>


    <input type="hidden" name="dataurl" id="dataurl" value="/admin/product/ashx/ProductTags.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/QW.Repeater.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/admin/product/scripts/ProductTags.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="Server">
</asp:Content>
