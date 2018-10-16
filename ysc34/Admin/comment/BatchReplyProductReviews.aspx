<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="BatchReplyProductReviews.aspx.cs" Inherits="Hidistro.UI.Web.Admin.BatchReplyProductReviews" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <style>
        html {
            background: #fff !important;
        }

        body {
            padding: 0;
            width: 100%;
        }

        #mainhtml {
            margin: 0;
            padding: 20px 20px 60px 20px;
            width: 100% !important;
        }

        abbr {
            float: left;
            width: 400px;
            word-wrap: break-word;
        }
    </style>
    <script type="text/javascript">
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtReply', 1, 500, false, null, '回复不能为空，长度限制在500个字符以内'))
        }

        $(document).ready(function () {
            InitValidators();
            //$input.rating();
        });
    </script>
    <link rel="stylesheet" href="/Utility/star/star-rating.css" rev="stylesheet" type="text/css">
    <link href="/Utility/hishopUpload/hishopUpload.css" rel="stylesheet" />
    <script src="/Utility/star/star-rating.js"></script>  
    <div class="dataarea mainwidth databody">


        <div class="datafrom">
            <div class="formitem">
                <ul>
                    <li><span class="formitemtitle Pw_100" style="width: 138px;">回复：</span>
                        <span id="text_null">
                            <asp:Literal runat="server" ID="lblReply"></asp:Literal></span>
                        <span style="float: left; position: relative; z-index: 0;width:700px;" class="wordCount" id="wordCount">
                            <asp:TextBox ID="txtReply" runat="server" TextMode="MultiLine" Height="100" Width="550" CssClass="forminput form-control"></asp:TextBox>
                            <span class="word" style="width:550px;text-align:right;color:#999;">500</span>
                        </span>
                    </li>
                </ul>
                <div class="modal_iframe_footer">
                    <asp:Button ID="btnReplyProductConsultation"  OnClientClick="return PageIsValid();" runat="server" Text="批量回复" CssClass="btn btn-primary" />
                </div>
            </div>
        </div>
    </div>
    <div class="bottomarea testArea">
        <!--顶部logo区域-->
    </div>
    <script>
        $(function () {
            if ($("#text_null").html() != null) {
                $(".word").hide();
            }


            //先选出 textarea 和 统计字数 dom 节点
            var wordCount = $("#wordCount"),
                textArea = wordCount.find("textarea"),
                word = wordCount.find(".word");
            //调用
            statInputNum(textArea, word);

        });
        /*
        * 剩余字数统计
        * 注意 最大字数只需要在放数字的节点哪里直接写好即可 如：<var class="word">200</var>
        */
        function statInputNum(textArea, numItem) {
            var max = numItem.text(),
                curLength;
            textArea[0].setAttribute("maxlength", max);
            curLength = textArea.val().length;
            numItem.text(max - curLength);
            textArea.on('input propertychange', function () {
                numItem.text(max - $(this).val().length);
            });
        }
</script>

</asp:Content>
