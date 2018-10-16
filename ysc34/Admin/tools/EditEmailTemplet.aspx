<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.EditEmailTemplet" MasterPageFile="~/Admin/Admin.Master" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register src="../Ascx/ImageList.ascx" tagname="ImageList" tagprefix="uc1" %>


<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="sendmessagetemplets.aspx">消息提醒</a></li>
                <li class="hover"><a href="javascript:void">编辑</a></li>
            </ul>
        </div>
    </div>
    <div class="areacolumn clearfix">


        <div class="columnright">

            <div class="formitem">
                <ul>
                    <li><span class="formitemtitle">消息类型：</span>
                        <abbr class="formitemtitle">
                            <asp:Label ID="litEmailType" runat="server"></asp:Label>
                        </abbr>
                    </li>
                    <li class="clearfix"><span class="formitemtitle">标签说明：</span>
                        <span>
                            <asp:Literal ID="litEmailDescription" runat="server"></asp:Literal></span>
                    </li>
                    <li class="mb_0"><span class="formitemtitle">邮件主题：</span>
                        <asp:TextBox ID="txtEmailSubject" runat="server" Width="300px" CssClass="forminput form-control"></asp:TextBox>
                        <p id="txtEmailSubjectTip" runat="server"></p>
                    </li>
                    <li><span class="formitemtitle">邮件内容：</span>
                        <span>
                            <Hi:Ueditor ID="fcContent" runat="server" Width="660" />
                        </span>
                        <p>邮件內容不能为空，长度限不能超过4000个字符</p>
                    </li>
                    <li>
                        <span class="formitemtitle">
                            &nbsp;&nbsp;&nbsp;
                        </span>
                        <asp:Button ID="btnSaveEmailTemplet" runat="server" OnClientClick="return PageIsValid();" Text="保存" CssClass="btn btn-primary" />

                    </li>
                </ul>               
            </div>            
        </div>
    </div>
  
    <div class="databottom">
        <div class="databottom_bg"></div>
    </div>
    <div class="bottomarea testArea">
        <!--顶部logo区域-->
    </div>

    <uc1:ImageList ID="ImageList" runat="server" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
   
    <script type="text/javascript" language="javascript">
        function InitValidators() {

            initValid(new InputValidator('ctl00_contentHolder_txtEmailSubject', 1, 60, false, null, '邮件主题不能为空，长度限制在1-60个字符之间'))
        }
        $(document).ready(function () { InitValidators(); });
    </script>
</asp:Content>

