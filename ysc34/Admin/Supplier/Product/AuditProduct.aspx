<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AuditProduct.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Supplier.Product.AuditProduct" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
     <link rel="stylesheet" href="/admin/css/bootstrap.min.css" type="text/css" media="screen" />
    <link rel="stylesheet" href="/admin/css/css.css" type="text/css" media="screen" />
    <link rel="stylesheet" href="/admin/css/bootstrap-datetimepicker.css" type="text/css" media="screen" />
    <link rel="stylesheet" href="/admin/css/windows.css" type="text/css" media="screen" />
    <link rel="stylesheet" href="/admin/css/pagevalidator.css" type="text/css" media="screen" />
    <link rel="stylesheet" href="/utility/skins/blue.css?v=3.0" />
    <script src="../../js/jquery-1.8.3.min.js"></script>
    <script src="../../../Utility/jquery.artDialog.js"></script>
    <script src="../../../Utility/windows.js?v=3.0"></script>
     <script type="text/javascript">
        function checkReson() {
            if ($("#tbxReson").val() == "") {
                ShowMsg("拒绝时必须填写拒绝理由");
                return false;
            }
            return true;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
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
            padding: 0 20px 0px 20px;
            width: 100% !important;
        }

        .searcharea {
            padding: 20px 0 0 0;
        }

            .searcharea li span {
                float: left;
                margin: 0 5px;
            }

        h3{
            font-size: 16px;
            line-height: 40px;
            width:100%;
            color:red;
            text-align:center;
        }
     
    </style>
    <div class="areacolumn clearfix" style="padding:0 25px;">
        <h3>商品审核通过默认上架</h3>
        <ul>
            <li><span style="float:left">备注：</span>
                <asp:TextBox ID="tbxReson" runat="server" MaxLength="200" TextMode="MultiLine" CssClass="form_input_l form-control" Height="140px" Width="300" placeholder="拒绝时必须填写拒绝理由"></asp:TextBox></li>
        </ul>
        <br />
        <div style="text-align:center;">
        <asp:Button ID="btnPass" runat="server" Text="通过审核" CssClass="btn btn-primary" OnClick="btnPass_Click" />
        <asp:Button ID="btnFailed" runat="server" Text="拒绝" CssClass="btn submit_DAqueding ml_10" OnClick="btnFailed_Click" OnClientClick="return checkReson();" />
            </div>
            </div>
    </form>
</body>
</html>


