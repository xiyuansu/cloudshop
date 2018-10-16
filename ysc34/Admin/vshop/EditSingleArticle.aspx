<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="EditSingleArticle.aspx.cs" Inherits="Hidistro.UI.Web.Admin.vshop.EditSingleArticle" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<%@ Register src="../Ascx/ImageList.ascx" tagname="ImageList" tagprefix="uc1" %>


<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <script type="text/javascript">
        var auth = "<%=(Request.Cookies[FormsAuthentication.FormsCookieName]==null ? string.Empty : Request.Cookies[FormsAuthentication.FormsCookieName].Value) %>";
    </script>
    <script src="../js/swfupload/swfupload.js" type="text/javascript"></script>
    <script src="../js/swfupload/handlers.js" type="text/javascript"></script>
    <link href="../css/MutiArticle.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="/utility/jquery.hishopUpload.js" ></script>
    <script type="text/javascript" src="/utility/jquery.form.js"></script>
    <link id="cssLink" rel="stylesheet" href="../css/style.css" type="text/css" media="screen" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" ClientIDMode="Static"
    runat="server">
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
                target: "#uploadpic",
                targetType: "src",
                displayImgSrc: $("#<%=hdpic.ClientID%>").val()
            });
        })
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

        function getUploadImages() {
            var srcLogo = $('#logoContainer span[name="spanButtonPlaceholder"]').hishopUpload("getImgSrc");
            $("#<%=hdpic.ClientID%>").val(srcLogo);
            return true;
        }
    </script>
    <style>
        .validator2 {
            padding: 0 20px;
        }

            .validator2 ul li {
                margin-bottom: 10px !important;
            }

                .validator2 ul li p {
                    margin-left: 0 !important;
                    width: 100% !important;
                }

        .box_body {
            position: relative;
        }
       
    </style>
    <div class="dataarea mainwidth databody top_10">
        <div class="title">
            <ul class="title-nav">
                <li><a href="ReplyOnKey.aspx">管理</a></li>
                <li class="hover"><a href="javascript:void">编辑单图文</a></li>
            </ul>
        </div>

        <div class="datafrom top_10">
            <div class="tw_body">
                <div class="tw_box box_left">
                    <div class="body" style="width: 328px;">
                        <asp:Label ID="LbimgTitle" runat="server" Text="标题"></asp:Label>
                        <div class="img_fm">
                            <div id="img_default" style="display: block; width: 328px;" class="gy_bg">封面图片</div>
                            <img id="uploadpic" runat="server" class="fmImg" width="300" />
                        </div>
                        <asp:Label ID="Lbmsgdesc" runat="server" Text=""></asp:Label>
                    </div>
                    <div class="formitem validator2">
                        <ul>
                            <li><span class="formitemtitle" style="width: 75px !important">回复类型：</span>
                                <asp:CheckBox ID="chkKeys" runat="server" Text="关键字回复" CssClass="icheck icheck-label-5 mr5 mb_0" />
                                <asp:CheckBox ID="chkSub" runat="server" Text="关注时回复" CssClass="icheck icheck-label-5 mr5 mb_0" />
                                <asp:CheckBox ID="chkNo" runat="server" Text="无匹配回复" CssClass="icheck icheck-label-5 mb_0" />
                            </li>
                            <li class="likey"><span class="formitemtitle" style="width: 75px !important"><em>*</em>关键字：</span>
                                <asp:TextBox ID="txtKeys" Width="190" runat="server" CssClass="forminput form-control" Style="width: 253px;" placeholder="用户可通过该关键字搜到到这个内容"></asp:TextBox>
                                <!--<p id="ctl00_contentHolder_txtKeysTip"></p>-->
                            </li>
                            <li class="likey"><span class="formitemtitle" style="width: 75px !important">匹配模式：</span>
                                <Hi:YesNoRadioButtonList ID="radMatch" runat="server" CssClass="icheck icheck-label-10" RepeatLayout="Flow" YesText="模糊匹配" NoText="精确匹配" />
                            </li>
                            <li><span class="formitemtitle" style="width: 75px !important">状态：</span>
                                <Hi:YesNoRadioButtonList ID="radDisable" runat="server" CssClass="icheck icheck-label-10" RepeatLayout="Flow" YesText="启用" NoText="禁用" />
                            </li>
                        </ul>
                        <asp:Button ID="btnCreate" runat="server" Text="保 存" CssClass="btn btn-primary mb_20" Style="margin-left: 75px;" OnClientClick="return CheckKey();" OnClick="btnCreate_Click" />
                    </div>

                </div>
                <div id="box_move" class="tw_box box_left box_body" style="width: 580px; left: 603px; padding: 10px 0 20px;">
                    <div class="cont_body">
                        <div class="fgroup">
                            <span class="text_title"><em>*</em>标题：</span>
                            <asp:TextBox ID="Tbtitle" runat="server" CssClass="form_input_m form-control" onkeyup="syncSingleTitle(this.value)"></asp:TextBox>
                        </div>
                        <div class="fgroup">
                            <div style="width: 100%; height: 130px;">
                                <span class="text_title"><em>*</em>封 面：</span>
                                <div class="tip-text-muted pull-left" id="logoContainer">
                                    <span name="spanButtonPlaceholder" class="imgbox"></span>
                                    <asp:HiddenField ID="HiddenField1" runat="server" />

                                </div>
                            </div>
                        </div>

                        <div class="fgroup">
                            <span class="text_title"><em>*</em>摘要：</span>
                            <asp:TextBox ID="Tbdescription" runat="server" TextMode="MultiLine" CssClass="form_input_l form-control" onkeyup="syncAbstract(this.value)"></asp:TextBox>
                        </div>
                        <div class="fgroup diyLink">
                            <span class="text_title">自定义链接：</span>
                            <asp:TextBox ID="TbUrl" runat="server" CssClass="form_input_m form-control" placeholder="可不填，若填写则优先跳转"></asp:TextBox>
                        </div>
                        <div class="fgroup SubLink" style="height: 32px;">
                            <span class="text_title">自定义链接：</span>
                            <asp:DropDownList runat="server" ID="ddlsubType" CssClass="iselect">
                                <asp:ListItem Text="自定义链接" Value="0"></asp:ListItem>
                                <asp:ListItem Text="优惠券" Value="1"></asp:ListItem>
                            </asp:DropDownList>

                            <asp:TextBox ID="TbUrltoSub" runat="server" ClientIDMode="Static" CssClass="form-control form_input_s" placeholder="可不填，若填写则优先跳转"></asp:TextBox>
                            <!--<em class="UrltoSubTip"></em>-->

                            <asp:DropDownList runat="server" ID="ddlCoupon" ClientIDMode="Static" CssClass="iselect" Width="220">
                            </asp:DropDownList>

                        </div>
                        <div style="float: left;">
                            <Hi:Ueditor ID="fkContent" runat="server" Width="500" />
                        </div>
                    </div>
                    <i class="arrow arrow_out" style="margin-top: 0px;"></i>
                    <i class="arrow arrow_in" style="margin-top: 0px;"></i>
                </div>
                <div id="nextTW"></div>
            </div>
            <asp:HiddenField ID="hdpic" runat="server" />
            <div class="clear"></div>
        </div>
    </div>

   <uc1:ImageList ID="ImageList" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script src="../js/MultiBox.js" type="text/javascript"></script>
    <script src="../js/ReplyOnKey.js" type="text/javascript"></script>
</asp:Content>
