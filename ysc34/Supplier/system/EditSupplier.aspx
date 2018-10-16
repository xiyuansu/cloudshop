<%@ Page Title="" Language="C#" MasterPageFile="~/Supplier/Admin.Master" AutoEventWireup="true" CodeBehind="EditSupplier.aspx.cs" Inherits="Hidistro.UI.Web.Supplier.EditSupplier" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register Src="/admin/Ascx/ImageList.ascx" TagName="ImageList" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <style type="text/css">
        .formitem li {
            font-size: 14px;
        }

            .formitem li p {
                color: #808080;
            }

            .formitem li .msgError {
                color: red;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript">
        var autoGetOpenId = null;
        function doSubmit() {
            // 1.先执行jquery客户端验证检查其他表单项
            if (!PageIsValid())
                return false;

            var k = /^\s*$/g;

            //验证联系人
            var contactMan = $("#ctl00_contentHolder_txtContactMan").val();
            if (k.test(contactMan)) {
                $("#ctl00_contentHolder_txtContactMan").focus();
                ShowMsg("联系人不能为空！", false);
                return false;
            }

            //电话
            var tel = $("#ctl00_contentHolder_txtTel").val();
            var isPhone = /^([0-9]{3,4}-)?[0-9]{7,8}$/;//座机号规则
            var isMobbile = /^0?(13|15|18|14|17)[0-9]{9}$/g;//手机号规则
            if (!isMobbile.test(tel) && !isPhone.test(tel)) {
                $("#ctl00_contentHolder_txtTel").focus();
                ShowMsg("请输入正确的电话号码(手机或者座机)", false);
                return false;
            }

            //地区
            if ($("#regionSelectorValue").val() == "") {
                $("#regionSelectorValue").focus();
                ShowMsg("请选择一个所在区域", false);
                return false;
            }
            //if ($("#areaname").text().trim().indexOf("请选择") > -1) {
            //    $("#areaname").focus();
            //    ShowMsg("请选择所在县/区", false);
            //    return false;
            //}

            //详细地址
            var txtAddress = $("#ctl00_contentHolder_txtAddress").val();
            if (k.test(txtAddress)) {
                $("#ctl00_contentHolder_txtAddress").focus();
                ShowMsg("详细地址不能为空！", false);
                return false;
            }

            return true;
        }

        $(document).ready(function (e) {
            InitValidators();

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
        });

        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtAddress', 2, 50, false, null, '详细地址不能为空长度必须为2-50个字符'));
            initValid(new InputValidator('ctl00_contentHolder_txtContactMan', 2, 8, false, null, '联系人不能为空，长度必须为2-8个字符'));
            initValid(new InputValidator('ctl00_contentHolder_txtTel', 7, 20, false, '^0?(13|15|18|14|17)[0-9]{9}$|^\\d{2,4}-\\d{7,8}(-\\d{2,4})?', '联系电话不能为空，请输入合法的电话或者手机号码'));
        }

        var tryTimes = 0;
        function getAdminOpenId() {
            $.ajax({
                url: "/Supplier/SupplierAdmin.ashx",
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
                    }
                    else {
                        if (tryTimes < 20) {
                            getAdminOpenId();
                            tryTimes += 1;
                        }
                    }
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a href="javascript:void">基本资料</a></li>
            </ul>
        </div>

        <input type="hidden" id="txtRegionId" value="" />
        <div class="datafrom">
            <div class="formitem validator1 depot-p">
                <ul>
                    <li>
                        <h2 class="colorE">基本资料</h2>
                    </li>
                    <li><span class="formitemtitle">供应商名称：</span>
                        <asp:Literal ID="ltSupplierName" runat="server" />
                        <p style="color: #808080;">供应商名称只能由平台统一修改</p>
                    </li>
                    <li id="qtyRow"><span class="formitemtitle"><em>*</em>联系人：</span><asp:TextBox
                        runat="server" CssClass="forminput form-control" ID="txtContactMan" Width="400px" MaxLength="8" placeholder="长度必须为2-8个字符" />
                        <p id="ctl00_contentHolder_txtContactManTip">
                            联系人长度为2-8个字符
                        </p>
                    </li>
                    <li id="warningqtyRow"><span class="formitemtitle"><em>*</em>联系电话：</span><asp:TextBox
                        runat="server" CssClass="forminput form-control" ID="txtTel" Text="" Width="400px" MaxLength="20" placeholder="不能为空，合法的电话或者手机号码" />
                        <p id="ctl00_contentHolder_txtTelTip">
                            不能为空，合法的电话或者手机号码
                        </p>
                    </li>
                    <li><span class="formitemtitle"><em>*</em>所在地：</span>
                        <Hi:RegionSelector ID="dropRegion" runat="server" />
                        <p id="ctl00_contentHolder_dropRegionTip">
                            选择供应商所在区域
                        </p>
                    </li>
                    <li><span class="formitemtitle"><em>*</em>详细地址：</span>
                        <asp:TextBox runat="server" CssClass="forminput form-control" ID="txtAddress" Width="400px" MaxLength="50" placeholder="不能为空长度必须为2-50个字符" />
                        <p id="ctl00_contentHolder_txtAddressTip">
                            详细地址不能为空长度必须为2-50个字符
                        </p>
                    </li>
                    <li><span class="formitemtitle"><em></em>供应商介绍：</span>
                        <div id="notes1" style="float: left; margin-left: 250px;">
                            <Hi:Ueditor ID="editDescription" runat="server" Width="660" />
                        </div>
                        <p>可以填写供应商介绍以及相关的资质,营业执照等信息</p>
                    </li>
                    <li id="showOpenId"><span class="formitemtitle">微信OpenId：</span>
                        <asp:TextBox ID="txtWxOpenId" CssClass="forminput form-control" runat="server" ClientIDMode="Static" />&nbsp;&nbsp;<a class="a_link" href="javascript:void(0)" id="reGetOpenId" clientidmode="Static" runat="server">重新获取OpenId</a>
                        <p id="txtWxOpenIdTip">配置好微信AppId与AppSecret就可以使用微信扫描下面的二维码自动获取OpenId</p>
                    </li>
                    <li id="getOpenId" runat="server" clientidmode="Static"><span class="formitemtitle">获取OpenId：</span>
                        <asp:Image runat="server" ID="OpenIdQrCodeImg" Width="150px" />
                        <br />
                        <p id="ctl00_contentHolder_OpenIdQrCodeImgTip">请使用供应商管理员微信扫描该二维码，后续会将该供应商订单通知发送到管理员微信上</p>
                    </li>
                </ul>
                <ul class="btntf clear">
                    <li>
                        <span class="formitemtitle">&nbsp;</span>
                        <asp:Button runat="server" ID="btnAdd" Text="保 存" OnClientClick="return doSubmit();"
                            CssClass="btn btn-primary inbnt" />
                    </li>
                </ul>
            </div>
        </div>
    </div>
    <!-- start ImgPicker -->
    <uc1:ImageList ID="ImageList" runat="server" />
</asp:Content>
