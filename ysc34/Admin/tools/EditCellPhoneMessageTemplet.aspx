<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="EditCellPhoneMessageTemplet.aspx.cs" Inherits="Hidistro.UI.Web.Admin.EditCellPhoneMessageTemplet" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>


<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
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
          
            <div class="formitem">
                <ul>
                    <li><span class="formitemtitle ">消息类型：</span>
                        <abbr class="formselect">
                            <asp:Label ID="litEmailType" runat="server"></asp:Label>
                        </abbr>
                    </li>
                    <li class="clearfix"><span class="formitemtitle ">标签说明：</span>
                        <span><asp:HiddenField ID="hidTagDescription" runat="server" />
                            <asp:Literal ID="litTagDescription" runat="server"></asp:Literal></span>
                    </li>
                     <li>
                         <span class="formitemtitle ">模板CODE：</span>
                        <asp:TextBox  ID="txtSMSTemplateCode" runat="server" Width="550px"  CssClass="forminput form-control"></asp:TextBox>
                        <p id="P1" runat="server">与阿里云的模板CODE保持一致。</p>
                    </li>

                    <li><span class="formitemtitle ">内容模板参数：</span>
                        <asp:TextBox TextMode="MultiLine" ID="txtContent" runat="server" Width="550px" Height="200px"  Enabled="true" CssClass="forminput form-control"></asp:TextBox>
                        <p id="txtContentTip" runat="server">json内容不能为空，且长度限制在1-300个字符之间</p>
                    </li>
                     <li><span class="formitemtitle ">内容模板内容：</span>
                        <asp:TextBox TextMode="MultiLine" ID="txtSMSTemplateContent" runat="server" Width="550px" Height="200px"  Enabled="true" CssClass="forminput form-control"></asp:TextBox>
                        <p id="P2" runat="server">内容不能为空，直接拷贝到阿里云短信模板里</p>
                    </li>
                </ul>
            </div>
            <div class="ml_198">
                <asp:Button ID="btnSaveCellPhoneMessageTemplet" runat="server" OnClientClick="return PageIsValid();" Text="保存" CssClass="btn btn-primary inbnt" /></div>
        </div>
    </div>
 


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" language="javascript">
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtContent', 1, 300, false, null, '内容不能为空，且长度限制在1-300个字符之间'))

        }
        $(document).ready(function () { InitValidators(); });
    </script>
</asp:Content>
