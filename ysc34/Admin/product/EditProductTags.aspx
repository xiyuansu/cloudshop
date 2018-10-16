<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="EditProductTags.aspx.cs" Inherits="Hidistro.UI.Web.Admin.EditProductTags" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <link href="../css/bootstrap-switch.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="Server">
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
    <div class="areacolumn clearfix">
        <div class="columnright" style="width: 500px;">

            <div class="formitem">
                <div class="frame-content" style="width:770px;padding-bottom: 20px;">
                    <Hi:ProductTagsLiteral ID="litralProductTag" runat="server"></Hi:ProductTagsLiteral>
                    <span style="display:none;">
                    <Hi:TrimTextBox runat="server" ID="txtProductTag" TextMode="MultiLine" ></Hi:TrimTextBox></span>
                </div>
                <div class="modal_iframe_footer">
                    <asp:Button ID="btnUpdateProductTags" runat="server" Text="确  定" OnClientClick="return checkSubmit()" CssClass="btn btn-primary" />
                </div>
            </div>

        </div>
    </div>
    <script src="producttag.helper.js"></script>
    <script type="text/javascript" src="../js/bootstrap-switch.js"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function (e) {
            AddAccountInitValidators();
        });
        function AddAccountInitValidators() {

        }
        function GetproductTags() {
            var v_str = "";

            $("input[type='checkbox'][name='productTags']:checked").each(function (rowIndex, rowItem) {
                v_str += $(rowItem).attr("value") + ",";
            });

            if (v_str.length == 0) {
                alert("请选择商品标签");
                return false;
            }
            return v_str.substring(0, v_str.length - 1);
        }

        function checkSubmit() {
            GetproductTags();

            return true;
        }

    </script>
</asp:Content>
