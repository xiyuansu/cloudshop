<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="SplittinDrawRequest.aspx.cs" Inherits="Hidistro.UI.Web.Admin.SplittinDrawRequest" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="Server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a href="javascript:void">银行卡结算</a></li>
                <li><a href="SplittinDrawRequestWeixin.aspx">微信结算</a></li>
                <li><a href="SplittinDrawRequestAlipay.aspx">支付宝结算</a></li>
                <li><a href="CommissionCashSetting.aspx">佣金提现设置</a></li>
                <asp:Literal runat="server" ID="litUserName" />
            </ul>
        </div>

        <!--数据列表区域-->
        <div class="datalist clearfix">
            <!--搜索-->
            <div class="searcharea">
                <ul>
                    <li>
                        <span>分销员：</span>
                        <span>
                            <input type="text" id="txtUserName" class="forminput form-control" /></span>
                    </li>
                    <li>
                        <span>选择时间段：</span>
                        <span>
                            <Hi:CalendarPanel runat="server" ID="calendarStart" ClientIDMode="Static"></Hi:CalendarPanel></span>
                        <span class="Pg_1010">至</span>
                        <span>
                            <Hi:CalendarPanel runat="server" ID="calendarEnd" ClientIDMode="Static"></Hi:CalendarPanel></span>
                    </li>
                    <li>
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" />
                    </li>
                    <li>
                        <p>

                            <a href="javascript:ExportToExcel()">导出数据</a>
                        </p>
                    </li>
                </ul>
            </div>
            <!--S DataShow-->
            <table class="table table-striped">
                <thead>
                    <tr>
                        <th scope="col" style="width: 35%;">分销员</th>
                        <th scope="col" style="width: 15%;">申请时间</th>
                        <th scope="col" style="width: 15%;">提现金额</th>
                        <th scope="col" style="width: 15%;">备注</th>
                        <th scope="col">操作</th>
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
                    <td>
                        <a href="/admin/member/EditMember.aspx?userId={{item.UserId}}">{{item.UserName}}</a>
                    </td>
                    <td>{{item.RequestDate | artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}</td>
                    <td>{{item.Amount.toFixed(2)}}</td>
                    <td>{{item.Remark}}</td>



                    <td style="text-align: center;">
                        <span class="submit_quanxuan">
                            <a href="javascript:void(0)" onclick="ShowUnLineReCharge('确认付款','{{item.JournalNumber}}','{{item.UserName}}','{{item.BankName}}','{{item.AccountName}}','{{item.MerchantCode}}')" class="SmallCommonTextButton">确认付款</a>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <a href="javascript:void(0)" onclick="ShowRefuseRequest('拒绝申请','<%#Eval("") %>{{item.JournalNumber}}','{{item.UserName}}')" class="SmallCommonTextButton">拒绝</a>
                        </span>

                    </td>
                </tr>
                {{/each}}
                  
            </script>
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
                <span class="formitemtitle " id="RefuseName">拒绝<label id="lblRefuseName" class="colorBlue"></label>的提现申请</span>
            </li>
            <li>
                <span class="formitemtitle c-666" id="Reason">拒绝原因：</span>
                <asp:TextBox runat="server" ID="txtReason" ClientIDMode="Static" TextMode="MultiLine" CssClass="form-control float" Rows="5" Width="380"> </asp:TextBox>
            </li>
        </ul>
    </div>
    <div style="display: none">
        <%--<asp:Button ID="btnRequest" runat="server" Text="确 定" CssClass="submit_sure" />--%>
        <input id="btnRequest" onclick="DrawRequest()" class="submit_sure" value="确定" />
        <asp:HiddenField runat="server" ClientIDMode="Static" ID="JournalNumber" />
        <asp:HiddenField runat="server" ClientIDMode="Static" ID="ChargeType" />
    </div>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <input type="hidden" name="dataurl" id="dataurl" value="/admin/member/ashx/SplittinDrawRequestManage.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/admin/member/scripts/SplittinDrawRequestManage.js" type="text/javascript"></script>
</asp:Content>
