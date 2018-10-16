<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="EditMultiArticle.aspx.cs" Inherits="Hidistro.UI.Web.Admin.vshop.EditMultiArticle" %>
<%@ Register src="../Ascx/ImageList.ascx" tagname="ImageList" tagprefix="uc1" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <link href="../css/MutiArticle.css" rel="stylesheet" type="text/css" />
    <script src="../js/swfupload/swfupload.js" type="text/javascript"></script>
    <script src="../js/swfupload/handlers.js" type="text/javascript"></script>
    <script type="text/javascript">
        var auth = "<%=(Request.Cookies[FormsAuthentication.FormsCookieName]==null ? string.Empty : Request.Cookies[FormsAuthentication.FormsCookieName].Value) %>";
    </script>
    <script type="text/javascript" src="/utility/jquery.hishopUpload.js" ></script>
    <script type="text/javascript" src="/utility/jquery.form.js"></script>
    <link id="cssLink" rel="stylesheet" href="../css/style.css" type="text/css" media="screen" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server" ClientIDMode="Static">
    <script type="text/javascript">
        $(document).ready(function () {
            ShowKey();
            $("#chkKeys").bind("ifChanged", function () { ShowKey() });
            $("#chkSub").bind("ifChanged", function () { ShowKey() });
            $("#ddlsubType").change(function () { ShowKey(); });
        });
        function ShowKey() {
            if ($("#chkKeys:checked").length > 0) {
                $(".likey").show();
            }
            else {
                $(".likey").hide();
            }
            if ($("#chkSub:checked").length > 0) {
                $(".SubLink").show();
                $(".diyLink").hide();
            }
            else {
                $(".SubLink").hide();
                $(".diyLink").show();
            }
            if ($("#ddlsubType").val() == "0") {
                $("#ddlCoupon").closest('.iselect').hide();
                $("#TbUrltoSub").show();
                $(".UrltoSubTip").show();
            }
            else if ($("#ddlsubType").val() == "1") {
                $("#ddlCoupon").closest('.iselect').show();
                $("#TbUrltoSub").hide();
                $(".UrltoSubTip").hide();
            }
        }
    </script>
    <style>
        .validator2 { padding: 0 20px; }

            .validator2 ul li { margin-bottom: 10px !important; }

                .validator2 ul li p { margin-left: 0 !important; width: 100% !important; }

        .box_body { position: relative; }
    </style>
    <div class="dataarea mainwidth databody top_10">
        <div class="title">
            <ul class="title-nav">
                <li><a href="ReplyOnKey.aspx">管理</a></li>
                <li class="hover"><a href="javascript:void">编辑多图文</a></li>
            </ul>
        </div>

        <div class="datafrom top_10">
            <div class="tw_body">
                <div class="tw_box box_left">
                    <div id="sbox0" class="body" style="width: 328px;">
                        <div class="img_fm" onmousemove="sBoxzzShow('0')">
                            <div id="fm0" style="display: block;" class="gy_bg fmImg">
                                封面图片
                            </div>
                            <img id="img0" class="fmImg" src="" />
                            <p class="abstractVal">
                                <span id="title0" style="margin-left: 4px;">摘要</span>
                            </p>
                            <div id="zz_sbox0" class="zzc" onmouseout="sBoxzzHide('0')" style="line-height: 178px;">
                                <a href="javascript:void(0)" onclick="editTW(0);">修改</a>
                            </div>
                        </div>
                    </div>

                    <div id="sbox1" class="baseBorder twSBox" onmousemove="sBoxzzShow(1)" style="position: relative; width: 368px;">
                        <div class="body">
                            <div id="title1" class="info">
                                <p>标题</p>
                            </div>
                            <div class="simg">
                                <div style="width: 100%; height: 100%; background-color: rgb(236,236,236); line-height: 100px; color: #c0c0c0; font-weight: bold; text-align: center;">
                                    预览图
                                </div>
                                <img id="img1" src="" class="fmImg" />
                            </div>
                            <div class="clearfix"></div>
                        </div>
                        <div id="zz_sbox1" onmouseout="sBoxzzHide(1)" class="zzc" style="line-height: 121px; height: 121px;">
                            <a href="javascript:void(0)" onclick="editTW(1);">修改</a>
                        </div>
                    </div>

                    <span id="addSBoxInfoHere"></span>
                    <div class="baseBorder twSBox boxAdd" style="width: 368px;">
                        <a href="javascript:void(0)" onclick="addSBox()">添加一个图文</a>
                    </div>
                    <div class="formitem validator2">
                        <ul>
                            <li><span class="formitemtitle text-right" style="width: 75px !important">回复类型：</span>
                                <asp:CheckBox ID="chkKeys" runat="server" Text="关键字回复" CssClass="icheck icheck-label-5 mr5 mb_0" />
                                <asp:CheckBox ID="chkSub" runat="server" Text="关注时回复" CssClass="icheck icheck-label-5 mr5 mb_0" />
                                <asp:CheckBox ID="chkNo" runat="server" Text="无匹配回复" CssClass="icheck icheck-label-5 mb_0" />
                            </li>
                            <li class="likey"><span class="formitemtitle text-right" style="width: 75px !important"><em>*</em>关键字：</span>
                                <asp:TextBox ID="txtKeys" runat="server" CssClass="form-control forminput" Style="width: 253px;" placeholder="用户可通过该关键字搜到到这个内容"></asp:TextBox>
                                <!--<p id="ctl00_contentHolder_txtKeysTip"></p>-->
                            </li>
                            <li class="likey"><span class="formitemtitle text-right" style="width: 75px !important">匹配模式：</span>
                                <Hi:YesNoRadioButtonList ID="radMatch" runat="server" CssClass="icheck icheck-label-10" RepeatLayout="Flow" YesText="模糊匹配" NoText="精确匹配" />
                            </li>
                            <li><span class="formitemtitle text-right" style="width: 75px !important">状态：</span>
                                <Hi:YesNoRadioButtonList ID="radDisable" runat="server" CssClass="icheck icheck-label-10" RepeatLayout="Flow" YesText="启用" NoText="禁用" />
                            </li>
                        </ul>
                        <input type="button" onclick="return checkJson()" class="btn btn-primary mb_20" style="margin-left: 75px;" value="保存" />
                    </div>
                </div>
                <div id="box_move" class="tw_box box_left box_body" style="width: 580px; left: 603px; padding: 10px 0 20px;">
                    <div class="cont_body">
                        <div class="fgroup">
                            <span class="text_title"><em>*</em>标题：</span>
                            <input id="title" type="text" class="form_input_m form-control" />
                        </div>
                        <div class="fgroup">
                            <div style="width: 100%; height: 130px;">
                                <span class="text_title"><em>*</em>封 面：</span>
                                <div class="tip-text-muted pull-left" id="logoContainer">
                                    <span name="spanButtonPlaceholder" class="imgbox"></span>
                                    <asp:HiddenField ID="hdpic" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="fgroup" id="w_url">
                            <span class="text_title">自定义链接：</span>
                            <input id="urlData" class="form_input_m form-control" type="text" value="" placeholder="可不填，若填写则优先跳转" />
                        </div>
                        <div id="msg" style="color: red; font-size: 14px;"></div>
                        <div>
                            <asp:TextBox TextMode="MultiLine" runat="server" ID="fkContent" ClientIDMode="Static" Width="500"></asp:TextBox>
                        </div>
                        <div class="fgroup" style="margin-top: 10px;">
                            <input type="button" class="btn btn-primary" value="添加" onclick="saveData()" />
                        </div>
                    </div>
                    <i class="arrow arrow_out" style="margin-top: 0px;"></i><i class="arrow arrow_in"
                        style="margin-top: 0px;"></i>
                </div>
                <div id="nextTW"></div>
            </div>
            <div id="modelSBox" style="display: none;">
                <div id="sboxrpcode1366" class="baseBorder twSBox" style="position: relative;">
                    <div class="body">
                        <div id="titlerpcode1366" class="info">
                            <p>
                                标题
                            </p>
                        </div>
                        <div class="simg">
                            <div style="width: 100px; height: 100px; background-color: rgb(236,236,236); line-height: 100px; color: #c0c0c0; font-weight: bold; text-align: center;">
                                预览图
                            </div>
                            <img id="imgrpcode1366" src="pig1.png" style="width: 100%; height: 100%; position: absolute; top: 0; left: 0;" />
                        </div>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div id="zz_sboxrpcode1366"  class="zzc" style="line-height: 121px; height: 121px;">
                        <a href="javascript:void(0)" id="zz_editrpcode1366" >修改</a> <a href="javascript:void(0)" id="zz_delrpcode1366">删除</a>
                    </div>
                </div>
            </div>
            <div class="clearfix"></div>
        </div>
        <input id="Articlejson" name="Articlejson" type="hidden" />
    </div>
    <script src="../js/jquery-json-2.4.js" type="text/javascript"></script>
    <script src="../js/MultiBox.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $('#logoContainer span[name="spanButtonPlaceholder"]').hishopUpload(
                          {
                              title: '缩略图',
                              url: "/admin/UploadHandler.ashx?action=newupload",
                              imageDescript: '',
                              imgFieldName: "siteLogo",
                              pictureSize: '360*200',
                              imagesCount: 1,
                              target: "#img0",
                              targetType: "src"
                          });
        });

        function getUploadImages() {
            var srcLogo = $('#logoContainer span[name="spanButtonPlaceholder"]').hishopUpload("getImgSrc");
            $("#<%=hdpic.ClientID%>").val(srcLogo);
            return true;
        }
        var UEEditor;
        edit = true;//定义当前JS执行为编辑状态
        var MaterialID = '<%=MaterialID%>';

        function loadJsonData() {
            UEEditor = UE.getEditor('fkContent');
            UEEditor.ready(function (e) {
                editTW(0);
                tws = eval(<%=articleJson%>);
                dataBind();
            });
        }
        $(document).ready(function (e) {
            loadJsonData();
        });

        function EditMultArticles() {
            if ($("#chkKeys:checked").length > 0) {
                if ($("#txtKeys").val() == "") {
                    alert("你选择了，关键字回复，请填写关键字！");
                    return;
                }
            }
            if ($("#chkKeys:checked,#chkSub:checked,#chkNo:checked").length == 0) {
                alert("请选择回复类型");
                return;
            }

            $.ajax({
                url: "./EditMultiArticle?cmd=Update&id=" + MaterialID,
                type: "POST",
                dataType: "text",
                data: {
                    "MultiArticle": $("#Articlejson").val()
                    , "Keys": $("#txtKeys").val()
                    , "chkKeys": $("#chkKeys:checked").length == 1
                    , "chkSub": $("#chkSub:checked").length == 1
                    , "chkNo": $("#chkNo:checked").length == 1
                    , "radMatch": $("#radMatch_0:checked").length == 1
                    , "radDisable": $("#radDisable_0:checked").length == 1
                },
                success: function (msg) {
                    if (msg == "true") {
                        alert("修改成功！");
                        window.location.href = "ReplyOnKey.aspx";
                    }
                    else if (msg == "key") {
                        alert("关键字重复，请重新填写！");
                        $("#txtKeys").focus();
                    }
                    else {
                        alert("修改失败！");
                    }
                },
                error: function (xmlHttpRequest, error) {
                    alert(error);
                }
            });
        }
    </script>
    <!-- start ImgPicker -->
    <script type="text/j-template" id="tpl_popbox_ImgPicker">
        <div id="ImgPicker">
            <div class="tabs clearfix">
                <a href="javascript:;" class="active tabs_a fl" data-origin="imgpicker" data-index="1">选择图片</a>
                <a href="javascript:;" class="tabs_a fl j-initupload" data-origin="imgpicker" data-index="2">上传新图片</a>
            </div>
            <!-- end tabs-->
            <div class="tabs-content" data-origin="imgpicker">
                <div class="tc" data-index="1">
                    <ul class="img-list imgpicker-list clearfix"></ul>
                    <!-- end img-list -->
                    <div class="imgpicker-actionPanel clearfix">
                        <div class="fl">
                            <a href="javascript:;" class="btn btn-primary" id="j-btn-listuse">使用选中图片</a>
                        </div>
                        <div class="fr">
                            <div class="paginate"></div>
                        </div>
                    </div>
                    <!-- end imgpicker-actionPanel -->
                </div>

                <div class="tc hide" data-index="2">
                    <div class="uploadifyPanel clearfix">
                        <ul class="img-list imgpicker-upload-preview"></ul>
                        <input type="file" name="imgpicker_upload_input" id="imgpicker_upload_input">
                    </div>

                    <div class="imgpicker-actionPanel">
                        <a href="javascript:;" class="btn btn-primary" id="j-btn-uploaduse">使用上传的图片</a>
                    </div>
                    <!-- end imgpicker-actionPanel -->
                </div>
            </div>
            <!-- end tabs-content -->
        </div>
    </script>

    <script type="text/j-template" id="tpl_popbox_ImgPicker_listItem">
        <# _.each(dataset,function(url){ #>
        <li>
            <span class="img-list-overlay"><i class="img-list-overlay-check"></i></span>
            <img src="<#= url #>">
        </li>
        <# }) #>
    </script>

    <script type="text/j-template" id="tpl_popbox_ImgPicker_uploadPrvItem">
        <li>
            <span class="img-list-btndel j-imgpicker-upload-btndel"><i class="gicon-trash white"></i></span>
            <span class="img-list-overlay"></span>
            <img src="<#= url #>">
        </li>
    </script>
    <!-- end ImgPicker -->

             <uc1:ImageList ID="ImageList" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
        <%--富文本编辑器start--%>
    <link rel="stylesheet" href="/Utility/Ueditor/css/dist/component-min.css" />
    <link rel="stylesheet" href="/Utility/Ueditor/plugins/uploadify/uploadify-min.css" />

    <script type="text/javascript" src="/Utility/Ueditor/js/dist/lib-min.js"></script>
    <script type="text/javascript" src="/Utility/Ueditor/js/config.js"></script>
    <script type="text/javascript" src="/Utility/Ueditor/plugins/uploadify/jquery.uploadify.min.js?ver=940"></script>
    <script type="text/javascript" src="/Utility/Ueditor/plugins/ueditor/ueditor.config.js"></script>
    <script type="text/javascript" src="/Utility/Ueditor/plugins/ueditor/ueditor.all.min.js?v=3.0"></script>
    <script type="text/javascript" src="/Utility/Ueditor/plugins/ueditor/diy_imgpicker.js"></script>
    <script type="text/javascript" src="/Utility/Ueditor/js/dist/componentindex-min.js"></script>

    <%--富文本编辑器end--%>
    <script src="../js/ReplyOnKey.js" type="text/javascript"></script>
</asp:Content>
