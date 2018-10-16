<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.ReplyOnKey" CodeBehind="~/Admin/vshop/ReplyOnKey.aspx.cs" MasterPageFile="~/Admin/Admin.Master" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>




<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a href="javascript:void">管理</a></li>
                <li><a href="AddReplyOnKey.aspx">添加文本</a></li>
                <li><a href="AddSingleArticle.aspx">添加单图文</a></li>
                <li><a href="AddMultiArticle.aspx">添加多图文</a></li>
            </ul>
        </div>
        <%--   <div class="btn add_btn_sm">
            <a href="AddReplyOnKey.aspx" class="btn btn-primary"><i class="glyphicon glyphicon-plus" aria-hidden="true"></i>&nbsp;添加文本回复</a>
            <a href="AddSingleArticle.aspx" class="btn btn-primary"><i class="glyphicon glyphicon-plus" aria-hidden="true"></i>&nbsp;添加单图文回复</a>
            <a href="AddMultiArticle.aspx" class="btn btn-primary"><i class="glyphicon glyphicon-plus" aria-hidden="true"></i>&nbsp;添加多图文回复</a>
        </div>--%>
        <div class="clear"></div>
        <!--数据列表区域-->
        <div class="datalist clearfix">
            <!--S DataShow-->
            <table cellspacing="0" border="0" class="table table-striped table-fixed">
                <thead>
                    <tr>
                        <th scope="col" width="400">关键字</th>
                        <th scope="col" width="100">回复类型</th>
                        <th scope="col" width="60">发布</th>
                        <th scope="col" width="60">类型</th>
                        <th scope="col" width="150">最后修改日期</th>
                        <th scope="col">最后修改人</th>
                        <th class="td_left td_right_fff" scope="col" width="80">操作</th>
                    </tr>
                </thead>
                <tbody id="datashow"></tbody>
            </table>
            <!--E DataShow-->
            <div class="blank5 clearfix">
            </div>
        </div>
    </div>
    <div class="databottom">
    </div>
    <div class="bottomarea testArea">
        <!--顶部logo区域-->
    </div>



    <!--S Data Template-->
    <script id="datatmpl" type="text/html">
        {{each rows as item index}}
                <tr>
                    <td>{{item.Keys}}</td>
                    <td>{{item.ReplyTypeName}}</td>
                    <td>
                        <a href="javascript:Post_Release('{{item.ReplyId}}');" class="bt-release">{{if item.IsDisable}}
                        <img alt="点击发布" src="../images/ta.gif" />
                            {{else}}
                        <img alt="点击取消" src="../images/iconaf.gif" />
                            {{/if}}
                        </a>
                    </td>
                    <td>{{item.MessageTypeName}}</td>
                    <td>{{ item.LastEditDate | artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}</td>
                    <td>{{item.LastEditor}}</td>
                    <td>
                        <div class="operation">
                            {{if item.MessageType==1}}
                        <span><a href="EditReplyOnKey.aspx?id={{item.ReplyId}}">编辑</a></span>
                            {{else if item.MessageType==2}}
                        <span><a href="EditSingleArticle.aspx?id={{item.ReplyId}}">编辑</a></span>
                            {{else if item.MessageType==4}}
                        <span><a href="EditMultiArticle.aspx?id={{item.ReplyId}}">编辑</a></span>
                            {{/if}}
                        <span><a href="javascript:Post_Delete('{{item.ReplyId}}')">删除</a></span>
                        </div>
                    </td>

                </tr>
        {{/each}}
    </script>
    <!--E Data Template-->
    <input type="hidden" name="dataurl" id="dataurl" value="/Admin/vshop/ashx/ReplyOnKey.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Utility/QW.Repeater.js" type="text/javascript"></script>
    <script src="/Admin/vshop/scripts/ReplyOnKey.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
