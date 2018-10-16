<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="OpenIdServices.aspx.cs" Inherits="Hidistro.UI.Web.Admin.member.OpenIdServices" %>

<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <style>
        .loginTabao {
            border: 1px solid #dedfd7;
            background: #fbffda;
            padding: 10px 5px;
            color: #666;
            height: 180px;
        }

            .loginTabao .taobaoTitle {
                font-size: 14px;
                font-weight: bold;
                color: #306fa2;
                line-height: 24px;
                height: 24px;
            }

            .loginTabao ul li.li1 {
                height: 46px;
                line-height: 20px;
                linclear: both;
                overflow: visible;
            }

            .loginTabao ul li.li2 {
                line-height: 30px;
                height: 30px;
                overflow: visible;
            }

                .loginTabao ul li.li2 a img {
                    border: none;
                    margin-bottom: 3px;
                }

                .loginTabao ul li.li2 label, .loginTabao ul li.li2 a {
                    float: left;                    
                }

            .loginTabao ul.ul2 {
                clear: both;
            }

                .loginTabao ul.ul2 a {
                    padding-left: 3px;
                    color: #306fa2;
                }

                .loginTabao ul.ul2 li {
                    clear: both;
                    word-break: break-all;
                }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <h1 style="font-size:16px;line-height:32px;">
                已开启
            </h1>
        </div>
        <div class="datalist clearfix">
            <asp:Panel runat="server" ID="pnlConfigedList">
                <asp:Repeater ID="grdConfigedItemsNew" runat="server" OnItemCommand="grdConfigedItemsNew_ItemCommand">
                    <HeaderTemplate>
                        <table cellspacing="0" rules="all" border="1" id="ctl00_contentHolder_grdEmptyList" style="border-collapse: collapse;">
                            <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td style="width: 150px; text-align:center">
                                <asp:HiddenField ID="hfFullName" runat="server" Value='<%#Eval("FullName") %>' />
                                <img src='<%#Eval("Logo") %>' style="border-width: 0px;"></td>
                            <td style="width: 100px;"><%#Eval("DisplayName") %></td>
                            <td>
                                <%# Globals.HtmlDecode(Eval("ShortDescription").ToString())%>
                            </td>
                            <td align="center"  style="width:100px;">
                                <asp:HyperLink ID="HyperLink1" runat="server" Text="配置" NavigateUrl='<%# "OpenIdSettings.aspx?t="+Eval("FullName") %>'></asp:HyperLink>
                                |
                    <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="return confirm('确定要关闭此信任登录吗？');" CommandName="Delete" Text="关闭"></asp:LinkButton>
                                <%# GetHelpDoc(Eval("FullName")) %>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate></tbody></table></FooterTemplate>

                </asp:Repeater>

            </asp:Panel>
            <asp:Panel runat="server" ID="pnlConfigedNote">
                <span>还没有配置任何信任登录信息，请从未开启的列表中选择配置。</span>
            </asp:Panel>
        </div>
        <div class="title mt_20">
           <h1 style="font-size:16px;line-height:32px;">
                未开启
            </h1>
        </div>
        <div class="datalist">
            <asp:Panel runat="server" ID="pnlEmptyList" >
                <asp:Repeater ID="grdEmptyListNew" runat="server" OnItemCommand="grdEmptyListNew_ItemCommand">
                    <HeaderTemplate>
                        <table cellspacing="0" rules="all" border="1" id="ctl00_contentHolder_grdEmptyList" style="border-collapse: collapse;width:100%;">
                            <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td style="width: 150px;">
                                <asp:HiddenField ID="hfFullName" runat="server" Value='<%#Eval("FullName") %>' />
                                <img src='<%#Eval("Logo") %>' style="border-width: 0px;"></td>
                            <td style="width: 100px;"><%#Eval("DisplayName") %></td>
                            <td>
                                <%# Globals.HtmlDecode(Eval("ShortDescription").ToString())%>
                            </td>
                            <td align="center" style="width: 100px;">
                                <asp:HyperLink ID="HyperLink1" runat="server" Text="配置" NavigateUrl='<%# "OpenIdSettings.aspx?t="+Eval("FullName") %>'></asp:HyperLink>
                                <%# GetHelpDoc(Eval("FullName")) %>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate></tbody></table></FooterTemplate>
                </asp:Repeater>

            </asp:Panel>
            <asp:Panel runat="server" ID="pnlEmptyNote">
                <span>所有可使用的信任登录都已开启。</span>
            </asp:Panel>
        </div>
    </div>
    <script type="text/javascript">
        window.onload = function () {
            //已经开启
            $("#ctl00_contentHolder_pnlConfigedList tr,#ctl00_contentHolder_pnlEmptyList tr").each(function () {
                //支付宝快捷登录
                var hfFullName = $("input[name$=hfFullName]", this).val();
                $("a[title=在线申请]", this).text("在线申请").children().remove();
                switch (hfFullName) {
                    case "hishop.plugins.openid.alipay.alipayservice":                        
                        $("a[title=在线申请]", this).addClass("btn btn-info");
                        //支付宝
                        break;
                    case "hishop.plugins.openid.qq.qqservice":
                        //QQ
                        $("a[title=在线申请]", this).addClass("btn btn-danger");
                        break;
                    case "hishop.plugins.openid.taobao.taobaoservice":
                        //淘宝
                        $("a[title=在线申请]", this).addClass("btn btn-warning");
                        break;
                    case "hishop.plugins.openid.sina.sinaservice":
                        //新浪微博
                        $("a[title=在线申请]", this).addClass("btn btn-danger");
                        break;
                    case "hishop.plugins.openid.weixin.weixinservice":
                        //微信扫码
                        $("a[title=在线申请]", this).addClass("btn btn-success");
                        break;
                    default:
                        $("a[title=在线申请]", this).addClass("btn btn-primary");
                        break;
                }
            });
            //未开启
        }
    </script>
</asp:Content>
