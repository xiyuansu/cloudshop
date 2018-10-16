<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="SetTemplates.aspx.cs" Inherits="Hidistro.UI.Web.Admin.SetTemplates" %>

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
    </style>
    <div class="areacolumn clearfix">
        <div class="columnright" style="width: 100%;">

            <div class="formitem">
                <div class="frame-content" style="padding-bottom: 20px;">
                    <ul>
                        <li style="height:250px;"><span>请选择运费模板：</span><span>
                            <Hi:ShippingTemplatesDropDownList ID="dropShippingTemplateId" ClientIDMode="Static" runat="server" NullToDisplay="请选择运费模板" CssClass="iselect" />

                        </span></li>
                    </ul>
                </div>
                <div class="modal_iframe_footer">
                    <asp:Button ID="btnSetTemplates" runat="server" Text="确  定" OnClientClick="return checkSubmit()" CssClass="btn btn-primary" />
                </div>
            </div>

        </div>
    </div>
    <script src="producttag.helper.js"></script>
    <script type="text/javascript" src="../js/bootstrap-switch.js"></script>
    <script language="javascript" type="text/javascript">
  
        function checkSubmit() {
            if ($("#dropShippingTemplateId").val() == "") {
                alert("请选择一个运费模板。")
                return false;
            }

            return true;
        }

    </script>
</asp:Content>
