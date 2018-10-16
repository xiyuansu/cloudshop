<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.ImageType" MasterPageFile="~/Admin/Admin.Master" CodeBehind="ImageType.aspx.cs" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>




<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <!--面包屑-->
    <div class="dataarea mainwidth databody">
        <div class="btn_bottom mb_20" style="float: left; width: 100%;">
            <asp:Button ID="ImageTypeAdd" runat="server" OnClientClick="ImageType(); return false;" Text="添加分类" CssClass="btn btn-primary" />

        </div>
        <div class="datalist clearfix">
            <div class="imageDataLeft">
                <table cellspacing="0" border="0" class="table table-striped table-fixed">
                    <thead>
                        <tr>
                            <th>分类名称</th>
                            <th width="15%">图片数</th>
                            <th width="15%">排序</th>
                            <th style="width: 20%;">操作</th>
                        </tr>
                    </thead>
                    <!--S DataShow-->
                    <tbody id="datashow"></tbody>
                    <!--E DataShow-->
                </table>
            </div>

        </div>
    </div>

    <!--添加分类-->
    <div id="addImageType" style="display: none;">
        <div class="formitem">
            <input type="hidden" class="cateid" value="" />
            <ul>
                <li class="mb_0"><span class="formitemtitle"><em>*</em>分类名称：</span>
                    <input type="text" class="forminput form-control catename" placeholder="长度限制在20个字符以内" style="width: 250px;" />
                    <p id="contentHolder_AddImageTypeNameTip" class="Pa_100"></p>
                </li>
            </ul>

        </div>
    </div>


    <!--S Data Template-->
    <script id="datatmpl" type="text/html">
        {{each rows as item index}}
                <tr>
                    <td>{{item.CategoryName}}</td>
                    <td>{{item.PhotoCounts}}</td>
                    <td>{{if item.CategoryId>0}}
                        <input type="text" class="forminput form-control txt-sort" onblur="Post_Sort('{{item.CategoryId}}',this.value)" onkeyup="this.value=this.value.replace(/\D/g,'')"
                            onafterpaste="this.value=this.value.replace(/\D/g,'')" value="{{item.DisplaySequence}}" style="width: 60px;" />
                        {{else}}
                        -
                        {{/if}}
                    </td>
                    <td>{{if item.CategoryId>0}}
                        <div class="operation">
                            <span><a href="javascript:ImageType('{{item.CategoryId}}','{{item.CategoryName}}')">编辑</a></span>
                            <span><a href="javascript:Post_Delete('{{item.CategoryId}}')">删除</a></span>
                        </div>
                        {{/if}}
                    </td>
                </tr>
        {{/each}}
  
    </script>
    <!--E Data Template-->
    <input type="hidden" name="dataurl" id="dataurl" value="/Admin/store/ashx/ImageType.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Utility/QW.Repeater.js" type="text/javascript"></script>
    <script src="/Admin/store/scripts/ImageType.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" language="javascript">
        function InitValidators() {
            initValid(new InputValidator('contentHolder_AddImageTypeName', 1, 20, false, null, '类别名称不能为空，长度限制在20个字符以内'))
        }

        $(document).ready(function () { InitValidators(); });

        function ImageType(id, name) {
            var title = "添加分类";
            if (id > 0) {
                title = "编辑分类";
            }
            $('.hishop_menu_scroll', window.parent.document).css("opacity", "0.3");
           var dlg= art.dialog({
                title: title,
                content: "<div id='catebox'></div>",
                width: 460,
                lock: true,
                button: [{
                    name: '保存',
                    callback: function () {
                        var catebox = $("#catebox");
                        var cateid= $(".cateid", catebox).val();
                        var catename = $(".catename", catebox).val();
                        if (catename.length < 1) {
                            alert("请填写分类名称");
                            return;
                        }
                        if (catename.length > 20) {
                            alert("类别名称不能为空，长度限制在20个字符以内");
                            return;
                        }
                        var pdata = {
                            id: cateid, name: catename, action: "Edit"
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
                                    ReloadPageData();
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
                close:function(){
                    $('.hishop_menu_scroll', window.parent.document).css("opacity", "1");
                },
                init: function () {
                    var catebox = $("#catebox");
                    catebox.append($("#addImageType").html());
                    $(".cateid", catebox).val(id);
                    $(".catename", catebox).val(name);
                }
            });
            //DivWindowOpen(630, 200, 'addImageType');
            
        }
    </script>
</asp:Content>
