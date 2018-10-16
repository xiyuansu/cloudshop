<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="BatchEditCommissionRate.aspx.cs" Inherits="Hidistro.UI.Web.Admin.depot.BatchEditCommissionRate" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content2" ContentPlaceHolderID="headHolder" runat="server">
    <meta name="Generator" content="EditPlus">  
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="Server">
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
            width: 100% !important;
        }
    </style>
    <div class="areacolumn clearfix">
        <div class="columnright">
            <div class="formitem validator2">
                <ul>
                   <li><span class="formitemtitle " style="width: 120px;">批量设置平台佣金：</span>
                        <div class="input-group">
                            <asp:TextBox ID="txtCommissionRate" CssClass="forminput form-control" runat="server" MaxLength="5" />
                            <span class="input-group-addon">%</span>
                        </div>
                        <p id="ctl00_contentHolder_txtCommissionRateTip"  style="width:224px; margin-left:120px;"></p>
                    </li>
                    <li>
                        <span class="formitemtitle " style="width: 120px;">&nbsp;</span>
                        <div class="input-group">
                        <asp:Button ID="btnSubmitBatch" OnClientClick="return doSubmit()" Text="确 定" Width="240px" CssClass="btn btn-primary" runat="server" />
                    </div>
                   </li>
                </ul>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function (e) {
            InitValidators();
        });

        function InitValidators() {

            initValid(new InputValidator('ctl00_contentHolder_txtCommissionRate', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '数据类型错误，最多只能输入两位小数'))
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtCommissionRate', 0, 100, '输入的数值超出了系统表示范围'));

        }
        function doSubmit() {
            // 1.先执行jquery客户端验证检查其他表单项
            if (!PageIsValid())
                return false;
            return true;
        }

    </script>
</asp:Content>