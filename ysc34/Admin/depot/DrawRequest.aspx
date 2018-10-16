<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="DrawRequest.aspx.cs" Inherits="Hidistro.UI.Web.Admin.depot.DrawRequest" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script language="javascript" type="text/javascript">

        function ShowUnLineReCharge(opers, id, UserName, BankName, AccountName, MerchantCode) {
            var BDRID = id;

            if (id.toString().length < 1) {
                ShowMsg("错误的编号", false);
                return;
            }
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

            if (id.toString().length < 1) {
                ShowMsg("错误的编号", false);
                return;
            }
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
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a href="javascript:return false;">银行卡付款</a></li>
                <li><a href="DrawRequest4Ali.aspx">支付宝付款</a></li>
            </ul>
        </div>
        <div class="datalist">
            <div class="searcharea">
                <ul>
                    <li><span>用户名：</span><span>
                        <input type="text" id="txtSearchText" class="forminput form-control"  style="width: 110px;"/></span></li>
                    <li>
                        <span>门店：</span>
                        <abbr class="formselect">
                            <Hi:StoreDropDownList ID="dropStore" ClientIDMode="Static" ShowPlatform="false" runat="server" NullToDisplay="请选择门店" CssClass="iselect"></Hi:StoreDropDownList>
                        </abbr>
                    </li>
                    <li><span>申请时间：</span><span>
                        <Hi:CalendarPanel runat="server" ID="calendarStartDate" ClientIDMode="Static" Width="100"></Hi:CalendarPanel>
                    </span><span class="Pg_1010">至</span>
                        <span>
                            <Hi:CalendarPanel runat="server" ID="calendarEndDate" ClientIDMode="Static" IsEndDate="true" Width="100"></Hi:CalendarPanel>
                        </span>
                    </li>
                    <li>
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" /></li>
                    <li>
                        <p><a href="javascript:Post_ExportExcel()">导出数据</a></p>
                    </li>
                </ul>
            </div>
            <table cellspacing="0" border="0" class="table table-striped table-fixed">
                <thead>
                <tr>
                    <th width="20%">用户名</th>
                    <th width="20%">门店</th>
                    <th width="20%">申请时间</th>
                    <th width="20%">提现金额</th>
                  <%--  <th width="20%">备注</th>--%>
                    <th >操作</th>
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
                    <td>{{item.UserName}}</td>
                    <td>{{item.StoreName}}</td>
                    <td>{{item.RequestTime |artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}</td>
                    <td>{{item.Amount.toFixed(2)}}</td>
                   <%-- <td>{{item.Remark}}</td>--%>
                    <td>
                        <span class="submit_quanxuan">
                            <a href="javascript:ShowUnLineReCharge('确认付款','{{item.Id}}','{{item.UserName}}','{{item.BankName}}','{{item.AccountName}}','{{item.MerchantCode}}');" class="SmallCommonTextButton">确认付款</a>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <a href="javascript:ShowRefuseRequest('{{item.Id}}','{{item.UserName}}')" class="SmallCommonTextButton">拒绝</a>
                        </span>
                    </td>
                </tr>
        {{/each}}
       
    </script>
    <!--E Data Template-->
    <input type="hidden" name="dataurl" id="dataurl" value="/Admin/Depot/ashx/DrawRequest.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Admin/Depot/scripts/DrawRequest.js" type="text/javascript"></script>
</asp:Content>
