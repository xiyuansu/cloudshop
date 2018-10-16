<%@ Page Title="" Language="C#" MasterPageFile="~/Supplier/Admin.Master" AutoEventWireup="true" CodeBehind="OrderRemark.aspx.cs" Inherits="Hidistro.UI.Web.Supplier.sales.OrderRemark" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>


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
            padding: 20px 20px 90px 20px;
            width: 100% !important;
        }

      
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script language="javascript" type="text/javascript">
        $(document).ready(function (e) {
            $('input[name="ctl00$contentHolder$orderRemarkImageForRemark"]').on('ifChecked', function (event) {
                $("#hidRemarkImage").val($(this).val());
            });
        });
        function ValidationRemark() {
            var remarkImage = $("#hidRemarkImage").val();
            if (remarkImage == "") {
                alert("请选择标志");
                return false;
            }
            var remark = $("#ctl00_contentHolder_txtRemark").val();
            if (remark == ""||remark.length>300) {
                alert("请输入备忘录，字符长度限制在300个字符以内");
                return false;
            }
            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <input type="hidden" id="hidRemarkImage" value="" runat="server" clientidmode="Static" />
    <div class="frame-content">
        <p><span class="frame-span frame-input110">订单编号：</span><asp:Literal ID="spanOrderId" runat="server" /></p>

        <p><span class="frame-span frame-input110">成交时间：</span><hi:formatedtimelabel runat="server" id="lblorderDateForRemark" /></p>
        <p style="display: none;">
            <span class="frame-span frame-input110">订单实收款(元)：</span><em><hi:formatedmoneylabel
                id="lblorderTotalForRemark" runat="server" /></em>
        </p>
        <span class="frame-span frame-input110">标志：</span><hi:orderremarkimageradiobuttonlist runat="server" repeatdirection="Horizontal" cellpadding="10" cellspacing="10" cssclass="icheck" id="orderRemarkImageForRemark" />

        <p><span class="frame-span frame-input110">备忘录：</span><asp:TextBox ID="txtRemark" TextMode="MultiLine" placeholder="长度限制在300个字符以内" runat="server" Width="300" Height="60" CssClass="forminput form-control" /></p>
    </div>
     <div class="modal_iframe_footer">
            <asp:Button ID="btnModifyRemark" runat="server" Text="确 定" CssClass="btn btn-primary" OnClientClick="return ValidationRemark()" OnClick="btnModifyRemark_Click" />
            <input type="button" class="btn btn-default" value="取 消" onclick="javascript: art.dialog.close();" />
        </div>
    <script>
        $(function () {

        })
    </script>
</asp:Content>
