<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="EditSaleCounts.aspx.cs" Inherits="Hidistro.UI.Web.Admin.product.EditSaleCounts" Title="无标题页" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <style>
        html { background: #fff !important; }

        body { padding: 0; width: 100%; }

        #mainhtml { margin: 0; padding: 20px 20px 60px 20px; width: 100% !important; }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">

    <div class="dataarea mainwidth databody">

        <div class="searcharea a_none mb_0">
            <ul>
                <li><font>直接调整：前台显示销售数量&nbsp;=&nbsp;</font>
                    <input name="txtSaleCounts" type="text" maxlength="20" id="txtSaleCounts" class="forminput form-control" style="width: 60px;" />
                </li>
                <li>
                    <input type="button" name="btnAddOK" value="确定" id="btnAddOK" class="btn btn-info" /></li>
                <li style="margin: 0 0 0 10px;"><font>公式调整：前台显示销售数量&nbsp;=&nbsp;实际销售数量
                    <select name="ddlOperation" id="ddlOperation" class="select_s">
                        <option value="+">+</option>
                        <option value="*">*</option>
                    </select>
                </font>
                    <input name="txtOperationSaleCounts" type="text" maxlength="20" id="txtOperationSaleCounts" class="forminput form-control" style="width: 60px;" />
                </li>
                <li>&nbsp;&nbsp;
                    <input type="button" name="btnOperationOK" value="确定" id="btnOperationOK" class="btn btn-info" /></li>
            </ul>
        </div>

        <div class="datalist clearfix">
            <table class="table table-striped" cellspacing="0" border="0" id="ctl00_contentHolder_grdSelectedProducts" style="width: 100%; border-collapse: collapse;">
                <thead>
                    <tr>
                        <th scope="col" width="15%">商品图片</th>
                        <th scope="col" width="40%">商品名称</th>
                        <th scope="col" width="15%">商家编码</th>
                        <th scope="col">实际销售数量</th>
                        <th scope="col">前台显示销售数量</th>
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
                                <td><%#Eval("ProductName") %>                                </td>
                                <td><%#Eval("ProductCode") %></td>
                                <td class="hid_salecount"><%#Eval("SaleCounts")%></td>
                                <td>
                                    <asp:TextBox ID="hidProductId" runat="server" CssClass="hide hid_productid" Text='<%#Eval("ProductId") %>'></asp:TextBox>
                                    <asp:TextBox ID="txtShowSaleCounts" runat="server" CssClass="forminput form-control ipt_salecount" Width="80px" Text='<%#Eval("ShowSaleCounts") %>'></asp:TextBox>
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

    <script type="text/javascript">
        function CloseWindow() {
            var win = art.dialog.open.origin; //来源页面
            // 如果父页面重载或者关闭其子对话框全部会关闭
            win.location.reload();
        }

        //return false;

        $(function () {
            $("#btnAddOK").click(function () {
                var num = $("#txtSaleCounts").val();
                if (isNaN(num)) {
                    $("#txtSaleCounts").val("");
                    ShowMsg("销量填写只能为数字！", false);
                    return;
                }
                var salenum = parseInt(num);
                if (salenum >= 0) {
                    $(".ipt_salecount").val(salenum);
                }
            });

            $("#btnOperationOK").click(function () {
                var num = $("#txtOperationSaleCounts").val();
                var oper = $("#ddlOperation").val();
                if (isNaN(num)) {
                    $("#txtOperationSaleCounts").val("");
                    ShowMsg("销量操作数填写只能为数字！", false);
                    return;
                }
                salenum = parseInt(num);
                if (salenum <= 0) {
                    $("#txtOperationSaleCounts").val("");
                    ShowMsg("销量操作数只能为正整数！", false);
                    return;
                }
                $(".ipt_salecount").each(function () {
                    var _t = $(this);
                    var cursale = $(".hid_salecount", _t.parent().parent()).text();
                    var _cv = parseInt(cursale);
                    var _v = parseInt(_t.val()); 
                    switch(oper){
                        case "*":
                            _v = _cv * salenum;
                            break;
                        case "+":
                            _v = _cv + salenum;
                            break;
                    }
                    if (_v < 0) _v = 0;
                    _t.val(_v);
                });
            });
        });
    </script>
</asp:Content>
