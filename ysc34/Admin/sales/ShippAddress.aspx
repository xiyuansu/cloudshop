<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ShippAddress.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ShippAddress" %>

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
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">

    <%--收货地址--%>

    <div id="dlgShipTo">
        <div class="frame-content">
            <p>
                <span class="frame-span frame-input90"><em>*</em>收货人姓名：</span>
                <asp:TextBox ID="txtShipTo" runat="server" Width="320px" CssClass="forminput form-control" placeholder="收货人名字不能为空，长度在2-20个字符之间"></asp:TextBox>
            </p>
            <div style="margin-top: 5px;">
                <span class="frame-span frame-input90">收货人地址：</span>
                <Hi:RegionSelector runat="server" ID="dropRegions" IsShift="false" />
            </div>
            <p style="margin-top: 40px;">
                <span class="frame-span frame-input90"><em>*</em>详细地址：</span>
                <asp:TextBox ID="txtAddress" runat="server" CssClass="forminput form-control" placeholder="详细地址不能为空，长度在3-200个字符之间" TextMode="multiLine" Style="width:320px;height:60px;"></asp:TextBox>
            </p>
            <p style="display:none;">
                <span class="frame-span frame-input90">邮政编码：</span>
                <asp:TextBox ID="txtZipcode" runat="server" CssClass="forminput form-control" placeholder="请填写6位数字邮编" Width="320px"></asp:TextBox>
            </p>
            <p><span class="frame-span frame-input90">电话号码：</span><asp:TextBox ID="txtTelPhone" Width="320px" runat="server" placeholder="电话号码长度在3-20个字符之间" CssClass="forminput form-control"></asp:TextBox></p>
            <p><span class="frame-span frame-input90">手机号码：</span><asp:TextBox ID="txtCellPhone" Width="320px" runat="server" placeholder="请填写正确的手机号码" CssClass="forminput form-control"></asp:TextBox></p>
        </div>
        <div class="modal_iframe_footer">
            <asp:Button ID="btnMondifyAddress" runat="server" Text="确 定" CssClass="btn btn-primary" OnClientClick="return ValidationAddress()" />
            <input type="button" class="btn btn-default" value="取 消" onclick="javascript: art.dialog.close();" />
        </div>
    </div>

    <script>
        function ValidationAddress() {
            arrytext = null;
            var shipTo = document.getElementById("ctl00_contentHolder_txtShipTo").value;
            if (shipTo.length < 2 || shipTo.length > 20) {
                alert("收货人名字不能为空，长度在2-20个字符之间");
                return false;
            }
            var address = document.getElementById("ctl00_contentHolder_txtAddress").value;
            if (address.length < 3 || address.length > 200) {
                alert("详细地址不能为空，长度在3-200个字符之间");
                return false;
            }
            var telPhone = document.getElementById("ctl00_contentHolder_txtTelPhone").value;
            if ((telPhone.length < 3 || telPhone.length > 20) && telPhone.length > 0) {
                alert("电话号码可以为空，或者填写长度在3-20个字符之间");
                return false;
            }

            var cellPhone = $("#ctl00_contentHolder_txtCellPhone").val();
            
            var cellPhoneReg = /^(0?(13|15|18|14|17)[0-9]{9})$/g;
            
            if (cellPhone != "" && !cellPhoneReg.test(cellPhone)) {
                alert("请填写正确的手机号码");
                return false;
            }
            if (telPhone == "" && cellPhone == "") {
                alert("手机号码和电话号码至少要填写一个");
                return false;
            }
            return true;
        }
    </script>
</asp:Content>


 