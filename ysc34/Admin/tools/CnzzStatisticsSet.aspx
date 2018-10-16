<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="CnzzStatisticsSet.aspx.cs" Inherits="Hidistro.UI.Web.Admin.CnzzStatisticsSet" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="Hidistro.Context" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <link href="../css/bootstrap-switch.css" rel="stylesheet" />
    <script type="text/javascript" src="../js/bootstrap-switch.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {

            $('input[name="ctl00$contentHolder$ctl00"]').on('switchChange.bootstrapSwitch', function (event, state) {

                $.ajax({
                    url: "/Handler/RegionHandler.ashx",
                    type: 'post', dataType: 'json', timeout: 10000,
                    data: { action: "openCnzz", state: state },
                    success: function (resultData) {
                        if (resultData.Status == "0") {
                            alert('关闭成功！');
                        }
                        else if (resultData.Status == "1") {
                            alert('开启成功！');
                        }
                        else {
                            alert('操作失败');
                        }
                    }
                });
            });
        })
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

  <div class="dataarea mainwidth databody"> 

        <div class="datafrom">
            <div class="formitem clearfix validator3">
                <ul runat="server" id="div_pan1">
                    <li>
                        <h2 class="colorE">创建账号</h2>
                    </li>
                    <li>
                        <span class="formitemtitle">创建账号：</span>
                        <asp:LinkButton ID="hlinkCreate" CssClass="btn btn-primary" Text="创建统计账号" runat="server"></asp:LinkButton>
                    </li>
                    <li>
                        <span class="formitemtitle">帮助：</span>
                        <span>如果您是第一次创建统计账号，请点击创建统计账号，这样统计功能即可开始使用。
                        </span>
                    </li>
                </ul>
                <ul runat="server" id="div_pan2">
                    <li>
                        <h2 class="colorE">开启或关闭统计</h2>
                    </li>

                    <li>
                        <span class="formitemtitle">开启/关闭：</span>
                        <Hi:OnOff runat="server" ID="ooOpenCnzz"></Hi:OnOff>
                    </li>
                    <li>
                        <span class="formitemtitle">帮助：</span>
                        <span>您的帐号已经创建，可以通过点击上方的开关进行开启和关闭，如<br />
                            果开启动就会在前台进行统计，可以通过查看统计的链接查看统计结果。
                        </span>
                    </li>
                </ul>

         </div>
     </div>
</div>

  <div class="bottomarea testArea">
    <!--顶部logo区域-->
  </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
