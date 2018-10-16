<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ReferralsLink.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ReferralsLink" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>


<%@ Import Namespace="Hidistro.Core" %>
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
            padding: 20px 20px 90px 20px;
            width: 100% !important;
        }
        #btnCopy{
            float:right;
        }
      
    </style>
    <div class="areacolumn clearfix">
        <div class="columnright">
            <div class="formitem validator2">
                <ul>
                    <li>
                        <span class="formitemtitle" style="width:80px;">分销链接：</span>
                        <asp:Label runat="server" ID="lblReferralsLink" Text=""></asp:Label>&nbsp;&nbsp;
                        <input type="button" class="btn btn-default" value="复制" id="btnCopy" />
                    </li>
                    <li>
                        <span class="formitemtitle" style="width:80px;">二维码：</span>
                        <img id="imgsrc" type="img" width="100px" />
                    </li>
                </ul>
            </div>
        </div>
    </div>
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
