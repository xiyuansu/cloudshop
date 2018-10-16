<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="XCXShopTempEdit.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Applet.XCXShopTempEdit" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<%@ Register Src="../Ascx/ImageListView.ascx" TagName="ImageListView" TagPrefix="uc1" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>编辑店铺主页 - 小程序商城</title>
    <link rel="stylesheet" href="/Admin/shop/Public/css/dist/component-min.css" />
    <link rel="stylesheet" href="/Admin/shop/Public/plugins/jbox/jbox-min.css" />
    <!-- diy css start-->
    <link rel="stylesheet" href="/Admin/shop/PublicMob/css/style.css" />
    <link rel="stylesheet" href="/Admin/shop/Public/plugins/diy/diy-min.css" />
    <link rel="stylesheet" href="/Utility/Ueditor/plugins/uploadify/uploadify-min.css" />
    <link rel="stylesheet" href="/Admin/shop/Public/plugins/colorpicker/colorpicker.css" />
    <!-- diy css end-->
    <link rel="stylesheet" href="/Admin/shop/Public/plugins/colorpicker/colorpicker.css" />
    <link rel="stylesheet" href="/Templates/common/style/head.css" />
    <link rel="stylesheet" href="/Admin/shop/Public/css/dist/home/Shop/edit_homepage.css" />
    <link href="/Utility/iconfont/iconfont.css" rel="Stylesheet" />
    <script type="text/javascript" src="../../Utility/jquery-1.8.3.min.js"></script>
    <style>
        .app_zt .tab {
            margin-left: 15px;
        }

            .app_zt .tab a {
                width: 110px;
                height: 74px;
            }

        .diy-actions {
           overflow-y:auto;
           overflow-x:hidden;
        }

        .app_1 {
            background: url(../images/app_1.jpg);
        }

        .app_2 {
            background: url(../images/app_2.jpg);
        }

        .app_3 {
            background: url(../images/app_3.jpg);
        }

        .app_4 {
            background: url(../images/app_4.jpg);
        }

        .app_5 {
            background: url(../images/app_5.jpg);
        }

        .app_8 {
            background: url(../images/app_8.png);
        }

        .app_9 {
            background: url(../images/app_9.png);
        }

        .app_1:hover {
            background: url(../images/app_1_1.jpg) !important;
        }

        .app_2:hover {
            background: url(../images/app_2_1.jpg) !important;
        }

        .app_3:hover {
            background: url(../images/app_3_1.jpg) !important;
        }

        .app_4:hover {
            background: url(../images/app_4_1.jpg) !important;
        }

        .app_5:hover {
            background: url(../images/app_5_1.jpg) !important;
        }

        .app_8:hover {
            background: url(../images/app_8_1.png) !important;
        }

        .app_9:hover {
            background: url(../images/app_9_1.png) !important;
        }

        .diy .members_con {
            margin: 0 !important;
        }

        .members_nav1 {
            margin: 0 !important;
        }

            .members_nav1 ul li span {
                width: 100% !important;
            }

                .members_nav1 ul li span a img {
                    width: 100% !important;
                    height: 100% !important;
                }

        .title_img {
            width: 320px !important;
            height: 36px;
            border-top: 1px solid #e7e5ea;
        }

        .big_img {
            width: 128px !important;
            height: 153px;
            border-right: 1px solid #e7e5ea;
            border-top: 1px solid #e7e5ea;
        }

        .big1_img {
            width: 159px !important;
            height: 75px;
            border-right: 1px solid #e7e5ea;
            border-top: 1px solid #e7e5ea;
        }

        .mid_img {
            width: 190px !important;
            height: 76px;
            border-top: 1px solid #e7e5ea;
        }

        .mid1_img {
            width: 125px !important;
            height: 75px;
            border-top: 1px solid #e7e5ea;
            border-right: 1px solid #e7e5ea;
        }

        .small_img {
            width: 95px !important;
            height: 76px;
            border-right: 1px solid #e7e5ea;
            border-top: 1px solid #e7e5ea;
        }

        .small1_img {
            width: 96px !important;
            height: 75px;
            border-right: 1px solid #e7e5ea;
            border-top: 1px solid #e7e5ea;
        }

        .small2_img {
            width: 79px !important;
            height: 75px;
            border-right: 1px solid #e7e5ea;
            border-top: 1px solid #e7e5ea;
        }

        .small_img:last-child {
            border-right: 0;
        }

        .ad_img {
            width: 320px !important;
            height: 52px !important;
        }
    </style>

