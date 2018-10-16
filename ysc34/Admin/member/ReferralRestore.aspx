<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ReferralRestore.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ReferralRestore" %>
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
            <div class="formitemtitle">
                <h4>确认恢复该分销员身份？</h4>
            </div>
            <div class="formitem_con">分销员恢复身份后，他的原下级会员将继续归属于该分销员。</div>
            
            <div class="modal_iframe_footer">
                <asp:Button ID="btnConfirm" runat="server" Text="确 定" CssClass="btn btn-primary" OnClick="btnConfirm_Click" />
                <input type="button" class="btn btn-default" value="取 消" onclick="javascript: art.dialog.close();" />
            </div>
        </div>
    </div>
</asp:Content>
