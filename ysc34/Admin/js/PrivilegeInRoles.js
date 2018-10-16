//父级权限
var privilegeCount = 26;
$(document).ready(function () {

    var $0 = $("#ctl00_contentHolder_cbAll"); $0.attr("Privilege", "0");

    $0 = $("#ctl00_contentHolder_cbSummary"); $0.attr("Privilege", "99999");

    // 商城管理
    $0 = $("#ctl00_contentHolder_cbShop"); $0.attr("Privilege", "10");
    $0 = $("#ctl00_contentHolder_cbSiteContent"); $0.attr("Privilege", "101");
    $0 = $("#ctl00_contentHolder_cbSMSSettings"); $0.attr("Privilege", "102");
    $0 = $("#ctl00_contentHolder_cbEmailSettings"); $0.attr("Privilege", "103");
    $0 = $("#ctl00_contentHolder_cbPaymentModes"); $0.attr("Privilege", "104");
    //$0 = $("#ctl00_contentHolder_cbShippingModes"); $0.attr("Privilege", "105");//配送设置-配送方式列表
    $0 = $("#ctl00_contentHolder_cbShippingTemplets"); $0.attr("Privilege", "106");
    $0 = $("#ctl00_contentHolder_cbExpressComputerpes"); $0.attr("Privilege", "107");
    $0 = $("#ctl00_contentHolder_cbMessageTemplets"); $0.attr("Privilege", "108");
    $0 = $("#ctl00_contentHolder_cbPictureMange"); $0.attr("Privilege", "109");
    $0 = $("#ctl00_contentHolder_cbSiteMap"); $0.attr("Privilege", "1010");
    $0 = $("#ctl00_contentHolder_cbWeiXinTemplatesSet"); $0.attr("Privilege", "1011");
    $0 = $("#ctl00_contentHolder_cbMobbilePaySet"); $0.attr("Privilege", "1012");
    $0 = $("#ctl00_contentHolder_cbRegisterSetting"); $0.attr("Privilege", "1013");
    $0 = $("#ctl00_contentHolder_cbAreaManage"); $0.attr("Privilege", "1014");
    $0 = $("#ctl00_contentHolder_cbDadaLogistics"); $0.attr("Privilege", "1015");
    //页面管理
    $0 = $("#ctl00_contentHolder_cbPageManger"); $0.attr("Privilege", "11");//模板管理
    $0 = $("#ctl00_contentHolder_cbManageThemes"); $0.attr("Privilege", "111");
    $0 = $("#ctl00_contentHolder_cbAfficheList"); $0.attr("Privilege", "112");
    $0 = $("#ctl00_contentHolder_cbHelpCategories"); $0.attr("Privilege", "113");
    $0 = $("#ctl00_contentHolder_cbHelpList"); $0.attr("Privilege", "114");
    $0 = $("#ctl00_contentHolder_cbArticleCategories"); $0.attr("Privilege", "115");
    $0 = $("#ctl00_contentHolder_cbArticleList"); $0.attr("Privilege", "116");
    $0 = $("#ctl00_contentHolder_cbFriendlyLinks"); $0.attr("Privilege", "117");
    $0 = $("#ctl00_contentHolder_cbManageHotKeywords"); $0.attr("Privilege", "118");
    $0 = $("#ctl00_contentHolder_cbEditeThems"); $0.attr("Privilege", "11_0");//可视化编辑
    $0 = $("#ctl00_contentHolder_cbSetHeaderMenu"); $0.attr("Privilege", "11_1");//设置页头菜单
    $0 = $("#ctl00_contentHolder_cbSetWapCTemplates"); $0.attr("Privilege", "11_2");//分类模板
    $0 = $("#ctl00_contentHolder_cbDefineTopics"); $0.attr("Privilege", "11_3");//自定义页面
    $0 = $("#ctl00_contentHolder_cbWapThemeSettings"); $0.attr("Privilege", "11_4");//移动端模板设置
    $0 = $("#ctl00_contentHolder_cbWapThemeEdit"); $0.attr("Privilege", "11_5");//移动端模板编辑
    $0 = $("#ctl00_contentHolder_cbPcThememEdit"); $0.attr("Privilege", "11_6");//PC端模板设置
    $0 = $("#ctl00_contentHolder_cbVShopMenu"); $0.attr("Privilege", "11_7");//PC端模板设置
    

    //商品管理
    $0 = $("#ctl00_contentHolder_cbProductCatalog"); $0.attr("Privilege", "12");
    $0 = $("#ctl00_contentHolder_cbManageProducts"); $0.attr("Privilege", "121");
    $0 = $("#ctl00_contentHolder_cbManageProductsView"); $0.attr("Privilege", "121_1");
    $0 = $("#ctl00_contentHolder_cbManageProductsAdd"); $0.attr("Privilege", "121_2");
    $0 = $("#ctl00_contentHolder_cbManageProductsEdit"); $0.attr("Privilege", "121_3");
    $0 = $("#ctl00_contentHolder_cbManageProductsDelete"); $0.attr("Privilege", "121_4");
    $0 = $("#ctl00_contentHolder_cbInStock"); $0.attr("Privilege", "121_5");
    $0 = $("#ctl00_contentHolder_cbManageProductsUp"); $0.attr("Privilege", "121_6");
    $0 = $("#ctl00_contentHolder_cbManageProductsDown"); $0.attr("Privilege", "121_7");

    $0 = $("#ctl00_contentHolder_cbProductUnclassified"); $0.attr("Privilege", "122");
    $0 = $("#ctl00_contentHolder_cbSubjectProducts"); $0.attr("Privilege", "123");
    $0 = $("#ctl00_contentHolder_cbProductBatchUpload"); $0.attr("Privilege", "124");
    $0 = $("#ctl00_contentHolder_cbProductBatchExport"); $0.attr("Privilege", "125")

    $0 = $("#ctl00_contentHolder_cbProductTypes"); $0.attr("Privilege", "126");
    $0 = $("#ctl00_contentHolder_cbProductTypesView"); $0.attr("Privilege", "126_1");
    $0 = $("#ctl00_contentHolder_cbProductTypesAdd"); $0.attr("Privilege", "126_2");
    $0 = $("#ctl00_contentHolder_cbProductTypesEdit"); $0.attr("Privilege", "126_3");
    $0 = $("#ctl00_contentHolder_cbProductTypesDelete"); $0.attr("Privilege", "126_4");

    $0 = $("#ctl00_contentHolder_cbManageCategories"); $0.attr("Privilege", "127");
    $0 = $("#ctl00_contentHolder_cbManageCategoriesView"); $0.attr("Privilege", "127_1");
    $0 = $("#ctl00_contentHolder_cbManageCategoriesAdd"); $0.attr("Privilege", "127_2");
    $0 = $("#ctl00_contentHolder_cbManageCategoriesEdit"); $0.attr("Privilege", "127_3");
    $0 = $("#ctl00_contentHolder_cbManageCategoriesDelete"); $0.attr("Privilege", "127_4");

    $0 = $("#ctl00_contentHolder_cbBrandCategories"); $0.attr("Privilege", "128");
    $0 = $("#ctl00_contentHolder_cbProductConsultationsManage"); $0.attr("Privilege", "129");
    $0 = $("#ctl00_contentHolder_cbProductReviewsManage"); $0.attr("Privilege", "1210");

    //$0 = $("#ctl00_contentHolder_cbTopicManager"); $0.attr("Privilege", "129");
    //$0 = $("#ctl00_contentHolder_cbTopicAdd"); $0.attr("Privilege", "129_1");
    //$0 = $("#ctl00_contentHolder_cbTopicEdit"); $0.attr("Privilege", "129_2");
    //$0 = $("#ctl00_contentHolder_cbTopicDelete"); $0.attr("Privilege", "129_3");

    $0 = $("#ctl00_contentHolder_cbSyncTaobao"); $0.attr("Privilege", "129_1");//淘宝业务管理-同步淘宝

    //订单管理
    $0 = $("#ctl00_contentHolder_cbSales"); $0.attr("Privilege", "13");
    $0 = $("#ctl00_contentHolder_cbManageOrder"); $0.attr("Privilege", "131");
    $0 = $("#ctl00_contentHolder_cbManageOrderView"); $0.attr("Privilege", "131_1");
    $0 = $("#ctl00_contentHolder_cbManageOrderDelete"); $0.attr("Privilege", "131_2");
    $0 = $("#ctl00_contentHolder_cbManageOrderEdit"); $0.attr("Privilege", "131_3");
    $0 = $("#ctl00_contentHolder_cbManageOrderConfirm"); $0.attr("Privilege", "131_4");
    $0 = $("#ctl00_contentHolder_cbManageOrderSendedGoods"); $0.attr("Privilege", "131_5");
    $0 = $("#ctl00_contentHolder_cbExpressPrint"); $0.attr("Privilege", "131_6");
    $0 = $("#ctl00_contentHolder_cbManageOrderRefund"); $0.attr("Privilege", "131_7");
    $0 = $("#ctl00_contentHolder_cbManageOrderRemark"); $0.attr("Privilege", "131_8");

    $0 = $("#ctl00_contentHolder_cbExpressTemplates"); $0.attr("Privilege", "132");
    $0 = $("#ctl00_contentHolder_cbShipper"); $0.attr("Privilege", "133");

    $0 = $("#ctl00_contentHolder_cbOrderRefundApply"); $0.attr("Privilege", "134");
    $0 = $("#ctl00_contentHolder_cbOrderReturnsApply"); $0.attr("Privilege", "135");
    $0 = $("#ctl00_contentHolder_cbOrderReplaceApply"); $0.attr("Privilege", "136");


    $0 = $("#ctl00_contentHolder_cbManageDebitNote"); $0.attr("Privilege", "137");
    $0 = $("#ctl00_contentHolder_cbManageRefundNote"); $0.attr("Privilege", "138");
    $0 = $("#ctl00_contentHolder_cbManageSendNote"); $0.attr("Privilege", "139");
    $0 = $("#ctl00_contentHolder_cbManagerEturnNote"); $0.attr("Privilege", "1310");
    $0 = $("#ctl00_contentHolder_cbAddExpressTemplate"); $0.attr("Privilege", "1311");
    $0 = $("#ctl00_contentHolder_cbAddShipper"); $0.attr("Privilege", "1312");
    $0 = $("#ctl00_contentHolder_cbShippers"); $0.attr("Privilege", "1313");
    $0 = $("#ctl00_contentHolder_cbSetOrderOption"); $0.attr("Privilege", "1314");
    $0 = $("#ctl00_contentHolder_cbCustomPrintItem"); $0.attr("Privilege", "1315");//快递单管理-自定义打印项

    $0 = $("#ctl00_contentHolder_cbSetSynJDParam"); $0.attr("Privilege", "1316");//同步京东-参数设置
    $0 = $("#ctl00_contentHolder_cbSetSynJDOrder"); $0.attr("Privilege", "1317");//同步京东-同步订单





    //会员管理
    $0 = $("#ctl00_contentHolder_cbManageUsers"); $0.attr("Privilege", "14");
    $0 = $("#ctl00_contentHolder_cbManageMembers"); $0.attr("Privilege", "141");
    $0 = $("#ctl00_contentHolder_cbManageMembersView"); $0.attr("Privilege", "141_1");
    $0 = $("#ctl00_contentHolder_cbManageMembersEdit"); $0.attr("Privilege", "141_2");
    $0 = $("#ctl00_contentHolder_cbManageMembersDelete"); $0.attr("Privilege", "141_3");

    $0 = $("#ctl00_contentHolder_cbMemberRanks"); $0.attr("Privilege", "142");
    $0 = $("#ctl00_contentHolder_cbMemberRanksView"); $0.attr("Privilege", "142_1");
    $0 = $("#ctl00_contentHolder_cbMemberRanksAdd"); $0.attr("Privilege", "142_2");
    $0 = $("#ctl00_contentHolder_cbMemberRanksEdit"); $0.attr("Privilege", "142_3");
    $0 = $("#ctl00_contentHolder_cbMemberRanksDelete"); $0.attr("Privilege", "142_4");

    $0 = $("#ctl00_contentHolder_cbOpenIdServices"); $0.attr("Privilege", "143");
    $0 = $("#ctl00_contentHolder_cbOpenIdSettings"); $0.attr("Privilege", "144");



    $0 = $("#ctl00_contentHolder_cbReceivedMessages"); $0.attr("Privilege", "145");
    $0 = $("#ctl00_contentHolder_cbSendedMessages"); $0.attr("Privilege", "146");
    $0 = $("#ctl00_contentHolder_cbSendMessage"); $0.attr("Privilege", "147");
    $0 = $("#ctl00_contentHolder_cbManageLeaveComments"); $0.attr("Privilege", "149");
    $0 = $("#ctl00_contentHolder_cbUpdateMemberPoint"); $0.attr("Privilege", "149");

    $0 = $("#ctl00_contentHolder_cbAccountSummary"); $0.attr("Privilege", "1410");
    $0 = $("#ctl00_contentHolder_cbReCharge"); $0.attr("Privilege", "1411");
    $0 = $("#ctl00_contentHolder_cbBalanceDrawRequest"); $0.attr("Privilege", "1412");
    $0 = $("#ctl00_contentHolder_cbBalanceDetailsStatistics"); $0.attr("Privilege", "1413");
    $0 = $("#ctl00_contentHolder_cbBalanceDrawRequestStatistics"); $0.attr("Privilege", "1414");
    $0 = $("#ctl00_contentHolder_cbMemberChart"); $0.attr("Privilege", "1415");
    $0 = $("#ctl00_contentHolder_cbMemberTags"); $0.attr("Privilege", "1416");

    //营销分销
    $0 = $("#ctl00_contentHolder_cbMarketing"); $0.attr("Privilege", "16");
    $0 = $("#ctl00_contentHolder_cbGifts"); $0.attr("Privilege", "161");
    $0 = $("#ctl00_contentHolder_cbProductPromotion"); $0.attr("Privilege", "162");
    $0 = $("#ctl00_contentHolder_cbOrderPromotion"); $0.attr("Privilege", "163");
    $0 = $("#ctl00_contentHolder_cbOrderPromotion"); $0.attr("Privilege", "164");
    $0 = $("#ctl00_contentHolder_cbVManageActivity"); $0.attr("Privilege", "165");  
    $0 = $("#ctl00_contentHolder_cbGroupBuy"); $0.attr("Privilege", "166");
    $0 = $("#ctl00_contentHolder_cbCountDown"); $0.attr("Privilege", "167");
    $0 = $("#ctl00_contentHolder_cbCoupons"); $0.attr("Privilege", "168");
    $0 = $("#ctl00_contentHolder_cbUsedCoupons"); $0.attr("Privilege", "169");
    $0 = $("#ctl00_contentHolder_cbSetSEO"); $0.attr("Privilege", "1610");  
    $0 = $("#ctl00_contentHolder_cbRegisterSendCoupons"); $0.attr("Privilege", "1611");
    $0 = $("#ctl00_contentHolder_cbCombinationBuy"); $0.attr("Privilege", "1612");
    $0 = $("#ctl00_contentHolder_cbVManageLotteryActivity"); $0.attr("Privilege", "1613");
    $0 = $("#ctl00_contentHolder_cbFightGroupManage"); $0.attr("Privilege", "1614");
    $0 = $("#ctl00_contentHolder_cbVManageLotteryTicket"); $0.attr("Privilege", "1615");
    $0 = $("#ctl00_contentHolder_cbVManageRedEnvelope"); $0.attr("Privilege", "1616");
    $0 = $("#ctl00_contentHolder_cbAppLotteryDrawSet"); $0.attr("Privilege", "1617");
    $0 = $("#ctl00_contentHolder_cbPreSale"); $0.attr("Privilege", "1618");
    $0 = $("#ctl00_contentHolder_cbAppDownloadCoupons"); $0.attr("Privilege", "1619");  
    $0 = $("#ctl00_contentHolder_cbRechargeGift"); $0.attr("Privilege", "1620");
    $0 = $("#ctl00_contentHolder_cbVotes"); $0.attr("Privilege", "1621");


    //统计报表
    $0 = $("#ctl00_contentHolder_cbTotalReport"); $0.attr("Privilege", "18");
    //$0 = $("#ctl00_contentHolder_cbSaleTotalStatistics"); $0.attr("Privilege", "181");
    //$0 = $("#ctl00_contentHolder_cbUserOrderStatistics"); $0.attr("Privilege", "182");
    //$0 = $("#ctl00_contentHolder_cbSaleList"); $0.attr("Privilege", "183");
    //$0 = $("#ctl00_contentHolder_cbSaleTargetAnalyse"); $0.attr("Privilege", "184");

    //$0 = $("#ctl00_contentHolder_cbMemberArealDistributionStatistics"); $0.attr("Privilege", "188");
    //$0 = $("#ctl00_contentHolder_cbUserIncreaseStatistics"); $0.attr("Privilege", "189");
    //$0 = $("#ctl00_contentHolder_cbPurchaseOrderStatistics"); $0.attr("Privilege", "1810");

    //统计分析
    $0 = $("#ctl00_contentHolder_cbTransactionAnalysis"); $0.attr("Privilege", "181");
    $0 = $("#ctl00_contentHolder_cbTrafficStatistics"); $0.attr("Privilege", "182");
    $0 = $("#ctl00_contentHolder_cbProductAnalysis"); $0.attr("Privilege", "183");
    $0 = $("#ctl00_contentHolder_cbMembertAnalysis"); $0.attr("Privilege", "184");
    $0 = $("#ctl00_contentHolder_cbWachaWeChatFanGrowthAnalysis"); $0.attr("Privilege", "185");
    $0 = $("#ctl00_contentHolder_cbWeChatFansInteractiveAnalysis"); $0.attr("Privilege", "186");


    //微商城
    $0 = $("#ctl00_contentHolder_cbManageVShop"); $0.attr("Privilege", "19");
    $0 = $("#ctl00_contentHolder_cbVServerConfig"); $0.attr("Privilege", "191");
    $0 = $("#ctl00_contentHolder_cbVReplyOnKey"); $0.attr("Privilege", "192");
    $0 = $("#ctl00_contentHolder_cbVManageMenu"); $0.attr("Privilege", "193");
   // $0 = $("#ctl00_contentHolder_cbVShopTempEdit"); $0.attr("Privilege", "1904");
   // $0 = $("#ctl00_contentHolder_cbVShopMenu"); $0.attr("Privilege", "1905");      
   // $0 = $("#ctl00_contentHolder_cbVPayConfig"); $0.attr("Privilege", "1910");
    //$0 = $("#ctl00_contentHolder_cbVShengPaySet"); $0.attr("Privilege", "1911");
    //$0 = $("#ctl00_contentHolder_cbVOffLinePay"); $0.attr("Privilege", "1912");
    //$0 = $("#ctl00_contentHolder_cbVshopAliPay"); $0.attr("Privilege", "1913");
    //$0 = $("#ctl00_contentHolder_cbWxAppletSetting"); $0.attr("Privilege", "1914");
    //$0 = $("#ctl00_contentHolder_cbVXCXPayConfig"); $0.attr("Privilege", "1915");
    
   

    //触屏版

    //$0 = $("#ctl00_contentHolder_cbWapManage"); $0.attr("Privilege", "20");
    //$0 = $("#ctl00_contentHolder_cbWapShopTempEdit"); $0.attr("Privilege", "2001");
    //$0 = $("#ctl00_contentHolder_cbWapShopMenu"); $0.attr("Privilege", "2002");
    //$0 = $("#ctl00_contentHolder_cbWapAliPaySet"); $0.attr("Privilege", "2003");
    //$0 = $("#ctl00_contentHolder_cbWapShengPaySet"); $0.attr("Privilege", "2004");
    //$0 = $("#ctl00_contentHolder_cbWapOffLinePay"); $0.attr("Privilege", "2005");
    //$0 = $("#ctl00_contentHolder_cbWapBankUnionSet"); $0.attr("Privilege", "2006");
    //$0 = $("#ctl00_contentHolder_cbWapAliPayCrossBorderSet"); $0.attr("Privilege", "2007");
   
    //生活号（原支付宝服务窗）
    $0 = $("#ctl00_contentHolder_cbAliohManage"); $0.attr("Privilege", "22");
    //$0 = $("#ctl00_contentHolder_cbAliohShopTempEdit"); $0.attr("Privilege", "2201");
    //$0 = $("#ctl00_contentHolder_cbAliohShopMenu"); $0.attr("Privilege", "2202");
    $0 = $("#ctl00_contentHolder_cbAliohServerConfig"); $0.attr("Privilege", "221");
    $0 = $("#ctl00_contentHolder_cbAliohManageMenu"); $0.attr("Privilege", "222");
   // $0 = $("#ctl00_contentHolder_cbAliohAliPaySet"); $0.attr("Privilege", "2205");
  //  $0 = $("#ctl00_contentHolder_cbAliohShengPaySet"); $0.attr("Privilege", "2206");
  //  $0 = $("#ctl00_contentHolder_cbAliohOfflinePaySet"); $0.attr("Privilege", "2207");
   

    //APP版
    $0 = $("#ctl00_contentHolder_cbAppManage"); $0.attr("Privilege", "21");
    $0 = $("#ctl00_contentHolder_cbAppProductSetting"); $0.attr("Privilege", "211");
    $0 = $("#ctl00_contentHolder_cbAppHomePageEdit"); $0.attr("Privilege", "212");
    $0 = $("#ctl00_contentHolder_cbAppAndroidUpgrade"); $0.attr("Privilege", "213");
    $0 = $("#ctl00_contentHolder_cbAPPIosUpgrade"); $0.attr("Privilege", "214");
    $0 = $("#ctl00_contentHolder_cbAppAliPaySet"); $0.attr("Privilege", "215");
    $0 = $("#ctl00_contentHolder_cbAppWeixinPay"); $0.attr("Privilege", "216");
    $0 = $("#ctl00_contentHolder_cbAppShengPaySet"); $0.attr("Privilege", "217");
    $0 = $("#ctl00_contentHolder_cbAppOffLinePaySet"); $0.attr("Privilege", "218");
    $0 = $("#ctl00_contentHolder_cbAppBankUnionSet"); $0.attr("Privilege", "219");
    $0 = $("#ctl00_contentHolder_cbAppStartPageSet"); $0.attr("Privilege", "2110");  
    $0 = $("#ctl00_contentHolder_cbAppPushSet"); $0.attr("Privilege", "2111");
    $0 = $("#ctl00_contentHolder_cbAppPushRecords"); $0.attr("Privilege", "2112");
    
    

    
    //门店管理

    $0 = $("#ctl00_contentHolder_cbStoreManagement"); $0.attr("Privilege", "23");
    $0 = $("#ctl00_contentHolder_cbStoreSetting"); $0.attr("Privilege", "231");
    $0 = $("#ctl00_contentHolder_cbStoresList"); $0.attr("Privilege", "232");
    $0 = $("#ctl00_contentHolder_cbAddStores"); $0.attr("Privilege", "233");
    $0 = $("#ctl00_contentHolder_cbStoreBalance"); $0.attr("Privilege", "234");
    $0 = $("#ctl00_contentHolder_cbSendGoodOrders"); $0.attr("Privilege", "235");
    $0 = $("#ctl00_contentHolder_cbIbeaconEquipmentList"); $0.attr("Privilege", "236");
    $0 = $("#ctl00_contentHolder_cbIbeaconPageList"); $0.attr("Privilege", "237");
    $0 = $("#ctl00_contentHolder_cbIbeaconEffectStatic"); $0.attr("Privilege", "238");//ibeacon管理-效果统计
    $0 = $("#ctl00_contentHolder_cbHIPOSSetting"); $0.attr("Privilege", "23_0");//HIPOS管理-HIPOS设置
    $0 = $("#ctl00_contentHolder_cbHIPOSBind"); $0.attr("Privilege", "23_1");//HIPOS管理-HIPOS绑定
    $0 = $("#ctl00_contentHolder_cbHIPOSDeal"); $0.attr("Privilege", "23_2");//HIPOS管理-HIPOS交易
    $0 = $("#ctl00_contentHolder_cbMarketingImageList"); $0.attr("Privilege", "23_3");
    $0 = $("#ctl00_contentHolder_cbMarktingList"); $0.attr("Privilege", "23_4");
    $0 = $("#ctl00_contentHolder_cbTagList"); $0.attr("Privilege", "23_5");
    $0 = $("#ctl00_contentHolder_cbStoreAppPushSet"); $0.attr("Privilege", "23_6");
    $0 = $("#ctl00_contentHolder_cbStoreAppDownLoad"); $0.attr("Privilege", "23_7");
  
    //分销员
    $0 = $("#ctl00_contentHolder_cbReferral"); $0.attr("Privilege", "24");
    $0 = $("#ctl00_contentHolder_cbReferralRequest"); $0.attr("Privilege", "241");
    $0 = $("#ctl00_contentHolder_cbReferrals"); $0.attr("Privilege", "242");
    $0 = $("#ctl00_contentHolder_cbDeductSettings"); $0.attr("Privilege", "243");
    $0 = $("#ctl00_contentHolder_cbReferralSettings"); $0.attr("Privilege", "244");
    $0 = $("#ctl00_contentHolder_cbSplittinDrawRequest"); $0.attr("Privilege", "245");
    $0 = $("#ctl00_contentHolder_cbSplittinDrawRecord"); $0.attr("Privilege", "246");
    $0 = $("#ctl00_contentHolder_cbReferralGrades"); $0.attr("Privilege", "247");
    $0 = $("#ctl00_contentHolder_cbAddReferralGrade"); $0.attr("Privilege", "248");
    $0 = $("#ctl00_contentHolder_cbEditReferralGrade"); $0.attr("Privilege", "249");
    $0 = $("#ctl00_contentHolder_cbDeleteReferralGrade"); $0.attr("Privilege", "2410");

    //供应商
    $0 = $("#ctl00_contentHolder_cbSupplier"); $0.attr("Privilege", "25");
    $0 = $("#ctl00_contentHolder_cbSupplierList"); $0.attr("Privilege", "251");
    $0 = $("#ctl00_contentHolder_cbAddSupplier"); $0.attr("Privilege", "252");
    $0 = $("#ctl00_contentHolder_cbSupplierDetails"); $0.attr("Privilege", "253");
    $0 = $("#ctl00_contentHolder_cbEditSupplier"); $0.attr("Privilege", "2531");
    $0 = $("#ctl00_contentHolder_cbSupplierAuditPdList"); $0.attr("Privilege", "254");
    $0 = $("#ctl00_contentHolder_cbSupplierPdList"); $0.attr("Privilege", "255");
    $0 = $("#ctl00_contentHolder_cbxSupplierAudit"); $0.attr("Privilege", "256");
    $0 = $("#ctl00_contentHolder_cbxSupplierEditPd"); $0.attr("Privilege", "257");
    $0 = $("#ctl00_contentHolder_cbSupplierOrderList"); $0.attr("Privilege", "258");
    $0 = $("#ctl00_contentHolder_cbSupplierRefund"); $0.attr("Privilege", "259");
    $0 = $("#ctl00_contentHolder_cbSupplierReturns"); $0.attr("Privilege", "2510");
    $0 = $("#ctl00_contentHolder_cbSupplierReplace"); $0.attr("Privilege", "2511");
    $0 = $("#ctl00_contentHolder_cbSupplierBalance"); $0.attr("Privilege", "2512");
    $0 = $("#ctl00_contentHolder_cbSupplierDrawList"); $0.attr("Privilege", "2513");
    $0 = $("#ctl00_contentHolder_cbSupplierBalanceOrder"); $0.attr("Privilege", "2514");
    $0 = $("#ctl00_contentHolder_cbSupplierBalanceDetail"); $0.attr("Privilege", "2515");

    //小程序
    $0 = $("#ctl00_contentHolder_cbApplet"); $0.attr("Privilege", "26");
    $0 = $("#ctl00_contentHolder_cbAppletProductSetting"); $0.attr("Privilege", "261");
    $0 = $("#ctl00_contentHolder_cbAppletTempEdit"); $0.attr("Privilege", "262");
    $0 = $("#ctl00_contentHolder_cbAppletPayConfig"); $0.attr("Privilege", "263");
    $0 = $("#ctl00_contentHolder_cbAppletMessageTemplate"); $0.attr("Privilege", "264");

    //O2O小程序
    $0 = $("#ctl00_contentHolder_cbO2OApplet"); $0.attr("Privilege", "27");
    $0 = $("#ctl00_contentHolder_cbO2OAppletPayConfig"); $0.attr("Privilege", "271");
    $0 = $("#ctl00_contentHolder_cbO2OAppletMessageTemplate"); $0.attr("Privilege", "272");
  

    // 加载后的全选操作 ------------------------------------------------------------------------------------------------------------------------------------

    showOneLayerOnLoad();

    function showOneLayerOnLoad() {

        var flag;

        for (var i = 10; i <= privilegeCount; i++) {
            $_Control1 = $("input[type='checkbox'][Privilege='" + i + "']");
            var result1 = showTwoLayerOnLoad(i);
            // 如果当前一级下没有下级则判断自己，如果没有选择设置为 false            
            if (result1 == "no" && !$_Control1.attr("checked"))
                flag = false;
            else if (result1)
                $_Control1.attr("checked", true);
            else
                flag = false;
        }
        if (flag)
            $("input[type='checkbox'][Privilege='0']").attr("checked", true);
    };

    function showTwoLayerOnLoad(one) {

        var flag2 = true;
        for (var j = 1; j <= 30; j++) {

            $_Control2 = $("input[type='checkbox'][Privilege='" + one + j + "']");
            console.log(one+"-"+ j);
            // 如果当前一级下没有下级则返回 no ,告诉上级无下级
            if ($_Control2.attr("id") == undefined && j == 1) {
                flag2 = "no";
                return flag2;
            }
                // 如果已经循环到尽头则返回结果
            else if ($_Control2.attr("id") == undefined) {
                return flag2;
            }
                // 如果有下级且没到尽头继续操作
            else if ($_Control2.attr("id") != undefined) {

                // 判断当前的二级下的三级情况
                var result2 = showTheeLayerOnLoad(one, j);


                // 如果当前二级下没有三级则判断自己,如果没选择设置为 false
                if (result2 == "no" && !$_Control2.attr("checked"))
                    flag2 = false;
                else if (result2)
                    $_Control2.attr("checked", true);
                else
                    flag2 = false;
            }
        }

        return flag2;
    };

    function showTheeLayerOnLoad(one, two) {

        var flag3 = true;
        for (var k = 1; k <= 30; k++) {

            $_Control3 = $("input[type='checkbox'][Privilege$='" + one + two + "_" + k + "']");

            // 如果当前二级下没有下级则返回 no ,告诉上级无下级
            if ($_Control3.attr("id") == undefined && k == 1)
                return "no";
                // 如果已经循环到尽头则返回结果
            else if ($_Control3.attr("id") == undefined)
                return flag3;
                // 如果有下级且没到尽头继续操作
            else if ($_Control3.attr("id") != undefined && !$_Control3.attr("checked"))
                flag3 = false;
        }

        return flag3;
    };

    // 单击触发事件 -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    $("input[type='checkbox']").bind('click', function () {

        var value = this.checked;
        // 全选
        if ($(this).attr("privilege") == "0")
            $("input[type='checkbox']").attr("checked", value);
            // 一层的选择
        else if ($(this).attr("privilege") >= 10 && $(this).attr("privilege") <= privilegeCount) {
            // 没有被选择时
            if (!value) {
                $("input[type='checkbox'][Privilege='0']").attr("checked", false);
                $("input[type='checkbox'][Privilege^='" + $(this).attr("privilege") + "']").attr("checked", value);
            }
                // 选择时
            else {
                if (IsOneLayerAllChecked()) {
                    $("input[type='checkbox'][Privilege='0']").attr("checked", true);
                }
                $("input[type='checkbox'][Privilege^='" + $(this).attr("privilege") + "']").attr("checked", value);
            }
            //showTheeLayer2($(this).attr("privilege"), value);
        }
            // 二层的选择
        else if ($(this).attr("privilege").length = 3 && $(this).attr("privilege") > 100) {
            // 没有被选择时
            if (!value) {
                $("input[type='checkbox'][Privilege='0']").attr("checked", false);
                $("input[type='checkbox'][Privilege='" + $(this).attr("privilege").substring(0, 1) + "']").attr("checked", false);
            }
                // 选择时
            else {
                if (IsTwoLayerAllCheckedOfOne($(this).attr("privilege").substring(0, 1)))
                    $("input[type='checkbox'][Privilege='" + $(this).attr("privilege").substring(0, 1) + "']").attr("checked", true);
                if (IsOneLayerAllChecked())
                    $("input[type='checkbox'][Privilege='0']").attr("checked", true);
            }
            showTheeLayer2($(this).attr("privilege"), value);
        }
            // 三层的选择
        else {
            // 没有被选择时
            if (!value) {
                $("input[type='checkbox'][Privilege='0']").attr("checked", false);
                $("input[type='checkbox'][Privilege='" + $(this).attr("privilege").substring(0, 1) + "']").attr("checked", false);
                $("input[type='checkbox'][Privilege='" + $(this).attr("privilege").substring(0, this.Privilege.indexOf("_")) + "']").attr("checked", false);
            }
                // 选择时
            else {
                if (IsThreeLayerAllCheckedOfTwo($(this).attr("privilege").substring(0, $(this).attr("privilege").indexOf("_"))))
                    $("input[type='checkbox'][Privilege='" + $(this).attr("privilege").substring(0, $(this).attr("privilege").indexOf("_")) + "']").attr("checked", true);
                if (IsTwoLayerAllCheckedOfOne($(this).attr("privilege").substring(0, 1)))
                    $("input[type='checkbox'][Privilege='" + $(this).attr("privilege").substring(0, 1) + "']").attr("checked", true);
                if (IsOneLayerAllChecked())
                    $("input[type='checkbox'][Privilege='0']").attr("checked", true);
            }
        }
    })

    // 选择后判断父类是否应该被选择-----------------------------------------------------------------------------------------------------------------------------------------

    // 判断一层是否都选中了
    var IsOneLayerAllChecked = function () {
        for (var i = 10; i <= privilegeCount; i++) {
            if (!$("input[type='checkbox'][Privilege='" + i + "']").attr("checked")) {
                return false;
            }
        }
        return true;
    }

    // 判断某一层下的二层是否都选中了
    var IsTwoLayerAllCheckedOfOne = function (one) {
        for (var i = 1; i <= 30; i++) {
            $_Control2 = $("input[type='checkbox'][Privilege='" + one + i + "']");
            if ($_Control2.attr("id") == undefined) {
                break;
            }

            if (!$_Control2.attr("checked")) {
                return false;
            }
        }
        return true;
    }

    // 判断某二层下的三层是否都选中了
    var IsThreeLayerAllCheckedOfTwo = function (two) {
        for (var i = 1; i <= 30; i++) {
            $_Control3 = $("input[type='checkbox'][Privilege='" + two + "_" + i + "']");

            if ($_Control3.attr("id") == undefined) {
                break;
            }

            if (!$_Control3.attr("checked")) {
                return false;
            }
        }
        return true;
    }

    // 全选反选操作-----------------------------------------------------------------------------------------------------------------------------------------------------------------

    //    var showOneLayer = function(one,value) { 
    //    
    //        for(var i=1;i<=10;i++)
    //        {
    //            
    //            $("input[type='checkbox'][Privilege='"+i+"']).attr("checked",value)"); 
    //                      
    //            showTwoLayer(i,value);
    //        } 
    //    };

    //    var showTwoLayer = function(one,value) { 
    //    
    //        for(var j=1;j<=13;j++)
    //        {
    //            
    //            $_Control1=$("input[type='checkbox'][Privilege='"+one+j+"']"); 
    //                      
    //            if ($_Control1.attr("id")==undefined)
    //            {
    //                break;
    //            }
    //            $_Control1.attr("checked",value);
    //            
    //            showTheeLayer(one,j ,value);
    //        } 
    //    };

    var showTheeLayer = function (one, two, value) {

        for (var k = 1; k <= 30; k++) {

            $_Control2 = $("input[type='checkbox'][Privilege$='" + one + two + "_" + k + "']");

            if ($_Control2.attr("id") == undefined) {
                break;
            }
            $_Control2.attr("checked", value);

        }
    };

    var showTheeLayer2 = function (two, value) {

        for (var k = 1; k <= 30; k++) {

            $_Control2 = $("input[type='checkbox'][Privilege$='" + two + "_" + k + "']");

            if ($_Control2.attr("id") == undefined) {
                break;
            }
            $_Control2.attr("checked", value);

        }
    };

}
);
