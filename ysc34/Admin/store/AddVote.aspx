<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddVote.aspx.cs" Inherits="Hidistro.UI.Web.Admin.AddVote" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>


<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
   <script type="text/javascript" src="/utility/jquery.hishopUpload.js" ></script>
    <script type="text/javascript" src="/utility/jquery.form.js"></script>
    <link id="cssLink" rel="stylesheet" href="../css/style.css" type="text/css" media="screen" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="areacolumn clearfix databody">
        <div class="areacolumn clearfix">
                <div class="title">
                    <ul class="title-nav">
                        <li><a href="Votes.aspx">管理</a></li>
                        <li class="hover"><a href="javascript:void">添加</a></li>

                    </ul>

                </div>
            <div class="columnright">
                <div class="formitem mt_10">
                    <ul>
                        <li class="mb_0"><span class="formitemtitle"><em>*</em>投票标题：</span>
                            <asp:TextBox ID="txtAddVoteName" runat="server" CssClass="form_input_m form-control" placeholder="长度限制在60个字符以内" />
                            <p id="ctl00_contentHolder_txtAddVoteNameTip"></p>
                        </li>

                        <li style="display: none;"><span class="formitemtitle">是否开启：</span>
                            <asp:CheckBox ID="checkIsBackup" Checked="true" runat="server" />
                        </li>
                        <li class="mb_0"><span class="formitemtitle"><em>*</em>最多可选项数：</span>
                            <asp:TextBox ID="txtMaxCheck" runat="server" Text="1" CssClass="form_input_m form-control" placeholder="范围为1-100之间的整数" />
                            <p id="ctl00_contentHolder_txtMaxCheckTip"></p>
                        </li>

                        <li>
                            <span class="formitemtitle">开始日期：</span>
                            <Hi:CalendarPanel runat="server" ID="calendarStartDate"></Hi:CalendarPanel>
                        </li>
                        <li>
                            <span class="formitemtitle">结束日期：</span>
                            <Hi:CalendarPanel runat="server" ID="calendarEndDate"></Hi:CalendarPanel>
                        </li>
                        <li class="mb_0"><span class="formitemtitle"><em>*</em>投票选项：</span>
                            <asp:TextBox ID="txtValues" CssClass="form_input_l form-control" runat="server" Height="100" TextMode="MultiLine" placeholder="一行一个投票选项"></asp:TextBox>
                            <p id="ctl00_contentHolder_txtValuesTip"></p>
                        </li>
                        <li>
                            <span class="formitemtitle">展示位置：</span>
                            <span class="icheck-label-5-10">
                                <asp:CheckBox ID="chkDisplayWeixinStatic" ClientIDMode="Static" runat="server" Text="微商城" CssClass="icheck ml_20" /></span>
                            <asp:CheckBox ID="chkDisplayWeixin"  runat="server"  AutoPostBack="True" ClientIDMode="Static" Style="display:none;"  OnCheckedChanged="chkDisplayWeixin_CheckedChanged" />
                        </li>
                        <li class="mb_0" id="likey" runat="server" visible="false"><span class="formitemtitle">关键字：</span>
                            <asp:TextBox ID="txtKeys" runat="server" CssClass="form_input_l form-control" placeholder="长度限制在60个字符以内" />
                            <p id="ctl00_contentHolder_txtKeysTip"></p>
                        </li>
                        <li id="liimg" runat="server" visible="false"><span class="formitemtitle">封面图片：</span>
                            <span name="voteImage" class="imgbox"></span>
                            <asp:HiddenField ID="hidUploadImages" runat="server" />
                            <asp:HiddenField ID="hidOldImages" runat="server" />
                        </li>

                    </ul>
                    <div class="ml_198">
                        <asp:Button ID="btnAddVote" runat="server" Text="添 加" OnClientClick="return getUploadImages()" CssClass="btn btn-primary" />
                    </div>
                </div>

            </div>
        </div>


    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">

    <script type="text/javascript" language="javascript">
        // 初始化图片上传控件
        function initImageUpload() {
            $('#<%=liimg.ClientID%> span[name="voteImage"]').hishopUpload(
                           {
                               title: '封面图片',
                               url: "/admin/UploadHandler.ashx?action=newupload",
                               imageDescript: '',
                               displayImgSrc: '',
                               imgFieldName: "voteImage",
                               defaultImg: '',
                               pictureSize: '320*200',
                               imagesCount: 1,
                               dataWidth: 9
                           });

        }

        //获取上传成功后的图片路径
        function getUploadImages() {
            if (!PageIsValid()) return false;
            var srcImg = $('#<%=liimg.ClientID%> span[name="voteImage"]').hishopUpload("getImgSrc");
            $("#<%=hidUploadImages.ClientID%>").val(srcImg);
            return true;
        }

        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtAddVoteName', 1, 60, false, null, '投票调查的标题，长度限制在60个字符以内'));
            initValid(new InputValidator('ctl00_contentHolder_txtKeys', 1, 60, true, null, '关键字，长度限制在60个字符以内'));
            initValid(new InputValidator('ctl00_contentHolder_txtMaxCheck', 1, 10, false, '-?[0-9]\\d*', '设置一次投票最多可以选择投几个选项'));
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtMaxCheck', 1, 100, '最多可选项数不允许为空，范围为1-100之间的整数'));
            initValid(new InputValidator('ctl00_contentHolder_txtValues', 0, 300, false, null, '在输入框中用回车换行区分多个选项值'));
        }

        $(document).ready(function () {
            InitValidators();
            initImageUpload();
            $("#chkDisplayWeixinStatic").on('ifChecked', function (event) {
                $("#chkDisplayWeixin").click();
            });
            $("#chkDisplayWeixinStatic").on('ifUnchecked', function (event) {
                $("#chkDisplayWeixin").click();
            });
        });
    </script>
</asp:Content>
