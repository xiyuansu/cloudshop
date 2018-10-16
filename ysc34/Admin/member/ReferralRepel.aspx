<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ReferralRepel.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ReferralRepel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script language="javascript" type="text/javascript">
        function checkInput() {
            if ($("#txtRemark").val().length > 500) {
                alert("备注信息长度不能超过500");
                return false;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <style>
        html { background: #fff !important; color: #000; }

        body { padding: 0; width: 100%; }

        #mainhtml { margin: 0; padding: 0px 20px 90px 20px; width: 100% !important; }
        #btnCopy { float: right; }
    </style>
    <div class="areacolumn clearfix">
        <div class="columnright">
          
            <div class="formitem_con">清退后不影响该分销员作为会员所享受的权益<br />商家备注：</div>

            <div class="frame-content" style="width:400px">
                <p>
                        <asp:TextBox TextMode="MultiLine" ID="txtRemark" ClientIDMode="Static" CssClass="forminput form-control" Height="80px" Width="390px" runat="server"></asp:TextBox>
                </p>
            </div>
            <div class="modal_iframe_footer">
                <asp:Button ID="btnConfirm" runat="server" Text="确 定" CssClass="btn btn-primary" OnClientClick="return checkInput()" OnClick="btnConfirm_Click" />
                <input type="button" class="btn btn-default" value="取 消" onclick="javascript: art.dialog.close();" />
            </div>
        </div>
    </div>
</asp:Content>
