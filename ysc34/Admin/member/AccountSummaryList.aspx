<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AccountSummaryList.aspx.cs" Inherits="Hidistro.UI.Web.Admin.AccountSummaryList" %>
 
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

 

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="VIPbg fonts YFKtongji">
            <ul>
                <li>累计充值总额<strong class="colorB" id="lbtotalIncome"></strong></li>
                <li>账户总余额<strong class="colorB" id="lbtotalBalance"></strong></li>
                <li>今日消费<strong class="colorB" id="lbtodayExpenses"></strong></li>
                <li>今日充值<strong class="colorB" id="lbtodayIncome"></strong></li>
            </ul>
        </div>
        <div class="datalist clearfix" id="datalist">
            <!--搜索-->
            <div class="searcharea">
                <ul>
                    <li>
                        <span>会员名称：</span>
                        <span>
                            <input type="text" id="txtUserName" class="forminput form-control" />
                        </span>
                    </li>
                    <li>
                        <span>姓名/昵称：</span>
                        <span>
                            <input type="text" id="txtRealName" class="forminput form-control" />
                        </span>
                    </li>
                    <li>
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" /></li>
                    <li>
                        <p><a href="javascript:ExportToExcel()">导出数据</a></p>
                    </li>
                </ul>
            </div>

            <table class="table table-striped">
                <tr>
                    <th scope="col" width="180">会员名</th>
                    <th scope="col">姓名/昵称</th>
                    <th scope="col" width="160">账户总额</th>
                    <th scope="col" width="160">冻结金额</th>
                    <th scope="col" width="160">可用余额</th>
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




    <div id="addAttribute" style="display: none;">
        <div class="areacolumn clearfix">
            <div class="columnright" style="width: 500px; float: left; padding: 0;">
                <div class="formitem mt0">
                    <div style="text-align: center;" class="VIPbg fonts"><span id="spUserName"></span></div>
                    <ul>
                        <li><span class="formitemtitle" style="width: 100px;">可用余额：</span> <strong class="colorA"><span id="spUseableBalance"></span></strong>元</li>
                        <li class="mb_0">
                            <span class="formitemtitle" style="width: 100px;"><em>*</em>&nbsp;加款金额：</span>
                            <div class="input-group">
                                <span class="input-group-addon">￥</span>
                                <asp:TextBox ID="txtReCharge" ClientIDMode="static" runat="server" CssClass="form_input_s form-control" placeholder="正数为加，负数为减"></asp:TextBox>
                            </div>
                            <p id="txtReChargeTip" style="margin-left: 115px; width: 380px;"></p>
                        </li>
                        <li class="mb_0">
                            <span class="formitemtitle" style="width: 100px;">备注：</span>
                            <asp:TextBox ID="txtRemark" runat="server" ClientIDMode="Static" CssClass="form_input_l form-control" Height="90px" TextMode="MultiLine"></asp:TextBox>
                            <p id="txtRemarkTip" style="margin-left: 121px; width: 188px;"></p>
                        </li>
                    </ul>

                </div>
            </div>
        </div> 
    </div>

    <div style="display: none">
        <input type="hidden" id="currentUserId" clientidmode="Static" runat="server" />
        <input type="hidden" id="curentBalance" clientidmode="Static" runat="server" />
        <input type="button" name="btnAddBalance" id="btnAddBalance" value="添加" />
    </div>

    <!--S Data Template-->
    <script id="datatmpl" type="text/html">
        {{each rows as item index}}
                <tr>
                    <td>{{item.UserName}}</td>
                    <td>{{if item.RealName}}
                        {{item.RealName}}
                        {{else}}
                        {{item.NickName}}
                        {{/if}}
                    </td>
                    <td>{{if item.Balance}}
                        {{item.Balance.toFixed(2)}}
                        {{else}}
                        0.00
                        {{/if}}
                    </td>
                    <td>{{if item.RequestBalance}}
                        {{item.RequestBalance.toFixed(2)}}
                        {{else}}
                        0.00
                        {{/if}}</td>
                    <td>{{if item.UseableBalance}}
                        {{item.UseableBalance.toFixed(2)}}
                        {{else}}
                        0.00
                        {{/if}}</td>
                    <td>
                        <div class="operation">
                            <span><a href="javascript:ShowAddDiv('{{item.UserId}}', '{{item.UserName}}', '{{item.Balance}}', '{{item.RequestBalance}}');">加款</a></span>
                            <span>
                                <a href="javascript:ToDetail({{item.UserId}})">明细</a>
                            </span>
                        </div>
                    </td>
                </tr>
        {{/each}}
    </script>
    <!--E Data Template-->

    <input type="hidden" name="dataurl" id="dataurl" value="/admin/member/ashx/AccountSummaryList.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/admin/member/scripts/AccountSummaryList.js?v=3.31" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" language="javascript">

        function InitValidators() {
            initValid(new InputValidator('txtReCharge', 1, 11, false, null, '加款金额不能为空且必须为数字,数字长度必须<=11位！'))
            initValid(new InputValidator('txtRemark', 1, 200, true, null, '备注的字数不可以超过200字！'))

        }
        var formtype = "";

        //加款明细
        function ShowAddDiv(userId, userName, balance, freezeBalance) {

            $("#spUserName").html("给\"" + userName + "\"账户加款");
            $("#spUseableBalance").html((Number(balance) - Number(freezeBalance)).toFixed(2));
            $("#currentUserId").val(userId);
            $("#curentBalance").val(balance);
            arrytempstr = null;
            formtype = "addmoney";
            DialogShow("加款操作", "addmoney", "addAttribute", "btnAddBalance");
            InitValidators();
        }


        function validatorForm() {
            if (!PageIsValid())
                return false;
            switch (formtype) {
                case "addmoney":
                    var moneystr = $("#txtReCharge").val().replace(/\s/g, "");
                    if (moneystr == "" || moneystr == null || moneystr == "undefined") {
                        alert("加款金额不允许为空！");
                        return false;
                    }

                    if (!parseFloat(moneystr)) {
                        alert("请输入正确的金额");
                        return false;
                    }

                    setArryText("txtReCharge", moneystr);
                    setArryText("txtRemark", $("#txtRemark").val());
					$('.hishop_menu_scroll', window.parent.document).css("opacity", "1");
                    break;
            };
            return true;
        }
    </script>
</asp:Content>
