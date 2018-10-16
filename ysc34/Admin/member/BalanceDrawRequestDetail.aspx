<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="BalanceDrawRequestDetail.aspx.cs" Inherits="Hidistro.UI.Web.Admin.BalanceDrawRequestDetail" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>


<%@ Import Namespace="Hidistro.Core" %>
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
            padding: 20px 20px 90px 20px;
            width: 100% !important;
        }

        #btnCopy {
            float: right;
        }
    </style>
    <div class="areacolumn clearfix">
        <div class="columnright">
            <div class="formitem_con">
                <span class="formitemtitle" style="width: 80px;">用户名称：</span>
                <asp:Label runat="server" ID="lblUserName" Text=""></asp:Label><br />
                  <%if (isAlipay)
                      { %>
                <span class="formitemtitle" style="width: 80px;">支付宝姓名：</span>
                <asp:Label runat="server" ID="lblAlipayRealName" Text=""></asp:Label><br />
                <span class="formitemtitle" style="width: 80px;">支付宝账号：</span>
                <asp:Label runat="server" ID="lblAlipayCode" Text=""></asp:Label><br />

                <%}
                    else if (isWeixin)
                    { %> <%}
                   else
                   { %>

                <span class="formitemtitle" style="width: 80px;">开户银行：</span>
                <asp:Label runat="server" ID="lblBankName" Text=""></asp:Label><br />

                <span class="formitemtitle" style="width: 80px;">开户名称：</span>
                <asp:Label runat="server" ID="lblAccountName" Text=""></asp:Label><br />

                <span class="formitemtitle" style="width: 80px;">银行账号：</span>
                <asp:Label runat="server" ID="lblMerchantCode" Text=""></asp:Label><br />

                <%} %>
            </div>
            <div class="modal_iframe_footer">
                <input type="button" class="btn btn-primary" value="知道了" onclick="javascript: art.dialog.close();" />
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
