<%@ Page Title="" Language="C#" MasterPageFile="~/Depot/Admin.Master" AutoEventWireup="true" CodeBehind="AppletConfig.aspx.cs" Inherits="Hidistro.UI.Web.Depot.home.AppletConfig" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <script type="text/javascript" src="/utility/jquery.hishopUpload.js"></script>
    <script type="text/javascript" src="/utility/jquery.form.js"></script>
    <style>
        #addlist tr th {
            border: 1px solid #e4e4e4 !important;
            background: #fff;
            padding: 10px 15px !important;
        }

        #addlist tr td {
            border: solid #e4e4e4 !important;
            border-width: 0 1px 1px 1px !important;
            background: #fff;
            padding: 10px 15px !important;
        }

            #addlist tr td:first-child, #addlist tr th:first-child {
                border-right: 0 !important;
            }

            #addlist tr td:last-child, #addlist tr th:last-child {
                border-left: 0 !important;
            }

        .icon_close {
            width: 16px;
            height: 16px;
            background: url(../images/icon_close.png);
            display: inline-block;
            cursor: pointer;
        }

            .icon_close:hover {
                width: 16px;
                height: 16px;
                background: url(../images/icon_close1.png);
                display: inline-block;
                cursor: pointer;
            }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a class="hover">小程序首页配置</a></li>
            </ul>
        </div>

        <div class="datafrom">
            <div class="formitem validator1 depot-p">
                <ul>
                    <li class="clearfix"><span class="formitemtitle">店铺轮播图：</span>
                        <div id="imageContainer">
                            <span name="productImages" class="imgbox"></span>
                        </div>
                        <p>
                            限定上传5张图片
                        </p>
                    </li>
                    <li><span class="formitemtitle">商品推荐楼层：</span>
                        新建的商品推荐楼层将会先小程序首页显示
                    </li>
                    <li><span class="formitemtitle">&nbsp;</span>
                        <asp:LinkButton ID="lkbAddAppletFloor" runat="server" CssClass="btn btn-default float" NavigateUrl="AddAppletFloor.aspx"  OnClientClick="return getUploadImages();">新建楼层</asp:LinkButton>
                        <p></p>
                    </li>
                    <li><span class="formitemtitle">&nbsp;</span>

                        <div style="margin-left: 200px;" id="divSomeProducts">
                            <table id="addlist" style="width: 80%;" class="table table-striped bundling-table table-fixed">
                                <tr>
                                    <th width="45%">楼层名称</th>
                                    <th width="15%">关联商品</th>
                                    <th width="15%">楼层排序</th>
                                    <th width="15%">操作</th>
                                </tr>
                                <tbody id="datashow"></tbody>
                            </table>
                        </div>
                        <!--S Data Template-->
                        <script id="datatmpl" type="text/html">
                            {{each rows as item index}}<tr>
                                <td>{{item.FloorName}}</td>
                                <td>{{item.Quantity}}</td>
                                <td>
                                    <input type="text" value="{{item.DisplaySequence}}" class="setorder" floorid="{{item.FloorId}}" oldvalue="{{item.DisplaySequence}}" />
                                </td>
                                <td style="white-space: nowrap;">
                                    <span class="submit_bianji"><a href="EditAppletFloor?FloorId={{item.FloorId}}" floorid="{{item.FloorId}}" title="{{item.FloorId}}">编辑</a></span>
                                    <span class="submit_bianji"><a href="javascript:void(0)" class="delfloor" floorid="{{item.FloorId}}" title="{{item.FloorId}}">删除</a></span>
                                </td>
                            </tr>
                            {{/each}}
                        </script>
                    </li>
                </ul>
                <ul class="btntf Pa_198 clear">
                    <li>
                        <asp:Button ID="Button1" runat="server" Text="保 存" CssClass="btn btn-primary inbnt" OnClick="Button1_Click" OnClientClick="return getUploadImages();" /></li>
                </ul>
            </div>
        </div>
    </div>

    <asp:HiddenField runat="server" ID="hidSelectProducts" />
    <asp:HiddenField runat="server" ID="hidProductIds" />
    <asp:HiddenField runat="server" ID="hidAllSelectedProducts" />
    <input type="hidden" name="dataurl" id="dataurl" value="/Depot/Home/ashx/AppletConfig.ashx" />
    <asp:HiddenField ID="hidUploadImages" runat="server" />
    <asp:HiddenField ID="hidOldImages" runat="server" />
    <asp:HiddenField ID="hidSKUUploadImages" runat="server" />
    <asp:HiddenField ID="hidSKUOldImages" runat="server" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Depot/home/scripts/AppletConfig.js?v=3.33" type="text/javascript"></script>

</asp:Content>
