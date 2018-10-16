<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/Admin.Master" CodeBehind="ReplyProductConsultations.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ReplyProductConsultations" %>


<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="server">
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

        abbr {
            float: left;
            width: 400px;
            word-wrap: break-word;
        }
    </style>

    <div class="dataarea mainwidth databody">


        <div class="datafrom">
            <div class="formitem">
                <ul>
                    <li><span class="formitemtitle Pw_100" style="width:138px;">咨询用户：</span><asp:Literal ID="litUserName" runat="server"></asp:Literal></li>

                    <li><span class="formitemtitle Pw_100" style="width:138px;">咨询时间：</span><Hi:FormatedTimeLabel ID="lblTime" runat="server"></Hi:FormatedTimeLabel></li>
                    <li><span class="formitemtitle Pw_100" style="width:138px;">咨询内容：</span><abbr class="colorQ"><asp:Literal ID="litConsultationText" runat="server"></asp:Literal></abbr></li>
                    <li><span class="formitemtitle Pw_100" style="width:138px;">回复：</span>
                        <span style="float: left; position: relative; z-index:0;">
                            <asp:TextBox ID="txtReply" runat="server" TextMode="MultiLine" Height="100" Width="550" CssClass="forminput form-control"></asp:TextBox>
                        </span>
                    </li>
                </ul>
                <div class="modal_iframe_footer">
                    <asp:Button ID="btnReplyProductConsultation" runat="server" Text="回复" CssClass="btn btn-primary" />
                </div>
            </div>
        </div>
    </div>
    <div class="bottomarea testArea">
        <!--顶部logo区域-->
    </div>
</asp:Content>
