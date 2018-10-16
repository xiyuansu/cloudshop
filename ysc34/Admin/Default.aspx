<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.Main" MasterPageFile="~/Admin/Admin.Master" CodeBehind="Default.aspx.cs" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <style type="text/css">
        body {
            width: 1200px;
            overflow-x: hidden;
            margin: 0 auto;
            padding:0;
        }

        #mainhtml {
            padding: 0;
            background:none;
            margin: 20px 0 50px 0;
        }

        .col-sm-4 {
            padding: 0 15px 0 0;
        }

        .col-sm-8 {
            padding-left: 0;
        }

    </style>
    <script type="text/javascript">
        $('.hishop_menu_scroll', window.parent.document).hide();
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">    
    <div class="row col-index">
        <div class="col-sm-4 col-m">
            <div class="col-sm-12">
                <img class="col-l" alt="今日订单金额" src="images/icon_orderamount.png" />
                <div class="col-r">
                    <b>
                        <Hi:ClassShowOnDataLitl runat="server" DefaultText="￥0.00" ID="lblTodayOrderAmout" /></b>
                    <p>今日订单金额</p>
                </div>
            </div>
        </div>
        <div class="col-sm-4  col-m">
            <div class="col-sm-12">
                <img class=" col-l" alt="今日成交订单" src="images/icon_ordernumber.png" />
                <div class="col-r">
                    <b>
                        <Hi:ClassShowOnDataLitl runat="server" DefaultText="0条" ID="lblTodayFinishOrder"></Hi:ClassShowOnDataLitl></b>
                    <p>今日成交订单数</p>
                </div>
            </div>
        </div>
        <div class="col-sm-4 col-m" style="padding-right:0;">
            <div class="col-sm-12">
                <img class="col-l" alt="今日新增会员" src="images/icon_memberadd.png" />
                <div class="col-r">
                    <b>
                        <Hi:ClassShowOnDataLitl runat="server" DefaultText="0位" ID="ltrTodayAddMemberNumber" /></b>
                    <p>今日新增会员</p>
                </div>
            </div>
        </div>

        <div class="col-sm-8">
            <div class="col-sm-12 col-index-1">
                <h3>
                    <img alt="待处理事务" src="images/icon_note.png" />&nbsp; 待处理事务</h3>
                <ul>
                    <li>
                        <div>待发货订单<b><asp:HyperLink ID="ltrWaitSendOrdersNumber" runat="server"></asp:HyperLink></b></div>
                    </li>
                    <li id="liUserDraw" runat="server" clientidmode="Static">
                        <div>会员提现<b><Hi:ClassShowOnDataLitl runat="server" DefaultText="0条" ID="lblMemberBlancedrawRequest"></Hi:ClassShowOnDataLitl></b></div>
                    </li>
                    <li id="liMessage" runat="server" clientidmode="Static">
                        <div>站内信<b><asp:HyperLink ID="hpkMessages" runat="server"></asp:HyperLink></b></div>
                    </li>
                    <li>
                        <div>商品咨询<b><asp:HyperLink ID="hpkZiXun" runat="server"></asp:HyperLink></b></div>
                    </li>
                    <li>
                        <div>已达到警戒库存商品<b><asp:HyperLink ID="hpkIsWarningStockNum" runat="server"></asp:HyperLink></b></div>
                    </li>
                     <li id="liSupplierApply1" runat="server" clientidmode="Static">
                        <div>待审核商品<b><asp:HyperLink ID="hpkSupplierPdAuditNum" runat="server"></asp:HyperLink></b></div>
                    </li>
                    <li id="liSupplierApply2" runat="server" clientidmode="Static">
                        <div>供应商提现申请<b><asp:HyperLink ID="hpkSupplierDrawNum" runat="server"></asp:HyperLink></b></div>
                    </li>
                     <li id="liBirthday" runat="server" clientidmode="Static">
                        <div>生日会员<b><asp:HyperLink ID="hpkBirthdayNum" runat="server"></asp:HyperLink></b></div>
                    </li>
                </ul>
            </div>
        </div>
        <div class="col-sm-4" style="padding-right:0;">
            <div class="col-sm-12 col-index-1">
                <h3>
                    <img src="images/icon_statistic.png" alt="商城信息统计" />&nbsp;商城信息统计</h3>
                <ol>
                    <li>
                        <div>
                            <p>
                                <Hi:ClassShowOnDataLitl runat="server" DefaultText="0位" ID="lblTotalMembers"></Hi:ClassShowOnDataLitl></p>
                            <span>会员总数</span>
                        </div>
                    </li>
                    <li>
                        <div>
                            <p>
                                <Hi:ClassShowOnDataLitl runat="server" DefaultText="0条" ID="lblTotalProducts"></Hi:ClassShowOnDataLitl></p>
                            <span>商品总数</span>
                        </div>
                    </li>
                    <li id="liUserBalance" runat="server" clientidmode="Static">
                        <div style="border: 0">
                            <p>
                                <Hi:ClassShowOnDataLitl runat="server" DefaultText="￥0.00" ID="lblMembersBalanceTotal" /></p>
                            <span>预付款总额</span>
                        </div>
                    </li>
                    <li>
                        <div style="border: 0">
                            <p>
                                <Hi:ClassShowOnDataLitl runat="server" DefaultText="￥0.00" ID="lblOrderPriceMonth"></Hi:ClassShowOnDataLitl></p>
                            <span>近30天订单金额</span>
                        </div>
                    </li>
                     <li id="liSupplierProduct" runat="server" clientidmode="Static">
                        <div style="border: 0">
                            <p>
                                <%--<asp:HyperLink ID="lblSupplierProductNum" runat="server"></asp:HyperLink>--%>
                                <Hi:ClassShowOnDataLitl runat="server" DefaultText="0件" ID="lblSupplierProductNum"></Hi:ClassShowOnDataLitl></p>
                            <span>供应商商品</span>
                        </div>
                    </li>
                     <li id="liSupplierDrawRequest" runat="server" clientidmode="Static">
                        <div style="border: 0">
                            <p>
                                <Hi:ClassShowOnDataLitl runat="server" DefaultText="￥0.00" ID="lblSupplierDrawTotal"></Hi:ClassShowOnDataLitl></p>
                            <span>供应商提现总额</span>
                        </div>
                    </li>
                </ol>
            </div>
        </div>

        <div class="col-sm-8">
            <div class="col-sm-12 col-index-1" style="padding-bottom:20px;">
                <h3>
                    <img src="images/icon-shortcut.png" alt="运营快捷入口" />&nbsp;运营快捷入口</h3>
                <div class="col-sm-2">
                    <a href="javascript:ShowSecondMenuLeft('商 品','product/selectcategory.aspx',null,null)">
                        <div class="img-bg">
                            <i class="iconfont">&#xe610;</i>
                        </div>
                        添加商品</a>
                </div>
                <div class="col-sm-2">
                    <a href="javascript:ShowSecondMenuLeft('订 单','sales/manageorder.aspx',null,null)">
                        <div class="img-bg">
                               <i class="iconfont">&#xe608;</i>
                        </div>
                        订单列表</a>
                </div>
                <div class="col-sm-2">
                    <a href="javascript:ShowSecondMenuLeft('会 员','member/managemembers.aspx',null,null)">
                        <div class="img-bg">
                            <i class="iconfont">&#xe60b;</i>
                        </div>
                        会员管理</a>
                </div>
                <div class="col-sm-2" id="divUserBalance" runat="server" clientidmode="Static">
                    <a href="javascript:ShowSecondMenuLeft('会员','member/accountsummarylist',null,null)">
                        <div class="img-bg">
                            <i class="iconfont">&#xe616;</i>
                        </div>
                        会员预存款</a>
                </div>
                <div class="col-sm-2">
                    <a href="javascript:ShowSecondMenuLeft('统计','report/trafficreport.aspx',null,null)">
                        <div class="img-bg">
                            <i class="iconfont">&#xe617;</i>
                        </div>
                        网站流量</a>
                </div>
                <div class="col-sm-2">
                    <a href="javascript:ShowSecondMenuLeft('系统','tools/sendmessagetemplets.aspx',null,null)">
                        <div class="img-bg">
                            <i class="iconfont">&#xe609;</i>
                        </div>
                        短信营销</a>
                </div>
                <div class="col-sm-2" id="divGroupbuy" runat="server" clientidmode="Static">
                    <a href="javascript:ShowSecondMenuLeft('营销','promotion/groupbuys.aspx',null,'经典团购')">
                        <div class="img-bg">
                           <i class="iconfont">&#xe611;</i>
                        </div>
                        经典团购</a>
                </div>
                <div class="col-sm-2">
                    <a href="javascript:ShowSecondMenuLeft('营销','promotion/countdowns.aspx',null,'限时抢购')">
                        <div class="img-bg">
                           <i class="iconfont">&#xe615;</i>
                        </div>
                        限时抢购</a>
                </div>
                <div class="col-sm-2">
                    <a href="javascript:ShowSecondMenuLeft('营销','promotion/productpromotions.aspx',null,'有买有送')">
                        <div class="img-bg">
                           <i class="iconfont">&#xe60f;</i>
                        </div>
                        有买有送</a>
                </div>
                <div class="col-sm-2">
                    <a href="javascript:ShowSecondMenuLeft('营销','promotion/orderpromotions.aspx',null,'满额优惠')">
                        <div class="img-bg">
                           <i class="iconfont">&#xe607;</i>
                        </div>
                        满额优惠</a>
                </div>
                <div class="col-sm-2">
                    <a href="javascript:ShowSecondMenuLeft('营销','promotion/productpromotions.aspx?isWholesale=true',null,'单品批发')">
                        <div class="img-bg">
                           <i class="iconfont">&#xe60e;</i>
                        </div>
                        单品批发</a>
                </div>
                <div class="col-sm-2">
                    <a href="javascript:ShowSecondMenuLeft('统计','report/transactionanalysisreport.aspx',null,null)">
                        <div class="img-bg">
                            <i class="iconfont">&#xe60d;</i>
                        </div>
                        交易分析</a>
                </div>
            </div>
        </div>

        <div class="col-sm-4" style="padding-right:0;">
            <div class="col-sm-12 col-index-1" style="padding-bottom:20px;">
                <h3>
                    <img src="images/icon_notice.png" alt="公告动态" />&nbsp;公告动态</h3>
                <div class="hishop_list_tab">
                    <div style="height: 290px; overflow-y: hidden;">
                        <iframe src="https://notice.huz.com.cn/ydysc/v3-1.html" scrolling="no" frameborder="0" width="100%" height="100%"></iframe>

                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function ShowSecondMenuLeft(firstnode, secondurl, threeurl,itemName) {
            window.parent.ShowMenuLeft(firstnode, secondurl, threeurl, itemName);
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server"></asp:Content>

