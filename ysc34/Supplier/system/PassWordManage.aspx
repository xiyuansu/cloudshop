<%@ Page Title="" Language="C#" MasterPageFile="~/Supplier/Admin.Master" AutoEventWireup="true" CodeBehind="PassWordManage.aspx.cs" Inherits="Hidistro.UI.Web.Supplier.PassWordManage" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <script type="text/javascript" src="/admin/js/commonvalidator.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript">
        $(function () {
            $('.title-nav li').each(function (a) {
                $(this).click(function () {
                    ///----先把所有同级栏目选中样式去掉----
                    $('.title-nav li').each(function (i) {
                        $(this).removeClass("hover");//先把所有选中样式去掉
                        $("#ctl00_contentHolder_divright" + i).hide();//原先的div隐藏掉
                    });
                    ////------------

                    $(this).addClass("hover");//为当前添加选中样式
                    $("#ctl00_contentHolder_divright" + a).show();//当前的div显示
                });
            });
        });

        //是否设置了交易密码，1有交易密码、0没交易密码
        function Isnulltradepass() {
            return ($("#istradepass").html().trim() == "0") ? true : false;//trim是去掉左右空格，方法在/admin/js/commonvalidator.js里面
        }

        //登录密码修改 验证
        function doSubmit() {
            var k = /^\s*$/g;

            //登录原始密码
            var txtoldPassword = $("#ctl00_contentHolder_txtoldPassword").val();
            if (k.test(txtoldPassword)) {
                $("#ctl00_contentHolder_txtoldPassword").focus();
                ShowMsg("当前登录密码不能为空",false);
                return false;
            }

            //是否与原来登录密码一致
            var posturl = "/Supplier/SupplierAdmin.ashx?action=IsLoginPassword&txtoldPassword=" + txtoldPassword + "&r=" + Math.random();
            var date = ajaxjson(posturl);//当前方法在/admin/script/pagevalidator.js里面
            if (date != null) {
                if (date.success == "false") {
                    $("#ctl00_contentHolder_txtoldPassword").focus();
                    ShowMsg(date.msg, false);
                    return false;
                }
            }

            //新登录密码：
            var txtnewPassword = $("#ctl00_contentHolder_txtnewPassword").val();
            if (k.test(txtnewPassword)) {
                $("#ctl00_contentHolder_txtnewPassword").focus();
                ShowMsg("登录新密码不能为空", false);
                return false;
            }

            if (txtnewPassword.length < 6 || txtnewPassword.length > 20) {
                $("#ctl00_contentHolder_txtnewPassword").focus();
                ShowMsg("登录新密码长度必须为6-20个字符！", false);
                return false;
            }

            //俩次密码是否正常
            if (txtnewPassword != $("#ctl00_contentHolder_txtnewPasswordConfirm").val()) {
                $("#ctl00_contentHolder_txtnewPasswordConfirm").focus();
                ShowMsg("新登录密码两次密码输入不一致！", false);
                return false;
            }
            return true;
        }

        //交易密码为空时 文本框验证
        function doTradePassword_Check() {
            var k = /^\s*$/g;

            //alert(!Isnulltradepass())
            //设置交易密码 需验证原来交易密码框
            if (!Isnulltradepass()) {
                //原新交易密码
                var txtoldTradePassword = $("#ctl00_contentHolder_txtoldTradePassword").val();
                if (k.test(txtoldTradePassword)) {
                    $("#ctl00_contentHolder_txtoldTradePassword").focus();
                    ShowMsg("当前交易密码不能为空！", false);
                    return false;
                }

                ////是否与原来交易密码一致
                var posturl = "/Supplier/SupplierAdmin.ashx?action=IsTradePassword&txtoldTradePassword=" + txtoldTradePassword + "&r=" + Math.random();
                var date = ajaxjson(posturl);//当前方法在/admin/js/commonvalidator.js里面
                if (date != null) {
                    if (date.success == "false") {
                        $("#ctl00_contentHolder_txtoldTradePassword").focus();
                        ShowMsg(date.msg, false);
                        return false;
                    }
                }
            }

            //确认交易密码
            var txtTradePassword = $("#ctl00_contentHolder_txtTradePassword").val();
            if (k.test(txtTradePassword)) {
                $("#ctl00_contentHolder_txtTradePassword").focus();
                ShowMsg("新交易密码不能为空！", false);
                return false;
            }

            if (txtTradePassword.length < 6 || txtTradePassword.length > 20) {
                $("#ctl00_contentHolder_txtTradePassword").focus();
                ShowMsg("用户新交易密码长度必须为6-20个字符！", false);
                return false;
            }

            //俩次密码是否正常
            if (txtTradePassword != $("#ctl00_contentHolder_txtTradePasswordConfirm").val()) {
                $("#ctl00_contentHolder_txtTradePasswordConfirm").focus();
                ShowMsg("新交易密码两次密码输入不一致！", false);
                return false;
            }

            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">

    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li id="lipass0" runat="server" class="hover"><a href="javascript:void">登录密码</a></li>
                <li id="lipass1" runat="server"><a href="javascript:void">交易密码</a></li>
            </ul>
        </div>

        <div class="datafrom">
            <div class="formitem validator1 depot-p" id="divright0" runat="server">
                <ul>
                    <li class="mb_0"><span class="formitemtitle">用户名：</span>
                        <strong style="font-size:14px;"><asp:Literal runat="server" ID="lblUserName" /></strong>
                        <p></p>
                    </li>

                    <li class="mb_0"><span class="formitemtitle"><em>*</em>当前登录密码：</span>
                        <asp:TextBox runat="server" CssClass="forminput form-control" ID="txtoldPassword" Width="250px" TextMode="Password" placeholder="不为空字符" />
                        <p id="ctl00_contentHolder_txtoldPasswordTip">
                        </p>
                    </li>
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>新登录密码：</span>
                        <asp:TextBox runat="server" CssClass="forminput form-control" ID="txtnewPassword" Width="250px" TextMode="Password" MaxLength="20" placeholder="6-20个字符" />
                        <p id="ctl00_contentHolder_txtnewPasswordTip">
                        </p>
                    </li>
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>确认新登录密码：</span>
                        <asp:TextBox runat="server" CssClass="forminput form-control" ID="txtnewPasswordConfirm" Width="250px" TextMode="Password" MaxLength="20" placeholder="6-20个字符" />
                        <p id="ctl00_contentHolder_txtnewPasswordConfirmTip">
                        </p>
                    </li>
                </ul>
                <ul class="btntf clear">
                    <li>
                        <span class="formitemtitle">&nbsp;</span>
                        <asp:Button runat="server" ID="btnSavePass" Text="保存" OnClientClick="return doSubmit();"
                        CssClass="btn btn-primary inbnt" />
                    </li>
                </ul>
            </div>

            <div class="formitem validator1 depot-p" id="divright1" style="display: none;" runat="server">
                <div style="display: none;" id="istradepass"><asp:Literal ID="ltistradepass" runat="server" /></div>
                <%--是否有交易密码，1有，其他无--%>
                <ul>
                    <li class="mb_0" style="padding-left: 150px; color: red;" id="liTradePass_Empty" runat="server" visible="false">请及时设置交易密码，否则无法提现！
                    </li>

                    <li class="mb_0" id="liTradePass_Old" runat="server"><span class="formitemtitle"><em>*</em>当前交易密码：</span>
                        <asp:TextBox runat="server" CssClass="forminput form-control" ID="txtoldTradePassword" Width="250px" TextMode="Password" placeholder="不为空字符" />
                        <p id="ctl00_contentHolder_txtoldTradePasswordTip">
                        </p>
                    </li>

                    <li class="mb_0"><span class="formitemtitle"><em>*</em>新交易密码：</span>
                        <asp:TextBox runat="server" CssClass="forminput form-control" ID="txtTradePassword" Width="250px" TextMode="Password" MaxLength="20" placeholder="6-20个字符" />
                        <p id="ctl00_contentHolder_txtTradePasswordTip">
                        </p>
                    </li>
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>确认新交易密码：</span>
                        <asp:TextBox runat="server" CssClass="forminput form-control" ID="txtTradePasswordConfirm" Width="250px" TextMode="Password" MaxLength="20" placeholder="6-20个字符" />
                        <p id="ctl00_contentHolder_txtTradePasswordConfirmTip">
                        </p>
                    </li>
                </ul>

                <ul class="btntf clear">
                    <li>
                        <span class="formitemtitle">&nbsp;</span>
                        <asp:Button runat="server" ID="btnSaveTradePass" Text="保存" OnClientClick="return doTradePassword_Check();"
                        CssClass="btn btn-primary inbnt" />
                    </li>
                </ul>
            </div>
        </div>
    </div>
</asp:Content>
