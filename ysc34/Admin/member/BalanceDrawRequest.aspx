<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="BalanceDrawRequest.aspx.cs" Inherits="Hidistro.UI.Web.Admin.BalanceDrawRequest" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>


<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="Server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a href="javascript:void">银行卡付款</a></li>
                <li><a href="BalanceDrawRequestWeixin.aspx" class="">微信付款</a></li>
                <li><a href="BalanceDrawRequestAlipay.aspx" class="">支付宝付款</a></li>
            </ul>
        </div>
        <!--数据列表区域-->
        <div class="datalist clearfix">
            <!--搜索-->
            <div class="searcharea">
                <ul>
                    <li>
                        <span>用户名：</span><span>
                              <input type="text" id="txtUserName" class="forminput form-control" /></span>
                        <span style="margin-left: 11px;">选择时间段：</span>
                        <span>
                            <Hi:CalendarPanel runat="server" ClientIDMode="Static" ID="calendarStart"></Hi:CalendarPanel>
                        </span>
                        <span class="Pg_1010">至</span>
                        <span>
                            <Hi:CalendarPanel runat="server" ClientIDMode="Static" ID="calendarEnd"></Hi:CalendarPanel>
                        </span>
                    </li>
                    <li>
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" /></li>
                    <li>
                        <p><a href="javascript:Post_ExportExcel()">导出数据</a></p>
                    </li>
                </ul>
            </div>

            <!--S DataShow-->

            <table class="table table-striped">
                <thead>
                    <tr>
                        <th scope="col" style="width: 226px;">用户名</th>
                        <th scope="col" style="width: 180px;">申请时间</th>
                        <th scope="col" style="width: 120px;">提现金额</th>
                        <th class="td_left td_right_fff" scope="col">备注</th>
                        <th scope="col" style="width: 200px;">操作</th>
                    </tr>
                </thead>
                <tbody id="datashow"></tbody>
            </table>
            <!--E DataShow-->
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


    <div class="formitem validator1" id="divunlinerecharge" style="padding: 0 !important; display: none;">
        <ul class="MainUnit">
            <li>
                <span class="formitemtitle" id="UserName">用户名：<label id="lblUserName"></label></span>
            </li>
            <li>
                <span class="formitemtitle" id="BankName">开户行名称：<label id="lblBankName"></label></span>
            </li>
            <li>
                <span class="formitemtitle" id="AccountName">开户人姓名：<label id="lblAccountName"></label></span>
            </li>
            <li>
                <span class="formitemtitle" id="MerchantCode">银行账号：<label id="lblMerchantCode"></label></span>
            </li>
            <li>
                <span class="formitemtitle">同意此用户提现申请，<span style="color: red">并确认已经将提现金额转账给客户的对应账号。</span></span>
            </li>
        </ul>
    </div>

    <div class="formitem validator1" id="divrefuserequest" style="padding: 0 !important; display: none;">
        <ul class="MainUnit">
            <li class="mb_20 c-666">
                <span class="formitemtitle " id="RefuseName">拒绝<label id="lblRefuseName" style="color: blue;"></label>的提现申请</span>
            </li>
            <li>
                <span class="formitemtitle c-666" id="Reason">拒绝原因：</span>
                <asp:TextBox runat="server" ID="txtReason" ClientIDMode="Static" TextMode="MultiLine" CssClass="form-control" Rows="5" Width="380"> </asp:TextBox>
            </li>
        </ul>
    </div>
    <div style="display: none">
        <asp:HiddenField runat="server" ClientIDMode="Static" ID="BDRID" />
        <asp:HiddenField runat="server" ClientIDMode="Static" ID="ChargeType" />
    </div>

    <!--S Data Template-->
    <script id="datatmpl" type="text/html">
        {{each rows as item index}}
                <tr>
                    <td>
                        <a href="/admin/member/EditMember.aspx?userId={{item.UserId}}" class="SmallCommonTextButton">{{item.UserName}}</a>
                    </td>
                    <td>{{item.RequestTime |artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}</td>
                    <td>{{item.Amount.toFixed(2)}}</td>
                    <td>{{item.Remark}}</td>
                    <td style="width: 20%;">
                        <span class="submit_quanxuan">
                            <a href="javascript:ShowUnLineReCharge('{{item.ID}}','{{item.UserName}}','{{item.BankName}}','{{item.AccountName}}','{{item.MerchantCode}}')" class="SmallCommonTextButton">确认付款</a>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <a href="javascript:ShowRefuseRequest('{{item.ID}}','{{item.UserName}}')" class="SmallCommonTextButton">拒绝</a>
                        </span>
                    </td>
                </tr>
        {{/each}}
    </script>
    <!--E Data Template-->

    <input type="hidden" name="dataurl" id="dataurl" value="/admin/member/ashx/BalanceDrawRequest.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/admin/member/scripts/BalanceDrawRequest.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script language="javascript" type="text/javascript">

        function ShowUnLineReCharge(id, UserName, BankName, AccountName, MerchantCode) {
            var BDRID = id;

            $('#ChargeType').val('UnLineReCharge');
            $('#BDRID').val(BDRID);
            $('#lblUserName').html(UserName);
            $('#lblBankName').html(BankName);
            $('#lblAccountName').html(AccountName);
            $('#lblMerchantCode').html(MerchantCode);

            var dlg = art.dialog({
                id: "balancedrawrequest",
                title: "确认付款",
                content: $("#divunlinerecharge").html(),
                resize: true,
                fixed: true,
                button: [{
                    name: '确 认',
                    callback: function () {
                        Post_Confirm(id);
                    },
                    focus: true
                }, {
                    name: "取消"
                }]
            });
        }
        function ShowRefuseRequest(id, UserName) {
            var BDRID = id;

            $('#ChargeType').val('RefuseRequest');
            $('#BDRID').val(BDRID);
            $('#lblRefuseName').html(UserName);
            setArryText('txtReason', '');

            var dlg = art.dialog({
                id: "balancedrawrequest",
                title: "拒绝申请",
                content: $("#divrefuserequest").html(),
                init: function () {
                    $("#txtReason").val("");
                },
                resize: true,
                fixed: true,
                button: [{
                    name: '确 认',
                    callback: function () {
                        var Reason = $("#txtReason").val();
                        if (Reason.length < 1) {
                            alert("请填写拒绝申请的原因");
                            return false;
                        }
                        if (Reason.length > 50) {
                            alert("拒绝原因不可超过50字");
                            return false;
                        }
                        Post_Refuse(id, Reason);
                        $("#txtReason").val("");
                    },
                    focus: true
                }, {
                    name: "取消"
                }]
            });
        }
    </script>
</asp:Content>
