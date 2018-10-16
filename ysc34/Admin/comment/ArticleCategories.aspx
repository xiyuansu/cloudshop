<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.ArticleCategories" CodeBehind="ArticleCategories.aspx.cs"
    MasterPageFile="~/Admin/Admin.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <!--选项卡-->
    <div class="databody mainwidth">
        <div class="searcharea a_none">

            <a class="btn btn-primary float_r" href="javascript:DialogFrame('comment/AddArticleCategory.aspx?source=add','添加文章分类',null,null,function(e){location.reload();})">添加分类</a>

        </div>
        <!--选项卡-->
        <div class="dataarea mainwidth">
            <!--数据列表区域-->
            <div class="datalist clearfix">

                <table class="table table-striped">
                    <thead>
                        <tr>
                            <th width="15%">分类编号</th>
                            <th width="35%">分类名称</th>
                            <th width="25%">显示顺序</th>
                            <th>操作</th>
                        </tr>
                    </thead>
                    <tbody id="datashow">
                    </tbody>
                </table>
                <!--S Data Template-->
                <script id="datatmpl" type="text/html">
                    {{each rows as item index}}
                 
                    <tr>
                        <td>{{item.CategoryId}}
                        </td>
                        <td>
                            <img src="{{item.IconUrl}}" class="Img100_30" style="border: none;" />
                            {{item.Name}}
                        </td>
                        <td>
                            <input id="Text1" type="text" class="form-control txtdisplay" data-id="{{item.CategoryId}}" data-oldvalue="{{item.DisplaySequence}}" value='{{item.DisplaySequence}}' style="width: 60px;" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" /></td>
                        <td>
                            <div class="operation">
                                <span>
                                    <a href="javascript:DialogFrame('comment/EditArticleCategory.aspx?&callback=CloseDialogAndReloadData&CategoryId={{item.CategoryId}}','编辑文章分类',null,null,function(e){  databox.QWRepeater('reload');})">编辑</a>
                                </span>
                                <span>
                                    <a href="javascript:Post_Deletes({{item.CategoryId}})">删除</a>
                                </span>
                            </div>
                        </td>
                    </tr>
                    {{/each}}
                </script>
                <!--E Data Template-->
            </div>
        </div>
    </div>
    <span id="SpanVildateMsg" style="visibility: hidden; width: 100%; z-index: 999; height: 0px; position: absolute;"></span>

    <script>
        function CheckOrderNumber() {
            var reg = /^[0-9]*$/;
            tag = true;
            $(".datalist input[type='text']").each(function (index, item) {
                if (!reg.test($(this).val().replace(/\s/g, ""))) {
                    alert("排序值不允许为非负数！");
                    tag = false;
                }
            });
            return tag;
        }
    </script>
    <input type="hidden" name="dataurl" id="dataurl" value="/admin/comment/ashx/ArticleCategories.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/QW.Repeater.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/admin/comment/scripts/ArticleCategories.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
