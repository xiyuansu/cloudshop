<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="RealNameCertification.aspx.cs" Inherits="Hidistro.UI.Web.Admin.sales.RealNameCertification" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <style type="text/css">
        html {
            background: #fff !important;
        }

        body {
            padding: 0;
            width: 100%;
        }

        .idImage {
            width: 280px;
            height: 80px;
        }

            .idImage img {
                width: 120px;
                margin: 3px;
                height: 74px;
            }

        table td.td_right {
            text-align: right;
            width: 20%;
        }
        /*图片放大*/
        .zoomImg {
            display: none;
            width: 350px;
            height: 210px;
            position: absolute;
            align-content: center;
            left: 120px;
            top: 80px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
     <script type="text/javascript">
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtIDRemark', 1, 100, false, null, '备注在1至100个字符之间'));
        }

        $(document).ready(function () {

            if ($("#hidCertificationModel").val() != '2') {
                $("#IDimg").hide();
            }
            //放大图片
            $("#IDImageJust,#IDImageAnti").click(function () {
                if ($(this).attr("src") != "" && typeof ($(this).attr("src")) != "undefined") {
                    var id = "#" + $(this).attr("id") + "_zoom";
                    $(id).attr("src", $(this).attr("src"))
                    $(id).show();
                }
            })

            $("#IDImageJust_zoom,#IDImageAnti_zoom").click(function () {
                $(this).hide();
            })
            //
            $("#btnRefuse").click(function () {
                InitValidators();
            })
        });

    </script>
    <script type="text/javascript" src="/utility/jquery.cssforms.js"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
   <div class="list">
        <input type="hidden" runat="server" clientidmode="Static" id="hidCertificationModel" />
        <div class="Settlement">
            <table width="100%" border="0" cellspacing="0" class="table">
                <tr>
                    <td class="td_right">收货人：</td>
                    <td>
                        <asp:Label runat="server" ID="lblShipTo" Width="280px" />
                    </td>
                </tr>
                <tr>
                    <td class="td_right">真实姓名：</td>
                    <td>
                        <asp:Label runat="server" ID="lblRealName" Width="280px" />
                    </td>
                </tr>
                <tr>
                    <td class="td_right">身份证号：</td>
                    <td>
                        <asp:Label runat="server" ID="lblIDNumber" Width="280px" />
                    </td>
                </tr>
                <tr id="IDimg">
                    <td class="td_right">身份证照片：</td>
                    <td>
                        <div class="idImage">
                            <img src="" id="IDImageJust" runat="server" alt="证件照正面" clientidmode="Static" />
                            <img src="" id="IDImageAnti" runat="server" alt="证件照反面" clientidmode="Static" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="td_right">备注：</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtIDRemark" Width="280px" class="forminput form-control" />
                        <span id="txtIDRemarkTip" runat="server"></span>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div>
        <img src="" id="IDImageJust_zoom" runat="server" class="zoomImg" clientidmode="Static" />
        <img src="" id="IDImageAnti_zoom" runat="server" class="zoomImg" clientidmode="Static" />
    </div>
    <div class="modal_iframe_footer">
        <asp:Button ID="btnPass" runat="server" Text="通过认证" OnClientClick="return PageIsValid();" class="btn btn-primary" />
        <asp:Button ID="btnRefuse" runat="server" Text="拒绝" OnClientClick="return PageIsValid();" class="btn btn-primary" ClientIDMode="Static" />
    </div>
</asp:Content>
