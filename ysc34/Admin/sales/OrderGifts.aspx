<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="OrderGifts.aspx.cs" Inherits="Hidistro.UI.Web.Admin.OrderGifts" %>


<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <style>
        html { background: #fff !important; }

        body { padding: 0; width: 100%; }

        #mainhtml { margin: 0; padding: 20px 20px 60px 20px; width: 100% !important; }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

    <div class="Goodsgifts">

        <div class="left">
            <p style="width: 100%; float: left; margin: 10px 0; line-height: 35px;">
                <input type="text" id="txtSearchText" class="forminput form-control" style="width: 160px; margin-right: 10px;" />
                <input type="button" name="btnSearch" value="查询" id="btnSearch" style="margin-left: 5px;" class="btn btn-primary" />
            </p>
            <div class="content">
                <div class="youhuiproductlist">

                    <!--S DataShow-->
                    <div class="datalist">
                        <div id="datashow"></div>
                    </div>
                    <!--E DataShow-->
                    <div class="r">

                        <!--S Pagination-->
                        <div class="pagination" id="showpager"></div>
                        <!--E Pagination-->
                    </div>


                    <!--S Data Template-->
                    <script id="datatmpl" type="text/html">
                        {{each rows as item index}}
                    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="conlisttd">

                        <tbody>
                            <tr>
                                <td width="14%" rowspan="2" class="img">{{if item.ThumbnailUrl40}}
                                    <img src="{{item.ThumbnailUrl40}}" style="border-width: 0px; width: 40px; height: 40px;">
                                    {{else}}
                                    <img src="/utility/pics/none.gif" style="border-width: 0px; width: 40px; height: 40px;">
                                    {{/if}}
                                </td>
                                <td height="32" colspan="4" class="br_none"><span class="Name" style="line-height: 32px;">{{item.Name}}</span></td>
                            </tr>
                            <tr>
                                <td width="32%" valign="top"><span class="colorC fl" style="line-height: 32px;">成本价：{{if item.CostPrice}}{{item.CostPrice.toFixed(2)}}{{else}}-{{/if}}</span></td>
                                <td width="19%" valign="top"></td>
                                <td width="14%" align="left" valign="top">
                                    <input type="text" value="1" class="in_quantity forminput form-control" style="height: 32px; width: 40px;" /></td>
                                <td width="15%" valign="top">
                                    <a class="btn btn-info" href="javascript:void(0);" onclick="Post_Add(this)" data-id="{{item.GiftId}}" style="margin-bottom: 5px;">添加</a></td>
                            </tr>
                        </tbody>
                    </table>
                        {{/each}}
                    </script>
                    <!--E Data Template-->
                </div>
            </div>
        </div>
        <div class="right">
            <p style="width: 100%; float: left; margin: 9px 0;">
                <span style="float: left; line-height: 33px; font-size: 14px; font-weight: bold;">已添加的礼品</span>
                <a class="btn btn-warning" style="float: right;" href="javascript:Post_Clear()">清空列表</a>
            </p>
            <div class="content">
                <div class="youhuiproductlist">
                    <!--S DataShow-->
                    <div class="datalist">
                        <div id="datashow2"></div>
                    </div>
                    <!--E DataShow-->
                    <div class="r">

                        <!--S Pagination-->
                        <div class="pagination" id="showpager2"></div>
                        <!--E Pagination-->
                    </div>

                    <!--S Data Template-->
                    <script id="datatmpl2" type="text/html">
                        {{each rows as item index}}
                        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="conlisttd">
                            <tbody>
                                <tr>
                                    <td width="14%" rowspan="2" class="img">{{if item.ThumbnailsUrl}}
                                    <img src="{{item.ThumbnailsUrl}}" style="border-width: 0px; width: 40px; height: 40px;">
                                        {{else}}
                                    <img src="/utility/pics/none.gif" style="border-width: 0px; width: 40px; height: 40px;">
                                        {{/if}}
                                    </td>
                                    <td height="27" colspan="4" class="br_none"><span class="Name">{{item.GiftName}}</span></td>
                                </tr>
                                <tr>
                                    <td width="27%" height="28" valign="top"><span class="colorC">成本价：{{if item.CostPrice}}{{item.CostPrice.toFixed(2)}}{{else}}0.00{{/if}}</span></td>
                                    <td width="27%" valign="top">赠送数量：{{item.Quantity}}</td>
                                    <td width="15%" align="left" valign="top">&nbsp;</td>
                                    <td width="15%" align="left" valign="top"><span class="submit_shanchu"><a href="javascript:Post_Delete({{item.GiftId}})">删除</a></span></td>
                                </tr>

                            </tbody>
                        </table>
                        {{/each}}
                    </script>
                    <!--E Data Template-->
                </div>
            </div>
        </div>
    </div>


    <input type="hidden" name="hidCurOrderId" id="hidCurOrderId" value="<%=orderId %>" />
    <input type="hidden" name="dataurl" id="dataurl" value="/Admin/sales/ashx/OrderGifts.ashx" />
    <input type="hidden" name="dataurl2" id="dataurl2" value="/Admin/sales/ashx/OrderGifts.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Admin/sales/scripts/OrderGifts.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
