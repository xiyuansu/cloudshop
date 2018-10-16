<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="RedEnvelopeDetails.aspx.cs" Inherits="Hidistro.UI.Web.Admin.vshop.RedEnvelopeDetails" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>


<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <input type="hidden" id="hidRedEnvelopeId" name="hidRedEnvelopeId" value="<%=redEnvelopeId %>" />
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a href="javascript:void">红包详情</a></li>
                <li><a href="ManageRedEnvelope.aspx">管理</a></li>
                <li><a id="AddRedEnvelope" href="AddRedEnvelope.aspx" runat="server">新增</a></li>
            </ul>
        </div>

        <div class="datalist clearfix">
            <!--S DataShow-->
            <table cellspacing="0" border="0" class="table table-striped table-fixed">
                <thead>
                    <tr>
                        <td>红包名称</td>
                        <td width="20%">用户名</td>
                        <td width="20%">昵称</td>
                        <td width="20%">领取时间</td>
                        <td width="20%">红包金额(元)</td>
                    </tr>
                </thead>
                <tbody id="datashow"></tbody>
            </table>
            <!--E DataShow-->
            <div class="blank12 clearfix">
            </div>
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


    <!--S Data Template-->
    <script id="datatmpl" type="text/html">
        {{each rows as item index}}
                <tr>
                    <td>{{item.RedEnvelopeName}}</td>
                    <td>{{item.UserName}}</td>
                     <td>{{item.NickName}}</td>
                    <td>{{ item.GetTime | artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}</td>
                    <td>{{item.Amount.toFixed(2)}}</td>
                </tr>
        {{/each}}
    </script>
    <!--E Data Template-->
    <input type="hidden" name="dataurl" id="dataurl" value="/Admin/vshop/ashx/RedEnvelopeDetails.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Admin/vshop/scripts/RedEnvelopeDetails.js" type="text/javascript"></script>
</asp:Content>
