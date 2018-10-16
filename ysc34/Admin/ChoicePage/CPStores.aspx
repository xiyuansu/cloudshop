<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="CPStores.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ChoicePage.CPStores" %>


<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <style>
        html { background: #fff !important; }

        body { padding: 0; width: 100%; }

        #mainhtml { margin: 0; padding: 20px 20px 60px 20px; width: 100% !important; }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div id="adddiv" class="dataarea mainwidth databody">
        <div class="datalist clearfix">
            <!-- 搜索-->
            <div class="searcharea">
                <ul id="so_more_none">
                    <li><span>门店名称：</span><input type="text" id="txtStoresName" class="forminput form-control" />
                    </li>
         
                    <li><span>地址：</span><span><Hi:RegionSelector ID="dropRegion" runat="server" ClientIDMode="Static" />
                    </span>
                    </li>
                    <li>标签：
                        <asp:DropDownList ID="ddlTags" DataTextField="TagName" CssClass="dropdown_box" DataValueField="TagId" runat="server"></asp:DropDownList>
                    </li>

                    <li>
                        <input type="button" name="btnSearch" value="查询" id="btnSearch"  class="btn" />
                    </li>
                </ul>
            </div>


            <!--结束-->
            <!--S warp-->
            <div class="dataarea mainwidth databody">
                <div class="datalist">
                    <table class="table table-striped">
                        <thead>
                            <tr>
                                <th width="5%">
                                    <input name="CheckBoxGroup" type="checkbox" onclick="checkThisPageAll()" id="CheckBoxGroup" />

                                </th>
                                <th width="35%">门店名称</th>
                                <th width="35%">门店地址</th>
                                <th>门店标签</th>
                            </tr>
                        </thead>
                        <!--S DataShow-->
                        <tbody id="datashow"></tbody>
                        <!--E DataShow-->
                    </table>
                    <div class="blank12 clearfix"></div>
                </div>

                <!--S Pagination-->
                <div class="page">
                    <div class="bottomPageNumber clearfix">
                        <div class="pageNumber">
                        <em>
                            <input type="checkbox" value="cbxAllEnable" id="cbxAllEnable" name="cbxAllEnable" onclick="getAllStoreEnable()" /><label for="cbxAllEnable">全部可选门店</label>&nbsp;<label>已选择</label><span id="spancheckNum">0</span>家</em>
                            <div class="pagination" id="showpager"></div>
                        </div>
                    </div>
                </div>

                <!--E Pagination-->
                  <div class="modal_iframe_footer">
                    <a href="javascript:void(0)" class="btn btn-primary" onclick="return SelectStores()">添加</a>
                </div>
            </div>
            <script id="datatmpl" type="text/html">
                {{each Models as item index}}
                          <tr>
                              <td><span class="icheck">{{if item.Status==0}}
                                      {{if existsStorId(item.StoreId)}}
                                      <input name="cbxStoreGroup" type="checkbox" checked="checked" value='{{item.StoreId}}' onclick='checkIt(this)' />
                                  {{else}}
                                     <input name="cbxStoreGroup" type="checkbox" value='{{item.StoreId}}' onclick='checkIt(this)' />
                                  {{/if}}
                                  {{else}}                                  
                                  <input name="cbxStoreGroup" type="checkbox" value='{{item.StoreId}}' disabled="disabled" />
                                  {{/if}}
                              </span></td>
                              <td><span>{{item.StoreName}}</span>
                                  {{if item.Status==1}}
                                  <em class="wenhao"><div>未上架</div></em>
                                  {{else if item.Status==2}}
                                 <em class="wenhao"><div>此门店同一商品已参加其他冲突活动</div></em>
                                  {{/if}}
                              </td>
                              <td>{{item.Address}}</td>
                              <td>{{item.Tags}}</td>
                          </tr>
                {{/each}}
            </script>
        </div>
    </div>

	
     <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/admin/promotion/scripts/OrderPromotionStores.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
