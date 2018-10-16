<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/Admin.Master" CodeBehind="ReplyReceivedMessages.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ReplyReceivedMessages" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>


<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="server">


    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="receivedmessages.aspx?MessageStatus=1" >收件箱</a></li>
                <li><a href="receivedmessages.aspx?MessageStatus=2">已回复</a></li>
                <li><a href="receivedmessages.aspx?MessageStatus=3">未回复</a></li>
                <li class="hover"><a href="javascript:void">消息</a></li>

            </ul>
        </div>

        <div class="areacolumn clearfix">

            <div class="columnright">


                <div class="formitem">
                    <ul>
                        <li>
                            <span class="formitemtitle">标题：</span>
                            <strong class="colorE"><asp:Literal ID="litTitle" runat="server"></asp:Literal></strong>
                        </li>
                        <li>
                            <span class="formitemtitle">内容：</span>
                            <textarea id="txtContent" runat="server" name="txtContent" class="forminput form-control" rows="10" cols="54" style="color: #A0A0A0; width: 400px; height: 160px; resize: none;" readonly="readonly" disabled="disabled"></textarea>
                        </li>
                        <li class="mb_0">
                            <span class="formitemtitle"><em>*</em>标题：</span>
                            <asp:TextBox ID="txtTitle" runat="server" CssClass="forminput form-control" Width="400" placeholder="标题长度限制在1-60个字符内" ></asp:TextBox>
                            <p id="txtTitleTip" class="msgError" runat="server"></p>
                        </li>
                        <li class="mb_0"><span class="formitemtitle"><em>*</em>回复：</span>
                            <textarea id="txtContes" runat="server" class="forminput form-control" name="txtContes" rows="10" cols="54" style="width: 400px; height: 160px;" placeholder="回复长度限制在1-300个字符内"></textarea>
                            <p class="msgError" id="txtContesTip" runat="server"></p>
                        </li>
                        <li>
                            <span class="formitemtitle">&nbsp;</span>
                            <asp:Button ID="btnReplyReplyReceivedMessages" OnClientClick="return PageIsValid();" runat="server" Text="回复" CssClass="btn btn-primary" />
                        </li>
                    </ul>
                </div>

            </div>
        </div>



    </div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="Server">
    <script type="text/javascript" language="javascript">
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtTitle', 1, 60, false, null, '必填 回复标题不能为空，长度限制在60个字符以内'));
            initValid(new InputValidator('ctl00_contentHolder_txtContes', 1, 300, false, null, '必填 回复不能为空，长度限制在300个字符以内'));
        }
        $(document).ready(function () { InitValidators(); });
    </script>
</asp:Content>
