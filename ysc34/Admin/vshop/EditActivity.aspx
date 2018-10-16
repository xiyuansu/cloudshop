<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditActivity.aspx.cs" Inherits="Hidistro.UI.Web.Admin.EditActivity" MasterPageFile="~/Admin/Admin.Master" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>




<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="server">
    <script type="text/javascript" src="/utility/jquery.hishopUpload.js" ></script>
    <script type="text/javascript" src="/utility/jquery.form.js"></script>
    <link id="cssLink" rel="stylesheet" href="../css/style.css" type="text/css" media="screen" />
    <div class="areacolumn clearfix databody">
            <div class="title">
                <ul class="title-nav">
                    <li><a href="ManageActivity.aspx">管理</a></li>                       
                    <li  class="hover"><a href="javascript:void">编辑</a></li>
                    
                </ul>
            </div>
        <div class="columnright">
            <div class="formitem validator2">
                <ul>
                    <li class="mb_0"><span class="formitemtitle Pw_100"><em>*</em>活动名称：</span>
                        <asp:TextBox ID="txtName" runat="server" CssClass="forminput" placeholder="不能为空"></asp:TextBox>
                        <p id="ctl00_contentHolder_txtNameTip"></p>
                    </li>
                    <li class="mb_0"><span class="formitemtitle Pw_100"><em>*</em>开始时间：</span>
                    <Hi:CalendarPanel runat="server" ID="txtStartDate" placeholder="不能为空"></Hi:CalendarPanel>
                        <p id="ctl00_contentHolder_txtStartDateTip"></p>
                    </li>
                    <li class="mb_0"><span class="formitemtitle Pw_100"><em>*</em>结束时间：</span>
                    <Hi:CalendarPanel runat="server" ID="txtEndDate" placeholder="不能为空"></Hi:CalendarPanel>
                        <p id="ctl00_contentHolder_txtEndDateTip"></p>
                    </li>
                    <li class="mb_0"><span class="formitemtitle Pw_100">人数上限：</span>
                        <asp:TextBox ID="txtMaxValue" runat="server" CssClass="forminput" placeholder="不填写，则无上限"></asp:TextBox>
                        <p id="ctl00_contentHolder_txtMaxValueTip"></p>
                    </li>
                    <li class="mb_0"><span class="formitemtitle Pw_100">活动简介：</span>
                        <asp:TextBox ID="txtDescription" runat="server" Width="400px" Height="100px" TextMode="MultiLine"></asp:TextBox>
                        <p id="ctl00_contentHolder_txtDescriptionTip"></p>
                    </li>
                    <li class="mb_0"><span class="formitemtitle Pw_100"><em>*</em>活动表单项一：</span>
                        <asp:TextBox ID="txtItem1" runat="server" CssClass="forminput" placeholder="不能为空"></asp:TextBox>
                        <p id="ctl00_contentHolder_txtItem1Tip"></p>
                    </li>
                    <li class="mb_0"><span class="formitemtitle Pw_100">表单项二：</span>
                        <asp:TextBox ID="txtItem2" runat="server" CssClass="forminput"></asp:TextBox>
                        <p id="ctl00_contentHolder_txtItem2Tip"></p>
                    </li>
                    <li class="mb_0"><span class="formitemtitle Pw_100">表单项三：</span>
                        <asp:TextBox ID="txtItem3" runat="server" CssClass="forminput"></asp:TextBox>
                        <p id="ctl00_contentHolder_txtItem3Tip"></p>
                    </li>
                    <li class="mb_0"><span class="formitemtitle Pw_100">表单项四：</span>
                        <asp:TextBox ID="txtItem4" runat="server" CssClass="forminput"></asp:TextBox>
                        <p id="ctl00_contentHolder_txtItem4Tip"></p>
                    </li>
                    <li class="mb_0"><span class="formitemtitle Pw_100">表单项五：</span>
                        <asp:TextBox ID="txtItem5" runat="server" CssClass="forminput"></asp:TextBox>
                        <p id="ctl00_contentHolder_txtItem5Tip"></p>
                    </li>
                    <li style="display: none"><span class="formitemtitle Pw_100">结束说明：</span>
                        <asp:TextBox ID="txtCloseRemark" runat="server" Width="400px" Height="100px" TextMode="MultiLine"></asp:TextBox>
                        <p id="ctl00_contentHolder_txtCloseRemarkTip"></p>
                    </li>
                    <li class="mb_0"><span class="formitemtitle Pw_100"><em>*</em>关键字：</span>
                        <asp:TextBox ID="txtKeys" runat="server" CssClass="forminput" placeholder="同时作为图文推送的标题来使用"></asp:TextBox>
                        <p id="ctl00_contentHolder_txtKeysTip"></p>
                    </li>
                    <li class="mb_0">
                        <span class="formitemtitle Pw_100"><em>*</em>图片封面：</span>
                        <div id="imageContainer">
                            <span name="articleImage" class="imgbox"></span>
                            <asp:HiddenField ID="hidUploadImages" runat="server" />
                            <asp:HiddenField ID="hidOldImages" runat="server" />
                        </div>
                        <p id="ctl00_contentHolder_hidUploadImagesTip"></p>
                    </li>
                </ul>
                <ul class="ml_198">
                    <asp:Button ID="btnEditActivity" runat="server" Text="修改" OnClientClick="return getUploadImages();" CssClass="btn btn-primary" />
                </ul>
            </div>
        </div>
    </div>
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

        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtName', 1, 100, false, null, '活动名称不能为空，在1至100个字符之间'));
            initValid(new InputValidator('ctl00_contentHolder_txtKeys', 1, 50, false, null, '关键字不能为空，在1至50个字符之间'));
            initValid(new InputValidator('ctl00_contentHolder_txtStartDate', 1, 50, false, null, '开始时间不能为空 '));
            initValid(new InputValidator('ctl00_contentHolder_txtEndDate', 1, 50, false, null, '结束时间不能为空'));
            initValid(new InputValidator('ctl00_contentHolder_txtItem1', 1, 20, false, null, '活动表单项一不能为空，字数限制在1-20字'));

            initValid(new InputValidator('ctl00_contentHolder_txtMaxValue', 1, 10, true, '[0-9]\\d*', '人数上限必须是整数，且在0-9999999999之间'));
            initValid(new InputValidator('ctl00_contentHolder_txtDescription', 1, 500, true, null, '活动简介字数限制在0-500字内'));
            initValid(new InputValidator('ctl00_contentHolder_txtItem2', 1, 20, true, null, '活动表单项二字数限制在1-20字'));
            initValid(new InputValidator('ctl00_contentHolder_txtItem3', 1, 20, true, null, '活动表单项三字数限制在1-20字'));
            initValid(new InputValidator('ctl00_contentHolder_txtItem4', 1, 20, true, null, '活动表单项四字数限制在1-20字'));
            initValid(new InputValidator('ctl00_contentHolder_txtItem5', 1, 20, true, null, '活动表单项五字数限制在1-20字'));
        }
        $(document).ready(function () { InitValidators(); initImageUpload(); });
    </script>
</asp:Content>
