<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddDeliveryScop.aspx.cs" Inherits="Hidistro.UI.Web.Admin.AddDeliveryScop" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script language="javascript" type="text/javascript">
        function doSave() {
            // var RegionId = $("").val();
            var RegionId = $("#regionSelectorValue").val();

            var RegionName = $("#regionSelectorName").val();

            
            if (RegionId == "" || RegionId == undefined || RegionId == "0") {
                alert("请选择一个区域");
            }
            else {
                artDialog.open.origin.SaveDeliveryScop(RegionId, RegionName);
                art.dialog.close();
            }

        }
    </script>
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

<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="datafrom">
        <div class="formitem validator1" style="height: 240px;">
            <ul style="margin: 0px 0 0 20px;">
                <li><span class="formitemtitle">选择配送区域：</span><p></p>
                </li>
                <li>
                    <Hi:RegionSelector ID="dropRegion" runat="server" IsShift="false" />
                </li>
            </ul>
        </div>
    </div>
    <div class="modal_iframe_footer">
        <input  type="button" id="saveBtn" onclick="doSave()" class="btn btn-primary" value="保存" />

    </div>
</asp:Content>
