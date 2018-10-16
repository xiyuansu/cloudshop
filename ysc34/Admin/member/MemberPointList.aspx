<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="MemberPointList.aspx.cs" Inherits="Hidistro.UI.Web.Admin.member.MemberPointList" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li class="hover">
                    <a href="#" class="hover">积分管理</a></li>
                <li>
                    <a href="/admin/promotion/setShoppingScore.aspx">积分规则设置</a></li>
            </ul>
        </div>
    </div>
    <!--选项卡-->

    <div class="dataarea mainwidth databody">
        <div class="datalist clearfix">
            <div class="searcharea">
                <ul>
                    <li>
                        <span>会员名：</span>
                        <span>
                         
                            <input type="text" id="txtUserName" class="forminput form-control" />
                        </span>
                    </li>
                    <li>
                        <span>会员等级：</span>
                        <abbr class="formselect">
                            <Hi:MemberGradeDropDownList ID="rankList" ClientIDMode="Static" runat="server" AllowNull="true" NullToDisplay="请选择会员等级" CssClass="iselect" />
                        </abbr>
                    </li>
                    <li>
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" /></li>
                </ul>
            </div>
            <div class="functionHandleArea m_none clearfix">

                <!--结束-->
                <div class="batchHandleArea">
                    <ul>
                        <li class="batchHandleButton">
                            <span class="checkall" id="ctl00_contentHolder_grdMemberPointList_ctl01_label">
                                <input name="CheckBoxGroupTitle" type="checkbox" class="icheck" id="checkall" /></span>
                            <span>
                                <a class="btn btn-default" href="javascript:void(0);" onclick="batchEdit()">批量修改积分</a>
                            </span>
                            <span>&nbsp;&nbsp;<a href="javascript:ExportToExcel()" class="btn btn-primary">导出数据</a></span>
                        </li>
                    </ul>
                    <div class="pageHandleArea pull-right">
                        <ul>
                            <li class="paginalNum"><span>每页显示数量：</span>
                                <Hi:PageSizeDropDownList ID="PageSizeDropDownList" runat="server"></Hi:PageSizeDropDownList>
                            </li>
                        </ul>
                    </div>
                </div>

            </div>


            <table class="table table-striped">
                <tr>
                    <th style="width: 50px;"></th>
                    <th scope="col">会员名</th>
                    <th scope="col" width="160">可用积分</th>
                    <th scope="col" width="160">历史积分</th>
                    <th scope="col" width="160">会员等级</th>
                    <th scope="col" width="160">操作</th>
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


    <!--S Data Template-->
    <script id="datatmpl" type="text/html">
        {{each rows as item index}}
                <tr>
                    <td>
                        <input name="CheckBoxGroup" type="checkbox" value='{{item.UserId}}' class="icheck" /></td>
                    <td>{{item.UserName}}</td>
                    <td>{{item.Points}}</td>
                    <td>{{item.HistoryPoint}}</td>
                    <td>{{item.GradeName}}</td>
                    <td class="operation">
                        <span><a href="javascript:edit({{item.UserId}})">修改</a></span>
                        <span><a href="javascript:ToDetail({{item.UserId}},'{{item.UserName}}',{{item.Points}},{{item.HistoryPoint}})">查看明细</a> </span>
                    </td>
                </tr>
        {{/each}}
    </script>
    <!--E Data Template-->

    <input type="hidden" name="dataurl" id="dataurl" value="/admin/member/ashx/MemberPointList.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/admin/member/scripts/MemberPointList.js?v=3.31" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript">
        function getUserIds() {
            var v_str = "";
            $("input[type='checkbox'][name='CheckBoxGroup']:checked").each(function (rowIndex, rowItem) {
                v_str += $(rowItem).attr("value") + ",";
            });

            if (v_str.length == 0) {
                alert("请选择会员");
                return "";
            }
            else {
                return v_str.substring(0, v_str.length - 1);
            }
        }
        function batchEdit() {
            var userIds = getUserIds();
            if (userIds.length > 0) {
                //location.href = "BatchEditMemberPoint.aspx?userIds=" + userIds;
                DialogFrame("member/BatchEditMemberPoint.aspx?userIds=" + userIds + "&callback=ShowSuccessAndReloadData", "批量修改积分", 680, 520, null);
            }
        }
        function edit(userId) {
            DialogFrame("member/EditMemberPoint.aspx?userId=" + userId + "&callback=ShowSuccessAndReloadData", "修改积分", 580, 458, null);
        }
    </script>
</asp:Content>
