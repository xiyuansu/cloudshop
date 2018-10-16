<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="BatchEditMemberPoint.aspx.cs" Inherits="Hidistro.UI.Web.Admin.member.BatchEditMemberPoint" %>

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
            padding: 20px 20px 60px 20px;
            width: 100% !important;
        }
    </style>
    <div class="dataarea mainwidth databody">
        <div class="datalist clearfix">
            <asp:Repeater ID="grdSelectedUsers" runat="server">
                <HeaderTemplate>
                    <table cellspacing="0" border="0" id="ctl00_contentHolder_grdSelectedUsers" class="table table-striped">
                        <tbody>
                            <tr>
                                <th>会员名</th>
                                <th width="200">可用积分</th>
                                <th width="200">备注</th>
                            </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:Literal ID="lblUserName" runat="server" Text='<%# Eval("UserName") %>' />
                            <asp:HiddenField runat="server" ID="hidUserId" Value='<%# Eval("UserId") %>' />
                        </td>
                        <td>
                            <asp:Literal ID="Literal1" runat="server" Text='<%# Eval("Points") %>' />&nbsp;
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtListRemark" CssClass="form-control"></asp:TextBox>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate></tbody></table></FooterTemplate>
            </asp:Repeater>
        </div>
    </div>
    <div class="areacolumn clearfix">
        <div class="columnright">
            <div class="formitem">
                <ul>
                    <li class="mb_0"><span class="formitemtitle " style="width: 100px;"><em>*</em>批量操作：</span>
                        <div class="input-group">
                            <span class="input-group-addon">
                                <input type="radio" id="radAdd" name="EditPoints" checked="true" runat="server" class="icheck" onclick="onRadioClick(1)" /><label for="ctl00_contentHolder_radAdd">增加</label></span>
                            <asp:TextBox ID="txtAddPoints" CssClass="forminput form-control" runat="server" MaxLength="5" placeholder="请输入大于0的整数" />
                            <span class="input-group-addon">分</span>
                        </div>
                        <p id="ctl00_contentHolder_txtAddPointsTip" style="width: 300px;"></p>
                    </li>
                    <li class="mb_0">
                        <span class="formitemtitle" style="width: 100px;">&nbsp;</span>
                        <div class="input-group">
                            <span class="input-group-addon">
                                <input type="radio" id="RadMinus" name="EditPoints" runat="server" class="icheck"  onclick="onRadioClick(2)" /><label for="ctl00_contentHolder_RadMinus">减少</label></span>
                            <asp:TextBox ID="txtMinusPoints" CssClass="forminput form-control" runat="server" MaxLength="5" disabled="disabled" placeholder="请输入大于0的整数" />
                            <span class="input-group-addon">分</span>
                        </div>
                        <p id="ctl00_contentHolder_txtMinusPointsTip" style="width: 300px;"></p>
                    </li>
                    <li><span class="formitemtitle" style="width: 100px;">批量备注：</span>
                        <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" CssClass="forminput form-control" Width="320" Height="120" MaxLength="1000"></asp:TextBox>
                    </li>
                </ul>
                <div class="modal_iframe_footer">
                    <asp:Button ID="btnSubmitBatchPoint" OnClientClick="return validForm()" Text="确 定" CssClass="btn btn-primary" runat="server" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript">
        function validForm() {
            if ($("#ctl00_contentHolder_radAdd").is(":checked") == true || $("ctl00_contentHolder_radAdd").is(":checked") == "checked") {
                var points = $("#ctl00_contentHolder_txtAddPoints").val();
                if (points.length <= 0) {
                    alert("请输入要增加的积分");
                    return false;
                }
                if (!parseInt(points)) {
                    alert("请输入正确的积分数");
                    return false;
                }
                if (points <= 0) {
                    alert("请输入大于0的积分数");
                    return false;
                }
            }
            else if ($("#ctl00_contentHolder_RadMinus").is(":checked") == true || $("ctl00_contentHolder_RadMinus").is(":checked") == "checked") {
                var points = $("#ctl00_contentHolder_txtMinusPoints").val();
                if (points.length <= 0) {
                    alert("请输入要减少的积分");
                    return false;
                }
                if (!parseInt(points)) {
                    alert("请输入正确的积分数");
                    return false;
                }
                if (points <= 0) {
                    alert("请输入大于0的积分数");
                    return false;
                }
            }
        }
        function onRadioClick(val) {
            if (val == 1) {
                $("#ctl00_contentHolder_txtAddPoints").removeAttr("disabled");
                $("#ctl00_contentHolder_txtMinusPoints").val("");
                $("#ctl00_contentHolder_txtMinusPoints").attr("disabled", "disabled");
                $("#ctl00_contentHolder_txtAddPoints").focus();
            }
            else {
                $("#ctl00_contentHolder_txtAddPoints").val("");
                $("#ctl00_contentHolder_txtAddPoints").attr("disabled", "disabled");
                $("#ctl00_contentHolder_txtMinusPoints").removeAttr("disabled");
                $("#ctl00_contentHolder_txtMinusPoints").focus();
            }
        }
    </script>
</asp:Content>
