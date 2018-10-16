<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master"  AutoEventWireup="true" CodeBehind="BigWheelLink.aspx.cs" Inherits="Hidistro.UI.Web.Admin.vshop.BigWheelLink" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>


<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content2" ContentPlaceHolderID="headHolder" runat="server">
    <meta name="Generator" content="EditPlus">  
</asp:Content>
  
<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="Server">
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
            /*padding: 20px 20px 90px 20px;*/
            padding: 0;
            width: 100% !important;
        }
        #btnCopy{
            float:none !important;
            margin-left:30px;
        }
        #ctl00_contentHolder_lblReferralsLink{
            width:100%;
            margin-top:10px;
        }
        .copy{
            float:left;
            width:100%;
            margin-top:10px;
        }
      
    </style>
    <asp:HiddenField runat="server" ID="hidActivityID" />
    <asp:HiddenField runat="server" ID="hidTypeId" />
    <asp:HiddenField runat="server" ID="hidClientName" />
    <div class="areacolumn clearfix">
        <div class="columnright">
            <div class="formitem validator2">
                <div style="text-align:center">
                        <img id="imgsrc" name="imgSrc" type="img" width="100px" />
                        <%--<span style="text-align:center;width:100%;margin-top:10px;">扫一扫立即参与活动</span>--%>
                        <asp:Label runat="server" ID="lblReferralsLink" Text=""></asp:Label>
                    <div class="copy">
                        <a href="#" onclick="$('#ctl00_contentHolder_btnDownLoad').click()">下载二维码</a>
                        <a  href="#"  id="btnCopy" >复制链接</a>

                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:Button runat="server" ID="btnDownLoad" OnClick="btnDownLoad_Click" style="display:none" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" src="../js/zclip/jquery.zclip.min.js"></script>
    <script type="text/javascript">
        $(function () {
            $('#btnCopy').zclip({
                path: "../js/zclip/ZeroClipboard.swf",
                copy: function () {
                    return $('#ctl00_contentHolder_lblReferralsLink').text();
                },
                afterCopy: function () {
                    alert("成功复制到剪切板：" + $('#ctl00_contentHolder_lblReferralsLink').text());
                }
            });
            showReferralsLink();
        });

        function showReferralsLink() {
            var surl = $("#ctl00_contentHolder_lblReferralsLink").text();
            $("#imgsrc").attr("src", "/api/VshopProcess.ashx?action=GenerateTwoDimensionalImage&url=" + surl);
        }
    </script>
</asp:Content>
