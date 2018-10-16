<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="EditLotteryTicket.aspx.cs" Inherits="Hidistro.UI.Web.Admin.EditLotteryTicket" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <script type="text/javascript" src="/utility/jquery.hishopUpload.js" ></script>
    <script type="text/javascript" src="/utility/jquery.form.js"></script>
    <link id="cssLink" rel="stylesheet" href="../css/style.css" type="text/css" media="screen" />
    <link rel="stylesheet" href="../css/bootstrap-switch.css" />
    <script type="text/javascript" src="../js/bootstrap-switch.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="areacolumn clearfix databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="ManageLotteryTicket.aspx">管理</a></li>
                <li class="hover"><a href="javascript:void">编辑</a></li>
            </ul>
        </div>

        <div class="areacolumn clearfix">
            <div class="columnright">
                <div class="formitem validator2">
                    <ul>
                        <li class="mb_0"><span class="formitemtitle "><em>*</em>活动名称：</span>
                            <asp:TextBox ID="txtActiveName" runat="server" ClientIDMode="Static" CssClass="forminput form-control" placeholder="不能为空，且只能输入60个字"></asp:TextBox>
                            <p id="txtActiveNameTip"></p>
                        </li>
                        <li class="mb_0"><span class="formitemtitle"><em>*</em>报名开始日期：</span>
                            <Hi:CalendarPanel runat="server" ClientIDMode="Static" ID="calendarStartDate" placeholder="不能为空"></Hi:CalendarPanel>
                            <p id="calendarStartDateTip"></p>
                        </li>
                        <li class="mb_0"><span class="formitemtitle"><em>*</em>抽奖开始时间：</span>
                            <Hi:CalendarPanel runat="server" ClientIDMode="Static" ID="calendarOpenDate" placeholder="同时也是报名结束时间"></Hi:CalendarPanel>
                            <p id="calendarOpenDateTip"></p>
                        </li>
                        <li class="mb_0"><span class="formitemtitle"><em>*</em>抽奖结束日期：</span>
                            <Hi:CalendarPanel runat="server" ClientIDMode="Static" ID="calendarEndDate" placeholder="不能为空" Width="230"></Hi:CalendarPanel>
                            <p id="calendarEndDateTip"></p>
                        </li>
                        <li><span class="formitemtitle"><em>*</em>会员设置：</span>
                            <asp:CheckBoxList ID="cbList" runat="server" ClientIDMode="Static" RepeatDirection="Horizontal" CssClass="icheck">
                            </asp:CheckBoxList>
                        </li>
                        <li class="mb_0"><span class="formitemtitle"><em>*</em>人数下限：</span>
                            <asp:TextBox ID="txtMinValue" runat="server" ClientIDMode="Static" CssClass="forminput form-control" placeholder="不能为空，且只能为正整数"></asp:TextBox>
                            <p id="txtMinValueTip"></p>
                        </li>
                        <li class="mb_0"><span class="formitemtitle">邀请码：</span>
                            <asp:TextBox ID="txtCode" runat="server" ClientIDMode="Static" CssClass="forminput form-control" placeholder="留空则表示无需邀请码"></asp:TextBox>
                            <p id="txtCodeTip"></p>
                        </li>
                        <li class="mb_0"><span class="formitemtitle"><em>*</em>关键字：</span>
                            <asp:TextBox ID="txtKeyword" runat="server" ClientIDMode="Static" CssClass="forminput form-control" placeholder="不能为空"></asp:TextBox>
                            <p id="txtKeywordTip"></p>
                        </li>
                        <li class="mb_0"><span class="formitemtitle">活动简介：</span>
                            <asp:TextBox ID="txtdesc" runat="server" ClientIDMode="Static" Rows="5" Height="100px" Width="400px" CssClass="forminput form-control"
                                TextMode="MultiLine"></asp:TextBox>
                            <p id="txtdescTip"></p>
                        </li>
                        <li class="mb_0">
                            <span class="formitemtitle"><em>*</em>一等奖描述：</span>
                            <asp:TextBox ID="txtPrize1" runat="server" ClientIDMode="Static" CssClass="forminput form-control"></asp:TextBox>
                            <span>&nbsp;奖品数量：&nbsp;</span>
                            <asp:TextBox ID="txtPrize1Num" runat="server" ClientIDMode="Static" CssClass="forminput form-control"></asp:TextBox>
                            <p id="txtPrize1Tip" style="width: 300px;"></p>
                            <p id="txtPrize1NumTip" style="margin-left: 0px; width: 300px;"></p>
                        </li>
                        <li class="mb_0"><span class="formitemtitle"><em>*</em>二等奖描述：</span>
                            <asp:TextBox ID="txtPrize2" runat="server" ClientIDMode="Static" CssClass="forminput form-control"></asp:TextBox>
                            <span>&nbsp;奖品数量：&nbsp;</span>
                            <asp:TextBox ID="txtPrize2Num" runat="server" ClientIDMode="Static" CssClass="forminput form-control"></asp:TextBox>
                            <p id="txtPrize2Tip" style="width: 300px;"></p>
                            <p id="txtPrize2NumTip" style="margin-left: 0px; width: 300px;"></p>
                        </li>
                        <li class="mb_0"><span class="formitemtitle"><em>*</em>三等奖描述：</span>
                            <asp:TextBox ID="txtPrize3" runat="server" ClientIDMode="Static" CssClass="forminput form-control"></asp:TextBox>
                            <span>&nbsp;奖品数量：&nbsp;</span>
                            <asp:TextBox ID="txtPrize3Num" runat="server" ClientIDMode="Static" CssClass="forminput form-control"></asp:TextBox></li>
                        <p id="txtPrize3Tip" style="width: 300px;"></p>
                        <p id="txtPrize3NumTip" style="margin-left: 0px; width: 300px;"></p>
                        <li>
                            <span class="formitemtitle">开启四五六等奖</span>
                            <Hi:OnOff runat="server" ID="ooOpen"></Hi:OnOff>

                        </li>
                        <li class="hiddenli mb_0"><span class="formitemtitle"><em>*</em>四等奖描述：</span>
                            <asp:TextBox ID="txtPrize4" runat="server" ClientIDMode="Static" CssClass="forminput form-control"></asp:TextBox>
                            <span>&nbsp;<em>*</em>奖品数量：&nbsp;</span>
                            <asp:TextBox ID="txtPrize4Num" runat="server" ClientIDMode="Static" CssClass="forminput form-control"></asp:TextBox>
                            <p id="txtPrize4Tip" style="width: 300px;"></p>
                            <p id="txtPrize4NumTip" style="margin-left: 0px; width: 300px;"></p>

                        </li>
                        <li class="hiddenli mb_0"><span class="formitemtitle"><em>*</em>五等奖描述：</span>
                            <asp:TextBox ID="txtPrize5" runat="server" ClientIDMode="Static" CssClass="forminput form-control"></asp:TextBox>
                            <span>&nbsp;<em>*</em>奖品数量：&nbsp;</span>
                            <asp:TextBox ID="txtPrize5Num" runat="server" ClientIDMode="Static" CssClass="forminput form-control"></asp:TextBox>
                            <p id="txtPrize5Tip" style="width: 300px;"></p>
                            <p id="txtPrize5NumTip" style="margin-left: 0px; width: 300px;"></p>

                        </li>
                        <li class="hiddenli mb_0"><span class="formitemtitle"><em>*</em>六等奖描述：</span>
                            <asp:TextBox ID="txtPrize6"
                                runat="server" ClientIDMode="Static" CssClass="forminput form-control"></asp:TextBox>
                            <span>&nbsp;<em>*</em>奖品数量：&nbsp;</span>
                            <asp:TextBox ID="txtPrize6Num" runat="server" ClientIDMode="Static" CssClass="forminput form-control"></asp:TextBox>
                            <p id="txtPrize6Tip" style="width: 300px;"></p>
                            <p id="txtPrize6NumTip" style="margin-left: 0px; width: 300px;"></p>
                        </li>
                        <li>
                            <span class="formitemtitle"><em>*</em>图片封面：</span>
                            <div id="imageContainer">
                                <span name="articleImage" class="imgbox"></span>
                                <asp:HiddenField ID="hidUploadImages" runat="server" />
                                <asp:HiddenField ID="hidOldImages" runat="server" />
                            </div>
                        </li>
                    </ul>
                    <div class="ml_198">
                        <asp:Button ID="btnUpdateActivity" runat="server" OnClientClick="return getUploadImages();"
                            Text="保 存" CssClass="btn btn-primary" OnClick="btnUpdateActivity_Click" />
                    </div>
                </div>
            </div>
        </div>
        <div class="databottom">
            <div class="databottom_bg">
            </div>
        </div>
        <div class="bottomarea testArea">
            <!--顶部logo区域-->
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
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
                               pictureSize: '320*200',
                               imagesCount: 1,
                               dataWidth: 9
                           });

        }

        //获取上传成功后的图片路径
        function getUploadImages() {
            if (!PageIsValid()) return false;
            var srcImg = $('#imageContainer span[name="articleImage"]').hishopUpload("getImgSrc");
            $("#<%=hidUploadImages.ClientID%>").val(srcImg);
            if (srcImg == "") {
                alert("请上传图片封面");
                return false;
            }
            return true;
        }


        $(document).ready(function () { InitValidators(); initImageUpload(); });
        function CheckOpen(Open) {
            if (Open) {
                initValid(new InputValidator('txtPrize4', 1, 60, false, null, '奖品描述不能为空'));
                initValid(new InputValidator('txtPrize5', 1, 60, false, null, '奖品描述不能为空'));
                initValid(new InputValidator('txtPrize6', 1, 60, false, null, '奖品描述不能为空'));
                initValid(new InputValidator('txtPrize4Num', 1, 10, false, '[0-9]\\d*', '不能为空，且必须为1-10位的整数'));
                initValid(new InputValidator('txtPrize5Num', 1, 10, false, '[0-9]\\d*', '不能为空，且必须为1-10位的整数'));
                initValid(new InputValidator('txtPrize6Num', 1, 10, false, '[0-9]\\d*', '不能为空，且必须为1-10位的整数'));
                $(".hiddenli").show();
            }
            else {
                removeVerification("txtPrize4,txtPrize5,txtPrize6,txtPrize4Num,txtPrize5Num,txtPrize6Num");
                $(".hiddenli").hide();
            }
        }
        function InitValidators() {
            initValid(new InputValidator('txtActiveName', 1, 60, false, null, '不能为空，长度限制在60个字符以内'));
            initValid(new InputValidator('txtKeyword', 1, 60, false, null, '不能为空，长度限制在60个字符以内'));
            initValid(new InputValidator('txtMinValue', 1, 10, false, '[0-9]\\d*', '人数下限必须是整数'));
            initValid(new InputValidator('txtPrize1', 1, 60, false, null, '奖品描述不能为空'));
            initValid(new InputValidator('txtPrize2', 1, 60, false, null, '奖品描述不能为空'));
            initValid(new InputValidator('txtPrize3', 1, 60, false, null, '奖品描述不能为空'));
            initValid(new InputValidator('txtPrize1Num', 1, 10, false, '[0-9]\\d*', '不能为空，且必须为1-10位的整数'));
            initValid(new InputValidator('txtPrize2Num', 1, 10, false, '[0-9]\\d*', '不能为空，且必须为1-10位的整数'));
            initValid(new InputValidator('txtPrize3Num', 1, 10, false, '[0-9]\\d*', '不能为空，且必须为1-10位的整数'));
            initValid(new InputValidator('calendarStartDate', 1, 50, false, null, '不能为空'));
            initValid(new InputValidator('calendarOpenDate', 1, 50, false, null, '不能为空'));
            initValid(new InputValidator('calendarEndDate', 1, 50, false, null, '不能为空'));

            initValid(new InputValidator('txtCode', 1, 60, true, null, '长度限制在60个字符以内'));
            initValid(new InputValidator('txtdesc', 1, 500, true, null, '长度限制在500个字符以内'));
            if ($('input[name="ctl00$contentHolder$ctl00"]').bootstrapSwitch('state') == false) {
                CheckOpen(false);
            } else {
                CheckOpen(true);
            }
            $('input[name="ctl00$contentHolder$ctl00"]').on('switchChange.bootstrapSwitch', function (event, state) {
                CheckOpen(state);
            });
        }
        $(document).ready(function () { InitValidators(); initImageUpload(); });
    </script>
</asp:Content>
