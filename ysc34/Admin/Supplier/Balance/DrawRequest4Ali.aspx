<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="DrawRequest4Ali.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Supplier.Balance.DrawRequest4Ali" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script language="javascript" type="text/javascript">

        function DrawRequestAlipay() {
            var ids = "", UserName = "", CodeNum = 0;
            $('input[name="CheckBoxGroup"]:checked').each(function () {
                ids += $(this).val() + ",";
                UserName += $(this).attr("uname") + ";";
                CodeNum++;
            })
            if (ids == "") {
                alert("请至少选择一项");
                return;
            }
            if (UserName.length > 50)
                UserName = UserName.substr(0, 50) + "...";
            UserName += "(" + CodeNum + "个供应商)";
            $('#BDRID').val(ids);
            $('#lblUserName').html(UserName);

            var dlg = art.dialog({
                id: "balancedrawrequest",
                title: "确认付款",
                content: $("#divunlinerecharge").html(),
                resize: true,
                fixed: true,
                button: [{
                    name: '确 认',
                    callback: function () {
                        Post_Confirm(ids);
                    },
                    focus: true
                }, {
                    name: "取消"
                }]
            });

            return false;
        }
        function ShowUnLineReCharge(id, UserName) {

            if (id.toString().length < 1) {
                ShowMsg("错误的编号", false);
                return;
            }
            $('#ChargeType').val('UnLineReCharge');
            $('#BDRID').val(id);
            $('#lblUserName').html(UserName);

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
            if (id.toString().length < 1) {
                ShowMsg("错误的编号", false);
                return;
            }
            $('#ChargeType').val('RefuseRequest');
            $('#BDRID').val(id);
            $('#lblRefuseName').html(UserName);

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
        function ShowCancelReCharge(id, UserName) {

            if (id.toString().length < 1) {
                ShowMsg("错误的编号", false);
                return;
            }

            $('#ChargeType').val('CancelReCharge');
            $('#BDRID').val(id);
            $('#lblcancelUserName').html(UserName);

            var dlg = art.dialog({
                id: "balancedrawrequest",
                title: "取消付款",
                content: $("#divcancelrecharge").html(),
                resize: true,
                fixed: true,
                button: [{
                    name: '确 认',
                    callback: function () {
                        Post_Cancel(id);
                    },
                    focus: true
                }, {
                    name: "取消"
                }]
            });
        }

        function OpenNewWin(IDList) {
            window.open("/Admin/member/BalanceDrawRequestOnLine.aspx?Ids=" + IDList + "&Type=balance4Supplier", "_blank");
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="DrawRequest.aspx">银行卡付款</a></li>
                <li class="hover"><a href="javascript:return false;">支付宝付款</a></li>
            </ul>
        </div>
        <div class="datalist">
            <div class="searcharea">
                <ul>
                    <li><span>用户名：</span><span><input type="text" id="txtSearchText" class="forminput form-control" style="width:110px;" /></span></li>
                    <li>
                        <span>供应商：</span>
                        <abbr class="formselect">
                            <Hi:SuplierDropDownList ID="dropSuplier" ClientIDMode="Static" runat="server" NullToDisplay="请选择供应商" CssClass="iselect"></Hi:SuplierDropDownList>
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
            <div class="functionHandleArea clearfix">
                <div class="batchHandleArea">
                    <div class="checkall">
                        <input name="CheckBoxGroupTitle" type="checkbox" class="icheck" id="checkall" />
                    </div>
                    <span class="btn btn-default">
                        <a href="javascript:void(0);" onclick="DrawRequestAlipay()">支付宝批量付款</a></span>
                    <!--分页功能-->
                    <div class="paginalNum">
                        <span>每页显示数量：</span>
                        <Hi:PageSizeDropDownList ID="PageSizeDropDownList" runat="server"></Hi:PageSizeDropDownList>
                    </div>
                    <!--结束-->
                </div>
            </div>
            <table cellspacing="0" border="0" class="table table-striped table-fixed">
                <thead>
                    <tr>
                        <th width="5%"></th>
                        <th width="10%">用户名</th>
                        <th width="10%">供应商</th>
                        <th width="10%">申请时间</th>
                        <th width="10%">提现金额</th>
                        <th width="20%">备注</th>
                        <th width="10%">状态</th>
                        <th>操作</th>
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
                <span class="formitemtitle" id="UserName">同意<label id="lblUserName" style="color: blue;"></label>的提现申请</span>
            </li>
            <li>
                <span class="formitemtitle" style="width: 400px;"><span style="color: red">点击“确认”后，系统将跳转至支付宝打款平台（请不要屏蔽打开的新窗口）</span> ，并且提现状态改为付款中，支付宝完成付款后自动删除该条记录（并记录在提现记录中）或状态改为付款失败（如果支付宝正常打款时失败）。</span>
            </li>
        </ul>
    </div>
    <div class="formitem validator1" id="divrefuserequest" style="padding: 0 !important; display: none;">
        <ul class="MainUnit">
            <li>
                <span class="formitemtitle " id="RefuseName">拒绝<label id="lblRefuseName" style="color: blue;"></label>的提现申请</span>
            </li>
            <li>
                <span class="formitemtitle" id="Reason">拒绝原因：</span><br />
                <asp:TextBox runat="server" ID="txtReason" ClientIDMode="Static" TextMode="MultiLine" CssClass="form-control" Rows="5" Width="380"> </asp:TextBox>
            </li>
        </ul>
    </div>
    <div class="formitem validator1" id="divcancelrecharge" style="padding: 0 !important; display: none;">
        <ul class="MainUnit">
            <li>
                <span class="formitemtitle" id="cancelUserName">取消<label id="lblcancelUserName" style="color: blue;"></label>的付款</span>
            </li>
            <li>
                <span class="formitemtitle">
                    <span style="color: red">点击“确认”后，系统将自动将会员提现状态改为付款失败。</span>
                </span>
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
                    <td>{{if item.RequestState==2}}
                        &nbsp;
                        {{else}}
                        <input type="checkbox" name="CheckBoxGroup" value="{{item.ID}}" uname="{{item.UserName}}" class="icheck" />
                        {{/if}}
                    </td>
                    <td>{{item.UserName}}</td>
                    <td>{{item.SupplierName}}</td>
                    <td>{{item.RequestTime |artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}</td>
                    <td>{{item.Amount.toFixed(2)}}</td>
                    <td>{{item.Remark}}</td>
                    <td>{{item.RequestStateText}}
                        {{if item.RequestState==3}}
                        <i class='glyphicon glyphicon-question-sign' data-container='body' style='cursor: pointer' data-toggle='popover' data-placement='left' title='{{item.RequestError}}'></i>
                        {{/if}}
                    </td>
                    <td>
                        <span class="submit_quanxuan">{{if item.RequestState==2}}
                                <a href="javascript:ShowCancelReCharge('{{item.ID}}','{{item.UserName}}')" class="SmallCommonTextButton">取消付款</a>
                            &nbsp;
                                {{/if}}
                                {{if item.RequestState!=2}}
                                <a href="javascript:ShowUnLineReCharge('{{item.ID}}','{{item.UserName}}')" class="SmallCommonTextButton">确认付款</a>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <a href="javascript:ShowRefuseRequest('{{item.ID}}','{{item.UserName}}')" class="SmallCommonTextButton">拒绝</a>
                            {{/if}}
                        </span>
                    </td>
                </tr>
        {{/each}}
     
    </script>
    <!--E Data Template-->
    <input type="hidden" name="dataurl" id="dataurl" value="/Admin/Supplier/Balance/ashx/DrawRequest4Ali.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Admin/Supplier/Balance/scripts/DrawRequest4Ali.js" type="text/javascript"></script>
</asp:Content>
