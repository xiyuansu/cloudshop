<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddQRCodeHiPOS.aspx.cs" Inherits="Hidistro.UI.Web.Admin.depot.AddQRCodeHiPOS" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>


<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <style>
        body {
            padding: 0px;
            width: 350px;
        }

        #mainhtml {
            margin: 0px;
            width: 350px;
            padding: 0px 20px;
        }
    </style>
    <script type="text/javascript">
        var intervalHiPOS = 0;
        $(function () {
            ShowShareLink();
        })
        function ShowShareLink() {
            var url = "<%=this.Url%>";
            $("#ctl00_contentHolder_imgQRCode").attr("src", "/api/VshopProcess.ashx?action=GenerateTwoDimensionalImage&url=" + encodeURIComponent(url));
        }
        setTimeout(function () {
            intervalHiPOS = setInterval(function () {
                $.ajax({
                    type: "POST",
                    url: "/API/HiPOSAPI.ashx?action=authqrCheck",
                    dataType: "json",
                    async: false,
                    success: function (d) {
                        var result = eval(d);
                        if (result.Status == 0) {
                            var alias = "设备名称：" + result.Alias;
                            var storeHiPOSId = result.StoreHiPOSId;
                            $("#liAlias").show().text(alias);
                            $.ajax({
                                type: "POST",
                                url: "/API/HiPOSAPI.ashx?action=setstorehipos",
                                data:{storeHiPOSId:storeHiPOSId},
                                dataType: "json",
                                async: false,
                                success: function (f) {
                                    var res = eval(f);
                                    if (res.Status == 1) {
                                        $("#spHiPOSBindStatus").text("绑定成功！").css("color", "green").css("width", "300px").css("text-align", "center").css("font-size", "20px");
                                    }
                                    else {
                                        $("#spHiPOSBindStatus").text("绑定失败！").css("color", "red").css("width", "300px").css("text-align", "center").css("font-size", "20px");
                                    }
                                }
                            });


                            clearInterval(intervalHiPOS);
                            setTimeout(function () {
                                $("#ctl00_contentHolder_btnClose").trigger("click");
                            }, 3000);

                        }
                    }
                });
            }, 2000);
        }, 5000);
    </script>
    <div class="dataarea mainwidth databody" style="width: 300px;">
        <div class="datafrom" style="padding: 0px;">
            <div class="formitem validator1">
                <ul>
                    <li style="text-align: center; margin-bottom: 10px; font-weight: bold; font-size: 20px;">
                        <asp:Literal ID="ltStoreName" runat="server"></asp:Literal>
                    </li>
                    <li style="text-align: center; margin-bottom: 10px;">
                        <asp:Image ID="imgQRCode" runat="server" Width="180" Height="180" />
                    </li>
                    <li style="text-align: center; margin-bottom: 10px; display: none;" id="liAlias">
                        <asp:Literal ID="ltHiPOSAlias" runat="server"></asp:Literal>
                    </li>
                    <li style="margin-bottom: 0px;"><span style="padding: 0 5px; line-height: 1.8; color: #808080" id="spHiPOSBindStatus">说明：请将需绑定的POS机对准该二维码，扫码成功后即完成绑定。
                    </span>

                    </li>
                </ul>

            </div>
        </div>

    </div>

    <asp:Button ID="btnClose" runat="server" Text="Button" Style="display: none;" OnClick="btnClose_Click" />
</asp:Content>
