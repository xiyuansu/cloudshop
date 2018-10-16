<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="CPTopicList.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ChoicePage.CPTopicList" %>




<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <style>
        html { background: #fff !important; }

        body { padding: 0; width: 100%; }

        #mainhtml { margin: 0; padding: 20px 20px 60px 20px; width: 100% !important; }

        .carat { margin-right: 0 !important; }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <textarea name="hidformData" id="hidformData" style="display: none;"><%=formData %></textarea>
    <input type="hidden" name="hidreturnUrl" id="hidreturnUrl" value="<%=returnUrl %>" />
    <div class="dataarea mainwidth databody">

        <!--数据列表区域-->
        <div class="datalist clearfix">

            <!--数据列表区域-->

            <table class="table table-striped">
                <thead>
                    <tr>
                        <th width="120">页面标题</th>
                        <th width="120">页面地址</th>
                        <th width="50">操作</th>
                    </tr>
                </thead>
                <!--S DataShow-->
                <tbody id="datashow"></tbody>
                <!--E DataShow-->
            </table>
        </div>
        <!--S Pagination-->
        <div class="page">
            <div class="bottomPageNumber clearfix">
                <div class="pageNumber">
                    <div class="pagination" id="showpager"></div>
                </div>
            </div>
        </div>
        <!--E Pagination-->
    </div>
    <div id="divShareProduct" style="display: none">
        <div class="frame-content">
            <p>
                <span id="SpanShareId"></span>
            </p>

            <table style="width: 300px; height: 340px;">
                <tr>
                    <td>
                        <img id="imgsrc" src="" type="img" width="300px" /></td>
                </tr>
            </table>
        </div>
    </div>


    <!--S Data Template-->
    <script id="datatmpl" type="text/html">
        {{each rows as item index}}
                <tr>
                    <td>{{item.Title}}</td>
                    <td><%=Globals.HostPath(HttpContext.Current.Request.Url)%>/appshop/Topics?TopicId={{item.TopicId}}</td>
                    <td>
                        <a class="a_select" href="javascript:closewindow('{{item.TopicId}}')">选择</a>
                    </td>
                </tr>
        {{/each}}
    </script>
    <!--E Data Template-->

    <input type="hidden" name="callback" id="callback" value="<%=JsCallBack%>" />
    <input type="hidden" name="dataurl" id="dataurl" value="/admin/ChoicePage/ashx/CPTopicList.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/admin/ChoicePage/scripts/CPTopicList.js" type="text/javascript"></script>


    <script type="text/javascript">
        function closewindow(id) {
            var win = art.dialog.open.origin;
            var callback = $("#callback").val();
            var returnUrl = $("#hidreturnUrl").val();
            var formData = $("#hidformData").val();
            if (callback && callback.length > 0) {
                artwin[callback]();
            } else {
                if (returnUrl && returnUrl.length > 0) {
                    if (returnUrl.indexOf("?") > -1) {
                        returnUrl += "&";
                    } else {
                        returnUrl += "?";
                    }
                    returnUrl += "TopicId=" + id + "&";
                    if (formData.length > 0) {
                        returnUrl += "formData=" + formData + "&";
                    }
                    win.location.href = returnUrl;
                } else {
                    win.location.reload();
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script language="javascript" type="text/javascript">

        function ShowShareLink(ShareUrl) {
            $("#imgsrc").attr("src", "/api/VshopProcess.ashx?action=GenerateTwoDimensionalImage&url=" + ShareUrl);
            $("#SpanShareId").html(ShareUrl);
            ShowMessageDialog("我要分享", 'sharedetails', 'divShareProduct');
        }
    </script>
</asp:Content>
