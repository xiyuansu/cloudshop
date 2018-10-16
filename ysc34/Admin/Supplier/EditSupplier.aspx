<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/Admin.Master" CodeBehind="EditSupplier.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Supplier.EditSupplier" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register Src="/admin/Ascx/ImageList.ascx" TagName="ImageList" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <script type="text/javascript" src="/admin/js/commonvalidator.js"></script>
    <script type="text/javascript" src="/utility/jquery.hishopUpload.js"></script>
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
        $(function () {
            $('.title-nav').each(function (i) {
                $(this).find("li").each(function (a) {
                    $(this).click(function () {
                        ///----先把所有同级栏目选中样式去掉----
                        $('.title-nav').each(function (i) {
                            $(this).find("li").each(function (a) {
                                $(this).removeClass("hover");//先把所有选中样式去掉
                                $("#right" + a).hide();//原先的div隐藏掉
                            });
                        });
                        ////------------

                        $(this).addClass("hover");//为当前添加选中样式
                        $("#right" + a).show();//当前的div显示
                    });
                });
            });
        });


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
            if (CheckName("ctl00_contentHolder_txtSupplierName", "CheckSupplierName", "SupplierName") == false) {
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

            return true;
        }

        $(document).ready(function (e) {
            InitValidators();
        });

        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtSupplierName', 2, 20, false, null, '供应商名称长度必须为2-20个字符'));
            initValid(new InputValidator('ctl00_contentHolder_txtAddress', 2, 50, false, null, '详细地址不能为空长度必须为2-50个字符'));
            initValid(new InputValidator('ctl00_contentHolder_txtContactMan', 2, 8, false, null, '联系人不能为空，长度必须为2-8个字符'));
            initValid(new InputValidator('ctl00_contentHolder_txtTel', 7, 20, false, '^0?(13|15|18|14|17)[0-9]{9}$|^\\d{2,4}-\\d{7,8}(-\\d{2,4})?', '联系电话不能为空，请输入合法的电话或者手机号码'));
        }


        //检测供应商名称 或 用户名是否存在
        //byId检测的id号，CheckAction请求的action方法，CheckName检测数据库表名称
        function CheckName(byId, CheckAction, CheckName) {
            var txtName = $("#" + byId).val();
            var posturl = "/Admin/Supplier/SupplierHandler.ashx?action=" + CheckAction + "&" + CheckName + "=" + escape(txtName) + "&r=" + Math.random();
            posturl += "&supplierId=" + $("#supplierId").val();
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

        ////供应商改变状态------开始------
        function updatesupperstate(statusvalue) {
            switch (statusvalue) { //这里弹窗点击确定没反应后面待解决再用下面方法
                case 2://冻结
                    formtype = "_frozen";
                    DialogShow("供应商账号冻结", "supplier_frozen", "divUpStatus2", "btn_frozen");
                    break;
                default://恢复
                    formtype = "_recovery";
                    DialogShow("供应商账号恢复", "supplier_recovery", "divUpStatus1", "btn_recovery");
                    break;
            }
        }

        //异步更改供应商状态
        function editstatus(statusvalue) {
            var supplierId = $("#supplierId").val();
            var posturl = "/Admin/Supplier/SupplierHandler.ashx?action=EditSupplierStatus&supplierId=" + supplierId + "&statusvalue=" + statusvalue;

            var date = ajaxjson(posturl);//当前方法在/admin/js/commonvalidator.js里面
            if (date != null) {
                if (date.success == "true") {//处理成功
                    ShowMsg(date.msg, true);//弹出成功提示
                    var statusHTML = "正常 &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href=\"javascript:void(0);\" onclick=\"updatesupperstate(2)\">冻结</a>";
                    if (statusvalue == 2) {
                        statusHTML = "冻结 &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href=\"javascript:void(0);\" onclick=\"updatesupperstate(1)\">恢复</a>";
                    }
                    $("#divStatus").html(statusHTML);
                } else {
                    ShowMsg(date.msg, false);//弹出失败提示
                    return false;
                }
            }
            $('.hishop_menu_scroll', window.parent.document).css("opacity", "1");
        }

        //检测验证(它是window.js里DialogShow方法调用)，这个页面特别验证，直接返回true就行
        function validatorForm() { return true; }
        ////供应商改变状态------结束------
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <input type="hidden" id="supplierId" value='<%=Request["supplierId"] %>' /><%--供应商Id--%>
    <div style="display: none;" id="managerId">
        <asp:Literal ID="lblManageId" runat="server" /></div>
    <%--供应商管理用户Id--%>
    <%--供应商管理用户Id，便于上面检查修改供应商名称是否存在--%>

    <%--点击状态供应商“恢复” 弹出内容--%>
    <div style="display: none;">
        <div id="divUpStatus1">
            供应商恢复登录，所有商品需要重新审核，是否继续？
        </div>
        <input type="button" id="btn_recovery" value="恢复" onclick="editstatus(1)" />

        <div id="divUpStatus2">
            供应商下商品将全部下架，账号数据会保留，但供应商不能登录后台，是否继续？
        </div>
        <input type="button" id="btn_frozen" value="冻结" onclick="editstatus(2)" />
    </div>


    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li>
                    <a href="SupplierList.aspx">管理</a></li>
                <li class="hover">
                    <a href="javascript:void">基本信息</a></li>
                <li>
                    <a href="javascript:void">密码管理</a></li>
            </ul>
        </div>
    </div>
    <div class="areacolumn clearfix">
        <div class="columnright" id="right0" style="display: none;">
        </div>

        <div class="columnright" id="right1">
            <div class="formitem clearfix">
                <ul>
                    <li class="mb_0"><span class="formitemtitle">用户名：</span>
                        <asp:Literal runat="server" ID="lblUserName" />
                    </li>
                    <li class="mb_0"><span class="formitemtitle">状态：</span>
                        <div id="divStatus">
                            <asp:Literal runat="server" ID="lblStatus" />
                        </div>
                    </li>

                    <li class="mb_0"><span class="formitemtitle"><em>*</em>供应商名称：</span>
                        <Hi:TrimTextBox runat="server" ID="txtSupplierName" Width="350px" CssClass="form_input_m form-control" MaxLength="20" placeholder="不超过20个字符"></Hi:TrimTextBox>
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
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>联系电话：</span>
                        <Hi:TrimTextBox runat="server" CssClass="form_input_m form-control" ID="txtTel" Text="" placeholder="不能为空，合法的电话或者手机号码" MaxLength="20" />
                        <font style="padding-left: 250px; color: #808080; clear: both; float: left;">格式如：15088888888/0731-88888888</font>
                        <p id="ctl00_contentHolder_txtTelTip">
                        </p>
                    </li>
                    <li style="clear: none; overflow: visible;"><span class="formitemtitle"><em>*</em>所在地：</span>
                        <Hi:RegionSelector ID="dropRegion" runat="server" />
                    </li>
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>详细地址：</span>
                        <div class="input-group">
                            <Hi:TrimTextBox runat="server" CssClass="form-control" ID="txtAddress" MaxLength="50" placeholder="不能为空长度必须为2-50个字符" Width="400" />
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
                </ul>
            </div>
            <div class="ml_198">
                <asp:Button runat="server" ID="btnAdd" Text="保存" OnClientClick="return doSubmit();"
                    CssClass="btn btn-primary" Style="float: left;" />
                <%--<asp:Button runat="server" ID="btnAddToWXStores" Text="保存并同步至微信供应商" Style="float: left; margin-left:50px;" OnClientClick="return doSubmit();" OnClick="btnAddToWXStores_Click"
                                CssClass="btn btn-primary inbnt" />--%>
            </div>
        </div>

        <div class="columnright" id="right2" style="display: none;">
            <style type="text/css">
                .czpass {
                    background-color: #0091ea;
                    color: #fff;
                    float: left;
                    margin-left: 200px;
                    cursor: pointer;
                    padding: 0px 10px;
                    border-radius: 4px;
                }

                .qkpass {
                    background-color: #0091ea;
                    color: #fff;
                    float: left;
                    margin-left: 20px;
                    cursor: pointer;
                    padding: 0px 10px;
                    border-radius: 4px;
                }
            </style>
            <div class="formitem clearfix">
                <ul>
                    <li class="mb_0"><span class="formitemtitle">用户名：</span>
                        <asp:Literal runat="server" ID="lblUserName2" />
                    </li>
                    <li><span class="formitemtitle">供应商：</span>
                        <asp:Literal runat="server" ID="lblSupplierName" />
                    </li>
                    <li class="mb_0">
                        <div class="czpass" onclick="passupdate('czpass')">重置登录密码</div>
                        <div class="qkpass" onclick="passupdate('qkpass')">清空交易密码</div>

                        <div style="display: none;">
                            <div id="divczpass">
                                确定重置登录密码吗？
                            </div>
                            <input type="button" id="btncz" onclick="czpass()" value="重置1" />

                            <div style="display: none;" id="divqkpass">
                                确定要清空交易密码吗？
                            </div>
                            <input type="button" id="btnqk" onclick="qkpass()" value="清空1" />
                        </div>
                        <script type="text/javascript">
                            function passupdate(typename) {
                                switch (typename) {
                                    case "czpass"://重置登录密码
                                        DialogShow("重置登录密码", "supplierczpass", "divczpass", "btncz");
                                        break;
                                    case "qkpass"://清空交易密码
                                        DialogShow("清空交易密码", "supplierqkpass", "divqkpass", "btnqk");
                                        break;
                                }
                            }

                            function czpass() {//重置登录密码
                                var managerId = $("#managerId").html();//供应商管理id
                                var posturl = "/Admin/Supplier/SupplierHandler.ashx?action=ResetSupplierPass&managerId=" + managerId;

                                var data = ajaxjson(posturl);//当前方法在/admin/js/commonvalidator.js里面
                                if (data != null) {
                                    //ShowMsg(data.msg);//弹出提示
                                    alert(data.msg);
                                    $('.hishop_menu_scroll', window.parent.document).css("opacity", "1");
                                }
                            }

                            function qkpass() {//清空交易密码
                                var supplierId = $("#supplierId").val();//供应商id
                                var posturl = "/Admin/Supplier/SupplierHandler.ashx?action=EmptySupplierTransactionPass&supplierId=" + supplierId;

                                var data = ajaxjson(posturl);//当前方法在/admin/js/commonvalidator.js里面
                                if (data != null) {
                                    //alert(data.msg);//弹出提示
                                    if (data.success == "true") {
                                        ShowMsg(data.msg, true);
                                    } else {
                                        ShowMsg(data.msg, false);
                                    }
                                }
                            }
                        </script>
                    </li>
                </ul>
            </div>
        </div>
    </div>
    <!-- start ImgPicker -->
    <uc1:ImageList ID="ImageList" runat="server" />
</asp:Content>
