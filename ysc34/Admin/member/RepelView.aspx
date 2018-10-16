<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="RepelView.aspx.cs" Inherits="Hidistro.UI.Web.Admin.RepelView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <style>
        html { background: #fff !important; color: #000; }

        body { padding: 0; width: 100%; }

        #mainhtml { margin: 0; padding: 20px 20px 90px 20px; width: 100% !important; }
        #btnCopy { float: right; }
    </style>
    <div class="areacolumn clearfix">
        <div class="columnright">
            <div class="frame-content" style="width: 400px">
                <p>
                    <span class="frame-span frame-span" style="width: auto;">清退时间：</span>
                    <asp:Literal ID="litRepelTime" runat="server"></asp:Literal>
                </p>
                <p id="remarkRow" runat="server">
                    <span class="frame-span frame-span" style="width: auto;">商家备注：</span>
                    <asp:Literal ID="litRepelRemark" runat="server"></asp:Literal>
                </p>
            </div>
            <div class="modal_iframe_footer">
                <input type="button" class="btn btn-default" value="返回" onclick="javascript: art.dialog.close();" />
            </div>
        </div>
    </div>
</asp:Content>
