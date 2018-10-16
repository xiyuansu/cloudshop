<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="EditBaseInfo.aspx.cs" Inherits="Hidistro.UI.Web.Admin.product.EditBaseInfo" Title="无标题页" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <style>
        html { background: #fff !important; }

        body { padding: 0; width: 100%; }

        #mainhtml { margin: 0; padding: 0 20px 60px 20px; width: 100% !important; }

        .searcharea { padding: 20px 0 0 0; }

            .searcharea li span { float: left; margin: 0 5px; }
    </style>
    <div class="dataarea mainwidth databody">
        <div class="searcharea ">
            <ul>
                <li>
                    <span>商品名称： 增加前缀</span>
                    <asp:TextBox ID="txtPrefix" runat="server" Width="80px" MaxLength="20" CssClass=" forminput form-control" />
                    <span>增加后缀</span>
                    <asp:TextBox ID="txtSuffix" runat="server" Width="80px" MaxLength="20" CssClass=" forminput form-control" /></li>
                <li>
                    <asp:Button ID="btnAddOK" runat="server" Text="确定" CssClass="btn btn-info" /></li>
                <li style="margin-left: 10px;"><span>查找字符串</span>
                    <asp:TextBox ID="txtOleWord" runat="server" Width="80px" CssClass=" forminput form-control" />
                    <span>替换成</span>
                    <asp:TextBox ID="txtNewWord" runat="server" Width="80px" CssClass=" forminput form-control" /></li>
                <li>
                    <asp:Button ID="btnReplaceOK" runat="server" Text="确定" CssClass="btn btn-info" /></li>
            </ul>
        </div>

        <div class="datalist clearfix">
            <table class="table table-striped" cellspacing="0" border="0" style="width: 100%; border-collapse: collapse;">
                <thead>
                    <tr>
                        <th scope="col" width="12%">商品图片</th>
                        <th scope="col">商品名称</th>
                        <th scope="col" width="10%">排序</th>
                        <th scope="col" width="12%">商家编码</th>
                        <th scope="col" style="width: 12%;">市场价</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rptSelectedProducts" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <a href='<%#"../../ProductDetails.aspx?productId="+Eval("ProductId")%>' target="_blank">
                                        <Hi:ListImage ID="ListImage2" runat="server" DataField="ThumbnailUrl40" />
                                    </a>
                                </td>
                                <td>
                                    <asp:TextBox ID="hidProductId" runat="server" CssClass="hide ipt_skuid" Text='<%#Eval("ProductId") %>'>
                                    </asp:TextBox><asp:TextBox ID="txtProductName" runat="server" CssClass=" forminput form-control" Width="280px" Text='<%#Eval("ProductName") %>'></asp:TextBox>
                                </td>
                                 <td>
                                    <asp:TextBox ID="txtDisplaySequence" runat="server" CssClass=" forminput form-control" Width="80px" Text='<%#Eval("DisplaySequence") %>'></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtProductCode" runat="server" CssClass=" forminput form-control" Width="80px" Text='<%#Eval("ProductCode") %>'></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtMarketPrice" runat="server" CssClass=" forminput form-control" Width="80px" Text='<%#Eval("MarketPrice", "{0:F2}") %>'></asp:TextBox>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>

                </tbody>
            </table>
        </div>

        <div class="modal_iframe_footer">
            <asp:Button ID="btnSaveInfo" runat="server" OnClientClick="return PageIsValid();" Text="保存" CssClass="btn btn-primary" />
        </div>
    </div>

    <script>
        function CloseWindow() {
            var win = art.dialog.open.origin; //来源页面
            // 如果父页面重载或者关闭其子对话框全部会关闭
            win.location.reload();
        };

        //return false;  
    </script>

</asp:Content>
