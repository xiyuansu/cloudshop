<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="SearchMarketingImage.aspx.cs" Inherits="Hidistro.UI.Web.Depot.home.SearchMarketingImage" Title="选择营销图片" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="Server">
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
            padding: 20px 20px 80px 20px;
            width: 100% !important;
        }
    </style>
    <div class="dataarea mainwidth databody">
        <!--数据列表区域-->
        <div class="datalist clearfix">
            <div class="searcharea clearfix">
                <ul>
                    <li><span>图片名称：</span><span>
                        <input type="text" id="txtImageName" class="forminput form-control" value="" /></span></li>
                    <li>
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" />
                    </li>
                </ul>
            </div>


            <!--S DataShow-->
            <table class="table table-striped">
                <thead>
                    <tr>
                                    <th width="10%">图片</th>
                                    <th width="15%" nowrap="nowrap">图片名称</th>
                                    <th>使用说明</th>
                                    <th nowrap="nowrap" width="15%">操作</th>
                    </tr>
                </thead>
                <tbody id="datashow">
                </tbody>
            </table>
        </div>
        <!--E DataShow-->

        <!--S Pagination-->
        <div class="page">
            <div class="bottomPageNumber clearfix">
                <div class="pageNumber">
                    <div class="pagination" id="showpager"></div>
                </div>
            </div>
        </div>
        <!--E Pagination-->
    </div>

    
            <script id="datatmpl" type="text/html">
                {{each rows as item index}}
                          <tr>
                              <td>
                                  <img src="{{item.ImageUrl}}" width="40" height="40" />
                              </td>
                              <td>{{item.ImageName}}</td>
                              <td>{{item.Description}}</td>
                              <td nowrap="nowrap">
                                  <span><a href="javascript:void(0);" onclick="CheckImage('{{item.ImageId}}','{{item.ImageName}}')">选择</a></span>
                              </td>
                          </tr>
                {{/each}}
           

            </script>
    <input type="hidden" id="hidFilterProductIds" runat="server" clientidmode="Static" />
    <input type="hidden" id="hidIsIncludeHomeProduct" runat="server" clientidmode="Static" />
    <input type="hidden" id="hidIsIncludeAppletProduct" runat="server" clientidmode="Static" />
    <input type="hidden" name="dataurl" id="dataurl" value="/Depot/home/ashx/SearchMarketingImage.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Depot/home/scripts/SearchMarketingImage.js?v=3.1" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="Server">
    <script language="javascript" type="text/javascript">

        function AddBindProduct() {
            var chks = $("input[name='CheckBoxGroup']:checked");
            if (chks.length <= 0) {
                alert("请选择商品");
                return false;
            }
            var origin = artDialog.open.origin;
            var arr = new Array();
            $(chks).each(function (i, item) {
                arr.push($(item).val());
            });
            if (origin.document.getElementById("ctl00_contentHolder_hidSelectProducts") != null && origin.document.getElementById("ctl00_contentHolder_hidSelectProducts") != undefined) {
                $(origin.document.getElementById("ctl00_contentHolder_hidSelectProducts")).val(arr.join(",,,"));
            }
            if (origin.document.getElementById("hidSelectProducts") != null && origin.document.getElementById("hidSelectProducts") != undefined) {
                $(origin.document.getElementById("hidSelectProducts")).val(arr.join(",,,"));
            }
            art.dialog.close();
        }

        function AddAllProduct() {
            GetAllProductIds();
        }
    </script>
</asp:Content>
