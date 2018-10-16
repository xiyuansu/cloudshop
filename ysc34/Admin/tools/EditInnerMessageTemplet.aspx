<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="EditInnerMessageTemplet.aspx.cs" Inherits="Hidistro.UI.Web.Admin.EditInnerMessageTemplet" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="sendmessagetemplets.aspx" ">消息提醒</a></li>
                <li  class="hover"><a href="javascript:void">编辑</a></li>
                
            </ul>
           
        </div>
    </div>
    <div class="areacolumn clearfix">

        <div class="columnright">
            
            <div class="formitem ">
                <ul>
                    <li><span class="formitemtitle">消息类型：</span>
                        <abbr class="formselect">
                            <asp:Label ID="litEmailType" runat="server"></asp:Label>
                        </abbr>
                    </li>
                    <li class="clearfix"><span class="formitemtitle">标签说明：</span>
                        <span>
                            <asp:Literal ID="litTagDescription" runat="server"></asp:Literal></span>
                    </li>
                    <li class="mb_0"><span class="formitemtitle">站内信标题：</span><asp:TextBox ID="txtMessageSubject" Width="300px" runat="server" CssClass="form_input_l form-control" placeholder="长度限制在1-60个字符之间"></asp:TextBox>
                        <p id="txtMessageSubjectTip" runat="server"></p>
                    </li>
                    <li><span class="formitemtitle">内容模板：</span>
                        <asp:TextBox CssClass="forminput form-control" TextMode="MultiLine" ID="txtContent" runat="server" Width="500px" Height="200px"></asp:TextBox><br />
                        <p id="txtContentTip" runat="server">内容不能为空，长度限制在1-300个字符之间</p>
                    </li>
                </ul>
            </div>
            <div class="ml_198">
                <asp:Button ID="btnSaveMessageTemplet" runat="server" OnClientClick="return PageIsValid();" Text="保存" CssClass="btn btn-primary" /></div>
        </div>
    </div>
    <div class="databottom">
        <div class="databottom_bg"></div>
    </div>
    <div class="bottomarea testArea">
        <!--顶部logo区域-->
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" language="javascript">
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtMessageSubject', 1, 60, false, null, '站内信标题不能为空，长度限制在1-60个字符之间'))
            initValid(new InputValidator('ctl00_contentHolder_txtContent', 1, 300, false, null, '内容不能为空，长度限制在1-300个字符之间'))

        }
        $(document).ready(function () { InitValidators(); });
    </script>
</asp:Content>
