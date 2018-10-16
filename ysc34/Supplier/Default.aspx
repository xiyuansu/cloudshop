<%@ Page Title="" Language="C#" MasterPageFile="~/Supplier/Admin.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Hidistro.UI.Web.Supplier.Default" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <style type="text/css">
        body {
            width: 1200px;
            overflow-x: hidden;
            margin: 0 auto;
            padding: 0;
        }

        #mainhtml {
            padding: 0;
            background: none;
            margin: 20px 0 50px 0;
        }

        .col-sm-4 {
            padding: 0 15px 0 0;
        }

        .col-sm-8 {
            padding-left: 0;
        }

        .divnotradepassTip {
            font-size: 16px;
            text-align: center;
            padding-bottom: 10px;
            clear: both;
            display: none;
            position: fixed;
            top: 0;
            width: 1200px;
            height: 50px;
            background: #000;
            z-index: 99;
            color: #fff;
            line-height: 50px;
            opacity: 0.6;
        }

            .divnotradepassTip a {
                color: red;
            }

        .table-gys tbody tr:first-child th {
            border: 0;
            height: 50px;
            border-bottom: 1px solid #e8e8e8;
            font-size: 14px;
            color: #616161;
        }

        .table-gys tbody tr td {
            border-top: 1px dashed #e8e8e8;
            color: #616161;
        }

            .table-gys tbody tr th:first-child, .table-gys tbody tr td:first-child {
                padding-left: 10px;
            }

        .ft {
            color: #000;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            if ($("#istradepass").html().trim() != "1") {
                $("#divnotradepassTip").slideDown();//没有设置交易密码弹出让 设置的提示
            }

            $("#close_divnotradepassTip").click(function () {
                $("#divnotradepassTip").hide();
            })
        });

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div style="display: none;" id="istradepass">
        <asp:Literal ID="ltistradepass" runat="server" />
    </div>
    <%--是否有交易密码，1有，其他无--%>
    <div id="divnotradepassTip" class="divnotradepassTip">
        为了你的账号安全，请先设置交易密码，去<a href="javascript:ShowSecondMenuLeft('系统', 'system/passwordmanage.aspx', null,null)" title="去设置交易密码">设置</a>
        <i id="close_divnotradepassTip" class="iconfont" style="float: right; margin-right: 15px; color: #808080; cursor: pointer;">&#xe601;</i>
    </div>
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
        <div class="col-sm-4 col-m" style="padding-right: 0;">
            <div class="col-sm-12">
                <img class="col-l" alt="已上架商品总数" src="images/icon_memberadd.png" />
                <div class="col-r">
                    <b>
                        <Hi:ClassShowOnDataLitl runat="server" DefaultText="0件" ID="ltrTodayAddMemberNumber" /></b>
                    <p>已上架商品总数</p>
                </div>
            </div>
        </div>

        <div class="col-sm-6" style="padding-left: 0;">
            <div class="col-sm-12 col-index-1">
                <h3>&nbsp; 待处理事务</h3>
                <ul>
                    <li>
                        <div>待发货订单<Hi:ClassShowOnDataLitl runat="server" DefaultText="<font class='ft'>0条</font>" ID="ltrWaitSendOrdersNumber"></Hi:ClassShowOnDataLitl></div>
                    </li>
                    <li>
                        <div>退货单<Hi:ClassShowOnDataLitl runat="server" DefaultText="<font class='ft'>0条</font>" ID="lblOrderReturnNum"></Hi:ClassShowOnDataLitl></div>
                    </li>
                    <li>
                        <div>库存警戒商品<Hi:ClassShowOnDataLitl runat="server" DefaultText="<font class='ft'>0条</font>" ID="hpkIsWarningStockNum"></Hi:ClassShowOnDataLitl>
                        </div>
                    </li>
                    <li>
                        <div>换货单<Hi:ClassShowOnDataLitl runat="server" DefaultText="<font class='ft'>0条</font>" ID="lblOrderReplaceNum"></Hi:ClassShowOnDataLitl></div>
                    </li>
                </ul>
            </div>
        </div>
        <div class="col-sm-6" style="padding-right: 0; padding-left: 10px;">
            <div class="col-sm-12 col-index-1">
                <h3>&nbsp;账户信息</h3>
                <ol>
                    <li>
                        <div>
                            可提现余额<b>
                                <Hi:ClassShowOnDataLitl runat="server" DefaultText="￥0.00" ID="lblBalance"></Hi:ClassShowOnDataLitl></b>
                        </div>
                    </li>
                    <li>
                        <div>
                            提现冻结金额<b>
                                <Hi:ClassShowOnDataLitl runat="server" DefaultText="￥0.00" ID="lblApplyRequestWaitDispose"></Hi:ClassShowOnDataLitl></b>
                        </div>
                    </li>
                    <li>
                        <div>
                            已提现总额<b>
                                <Hi:ClassShowOnDataLitl runat="server" DefaultText="￥0.00" ID="lblBalanceDrawRequested"></Hi:ClassShowOnDataLitl></b>
                        </div>
                    </li>
                </ol>
            </div>
        </div>


        <div class="col-sm-12" style="padding-right: 0;">
            <div class="col-sm-12 col-index-1">
                <h3>&nbsp; 商品销量TOP10</h3>
                <div style="padding: 0 30px;">
                    <asp:Repeater ID="grdProducts" runat="server">
                        <HeaderTemplate>
                            <table class="table table-gys grdProducts">
                                <tr>
                                    <th scope="col">排名</th>
                                    <th scope="col">商品名称</th>
                                    <th scope="col">订单数</th>
                                    <th scope="col">总销量</th>
                                    <th scope="col">供货总额</th>
                                </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <!--项目模板，会进行循环显示，放置表格第二行-->
                            <tr>
                                <td style="height: 50px; width: 120px;">
                                    <span class="tag" ><%#Container.ItemIndex+1%></span>
                                </td>
                                <td>
                                    <div class="p_list_fr">
                                        <a href='<%#"/ProductDetails.aspx?productId="+Eval("ProductId")%>'
                                            target="_blank">
                                            <%# Eval("ProductName") %></a>
                                    </div>
                                </td>
                                <td><%# Eval("OrderNum") %> </td>
                                <td>
                                    <%# Eval("AllSaleNum")%>
                                </td>
                                <td>￥<%# Eval("AllSaleCostPrice","{0:F2}") %></td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            <!--底部模板-->
                            </table>       
                            <!--表格结束部分-->
                        </FooterTemplate>

                    </asp:Repeater>
                </div>
            </div>
        </div>


    </div>
    <script type="text/javascript">
        function ShowSecondMenuLeft(firstnode, secondurl, threeurl, itemName) {
            window.parent.ShowMenuLeft(firstnode, secondurl, threeurl, itemName);
        }

        $(function () {
            $(".table tbody tr .tag").slice(0, 3).css("background", "#9ecf65");
        })
    </script>
</asp:Content>
