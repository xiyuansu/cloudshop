<%@ Page Title="营销图库" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="MarketingImageList.aspx.cs" Inherits="Hidistro.UI.Web.Admin.depot.MarketingImageList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="Config.aspx">门店设置</a></li>
                <li class="hover"><a href="javascript:return false;">营销图库</a></li>
                <li><a href="MarktingList.aspx">营销图标设置</a></li>
                <li><a href="TagList.aspx">门店标签设置</a></li>
                <li><a href="../StoreSetting.aspx">门店APP推送设置</a></li>
                <li><a href="StoreAppDonwload.aspx">门店APP下载设置</a></li>
                <li><a href="../../store/DadaLogistics.aspx">达达物流设置</a></li>
            </ul>
        </div>
        <blockquote class="blockquote-default blockquote-tip">门店可选择使用平台所设置的营销图片，并在图片下配置相应的商品</blockquote>
        <div class="searcharea">
            <ul>
                <li>
                    <span>图片名称：</span><span>
                        <input type="text" id="txtImageName" class="forminput form-control" />
                    </span>
                </li>
                <li>
                    <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" />
                </li>
            </ul>
        </div>
        <div class="datalist clearfix">
            <div class="functionHandleArea clearfix">
                <div class="batchHandleArea">
                    <ul>
                        <li class="batchHandleButton">
                            <span class="checkall">
                                <input name="CheckBoxGroupTitle" type="checkbox" class="icheck" id="checkall" /></span>
                            <span>
                                <a href="javascript:bat_deletes()" class="btn btn-default ml_20">删除</a>
                            </span>
                        </li>

                    </ul>
                    <a class="btn btn-primary float_r mb_10" href="javascript:Add();">新增营销图片</a>
                </div>
            </div>
            <!--S warp-->
            <div class="dataarea mainwidth databody">
                <div class="imageDataLeft" id="ImageDataList">
                    <!--S DataShow-->
                    <div class="datalist">
                        <table class="table table-striped">
                            <thead>
                                <tr>
                                    <th width="50"></th>
                                    <th width="10%">图片</th>
                                    <th width="15%" nowrap="nowrap">图片名称</th>
                                    <th>使用说明</th>
                                    <th nowrap="nowrap" width="15%">操作</th>
                                </tr>
                            </thead>
                            <tbody id="datashow"></tbody>
                        </table>
                        <div class="blank12 clearfix"></div>
                    </div>
                </div>
                <!--E DataShow-->
            </div>
            <script id="datatmpl" type="text/html">
                {{each rows as item index}}
                          <tr>
                              <td><span class="icheck">
                                  <input name="CheckBoxGroup" type="checkbox" value='{{item.ImageId}}' />
                              </span></td>
                              <td>
                                  <img src="{{item.ImageUrl}}" width="40" height="40" />
                              </td>
                              <td>{{item.ImageName}}</td>
                              <td>{{item.Description}}</td>
                              <td nowrap="nowrap">
                                  <span><a href="javascript:void(0);" onclick="Edit('{{item.ImageId}}')">编辑</a></span>
                                  <span class="submit_shanchu">
                                      <a href="javascript:void(0);" onclick="Post_Deletes('{{item.ImageId}}')">删除</a>
                                  </span>
                              </td>
                          </tr>
                {{/each}}
           

            </script>
        </div>
        <!--数据列表底部功能区域-->
        <div class="page">
            <div class="bottomPageNumber clearfix">
                <div class="pageNumber">
                    <div class="pagination" id="showpager"></div>
                </div>
            </div>
        </div>
    </div>
    <input type="hidden" name="dataurl" id="dataurl" value="/admin/depot/ashx/MarketingImageList.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/admin/depot/scripts/MarketingImageList.js" type="text/javascript"></script>
</asp:Content>
