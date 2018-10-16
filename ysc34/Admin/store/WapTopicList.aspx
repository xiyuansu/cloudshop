<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="WapTopicList.aspx.cs" Inherits="Hidistro.UI.Web.Admin.store.WapTopicList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a id="all" href="WapTopicList.aspx">页面列表</a></li>
                <li runat="server" id="li1"><a href="PcTopicTempEdit.aspx?topicId=0" target="_blank">自定义页面（PC）</a></li>
                <li runat="server" id="liH5"><a href="TopicTempEdit.aspx?topicId=0" target="_blank">自定义页面（微信）</a></li>
                <li runat="server" id="liApp"><a href="AppTopicTempEdit.aspx?topicId=0" target="_blank">自定义页面（APP）</a></li>
            </ul>
        </div>

        <div class="searcharea">
            <ul>
                <li>
                    <span>页面标题：</span>
                    <span>
                        <input id="txtTitle" type="text" class="forminput form-control" />
                        <asp:Label ID="lblStatus" runat="server" Style="display: none;" ClientIDMode="Static"></asp:Label>
                    </span>
                </li>
                <li>
                    <span>页面类型：</span><span>
                        <abbr class="formselect">
                            <asp:DropDownList runat="server" ID="ddlTopicType" CssClass="iselect" ClientIDMode="Static" />
                        </abbr>
                    </span>
                </li>
                <li>
                    <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary">
                </li>
            </ul>
        </div>
        <!--数据列表区域-->
        <div class="datalist clearfix">

            <!--S warp-->
            <!--S DataShow-->
            <table border="0" cellspacing="0" width="80%" cellpadding="0" class="table table-striped">
                <thead>
                    <tr>
                        <th style="width: 45%">页面标题
                        </th>
                        <th>页面类型
                        </th>
                        <th style="width: 30%">页面地址
                        </th>
                        <th>操作
                        </th>
                    </tr>
                </thead>
                <tbody id="datashow"></tbody>

            </table>
            <div class="blank12 clearfix"></div>

            <!--E DataShow-->
            <!--E warp-->
            <script id="datatmpl" type="text/html">
                {{each rows as item index}}
                <tr>
                    <td><span>{{item.Title}}</span>
                        {{if item.IsHomePage}}
                        <span style="color: red;">(已设为首页)</span>
                        {{/if}}
                    </td>
                    <td>{{item.TopicTypeStr}}</td>
                    <td>{{item.linkUrl}}</td>
                    <td class="operation">
                        <span id="alink" runat="server"><a href='javascript:void(0);' onclick="javascript:DialogFrame('store/WapTopicLink.aspx?TopicID={{item.TopicId}}', '专题分享', 385, 240, null);">链接</a></span>
                        <span><a href="{{item.EidUrl}}" target="_blank">编辑</a></span>
                         {{if !item.IsHomePage}}
                        <span><a href="javascript:Delete({{item.TopicId}})">删除</a></span>
                        {{/if}}
                        {{if item.TopicTypeStr =="PC"}}                              
                        {{if item.IsHomePage}}
                        <span><a href="javascript:cancelHomePage({{item.TopicId}})">取消首页</a></span>
                        {{/if}}
                        {{if !item.IsHomePage}}
                        <span><a href="javascript:setHomePage({{item.TopicId}})">设为首页</a></span>
                        {{/if}}
                        {{/if}}
                    </td>
                </tr>
                {{/each}}              

            </script>
        </div>
        <!--数据列表底部功能区域-->
        <div class="page">
            <div class="bottomPageNumber clearfix">
                <div class="pageNumber">
                    <div class="pagination" id="showpager"></div>
                </div>
            </div>
        </div>
    </div>
    <div id="divShareProduct" style="display: none">
        <div class="frame-content">
            <p>
                <span id="SpanShareId"></span>
            </p>

            <table style="width: 300px; height: 340px;" align="center">
                s
                <tr>
                    <td>
                        <img id="imgsrc" src="" type="img" width="300px" /></td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <input type="hidden" name="dataurl" id="dataurl" value="/admin/store/ashx/WapTop.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/admin/store/scripts/WapTop.js" type="text/javascript"></script>
</asp:Content>
