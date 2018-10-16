<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/Admin/Admin.Master" CodeBehind="AddSupplier.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Supplier.AddSupplier" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register Src="/admin/Ascx/ImageList.ascx" TagName="ImageList" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <script type="text/javascript" src="/admin/js/commonvalidator.js"></script>
        <script type="text/javascript" src="/utility/jquery.hishopUpload.js" ></script>
        <script type="text/javascript" src="/utility/jquery.form.js"></script>
    <link id="cssLink" rel="stylesheet" href="../css/style.css" type="text/css" media="screen" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript">
        // 初始化图片上传控件
        function initImageUpload() {
            var imgSrc = '<%=hidOldImages.Value%>';
            $('#imageContainer span[name="articleImage"]').hishopUpload(
                           {
                               title: '',
                               url: "/admin/UploadHandler.ashx?action=newupload",
                               imageDescript: '',
                               displayImgSrc: imgSrc,
                               imgFieldName: "articleImage",
                               defaultImg: '',
                               pictureSize: '180*65',
                               imagesCount: 1,
                               dataWidth: 9
                           });

        }

        //获取上传成功后的图片路径
        function getUploadImages() {
            if (!PageIsValid()) return false;
            var srcImg = $('#imageContainer span[name="articleImage"]').hishopUpload("getImgSrc");
            $("#<%=hidUploadImages.ClientID%>").val(srcImg);
           
            return true;
        }
        $(document).ready(function () { InitValidators(); initImageUpload(); });
        function doSubmit() {

            getUploadImages();
            // 1.先执行jquery客户端验证检查其他表单项
            if (!PageIsValid())
                return false;

            var k = /^\s*$/g;
            //验证供应商名
            var supplierName = $("#ctl00_contentHolder_txtSupplierName").val();
            if (k.test(supplierName)) {
                $("#ctl00_contentHolder_txtSupplierName").focus();
                ShowMsg("供应商名称不能为空！", false);
                return false;
            }

            //检测供应商名称是否使用
            if (CheckName("ctl00_contentHolder_txtSupplierName", "CheckSupplierName", "SupplierName")==false) {
                return false;
            }

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

            //检测用户名是否使用
            if (CheckName("ctl00_contentHolder_txtUserName", "CheckUserName", "UserName")==false) {
                return false;
            }

            //俩次密码是否正常
            if ($("#ctl00_contentHolder_txtUserPwd").val() != $("#ctl00_contentHolder_txtUserRePwd").val()) {
                $("#ctl00_contentHolder_txtUserRePwd").focus();
                ShowMsg("两次密码输入不一致！", false);
                return false;
            }

            return true;
        }

        $(document).ready(function (e) {
            InitValidators();
        });

        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtUserName', 2, 20, false, null, '用户名长度不能超过2-20个字符'));
            initValid(new InputValidator('ctl00_contentHolder_txtUserPwd', 6, 20, false, null, '用户登录密码长度必须为6-20个字符'));
            initValid(new InputValidator('ctl00_contentHolder_txtSupplierName', 2, 20, false, null, '供应商名称长度必须为2-20个字符'));
            initValid(new InputValidator('ctl00_contentHolder_txtAddress', 2, 50, false, null, '详细地址不能为空长度必须为2-50个字符'));
            initValid(new InputValidator('ctl00_contentHolder_txtContactMan', 2, 8, false, null, '联系人不能为空，长度必须为2-8个字符'));
            initValid(new InputValidator('ctl00_contentHolder_txtTel', 7, 20, false, "^0?(13|15|18|14|17)[0-9]{9}$|^\\d{2,4}-\\d{7,8}(-\\d{2,4})?", '联系电话不能为空，请输入合法的电话或者手机号码'));
        }


        //检测供应商名称 或 用户名是否存在
        //byId检测的id号，CheckAction请求的action方法，CheckName检测数据库表名称
        function CheckName(byId, CheckAction, CheckName) {
            var txtName = $("#" + byId).val();
            var posturl = "/Admin/Supplier/SupplierHandler.ashx?action=" + CheckAction + "&" + CheckName + "=" + escape(txtName) + "&r=" + Math.random();
            var date = ajaxjson(posturl);//当前方法在/admin/js/commonvalidator.js里面

            if (date != null) {
                if (date.success == "false") {
                    $("#" + byId).focus();
                    ShowMsg(date.msg, false);
                    return false;
                }
            }

            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">

        <div class="title">
            <ul class="title-nav">
                <li><a id="all" href="SupplierList.aspx" title="供应商管理列表">管理</a></li>
                <li class="hover"><a>添加</a></li>
            </ul>
        </div>
        <input type="hidden" id="txtRegionId" value="" /><%--所有地Id，区域表(Hishop_Regions)--%>

        <div class="datafrom">
            <div class="formitem ">
                <ul>
                    <li>
                        <h2 class="colorE">供应商基本信息</h2>
                    </li>
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>供应商名称：</span>
                        <Hi:TrimTextBox runat="server" ID="txtSupplierName" Width="350px" CssClass="form_input_m form-control" MaxLength="20" placeholder="不超过20个字符"></Hi:TrimTextBox>
                        <p style="color:#808080;">供应商名称只能由平台统一修改</p>
                        <p id="ctl00_contentHolder_txtSupplierNameTip">
                        </p>
                    </li>
                        <li class="mb_0">
                        <span class="formitemtitle">供应商图标：</span>
                        <div id="imageContainer">
                            <span name="articleImage" class="imgbox"></span>
                            <asp:HiddenField ID="hidUploadImages" runat="server" />
                            <asp:HiddenField ID="hidOldImages" runat="server" />
                        </div>
                        <p id="ctl00_contentHolder_hidUploadImagesTip"></p>
                    </li>
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>联系人：</span>
                        <Hi:TrimTextBox runat="server" CssClass="form_input_m form-control" ID="txtContactMan" MaxLength="8" placeholder="长度必须为2-8个字符" />
                        <p id="ctl00_contentHolder_txtContactManTip">
                        </p>
                    </li>
                    <li id="warningqtyRow" class="mb_0"><span class="formitemtitle"><em>*</em>联系电话：</span>
                        <Hi:TrimTextBox runat="server" CssClass="form_input_m form-control" ID="txtTel" Text="" placeholder="不能为空，合法的电话或者手机号码" MaxLength="20" />
                        <font style="padding-left:250px;color:#808080;clear:both;float:left;">格式如：15088888888/0731-88888888</font>
                        <p id="ctl00_contentHolder_txtTelTip">
                        </p>
                    </li>
                    <li style="clear: none; overflow: visible;"><span class="formitemtitle"><em>*</em>所在地：</span>
                        <Hi:RegionSelector ID="dropRegion" runat="server" />
                    </li>
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>详细地址：</span>
                        <div class="input-group"><Hi:TrimTextBox runat="server" CssClass="form-control" ID="txtAddress" MaxLength="50" placeholder="不能为空长度必须为2-50个字符" Width="400" />
                        <%--<a class="btn btn-primary ml_10 " id="js_search_pos" onclick="getResult()">搜索标注</a>--%>
                        </div>
                        <p id="ctl00_contentHolder_txtAddressTip">
                        </p>
                    </li>
                    <li class="mb_0"><span class="formitemtitle"><em></em>供应商介绍：</span>
                        <div id="notes1" style="float: left; margin-left: 250px;">
                            <Hi:Ueditor ID="editDescription" runat="server" Width="660" />
                        </div>
                        <p>可以填写供应商介绍以及相关的资质,营业执照等信息</p>
                    </li>
                    <li>
                        <h2 class="colorE">账号信息</h2>
                    </li>
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>用户名：</span>
                        <input style="display: none" /><!-- for disable autocomplete on chrome -->
                        <Hi:TrimTextBox runat="server" CssClass="form_input_m form-control" autocomplete="off" ID="txtUserName" MaxLength="20" placeholder="不超过20个字符" />
                        <p id="ctl00_contentHolder_txtUserNameTip">
                        </p>
                    </li>
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>登录密码：</span>
                        <input style="display: none" /><!-- for disable autocomplete on chrome -->
                        <Hi:TrimTextBox runat="server" CssClass="form_input_m form-control" autocomplete="off" TextMode="Password" ID="txtUserPwd" MaxLength="20" placeholder="不超过20个字符" />
                        <p id="ctl00_contentHolder_txtUserPwdTip">
                        </p>
                    </li>
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>确认登录密码：</span>
                        <Hi:TrimTextBox runat="server" CssClass="form_input_m form-control" TextMode="Password" ID="txtUserRePwd" MaxLength="20" placeholder="不超过20个字符" />
                        <p id="ctl00_contentHolder_txtUserRePwdTip">
                        </p>
                    </li>
                    <%--<li>
                        <h2 class="colorE">消息提醒</h2>
                    </li>
                    <li id="showOpenId"><span class="formitemtitle">微信OpenId：</span>
                        <asp:TextBox ID="txtWxOpenId" CssClass="form_input_m form-control" runat="server" ClientIDMode="Static" />
                        <p id="ctl00_contentHolder_txtWxOpenIdTip">配置好微信AppId与AppSecret就可以使用微信扫描下面的二维码自动获取OpenId</p>
                    </li>
                    <li id="getOpenId" runat="server"><span class="formitemtitle">获取OpenId：</span>
                        <asp:Image runat="server" ID="OpenIdQrCodeImg" Width="150px" />
                        <br />
                        <p id="ctl00_contentHolder_OpenIdQrCodeImgTip">请使用供应商管理员微信扫描该二维码，后续会将该供应商订单通知发送到管理员微信上</p>
                        <p style="color: red;">需要配置微信的订单支付消息模板 <a href="/admin/tools/sendmessagetemplets.aspx">去配置</a></p>
                    </li>--%>
                    <li>
                        <div class="ml_198">
                            <asp:Button runat="server" ID="btnAdd" Text="保 存" OnClientClick="return doSubmit();" Style="float: left;" CssClass="btn btn-primary inbnt" />
                            <%--<asp:Button runat="server" ID="btnAddToWXStores" Text="保存并同步至微信供应商" Style="float: left; margin-left:50px;" OnClientClick="return doSubmit();" OnClick="btnAddToWXStores_Click"
                                CssClass="btn btn-primary inbnt" />--%>
                        </div>
                    </li>
                </ul>
            </div>
        </div>
    </div>
             <!-- start ImgPicker -->
    <uc1:ImageList ID="ImageList" runat="server" />
</asp:Content>