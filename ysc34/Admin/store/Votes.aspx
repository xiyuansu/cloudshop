<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Votes.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Votes" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>


<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="server">

    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a href="javascript:void">管理</a></li>
                <li><a href="AddVote.aspx">添加</a></li>
            </ul>

        </div>
        <div class="datalist clearfix">
            <!--S DataShow-->
            <table border="0" cellspacing="0" width="80%" cellpadding="0" class="table table-striped">
                <thead>
                    <tr>
                        <th width="8%">展开</th>
                        <th width="30%">投票标题</th>
                        <th width="11%">最多可选项数</th>
                        <th width="9%">投票总数</th>
                        <th width="13%">微信端是否显示</th>
                        <th width="12%">操作</th>
                    </tr>
                </thead>
                <tbody id="datashow"></tbody>
            </table>
            <!--E DataShow-->

        </div>

        <!--S Data Template-->
        <script id="datatmpl" type="text/html">
            {{each rows as item index}}            
                    <tr>
                        <td>
                            <img id="themesImg" src="../../utility/pics/plus.gif" onclick="errorEventTable(this);" /></td>
                        <td>{{item.VoteName}}</td>
                        <td>{{item.MaxCheck}}</td>
                        <td>{{item.VoteCounts}}</td>
                        <td>{{if item.IsDisplayAtWX}}
                            <img src="../images/allright.gif" style="border-width: 0px;">
                            {{else}}
                            <img src="../images/wrong.gif" style="border-width: 0px;">
                            {{/if}}
                        </td>
                        <td>
                            <div class="operation">
                                <span><a href="/admin/store/EditVote.aspx?VoteId={{item.VoteId}}">编辑</a></span>
                                <span>
                                    <a href="javascript:Post_Delete({{item.VoteId}})">删除</a></span>
                            </div>
                        </td>
                    </tr>
            <tr style="display: none;">
                <td colspan="7">
                    <div class="tpiao">
                        <table class="table table-striped" cellspacing="0" border="0" style="border-collapse: collapse;">
                            <tbody>
                                <tr>
                                    <th class="spanB" scope="col">选项值</th>
                                    <th class="spanB" scope="col">比例示意图</th>
                                    <th class="spanB" scope="col">百分比</th>
                                    <th class="spanB" scope="col">票数</th>
                                </tr>
                                {{each item.VoteItems as vitem vi}}
                                    <tr>
                                        <td>
                                            <span style="display: inline-block; width: 50px;">{{vitem.VoteItemName}}</span>
                                        </td>
                                        <td>
                                            <div style="width:{{vitem.Lenth}}px" class="votelenth"></div>
                                        </td>
                                        <td>
                                            <span style="display: inline-block; width: 50px;">{{vitem.Percentage}}%</span>
                                        </td>
                                        <td>{{vitem.ItemCount}}</td>
                                    </tr>
                                {{/each}}
                            </tbody>
                        </table>
    </div>
    </td>
            </tr>
            {{/each}}
        </script>
        <!--E Data Template-->

        <input type="hidden" name="dataurl" id="dataurl" value="/admin/store/ashx/Votes.ashx" />
        <script src="/Utility/artTemplate.js" type="text/javascript"></script>
        <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
        <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
        <script src="/admin/store/scripts/Votes.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" language="javascript">
        var isIE = !!document.all;
        function errorEventTable(tableObject) {
            tableObject.runat = "server";
            if (isIE) {
                var nextNodeObject = tableObject.parentNode.parentNode.nextSibling;
            }
            else {
                var nextNodeObject = tableObject.parentNode.parentNode.nextSibling.nextSibling;
            }
            (nextNodeObject.style.display == "none") ? nextNodeObject.style.display = "" : nextNodeObject.style.display = "none";
            (nextNodeObject.style.display == "none") ? tableObject.src = '<%=  "/utility/pics/plus.gif" %>' : tableObject.src = '<%=  "/utility/pics/minus.gif" %>';
        }


    </script>
</asp:Content>
