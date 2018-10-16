<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="FightGroupShareLink.aspx.cs" Inherits="Hidistro.UI.Web.Admin.FightGroupShareLink" %>



<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
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
       
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <script src="../../Utility/clipboard/clipboard.js"></script>
    <script type="text/javascript" src="../js/zclip/jquery.zclip.min.js"></script>
    <script type="text/javascript">
        $(function () {
            $("#imgQRCode").attr("src", "/api/VshopProcess.ashx?action=GenerateTwoDimensionalImage&url=" + $("#spUrl").text());
            $('#d_clip_button').zclip({
                path: "../js/zclip/ZeroClipboard.swf",
                copy: function () {
                    return $('#spUrl').text();
                },
                afterCopy: function () {
                    alert("成功复制到剪切板：" + $('#spUrl').text());
                }
            });
        })
    </script>


    <div >
        <ul>
            <li><span class="formitemtitle">活动链接：</span>
                <span id="spUrl" style="float:left; margin-right:20px;"><asp:Literal ID="ltActivityUrl" runat="server"></asp:Literal></span>
                <div id="d_clip_container">
                    <div id="d_clip_button"  class="btn btn-default">复制</div>
                </div>                
            </li>
            <li style="margin-top:20px;"><span>二维码：</span><img id="imgQRCode" width="100" height="100" style="margin-left:20px;" />
            </li>
            
           
        </ul>

    </div>





</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
