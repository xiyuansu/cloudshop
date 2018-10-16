<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="WapThemeSettings.aspx.cs" Inherits="Hidistro.UI.Web.Admin.store.WapThemeSettings" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <style type="text/css">
        .catetem {
            clear: both;
        }

            .catetem .ctname {
                font-size: 16px;
                padding-bottom: 20px;
                padding-top: 20px;
            }

        .cttext {
            clear: both;
            font-size: 14px;
        }

            .cttext ul li {
                float: left;
                width: 240px;
                padding: 10px 10px;
                position: relative;
            }

                .cttext ul li .pic {
                    clear: both;
                    position: relative;
                }

                .cttext ul li:hover .pic {
                    opacity: 1;
                }

                .cttext ul li .pic2 {
                    clear: both;
                    position: relative;
                }

                    .cttext ul li .pic img, .cttext ul li .pic2 img {
                        width: 220px;
                        height: 350px;
                        border: 1px solid #ddd;
                    }

                .cttext ul li span {
                    font-weight: bold;
                    text-align: center;
                    display: block;
                    padding: 10px;
                }

                .cttext ul li p {
                    text-align: center;
                    padding: 10px 0px;
                }

                .cttext ul li .bn {
                    text-align: center;
                }

                .cttext ul li .btn-default {
                    cursor: auto;
                }

        .mline {
            float: left;
            clear: both;
            border-bottom: 1px solid #000;
            padding: 0px 10px;
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a id="all" href="WapThemeSettings">移动端模板设置</a></li>
            </ul>
        </div>
        <div class="datalist clearfix">
            <div class="catetem" id="divpwap">
                <div class="ctname"><span style="width:5px;background-color:#FF551F;">&nbsp;</span>&nbsp;当前使用的模板</div>
                <div class="cttext">
                    <ul>
                        <li>
                            <div class="pic" id="divwaptem1" runat="server">
                                <img id="imgCurrentImg" runat="server" />
                                <%-- admin/images/uc1.png--%>
                            </div>
                        </li>
                        <li style="width: 450px;">
                            <div class="pic" id="div1" runat="server">
                                <div>
                                    <img id="imgWapCode" style="width: 90px; height: 90px;" runat="server" />
                                    <%--/Storage/master/QRCode/3.0.ysctest.huz.cn_vshop.png--%>
                                    <div style="margin-top: 20px;">
                                        手机扫描此二维码，可直接在手机上访问移动端
                                        <a class="btn btn-default ml20" id="apreview" runat="server" target="_blank">预览
                                        </a>
                                    </div>
                                    <div style="margin-top: 20px;">
                                        移动端网址:<br />
                                        <asp:Literal ID="litWapUrl" runat="server"></asp:Literal>
                                    </div>
                                    <div style="margin-top: 20px;">
                                        模板名称:<asp:Literal ID="litThemeName" runat="server"></asp:Literal>
                                    </div>
                                    <div style="margin-top: 40px;">
                                        <a class="btn btn-primary" id="acurrentedit" runat="server" target="_blank">模板编辑
                                        </a>
                                    </div>
                                </div>
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
            <div class="catetem" id="divpwap2">
                <div class="ctname"><span style="width:5px;background-color:#FF551F;">&nbsp;</span>&nbsp;可选用的模板</div>
                <div class="cttext">
                    <ul>
                        <asp:Repeater ID="repThemes" runat="server" OnItemCommand="repThemes_ItemCommand">
                            <ItemTemplate>
                                <li>
                                    <div class="pic" id="div2" runat="server">
                                        <img src='<%#Eval("ThemeImgUrl") %>' />
                                        <asp:HiddenField ID="hidThemeImgUrl" runat="server" Value='<%#Eval("ThemeImgUrl") %>' />
                                    </div>
                                    <span><%#Eval("Name") %>
                                        <asp:HiddenField ID="hidThemeName" runat="server" Value='<%#Eval("Name") %>' />
                                        <asp:HiddenField ID="hidDirName" runat="server" Value='<%#Eval("ThemeName") %>' />
                                    </span>
                                    <div class="bn">
                                        <asp:Button ID="btnUse" runat="server" CssClass="btn btn-primary" CommandName="btnUse" Text="应用" title="应用当前模板" />
                                    </div>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>

