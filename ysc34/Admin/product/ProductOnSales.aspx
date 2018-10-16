<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="ProductOnSales.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ProductOnSales" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <style>
       #liProductType .iselect{ width:105px;}
       .red{ color:red;}
    </style>
    <script language="javascript" type="text/javascript">
        $(document).ready(function (e) {
            if (IsOpenReferral == 1) {
                $("#dropBatchOperation").append("<option value='16'>调整商品分销佣金</option>");
            }
        });

        function ToList() {
            var returnUrl = getParam("returnurl");
            if (returnUrl != "") {
                location.href = decodeURIComponent(returnUrl);
            } else {
                window.history.back();
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <input type="hidden" id="hidFilter" value="-1" runat="server" clientidmode="Static" />
            <ul class="title-nav">
                <li><a href="javascript:void(0);" class="tab_status" data-status="-1">全部商品</a></li>
                <li><a href="javascript:void(0);" class="tab_status" data-status="<%=Hidistro.Entities.Commodities.ProductSaleStatus.OnSale.GetHashCode() %>">出售中</a></li>
                <li><a href="javascript:void(0);" class="tab_status" data-status="<%=Hidistro.Entities.Commodities.ProductSaleStatus.UnSale.GetHashCode() %>">下架中</a></li>
                <li><a href="javascript:void(0);" class="tab_status" data-status="<%=Hidistro.Entities.Commodities.ProductSaleStatus.OnStock.GetHashCode() %>">仓库中</a></li>
            </ul>

        </div>

        <div class="datalist">
            <!--搜索-->
            <div class="searcharea">
                <ul>

                    <li><span>商品名称：</span><span>
                        <input type="text" id="txtSearchText" runat="server" clientidmode="Static" class="forminput form-control" />

                    </span></li>
                    <li><span>商家编码：</span><span>
                        <input type="text" id="txtSKU" runat="server" clientidmode="Static" class="forminput form-control" />
                    </span></li>

                    <li>
                        <span>商品分类：</span>
                        <abbr class="formselect">
                            <Hi:ProductCategoriesDropDownList ClientIDMode="Static" ID="dropCategories" runat="server" NullToDisplay="请选择商品分类"
                                CssClass="iselect" />
                        </abbr>
                    </li>
                    <li id="liProductType">
                        <span>商品种类：</span>
                        <asp:DropDownList ID="ddlProductType" ClientIDMode="Static" runat="server" CssClass="iselect">
                            <asp:ListItem Value="-1" Text="请选择"></asp:ListItem>
                            <asp:ListItem Value="0" Text="实物商品"></asp:ListItem>
                            <asp:ListItem Value="1" Text="服务商品"></asp:ListItem>
                        </asp:DropDownList>
                    </li>
                    <li>
                        <span>商品品牌：</span>
                        <abbr class="formselect">
                            <Hi:BrandCategoriesDropDownList runat="server" ClientIDMode="Static" ID="dropBrandList" NullToDisplay="请选择品牌"
                                CssClass="iselect">
                            </Hi:BrandCategoriesDropDownList>
                        </abbr>
                    </li>
                    <li>
                        <span>商品标签：</span>
                        <abbr class="formselect">
                            <Hi:ProductTagsDropDownList ClientIDMode="Static" runat="server" ID="dropTagList" NullToDisplay="请选择标签"
                                CssClass="iselect">
                            </Hi:ProductTagsDropDownList>
                        </abbr>
                    </li>
                    <li>
                        <span>运费模板：</span>
                        <abbr class="formselect">
                            <Hi:ShippingTemplatesDropDownList ID="dropShippingTemplateId" ClientIDMode="Static" runat="server" NullToDisplay="请选择运费模板" ShowNoSetTempaltes="true" CssClass="iselect" />
                        </abbr>
                    </li>
                    <li>

                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" />
                        <a href="javascript:;" id="so_more">更多搜索条件&nbsp;<i class="glyphicon glyphicon-menu-down"></i></a>
                        <input type="text" value="" id="so_more_input" style="display: none;" runat="server" clientidmode="Static" />
                    </li>
                </ul>
                <ul id="so_more_none" style="display: none;" runat="server" clientidmode="Static">
                    <li>
                        <span>商品类型：</span>
                        <abbr class="formselect">
                            <Hi:ProductTypeDownList ID="dropType" ClientIDMode="Static" runat="server" NullToDisplay="请选择类型" CssClass="iselect" />
                        </abbr>
                    </li>

                    <li>

                        <input type="checkbox" id="chkIsWarningStock" class="icheck kc-danger" runat="server" clientidmode="Static"/>库存报警
                    </li>

                </ul>
            </div>
            <div class="functionHandleArea clearfix">

                <div class="batchHandleArea">
                    <div class="checkall">
                        <input name="CheckBoxGroupTitle" type="checkbox" class="icheck" id="checkall" />
                    </div>
                    <select id="dropBatchOperation" class="iselect">
                        <option value="">批量操作</option>
                        <option value="1">商品上架</option>
                        <option value="2">商品下架</option>
                        <option value="3">商品入库</option>
                        <option value="10">调整基本信息</option>
                        <option value="11">调整显示销售数量</option>
                        <option value="12">调整库存</option>
                        <option value="17">调整警戒库存</option>
                        <option value="13">调整会员零售价</option>
                        <option value="15">调整商品关联标签</option>
                        <option value="18">设为跨境商品</option>
                        <option value="19">取消商品跨境</option>
                        <option value="20">设置运费模板</option>
                    </select>

                    <a href="javascript:bat_delete()" class="btn btn-default ml20">删除</a>

                    &nbsp;&nbsp;<asp:Button ID="btnImportToStore" Text="同步所有商品到门店" CssClass="btn btn-primary" runat="server" OnClick="btnImportToStore_Click" OnClientClick="return confirmimport()" Visible="false" />
                    <div class="paginalNum">
                        <span>每页显示数量：</span>
                        <Hi:PageSizeDropDownList ID="PageSizeDropDownList" runat="server"></Hi:PageSizeDropDownList>
                    </div>
                </div>
            </div>


            <!--S DataShow-->
            <table class="table table-striped">
                <thead>
                    <tr>
                        <th style="width: 5%;"></th>
                        <th scope="col" style="width: 5%;">排序</th>
                        <th scope="col">商品</th>
                        <th scope="col" style="width: 10%;">商家编码</th>
                        <th scope="col" style="width: 10%;">商品价格</th>
                        <th scope="col" style="width: 12%">商品状态</th>
                        <th scope="col" style="width: 5%;">库存</th>
                        <th scope="col" style="width: 5%;">销量</th>
                        <th scope="col" style="width: 15%;">操作</th>
                    </tr>
                </thead>
                <tbody id="datashow"></tbody>
            </table>

            <!--E DataShow-->
            <div class="blank12 clearfix"></div>

            <!--S Data Template-->
            <script id="datatmpl" type="text/html">
                {{each rows as item index}}
                <tr>
                    <td>
                        <span class="icheck">
                            <input name="CheckBoxGroup" type="checkbox" value="{{item.ProductId}}" />
                        </span>
                    </td>
                    <td>{{item.DisplaySequence}}</td>
                    <td>
                        <div style="float: left; margin-right: 10px;">
                            <a href="../../{{item.ProductType==1?'wapshop/ServiceProductDetails.aspx':'ProductDetails.aspx'}}?productId={{item.ProductId}}" target="_blank">

                                <img src="{{item.ThumbnailUrl40}}" style="border-width: 0px; width: 40px;">
                            </a>
                        </div>
                        <div class="" style="width: 80%; float: left;">
                            <span class="Name" style="text-align: left;">
							
							<a href="../../{{item.ProductType==1?'wapshop/ServiceProductDetails.aspx':'ProductDetails.aspx'}}?productId={{item.ProductId}}" target="_blank">
								<i class="black">{{item.ProductType==1?"（服务）":""}}</i>
							{{item.ProductName}}
							
							</a>
						
							</span>
							
                        </div>
                        
                    </td>
                    <td>{{item.ProductCode}}
                    </td>
                    <td>
                        <b>{{item.SalePrice.toFixed(2)}}</b>
                    </td>
                    <td>{{item.SaleStatus}}
                    </td>
                    <td>{{item.Stock}}
                    </td>
                    <td>{{item.SaleCounts}} 
                    </td>
                    <td style="white-space: nowrap;">

                        <div class="operation">
                            <span><a href="javascript:ToEdit({{item.ProductId}})">编辑</a></span>
                            <span><a href="javascript:void(0);" onclick="javascript:CollectionProduct('EditReleteProducts.aspx?productId={{item.ProductId}}')">相关商品</a></span>
                            <br />
                            <span>
                                <a href="javascript:deleteModel({{item.ProductId}})">删除</a>
                            </span>
                            <%if (Hidistro.Context.SettingsManager.GetMasterSettings().OpenMultStore)
                                { %>
                            <span id="spanstorestock" runat="server"><a href="javascript:void(0);" onclick="javascript:ShowStoreStock('StoreStocks.aspx?productId={{item.ProductId}}')">门店库存</a></span>

                            <span id="spandown" runat="server"><a href="javascript:void(0);" onclick="javascript:DownStorePd({{item.ProductId}})">门店下架</a></span>
                            <%}%>
                        </div>

                    </td>
                </tr>
                {{/each}}
                 
            </script>
            <!--E Data Template-->


        </div>

        <!--S Pagination-->
        <div class="flbotpage">
            <div class="page">
                <div class="bottomPageNumber clearfix">
                    <div class="pageNumber">
                        <div class="pagination" id="showpager"></div>
                    </div>
                </div>
            </div>
        </div>
        <!--E Pagination-->

    </div>

    <%-- 上架商品--%>
    <div id="divOnSaleProduct" style="display: none;">
        <div class="frame-content">
            <p>
                <em>确定要上架商品？上架后商品将前台出售</em>
            </p>
        </div>
    </div>
    <%-- 下架商品--%>
    <div id="divUnSaleProduct" style="display: none;">
        <div class="frame-content">
            <p>
                <em>确定要下架商品？</em>
            </p>
            <p>
                <em>（友情提示:正在参加活动的商品不能被下架的哦！）</em>
            </p>
        </div>
    </div>
    <%-- 入库商品--%>
    <div id="divInStockProduct" style="display: none;">
        <div class="frame-content">
            <p>
                <em>确定要将商品入库？</em>
            </p>
            <p>
                <em>（友情提示:正在参加活动的商品不能被入库的哦！）</em>
            </p>
        </div>
    </div>
    <%-- 设置包邮--%>
    <div id="divSetFreeShip" style="display: none;">
        <div class="frame-content">
            <p>
                <em>确定要设置这些商品包邮？</em>
            </p>
        </div>
    </div>
    <%-- 取消包邮--%>
    <div id="divCancelFreeShip" style="display: none;">
        <div class="frame-content">
            <p>
                <em>确定要取消这些商品的包邮？</em>
            </p>
        </div>
    </div>
    <%-- 门店下架--%>
    <div id="divUnSaleOffFromStore" style="display: none;">
        <div class="frame-content">
            <p>
                <em>确定将所有门店的此商品下架？</em>
            </p>
        </div>
    </div>
    <%-- 商品标签--%>
    <div id="divTagsProduct" style="display: none;">
        <div class="frame-content">
            <Hi:ProductTagsLiteral ID="litralProductTag" runat="server"></Hi:ProductTagsLiteral>
        </div>
    </div>
    <%-- 商品分销佣金--%>
    <div id="divDeduct" style="display: none;">
        <div class="frame-content">
            <table cellpadding="0" cellspacing="0" width="650px" border="0" class="fram-retreat">
                <tr>
                    <td align="right">会员直接上级抽佣比例：</td>
                    <td align="left" class="bd_td">
                        <div class="input-group">
                            <asp:TextBox ID="txtSubMemberDeduct" CssClass="form_input_s form-control" runat="server" />
                            <span class="input-group-addon">%</span>
                        </div>
                        <%--<asp:Literal ID="litSubMemberDeduct" runat="server" />--%><p>A邀请了B，B后续消费A所能获得的佣金</p>
                    </td>
                </tr>
                <tr>
                    <td align="right" width="30%">会员上二级抽佣比例：</td>
                    <td align="left" class="bd_td">
                        <div class="input-group">
                            <asp:TextBox ID="txtSecondLevelDeduct" CssClass="form_input_s form-control" runat="server" placeholder="20个字符英文和中文例:g/元" />
                            <span class="input-group-addon">%</span>
                        </div>
                        <%--<asp:Literal ID="litReferralDeduct" runat="server" />--%><p>A邀请的会员B后续发展了其它会员C，C消费时，A所能获得的佣金</p>
                    </td>
                </tr>
                <tr>
                    <td align="right">会员上三级抽佣比例：</td>
                    <td align="left" class="bd_td">
                        <div class="input-group">
                            <asp:TextBox ID="txtThreeLevelDeduct" CssClass="form_input_s form-control" runat="server" />
                            <span class="input-group-addon">%</span>
                        </div>
                        <p>A邀请的会员B后续发展了会员C，C又发展了会员D，D消费时，A所能获得的佣金</p>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <%-- 设为跨境商品--%>
    <div id="divOnSetCross_border" style="display: none;">
        <div class="frame-content">
            <p>
                <em>确定要设置这些商品为跨境？</em>
            </p>
        </div>
    </div>
    <%-- 取消商品跨境--%>
    <div id="divOnOffCross_border" style="display: none;">
        <div class="frame-content">
            <p>
                <em>确定要取消这些商品的跨境标签？</em>
            </p>
        </div>
    </div>
    <div style="display: none">
        <asp:Button ID="btnUpdateProductDeducts" runat="server" Text="调整商品分销佣金" CssClass="btn btn-primary" />
        <Hi:TrimTextBox runat="server" ID="txtProductTag" TextMode="MultiLine"></Hi:TrimTextBox>

        <input type="button" id="btnInStock" class="btn btn-primary" value="入库商品" onclick="ChageStatus('OnStock')" />
        <input type="button" id="btnUnSale" class="btn btn-primary" value="下架商品" onclick="ChageStatus('UnSale')" />
        <input type="button" id="btnSaleOff" class="btn btn-primary" value="门店下架" onclick="SaleOff()" />
        <input type="button" id="btnUpSale" class="btn btn-primary" value="上架商品" onclick="ChageStatus('OnSale')" />
        <asp:Button ID="btnSetFreeShip" runat="server" Text="设置包邮" CssClass="btn btn-primary" />
        <asp:Button ID="btnCancelFreeShip" runat="server" Text="取消包邮" CssClass="btn btn-primary" />
        <input type="button" id="btnSetCrossborder" class="btn btn-primary" value="设为跨境商品" onclick="UpdateCrossborder(true)" />
        <input type="button" id="btnOffCrossborder" class="btn btn-primary" value="取消跨境商品" onclick="UpdateCrossborder(false)" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <input type="hidden" name="dataurl" id="dataurl" value="/Admin/product/ashx/ProductOnSales.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Admin/product/scripts/ProductOnSales.js?v=3.5" type="text/javascript"></script>


</asp:Content>
