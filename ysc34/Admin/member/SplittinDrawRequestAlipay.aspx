<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="SplittinDrawRequestAlipay.aspx.cs" Inherits="Hidistro.UI.Web.Admin.SplittinDrawRequestAlipay" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="Server">
    <asp:HiddenField ID="hidIsDemoSite" runat="server" Value="0" ClientIDMode="Static" />
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="SplittinDrawRequest.aspx">银行卡结算</a></li>
                <li><a href="SplittinDrawRequestWeixin.aspx">微信结算</a></li>
                <li class="hover"><a href="javascript:void">支付宝结算</a></li>
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
                        <span style="margin-left: 11px;">分销员：</span>
                        <span>
                            <input type="text" id="txtUserName" class="forminput form-control" /></span>
                    </li>
                    <li>
                        <span>选择时间段：</span>
                        <span>
                            <Hi:CalendarPanel runat="server" ID="calendarStart" ClientIDMode="Static"></Hi:CalendarPanel></span>
                        <span class="Pg_1010">至</span>
                        <span>
                            <Hi:CalendarPanel runat="server" ID="calendarEnd" IsEndDate="true" ClientIDMode="Static"></Hi:CalendarPanel></span>
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

            <!--结束-->

            <div class="functionHandleArea clearfix">
                <div class="batchHandleArea">
                    <div class="checkall">
                        <input name="CheckBoxGroupTitle" type="checkbox" class="icheck" id="checkall" />
                    </div>
                    <span class="btn btn-default">
                        <a href="javascript:void(0);" onclick="DrawRequestAlipay()">支付宝批量付款</a></span>
                    <!--分页功能-->
                    <div class="paginalNum">
                        <ul>
                            <li class="paginalNum"><span>每页显示数量：</span><Hi:PageSizeDropDownList ID="PageSizeDropDownList" runat="server"></Hi:PageSizeDropDownList>
                            </li>
                        </ul>
                    </div>
                    <!--结束-->
                </div>
            </div>

            <!--S DataShow-->
            <table class="table table-striped">
                <thead>
                    <tr>
                        <th style="width: 5%"></th>
                        <th scope="col" style="width: 15%;">分销员</th>
                        <th scope="col" style="width: 15%;">申请时间</th>
                        <th scope="col" style="width: 12%;">提现金额</th>
                        <th scope="col" style="width: 10%;">操作人</th>
                        <th scope="col" style="width: 20%;">备注</th>
                        <th scope="col" style="width: 12%;">状态</th>
                        <th scope="col">操作</th>
                    </tr>
                </thead>
                <tbody id="datashow"></tbody>
            </table>

            <div class="blank12 clearfix"></div>
            <script id="datatmpl" type="text/html">
                {{each rows as item index}}
                <tr>
                    <td>{{if item.RequestState!=2}} 
                       <span class="icheck">
                           <input type="checkbox" value="{{item.JournalNumber}}" uname="{{item.UserName}}" name="CheckBoxGroup" />
                       </span>
                        {{/if}}
                    </td>

                    <td>
                        <a href="/admin/member/EditMember.aspx?userId={{item.UserId}}">{{item.UserName}}</a>
                    </td>
                    <td>{{item.RequestDate | artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}</td>
                    <td>{{item.Amount.toFixed(2)}}</td>
                    <td>{{item.ManagerUxerName}}</td>
                    <td>{{item.Remark}}</td>
                    <td>{{item.RequestStateStr}}{{=item.RequestStateImgStr}}</td>
                    <td>{{if item.RequestState!=2}}                     
                        <a href="javascript:void(0)" onclick="ShowUnLineReCharge('确认付款','{{item.JournalNumber}}','{{item.UserName}}')" class="SmallCommonTextButton">确认付款</a>
                        &nbsp;&nbsp; 
                              <a href='javascript:void(0)' onclick="ShowRefuseRequest('拒绝申请','{{item.JournalNumber}}','{{item.UserName}}')" class="SmallCommonTextButton">拒绝</a>
                        {{else}}
                        <a href="javascript:void(0)" onclick="ShowCancelReCharge('取消付款','{{item.JournalNumber}}','{{item.UserName}}')" class="SmallCommonTextButton">取消付款</a>

                        {{/if}}
                    </td>
                </tr>
                {{/each}}
                  
            </script>
            <!--E DataShow-->
            <div class="blank12 clearfix"></div>
        </div>
        <!--数据列表底部功能区域-->
        <div class="bottomBatchHandleArea clearfix">
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
                <span class="formitemtitle" style="width: 400px;"><span style="color: red">点击“确认”后，系统将跳转至支付宝打款平台（请不要屏蔽打开的新窗口）</span>，并且会员提现状态改为付款中，支付宝完成付款后自动删除该条记录（并记录在提现记录中）或状态改为付款失败（如果支付宝正常打款时失败）。</span>
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
                <asp:TextBox runat="server" ID="txtReason" ClientIDMode="Static" TextMode="MultiLine" Rows="5" Width="380"> </asp:TextBox>
            </li>
        </ul>
    </div>
    <div class="formitem validator1" id="divcancelrecharge" style="padding: 0 !important; display: none;">
        <ul class="MainUnit">
            <li>
                <span class="formitemtitle" id="cancelUserName">取消<label id="lblcancelUserName" style="color: blue;"></label>的付款</span>
            </li>
            <li>
                <span class="formitemtitle"><span style="color: red">点击“确认”后，系统将自动将会员提现状态改为付款失败。<span></span>
            </li>
        </ul>
    </div>
    <div style="display: none">
        <input id="btnRequest" onclick="DrawRequest()" class="submit_sure" value="确 定" />
        <input id="btnMustRequest" onclick="MoreDrawRequest()" class="submit_sure" value="确 定" />
        <asp:HiddenField runat="server" ClientIDMode="Static" ID="JournalNumber" />
        <asp:HiddenField runat="server" ClientIDMode="Static" ID="ChargeType" />
    </div>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">

    <input type="hidden" name="dataurl" id="dataurl" value="/admin/member/ashx/SplittinDrawRequestAlipayManage.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/admin/member/scripts/SplittinDrawRequestAlipayManage.js?v=3.2" type="text/javascript"></script>
</asp:Content>
