<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="CouponLink.aspx.cs" Inherits="Hidistro.UI.Web.Admin.CouponLink" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>




<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="Server">
    
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="newcoupons.aspx">管理</a></li>
                <li class="hover"><a href="javascript:void">优惠券链接</a></li>
            </ul>
        </div>
    </div>
    <div class="areacolumn clearfix">
        <div class="columnright">
            <div class="formitem validator2">
                <ul>
                    <li>
                        <span class="formitemtitle">优惠券链接：</span>
                        <asp:Label runat="server" ID="lblCouponLink" Text=""></asp:Label>&nbsp;&nbsp;
                        <input type="button" class="btn btn-default" value="复制" id="btnCopy" />
                    </li>
                    <li>
                        <span class="formitemtitle">二维码：</span>
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
                    return $('#ctl00_contentHolder_lblCouponLink').text();
                },
                afterCopy: function () {
                    alert("成功复制到剪切板：" + $('#ctl00_contentHolder_lblCouponLink').text());
                }
            });
            showCouponLink();
        });

        function showCouponLink() {
            var surl = $("#ctl00_contentHolder_lblCouponLink").text();
            $("#imgsrc").attr("src", "/api/VshopProcess.ashx?action=GenerateTwoDimensionalImage&url=" + surl);
        }
    </script>
</asp:Content>
