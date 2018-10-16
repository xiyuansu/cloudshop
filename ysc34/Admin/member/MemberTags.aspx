<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="MemberTags.aspx.cs" Inherits="Hidistro.UI.Web.Admin.member.MemberTags" %>


<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <style>
        .formitem ul li { float: left; width: 100%; line-height: 32px; margin-bottom: 10px; }
            .formitem ul li span { width: 100px; float: left; text-align: right; }
            .formitem ul li input { float: left; }
            .formitem ul li i { float: left; font-style: normal; }
    </style>
    <script type="text/javascript">
        //添加标签
        function addTag() {
            $("#hidTagId").val("");
            DialogShow("添加标签", 'addTag', 'div_TagInfo', 'btnSaveTag');
        }
        function editTag(obj) {
            $("#hidTagId").val($(obj).attr("tagId"));
            var name = $(obj).attr("tagName");
            var orderCount = $(obj).attr("orderCount");
            var orderTotalAmount = $(obj).attr("orderTotalAmount");
            $("#txtTagName").val(name);
            $("#txtOrderCount").val(orderCount);
            $("#txtOrderTotalAmount").val(orderTotalAmount);
            setArryText("txtTagName", $("#txtTagName").val().replace(/\s/g, ""));
            setArryText("txtOrderCount", $("#txtOrderCount").val().replace(/\s/g, ""));
            setArryText("txtOrderTotalAmount", $("#txtOrderTotalAmount").val().replace(/\s/g, ""));
            DialogShow("编辑标签", 'editTag', 'div_TagInfo', 'btnSaveTag');
        }
        //验证
        function validatorForm() {
            if ($("#txtTagName").val().trim() == "") {
                alert("请输入标签名称，限制输入20个字符以内");
                return false;
            }
            if ($("#txtOrderCount").val().trim() != "" && $("#txtOrderCount").val().trim() != "0") {
                var a = $("#txtOrderCount").val();
                var regr = /^[1-9]\d*$/;
                if (!regr.test(a) || a > 1000 || a <= 0) {
                    alert("交易笔数只能输入整数，限制在1-1000之间");
                    return false;
                }
            }
            if ($("#txtOrderTotalAmount").val().trim() != "") {
                var a = $("#txtOrderTotalAmount").val();
                var regr = /^\d+(?:\.\d{1,2})?$/;
                if (!regr.test(a) || a > 100000000) {
                    alert("请输入正确的累计金额，0-100000000之间，不超过两位小数");
                    return false;
                }
            }
            setArryText("txtTagName", $("#txtTagName").val().replace(/\s/g, ""));
            setArryText("txtOrderCount", $("#txtOrderCount").val().replace(/\s/g, ""));
            setArryText("txtOrderTotalAmount", $("#txtOrderTotalAmount").val().replace(/\s/g, ""));
            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <asp:HiddenField runat="server" ID="hidTagId" Value="" ClientIDMode="Static" />
    <div class="dataarea mainwidth databody">
        <!--数据列表区域-->
        <div class="datalist clearfix">
            <div class="searcharea">
                <input type="button" id="btnOpenAdd" class="btn btn-primary" value="添加标签" onclick="addTag()" />
            </div>

            <table class="table table-striped">
                <tr>
                    <th width="35%">标签名称</th>
                    <th width="10%">会员人数</th>
                    <th width="15%" class="td_left td_right_fff" scope="col">操作</th>
                </tr>
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
    <div class="databottom"></div>
    <div class="bottomarea testArea">
        <!--顶部logo区域-->
    </div>


    <div id="div_TagInfo" style="display: none;">
        <div class="frame-content">
            <div class="formitem">
                <ul>
                    <li><span><em>*</em>&nbsp;标签名称：</span>
                        <asp:TextBox ID="txtTagName" CssClass="form_input_m form-control" Style="margin-left: 10px;" runat="server" placeholder="限制在20字符以内" ClientIDMode="Static" />
                    </li>
                    <li>
                        <span>自动打标签条件：</span>
                        <span>累计成功交易&nbsp;&nbsp;</span>
                        &nbsp;&nbsp;<asp:TextBox ID="txtOrderCount" CssClass="form_input_xs form-control" runat="server" ClientIDMode="Static" />&nbsp;&nbsp;
                            <i>&nbsp;&nbsp;笔</i>
                    </li>
                    <li>
                        <span>或&nbsp;&nbsp;</span><span>累计购买金额&nbsp;&nbsp;</span>
                        &nbsp;&nbsp;<asp:TextBox ID="txtOrderTotalAmount" CssClass="form_input_xs form-control" runat="server" ClientIDMode="Static" />&nbsp;&nbsp;
                            <i>&nbsp;&nbsp;元</i>
                    </li>
                </ul>
            </div>
        </div>
    </div>

    <div style="display: none">
        <input type="button" name="btnSaveTag" value="保存" id="btnSaveTag" class="submit_DAqueding" />
    </div>

    <!--S Data Template-->
    <script id="datatmpl" type="text/html">
        {{each rows as item index}}
                <tr>
                    <td>{{item.TagName}}</td>
                    <td>
                        <a href="/admin/member/ManageMembers.aspx?tagId={{item.TagId}}">{{item.MemberCount}}</a>
                    </td>
                    <td>
                        <span><a href="javascript:;" onclick="editTag(this)" tagid="{{item.TagId}}" tagname="{{item.TagName}}" ordercount="{{item.OrderCount}}" ordertotalamount="{{item.OrderTotalAmount.toFixed(2)}}">编辑</a></span>
                        <span><a href="javascript:Post_Delete('{{item.TagId}}')">删除</a></span>
                    </td>
                </tr>
        {{/each}}
    </script>
    <!--E Data Template-->

    <input type="hidden" name="dataurl" id="dataurl" value="/admin/member/ashx/MemberTags.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/admin/member/scripts/MemberTags.js" type="text/javascript"></script>
</asp:Content>
