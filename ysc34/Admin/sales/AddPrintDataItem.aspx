<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddPrintDataItem.aspx.cs" Inherits="Hidistro.UI.Web.Admin.sales.AddPrintDataItem" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
      <style>
        html {
            background: #fff !important;
        }

        body {
            padding: 0;
            width: 600px !important;
        }

        #mainhtml {
            margin: 0;
            padding: 20px 20px 60px 20px;
            width: 100% !important;
        }

      
    </style>
    <script type="text/javascript" language="javascript">

        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtName', 1, null, false, null, '名称不能为空'))
            initValid(new InputValidator('ctl00_contentHolder_txtContent', 0, null, false, null, '内容不能为空'))
        }
        $(document).ready(function () {
            InitValidators();
        })

     
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="areacolumn clearfix">

        <div class="columnright">
            <div class="formitem validator3">
                <ul style="width:100%;">
                    <li class="mb_0"><span class="formitemtitle" style="width:100px;"><em>*</em>名称：</span>
                        <asp:TextBox ID="txtName" runat="server" placeholder="名称不能为空" CssClass="forminput form-control"></asp:TextBox>
                        <p id="ctl00_contentHolder_txtName" style="width:300px;">&nbsp;</p>
                    </li>
                    <li class="mb_0"><span class="formitemtitle" style="width:100px;"><em>*</em>内容：</span>
                        <asp:TextBox ID="txtContent" TextMode="MultiLine" placeholder="内容不能为空" Height="100"  runat="server" Width="390" CssClass="forminput form-control"></asp:TextBox>
                        <p id="ctl00_contentHolder_txtContent" style="width:300px;">&nbsp;</p>
                    </li>
                </ul>
                <div class="modal_iframe_footer">
                    <asp:Button ID="btnSave" runat="server" OnClientClick="return PageIsValid();" Text="保 存" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                </div>
            </div>

        </div>
    </div>
</asp:Content>
