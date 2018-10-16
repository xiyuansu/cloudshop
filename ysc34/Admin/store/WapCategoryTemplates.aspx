<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="WapCategoryTemplates.aspx.cs" Inherits="Hidistro.UI.Web.Admin.store.WapCategoryTemplates" %>

<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a id="all" href="WapCategoryTemplates.aspx">分类模板选择</a></li>
            </ul>            
        </div>

    <%--数据列表区域--%>
    <div class="datalist clearfix">
        <style type="text/css">
            .catetem{clear:both;}
            .catetem .ctname {font-size:16px;padding:20px;}
            .cttext{clear:both;font-size:14px;}
            .cttext ul li{float:left;width:240px;padding:0px 10px;position:relative;}
            .cttext ul li .pic{clear:both;opacity:0.8;position:relative;}
            .cttext ul li:hover .pic {opacity:1;}
            .cttext ul li .pic2{clear:both;position:relative;}
            .cttext ul li .pic img,.cttext ul li .pic2 img{width:220px;height:391px;border:1px solid #ddd;}
            .cttext ul li span{font-weight:bold;text-align:center;display:block;padding:10px;}
            .cttext ul li p{text-align:center;padding:10px 0px;}
            .cttext ul li .bn {text-align:center;}
            .cttext ul li .btn-default {cursor:auto;}
            /*选中图片样式*/
            .okdiv {background-color: #000;opacity: 0.4;width: 220px;height: 391px;top: 0px;position: absolute;z-index: 1;display:none;}
            .okbtn {position: absolute;top: 180px;left: 70px;color: #898989;z-index: 2;background-color: #ffffff;border-radius: 3px;font-size: 14px;padding: 5px 20px;vertical-align: top;display:none;}
            .cttext ul li .pic2 .okdiv,.cttext ul li .pic2 .okbtn {display:block;}

            .mline {float: left;clear: both;border-bottom:1px solid #000;padding: 0px 10px; width: 100%;}
        </style>
        <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>--%>
        <div class="catetem" id="divpwap" runat="server">
            <div class="ctname">模版选择（微商城、触屏版、生活号）<%--<font>【您正在使用“<asp:Literal ID="ltDTWapName" runat="server" />”分类模板】</font>--%></div>
            <div class="cttext">
                <ul>
                    <li>
                        <div class="pic" id="divwaptem1" runat="server"><img src="/admin/images/uc1.png" border="0" />
                            <div class="okdiv"></div><div class="okbtn">已应用</div>
                        </div>
                        <span>模板一</span>
                        <p>（适合只有一级分类的商城）</p>
                        <div class="bn">
                            <asp:Button ID="btnwaptem1" runat="server" CssClass="btn btn-primary" Text="应用" title="应用当前模板" OnClick="btnwaptem1_Click" />
                        </div>
                    </li>
                    <li>
                        <div class="pic" id="divwaptem2" runat="server"><img src="/admin/images/uc3.png" border="0" />
                            <div class="okdiv"></div><div class="okbtn">已应用</div>
                        </div>
                        <span>模板二</span>
                        <p>（适合只有一级分类的商城）</p>
                        <div class="bn">
                            <asp:Button ID="btnwaptem2" runat="server" CssClass="btn btn-primary" Text="应用" title="应用当前模板" OnClick="btnwaptem2_Click" />
                        </div>
                    </li>
                    <li>
                        <div class="pic" id="divwaptem3" runat="server"><img src="/admin/images/uc2.png" border="0" />
                            <div class="okdiv"></div><div class="okbtn">已应用</div>
                        </div>
                        <span>模板三</span>
                        <p>（适合有两级分类的商城）</p>
                        <div class="bn">
                            <asp:Button ID="btnwaptem3" runat="server" CssClass="btn btn-primary" Text="应用" title="应用当前模板" OnClick="btnwaptem3_Click" />
                        </div>
                    </li>
                    <li>
                        <div class="pic" id="divwaptem0" runat="server"><img src="/admin/images/uc0.png" border="0" />
                            <div class="okdiv"></div><div class="okbtn">已应用</div>
                        </div>
                        <span>模板四</span>
                        <p>（适合一、二、三级分类的商城）</p>
                        <div class="bn">
                            <asp:Button ID="btnwaptem0" runat="server" CssClass="btn btn-primary" Text="应用" title="应用当前模板" OnClick="btnwaptem0_Click" />
                        </div>
                    </li>
                </ul>
            </div>
        </div>
        <hr class="mline" id="hrmline" runat="server" />
        <div class="catetem" id="divpapp" runat="server">
            <div class="ctname">模板选择（安卓APP，苹果APP）<%--<font>【您正在使用“<asp:Literal ID="ltDTAppName" runat="server" />”分类模板】</font>--%></div>
            <div class="cttext">
                <ul>
                    <li>
                        <div class="pic" id="divapptem1" runat="server"><img src="/admin/images/uc1.png" border="0" />
                            <div class="okdiv"></div><div class="okbtn">已应用</div>
                        </div>
                        <span>模板一</span>
                        <p>（适合只有一级分类的商城）</p>
                        <div class="bn">
                            <asp:Button ID="btnapptem1" runat="server" CssClass="btn btn-primary" Text="应用" title="应用当前模板" OnClick="btnapptem1_Click" />
                        </div>
                    </li>
                    <li>
                        <div class="pic" id="divapptem2" runat="server"><img src="/admin/images/uc3.png" border="0" />
                            <div class="okdiv"></div><div class="okbtn">已应用</div>
                        </div>
                        <span>模板二</span>
                        <p>（适合只有一级分类的商城）</p>
                        <div class="bn">
                            <asp:Button ID="btnapptem2" runat="server" CssClass="btn btn-primary" Text="应用" title="应用当前模板" OnClick="btnapptem2_Click" />
                        </div>
                    </li>
                    <li>
                        <div class="pic" id="divapptem3" runat="server"><img src="/admin/images/uc2.png" border="0" />
                            <div class="okdiv"></div><div class="okbtn">已应用</div>
                        </div>
                        <span>模板三</span>
                        <p>（适合有两级分类的商城）</p>
                        <div class="bn">
                            <asp:Button ID="btnapptem3" runat="server" CssClass="btn btn-primary" Text="应用" title="应用当前模板" OnClick="btnapptem3_Click" />
                        </div>
                    </li>
                    <li>
                        <div class="pic" id="divapptem0" runat="server"><img src="/admin/images/uc0.png" border="0" />
                            <div class="okdiv"></div><div class="okbtn">已应用</div>
                        </div>
                        <span>模板四</span>
                        <p>（适合一、二、三级分类的商城）</p>
                        <div class="bn">
                            <asp:Button ID="btnapptem0" runat="server" CssClass="btn btn-primary" Text="应用" title="应用当前模板" OnClick="btnapptem0_Click" />
                        </div>
                    </li>
                </ul>
            </div>
        </div>

            <%--</ContentTemplate>
        </asp:UpdatePanel>--%>

    </div>
    <%--数据列表底部功能区域--%>
        
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">    
</asp:Content>