</head>
<body>
    <form id="thisForm" runat="server">
        <script>
            $(function () {
                var a = $(window).height();
                $('body').css("height", a);
            })
        </script>
           <Hi:ProductCategoriesDropDownList ID="dropCategories" runat="server" NullToDisplay="请选择商品分类"
                                CssClass="iselect" ClientIDMode="Static" style="display:none;" />
        <input type="hidden" id="pageclient" value="xcxshop" />
        <div class="container" style="margin: 0px 0 0 0px; padding: 0px 230px 0 0; width: 100%;">
            <div class="inner clearfix" style="margin: 0px; border: none;">
                <!-- end content-left -->
                <div class="content-right fl" style="border: none; width: 100%; height: 100%;">
                    <div class="page-top">
                        <div class="page-header">
                            <h2>编辑店铺主页</h2>
                            <span style="float: right; color: #aaa; line-height: 26px;">(请使用IE10以上版本或者google浏览器进行模板编辑)</span>
                        </div>
                    </div>
                    <input type="hidden" name="content" value="{&quot;page&quot;:{&quot;title&quot;:&quot;店铺首页&quot;},&quot;PModules&quot;:[{&quot;id&quot;:1,&quot;type&quot;:&quot;Header_style1&quot;,&quot;draggable&quot;:false,&quot;sort&quot;:0,&quot;content&quot;:{&quot;bg&quot;:&quot;/PublicMob/images/indexbg/01.jpg&quot;,&quot;photo&quot;:&quot;/PublicMob/images/header2.jpg&quot;},&quot;dom_conitem&quot;:null,&quot;dom_ctrl&quot;:null,&quot;ue&quot;:null}],&quot;LModules&quot;:[{&quot;id&quot;:9,&quot;type&quot;:6,&quot;draggable&quot;:true,&quot;sort&quot;:0,&quot;content&quot;:{&quot;layout&quot;:1,&quot;showPrice&quot;:true,&quot;showIco&quot;:true,&quot;showName&quot;:true,&quot;goodslist&quot;:[]},&quot;dom_conitem&quot;:null,&quot;dom_ctrl&quot;:null,&quot;ue&quot;:null},{&quot;id&quot;:10,&quot;type&quot;:4,&quot;draggable&quot;:true,&quot;sort&quot;:1,&quot;content&quot;:{&quot;layout&quot;:&quot;1&quot;,&quot;showPrice&quot;:true,&quot;showIco&quot;:true,&quot;showName&quot;:true,&quot;goodslist&quot;:[]},&quot;dom_conitem&quot;:null,&quot;dom_ctrl&quot;:null,&quot;ue&quot;:null},{&quot;id&quot;:11,&quot;type&quot;:4,&quot;draggable&quot;:true,&quot;sort&quot;:2,&quot;content&quot;:{&quot;layout&quot;:1,&quot;showPrice&quot;:true,&quot;showIco&quot;:true,&quot;showName&quot;:true,&quot;goodslist&quot;:[]},&quot;dom_conitem&quot;:null,&quot;dom_ctrl&quot;:null,&quot;ue&quot;:null},{&quot;id&quot;:&quot;201581010122837&quot;,&quot;type&quot;:2,&quot;draggable&quot;:true,&quot;sort&quot;:3,&quot;content&quot;:{&quot;title&quot;:&quot;标题名称&quot;,&quot;subtitle&quot;:&quot;『副标题』&quot;,&quot;direction&quot;:&quot;left&quot;,&quot;space&quot;:0},&quot;dom_conitem&quot;:null,&quot;dom_ctrl&quot;:null,&quot;ue&quot;:null},{&quot;id&quot;:&quot;201581010129469&quot;,&quot;type&quot;:2,&quot;draggable&quot;:true,&quot;sort&quot;:4,&quot;content&quot;:{&quot;title&quot;:&quot;标题名称&quot;,&quot;subtitle&quot;:&quot;『副标题』&quot;,&quot;direction&quot;:&quot;left&quot;,&quot;space&quot;:0},&quot;dom_conitem&quot;:null,&quot;dom_ctrl&quot;:null,&quot;ue&quot;:null},{&quot;id&quot;:&quot;201581010130861&quot;,&quot;type&quot;:3,&quot;draggable&quot;:true,&quot;sort&quot;:5,&quot;content&quot;:null,&quot;dom_conitem&quot;:null,&quot;dom_ctrl&quot;:null,&quot;ue&quot;:null},{&quot;id&quot;:&quot;201581010131229&quot;,&quot;type&quot;:4,&quot;draggable&quot;:true,&quot;sort&quot;:6,&quot;content&quot;:{&quot;layout&quot;:1,&quot;showPrice&quot;:true,&quot;showIco&quot;:true,&quot;showName&quot;:1,&quot;goodslist&quot;:[]},&quot;dom_conitem&quot;:null,&quot;dom_ctrl&quot;:null,&quot;ue&quot;:null},{&quot;id&quot;:&quot;201581010131701&quot;,&quot;type&quot;:5,&quot;draggable&quot;:true,&quot;sort&quot;:7,&quot;content&quot;:{&quot;layout&quot;:1,&quot;showPrice&quot;:true,&quot;showIco&quot;:true,&quot;showName&quot;:true,&quot;group&quot;:null,&quot;goodsize&quot;:6,&quot;goodslist&quot;:[{&quot;item_id&quot;:&quot;1&quot;,&quot;link&quot;:&quot;#&quot;,&quot;pic&quot;:&quot;/Public/images/diy/goodsView1.jpg&quot;,&quot;price&quot;:&quot;100.00&quot;,&quot;title&quot;:&quot;第一个商品&quot;},{&quot;item_id&quot;:&quot;2&quot;,&quot;link&quot;:&quot;#&quot;,&quot;pic&quot;:&quot;/Public/images/diy/goodsView2.jpg&quot;,&quot;price&quot;:&quot;200.00&quot;,&quot;title&quot;:&quot;第二个商品&quot;},{&quot;item_id&quot;:&quot;3&quot;,&quot;link&quot;:&quot;#&quot;,&quot;pic&quot;:&quot;/Public/images/diy/goodsView3.jpg&quot;,&quot;price&quot;:&quot;300.00&quot;,&quot;title&quot;:&quot;第三个商品&quot;},{&quot;item_id&quot;:&quot;4&quot;,&quot;link&quot;:&quot;#&quot;,&quot;pic&quot;:&quot;/Public/images/diy/goodsView4.jpg&quot;,&quot;price&quot;:&quot;400.00&quot;,&quot;title&quot;:&quot;第四个商品&quot;}]},&quot;dom_conitem&quot;:null,&quot;dom_ctrl&quot;:null,&quot;ue&quot;:null},{&quot;id&quot;:&quot;201581010132197&quot;,&quot;type&quot;:6,&quot;draggable&quot;:true,&quot;sort&quot;:8,&quot;content&quot;:null,&quot;dom_conitem&quot;:null,&quot;dom_ctrl&quot;:null,&quot;ue&quot;:null},{&quot;id&quot;:&quot;201581010132518&quot;,&quot;type&quot;:7,&quot;draggable&quot;:true,&quot;sort&quot;:9,&quot;content&quot;:{&quot;dataset&quot;:[{&quot;linkType&quot;:0,&quot;link&quot;:&quot;&quot;,&quot;title&quot;:&quot;&quot;,&quot;showtitle&quot;:&quot;&quot;}]},&quot;dom_conitem&quot;:null,&quot;dom_ctrl&quot;:null,&quot;ue&quot;:null},{&quot;id&quot;:&quot;201581010132816&quot;,&quot;type&quot;:14,&quot;draggable&quot;:true,&quot;sort&quot;:10,&quot;content&quot;:{&quot;website&quot;:&quot;&quot;},&quot;dom_conitem&quot;:null,&quot;dom_ctrl&quot;:null,&quot;ue&quot;:null},{&quot;id&quot;:&quot;201581010133150&quot;,&quot;type&quot;:13,&quot;draggable&quot;:true,&quot;sort&quot;:11,&quot;content&quot;:{&quot;layout&quot;:&quot;1&quot;,&quot;dataset&quot;:[{&quot;linkType&quot;:0,&quot;link&quot;:&quot;#&quot;,&quot;title&quot;:&quot;导航名称&quot;,&quot;pic&quot;:&quot;/Public/images/diy/waitupload.png&quot;},{&quot;linkType&quot;:0,&quot;link&quot;:&quot;#&quot;,&quot;title&quot;:&quot;导航名称&quot;,&quot;pic&quot;:&quot;/Public/images/diy/waitupload.png&quot;},{&quot;linkType&quot;:0,&quot;link&quot;:&quot;#&quot;,&quot;title&quot;:&quot;导航名称&quot;,&quot;pic&quot;:&quot;/Public/images/diy/waitupload.png&quot;}]},&quot;dom_conitem&quot;:null,&quot;dom_ctrl&quot;:null,&quot;ue&quot;:null},{&quot;id&quot;:&quot;201581010133669&quot;,&quot;type&quot;:12,&quot;draggable&quot;:true,&quot;sort&quot;:12,&quot;content&quot;:{&quot;style&quot;:&quot;0&quot;,&quot;marginstyle&quot;:&quot;0&quot;,&quot;dataset&quot;:[{&quot;link&quot;:&quot;/Shop/index&quot;,&quot;linkType&quot;:6,&quot;showtitle&quot;:&quot;首页&quot;,&quot;title&quot;:&quot;店铺主页&quot;,&quot;pic&quot;:&quot;/PublicMob/images/ind3_1.png&quot;,&quot;bgColor&quot;:&quot;#07a0e7&quot;,&quot;cloPicker&quot;:&quot;0&quot;,&quot;fotColor&quot;:&quot;#fff&quot;},{&quot;link&quot;:&quot;/Shop/index&quot;,&quot;linkType&quot;:6,&quot;showtitle&quot;:&quot;新品&quot;,&quot;title&quot;:&quot;&quot;,&quot;pic&quot;:&quot;/PublicMob/images/ind3_2.png&quot;,&quot;bgColor&quot;:&quot;#72c201&quot;,&quot;cloPicker&quot;:&quot;1&quot;,&quot;fotColor&quot;:&quot;#fff&quot;},{&quot;link&quot;:&quot;/Shop/index&quot;,&quot;linkType&quot;:6,&quot;showtitle&quot;:&quot;热卖&quot;,&quot;title&quot;:&quot;店铺主页&quot;,&quot;pic&quot;:&quot;/PublicMob/images/ind3_3.png&quot;,&quot;bgColor&quot;:&quot;#ffa800&quot;,&quot;cloPicker&quot;:&quot;2&quot;,&quot;fotColor&quot;:&quot;#fff&quot;},{&quot;link&quot;:&quot;/Shop/index&quot;,&quot;linkType&quot;:6,&quot;showtitle&quot;:&quot;推荐&quot;,&quot;title&quot;:&quot;&quot;,&quot;pic&quot;:&quot;/PublicMob/images/ind3_4.png&quot;,&quot;bgColor&quot;:&quot;#d50303&quot;,&quot;cloPicker&quot;:&quot;3&quot;,&quot;fotColor&quot;:&quot;#fff&quot;}]},&quot;dom_conitem&quot;:null,&quot;dom_ctrl&quot;:null,&quot;ue&quot;:null},{&quot;id&quot;:&quot;201581010133985&quot;,&quot;type&quot;:11,&quot;draggable&quot;:true,&quot;sort&quot;:13,&quot;content&quot;:{&quot;height&quot;:10},&quot;dom_conitem&quot;:null,&quot;dom_ctrl&quot;:null,&quot;ue&quot;:null},{&quot;id&quot;:&quot;201581010134412&quot;,&quot;type&quot;:10,&quot;draggable&quot;:true,&quot;sort&quot;:14,&quot;content&quot;:null,&quot;dom_conitem&quot;:null,&quot;dom_ctrl&quot;:null,&quot;ue&quot;:null},{&quot;id&quot;:&quot;201581010134700&quot;,&quot;type&quot;:3,&quot;draggable&quot;:true,&quot;sort&quot;:15,&quot;content&quot;:null,&quot;dom_conitem&quot;:null,&quot;dom_ctrl&quot;:null,&quot;ue&quot;:null},{&quot;id&quot;:&quot;20158101013522&quot;,&quot;type&quot;:2,&quot;draggable&quot;:true,&quot;sort&quot;:16,&quot;content&quot;:{&quot;title&quot;:&quot;标题名称&quot;,&quot;subtitle&quot;:&quot;『副标题』&quot;,&quot;direction&quot;:&quot;left&quot;,&quot;space&quot;:0},&quot;dom_conitem&quot;:null,&quot;dom_ctrl&quot;:null,&quot;ue&quot;:null},{&quot;id&quot;:&quot;201581010135317&quot;,&quot;type&quot;:9,&quot;draggable&quot;:true,&quot;sort&quot;:17,&quot;content&quot;:{&quot;showType&quot;:1,&quot;space&quot;:0,&quot;margin&quot;:10,&quot;dataset&quot;:[]},&quot;dom_conitem&quot;:null,&quot;dom_ctrl&quot;:null,&quot;ue&quot;:null},{&quot;id&quot;:&quot;201581010135628&quot;,&quot;type&quot;:8,&quot;draggable&quot;:true,&quot;sort&quot;:18,&quot;content&quot;:{&quot;dataset&quot;:[{&quot;linkType&quot;:0,&quot;link&quot;:&quot;#&quot;,&quot;title&quot;:&quot;导航名称&quot;,&quot;showtitle&quot;:&quot;导航名称&quot;,&quot;pic&quot;:&quot;/Public/images/diy/waitupload.png&quot;},{&quot;linkType&quot;:0,&quot;link&quot;:&quot;#&quot;,&quot;title&quot;:&quot;导航名称&quot;,&quot;showtitle&quot;:&quot;导航名称&quot;,&quot;pic&quot;:&quot;/Public/images/diy/waitupload.png&quot;},{&quot;linkType&quot;:0,&quot;link&quot;:&quot;#&quot;,&quot;title&quot;:&quot;导航名称&quot;,&quot;showtitle&quot;:&quot;导航名称&quot;,&quot;pic&quot;:&quot;/Public/images/diy/waitupload.png&quot;},{&quot;linkType&quot;:0,&quot;link&quot;:&quot;#&quot;,&quot;title&quot;:&quot;导航名称&quot;,&quot;showtitle&quot;:&quot;导航名称&quot;,&quot;pic&quot;:&quot;/Public/images/diy/waitupload.png&quot;}]},&quot;dom_conitem&quot;:null,&quot;dom_ctrl&quot;:null,&quot;ue&quot;:null},{&quot;id&quot;:&quot;201581010135911&quot;,&quot;type&quot;:1,&quot;draggable&quot;:true,&quot;sort&quot;:19,&quot;content&quot;:{&quot;fulltext&quot;:&quot;&amp;lt;p&amp;gt;『富文本编辑器』&amp;lt;/p&amp;gt;&quot;},&quot;ue&quot;:null,&quot;dom_conitem&quot;:null,&quot;dom_ctrl&quot;:null},{&quot;id&quot;:&quot;201581010136588&quot;,&quot;type&quot;:15,&quot;draggable&quot;:true,&quot;sort&quot;:20,&quot;content&quot;:{&quot;direct&quot;:0,&quot;imgsrc&quot;:&quot;&quot;,&quot;audiosrc&quot;:&quot;&quot;},&quot;dom_conitem&quot;:null,&quot;dom_ctrl&quot;:null,&quot;ue&quot;:null},{&quot;id&quot;:&quot;201581010104110&quot;,&quot;type&quot;:2,&quot;draggable&quot;:true,&quot;sort&quot;:21,&quot;content&quot;:{&quot;title&quot;:&quot;标题名称&quot;,&quot;subtitle&quot;:&quot;『副标题』&quot;,&quot;direction&quot;:&quot;left&quot;,&quot;space&quot;:0},&quot;dom_conitem&quot;:null,&quot;dom_ctrl&quot;:null,&quot;ue&quot;:null}]}" id="j-initdata">
                    <input type="hidden" name="template_id" value='default' runat="server" id="j_pageID">
                    <div class="diy clearfix div_center">
                        <div class="diy-actions">
                            <div class="diy-actions-title">添加模块</div>
                            <div class="diy-actions-addModules clearfix  app_zt">
                                <div class="tab">
                                    <a href="javascript:;" class="j-diy-addModule app_9" data-type="9"></a>
                                </div>
                                <div class="tab">
                                    <a href="javascript:;" class="j-diy-addModule app_8" data-type="8"></a>
                                </div>
                                <div class="tab">
                                    <a href="javascript:;" class="j-diy-addModule  app_1" data-type="20"></a>
                                </div>
                                <div class="tab">
                                    <a href="javascript:;" class="j-diy-addModule app_2" data-type="21"></a>
                                </div>
                                <div class="tab">
                                    <a href="javascript:;" class="j-diy-addModule app_3" data-type="22"></a>
                                </div>
                                <div class="tab">
                                    <a href="javascript:;" class="j-diy-addModule app_4" data-type="23"></a>
                                </div>
                                <div class="tab">
                                    <a href="javascript:;" class="j-diy-addModule app_5" data-type="24"></a>
                                </div>
                                  <div class="tab">
                                    <a href="javascript:;" class="j-diy-addModule " data-type="11">
                                      <b>辅助空白
                                        </b>
                                    </a>
                                </div>
                            </div>

                        </div>
                        <div class="diy-phone-outbox clearfix">
                            <div id="diy-phone">
                                <div class="diy-phone-header">
                                    <i class="diy-phone-receiver"></i>
                                    <img src="../images/app_head.jpg" class="app_head" />
                                </div>
                                <div class="diy-phone-contain" id="diy-contain">
                                    <div class="nodrag"></div>
                                    <div class="drag"></div>
                                </div>
                                <i class="diy-phone-footer"></i>
                            </div>

                        </div>
                        <div id="diy-ctrl">
                            <%--<div class="diy-ctrl-item" data-origin="pagetitle" style="margin-top: 85px;">
                                <div class="formitems">
                                    <label class="fi-name">页面标题：</label>
                                    <div class="form-controls">
                                        <input type="text" class="input j-pagetitle-ipt" value="店铺主页">
                                    </div>
                                </div>
                            </div>--%>
                        </div>
                        <div class="diy-actions-submit">
                            <a href="javascript:;" class="btn btn-primary" id="j-savePage-app">保存</a>
                            <a href="javascript:;" class="btn btn-danger" id="j-resetToInit">还原到初始模板</a>
                            <a href="javascript:scroll(0,0)" id="j-gotop" class="gotop" title="回到顶部"></a>
                        </div>
                    </div>
                </div>

                <!-- end content-right -->
            </div>
        </div>

        <!-- end container -->
        <!--gonggao-->
        <div class="footer">&copy; 2015 海商软件 , Inc. All rights reserved.</div>
        <!-- end footer -->

        <!-- end gotop -->

        <script type="text/j-template" id="tpl_tooltips">
        <div class="tooltips" style="display:none;">
            <span class="tooltips-arrow tooltips-arrow-<#= placement #>"><em>◆</em><i>◆</i></span>
            <#= content #>
        </div>
        </script>
        <!-- end tooltips -->

        <script type="text/j-template" id="tpl_hint">
        <div class="hint hint-<#= type #>"><#= content #></div>
        </script>
        <!-- end hint -->

        <script type="text/j-template" id="tpl_jbox_simple">
        <div class="mgt30"><#= content #></div>
        </script>
        <!-- end tpl_jbox_simple -->

        <script type="text/j-template" id="tpl_qrcode">
        <div id="qrcode">
            <img src="<#= src #>">
            <a href="javascript:;" class="qrcode-btn j-closeQrcode"><i class="gicon-remove white"></i></a>
        </div>
        </script>
        <!-- end qrcode -->

        <!-- diy common start -->
        <script type="text/j-template" id="tpl_diy_conitem">
        <div class="diy-conitem">
            <#= html #>
            <div class="diy-conitem-action">
                <div class="diy-conitem-action-btns">
                   <a href="javascript:;" class="diy-conitem-btn diy-Up j-Up">上移</a>
                    <a href="javascript:;" class="diy-conitem-btn diy-Down j-Down">下移</a>
                    <a href="javascript:;" class="diy-conitem-btn diy-edit j-edit">编辑</a>
                    <a href="javascript:;" class="diy-conitem-btn diy-del j-del">删除</a>
                </div>
            </div>
        </div>
        </script>

        <script type="text/j-template" id="tpl_diy_ctrl">
        <div class="diy-ctrl-item" data-origin="item">
            <#= html #>
        </div>
        </script>

        <script type="text/j-template" id="tpl_diy_con_type8">
        <div class="members_con">
            <section class="members_nav1">
                <ul style="background:#fff;">
                    <# if(content.dataset.length){ #>
                    <# _.each(content.dataset,function(item){ #>
                   <# if(item.pic && item.pic!=""){ #>
                    <li class="lisw<#= content.dataset.length #>">
                        <span><a href="<#= item.link #>"><img src="<#=item.pic#>" ></a></span>
                        <a class="members_nav1_name" href="<#= item.link #>"><#= item.showtitle #></a>
                    </li>
                    <#}else{#>
                    <li class="lisw<#= content.dataset.length #>">
                        <span><a href="<#= item.link #>"><img src="/Admin/shop/Public/images/diy/waitupload.png" ></a></span>
                        <a class="members_nav1_name" href="<#= item.link #>"><#= item.showtitle #></a>
                    </li>
                    <#}#>
                    <# }) #>
                    <# }else{ #>
                    <li>
                        <span><a href=""><img src="/Admin/shop/Public/images/diy/waitupload.png" ></a></span>
                        <a class="members_nav1_name" href="">导航文字</a>
                    </li>
                    <li>
                        <span><a href=""><img src="/Admin/shop/Public/images/diy/waitupload.png" ></a></span>
                        <a class="members_nav1_name" href="">导航文字</a>
                    </li>
                    <li>
                        <span><a href=""><img src="/Admin/shop/Public/images/diy/waitupload.png" ></a></span>
                        <a class="members_nav1_name" href="">导航文字</a>
                    </li>
                    <li>
                        <span><a href=""><img src="/Admin/shop/Public/images/diy/waitupload.png" ></a></span>
                        <a class="members_nav1_name" href="">导航文字</a>
                    </li>
                    <# } #>
                </ul>
            </section>
        </div>
        </script>

        <script type="text/j-template" id="tpl_diy_ctrl_type8">
        <ul class="ctrl-item-list">
            <# _.each(content.dataset,function(item){ #>
            <li class="ctrl-item-list-li clearfix">
                <div class="fl">
                    <div class="imgnav j-selectimg">
                        <# if(item.pic && item.pic!=""){ #>
                        <img src="<#= item.pic #>">
                        <span class="imgnav-reselect">重新选择</span>
                        <# }else{ #>
                        <img src="/Admin/shop/Public/images/diy/waitupload.png">
                        <p class="imgnav-select">选择图片</p>
                        <# } #>
                    </div>
                    <span class="fi-help-text txtCenter mgt5 j-verify-pic"></span>
                </div>

                <div class="fl imgnav-info">
                    <div class="formitems">
                        <label class="fi-name">链接到：</label>
                        <div class="form-controls">
                            <# if(item.linkType && item.linkType!=10 && item.linkType!=23 && item.linkType!=26){ #>        
                            <a href="<#= item.link?item.link:'javascript:void(0)' #>" class="badge badge-success" title="<#= item.title #>">
                                <span><#= HiShop.linkType[item.linkType] #></span>
                                <em class="badge-link ovfEps"><#= item.title #></em>
                            </a>
                            <#}#>
                            <# if(item.linkType==26){ #>
                            <input type="text" name="customlink" class="input xlarge" placeholder="请输入小程序AppID" value="<#= item.link #>">
                            <# } #>
                            <# if(item.linkType==23){ #>
                            <input type="text" name="customlink" class="input xlarge" placeholder="输入格式为：18888888888" value="<#= item.link #>">
                            <# } #>
                         <# if(item.linkType==10){ #>
                            <input type="text" name="customlink" class="input xlarge" placeholder="输入完整的URL(以http://或者https://开头)" value="<#= item.link #>">
                            <# } #>
                            <div class="droplist">
                                <a href="javascript:;" class="droplist-title j-droplist-toggle">
                                    <# if(item.linkType==0){ #>
                                    <span>请选择</span>
                                    <#}else{#>
                                    <span>修改</span>
                                    <#}#>
                                    <i class="gicon-chevron-down mgl5"></i>
                                </a>
                              <ul class="droplist-menu">
                                <li data-val="1"><a href="javascript:;"><#= HiShop.linkType[1] #></a></li>
                                <li data-val="2"><a href="javascript:;"><#= HiShop.linkType[2] #></a></li>
                                <li data-val="3"><a href="javascript:;"><#= HiShop.linkType[3] #></a></li>                               
                                <li data-val="4"><a href="javascript:;"><#= HiShop.linkType[4] #></a></li>  
                                <li data-val="24"><a href="javascript:;"><#= HiShop.linkType[24] #></a></li>  
                                <li data-val="7"><a href="javascript:;"><#= HiShop.linkType[7] #></a></li>
                                <li data-val="8"><a href="javascript:;"><#= HiShop.linkType[8] #></a></li>
                                <li data-val="10"><a href="javascript:;"><#= HiShop.linkType[10] #></a></li>  
                                <li data-val="12"><a href="javascript:;"><#= HiShop.linkType[12] #></a></li>
                                <li data-val="13"><a href="javascript:;"><#= HiShop.linkType[13] #></a></li>
                                <li data-val="14"><a href="javascript:;"><#= HiShop.linkType[14] #></a></li>
                                <li data-val="5"><a href="javascript:;"><#= HiShop.linkType[5] #></a></li>  
                                <li data-val="15"><a href="javascript:;"><#= HiShop.linkType[15] #></a></li>
                                <li data-val="16"><a href="javascript:;"><#= HiShop.linkType[16] #></a></li>
                                <li data-val="17"><a href="javascript:;"><#= HiShop.linkType[17] #></a></li>
                                <li data-val="18"><a href="javascript:;"><#= HiShop.linkType[18] #></a></li>
                                <li data-val="19"><a href="javascript:;"><#= HiShop.linkType[19] #></a></li>
                                <#if($("#hidOpenMultStore").val() == "1"){#>
                                <li data-val="20" t="0"><a href="javascript:;"><#= HiShop.linkType[20] #></a></li>
                                <#}#>
                                <li data-val="21"><a href="javascript:;"><#= HiShop.linkType[21] #></a></li>
                                <li data-val="22"><a href="javascript:;"><#= HiShop.linkType[22] #></a></li>
                                <li data-val="23"><a href="javascript:;"><#= HiShop.linkType[23] #></a></li>
                                <li data-val="26"><a href="javascript:;"><#= HiShop.linkType[26] #></a></li>
                            </ul>
                            </div> 
                           
                            <input type="hidden" class="j-verify" name="item_id" value="">
                            <span class="fi-help-text j-verify-linkType"></span>
                        </div>
                    </div>
                    <div class="formitems">
                        <label class="fi-name">导航名称：</label>
                        <div class="form-controls">
                            <input type="text" name="title" class="input xlarge" value="<#= item.showtitle #>">
                            <span class="fi-help-text"></span>
                        </div>
                    </div>
            <div class="formitems">
                        <label class="fi-name">建议尺寸：</label>
                        <div class="form-controls" style="line-height:28px;">
                            120*120
                        </div>
                    </div>
                </div>

                <div class="ctrl-item-list-actions">
                    <a href="javascript:;" title="上移" class="j-moveup"><i class="gicon-arrow-up"></i></a>
                    <a href="javascript:;" title="下移" class="j-movedown"><i class="gicon-arrow-down"></i></a>
                    <a href="javascript:;" title="删除" class="j-del"><i class="iconfont">&#xe601;</i></a>
                </div>
            </li>
            <# }) #>
            <# if(content.dataset.length < 4){ #>
            <li class="ctrl-item-list-add" title="添加">+</li>
                <# } #>
            </ul>
        <span class="fi-help-text mgt15 j-verify-least"></span>
        </script>

        <script type="text/j-template" id="tpl_diy_con_type9">
        <div class="members_con" style="margin:<# if(content.space == 1){ #>10px auto<# }else{ #>0 auto<# } #>">
            <# if(content.showType==1){ #>
            <section class="members_flash j-swipe" id="mySwipe">
                <ul class="clearfix">
                    <# if(content.dataset.length){ #>
                    <li>
                        <a href="<#= content.dataset[0].link #>" title="<#= content.dataset[0].showtitle #>">
                            <# if(content.dataset[0].pic!=""){ #>
                            <# if(content.is_compress){ #>
                            <img src="<#= content.dataset[0].pic #>300x300" width="100%" />
                            <# }else{ #>
                            <img src="<#= content.dataset[0].pic #>" width="100%" />
                            <# } #>
                            <# }else{ #>
                            <img src="/Admin/shop/Public/images/diy/waitupload.png" width="100%" />
                            <# } #>
                        </a>
                    </li>
                    <# }else{ #>
                    <li><a href="" title=""><img src="/Admin/shop/Public/images/diy/waitupload.png" width="100%" /></a></li>
                    <# } #>
                </ul>
                <section class="members_flash_time">
                    <# if(content.dataset.length>1){ #>
                    <# _.each(content.dataset,function(item,index){ #>
                    <span <# if(index==0){ #>class="cur"<# } #> ></span>
                    <# }) #>
                    <# } #>
                </section>
            </section>
            <# }else{ #>
            <section class="members_imgad">
                <ul class="clearfix">
                    <# if(content.dataset.length){ #>
                    <# _.each(content.dataset,function(item){ #>
                    <li style="margin-bottom:<# if(!content.margin){ #>0<# }else{ #><#= content.margin #><# } #>px">
                        <a href="<#= item.link #>" title="<#= item.showtitle #>">
                            <# if(item.pic!=""){ #>
                            <# if(item.is_compress){ #>
                            <img src="<#= item.pic #>300x300" width="100%" />
                            <# }else{ #>
                            <img src="<#= item.pic #>" width="100%" />
                            <# } #>
                            <# }else{ #>
                            <img src="/Admin/shop/Public/images/diy/waitupload.png" width="100%" />
                            <# } #>
                        </a>
                    </li>
                    <# }) #>
                    <# }else{ #>
                    <li><a href="" title=""><img src="/Admin/shop/Public/images/diy/waitupload.png" width="100%" /></a></li>
                    <# } #>
                </ul>
            </section>
            <# } #>
        </div>
        </script>

        <script type="text/j-template" id="tpl_diy_con_type9Phone">
        <div class="members_con" style="margin:<# if(content.space == 1){ #>10px auto<# }else{ #>0 auto<# } #>">
            <# if(content.showType==1){ #>
            <section class="members_flash j-swipe" id="mySwipe">
                <ul class="clearfix">
                    <# if(content.dataset.length){ #>
                        <# _.each(content.dataset,function(item){ #>
                            <li>
                                <a href="<#= item.link #>" title="<#= item.showtitle #>">
                                    <# if(item.pic!=""){ #>
                                    <# if(item.is_compress){ #>
                                    <img src="<#= item.pic #>300x300" width="100%" />
                                    <# }else{ #>
                                    <img src="<#= item.pic #>" width="100%" />
                                    <# } #>
                                    <# }else{ #>
                                    <img src="/Admin/shop/Public/images/diy/waitupload.png" width="100%" />
                                    <# } #>
                                </a>
                            </li>
                        <# }) #>
                    <# }else{ #>
                    <li><a href="" title=""><img src="/Admin/shop/Public/images/diy/waitupload.png" width="100%" /></a></li>
                    <# } #>
                </ul>
                <section class="members_flash_time">
                    <# if(content.dataset.length>1){ #>
                    <# _.each(content.dataset,function(item,index){ #>
                    <span <# if(index==0){ #>class="cur"<# } #> ></span>
                    <# }) #>
                    <# } #>
                </section>
            </section>
            <# }else{ #>
            <section class="members_imgad">
                <ul class="clearfix">
                    <# if(content.dataset.length){ #>
                    <# _.each(content.dataset,function(item){ #>
                    <li style="margin-bottom:<# if(!content.margin){ #>0<# }else{ #><#= content.margin #><# } #>px">
                        <a href="<#= item.link #>" title="<#= item.showtitle #>">
                            <# if(item.pic!=""){ #>
                            <# if(item.is_compress){ #>
                            <img src="<#= item.pic #>300x300" width="100%" />
                            <# }else{ #>
                            <img src="<#= item.pic #>" width="100%" />
                            <# } #>
                            <# }else{ #>
                            <img src="/Admin/Shop/Public/images/diy/imgad.jpg" width="100%" />
                            <# } #>
                        </a>
                    </li>
                    <# }) #>
                    <# }else{ #>
                    <li><a href="" title=""><img src="/Admin/shop/Public/images/diy/waitupload.png" width="100%" /></a></li>
                    <# } #>
                </ul>
            </section>
            <# } #>
        </div>
        </script>



        <script type="text/j-template" id="tpl_diy_ctrl_type9">
        <div class="formitems mgb10" style="display:none">
            <label class="fi-name">显示方式：</label>
            <div class="form-controls">
                <div class="radio-group">
                    <label><input type="radio" name="showType" value="1" <# if(content.showType==1){ #>checked<# } #> >折叠轮播</label>
                    <label><input type="radio" name="showType" value="2" <# if(content.showType==2){ #>checked<# } #>>分开显示</label>
                </div>
            </div>
        </div>
        <div class="formitems" style="display:none">
            <label class="fi-name">整体上下留白：</label>
            <div class="form-controls">
                <div class="radio-group">
                    <label><input type="radio" name="space" value="1" <# if(content.space==1 || content.space == undefined){ #>checked<# } #> >是</label>
                    <label><input type="radio" name="space" value="0" <# if(content.space==0){ #>checked<# } #> >否</label>
                </div>
            </div>
        </div>
        <# if(content.showType==2){ #>
        <div class="formitems inline">
            <label class="fi-name">每张图片上下距离：</label>
            <div class="form-controls">
                <div id="slider" class="fl"></div>
                <span class="fl mgl10 mgt5 ftsize14 j-ctrl-showheight"><# if(content.margin){ #><#= content.margin #><# }else{ #>0<# } #>px</span>
            </div>
        </div>
        <# } #>
        <ul class="ctrl-item-list">
            <# _.each(content.dataset,function(item){ #>
            <li class="ctrl-item-list-li clearfix">
                <div class="fl">
                    <div class="imgnav j-selectimg">
                        <# if(item.pic && item.pic!=""){ #>
                        <img src="<#= item.pic #>">        
                        <span class="imgnav-reselect">重新选择</span>
                        <# }else{ #>
                        <img src="/Admin/shop/Public/images/diy/waitupload.png">
                        <span class="imgnav-reselect">选择图片</span>
                        <# } #>
                    </div>
                    <span class="fi-help-text txtCenter mgt5 j-verify-pic"></span>
                </div>
                <div class="fl imgnav-info">
                    <div class="formitems">
                        <label class="fi-name">链接到：</label>
                        <div class="form-controls">
                          <# if(item.linkType && item.linkType!=10 && item.linkType!=23&& item.linkType!=26){ #>        
                            <a href="<#= item.link?item.link:'javascript:void(0)' #>" class="badge badge-success" title="<#= item.title #>">
                                <span><#= HiShop.linkType[item.linkType] #></span>
                                <em class="badge-link ovfEps"><#= item.title #></em>
                            </a>
                            <#}#>
                            <# if(item.linkType==26){ #>
                            <input type="text" name="customlink" class="input xlarge" placeholder="请输入小程序AppID" value="<#= item.link #>">
                            <# } #>
                            <# if(item.linkType==23){ #>
                            <input type="text" name="customlink" class="input xlarge" placeholder="输入格式为：18888888888" value="<#= item.link #>">
                            <# } #>
                         <# if(item.linkType==10){ #>
                            <input type="text" name="customlink" class="input xlarge" placeholder="输入完整的URL(以http://或者https://开头)" value="<#= item.link #>">
                            <# } #>
                            <div class="droplist">
                                <a href="javascript:;" class="droplist-title j-droplist-toggle">
                                    <# if(item.linkType==0){ #>
                                    <span>请选择</span>
                                    <#}else{#>
                                    <span>修改</span>
                                    <#}#>
                                    <i class="gicon-chevron-down mgl5"></i>
                                </a>                              
                               <ul class="droplist-menu">
                                 <li data-val="1"><a href="javascript:;"><#= HiShop.linkType[1] #></a></li>
                                <li data-val="2"><a href="javascript:;"><#= HiShop.linkType[2] #></a></li>
                                <li data-val="3"><a href="javascript:;"><#= HiShop.linkType[3] #></a></li>                                            
                                <li data-val="4"><a href="javascript:;"><#= HiShop.linkType[4] #></a></li>  
                                <li data-val="24"><a href="javascript:;"><#= HiShop.linkType[24] #></a></li>  
                                <li data-val="7"><a href="javascript:;"><#= HiShop.linkType[7] #></a></li>
                                <li data-val="8"><a href="javascript:;"><#= HiShop.linkType[8] #></a></li>
                                <li data-val="10"><a href="javascript:;"><#= HiShop.linkType[10] #></a></li>  
                                <li data-val="12"><a href="javascript:;"><#= HiShop.linkType[12] #></a></li>
                                <li data-val="13"><a href="javascript:;"><#= HiShop.linkType[13] #></a></li>
                                <li data-val="14"><a href="javascript:;"><#= HiShop.linkType[14] #></a></li>
                                <li data-val="5"><a href="javascript:;"><#= HiShop.linkType[5] #></a></li>  
                                <li data-val="15"><a href="javascript:;"><#= HiShop.linkType[15] #></a></li>
                                <li data-val="16"><a href="javascript:;"><#= HiShop.linkType[16] #></a></li>
                                <li data-val="17"><a href="javascript:;"><#= HiShop.linkType[17] #></a></li>
                                <li data-val="18"><a href="javascript:;"><#= HiShop.linkType[18] #></a></li>
                                <li data-val="19"><a href="javascript:;"><#= HiShop.linkType[19] #></a></li>
                                <#if($("#hidOpenMultStore").val() == "1"){#>
                                <li data-val="20" t="0"><a href="javascript:;"><#= HiShop.linkType[20] #></a></li>
                                <#}#>
                                <li data-val="21"><a href="javascript:;"><#= HiShop.linkType[21] #></a></li>
                                <li data-val="22"><a href="javascript:;"><#= HiShop.linkType[22] #></a></li>
                                <li data-val="23"><a href="javascript:;"><#= HiShop.linkType[23] #></a></li>
                                <li data-val="26"><a href="javascript:;"><#= HiShop.linkType[26] #></a></li>
                            </ul>
                            </div>
                            <input type="hidden" class="j-verify" name="item_id" value="">
                            <span class="fi-help-text j-verify-linkType"></span>
                        </div>
                    </div>
                    <div class="formitems">
                        <label class="fi-name">标题：</label>
                        <div class="form-controls">
                            <input type="text" name="title" class="input xlarge" value="<#= item.showtitle #>">
                            <span class="fi-help-text"></span>
                        </div>
                    </div>
              <div class="formitems">
                        <label class="fi-name">图片尺寸：</label>
            <div class="form-controls"  style="line-height:28px;">
                           750*360
            </div>
                    </div>
                </div>
                <div class="ctrl-item-list-actions">
                    <a href="javascript:;" title="上移" class="j-moveup"><i class="gicon-arrow-up"></i></a>
                    <a href="javascript:;" title="下移" class="j-movedown"><i class="gicon-arrow-down"></i></a>
                    <a href="javascript:;" title="删除" class="j-del"><i class="iconfont">&#xe601;</i></a>
                </div>
            </li>
            <# }) #>
            <# if(content.dataset.length
            <6){ #>
                <li class="ctrl-item-list-add" title="添加">+</li>
                <# } #>
        </ul>
        <span class="fi-help-text mgt15 j-verify-least"></span>
        </script>

        <script type="text/j-template" id="tpl_diy_con_type20">
        <div class="members_con">
            <section class="members_nav1">
           <ul class="clearfix">
                    <# if(content.dataset.length){ #>
                    <# _.each(content.dataset,function(item,e){ #>
                    <# if(content.layout == 1){ #>
                    <li class="board<#= content.dataset.length #><# if(content.layout == 1&&e == 0){ #> title_img<# } #><# if(content.layout == 1&&e == 1){ #> big_img<# } #><# if(content.layout == 1&&e == 2){ #> mid_img<# } #><# if(content.layout == 1&&e > 2){ #> small_img<# } #>">
                        <span>
                            <a href="<#= item.link #>">
             <# if(item.is_compress){ #>
                                
                                <img src="<#= item.pic #>_80x80" title="<#=content.dataset.length#>">
                                <# }else{ #>
                                <img src="<#= item.pic #>">
                                <# } #>
                            </a>
                        </span>
                    </li>
                    <# } #>
                    <# }) #>
                    <# }#>
                </ul>
            </section>
        </div>
        </script>

        <script type="text/j-template" id="tpl_diy_ctrl_type20">      
        <ul class="ctrl-item-list">
            <# _.each(content.dataset,function(item){ #>
            <li class="ctrl-item-list-li clearfix">
                <div class="fl">
                    <div class="imgnav j-selectimg">
                        <# if(item.pic && item.pic!=""){ #>
                        <# if(item.is_compress){ #>
                        <img src="<#= item.pic #>_80x80">
                        <# }else{ #>
                        <img src="<#= item.pic #>">
                        <# } #>
                        <span class="imgnav-reselect">重新选择</span>
                        <# }else{ #>
                        <p class="imgnav-select">选择图片</p>
                        <# } #>
                    </div>
                    <span class="fi-help-text txtCenter mgt5 j-verify-pic"></span>
                </div>
                <div class="fl imgnav-info">
            
                    <div class="formitems">
                        <label class="fi-name" ><# if(item.linkType==25){ #> 专题名称<#}else{#>链接到：<#}#></label>
                        <div class="form-controls" style="<# if(item.linkType==25){ #> display:none<# } #>">
                      <# if(item.linkType && item.linkType!=10 && item.linkType!=23 && item.linkType!=26){ #>        
                            <a href="<#= item.link?item.link:'javascript:void(0)' #>" class="badge badge-success" title="<#= item.title #>">
                                <span><#= HiShop.linkType[item.linkType] #></span>
                                <em class="badge-link ovfEps"><#= item.title #></em>
                            </a>
                            <#}#>
                           <# if(item.linkType==26){ #>
                            <input type="text" name="customlink" class="input xlarge" placeholder="请输入小程序AppID" value="<#= item.link #>">
                            <# } #>
                            <# if(item.linkType==23){ #>
                            <input type="text" name="customlink" class="input xlarge" placeholder="输入格式为：18888888888" value="<#= item.link #>">
                            <# } #>
                         <# if(item.linkType==10){ #>
                            <input type="text" name="customlink" class="input xlarge" placeholder="输入完整的URL(以http://或者https://开头)" value="<#= item.link #>">
                            <# } #>
                            <div class="droplist">
                                <a href="javascript:;" class="droplist-title j-droplist-toggle">
                                    <# if(item.linkType==0){ #>
                                    <span>请选择</span>
                                    <#}else{#>
                                    <span>修改</span>
                                    <#}#>
                                    <i class="gicon-chevron-down mgl5"></i>
                                </a>
                                <ul class="droplist-menu">
                                 <li data-val="1"><a href="javascript:;"><#= HiShop.linkType[1] #></a></li>
                                <li data-val="2"><a href="javascript:;"><#= HiShop.linkType[2] #></a></li>
                                <li data-val="3"><a href="javascript:;"><#= HiShop.linkType[3] #></a></li>                                            
                                <li data-val="4"><a href="javascript:;"><#= HiShop.linkType[4] #></a></li>  
                                <li data-val="24"><a href="javascript:;"><#= HiShop.linkType[24] #></a></li>  
                                <li data-val="7"><a href="javascript:;"><#= HiShop.linkType[7] #></a></li>
                                <li data-val="8"><a href="javascript:;"><#= HiShop.linkType[8] #></a></li>
                                <li data-val="10"><a href="javascript:;"><#= HiShop.linkType[10] #></a></li>  
                                <li data-val="12"><a href="javascript:;"><#= HiShop.linkType[12] #></a></li>
                                <li data-val="13"><a href="javascript:;"><#= HiShop.linkType[13] #></a></li>
                                <li data-val="14"><a href="javascript:;"><#= HiShop.linkType[14] #></a></li>
                                 <li data-val="5"><a href="javascript:;"><#= HiShop.linkType[5] #></a></li>
                                <li data-val="15"><a href="javascript:;"><#= HiShop.linkType[15] #></a></li>
                                <li data-val="16"><a href="javascript:;"><#= HiShop.linkType[16] #></a></li>
                                <li data-val="17"><a href="javascript:;"><#= HiShop.linkType[17] #></a></li>
                                <li data-val="18"><a href="javascript:;"><#= HiShop.linkType[18] #></a></li>
                                <li data-val="19"><a href="javascript:;"><#= HiShop.linkType[19] #></a></li>
                                <#if($("#hidOpenMultStore").val() == "1"){#>
                                <li data-val="20" t="0"><a href="javascript:;"><#= HiShop.linkType[20] #></a></li>
                                <#}#>
                                <li data-val="21"><a href="javascript:;"><#= HiShop.linkType[21] #></a></li>
                                <li data-val="22"><a href="javascript:;"><#= HiShop.linkType[22] #></a></li>
                                <li data-val="23"><a href="javascript:;"><#= HiShop.linkType[23] #></a></li>
                                <li data-val="26"><a href="javascript:;"><#= HiShop.linkType[26] #></a></li>
                            </ul>
                            </div> 
                            <input type="hidden" class="j-verify" name="item_id" value="">
                            <span class="fi-help-text j-verify-linkType"></span>
                        </div>                         
                    </div>             
                </div>

            </li>
            <# }) #>
        </ul>
        <span class="fi-help-text mgt15 j-verify-least"></span>
        </script>

          <script type="text/j-template" id="tpl_diy_con_type11">
        <div class="members_con">
            <section class="custom-space" style="height:<#= content.height #>px;"></section>
        </div>
        </script>
        
        <script type="text/j-template" id="tpl_diy_ctrl_type11">
        <div class="formitems inline">
            <label class="fi-name">高度：</label>
            <div class="form-controls">
                <div id="slider" class="fl"></div>
                <span class="fl mgl10 mgt5 ftsize14 j-ctrl-showheight"><#= content.height #>px</span>
            </div>
        </div>
        </script>

        <script type="text/j-template" id="tpl_diy_con_type21">
        <div class="members_con">
            <section class="members_nav1">
           <ul class="clearfix">
                    <# if(content.dataset.length){ #>
                    <# _.each(content.dataset,function(item,e){ #>
                    <# if(content.layout == 1){ #>
                    <li class="board<#= content.dataset.length #><# if(content.layout == 1&&e == 0){ #> title_img<# } #><# if(content.layout == 1&&e == 1|| e == 4){ #> mid1_img<# } #><# if(content.layout == 1&& e ==2||e==3||e==5||e==6){ #> small1_img<# } #>">
                        <span>
                            <a href="<#= item.link #>">
             <# if(item.is_compress){ #>
                                <img src="<#= item.pic #>_80x80">
                                <# }else{ #>
                                <img src="<#= item.pic #>">
                                <# } #>
                            </a>
                        </span>
                    </li>
                    <# } #>
                    <# }) #>
                    <# }#>
                </ul>
            </section>
        </div>
        </script>

        <script type="text/j-template" id="tpl_diy_ctrl_type21">
      <ul class="ctrl-item-list">
            <# _.each(content.dataset,function(item){ #>
            <li class="ctrl-item-list-li clearfix">
                <div class="fl">
                    <div class="imgnav j-selectimg">
                        <# if(item.pic && item.pic!=""){ #>
                        <# if(item.is_compress){ #>
                        <img src="<#= item.pic #>_80x80">
                        <# }else{ #>
                        <img src="<#= item.pic #>">
                        <# } #>
                        <span class="imgnav-reselect">重新选择</span>
                        <# }else{ #>
                        <p class="imgnav-select">选择图片</p>
                        <# } #>
                    </div>
                    <span class="fi-help-text txtCenter mgt5 j-verify-pic"></span>
                </div>

                <div class="fl imgnav-info">
                    <div class="formitems">
                        <label class="fi-name" ><# if(item.linkType==25){ #> 专题名称<#}else{#>链接到：<#}#></label>
                        <div class="form-controls" style="<# if(item.linkType==25){ #> display:none<# } #>">
                            <# if(item.linkType && item.linkType!=10 && item.linkType!=23 && item.linkType!=26){ #>        
                            <a href="<#= item.link?item.link:'javascript:void(0)' #>" class="badge badge-success" title="<#= item.title #>">
                                <span><#= HiShop.linkType[item.linkType] #></span>
                                <em class="badge-link ovfEps"><#= item.title #></em>
                            </a>
                            <#}#>
                           <# if(item.linkType==26){ #>
                            <input type="text" name="customlink" class="input xlarge" placeholder="请输入小程序AppID" value="<#= item.link #>">
                            <# } #>
                            <# if(item.linkType==23){ #>
                            <input type="text" name="customlink" class="input xlarge" placeholder="输入格式为：18888888888" value="<#= item.link #>">
                            <# } #>
                         <# if(item.linkType==10){ #>
                            <input type="text" name="customlink" class="input xlarge" placeholder="输入完整的URL(以http://或者https://开头)" value="<#= item.link #>">
                            <# } #>
                            <div class="droplist">
                                <a href="javascript:;" class="droplist-title j-droplist-toggle">
                                    <# if(item.linkType==0){ #>
                                    <span>请选择</span>
                                    <#}else{#>
                                    <span>修改</span>
                                    <#}#>
                                    <i class="gicon-chevron-down mgl5"></i>
                                </a>
                                <ul class="droplist-menu">
                                 <li data-val="1"><a href="javascript:;"><#= HiShop.linkType[1] #></a></li>
                                <li data-val="2"><a href="javascript:;"><#= HiShop.linkType[2] #></a></li>
                                <li data-val="3"><a href="javascript:;"><#= HiShop.linkType[3] #></a></li>                                            
                                <li data-val="4"><a href="javascript:;"><#= HiShop.linkType[4] #></a></li>  
                                <li data-val="24"><a href="javascript:;"><#= HiShop.linkType[24] #></a></li>
                                <li data-val="7"><a href="javascript:;"><#= HiShop.linkType[7] #></a></li>
                                <li data-val="8"><a href="javascript:;"><#= HiShop.linkType[8] #></a></li>
                                <li data-val="10"><a href="javascript:;"><#= HiShop.linkType[10] #></a></li>  
                                <li data-val="12"><a href="javascript:;"><#= HiShop.linkType[12] #></a></li>
                                <li data-val="13"><a href="javascript:;"><#= HiShop.linkType[13] #></a></li>
                                <li data-val="14"><a href="javascript:;"><#= HiShop.linkType[14] #></a></li>
                                <li data-val="5"><a href="javascript:;"><#= HiShop.linkType[5] #></a></li>  
                                <li data-val="15"><a href="javascript:;"><#= HiShop.linkType[15] #></a></li>
                                <li data-val="16"><a href="javascript:;"><#= HiShop.linkType[16] #></a></li>
                                <li data-val="17"><a href="javascript:;"><#= HiShop.linkType[17] #></a></li>
                                <li data-val="18"><a href="javascript:;"><#= HiShop.linkType[18] #></a></li>
                                <li data-val="19"><a href="javascript:;"><#= HiShop.linkType[19] #></a></li>
                                <#if($("#hidOpenMultStore").val() == "1"){#>
                                <li data-val="20" t="0"><a href="javascript:;"><#= HiShop.linkType[20] #></a></li>
                                <#}#>
                                <li data-val="21"><a href="javascript:;"><#= HiShop.linkType[21] #></a></li>
                                <li data-val="22"><a href="javascript:;"><#= HiShop.linkType[22] #></a></li>
                                <li data-val="23"><a href="javascript:;"><#= HiShop.linkType[23] #></a></li>
                                <li data-val="26"><a href="javascript:;"><#= HiShop.linkType[26] #></a></li>
                            </ul>
                            </div> 
                            <input type="hidden" class="j-verify" name="item_id" value="">
                            <span class="fi-help-text j-verify-linkType"></span>
                        </div>
                    </div>  
                </div>
            </li>
            <# }) #>
        </ul>
        <span class="fi-help-text mgt15 j-verify-least"></span>
        </script>

        <script type="text/j-template" id="tpl_diy_con_type22">
        <div class="members_con">
            <section class="members_nav1">
           <ul class="clearfix">
                    <# if(content.dataset.length){ #>
                    <# _.each(content.dataset,function(item,e){ #>
                    <# if(content.layout == 1){ #>
                    <li class="board<#= content.dataset.length #><# if(content.layout == 1&&e == 0){ #> title_img<# } #><# if(content.layout == 1&&e != 0){ #> big1_img<# } #>">
                        <span>
                            <a href="<#= item.link #>">
             <# if(item.is_compress){ #>
                                <img src="<#= item.pic #>_80x80">
                                <# }else{ #>
                                <img src="<#= item.pic #>">
                                <# } #>
                            </a>
                        </span>
                    </li>
                    <# } #>
                    <# }) #>
                    <# }#>
                </ul>
            </section>
        </div>
        </script>

        <script type="text/j-template" id="tpl_diy_ctrl_type22">
      <ul class="ctrl-item-list">
            <# _.each(content.dataset,function(item){ #>
            <li class="ctrl-item-list-li clearfix">
                <div class="fl">
                    <div class="imgnav j-selectimg">
                        <# if(item.pic && item.pic!=""){ #>
                        <# if(item.is_compress){ #>
                        <img src="<#= item.pic #>_80x80">
                        <# }else{ #>
                        <img src="<#= item.pic #>">
                        <# } #>
                        <span class="imgnav-reselect">重新选择</span>
                        <# }else{ #>
                        <p class="imgnav-select">选择图片</p>
                        <# } #>
                    </div>
                    <span class="fi-help-text txtCenter mgt5 j-verify-pic"></span>
                </div>

                <div class="fl imgnav-info">
                    <div class="formitems">
                       <label class="fi-name" ><# if(item.linkType==25){ #> 专题名称<#}else{#>链接到：<#}#></label>
                        <div class="form-controls" style="<# if(item.linkType==25){ #> display:none<# } #>">
                          <# if(item.linkType && item.linkType!=10 && item.linkType!=23&& item.linkType!=26){ #>        
                            <a href="<#= item.link?item.link:'javascript:void(0)' #>" class="badge badge-success" title="<#= item.title #>">
                                <span><#= HiShop.linkType[item.linkType] #></span>
                                <em class="badge-link ovfEps"><#= item.title #></em>
                            </a>
                            <#}#>
                           <# if(item.linkType==26){ #>
                            <input type="text" name="customlink" class="input xlarge" placeholder="请输入小程序AppID" value="<#= item.link #>">
                            <# } #>
                            <# if(item.linkType==23){ #>
                            <input type="text" name="customlink" class="input xlarge" placeholder="输入格式为：18888888888" value="<#= item.link #>">
                            <# } #>
                         <# if(item.linkType==10){ #>
                            <input type="text" name="customlink" class="input xlarge" placeholder="输入完整的URL(以http://或者https://开头)" value="<#= item.link #>">
                            <# } #>
                            <div class="droplist">
                                <a href="javascript:;" class="droplist-title j-droplist-toggle">
                                    <# if(item.linkType==0){ #>
                                    <span>请选择</span>
                                    <#}else{#>
                                    <span>修改</span>
                                    <#}#>
                                    <i class="gicon-chevron-down mgl5"></i>
                                </a>
                                <ul class="droplist-menu">
                                 <li data-val="1"><a href="javascript:;"><#= HiShop.linkType[1] #></a></li>
                                <li data-val="2"><a href="javascript:;"><#= HiShop.linkType[2] #></a></li>
                                <li data-val="3"><a href="javascript:;"><#= HiShop.linkType[3] #></a></li>                                            
                                <li data-val="4"><a href="javascript:;"><#= HiShop.linkType[4] #></a></li>  
                                <li data-val="24"><a href="javascript:;"><#= HiShop.linkType[24] #></a></li>
                                <li data-val="7"><a href="javascript:;"><#= HiShop.linkType[7] #></a></li>
                                <li data-val="8"><a href="javascript:;"><#= HiShop.linkType[8] #></a></li> 
                                <li data-val="10"><a href="javascript:;"><#= HiShop.linkType[10] #></a></li>  
                                <li data-val="12"><a href="javascript:;"><#= HiShop.linkType[12] #></a></li>
                                <li data-val="13"><a href="javascript:;"><#= HiShop.linkType[13] #></a></li>
                                <li data-val="14"><a href="javascript:;"><#= HiShop.linkType[14] #></a></li>
                                <li data-val="5"><a href="javascript:;"><#= HiShop.linkType[5] #></a></li>  
                                <li data-val="15"><a href="javascript:;"><#= HiShop.linkType[15] #></a></li>
                                <li data-val="16"><a href="javascript:;"><#= HiShop.linkType[16] #></a></li>
                                <li data-val="17"><a href="javascript:;"><#= HiShop.linkType[17] #></a></li>
                                <li data-val="18"><a href="javascript:;"><#= HiShop.linkType[18] #></a></li>
                                <li data-val="19"><a href="javascript:;"><#= HiShop.linkType[19] #></a></li>
                                <#if($("#hidOpenMultStore").val() == "1"){#>
                                <li data-val="20" t="0"><a href="javascript:;"><#= HiShop.linkType[20] #></a></li>
                                <#}#>
                                <li data-val="21"><a href="javascript:;"><#= HiShop.linkType[21] #></a></li>
                                <li data-val="22"><a href="javascript:;"><#= HiShop.linkType[22] #></a></li>
                                <li data-val="23"><a href="javascript:;"><#= HiShop.linkType[23] #></a></li>
                                <li data-val="26"><a href="javascript:;"><#= HiShop.linkType[26] #></a></li>
                            </ul>
                            </div> 
                           
                            <input type="hidden" class="j-verify" name="item_id" value="">
                            <span class="fi-help-text j-verify-linkType"></span>
                        </div>                         
                    </div>           
                </div>
            </li>
            <# }) #>
        </ul>
        <span class="fi-help-text mgt15 j-verify-least"></span>
        </script>
        <script type="text/j-template" id="tpl_diy_con_type23">
        <div class="members_con">
            <section class="members_nav1">
           <ul class="clearfix">
                    <# if(content.dataset.length){ #>
                    <# _.each(content.dataset,function(item,e){ #>
                    <# if(content.layout == 1){ #>
                    <li class="board<#= content.dataset.length #><# if(e == 0){ #> title_img<# } #><# if(e > 0&&e<5){ #> big1_img<# } #><# if(e > 4){ #> small2_img<# } #>">
                        <span>
                            <a href="<#= item.link #>">
             <# if(item.is_compress){ #>
                                <img src="<#= item.pic #>_80x80">
                                <# }else{ #>
                                <img src="<#= item.pic #>">
                                <# } #>
                            </a>
                        </span>
                    </li>
                    <# } #>
                    <# }) #>
                    <# }#>
                </ul>
            </section>
        </div>
        </script>

        <script type="text/j-template" id="tpl_diy_ctrl_type23">
       <ul class="ctrl-item-list">
            <# _.each(content.dataset,function(item){ #>
            <li class="ctrl-item-list-li clearfix">
                <div class="fl">
                    <div class="imgnav j-selectimg">
                        <# if(item.pic && item.pic!=""){ #>
                        <# if(item.is_compress){ #>
                        <img src="<#= item.pic #>_80x80">
                        <# }else{ #>
                        <img src="<#= item.pic #>">
                        <# } #>
                        <span class="imgnav-reselect">重新选择</span>
                        <# }else{ #>
                        <p class="imgnav-select">选择图片</p>
                        <# } #>
                    </div>
                    <span class="fi-help-text txtCenter mgt5 j-verify-pic"></span>
                </div>

                <div class="fl imgnav-info">
                    <div class="formitems">
                        <label class="fi-name" ><# if(item.linkType==25){ #> 专题名称<#}else{#>链接到：<#}#></label>
                        <div class="form-controls" style="<# if(item.linkType==25){ #> display:none<# } #>">
                         <# if(item.linkType && item.linkType!=10 && item.linkType!=23 && item.linkType!=26){ #>        
                            <a href="<#= item.link?item.link:'javascript:void(0)' #>" class="badge badge-success" title="<#= item.title #>">
                                <span><#= HiShop.linkType[item.linkType] #></span>
                                <em class="badge-link ovfEps"><#= item.title #></em>
                            </a>
                            <#}#>
                           <# if(item.linkType==26){ #>
                            <input type="text" name="customlink" class="input xlarge" placeholder="请输入小程序AppID" value="<#= item.link #>">
                            <# } #>
                            <# if(item.linkType==23){ #>
                            <input type="text" name="customlink" class="input xlarge" placeholder="输入格式为：18888888888" value="<#= item.link #>">
                            <# } #>
                         <# if(item.linkType==10){ #>
                            <input type="text" name="customlink" class="input xlarge" placeholder="输入完整的URL(以http://或者https://开头)" value="<#= item.link #>">
                            <# } #>
                            <div class="droplist">
                                <a href="javascript:;" class="droplist-title j-droplist-toggle">
                                    <# if(item.linkType==0){ #>
                                    <span>请选择</span>
                                    <#}else{#>
                                    <span>修改</span>
                                    <#}#>
                                    <i class="gicon-chevron-down mgl5"></i>
                                </a>
                                <ul class="droplist-menu">
                                 <li data-val="1"><a href="javascript:;"><#= HiShop.linkType[1] #></a></li>
                                <li data-val="2"><a href="javascript:;"><#= HiShop.linkType[2] #></a></li>
                                <li data-val="3"><a href="javascript:;"><#= HiShop.linkType[3] #></a></li>
                                <li data-val="4"><a href="javascript:;"><#= HiShop.linkType[4] #></a></li>  
                                <li data-val="24"><a href="javascript:;"><#= HiShop.linkType[24] #></a></li> 
                                <li data-val="7"><a href="javascript:;"><#= HiShop.linkType[7] #></a></li>
                                <li data-val="8"><a href="javascript:;"><#= HiShop.linkType[8] #></a></li>
                                <li data-val="10"><a href="javascript:;"><#= HiShop.linkType[10] #></a></li>  
                                <li data-val="12"><a href="javascript:;"><#= HiShop.linkType[12] #></a></li>
                                <li data-val="13"><a href="javascript:;"><#= HiShop.linkType[13] #></a></li>
                                <li data-val="14"><a href="javascript:;"><#= HiShop.linkType[14] #></a></li>
                                <li data-val="5"><a href="javascript:;"><#= HiShop.linkType[5] #></a></li>  
                                <li data-val="15"><a href="javascript:;"><#= HiShop.linkType[15] #></a></li>
                                <li data-val="16"><a href="javascript:;"><#= HiShop.linkType[16] #></a></li>
                                <li data-val="17"><a href="javascript:;"><#= HiShop.linkType[17] #></a></li>
                                <li data-val="18"><a href="javascript:;"><#= HiShop.linkType[18] #></a></li>
                                <li data-val="19"><a href="javascript:;"><#= HiShop.linkType[19] #></a></li>
                                <#if($("#hidOpenMultStore").val() == "1"){#>
                                <li data-val="20" t="0"><a href="javascript:;"><#= HiShop.linkType[20] #></a></li>
                                <#}#>
                                <li data-val="21"><a href="javascript:;"><#= HiShop.linkType[21] #></a></li>
                                <li data-val="22"><a href="javascript:;"><#= HiShop.linkType[22] #></a></li>
                                <li data-val="23"><a href="javascript:;"><#= HiShop.linkType[23] #></a></li>
                            </ul>
                            </div> 
                            <input type="hidden" class="j-verify" name="item_id" value="">
                            <span class="fi-help-text j-verify-linkType"></span>
                        </div>                         
                    </div>           
                </div>
            </li>
            <# }) #>
        </ul>
        <span class="fi-help-text mgt15 j-verify-least"></span>
        </script>
        <script type="text/j-template" id="tpl_diy_con_type24">
        <div class="members_con">
            <section class="members_nav1">
           <ul class="clearfix">
                    <# if(content.dataset.length){ #>
                    <# _.each(content.dataset,function(item,e){ #>
                    <# if(content.layout == 1){ #>
                    <li class="board ad_img">
                        <span>
                            <a href="<#= item.link #>">
             <# if(item.is_compress){ #>
                                <img src="<#= item.pic #>_80x80">
                                <# }else{ #>
                                <img src="<#= item.pic #>">
                                <# } #>
                            </a>
                        </span>
                    </li>
                    <# } #>
                    <# }) #>
                    <# }#>
                </ul>
            </section>
        </div>
        </script>

        <script type="text/j-template" id="tpl_diy_ctrl_type24">
       <ul class="ctrl-item-list">
            <# _.each(content.dataset,function(item){ #>
            <li class="ctrl-item-list-li clearfix">
                <div class="fl">
                    <div class="imgnav j-selectimg">
                        <# if(item.pic && item.pic!=""){ #>
                        <# if(item.is_compress){ #>
                        <img src="<#= item.pic #>_80x80">
                        <# }else{ #>
                        <img src="<#= item.pic #>">
                        <# } #>
                        <span class="imgnav-reselect">重新选择</span>
                        <# }else{ #>
                        <p class="imgnav-select">选择图片</p>
                        <# } #>
                    </div>
                    <span class="fi-help-text txtCenter mgt5 j-verify-pic"></span>
                </div>

                <div class="fl imgnav-info">
                    <div class="formitems">
                        <label class="fi-name">链接到：</label>
                        <div class="form-controls">
                    <# if(item.linkType && item.linkType!=10 && item.linkType!=23 && item.linkType!=26){ #>        
                            <a href="<#= item.link?item.link:'javascript:void(0)' #>" class="badge badge-success" title="<#= item.title #>">
                                <span><#= HiShop.linkType[item.linkType] #></span>
                                <em class="badge-link ovfEps"><#= item.title #></em>
                            </a>
                            <#}#>
                            <# if(item.linkType==26){ #>
                            <input type="text" name="customlink" class="input xlarge" placeholder="请输入小程序AppID" value="<#= item.link #>">
                            <# } #>
                            <# if(item.linkType==23){ #>
                            <input type="text" name="customlink" class="input xlarge" placeholder="输入格式为：18888888888" value="<#= item.link #>">
                            <# } #>
                         <# if(item.linkType==10){ #>
                            <input type="text" name="customlink" class="input xlarge" placeholder="输入完整的URL(以http://或者https://开头)" value="<#= item.link #>">
                            <# } #>
                            <div class="droplist">
                                <a href="javascript:;" class="droplist-title j-droplist-toggle">
                                    <# if(item.linkType==0){ #>
                                    <span>请选择</span>
                                    <#}else{#>
                                    <span>修改</span>
                                    <#}#>
                                    <i class="gicon-chevron-down mgl5"></i>
                                </a>
                                <ul class="droplist-menu">
                                 <li data-val="1"><a href="javascript:;"><#= HiShop.linkType[1] #></a></li>
                                <li data-val="2"><a href="javascript:;"><#= HiShop.linkType[2] #></a></li>         
                                 <li data-val="3"><a href="javascript:;"><#= HiShop.linkType[3] #></a></li>                       
                                <li data-val="4"><a href="javascript:;"><#= HiShop.linkType[4] #></a></li>  
                                <li data-val="24"><a href="javascript:;"><#= HiShop.linkType[24] #></a></li> 
                                <li data-val="7"><a href="javascript:;"><#= HiShop.linkType[7] #></a></li>
                                <li data-val="8"><a href="javascript:;"><#= HiShop.linkType[8] #></a></li>
                                <li data-val="10"><a href="javascript:;"><#= HiShop.linkType[10] #></a></li>  
                                <li data-val="12"><a href="javascript:;"><#= HiShop.linkType[12] #></a></li>
                                <li data-val="13"><a href="javascript:;"><#= HiShop.linkType[13] #></a></li>
                                <li data-val="14"><a href="javascript:;"><#= HiShop.linkType[14] #></a></li>
                                <li data-val="5"><a href="javascript:;"><#= HiShop.linkType[5] #></a></li>  
                                <li data-val="15"><a href="javascript:;"><#= HiShop.linkType[15] #></a></li>
                                <li data-val="16"><a href="javascript:;"><#= HiShop.linkType[16] #></a></li>
                                <li data-val="17"><a href="javascript:;"><#= HiShop.linkType[17] #></a></li>
                                <li data-val="18"><a href="javascript:;"><#= HiShop.linkType[18] #></a></li>
                                <li data-val="19"><a href="javascript:;"><#= HiShop.linkType[19] #></a></li>
                                <#if($("#hidOpenMultStore").val() == "1"){#>
                                <li data-val="20" t="0"><a href="javascript:;"><#= HiShop.linkType[20] #></a></li>
                                <#}#>
                                <li data-val="21"><a href="javascript:;"><#= HiShop.linkType[21] #></a></li>
                                <li data-val="22"><a href="javascript:;"><#= HiShop.linkType[22] #></a></li>
                                <li data-val="23"><a href="javascript:;"><#= HiShop.linkType[23] #></a></li>
                            </ul>
                            </div> 
                            <input type="hidden" class="j-verify" name="item_id" value="">
                            <span class="fi-help-text j-verify-linkType"></span>
                        </div>                         
                    </div>           
                </div>
            </li>
            <# }) #>
        </ul>
        <span class="fi-help-text mgt15 j-verify-least"></span>
        </script>

        <!-- type 13 board end -->

        <!-- end ImgPicker -->
        <!-- start ModulePicker -->
        <script type="text/j-template" id="tpl_popbox_ModulePicker">
        <div id="ModulePicker">
            <ul class="modulePicker-list"></ul>
            <div class="clearfix mgt10">
                <div class="paginate fr"></div>
            </div>
        </div>
        </script>

        <script type="text/j-template" id="tpl_popbox_ModulePicker_item">
        <# _.each(dataset,function(data){#>
        <li class="clearfix">
            <a href="<#= data.link #>" target="_blank" class="modulePicker-list-title fl ovfEps a_hover" title="<#= data.title #>"><#= data.title #></a>
            <a href="javascript:;" class="btn btn-primary btn-small fr j-select">选取</a>
        </li>
        <# }) #>
        </script>
        <!-- end ModulePicker -->
        <!-- start GoodsAndGroupPicker -->
        <script type="text/j-template" id="tpl_popbox_GoodsAndGroupPicker">
        <div id="GoodsAndGroupPicker">
            <div class="tabs clearfix">
                <a href="javascript:;" class="active tabs_a fl" data-origin="goodsandgroup" data-index="1">1商品</a>
                <a href="javascript:;" class="tabs_a fl j-tab-group" data-origin="goodsandgroup" data-index="2">商品分组</a>
            </div>
            <!-- end tabs -->
            <div class="tabs-content" data-origin="goodsandgroup">
                <div class="tc" data-index="1">
                    <ul class="gagp-goodslist"></ul>
                    <div class="clearfix mgt10">
                        <div class="fl">
                            <a href="javascript:;" class="btn btn-primary j-btn-goodsuse">确定使用</a>
                        </div>
                        <div class="paginate fr"></div>
                    </div>
                </div>
                <div class="tc hide" data-index="2">
                    <ul class="gagp-grouplist"></ul>
                    <div class="clearfix mgt10">
                        <div class="paginate fr"></div>
                    </div>
                </div>
            </div>
        </div>
        </script>

        <script type="text/j-template" id="tpl_popbox_GoodsAndGroupPicker_goodsitem">
        <# _.each(dataset,function(data){#>
        <li class="clearfix" data-item="<#= data.item_id #>">
            <a href="<#= data.link #>" class="fl" target="_blank" title="<#= data.title #>">
                <div class="table-item-img">
                    <# if(data.is_compress){ #>
                    <img src="<#= data.pic #>80x80" alt="<#= data.title #>">
                    <# }else{ #>
                    <img src="<#= data.pic #>" alt="<#= data.title #>">
                    <# } #>
                </div>
                <div class="table-item-info">
                    <p><#= data.title #></p>
                    <span class="price">&yen;<#= data.price #></span>
                </div>
            </a>
            <a href="javascript:;" class="btn fr j-select mgt15 mgr15">选取</a>
        </li>
        <# }) #>
        </script>

        <script type="text/j-template" id="tpl_popbox_GoodsAndGroupPicker_groupitem">
        <# _.each(dataset,function(data){#>
        <li class="clearfix" data-group="<#= data.group_id #>">
            <a href="<#= data.link #>" class="fl a_hover" target="_blank" title="<#= data.title #>"><#= data.title #></a>
            <a href="javascript:;" class="btn fr j-select">选取</a>
        </li>
        <# }) #>
        </script>
        <!-- end GoodsAndGroupPicker -->
        <!-- start MgzAndMgzCate -->
        <script type="text/j-template" id="tpl_popbox_MgzAndMgzCate">
        <div id="MgzAndMgzCate">
            <div class="tabs clearfix">
                <a href="javascript:;" class="active tabs_a fl" data-origin="MgzAndMgzCate" data-index="1">专题页面</a>
                <a href="javascript:;" class="tabs_a fl j-tab-mgzcate" data-origin="MgzAndMgzCate" data-index="2">页面分类</a>
            </div>
            <!-- end tabs -->
            <div class="tabs-content" data-origin="MgzAndMgzCate">
                <div class="tc" data-index="1">
                    <ul class="mgz-list mgz-list-panel1"></ul>
                    <div class="clearfix mgt10">
                        <div class="paginate fr"></div>
                    </div>
                </div>
                <div class="tc hide" data-index="2">
                    <ul class="mgz-list mgz-list-panel2"></ul>
                    <div class="clearfix mgt10">
                        <div class="fl">
                            <a href="javascript:;" class="btn btn-primary j-btn-use">确定使用</a>
                        </div>
                        <div class="paginate fr"></div>
                    </div>
                </div>
            </div>
        </div>
        </script>

        <script type="text/j-template" id="tpl_popbox_MgzAndMgzCate_item">
        <# _.each(dataset,function(data){#>
        <li class="clearfix">
            <a href="<#= data.link #>" class="fl a_hover" target="_blank" title="<#= data.title #>"><#= data.title #></a>
            <a href="javascript:;" class="btn fr j-select">选取</a>
        </li>
        <# }) #>
        </script>
        <!-- end MgzAndMgzCate -->
        <!-- start GamePicker -->
        <script type="text/j-template" id="tpl_popbox_GamePicker">
        <div id="GamePicker">
            <div class="tabs clearfix">
                <a href="javascript:;" class="active tabs_a fl" data-origin="GamePicker" data-index="1">大转盘</a>
                <a href="javascript:;" class="tabs_a fl j-tab-game" data-origin="GamePicker" data-index="2">刮刮卡</a>
                <a href="javascript:;" class="tabs_a fl j-tab-game" data-origin="GamePicker" data-index="3">砸金蛋</a>
                <a href="javascript:;" class="tabs_a fl j-tab-game" data-origin="GamePicker" data-index="4">微抽奖</a>
                <a href="javascript:;" class="tabs_a fl j-tab-game" data-origin="GamePicker" data-index="5">微报名</a>
            </div>
            <!-- end tabs -->
            <div class="tabs-content" data-origin="GamePicker">
                <div class="tc" data-index="1">
                    <ul class="game-list game-list-panel1"></ul>
                    <div class="clearfix mgt10">
                        <div class="paginate fr"></div>
                    </div>
                </div>
                <div class="tc hide" data-index="2">
                    <ul class="game-list game-list-panel2"></ul>
                    <div class="clearfix mgt10">
                        <div class="paginate fr"></div>
                    </div>
                </div>
                <div class="tc hide" data-index="3">
                    <ul class="game-list game-list-panel3"></ul>
                    <div class="clearfix mgt10">
                        <div class="paginate fr"></div>
                    </div>
                </div>
                <div class="tc hide" data-index="4">
                    <ul class="game-list game-list-panel4"></ul>
                    <div class="clearfix mgt10">
                        <div class="paginate fr"></div>
                    </div>
                </div>
                <div class="tc hide" data-index="5">
                    <ul class="game-list game-list-panel5"></ul>
                    <div class="clearfix mgt10">
                        <div class="paginate fr"></div>
                    </div>
                </div>
            </div>
        </div>
        </script>

        <script type="text/j-template" id="tpl_popbox_GamePicker_item">
        <# _.each(dataset,function(data){#>
        <li class="clearfix">
            <a href="<#= data.link #>" class="fl a_hover" target="_blank" title="<#= data.title #>"><#= data.title #></a>
            <a href="javascript:;" class="btn fr j-select">选取</a>
        </li>
        <# }) #>
        </script>
        <!-- end GamePicker -->

        <!-- start CouponListPicker -->
        <script type="text/j-template" id="tpl_popbox_CouponListPicker">
        <div id="GoodsAndGroupPicker">
            <div class="tabs clearfix">
                <a href="javascript:;" class="active tabs_a fl" data-origin="goodsandgroup" data-index="1">优惠券</a>
            </div>
            <!-- end tabs -->
            <div class="tabs-content" data-origin="goodsandgroup">
                <div class="tc" data-index="1">
                    <ul class="gagp-goodslist"></ul>
                    <div class="clearfix mgt10">
                        <div class="fl">
                            <a href="javascript:;" class="btn btn-primary j-btn-goodsuse">确定使用</a>
                        </div>
                        <div class="paginate fr"></div>
                    </div>
                </div>
            </div>
        </div>
        </script>

        <script type="text/j-template" id="tpl_popbox_CouponListPicker_graphicsitem">
            
        <table cellspacing="0" cellpadding="0" border="0" class="table table-striped" width="100%">
            <tbody>
            <tr>
                <th style="width: 168px; " >优惠券名称
                </th>
                <th >面额
                </th>
                <th >剩余张数
                </th>
                <th >使用条件
                </th>
                <th >有效期
                </th>
                <th >操作
                </th>
            </tr>
            <# _.each(dataset,function(data){#>
            <tr>
                <td>
                <a href="<#= data.link #>" target="_blank" title="<#= data.title #>">
                        <#= data.title #>            
                </a>
                </td>
                <td><#= data.Price #>&nbsp;
                </td>
                <td><#= data.Surplus #>&nbsp;
                </td>
                <td><#= data.OrderUseLimit #>&nbsp;
                </td>
                <td><#= data.Use_time #>
                </td>
                <td>
                <li data-item="<#= data.CouponId #>" style="border-bottom: 0px;">
                    <a href="javascript:;" class="btn fr j-select  mgr15">选取</a>
                </li>
                </td>
            </tr>
            <# }) #>
            </tbody>
        </table>
        </script>

        <!-- end CouponListPicker -->


        <!-- start GraphicPicker -->
        <script type="text/j-template" id="tpl_popbox_GraphicPicker">
        <div id="GoodsAndGroupPicker">
            <div class="tabs clearfix">
                <a href="javascript:;" class="active tabs_a fl" data-origin="goodsandgroup" data-index="1">商品</a>
                <a href="javascript:;" class="tabs_a fl j-tab-group" data-origin="goodsandgroup" data-index="2">商品分组</a>
            </div>
            <!-- end tabs -->
            <div class="tabs-content" data-origin="goodsandgroup">
                <div class="tc" data-index="1">
                    <ul class="gagp-goodslist"></ul>
                    <div class="clearfix mgt10">
                        <div class="fl">
                            <a href="javascript:;" class="btn btn-primary j-btn-goodsuse">确定使用</a>
                        </div>
                        <div class="paginate fr"></div>
                    </div>
                </div>
                <div class="tc hide" data-index="2">
                    <ul class="gagp-grouplist"></ul>
                    <div class="clearfix mgt10">
                        <div class="paginate fr"></div>
                    </div>
                </div>
            </div>
        </div>
        </script>

        <script type="text/j-template" id="tpl_popbox_GraphicPicker_graphicsitem">
        <# _.each(dataset,function(data){#>
        <li class="clearfix" data-item="<#= data.item_id #>">
            <a href="<#= data.link #>" class="fl" target="_blank" title="<#= data.title #>">
                <div class="table-item-img">
                    <# if(data.is_compress){ #>
                    <img src="<#= data.pic #>80x80" alt="<#= data.title #>">
                    <# }else{ #>
                    <img src="<#= data.pic #>" alt="<#= data.title #>">
                    <# } #>
                </div>
                <div class="table-item-info">
                    <p><#= data.title #></p>
                  
                </div>
            </a>
            <a href="javascript:;" class="btn fr j-select mgt15 mgr15">选取</a>
        </li>
        <# }) #>
        </script>

        <script type="text/j-template" id="tpl_popbox_GraphicPicker_groupitem">
        <# _.each(dataset,function(data){#>
        <li class="clearfix" data-group="<#= data.group_id #>">
            <a href="<#= data.link #>" class="fl a_hover" target="_blank" title="<#= data.title #>"><#= data.title #></a>
            <a href="javascript:;" class="btn fr j-select">选取</a>
        </li>
        <# }) #>
        </script>
        <!-- end GraphicPicker -->
        <!-- start PointExchangePicker -->
        <script type="text/j-template" id="tpl_popbox_PointExchangePicker">
        <div id="GamePicker">
         
            <div class="tabs-content" data-origin="GamePicker">
                <div class="tc" data-index="1">
                    <ul class="game-list game-list-panel1"></ul>
                    <div class="clearfix mgt10">
                        <div class="paginate fr"></div>
                    </div>
                </div>
                <div class="tc hide" data-index="2">
                    <ul class="game-list game-list-panel2"></ul>
                    <div class="clearfix mgt10">
                        <div class="paginate fr"></div>
                    </div>
                </div>
                <div class="tc hide" data-index="3">
                    <ul class="game-list game-list-panel3"></ul>
                    <div class="clearfix mgt10">
                        <div class="paginate fr"></div>
                    </div>
                </div>
                <div class="tc hide" data-index="4">
                    <ul class="game-list game-list-panel4"></ul>
                    <div class="clearfix mgt10">
                        <div class="paginate fr"></div>
                    </div>
                </div>
            </div>
        </div>
        </script>

        <script type="text/j-template" id="tpl_popbox_PointExchangePicker_item">
        <# _.each(dataset,function(data){#>
        <li class="clearfix">
            <a href="<#= data.link #>" class="fl a_hover" target="_blank" title="<#= data.title #>"><#= data.title #></a>
            <a href="javascript:;" class="btn fr j-select">选取</a>
        </li>
        <# }) #>
        </script>
        <!-- end PointExchangePicker -->

        <!-- start VotePicker -->
        <script type="text/j-template" id="tpl_popbox_VotePicker">
        <div id="GamePicker">
         
            <div class="tabs-content" data-origin="GamePicker">
                <div class="tc" data-index="1">
                    <ul class="game-list game-list-panel1"></ul>
                    <div class="clearfix mgt10">
                        <div class="paginate fr"></div>
                    </div>
                </div>
                <div class="tc hide" data-index="2">
                    <ul class="game-list game-list-panel2"></ul>
                    <div class="clearfix mgt10">
                        <div class="paginate fr"></div>
                    </div>
                </div>
                <div class="tc hide" data-index="3">
                    <ul class="game-list game-list-panel3"></ul>
                    <div class="clearfix mgt10">
                        <div class="paginate fr"></div>
                    </div>
                </div>
                <div class="tc hide" data-index="4">
                    <ul class="game-list game-list-panel4"></ul>
                    <div class="clearfix mgt10">
                        <div class="paginate fr"></div>
                    </div>
                </div>
            </div>
        </div>
        </script>

        <script type="text/j-template" id="tpl_popbox_VotePicker_item">
        <# _.each(dataset,function(data){#>
        <li class="clearfix">
            <a href="<#= data.link #>" class="fl a_hover" target="_blank" title="<#= data.title #>"><#= data.title #></a>
            <a href="javascript:;" class="btn fr j-select">选取</a>
        </li>
        <# }) #>
        </script>
        <!-- end VotePicker -->

        <!-- start CategoriesPicker -->
        <script type="text/j-template" id="tpl_popbox_CategoriesPicker">
        <div id="GoodsAndGroupPicker">
            <div class="tabs clearfix">
                <a href="javascript:;" class="active tabs_a fl" data-origin="goodsandgroup" data-index="1">商品</a>
                <a href="javascript:;" class="tabs_a fl j-tab-group" data-origin="goodsandgroup" data-index="2">商品分组</a>
            </div>
            <!-- end tabs -->
            <div class="tabs-content" data-origin="goodsandgroup">
                <div class="tc" data-index="1">
                    <ul class="gagp-goodslist"></ul>
                    <div class="clearfix mgt10">
                        <div class="fl">
                            <a href="javascript:;" class="btn btn-primary j-btn-goodsuse">确定使用</a>
                        </div>
                        <div class="paginate fr"></div>
                    </div>
                </div>
                <div class="tc hide" data-index="2">
                    <ul class="gagp-grouplist"></ul>
                    <div class="clearfix mgt10">
                        <div class="paginate fr"></div>
                    </div>
                </div>
            </div>
        </div>
        </script>

        <script type="text/j-template" id="tpl_popbox_CategoriesPicker_graphicsitem">
        <# _.each(dataset,function(data){#>
        <li class="clearfix" data-item="<#= data.item_id #>">
            <a href="<#= data.link #>" class="fl" target="_blank" onclick="return false" title="<#= data.title #>">
         
                <div class="table-item-info">
                    <p style="line-height:36px;">
                        <# if(data.depth==1){ #>
                            <strong>
                        <# }#>
                       <# if(data.search==0){ #>
                            <# for(var i=1;i<data.depth;i++){ #>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <# } #> 
                        <# } #>  
                        <#= data.title #>
                        <# if(data.depth==1){ #>
                            </strong>
                        <# } #>  
                    </p>                  
                </div>
            </a>
            <a href="javascript:;" class="btn fr j-select  mgr15" style="margin-top:5px;">选取</a>
        </li>
        <# }) #>
        </script>

        <script type="text/j-template" id="tpl_popbox_CategoriesPicker_groupitem">
        <# _.each(dataset,function(data){#>
        <li class="clearfix" data-group="<#= data.group_id #>">
            <a href="<#= data.link #>" class="fl a_hover" target="_blank" title="<#= data.title #>"><#= data.title #></a>
            <a href="javascript:;" class="btn fr j-select">选取</a>
        </li>
        <# }) #>
        </script>
        <!-- end CategoriesPicker -->

        <!-- start BrandsPicker -->
        <script type="text/j-template" id="tpl_popbox_BrandsPicker">
        <div id="GoodsAndGroupPicker">
            <div class="tabs clearfix">
                <a href="javascript:;" class="active tabs_a fl" data-origin="goodsandgroup" data-index="1">商品</a>
                <a href="javascript:;" class="tabs_a fl j-tab-group" data-origin="goodsandgroup" data-index="2">商品分组</a>
            </div>
            <!-- end tabs -->
            <div class="tabs-content" data-origin="goodsandgroup">
                <div class="tc" data-index="1">
                    <ul class="gagp-goodslist"></ul>
                    <div class="clearfix mgt10">
                        <div class="fl">
                            <a href="javascript:;" class="btn btn-primary j-btn-goodsuse">确定使用</a>
                        </div>
                        <div class="paginate fr"></div>
                    </div>
                </div>
                <div class="tc hide" data-index="2">
                    <ul class="gagp-grouplist"></ul>
                    <div class="clearfix mgt10">
                        <div class="paginate fr"></div>
                    </div>
                </div>
            </div>
        </div>
        </script>

        <script type="text/j-template" id="tpl_popbox_BrandsPicker_graphicsitem">
        <# _.each(dataset,function(data){#>
        <li class="clearfix" data-item="<#= data.item_id #>">
            <a href="<#= data.link #>" class="fl" target="_blank" title="<#= data.title #>">
             <div class="table-item-img">
                    <# if(data.is_compress){ #>
                    <img src="<#= data.pic #>80x80" alt="<#= data.title #>">
                    <# }else{ #>
                    <img src="<#= data.pic #>" alt="<#= data.title #>">
                    <# } #>
                </div>
                <div class="table-item-info">
                    <p><#= data.title #></p>
                  
                </div>
            </a>
            <a href="javascript:;" class="btn fr j-select mgt15 mgr15">选取</a>
        </li>
        <# }) #>
        </script>

        <script type="text/j-template" id="tpl_popbox_BrandsPicker_groupitem">
        <# _.each(dataset,function(data){#>
        <li class="clearfix" data-group="<#= data.group_id #>">
            <a href="<#= data.link #>" class="fl a_hover" target="_blank" title="<#= data.title #>"><#= data.title #></a>
            <a href="javascript:;" class="btn fr j-select">选取</a>
        </li>
        <# }) #>
        </script>
        <!-- end BrandsPicker -->
        <script type="text/j-template" id="tpl_popbox_ArticlePicker">
        <div id="GoodsAndGroupPicker">
            <div class="tabs clearfix">
                <a href="javascript:;" class="active tabs_a fl" data-origin="goodsandgroup" data-index="1">商品</a>
                <a href="javascript:;" class="tabs_a fl j-tab-group" data-origin="goodsandgroup" data-index="2">商品分组</a>
            </div>
            <!-- end tabs -->
            <div class="tabs-content" data-origin="goodsandgroup">
                <div class="tc" data-index="1">
                    <ul class="gagp-goodslist"></ul>
                    <div class="clearfix mgt10">
                        <div class="fl">
                            <a href="javascript:;" class="btn btn-primary j-btn-goodsuse">确定使用</a>
                        </div>
                        <div class="paginate fr"></div>
                    </div>
                </div>
                <div class="tc hide" data-index="2">
                    <ul class="gagp-grouplist"></ul>
                    <div class="clearfix mgt10">
                        <div class="paginate fr"></div>
                    </div>
                </div>
            </div>
        </div>
        </script>

        <script type="text/j-template" id="tpl_popbox_ArticlePicker_graphicsitem">
        <# _.each(dataset,function(data){#>
        <li class="clearfix" data-item="<#= data.item_id #>">
            <a href="<#= data.link #>" class="fl" target="_blank" title="<#= data.title #>">
           <%--  <div class="table-item-img">
                    <# if(data.is_compress){ #>
                    <img src="<#= data.pic #>80x80" alt="<#= data.item_title #>">
                    <# }else{ #>
                    <img src="<#= data.pic #>" alt="<#= data.item_title #>">
                    <# } #>
                </div>--%>
                <div class="table-item-info">
                    <p style="line-height:36px;"><#= data.title #></p>
                  
                </div>
            </a>
            <a href="javascript:;" class="btn fr j-select  mgr15">选取</a>
        </li>
        <# }) #>
        </script>



        <uc1:ImageListView ID="ImageListView" runat="server" />

        <!-- end tpl_albums_imgs -->

        <script type="text/j-template" id="icon_imgPicker">
        <div id="icon-container">
            <div class="albums-title clearfix">
                <span class="fl">选择图片</span>
                <a href="javascript:;" class="fr" id="Jclose" title="关闭">
                    
                </a>
            </div>
            <div class="albums-container">
                <div class="albums-cr-actions noborder">
                    <a href="javascript:;" data-style="style1" class="btn btn-primary mgl10 cur">风格一<i></i></a>
                    <a href="javascript:;" data-style="style2" class="btn btn-primary mgl10">风格二<i></i></a>
                    <a href="javascript:;" data-style="style3" class="btn btn-primary mgl10">风格三<i></i></a>
                </div>
                <div class="albums-color-tab">
                    <h2><a href="javascript:;" class="btn btn-primary mgl10">选择颜色</a><span>(小图标下面的文字仅供参考,背景色可自行修改)</span></h2>
                    <ul class="clearfix">
                        <li data-color="color0"><span class="color color0"></span><span>黑色</span></li>
                        <li data-color="color1"><span class="color color1"></span><span>白色</span></li>
                        <li data-color="color2"><span class="color color2"></span><span>灰色</span></li>
                        <li data-color="color3"><span class="color color3"></span><span>红色</span></li>
                        <li data-color="color4"><span class="color color4"></span><span>黄色</span></li>
                        <li data-color="color5"><span class="color color6"></span><span>蓝色</span></li>
                        <li data-color="color6"><span class="color color5"></span><span>绿色</span></li>
                        <li data-color="color7"><span class="color color7"></span><span>紫色</span></li>
                        <li data-color="color8"><span class="color color8"></span><span>橙色</span></li>
                    </ul>
                </div>
                <div class="albums-icon-tab"></div>
                <div class="albums-cr-ctrls clearfix">
                    <a href="javascript:;" id="j-useIcon" class="btn btn-primary fl">确定</a>
                </div>
            </div>
        </div>
        </script>
        <script type="text/j-template" id="icon_imglist">
        <ul class="clearfix">
            <# _.each(data,function(item){ #>
            <li><img src="<#= item #>" width="80" alt=""><span><i></i></span></li>
            <# }) #>
        </ul>
        </script>
        <!--图文素材弹窗选择器 -->
        <!-- start 本文图文 -->
        <script type="text/j-template" id="tpl_materialPicker_text_pre">
        <dl class="materialPrePanel mgt20">
            <dt>
                <div class="single-summary pd10"><#= summary #></div>
            </dt>
        </dl>
        </script>
        <!-- end 本文图文 -->
        <!-- start 单条图文选择器 -->
        <script type="text/j-template" id="tpl_materialPicker_single_table">
        <div style="text-align:right;"><a href="/MaterialOne/add" class="btn btn-success btn-small" target="_blank">添加单条图文</a></div>
        <table class="wxtables mgt15" style="width:650px;">
            <thead>
                <tr>
                    <td>标题</td>
                    <td>创建时间</td>
                    <td>操作</td>
                </tr>
            </thead>
            <tbody>
                <# if(list.length){ #>
                <# _.each(list,function(item){ #>
                <tr>
                    <td>
                        <div class="ng ng_single">
                            <div class="ng_item">
                                <div class="td_cont with_label">
                                    <span class="label label-success">图文</span>
                                    <div class="text">
                                        <a href="<#= item.link #>" target="_blank" class="part new_window" title="<#= item.title #>"><#= item.title #></a>
                                    </div>
                                </div>
                            </div>
                            <div class="ng_item view_more">
                                <a href="<#= item.link #>" class="td_cont clearfix new_window">
                                    <span class="pull-left">阅读全文</span>
                                    <span class="pull-right">&gt;</span>
                                </a>
                            </div>
                        </div>
                    </td>
                    <td><#= item.datetime #></td>
                    <td><a href="javascript:;" class="btn btn-small btn-primary j-select">选择</a></td>
                </tr>
                <# }) #>
                <# }else{ #>
                <tr><td colspan="4" class="txtCenter">暂无数据</td></tr>
                <# } #>
            </tbody>
        </table>

        <div class="clearfix mgt15">
            <div class="paginate fr"><#= page #></div>
        </div>
        </script>

        <script type="text/j-template" id="tpl_materialPicker_single_pre">
        <dl class="materialPrePanel mgt20" style="border: 1px solid #E7E7EB;">
            <dt>
                <h1 class="single-title first-t"><#= title #></h1>
                <p class="single-datetime first-d"><#= datetime #></p>
                <div class="cover-wrap">
                    <img src="<#= coverimg #>">
                </div>
                <p class="single-summary first-p"><#= summary #></p>
                <a href="<#= link #>" target="_blank" class="single-link clearfix first-a">
                    <span class="fl">阅读全文</span>
                    <span class="fr symbol">&gt;</span>
                </a>
            </dt>
        </dl>
        </script>
        <!-- end 单条图文选择器 -->
        <!-- start 多条图文选择器 -->
        <script type="text/j-template" id="tpl_materialPicker_mutil_table">
        <div style="text-align:right;"><a href="/MaterialMore/add" class="btn btn-success btn-small" target="_blank">添加多条图文</a></div>
        <table class="wxtables mgt15" style="width:650px;">
            <thead>
                <tr>
                    <td>标题</td>
                    <td>创建时间</td>
                    <td>操作</td>
                </tr>
            </thead>
            <tbody>
                <# if(list.length){ #>
                <# _.each(list,function(item){ #>
                <tr>
                    <td>
                        <div class="ng ng_multiple">
                            <div class="ng_item">
                                <div class="td_cont with_label">
                                    <span class="label label-success">图文1</span>
                                    <div class="text">
                                        <a href="<#= item.link #>" target="_blank" class="part new_window" title="<#= item.title #>"><#= item.title #></a>
                                    </div>
                                </div>
                            </div>
                            <# _.each(item.dataset,function(subitem){ #>
                            <div class="ng_item">
                                <div class="td_cont with_label">
                                    <span class="label label-success">图文2</span>
                                    <div class="text">
                                        <a href="<#= subitem.link #>" target="_blank" class="part new_window" title="<#= subitem.title #>"><#= subitem.title #></a>
                                    </div>
                                </div>
                            </div>
                            <# }) #>
                        </div>
                    </td>
                    <td><#= item.datetime #></td>
                    <td><a href="javascript:;" class="btn btn-small btn-primary j-select">选择</a></td>
                </tr>
                <# }) #>
                <# }else{ #>
                <tr><td colspan="4" class="txtCenter">暂无数据</td></tr>
                <# } #>
            </tbody>
        </table>

        <div class="clearfix mgt15">
            <div class="paginate fr"><#= page #></div>
        </div>
        </script>

        <script type="text/j-template" id="tpl_materialPicker_mutil_pre">
        <dl class="materialPrePanel mgt20 bgcfff border">
            <dt class="mb10 mt10">
                <a href="<#= redirect #>" target="_blank">
                    <div class="cover-wrap">
                        <img src="<#= coverimg #>" class="img-cover">
                    </div>
                    <h2 class="w262"><#= title #></h2>
                </a>
            </dt>
            <# _.each(dataset,function(item){ #>
            <dd class="newWidth">
                <a class="border-top_1 p" href="<#= item.link #>" target="_blank">
                    <h3><#= item.title #></h3>
                    <div class="pic"><img src="<#= item.img #>" alt=""></div>
                </a>
            </dd>
            <# }) #>
        </dl>
        </script>
        <!-- end 多条图文选择器 -->
        <!-- 自定义菜单 营销活动选项卡 -->
        <script type="text/j-template" id="tpl_menu_tab">
        <# _.each(list,function(item){ #>
        <li class="clearfix">
            <a href="<#= item.urlview#><#=item.link#>" class="fl a_hover" target="_blank" title="<#= item.title #>"><#= item.title #></a>
            <a href="javascript:;" data-link_id="<#= item.link_id#>" class="btn fr j-select">选取</a>
        </li>
        <# }) #>



        </script>


        <script type="text/j-template" id="tpl_menu_ump">
        <div id="GamePicker">
            <div class="tabs clearfix">
                <#for (var i in gamelist){#>
                <a href="javascript:void(0)" class="tabs_a j-tab-game fl " title="<#= gamelist[i] #>" data-keys="<#=i#>"><#= gamelist[i] #></a>
                <# } #>
            </div>
            <div class="tabs-content" data-origin="GamePicker">
                <div class="tc" data-index="1">
                    <ul class="game-list game-list-panel1"></ul>
                    <div class="clearfix mgt10">
                        <div class="paginate fr"><#= page #></div>
                    </div>
                </div>
            </div>
        </div>

        </script>
        <!-- 自定义菜单 活动页面 -->
        <script type="text/j-template" id="tpl_menu_ump">
        //
        <div>
            //      <table class="wxtables mgt15">
                //
                <thead>
                    //
                    <tr>
                        //
                        <td>标题</td>
                        //
                        <td width="60">操作</td>
                        //
                    </tr>
                    //
                </thead>
                //
                <tbody>
                    //              <# _.each(list,function(item){ #>
                    //
                    <tr>
                        //
                        <td><#= item.title #><input data-link_id="<#= item.link_id#>" type="hidden" value="<#= item.urlview#><#=item.link#>"></td>
                        //
                        <td><input type="button" class="btn btn-primary j-select-link" name="" value="选择"></td>
                        //
                    </tr>
                    //              <# }) #>
                    //
                </tbody>
                //
            </table>
            //      <div class="clearfix mgt15">
                //             <div class="paginate fr"><#= page #></div>
                //
            </div>
            //
        </div>
        </script>

        <!-- 自定义菜单 选择商品 -->
        <script type="text/j-template" id="tpl_menu_detail">

        <div id="GoodsAndGroupPicker">
            <ul class="gagp-goodslist">
                <# _.each(list,function(data){#>
                <li class="clearfix">
                    <a href="<#= data.link #><#= data.urlview #>" class="fl" target="_blank" title="<#= data.title #>">
                        <div class="table-item-img">
                            <img src="<#= data.file_path #>" alt="<#= data.title #>">
                        </div>
                        <div class="table-item-info">
                            <p><#= data.title #></p>
                            <span class="price">&yen;<#= data.price #></span>
                        </div>
                    </a>
                    <a href="javascript:;" data-link_id="<#= data.link_id#>" class="btn fr j-select mgt10">选取</a>
                </li>
                <# }) #>
            </ul>
            <div class="clearfix mgt15">
                <div class="paginate fr"><#= page #></div>
            </div>
        </div>
        </script>
        <script type="text/j-template" id="tpl_menu_page">
        <div class="clearfix mgt10">
            <div class="paginate fr"><#= page #></div>
        </div>
        </script>
        <!-- 选择自定义链接 -->
        <script type="text/j-template" id="tpl_menu_lst">
        <div id="GoodsAndGroupPicker">
            <ul class="gagp-goodslist">
                <# _.each(list,function(data){#>
                <li class="clearfix">
                    <a href="<#= data.link #><#= data.urlview #>" class="fl a_hover lh30" target="_blank" title="<#= data.title #>"><#= data.title #></a>
                    <a href="javascript:;" data-link_id="<#= data.link_id#>" class="btn fr j-select">选取</a>
                </li>
                <# }) #>
            </ul>
            <div class="clearfix mgt15">
                <div class="paginate fr"><#= page #></div>
            </div>
        </div>

        </script>

        <!-- 自定义菜单 商品分组 -->
        <script type="text/j-template" id="tpl_menu_group">
        <div id="GoodsAndGroupPicker">
            <div class="tabs-content" data-origin="goodsandgroup">
                <div class="tc" data-index="1">
                    <ul class="gagp-goodslist">
                        <# _.each(list,function(data){#>
                        <li class="clearfix">
                            <a href="<#= data.link #>" class="fl a_hover" target="_blank" title="<#= data.title #>"><#= data.title #></a>
                            <a href="javascript:;" class="btn fr j-select">选取</a>
                        </li>
                        <# }) #>
                    </ul>
                </div>
            </div>
        </div>
        <div class="clearfix mgt15">
            <div class="paginate fr"><#= page #></div>
        </div>
        </script>

        <!-- 自定义菜单 专题页面 -->
        <script type="text/j-template" id="tpl_menu_magazine">


        <div id="GoodsAndGroupPicker">
            <div class="tabs-content" data-origin="goodsandgroup">
                <div class="tc" data-index="1">
                    <ul class="gagp-goodslist">
                        <# _.each(list,function(data){#>
                        <li class="clearfix">
                            <a href="<#= data.link #>" class="fl a_hover" target="_blank" title="<#= data.title #>"><#= data.title #></a>
                            <a href="javascript:;" class="btn fr j-select">选取</a>
                        </li>
                        <# }) #>
                    </ul>
                </div>
            </div>
            <div class="clearfix mgt15">
                <div class="paginate fr"><#= page #></div>
            </div>
        </div>

        </script>

        <!-- 自定义菜单 专题分类 -->
        <script type="text/j-template" id="tpl_menu_sort">

        <div id="GoodsAndGroupPicker">
            <div class="tabs-content" data-origin="goodsandgroup">
                <div class="tc" data-index="1">
                    <ul class="gagp-goodslist">
                        <# _.each(list,function(data){#>
                        <li class="clearfix">
                            <a href="<#= data.link #>" class="fl a_hover" target="_blank" title="<#= data.title #>"><#= data.title #></a>
                            <a href="javascript:;" class="btn fr j-select">选取</a>
                        </li>
                        <# }) #>
                    </ul>
                </div>
            </div>
            <div class="clearfix mgt15">
                <div class="paginate fr"><#= page #></div>
            </div>
        </div>

        </script>

        <!-- start ImgPicker -->
        <script type="text/j-template" id="tpl_popbox_ImgPicker">
        <div id="ImgPicker">
            <div class="tabs clearfix">
                <a href="javascript:;" class="active tabs_a fl" data-origin="imgpicker" data-index="1">选择图片</a>
                <a href="javascript:;" class="tabs_a fl j-initupload" data-origin="imgpicker" data-index="2">上传新图片</a>
            </div>
            <!-- end tabs-->
            <div class="tabs-content" data-origin="imgpicker">
                <div class="tc" data-index="1">
                    <ul class="img-list imgpicker-list clearfix"></ul>
                    <!-- end img-list -->
                    <div class="imgpicker-actionPanel clearfix">
                        <div class="fl">
                            <a href="javascript:;" class="btn btn-primary" id="j-btn-listuse">使用选中图片</a>
                        </div>
                        <div class="fr">
                            <div class="paginate"></div>
                        </div>
                    </div>
                    <!-- end imgpicker-actionPanel -->
                </div>

                <div class="tc hide" data-index="2">
                    <div class="uploadifyPanel clearfix">
                        <ul class="img-list imgpicker-upload-preview"></ul>
                        <input type="file" name="imgpicker_upload_input" id="imgpicker_upload_input">
                    </div>

                    <div class="imgpicker-actionPanel">
                        <a href="javascript:;" class="btn btn-primary" id="j-btn-uploaduse">使用上传的图片</a>
                    </div>
                    <!-- end imgpicker-actionPanel -->
                </div>
            </div>
            <!-- end tabs-content -->
        </div>
        </script>

        <script type="text/j-template" id="tpl_popbox_ImgPicker_listItem">
        <# _.each(dataset,function(url){ #>
        <li>
            <span class="img-list-overlay"><i class="img-list-overlay-check"></i></span>
            <img src="<#= url #>">
        </li>
        <# }) #>
        </script>

        <script type="text/j-template" id="tpl_popbox_ImgPicker_uploadPrvItem">
        <li>
            <span class="img-list-btndel j-imgpicker-upload-btndel"><i class="gicon-trash white"></i></span>
            <span class="img-list-overlay"></span>
            <img src="<#= url #>">
        </li>
        </script>
        <!-- 自定义菜单中的单张图片 -->
        <script type="text/j-template" id="tpl_popbox_ImgPicker_listItem2">
        <# _.each(dataset,function(item){ #>
        <li>
            <span class="img-list-overlay"><i class="img-list-overlay-check"></i></span>
            <img src="<#= item.file_path #>" data-id="<#=item.file_id#>">
        </li>
        <# }) #>
        </script>
        <script type="text/j-template" id="tpl_popbox_ImgPicker_uploadPrvItem2">
        <li>
            <span class="img-list-btndel j-imgpicker-upload-btndel"><i class="gicon-trash white"></i></span>
            <span class="img-list-overlay"></span>
            <img src="<#= url #>" data-id="<#=id#>">
        </li>
        </script>
        <!-- end ImgPicker-->
        <!-- start audio -->
        <script type="text/j-template" id="tpl_popbox_Audio">
        <div id="ImgPicker">
            <div class="tabs clearfix">
                <a href="javascript:;" class="active tabs_a fl" data-origin="imgpicker" data-index="1">选择音频</a>
                <a href="javascript:;" class="tabs_a fl j-initupload" data-origin="imgpicker" data-index="2">上传新音频</a>
            </div>
            <!-- end tabs-->
            <div class="tabs-content" data-origin="imgpicker">
                <div class="tc" data-index="1">
                    <ul class="img-list imgpicker-list clearfix"></ul>
                    <!-- end img-list -->
                    <div class="imgpicker-actionPanel clearfix">
                        <div class="fl">
                            <a href="javascript:;" class="btn btn-primary" id="j-btn-listuse">使用选中音频</a>
                            <a href="javascript:;" class="btn btn-default" id="j-btn-listdel">删除选中音频</a>
                        </div>
                        <div class="fr">
                            <div class="paginate"></div>
                        </div>
                    </div>
                    <!-- end imgpicker-actionPanel -->
                </div>

                <div class="tc hide" data-index="2">
                    <div class="uploadifyPanel clearfix">
                        <ul class="img-list imgpicker-upload-preview"></ul>
                        <input type="file" name="imgpicker_upload_input" id="imgpicker_upload_input">
                    </div>

                    <div class="imgpicker-actionPanel">
                        <a href="javascript:;" class="btn btn-primary" id="j-btn-uploaduse">使用上传的音频</a>
                    </div>
                    <!-- end imgpicker-actionPanel -->
                </div>
            </div>
            <!-- end tabs-content -->
        </div>
        </script>
        <!-- 自定义菜单中的音频 -->
        <script type="text/j-template" id="tpl_popbox_ImgPicker_audio">
        <# _.each(dataset,function(item){ #>
        <li>
            <span class="img-list-overlay"><i class="img-list-overlay-check"></i></span>
            <div class="audio-flag" data-src="<#= item.file_path #>" data-id="<#=item.file_id#>"><i></i></div>
            <div class="audio-name">
                <b class="j-curname"><#= item.file_name #></b>
                <div class="j-edit-name">
                    <input type="text" name="audioName" value="<#= item.file_name #>">
                    <a href="javascript:;" class="btn btn-primary j-getAudioName" data-id="<#=item.file_id#>" title="确定保存">确定</a>
                </div>
                <p class="j-get-edit-name"><i class="gicon-pencil edit-img-name"></i></p>
            </div>
        </li>
        <# }) #>
        </script>
        <script type="text/j-template" id="tpl_popbox_ImgPicker_audio2">
        <li>
            <span class="img-list-btndel j-imgpicker-upload-btndel"><i class="gicon-trash white"></i></span>
            <span class="img-list-overlay"></span>
            <div data-src="<#= url #>" data-id="<#=id#>" width="64" height="64"><i></i></div>
        </li>
        </script>
        <!-- start video -->
        <script type="text/j-template" id="tpl_popbox_Video">
        <div id="ImgPicker">
            <div class="tabs clearfix">
                <a href="javascript:;" class="active tabs_a fl" data-origin="videolst" data-index="1">选择视频</a>
                <a href="javascript:;" class="tabs_a fl j-initupload" data-origin="video" data-index="2">上传新视频</a>
            </div>
            <!-- end tabs-->
            <div class="tabs-content" data-origin="video">
                <div class="tc" data-index="1">
                    <ul class="img-list imgpicker-list clearfix"></ul>
                    <!-- end img-list -->
                    <div class="imgpicker-actionPanel clearfix">
                        <div class="fl">
                            <a href="javascript:;" class="btn btn-primary" id="j-btn-listuse">使用选中视频</a>
                        </div>
                        <div class="fr">
                            <div class="paginate"></div>
                        </div>
                    </div>
                    <!-- end imgpicker-actionPanel -->
                </div>

                <div class="tc hide" data-index="2">
                    <div class="uploadifyPanel clearfix">
                        <ul class="img-list imgpicker-upload-preview"></ul>
                        <input type="file" name="imgpicker_upload_input" id="imgpicker_upload_input">
                    </div>
                    <div class="imgpicker-actionPanel">
                        <a href="javascript:;" class="btn btn-primary" id="j-btn-uploaduse">使用上传的视频</a>
                    </div>
                    <!-- end imgpicker-actionPanel -->
                </div>
            </div>
            <!-- end tabs-content -->
        </div>
        </script>
        <!-- 自定义菜单中的视频 -->
        <script type="text/j-template" id="tpl_popbox_ImgPicker_video">
        <# _.each(dataset,function(item){ #>
        <li>
            <span class="img-list-overlay"><i class="img-list-overlay-check"></i></span>
            <div class="video" data-src="<#= item.file_path #>" data-id="<#=item.file_id#>" width="64" height="64"><i></i></div>
        </li>
        <# }) #>
        </script>
        <script type="text/j-template" id="tpl_popbox_ImgPicker_video2">
        <li>
            <span class="img-list-btndel j-imgpicker-upload-btndel"><i class="gicon-trash white"></i></span>
            <span class="img-list-overlay"></span>
            <div class="video" data-src="<#= url #>" data-id="<#= id #>" width="64" height="64"><i></i></div>
        </li>
        </script>
        <!--商品专题-->
        <script type="text/j-template" id="tpl_popbox_TopicsPicker">
        <div id="GoodsAndGroupPicker">
            <div class="tabs-content" data-origin="goodsandgroup">
                <div class="tc" data-index="1">
                    <ul class="gagp-goodslist"></ul>
                    <div class="clearfix mgt10">
                        <div class="fl">
                            <a href="javascript:;" class="btn btn-primary j-btn-goodsuse">确定使用</a>
                        </div>
                        <div class="paginate fr"></div>
                    </div>
                </div>
                <div class="tc hide" data-index="2">
                    <ul class="gagp-grouplist"></ul>
                    <div class="clearfix mgt10">
                        <div class="paginate fr"></div>
                    </div>
                </div>
            </div>
        </div>
        </script>

        <script type="text/j-template" id="tpl_popbox_TopicsPicker_graphicsitem">
        <# _.each(dataset,function(data){#>
        <li class="clearfix" data-item="<#= data.topic_id #>">
            <a href="<#= data.link #>" class="fl" target="_blank" title="<#= data.title #>">
         
                <div class="table-item-info">
                    <p><#= data.title #></p>
                  
                </div>
            </a>
            <a href="javascript:;" class="btn fr j-select mgt15 mgr15">选取</a>
        </li>
        <# }) #>
        </script>
        <!--商品专题结束-->
        <asp:Literal ID="La_script" runat="server"></asp:Literal>


        <!-- Header_style1 tpl end-->
        <!--end front template  -->

        <script src="/Admin/shop/Public/js/dist/lib-min.js"></script>
        <script src="/Admin/shop/Public/plugins/jbox/jquery.jbox-min.js"></script>
        <script src="/Admin/shop/Public/plugins/zclip/jquery.zclip-min.js"></script>
        <!-- 线上环境 -->
        <script src="/Admin/shop/Public/js/dist/component-xcx.js"></script>
        <script src="/Admin/shop/Public/modulesJs/scroll.js"></script>
        <!--[if lt IE 10]>
    <script src="/Public/js/jquery/jquery.placeholder-min.js"></script>
    <script>
        $(function(){
            //修复IE下的placeholder
            $('.input,.textarea').placeholder();
        });
    </script>
    <![endif]-->
        <!-- diy js start-->
        <script src="/Utility/Ueditor/plugins/ueditor/ueditor.config.js"></script>
        <script src="/Utility/Ueditor/plugins/ueditor/ueditor.all.min.js?v=3.0"></script>
        <script src="/Utility/Ueditor/plugins/ueditor/diy_imgpicker.js"></script>
        <script src="/Utility/Ueditor/plugins/uploadify/jquery.uploadify.min.js?ver=940"></script>
        <script src="/Admin/shop/Public/js/jquery-ui/jquery-ui.min.js"></script>
        <script src="/Admin/shop/Public/js/config.js"></script>
        <script src="/Admin/shop/Public/plugins/diy/diy.core.js"></script>
        <script src="/Admin/shop/Public/plugins/diy/diy.base64code.js"></script>
        <script src="/Admin/shop/Public/plugins/diy/diy.data2html.js"></script>
        <script src="/Admin/shop/Public/plugins/diy/diy.events-app.js?v=3.35"></script>
        <script src="/Admin/shop/Public/plugins/diy/diy.verify.js"></script>
        <script src="/Admin/shop/Public/plugins/diy/diy.init.js"></script>
        <script src="/Admin/shop/Public/plugins/colorpicker/colorpicker.js"></script>
        <script src="/admin/js/bootstrap.min.js"></script>
        <script src="/Admin/shop/Public/plugins/colorpicker/colorpicker.js"></script>
        <script src="<%=scriptSrc %>"></script>
        <script src="/Admin/shop/Public/js/dist/home/Shop/edit_homepage.js"></script>

        <script>
            $(document).ready(function () {
                $('.container').css('padding', 0);
                $('body').css("background", '#fff');
                if ($('#j-initdata').val() == 0) {
                    $('#j-savePage').click();
                };
                // 控制添加商品的图片显示高度，确保商品布局正常
                $('.b_mingoods,.mingoods').each(function (index, el) {
                    var me = $(this),
                        imgHeight = me.find('img').width();
                    me.find('img').closest('a').height(imgHeight);
                });
                $('.board3').each(function (index, el) {
                    var me = $(this);
                    var bwidth = me.width();
                    if (me.hasClass('small_board') || !me.hasClass('big_board')) {
                        me.children('span').attr('style', 'height:' + bwidth + 'px !important;overflow:hidden;');
                    }
                    if (me.hasClass('big_board')) {
                        me.children('span').attr('style', 'height:' + (bwidth * 2 + 10) + 'px !important;overflow:hidden;');
                    }
                });

                //添加模块         
                $('.tab').each(function () {
                    var hoverimg = $("<img id='hoverimg' src='../images/two-columns-text.png'/ style='display:none'>");
                    $(this).children('a').append(hoverimg);
                    $(this).children('a').mouseover(function () {
                        $(this).children('a img').show();
                    }).mouseout(function () {
                        $(this).children('img').hide();
                    });
                });
            });
        </script>

        <script type="text/javascript">
            var pageID = "#<%=j_pageID.ClientID  %>";
            $(function () {
                $(".j-copy").zclip({
                    path: '/Public/plugins/zclip/ZeroClipboard.swf',
                    copy: function () {
                        return $(this).data('copy');
                    },
                    afterCopy: function () {
                        HiShop.hint("success", "内容已成功复制到您的剪贴板中");
                    }
                });
                $(".btn-notice").click(function () {
                    // $.post('/System/readAllNotice',{},function(){
                    //     window.location.reload();
                    // })
                    $.ajax({
                        url: '/System/readAllNotice',
                        type: 'POST',
                        success: function (data) {
                            if (data.status == 1) {
                                window.location.reload();
                            } else {
                                HiShop.hint("danger", data.msg);
                            }

                        }
                    })
                });


                (function () {
                    // 首页竖线到底
                    var height1 = $(".content-right").height();
                    var height2 = $(".content-left").height();
                    if (parseInt(height1) < parseInt(height2)) {
                        $(".content-right").css({ 'min-height': height2 });
                    };

                })();

            });
        </script>
        <!-- end session hint -->
        <script>
            $(function () {
                setTimeout(gggoup(), 5000);
                $('.gound_close').click(function () {
                    $('#gonggao').animate({ bottom: "-270px" }, 1000);
                });

                var viewheight=document.body.clientHeight-98;
                $(".diy-actions").height(viewheight);

            });

            function gggoup() {
                $('#gonggao').animate({ bottom: "3px" }, 1000);
            };
        </script>

        <asp:HiddenField ID="hidOpenMultStore" runat="server" ClientIDMode="Static" Value="0" />
    </form>
</body>
</html>
