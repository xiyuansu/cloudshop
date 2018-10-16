<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="BalanceDrawRequestWeixin.aspx.cs" Inherits="Hidistro.UI.Web.Admin.BalanceDrawRequestWeixin" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>


<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="Server">
    <asp:HiddenField ID="hidIsDemoSite" runat="server" Value="0" ClientIDMode="Static" />
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="BalanceDrawRequest.aspx">银行卡付款</a></li>
                <li class="hover"><a href="javascript:void">微信付款</a></li>
                <li><a href="BalanceDrawRequestAlipay.aspx">支付宝付款</a></li>
            </ul>
        </div>

        <!--数据列表区域-->
        <div class="datalist clearfix">
            <!--搜索-->
            <div class="searcharea">
                <ul>
                    <li>
                        <span>用户名：</span><span><input type="text" id="txtUserName" class="forminput form-control" /></span>
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

            <!--结束-->

            <div class="functionHandleArea clearfix">
                <div class="batchHandleArea">
                    <div class="checkall">
                        <input name="CheckBoxGroupTitle" type="checkbox" class="icheck" id="checkall" />
                    </div>
                    <span class="btn btn-default">
                        <a href="javascript:DrawRequestWeixin();">微信批量付款</a></span>
                    <!--分页功能-->
                    <div class="paginalNum">
                        <span>每页显示数量：</span>
                        <Hi:PageSizeDropDownList ID="PageSizeDropDownList" runat="server"></Hi:PageSizeDropDownList>
                    </div>
                    <!--结束-->
                </div>
            </div>

            <!--S DataShow-->

            <table class="table table-striped">
                <thead>
                    <tr>
                        <th scope="col" style="width: 50px;">&nbsp;</th>
                        <th scope="col" style="width: 180px;">用户名</th>
                        <th scope="col" style="width: 180px;">申请时间</th>
                        <th scope="col" style="width: 120px;">提现金额</th>
                        <th class="td_left td_right_fff" scope="col">备注</th>
                        <th scope="col" style="width: 70px;">状态</th>
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
                <span class="formitemtitle" id="UserName">同意<label id="lblUserName" style="color: blue;"></label>的提现申请</span>
            </li>
            <li>
                <span class="formitemtitle"><span style="color: red">点击“确认”后，系统将自动打款，</span>并且会员提现状态改为成功。</span>
            </li>
        </ul>
    </div>
    <div class="formitem validator1" id="divrefuserequest" style="padding: 0 !important; display: none;">
        <ul class="MainUnit">
            <li>
                <span class="formitemtitle " id="RefuseName">拒绝<label id="lblRefuseName" style="color: blue;"></label>的提现申请</span>
            </li>
            <li>
                <span class="formitemtitle" id="Reason">拒绝原因：</span>
                <asp:TextBox runat="server" ID="txtReason" ClientIDMode="Static" CssClass="form-control" TextMode="MultiLine" Rows="5" Width="380"> </asp:TextBox>
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
                        <input type="checkbox" value="{{item.ID}}" uname="{{item.UserName}}" name="CheckBoxGroup" class="icheck" />
                        {{/if}}
                    </td>
                    <td>
                        <a href="/admin/member/EditMember.aspx?userId={{item.UserId}}" class="SmallCommonTextButton">{{item.UserName}}</a>
                    </td>
                    <td>{{item.RequestTime |artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}</td>
                    <td>{{item.Amount.toFixed(2)}}</td>
                    <td>{{item.Remark}}</td>
                    <td>{{item.RequestStateText}}
                        {{if item.RequestState==3}}
                        <i class='glyphicon glyphicon-question-sign' data-container='body' style='cursor: pointer' data-toggle='popover' data-placement='left' title='{{item.RequestError}}'></i>
                        {{/if}}
                    </td>
                    <td>
                        <span class="submit_quanxuan">
                            <a href="javascript:ShowUnLineReCharge('{{item.ID}}','{{item.UserName}}')" class="SmallCommonTextButton">确认付款</a>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <a href="javascript:ShowRefuseRequest('{{item.ID}}','{{item.UserName}}')" class="SmallCommonTextButton">拒绝</a>
                        </span>
                    </td>
                </tr>
        {{/each}}
    </script>
    <!--E Data Template-->

    <input type="hidden" name="dataurl" id="dataurl" value="/admin/member/ashx/BalanceDrawRequestWeixin.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/admin/member/scripts/BalanceDrawRequestWeixin.js?v=3.2" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script language="javascript" type="text/javascript">

        function DrawRequestWeixin() {
            if ($("#hidIsDemoSite").val() == "1") {
                ShowMsg("演示站点，无法真实付款", false);
                return;
            }
            var ids = "", UserName = "", CodeNum = 0;
            $('input[name="CheckBoxGroup"]:checked').each(function () {
                ids += $(this).val() + ",";
                UserName += $(this).attr("uname") + ";";
                CodeNum++;
            })
            if (ids == "") {
                alert("请至少选择一项"); return false;
            }
            if (UserName.length > 50)
                UserName = UserName.substr(0, 50) + "...";
            UserName += "(" + CodeNum + "个会员)";
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
            if ($("#hidIsDemoSite").val() == "1") {
                ShowMsg("演示站点，无法真实付款", false);
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

            $('#ChargeType').val('RefuseRequest');
            $('#BDRID').val(id);
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
