<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="CheckVerification.aspx.cs" Inherits="Hidistro.UI.Web.Depot.home.CheckVerification" Title="验证核销" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="cc1" TagName="Order_ItemsList" Src="~/Admin/Ascx/Order_ItemsList.ascx" %>


<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="Server">
    
    <input type="hidden" id="hidRemarkImage" value="" runat="server" clientidmode="Static" />
    <div>
        <div><span class="frame-span">门店名称：</span><asp:Literal runat="server" ID="litStoreName" /></div>
        <div class=" clear"><span class="frame-span">订单编号：</span><asp:Literal runat="server" ID="litOrderId" /></div>

        <div class=" clear"><span class="frame-span">核销码：</span>
             <div>
             <asp:CheckBoxList ID="ckbList" runat="server" ClientIDMode="Static" CssClass="icheck"></asp:CheckBoxList>
             </div>
        </div>
        <div class=" clear"></div>
        <h3>基本信息：</h3>
        <asp:Repeater ID="orderInputItem" OnItemDataBound="Repeater1_ItemDataBound" runat="server">
                    <HeaderTemplate>
                        <table class=" table_one" style=" float:left; width:100%;font-size:14px;color: #666;">
                            <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr id="trGroup" runat="server">
                            <td width="80" align="right" style=" white-space:nowrap;">
                                <%# Eval("InputFieldTitle") %>：
                            </td>
                            <td>
                                <%# GetInputField(Eval("InputFieldType"),Eval("InputFieldValue")) %>
                                <input type="hidden" id="hidInputFieldGroup" runat="server" value='<%# Eval("InputFieldGroup") %>' />
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </tbody>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
        <cc1:Order_ItemsList runat="server" ID="itemsList" ShowAllItem="true" />
    </div>
     <div class="modal_iframe_footer" style="padding: 15px 0 10px 0;">
            <asp:Button ID="btnVerification" runat="server" Text="确认核销" CssClass="btn btn-primary" OnClick="btnVerification_Click" />
        </div>
    <input type="hidden"  id="hidIsVerification" runat="server" clientidmode="static" value="" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="Server">
     <style>
        html {
            background: #fff !important;
        }

        body {
            padding: 0;
            width: 100%;
            line-height:30px;
        }

        #mainhtml {
            margin: 0;
            padding: 20px 20px 90px 20px;
            width: 100% !important;
        }
        h3{
            font-size: 16px;
            color: #333;
            line-height: 30px;
            border-bottom:1px solid #ccc;
        }
      .frame-span{
          font-size:14px;color: #666; width:100px; text-align:right; float:left;
      }
      .table_one tbody tr td {
    line-height: 30px;
}
    </style>
    <script language="javascript" type="text/javascript">
        $(document).ready(function (e) {
            window.setTimeout(dialogClose, 3000);
        });
        function dialogClose() {
            if ($("#hidIsVerification").val() == "true") {
                art.dialog.close();
            }
        }
    </script>
</asp:Content>
