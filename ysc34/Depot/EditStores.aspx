<%@ Page Title="" Language="C#" MasterPageFile="~/Depot/Admin.Master" AutoEventWireup="true" CodeBehind="EditStores.aspx.cs" Inherits="Hidistro.UI.Web.Depot.EditStores" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register Src="/admin/Ascx/ImageList.ascx" TagName="ImageList" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript">
        var autoGetOpenId = null;
        $(document).ready(function (e) {
            var RegionScop = $("#txtRegionScop").val();
            var RegionScopName = $("#txtRegionScopName").val();
            if (RegionScop != "" && RegionScopName != "") {
                var RegionScopArr = RegionScop.split(',');
                var RegionScopNameArr = RegionScopName.split(',');
                for (var i = 0; i < RegionScopArr.length; i++) {
                    var RegionId = RegionScopArr[i];
                    var RegionName = RegionScopNameArr[i];
                    //区域信息html
                    var scopHTML = "<tr id=\"row_{0}\"><td>{1}</td></tr>".format(RegionId, RegionName);
                    $(scopHTML).insertAfter($("#scoplist tr:last"));
                }
            }
            if ($("#txtWxOpenId").val() == "") {
                autoGetOpenId = true
                getAdminOpenId();
            }

            $("#reGetOpenId").click(function (e) {
                if ($(this).text() == "重新获取OpenId") {
                    $(this).text("隐藏二维码");
                    $("#getOpenId").show();

                    if (autoGetOpenId == null) {
                        getAdminOpenId();
                        autoGetOpenId = true;
                    }
                }
                else {
                    $("#getOpenId").hide();
                    $(this).text("重新获取OpenId");
                }
            })

            InitValidators();
        });

        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtStoreUserName', 2, 20, false, null, '用户名长度不能超过2-20个字符'));
            initValid(new InputValidator('ctl00_contentHolder_txtStoresName', 2, 20, false, null, '门店名称长度必须为2-20个字符'));
            initValid(new InputValidator('ctl00_contentHolder_txtAddress', 2, 50, false, null, '详细地址不能为空长度必须为2-50个字符'));
            initValid(new InputValidator('ctl00_contentHolder_txtContactMan', 2, 10, false, null, '联系人不能为空，长度必须为2-20个字符'));
            initValid(new InputValidator('ctl00_contentHolder_txtTel', 7, 20, false, null, '联系电话不能为空，请输入合法的电话或者手机号码'));
            //initValid(new InputValidator('ctl00_contentHolder_txtServeRadius', 1, 5, false, '^(?!0)(?:[0-9]{1,4}|10000)$', '服务半径不能为空，在1-10000之间'));
            //initValid(new InputValidator('ctl00_contentHolder_txtServeRadius', 1, 10, true, '((0+(\\.[0-9]{1,3}))|[1-9]\\d*(\\.\\d{1,3})?)', '服务半径不能为空，在大于0至10000之间'))
            //appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtServeRadius', 0, 10000, '服务半径不能为空，在大于0至10000之间'));
        }
        var tryTimes = 0;
        function getAdminOpenId() {
            $.ajax({
                url: "/Admin/Admin.ashx",
                type: 'post',
                dataType: 'json',
                data: {
                    action: "GetAdminOpenId"
                },
                timeout: 30000,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    if (tryTimes < 20) {
                        getAdminOpenId();
                        tryTimes += 1;
                    }
                },
                success: function (data, textStatus) {
                    if (data.Status == "1") {
                        $("#txtWxOpenId").val(data.OpenId);
                    } else {
                        if (tryTimes < 20) {
                            getAdminOpenId();
                            tryTimes += 1;
                        }
                    }
                }
            });
        }

        String.prototype.format = function () {
            var args = arguments;
            return this.replace(/\{(\d+)\}/g, function (s, i) {
                return args[i];
            });
        }
        function doSubmit() {
            // 1.先执行jquery客户端验证检查其他表单项
            if (!PageIsValid())
                return false;
            if ($("#ctl00_contentHolder_txtUserPwd").val() != $("#ctl00_contentHolder_txtUserRePwd").val()) {
                alert("两次密码输入不一致！");
                return false;
            }

            var tel = $("#ctl00_contentHolder_txtTel").val();
            var isPhone = /^([0-9]{3,4}-)?[0-9]{7,8}$/;
            var isMobbile = /^0?(13|15|18|14|17)[0-9]{9}$/g;
            if (!isMobbile.test(tel) && !isPhone.test(tel)) {
                alert("请输入正确的电话号码(手机或者座机)");
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
                <li><a href="javascript:void" class="hover">编辑门店</a></li>
            </ul>
        </div>

        <input type="hidden" id="txtRegionId" value="" />
        <div class="datafrom">
            <div class="formitem validator1 depot-p">
                <ul>
                    <li>
                        <h2 class="colorE">帐户信息</h2>
                    </li>
                    <li class="clearfix"><span class="formitemtitle"><em>*</em>用户名：</span>
                        <asp:TextBox Width="160px" runat="server" CssClass="forminput form-control" ID="txtUserName" Enabled="false" />
                        <p id="ctl00_contentHolder_txtUserNameTip">
                            限定在20个字符
                        </p>
                    </li>
                    <li><span class="formitemtitle">密码：</span>
                        <asp:TextBox Width="160px" runat="server" CssClass="forminput form-control" ID="txtUserPwd" />
                        <p id="ctl00_contentHolder_txtUserPwdTip">
                            密码不修改请留空
                        </p>
                    </li>
                    <li><span class="formitemtitle">确认密码：</span>
                        <asp:TextBox Width="160px" runat="server" CssClass="forminput form-control" ID="txtUserRePwd" />
                        <p id="ctl00_contentHolder_txtUserRePwdTip">
                            两次密码必须一致
                        </p>
                    </li>
                    <li>
                        <h2 class="colorE">门店信息</h2>
                    </li>
                    <li><span class="formitemtitle">门店名称：</span>
                        <asp:TextBox runat="server" ID="txtStoresName" Width="400px" CssClass="forminput form-control" />
                        <p id="ctl00_contentHolder_txtStoresNameTip">
                            门店名称长度不能超过50个字符
                        </p>
                    </li>
                    <li><span class="formitemtitle">所在区域：</span>
                        <Hi:RegionSelector ID="dropRegion" runat="server" />
                        <p id="ctl00_contentHolder_dropRegionTip">
                            请选择门店所在区域
                        </p>
                    </li>
                    <li><span class="formitemtitle"><em>*</em>详细地址：</span>
                        <asp:TextBox runat="server" CssClass="forminput form-control" ID="txtAddress" Width="400px" />
                        <p id="ctl00_contentHolder_txtAddressTip">
                            详细地址不能为空长度必须为2-50个字符
                        </p>
                    </li>
                    <li id="qtyRow"><span class="formitemtitle"><em>*</em>联系人：</span><asp:TextBox
                        runat="server" CssClass="forminput form-control" ID="txtContactMan" Width="400px" />
                        <p id="ctl00_contentHolder_txtContactManTip">
                            联系人长度为8个字符
                        </p>
                    </li>
                    <li id="warningqtyRow"><span class="formitemtitle"><em>*</em>联系电话：</span><asp:TextBox
                        runat="server" CssClass="forminput form-control" ID="txtTel" Text="" Width="400px" />
                        <p id="ctl00_contentHolder_txtTelTip">
                            请输入正确的联系电话
                        </p>
                    </li>
                    <li><span class="formitemtitle"><em></em>门店介绍：</span>
                        <div id="notes1" style="float: left;">
                            <Hi:Ueditor ID="editDescription" runat="server" Width="660" />
                        </div>
                        <p>可以填写门店介绍以及相关的资质,营业执照等信息</p>
                    </li>
                    <li class="clearfix" id="liStoreTag" runat="server"><span class="formitemtitle">门店标签：</span>
                        <asp:Label ID="lblStoreTag" runat="server" style="font-size:14px;line-height: 28px;"></asp:Label>
                    </li>
                    <li class="clearfix"><span class="formitemtitle">配送方式：</span>
                        <asp:Label ID="lblDeliveMode" runat="server" style="font-size:14px;line-height: 28px;"></asp:Label>
                    </li>
                    <li class="clearfix" id="liServeRadius" runat="server"><span class="formitemtitle">配送半径：</span>
                        <asp:Label ID="lblServeRadius" runat="server" style="font-size:14px;line-height: 28px;"></asp:Label>
                    </li>
                    <li class="clearfix" id="liStoreFreight" runat="server"><span class="formitemtitle">配送费：</span>
                        <asp:Label ID="lblStoreFreight" runat="server" style="font-size:14px;line-height: 28px;"></asp:Label>
                    </li>
                    <li class="clearfix" id="liMinOrderPrice" runat="server"><span class="formitemtitle">起送价：</span>
                        <asp:Label ID="lblMinOrderPrice" runat="server" style="font-size:14px;line-height: 28px;"></asp:Label>
                    </li>
                    <li class="clearfix"><span class="formitemtitle">营业时间：</span>
                        <asp:Label ID="lblStoreOpenTime" runat="server" style="font-size:14px;line-height: 28px;"></asp:Label>
                    </li>
                    <li id="showOpenId"><span class="formitemtitle">微信OpenId：</span>
                        <asp:TextBox ID="txtWxOpenId" CssClass="forminput form-control" runat="server" ClientIDMode="Static" />&nbsp;&nbsp;<a href="javascript:void(0)" id="reGetOpenId" clientidmode="Static" runat="server">重新获取OpenId</a>
                        <p id="txtWxOpenIdTip">配置好微信AppId与AppSecret就可以使用微信扫描下面的二维码自动获取OpenId</p>
                    </li>
                    <li id="getOpenId" runat="server" clientidmode="Static"><span class="formitemtitle">获取OpenId：</span>
                        <asp:Image runat="server" ID="OpenIdQrCodeImg" Width="150px" />
                        <br />
                        <p id="ctl00_contentHolder_OpenIdQrCodeImgTip">请使用门店管理员微信扫描该二维码，后续会将该门店订单通知发送到管理员微信上</p>
                    </li>
                    <li id="liScopTitle" runat="server">
                        <h2 class="colorE">配送范围</h2>
                    </li>
                    <li>
                        <asp:HiddenField ID="txtRegionScop" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="txtRegionScopName" runat="server" ClientIDMode="Static" />
                    </li>
                    <li id="liScop" runat="server">
                        <div class="datalist clearfix" style="width: 500px;">
                            <table cellpadding="0" cellspacing="0" style="width: 100%; border-collapse: collapse;" id="scoplist">
                                <tr class="table_title">
                                    <th class="td_right td_left" scope="col" width="66%">配送范围</th>
                                </tr>

                            </table>
                        </div>
                    </li>
                  <%--  <li>
                        <h2 class="colorE">营业设置</h2>
                    </li>
                    <li>
                        <span>营业状态：</span>
                        <span>
                            <asp:RadioButtonList ID="radBusinessState" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="1" Text="正常营业"></asp:ListItem>
                                <asp:ListItem Value="0" Text="暂停营业"></asp:ListItem>
                            </asp:RadioButtonList>
                        </span>
                    </li>--%>
                </ul>
                <ul class="btntf Pa_198 clear">
                    <asp:Button runat="server" ID="btnAdd" Text="保 存" OnClientClick="return doSubmit();"
                        CssClass="btn btn-primary inbnt" />
                </ul>
            </div>
        </div>
    </div>
         <!-- start ImgPicker -->
    <uc1:ImageList ID="ImageList" runat="server" />
</asp:Content>
