<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="RolePermissions.aspx.cs" Inherits="Hidistro.UI.Web.Admin.RolePermissions" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>


<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <style>
        .PurviewItem {
            clear: both;
        }

        .PurviewItemSave {
            float: left;
            height: 25px;
            line-height: 25px;
            padding-left: 20px;
            margin: 0 0 0 5px;
            _margin-left: 3px;
            vertical-align: middle;
            background: url(images/saveitem.gif) no-repeat 0px 5px;
            padding-left: 20px;
        }

        .PurviewItem ul {
            width: 850px;
            list-style: none;
        }

            .PurviewItem ul li {
                float: left;
                height: 20px;
                line-height: 20px;
                margin-right: 8px;
                width: 140px;
            }

        .PurviewItem ol {
            clear: both;
            padding-left: 98px;
        }

            .PurviewItem ol li {
                float: left;
                height: 20px;
                line-height: 20px;
                margin-right: 8px;
                width: 140px;
            }

        .PurviewItemDivide {
            height: 1px;
            width: 100%;
            overflow: hidden;
            background-color: #ddd;
            margin: 5px 0;
        }

        .PurviewItemBackground {
            background: #E1F3FF;
            border: 1px solid #8ACEFF;
        }

        .clear {
            clear: both;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="datalist clearfix">
            <table width="100%" height="30px" border="0" cellspacing="0">
                <tr class="table_title">
                    <td width="9%"><strong>当前部门：</strong></td>
                    <td align="left"><span style="font-weight: 800;"><strong>
                        <asp:Literal runat="server" ID="lblRoleName"></asp:Literal></strong></span></td>
                </tr>

            </table>

            <div style="margin: 20px 0;">
                <asp:LinkButton ID="btnSetTop" runat="server" Text="保  存" CssClass="btn btn-primary" />
                <a href="Roles.aspx" class="btn btn-default ml_20">返回</a>
            </div>


            <div class="grdGroupList" style="padding: 10px; float: left; border-bottom: 1px solid #ddd">

                <div style="color: Blue; font-weight: 700;">
                    <label>
                        <asp:CheckBox ID="cbAll" runat="server" />全部选定</label>
                </div>
                <div class="PurviewItemDivide"></div>
                <div style="font-weight: 700; color: #000066">
                    <label>
                        <asp:CheckBox ID="cbSummary" runat="server" />后台即时营业信息</label>
                </div>
                <%--商品管理--%>
                <div style="clear: both; font-weight: 700; color: #000066">
                    <label>
                        <asp:CheckBox ID="cbProductCatalog" runat="server" />商品管理</label>
                </div>
                <div class="PurviewItemDivide"></div>
                <div style="padding-left: 20px;">
                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">商品管理：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbManageProducts" runat="server" />商品：</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbManageProductsView" runat="server" />查看</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbManageProductsAdd" runat="server" />添加</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbManageProductsEdit" runat="server" />编辑</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbManageProductsDelete" runat="server" />删除</label></li>
                            <li style="width: 90px">&nbsp;</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbInStock" runat="server" />入库</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbManageProductsUp" runat="server" />上架</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbManageProductsDown" runat="server" />下架</label></li>
                            <li style="width: 90px">&nbsp;</li>
                            <li>&nbsp;</li>
                        </ul>
                        <ol>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbProductUnclassified" runat="server" />未分类商品</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbSubjectProducts" runat="server" />商品标签管理</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbProductBatchUpload" runat="server" />批量上传</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbProductBatchExport" runat="server" />批量导出</label></li>
                        </ol>
                    </div>

                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">商品类型：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbProductTypes" runat="server" />商品类型：</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbProductTypesView" runat="server" />查看</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbProductTypesAdd" runat="server" />添加</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbProductTypesEdit" runat="server" />编辑</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbProductTypesDelete" runat="server" />删除</label></li>
                        </ul>
                    </div>
                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">商品分类：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbManageCategories" runat="server" />商品分类：</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbManageCategoriesView" runat="server" />查看</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbManageCategoriesAdd" runat="server" />添加</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbManageCategoriesEdit" runat="server" />编辑</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbManageCategoriesDelete" runat="server" />删除</label></li>
                        </ul>
                    </div>
                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">品牌分类：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbBrandCategories" runat="server" />品牌分类</label></li>
                        </ul>
                    </div>
                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">商品评论：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbProductConsultationsManage" runat="server" />商品咨询</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbProductReviewsManage" runat="server" />商品评论</label></li>
                        </ul>
                    </div>
                    <%-- <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">移动端专题：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbTopicManager" runat="server" />专题管理：</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbTopicAdd" runat="server" />添加</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbTopicEdit" runat="server" />编辑</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbTopicDelete" runat="server" />删除</label></li>
                        </ul>
                    </div>--%>
                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">淘宝业务管理：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbSyncTaobao" runat="server" />同步淘宝</label></li>
                        </ul>
                    </div>
                </div>
                <%--订单管理--%>
                <div style="clear: both; margin-top: 40px; font-weight: 700; color: #000066">
                    <label>
                        <asp:CheckBox ID="cbSales" runat="server" />订单管理</label>
                </div>
                <div class="PurviewItemDivide"></div>
                <div style="padding-left: 20px;">
                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">订单管理：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbManageOrder" runat="server" />订单管理</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbManageOrderView" runat="server" />查看</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbManageOrderDelete" runat="server" />删除</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbManageOrderEdit" runat="server" />修改</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbManageOrderConfirm" runat="server" />确认收款</label></li>
                            <li style="width: 90px;">&nbsp;</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbManageOrderSendedGoods" runat="server" />订单发货</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbExpressPrint" runat="server" />快递单打印</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbManageOrderRemark" runat="server" />管理员备注</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbSetOrderOption" runat="server" />订单设置</label></li>
                        </ul>

                    </div>

                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">退换货(款)：</li>
                            <li style="width: 90px;">
                                <label>
                                    <asp:CheckBox ID="cbOrderRefundApply" runat="server" />退款申请单</label></li>
                            <li style="width: 90px;">
                                <label>
                                    <asp:CheckBox ID="cbOrderReturnsApply" runat="server" />退货申请单</label></li>
                            <li style="width: 90px;">
                                <label>
                                    <asp:CheckBox ID="cbOrderReplaceApply" runat="server" />换货申请单</label></li>
                        </ul>
                    </div>
                    <div class="PurviewItem" style="display: none;">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">单据管理：</li>
                            <li style="width: 90px;">
                                <label>
                                    <asp:CheckBox ID="cbManageDebitNote" runat="server" />收款单</label></li>
                            <li style="width: 90px;">
                                <label>
                                    <asp:CheckBox ID="cbManageRefundNote" runat="server" />退款单</label></li>
                            <li style="width: 90px;">
                                <label>
                                    <asp:CheckBox ID="cbManageSendNote" runat="server" />发货单</label></li>
                            <li style="width: 90px;">
                                <label>
                                    <asp:CheckBox ID="cbManagerEturnNote" runat="server" />退货单</label></li>
                        </ul>
                    </div>

                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">快递单管理：</li>
                            <li style="width: 90px;">
                                <label>
                                    <asp:CheckBox ID="cbExpressTemplates" runat="server" />快递单模板</label></li>
                            <li style="width: 110px;">
                                <label>
                                    <asp:CheckBox ID="cbAddExpressTemplate" runat="server" />新增快递单模板</label></li>
                            <li style="width: 110px;">
                                <label>
                                    <asp:CheckBox ID="cbShippers" runat="server" />发货人信息管理</label></li>
                            <li style="width: 110px;">
                                <label>
                                    <asp:CheckBox ID="cbAddShipper" runat="server" />添加发货人信息</label></li>
                            <li style="width: 110px;">
                                <label>
                                    <asp:CheckBox ID="cbCustomPrintItem" runat="server" />自定义打印项</label></li>

                        </ul>
                    </div>
                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">同步京东：</li>
                            <li style="width: 90px;">
                                <label>
                                    <asp:CheckBox ID="cbSetSynJDParam" runat="server" />参数设置</label></li>
                            <li style="width: 90px;">
                                <label>
                                    <asp:CheckBox ID="cbSetSynJDOrder" runat="server" />同步订单</label></li>
                        </ul>
                    </div>




                </div>
                <%--会员管理--%>
                <div style="clear: both; margin-top: 40px; font-weight: 700; color: #000066">
                    <label>
                        <asp:CheckBox ID="cbManageUsers" runat="server" />会员管理</label>
                </div>
                <div class="PurviewItemDivide"></div>
                <div style="padding-left: 20px;">

                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">会员：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbManageMembers" runat="server" />会员：</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbManageMembersView" runat="server" />查看</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbManageMembersEdit" runat="server" />编辑</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbManageMembersDelete" runat="server" />删除</label></li>
                        </ul>
                    </div>


                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">会员等级：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbMemberRanks" runat="server" />会员等级：</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbMemberRanksView" runat="server" />查看</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbMemberRanksAdd" runat="server" />添加</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbMemberRanksEdit" runat="server" />编辑</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbMemberRanksDelete" runat="server" />删除</label></li>
                        </ul>
                    </div>
                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">会员分层：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbMemberChart" runat="server" />会员分层：</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbMemberTags" runat="server" />标签管理</label></li>
                        </ul>
                    </div>
                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">预付款管理：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbAccountSummary" runat="server" />账户查询</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbReCharge" runat="server" />账户加款</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbBalanceDrawRequest" runat="server" />提现申请明细</label></li>
                        </ul>
                    </div>
                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 91px; font-weight: 700;">预付款报表：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbBalanceDetailsStatistics" runat="server" />预付款报表</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbBalanceDrawRequestStatistics" runat="server" />提现报表</label></li>

                        </ul>
                    </div>

                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 91px; font-weight: 700;">信任登录：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbOpenIdServices" runat="server" />信任登录列表</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbOpenIdSettings" runat="server" />信任登录配置</label></li>

                        </ul>
                    </div>
                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">站内消息：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbReceivedMessages" runat="server" />收件箱</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbSendedMessages" runat="server" />发件箱</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbSendMessage" runat="server" />写新消息</label></li>
                        </ul>
                    </div>
                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">会员积分：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbUpdateMemberPoint" runat="server" />修改积分</label></li>
                        </ul>
                    </div>
                </div>

                <%--分销员--%>
                <asp:Panel ID="ReferralPanel" runat="server">
                    <div style="clear: both; margin-top: 40px; font-weight: 700; color: #000066">
                        <label>
                            <asp:CheckBox ID="cbReferral" runat="server" />分销员</label>
                    </div>
                    <div class="PurviewItemDivide"></div>
                    <div style="padding-left: 20px;">

                        <div class="PurviewItem">
                            <ul>
                                <li style="width: 90px; font-weight: 700;">分销员管理：</li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbReferralRequest" runat="server" />分销员审核</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbReferrals" runat="server" />分销员列表</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbDeductSettings" runat="server" />分佣规则设置</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbReferralSettings" runat="server" />分销员申请设置</label></li>
                            </ul>
                        </div>
                                                <div class="PurviewItem">
                            <ul>
                                <li style="width: 90px; font-weight: 700;">分销员等级：</li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbReferralGrades" runat="server" />分销员等级管理</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbAddReferralGrade" runat="server" />分销员等级添加</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbEditReferralGrade" runat="server" />分销员等级编辑</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbDeleteReferralGrade" runat="server" />分销员等级删除</label></li>
                            </ul>
                        </div>

                        <div class="PurviewItem">
                            <ul>
                                <li style="width: 90px; font-weight: 700;">结算管理：</li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbSplittinDrawRequest" runat="server" />结算申请</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbSplittinDrawRecord" runat="server" />历史结算</label></li>

                            </ul>
                        </div>


                    </div>
                </asp:Panel>





                <%--营销分销--%>
                <div style="clear: both; margin-top: 40px; font-weight: 700; color: #000066">
                    <label>
                        <asp:CheckBox ID="cbMarketing" runat="server" />营销中心</label>
                </div>
                <div class="PurviewItemDivide"></div>
                <div style="padding-left: 20px;">
                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">运营工具：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbVManageActivity" runat="server" />微报名</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbGifts" runat="server" />礼品中心</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbRegisterSendCoupons" runat="server" />注册送优惠券</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbAppDownloadCoupons" runat="server" />App推广红包</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbRechargeGift" runat="server" />充值赠送</label></li>
                        </ul>
                    </div>

                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 100px; font-weight: 700;">常用促销：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbOrderPromotion" runat="server" />满额优惠/混合批发</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbCombinationBuy" runat="server" />组合购</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbGroupBuy" runat="server" />经典团购</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbCountDown" runat="server" />限时抢购</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbCoupons" runat="server" />优惠券列表</label></li>
                            <li style="width: 240px;">
                                <label>
                                    <asp:CheckBox ID="cbProductPromotion" runat="server" />有买有送/单品批发/手机专享价</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbPreSale" runat="server" />预售</label></li>

                        </ul>
                    </div>
                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">互动营销：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbVManageLotteryActivity" runat="server" />大转盘/刮刮卡/砸金蛋</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbFightGroupManage" runat="server" />火拼团</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbVManageLotteryTicket" runat="server" />微抽奖</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbVotes" runat="server" />投票调查</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbVManageRedEnvelope" runat="server" />代金红包</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbAppLotteryDrawSet" runat="server" />摇一摇抽奖</label></li>

                        </ul>
                    </div>
                </div>

                <%--统计报表--%>
                <div style="clear: both; margin-top: 40px; font-weight: 700; color: #000066">
                    <label>
                        <asp:CheckBox ID="cbTotalReport" runat="server" />统计报表</label>
                </div>
                <div class="PurviewItemDivide"></div>
                <div style="padding-left: 20px;">
                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 94px; font-weight: 700;">统计分析：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbTransactionAnalysis" runat="server" />交易分析</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbTrafficStatistics" runat="server" />流量分析
                                </label>
                            </li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbProductAnalysis" runat="server" />商品分析</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbMembertAnalysis" runat="server" />会员分析</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbWachaWeChatFanGrowthAnalysis" runat="server" />微信粉丝增长分析</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbWeChatFansInteractiveAnalysis" runat="server" />微信粉丝互动分析</label></li>
                        </ul>

                    </div>

                </div>


                <%--  
                <div style="padding-left: 20px;">
                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 94px; font-weight: 700;">零售业务统计：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbSaleTotalStatistics" runat="server" />生意报告</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbUserOrderStatistics" runat="server" />订单统计</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbSaleList" runat="server" />销售明细</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbSaleTargetAnalyse" runat="server" />销售指标分析</label></li>
                        </ul>
                        <ol>
                            <li style="width: 110px;">
                                <label>
                                    <asp:CheckBox ID="cbMemberArealDistributionStatistics" runat="server" />会员分布统计</label></li>
                            <li style="width: 110px;">
                                <label>
                                    <asp:CheckBox ID="cbUserIncreaseStatistics" runat="server" />会员增长统计</label></li>
                        </ol>
                    </div>

                </div>--%>



                <%--系统管理--%>
                <div class="PurviewItemDivide"></div>
                <div style="font-weight: 700; color: #000066">
                    <label>
                        <asp:CheckBox ID="cbShop" runat="server" />系统管理</label>
                </div>
                <div class="PurviewItemDivide"></div>
                <div style="padding-left: 20px;">
                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">基本设置：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbSiteContent" runat="server" />网店基本设置</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbEmailSettings" runat="server" />邮件设置</label></li>
                            <li style="margin-left: -10px;">
                                <label>
                                    <asp:CheckBox ID="cbSMSSettings" runat="server" />手机短信设置</label></li>
                            <li style="margin-left: -10px;">
                                <label>
                                    <asp:CheckBox ID="cbWeiXinTemplatesSet" runat="server" />微信模板设置</label></li>
                            <li style="margin-left: -10px;">
                                <label>
                                    <asp:CheckBox ID="cbRegisterSetting" runat="server" />注册设置</label></li>

                        </ul>
                    </div>

                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">支付设置：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbPaymentModes" runat="server" />支付方式</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbMobbilePaySet" runat="server" />移动端支付</label></li>
                        </ul>
                    </div>
                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">配送设置：</li>
                            <%--<li>
                                <label>
                                    <asp:CheckBox ID="cbShippingModes" runat="server" />配送方式列表</label></li>--%>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbShippingTemplets" runat="server" />运费模板</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbExpressComputerpes" runat="server" />物流公司</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbDadaLogistics" runat="server" />达达物流配置</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbAreaManage" runat="server" />区域管理</label></li>
                        </ul>
                    </div>


                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 100px; font-weight: 700;">邮件短信模板：</li>
                            <li style="margin-left: -10px;">
                                <label>
                                    <asp:CheckBox ID="cbMessageTemplets" runat="server" />邮件短信模板</label></li>

                        </ul>
                    </div>

                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">图库管理：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbPictureMange" runat="server" />图库管理</label></li>
                        </ul>
                    </div>

                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">SEO优化：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbSiteMap" runat="server" />sitemap站点地图</label></li>
                        </ul>
                    </div>
                </div>
                <%--页面管理--%>
                <div style="clear: both; margin-top: 40px; font-weight: 700; color: #000066">
                    <label>
                        <asp:CheckBox ID="cbPageManger" runat="server" />页面管理</label>
                </div>
                <div class="PurviewItemDivide"></div>
                <div style="padding-left: 20px;">
                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">模板管理：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbManageThemes" runat="server" />PC端模板设置</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbWapThemeSettings" runat="server" />移动端模板设置</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbWapThemeEdit" runat="server" />移动端模板编辑</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbVShopMenu" runat="server" />移动端导航</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbPcThememEdit" runat="server" />PC端模板编辑</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbDefineTopics" runat="server" />自定义页面</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbSetHeaderMenu" runat="server" />设置页头菜单</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbSetWapCTemplates" runat="server" />分类模板</label></li>

                        </ul>
                    </div>
                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">内容管理：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbAfficheList" runat="server" />商城公告</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbHelpCategories" runat="server" />帮助分类</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbHelpList" runat="server" />帮助管理</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbArticleCategories" runat="server" />文章分类</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbArticleList" runat="server" />文章管理</label></li>
                        </ul>
                        <ol>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbFriendlyLinks" runat="server" />友情链接</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbManageHotKeywords" runat="server" />热门关键字</label></li>

                        </ol>
                    </div>
                </div>
                <%--门店管理--%>
                <asp:Panel ID="StorePanel" runat="server">
                    <div style="clear: both; margin-top: 40px; font-weight: 700; color: #000066">
                        <label>
                            <asp:CheckBox ID="cbStoreManagement" runat="server" />门店管理</label>
                    </div>
                    <div class="PurviewItemDivide"></div>
                    <div style="padding-left: 20px;">
                        <div class="PurviewItem">
                            <ul>
                                <li style="width: 90px; font-weight: 700;">门店管理：</li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbStoreSetting" runat="server" />门店设置</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbStoresList" runat="server" />门店管理</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbAddStores" runat="server" />添加门店</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbMarketingImageList" runat="server" />营销图库设置</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbMarktingList" runat="server" />营销图标设置</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbTagList" runat="server" />门店标签设置</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbStoreAppPushSet" runat="server" />门店APP推送设置</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbStoreAppDownLoad" runat="server" />门店App下载设置</label></li>

                            </ul>

                        </div>
                        <div class="PurviewItem">
                            <ul>
                                <li style="width: 90px; font-weight: 700;">结算管理：</li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbStoreBalance" runat="server" />结算管理</label></li>

                            </ul>
                        </div>
                        <div class="PurviewItem">
                            <ul>
                                <li style="width: 90px; font-weight: 700;">统计报表：</li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbSendGoodOrders" runat="server" />发货统计</label></li>

                            </ul>
                        </div>

                        <%-- <div class="PurviewItem">
                            <ul>
                                <li style="width: 90px; font-weight: 700;">ibeacon管理：</li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbIbeaconEquipmentList" runat="server" />设备管理</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbIbeaconPageList" runat="server" />页面管理</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbIbeaconEffectStatic" runat="server" />效果统计</label></li>
                            </ul>
                        </div>--%>
                        <div class="PurviewItem">
                            <ul>
                                <li style="width: 90px; font-weight: 700;">HIPOS管理：</li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbHIPOSSetting" runat="server" />HIPOS设置</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbHIPOSBind" runat="server" />HIPOS绑定</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbHIPOSDeal" runat="server" />HIPOS交易</label></li>
                            </ul>
                        </div>

                    </div>
                </asp:Panel>
                <%--微商城管理--%>
                <asp:Panel ID="vPanel" runat="server">
                    <div style="clear: both; margin-top: 40px; font-weight: 700; color: #000066;">
                        <label>
                            <asp:CheckBox ID="cbManageVShop" runat="server" />微商城管理</label>
                    </div>
                    <div class="PurviewItemDivide"></div>
                    <div style="padding-left: 20px;">
                        <div class="PurviewItem">
                            <ul>
                                <li style="width: 90px; font-weight: 700;">微信配置：</li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbVServerConfig" runat="server" />基本配置</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbVReplyOnKey" runat="server" />自定义回复</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbVManageMenu" runat="server" />自定义菜单</label></li>


                            </ul>
                        </div>
                    </div>
                </asp:Panel>
                <%--生活号（原支付宝服务窗）管理--%>
                <asp:Panel ID="AliohPanel" runat="server">
                    <div style="clear: both; margin-top: 40px; font-weight: 700; color: #000066;">
                        <label>
                            <asp:CheckBox ID="cbAliohManage" runat="server" />生活号管理</label>
                    </div>
                    <div class="PurviewItemDivide"></div>
                    <div style="padding-left: 20px;">
                        <div class="PurviewItem">
                            <ul>
                                <li style="width: 90px; font-weight: 700;">生活号配置：</li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbAliohServerConfig" runat="server" />基本配置</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbAliohManageMenu" runat="server" />自定义菜单</label></li>

                            </ul>
                        </div>
                    </div>
                </asp:Panel>
                <%--APP管理--%>
                <asp:Panel ID="appPaenl" runat="server">
                    <div style="clear: both; margin-top: 40px; font-weight: 700; color: #000066;">
                        <label>
                            <asp:CheckBox ID="cbAppManage" runat="server" />APP管理</label>
                    </div>
                    <div class="PurviewItemDivide"></div>
                    <div style="padding-left: 20px;">
                        <div class="PurviewItem">
                            <ul>
                                <li style="width: 90px; font-weight: 700;">首页配置：</li>
                                <%--<li>
                                    <label>
                                        <asp:CheckBox ID="cbAppManageAppBanner" runat="server" />轮播图配置</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbAppManageAppNavigate" runat="server" />图标配置</label></li>--%>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbAppProductSetting" runat="server" />商品配置</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbAppHomePageEdit" runat="server" />首页编辑</label></li>
                            </ul>
                        </div>
                        <div class="PurviewItem">
                            <ul>
                                <li style="width: 90px; font-weight: 700;">版本管理：</li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbAppAndroidUpgrade" runat="server" />Android升级</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbAPPIosUpgrade" runat="server" />ios升级</label></li>

                            </ul>
                        </div>
                        <div class="PurviewItem">
                            <ul>
                                <li style="width: 90px; font-weight: 700;">支付设置：</li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbAppAliPaySet" runat="server" />手机支付宝</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbAppWeixinPay" runat="server" />微信支付设置</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbAppShengPaySet" runat="server" />盛付通支付</label></li>
                                <%--<li>
                                    <label>
                                        <asp:CheckBox ID="cbAppOffLinePaySet" runat="server" />线下支付设置</label></li>--%>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbAppBankUnionSet" runat="server" />银联全渠道支付</label></li>
                            </ul>
                        </div>
                        <div class="PurviewItem">
                            <ul>
                                <li style="width: 90px; font-weight: 700;">启动设置：</li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbAppStartPageSet" runat="server" />启动页图片</label></li>

                                <li>&nbsp;</li>
                            </ul>
                        </div>
                        <div class="PurviewItem">
                            <ul>
                                <li style="width: 90px; font-weight: 700;">自定义推送：</li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbAppPushSet" runat="server" />推送设置</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbAppPushRecords" runat="server" />消息列表</label></li>

                            </ul>
                        </div>

                    </div>
                </asp:Panel>

                <!--小程序模块-->
                <asp:Panel ID="appletPanel" runat="server" Visible="false">
                    <div style="clear: both; margin-top: 40px; font-weight: 700; color: #000066;">
                        <label>
                            <asp:CheckBox ID="cbApplet" runat="server" />小程序</label>
                    </div>
                    <div class="PurviewItemDivide"></div>
                    <div style="padding-left: 20px;">
                        <div class="PurviewItem">
                            <ul>
                                <li style="width: 90px; font-weight: 700;">小程序：</li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbAppletProductSetting" runat="server" />商品配置</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbAppletTempEdit" runat="server" />首页编辑</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbAppletPayConfig" runat="server" />支付设置</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbAppletMessageTemplate" runat="server" />消息模板</label></li>
                            </ul>
                        </div>

                    </div>

                </asp:Panel>
                <asp:Panel ID="o2oAppletPanel" runat="server" Visible="false">
                    <div style="clear: both; margin-top: 40px; font-weight: 700; color: #000066;">
                        <label>
                            <asp:CheckBox ID="cbO2OApplet" runat="server" />O2O小程序</label>
                    </div>
                    <div class="PurviewItemDivide"></div>
                    <div style="padding-left: 20px;">
                        <div class="PurviewItem">
                            <ul>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbO2OAppletPayConfig" runat="server" />支付设置</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbO2OAppletMessageTemplate" runat="server" />消息模板</label></li>
                            </ul>
                        </div>
                    </div>
                </asp:Panel>
                <!--供应商模块-->
                <asp:Panel ID="supplierPanel" runat="server" Visible="false">
                    <div style="clear: both; margin-top: 40px; font-weight: 700; color: #000066;">
                        <label>
                            <asp:CheckBox ID="cbSupplier" runat="server" />供应商管理</label>
                    </div>
                    <div class="PurviewItemDivide"></div>
                    <div style="padding-left: 20px;">
                        <div class="PurviewItem">
                            <ul>
                                <li style="width: 90px; font-weight: 700;">供应商管理：</li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbSupplierList" runat="server" />供应商列表</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbAddSupplier" runat="server" />供应商添加</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbSupplierDetails" runat="server" />供应商查看</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbEditSupplier" runat="server" />供应商编辑</label></li>
                            </ul>
                        </div>
                        <div class="PurviewItem">
                            <ul>
                                <li style="width: 90px; font-weight: 700;">商品管理：</li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbSupplierAuditPdList" runat="server" />待审核商品列表</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbSupplierPdList" runat="server" />商品列表</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbxSupplierAudit" runat="server" />审核商品</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbxSupplierEditPd" runat="server" />编辑商品</label></li>
                            </ul>
                        </div>
                        <div class="PurviewItem">
                            <ul>
                                <li style="width: 90px; font-weight: 700;">订单管理：</li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbSupplierOrderList" runat="server" />订单列表</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbSupplierRefund" runat="server" />退款订单列表</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbSupplierReturns" runat="server" />退货订单列表</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbSupplierReplace" runat="server" />换货申请单列表</label></li>
                            </ul>
                        </div>
                        <div class="PurviewItem">
                            <ul>
                                <li style="width: 90px; font-weight: 700;">财务管理：</li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbSupplierBalance" runat="server" />财务对账</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbSupplierDrawList" runat="server" />提现申请</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbSupplierBalanceOrder" runat="server" />结算明细</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbSupplierBalanceDetail" runat="server" />收支明细</label></li>
                            </ul>
                        </div>

                    </div>

                </asp:Panel>
            </div>

            <asp:Button ID="btnSet1" runat="server" Text="保存" CssClass="btn btn-primary float mt_20"></asp:Button>
            <input type="button" value="返回" class="btn btn-default mt_20 ml_20" onclick="link()" />


        </div>
    </div>



</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" src="/admin/js/PrivilegeInRoles.js?v=3.40"></script>

    <script type="text/javascript" language="javascript">
        function link() {
            window.location.href = 'Roles.aspx';
        }
    </script>
</asp:Content>
