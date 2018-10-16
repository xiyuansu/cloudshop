using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[AdministerCheck(true)]
	public class RolePermissions : AdminPage
	{
		private bool hasWapRight = false;

		private bool hasAliohRight = false;

		private bool hasAppRight = false;

		private bool hasVstoreRight = false;

		private bool hasStoreRight = false;

		private bool hasReferralRight = false;

		private bool hasSupplierRight = false;

		private bool hasAppletRight = false;

		private bool hasO2OAppletRight = false;

		private int roleId;

		protected Literal lblRoleName;

		protected LinkButton btnSetTop;

		protected CheckBox cbAll;

		protected CheckBox cbSummary;

		protected CheckBox cbProductCatalog;

		protected CheckBox cbManageProducts;

		protected CheckBox cbManageProductsView;

		protected CheckBox cbManageProductsAdd;

		protected CheckBox cbManageProductsEdit;

		protected CheckBox cbManageProductsDelete;

		protected CheckBox cbInStock;

		protected CheckBox cbManageProductsUp;

		protected CheckBox cbManageProductsDown;

		protected CheckBox cbProductUnclassified;

		protected CheckBox cbSubjectProducts;

		protected CheckBox cbProductBatchUpload;

		protected CheckBox cbProductBatchExport;

		protected CheckBox cbProductTypes;

		protected CheckBox cbProductTypesView;

		protected CheckBox cbProductTypesAdd;

		protected CheckBox cbProductTypesEdit;

		protected CheckBox cbProductTypesDelete;

		protected CheckBox cbManageCategories;

		protected CheckBox cbManageCategoriesView;

		protected CheckBox cbManageCategoriesAdd;

		protected CheckBox cbManageCategoriesEdit;

		protected CheckBox cbManageCategoriesDelete;

		protected CheckBox cbBrandCategories;

		protected CheckBox cbProductConsultationsManage;

		protected CheckBox cbProductReviewsManage;

		protected CheckBox cbSyncTaobao;

		protected CheckBox cbSales;

		protected CheckBox cbManageOrder;

		protected CheckBox cbManageOrderView;

		protected CheckBox cbManageOrderDelete;

		protected CheckBox cbManageOrderEdit;

		protected CheckBox cbManageOrderConfirm;

		protected CheckBox cbManageOrderSendedGoods;

		protected CheckBox cbExpressPrint;

		protected CheckBox cbManageOrderRemark;

		protected CheckBox cbSetOrderOption;

		protected CheckBox cbOrderRefundApply;

		protected CheckBox cbOrderReturnsApply;

		protected CheckBox cbOrderReplaceApply;

		protected CheckBox cbManageDebitNote;

		protected CheckBox cbManageRefundNote;

		protected CheckBox cbManageSendNote;

		protected CheckBox cbManagerEturnNote;

		protected CheckBox cbExpressTemplates;

		protected CheckBox cbAddExpressTemplate;

		protected CheckBox cbShippers;

		protected CheckBox cbAddShipper;

		protected CheckBox cbCustomPrintItem;

		protected CheckBox cbSetSynJDParam;

		protected CheckBox cbSetSynJDOrder;

		protected CheckBox cbManageUsers;

		protected CheckBox cbManageMembers;

		protected CheckBox cbManageMembersView;

		protected CheckBox cbManageMembersEdit;

		protected CheckBox cbManageMembersDelete;

		protected CheckBox cbMemberRanks;

		protected CheckBox cbMemberRanksView;

		protected CheckBox cbMemberRanksAdd;

		protected CheckBox cbMemberRanksEdit;

		protected CheckBox cbMemberRanksDelete;

		protected CheckBox cbMemberChart;

		protected CheckBox cbMemberTags;

		protected CheckBox cbAccountSummary;

		protected CheckBox cbReCharge;

		protected CheckBox cbBalanceDrawRequest;

		protected CheckBox cbBalanceDetailsStatistics;

		protected CheckBox cbBalanceDrawRequestStatistics;

		protected CheckBox cbOpenIdServices;

		protected CheckBox cbOpenIdSettings;

		protected CheckBox cbReceivedMessages;

		protected CheckBox cbSendedMessages;

		protected CheckBox cbSendMessage;

		protected CheckBox cbUpdateMemberPoint;

		protected Panel ReferralPanel;

		protected CheckBox cbReferral;

		protected CheckBox cbReferralRequest;

		protected CheckBox cbReferrals;

		protected CheckBox cbDeductSettings;

		protected CheckBox cbReferralSettings;

		protected CheckBox cbReferralGrades;

		protected CheckBox cbAddReferralGrade;

		protected CheckBox cbEditReferralGrade;

		protected CheckBox cbDeleteReferralGrade;

		protected CheckBox cbSplittinDrawRequest;

		protected CheckBox cbSplittinDrawRecord;

		protected CheckBox cbMarketing;

		protected CheckBox cbVManageActivity;

		protected CheckBox cbGifts;

		protected CheckBox cbRegisterSendCoupons;

		protected CheckBox cbAppDownloadCoupons;

		protected CheckBox cbRechargeGift;

		protected CheckBox cbOrderPromotion;

		protected CheckBox cbCombinationBuy;

		protected CheckBox cbGroupBuy;

		protected CheckBox cbCountDown;

		protected CheckBox cbCoupons;

		protected CheckBox cbProductPromotion;

		protected CheckBox cbPreSale;

		protected CheckBox cbVManageLotteryActivity;

		protected CheckBox cbFightGroupManage;

		protected CheckBox cbVManageLotteryTicket;

		protected CheckBox cbVotes;

		protected CheckBox cbVManageRedEnvelope;

		protected CheckBox cbAppLotteryDrawSet;

		protected CheckBox cbTotalReport;

		protected CheckBox cbTransactionAnalysis;

		protected CheckBox cbTrafficStatistics;

		protected CheckBox cbProductAnalysis;

		protected CheckBox cbMembertAnalysis;

		protected CheckBox cbWachaWeChatFanGrowthAnalysis;

		protected CheckBox cbWeChatFansInteractiveAnalysis;

		protected CheckBox cbShop;

		protected CheckBox cbSiteContent;

		protected CheckBox cbEmailSettings;

		protected CheckBox cbSMSSettings;

		protected CheckBox cbWeiXinTemplatesSet;

		protected CheckBox cbRegisterSetting;

		protected CheckBox cbPaymentModes;

		protected CheckBox cbMobbilePaySet;

		protected CheckBox cbShippingTemplets;

		protected CheckBox cbExpressComputerpes;

		protected CheckBox cbDadaLogistics;

		protected CheckBox cbAreaManage;

		protected CheckBox cbMessageTemplets;

		protected CheckBox cbPictureMange;

		protected CheckBox cbSiteMap;

		protected CheckBox cbPageManger;

		protected CheckBox cbManageThemes;

		protected CheckBox cbWapThemeSettings;

		protected CheckBox cbWapThemeEdit;

		protected CheckBox cbVShopMenu;

		protected CheckBox cbPcThememEdit;

		protected CheckBox cbDefineTopics;

		protected CheckBox cbSetHeaderMenu;

		protected CheckBox cbSetWapCTemplates;

		protected CheckBox cbAfficheList;

		protected CheckBox cbHelpCategories;

		protected CheckBox cbHelpList;

		protected CheckBox cbArticleCategories;

		protected CheckBox cbArticleList;

		protected CheckBox cbFriendlyLinks;

		protected CheckBox cbManageHotKeywords;

		protected Panel StorePanel;

		protected CheckBox cbStoreManagement;

		protected CheckBox cbStoreSetting;

		protected CheckBox cbStoresList;

		protected CheckBox cbAddStores;

		protected CheckBox cbMarketingImageList;

		protected CheckBox cbMarktingList;

		protected CheckBox cbTagList;

		protected CheckBox cbStoreAppPushSet;

		protected CheckBox cbStoreAppDownLoad;

		protected CheckBox cbStoreBalance;

		protected CheckBox cbSendGoodOrders;

		protected CheckBox cbHIPOSSetting;

		protected CheckBox cbHIPOSBind;

		protected CheckBox cbHIPOSDeal;

		protected Panel vPanel;

		protected CheckBox cbManageVShop;

		protected CheckBox cbVServerConfig;

		protected CheckBox cbVReplyOnKey;

		protected CheckBox cbVManageMenu;

		protected Panel AliohPanel;

		protected CheckBox cbAliohManage;

		protected CheckBox cbAliohServerConfig;

		protected CheckBox cbAliohManageMenu;

		protected Panel appPaenl;

		protected CheckBox cbAppManage;

		protected CheckBox cbAppProductSetting;

		protected CheckBox cbAppHomePageEdit;

		protected CheckBox cbAppAndroidUpgrade;

		protected CheckBox cbAPPIosUpgrade;

		protected CheckBox cbAppAliPaySet;

		protected CheckBox cbAppWeixinPay;

		protected CheckBox cbAppShengPaySet;

		protected CheckBox cbAppBankUnionSet;

		protected CheckBox cbAppStartPageSet;

		protected CheckBox cbAppPushSet;

		protected CheckBox cbAppPushRecords;

		protected Panel appletPanel;

		protected CheckBox cbApplet;

		protected CheckBox cbAppletProductSetting;

		protected CheckBox cbAppletTempEdit;

		protected CheckBox cbAppletPayConfig;

		protected CheckBox cbAppletMessageTemplate;

		protected Panel o2oAppletPanel;

		protected CheckBox cbO2OApplet;

		protected CheckBox cbO2OAppletPayConfig;

		protected CheckBox cbO2OAppletMessageTemplate;

		protected Panel supplierPanel;

		protected CheckBox cbSupplier;

		protected CheckBox cbSupplierList;

		protected CheckBox cbAddSupplier;

		protected CheckBox cbSupplierDetails;

		protected CheckBox cbEditSupplier;

		protected CheckBox cbSupplierAuditPdList;

		protected CheckBox cbSupplierPdList;

		protected CheckBox cbxSupplierAudit;

		protected CheckBox cbxSupplierEditPd;

		protected CheckBox cbSupplierOrderList;

		protected CheckBox cbSupplierRefund;

		protected CheckBox cbSupplierReturns;

		protected CheckBox cbSupplierReplace;

		protected CheckBox cbSupplierBalance;

		protected CheckBox cbSupplierDrawList;

		protected CheckBox cbSupplierBalanceOrder;

		protected CheckBox cbSupplierBalanceDetail;

		protected Button btnSet1;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(this.Page.Request.QueryString["roleId"]))
			{
				base.GotoResourceNotFound();
			}
			else
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				this.AliohPanel.Visible = false;
				this.vPanel.Visible = false;
				this.appPaenl.Visible = false;
				this.ReferralPanel.Visible = false;
				this.StorePanel.Visible = false;
				this.cbO2OApplet.Visible = false;
				bool isTestDomain = Globals.IsTestDomain;
				if (masterSettings.OpenAliho == 1 | isTestDomain)
				{
					this.hasAliohRight = true;
					this.AliohPanel.Visible = true;
				}
				if (masterSettings.OpenVstore == 1 | isTestDomain)
				{
					this.hasVstoreRight = true;
					this.vPanel.Visible = true;
				}
				if (masterSettings.OpenWxApplet | isTestDomain)
				{
					this.hasAppletRight = true;
					this.appletPanel.Visible = true;
				}
				if (masterSettings.OpenWXO2OApplet | isTestDomain)
				{
					this.hasO2OAppletRight = true;
					this.o2oAppletPanel.Visible = true;
				}
				if (masterSettings.OpenMobbile == 1 | isTestDomain)
				{
					this.hasAppRight = true;
					this.appPaenl.Visible = true;
				}
				if (masterSettings.OpenReferral == 1)
				{
					this.ReferralPanel.Visible = true;
					this.hasReferralRight = true;
				}
				if (masterSettings.OpenMultStore | isTestDomain)
				{
					this.StorePanel.Visible = true;
					this.hasStoreRight = true;
				}
				if (masterSettings.OpenSupplier | isTestDomain)
				{
					this.supplierPanel.Visible = true;
					this.hasSupplierRight = true;
				}
				int.TryParse(this.Page.Request.QueryString["roleId"], out this.roleId);
				this.btnSet1.Click += this.btnSet_Click;
				this.btnSetTop.Click += this.btnSet_Click;
				if (!this.Page.IsPostBack)
				{
					RoleInfo role = ManagerHelper.GetRole(this.roleId);
					if (role != null)
					{
						this.lblRoleName.Text = role.RoleName;
					}
					if (this.Page.Request.QueryString["Status"] == "1")
					{
						this.ShowMsg("设置部门权限成功", true);
					}
					this.LoadData(this.roleId);
				}
			}
		}

		private void btnSet_Click(object sender, EventArgs e)
		{
			string empty = string.Empty;
			this.PermissionsSet(this.roleId, out empty);
			if (string.IsNullOrEmpty(empty))
			{
				this.ShowMsg("请勾选需要的权限", false);
			}
			else
			{
				this.Page.Response.Redirect(Globals.GetAdminAbsolutePath($"/store/RolePermissions.aspx?roleId={this.roleId}&Status=1"));
			}
		}

		private void LoadData(int roleId)
		{
			IList<int> privileges = ManagerHelper.GetPrivileges(roleId);
			this.cbSummary.Checked = privileges.Contains(1000);
			this.cbSiteContent.Checked = privileges.Contains(1001);
			this.cbVotes.Checked = privileges.Contains(8018);
			this.cbFriendlyLinks.Checked = privileges.Contains(2007);
			this.cbManageThemes.Checked = privileges.Contains(2001);
			this.cbWapThemeSettings.Checked = privileges.Contains(2014);
			this.cbWapThemeEdit.Checked = privileges.Contains(6004);
			this.cbPcThememEdit.Checked = privileges.Contains(2010);
			this.cbManageHotKeywords.Checked = privileges.Contains(2008);
			this.cbDefineTopics.Checked = privileges.Contains(2013);
			this.cbSetHeaderMenu.Checked = privileges.Contains(2011);
			this.cbSetWapCTemplates.Checked = privileges.Contains(2012);
			this.cbAfficheList.Checked = privileges.Contains(2002);
			this.cbHelpCategories.Checked = privileges.Contains(2003);
			this.cbHelpList.Checked = privileges.Contains(2004);
			this.cbArticleCategories.Checked = privileges.Contains(2005);
			this.cbArticleList.Checked = privileges.Contains(2006);
			this.cbEmailSettings.Checked = privileges.Contains(1002);
			this.cbSMSSettings.Checked = privileges.Contains(1003);
			this.cbWeiXinTemplatesSet.Checked = privileges.Contains(1010);
			this.cbMessageTemplets.Checked = privileges.Contains(1008);
			this.cbShippingTemplets.Checked = privileges.Contains(1006);
			this.cbExpressComputerpes.Checked = privileges.Contains(1007);
			this.cbPictureMange.Checked = privileges.Contains(1009);
			this.cbMobbilePaySet.Checked = privileges.Contains(1011);
			this.cbVShopMenu.Checked = privileges.Contains(6005);
			this.cbRegisterSetting.Checked = privileges.Contains(1012);
			this.cbAreaManage.Checked = privileges.Contains(1013);
			this.cbDadaLogistics.Checked = privileges.Contains(1014);
			this.cbProductTypesView.Checked = privileges.Contains(3017);
			this.cbProductTypesAdd.Checked = privileges.Contains(3018);
			this.cbProductTypesEdit.Checked = privileges.Contains(3019);
			this.cbProductTypesDelete.Checked = privileges.Contains(3020);
			this.cbManageCategoriesView.Checked = privileges.Contains(3021);
			this.cbManageCategoriesAdd.Checked = privileges.Contains(3022);
			this.cbManageCategoriesEdit.Checked = privileges.Contains(3023);
			this.cbManageCategoriesDelete.Checked = privileges.Contains(3024);
			this.cbBrandCategories.Checked = privileges.Contains(3025);
			this.cbManageProductsView.Checked = privileges.Contains(3001);
			this.cbManageProductsAdd.Checked = privileges.Contains(3002);
			this.cbManageProductsEdit.Checked = privileges.Contains(3003);
			this.cbManageProductsDelete.Checked = privileges.Contains(3004);
			this.cbInStock.Checked = privileges.Contains(3005);
			this.cbManageProductsUp.Checked = privileges.Contains(3006);
			this.cbManageProductsDown.Checked = privileges.Contains(3007);
			this.cbProductUnclassified.Checked = privileges.Contains(3010);
			this.cbProductBatchUpload.Checked = privileges.Contains(3012);
			this.cbProductBatchExport.Checked = privileges.Contains(3026);
			this.cbSubjectProducts.Checked = privileges.Contains(3011);
			this.cbSyncTaobao.Checked = privileges.Contains(3031);
			this.cbMemberRanksView.Checked = privileges.Contains(5004);
			this.cbMemberRanksAdd.Checked = privileges.Contains(5005);
			this.cbMemberRanksEdit.Checked = privileges.Contains(5006);
			this.cbMemberRanksDelete.Checked = privileges.Contains(5007);
			this.cbManageMembersView.Checked = privileges.Contains(5001);
			this.cbManageMembersEdit.Checked = privileges.Contains(5002);
			this.cbManageMembersDelete.Checked = privileges.Contains(5003);
			this.cbBalanceDrawRequest.Checked = privileges.Contains(9003);
			this.cbAccountSummary.Checked = privileges.Contains(9001);
			this.cbReCharge.Checked = privileges.Contains(9002);
			this.cbBalanceDetailsStatistics.Checked = privileges.Contains(5010);
			this.cbBalanceDrawRequestStatistics.Checked = privileges.Contains(5011);
			this.cbOpenIdServices.Checked = privileges.Contains(5008);
			this.cbOpenIdSettings.Checked = privileges.Contains(5009);
			this.cbUpdateMemberPoint.Checked = privileges.Contains(5012);
			this.cbMemberChart.Checked = privileges.Contains(5013);
			this.cbMemberTags.Checked = privileges.Contains(5014);
			this.cbSetOrderOption.Checked = privileges.Contains(4022);
			this.cbManageDebitNote.Checked = privileges.Contains(4016);
			this.cbManageRefundNote.Checked = privileges.Contains(4017);
			this.cbManageSendNote.Checked = privileges.Contains(4018);
			this.cbManagerEturnNote.Checked = privileges.Contains(4019);
			this.cbAddExpressTemplate.Checked = privileges.Contains(4020);
			this.cbAddShipper.Checked = privileges.Contains(4021);
			this.cbManageOrderView.Checked = privileges.Contains(4001);
			this.cbManageOrderDelete.Checked = privileges.Contains(4002);
			this.cbManageOrderEdit.Checked = privileges.Contains(4003);
			this.cbManageOrderConfirm.Checked = privileges.Contains(4004);
			this.cbManageOrderSendedGoods.Checked = privileges.Contains(4005);
			this.cbExpressPrint.Checked = privileges.Contains(4006);
			this.cbManageOrderRemark.Checked = privileges.Contains(4008);
			this.cbExpressTemplates.Checked = privileges.Contains(4009);
			this.cbShippers.Checked = privileges.Contains(4010);
			this.cbPaymentModes.Checked = privileges.Contains(1004);
			this.cbOrderRefundApply.Checked = privileges.Contains(4012);
			this.cbOrderReturnsApply.Checked = privileges.Contains(4014);
			this.cbOrderReplaceApply.Checked = privileges.Contains(4013);
			this.cbCustomPrintItem.Checked = privileges.Contains(4023);
			this.cbSetSynJDParam.Checked = privileges.Contains(4024);
			this.cbSetSynJDOrder.Checked = privileges.Contains(4025);
			if (this.hasStoreRight)
			{
				this.cbStoreSetting.Checked = privileges.Contains(14001);
				this.cbStoresList.Checked = privileges.Contains(14002);
				this.cbAddStores.Checked = privileges.Contains(14003);
				this.cbStoreBalance.Checked = privileges.Contains(14004);
				this.cbSendGoodOrders.Checked = privileges.Contains(14005);
				this.cbHIPOSSetting.Checked = privileges.Contains(14009);
				this.cbHIPOSBind.Checked = privileges.Contains(14010);
				this.cbHIPOSDeal.Checked = privileges.Contains(14011);
				this.cbMarketingImageList.Checked = privileges.Contains(14014);
				this.cbMarktingList.Checked = privileges.Contains(14015);
				this.cbTagList.Checked = privileges.Contains(14016);
				this.cbStoreAppPushSet.Checked = privileges.Contains(14017);
				this.cbStoreAppDownLoad.Checked = privileges.Contains(14013);
			}
			this.cbTransactionAnalysis.Checked = privileges.Contains(10010);
			this.cbTrafficStatistics.Checked = privileges.Contains(10011);
			this.cbProductAnalysis.Checked = privileges.Contains(10012);
			this.cbMembertAnalysis.Checked = privileges.Contains(10013);
			this.cbWachaWeChatFanGrowthAnalysis.Checked = privileges.Contains(10014);
			this.cbWeChatFansInteractiveAnalysis.Checked = privileges.Contains(10015);
			this.cbGifts.Checked = privileges.Contains(8001);
			this.cbGroupBuy.Checked = privileges.Contains(8005);
			this.cbCountDown.Checked = privileges.Contains(8006);
			this.cbCoupons.Checked = privileges.Contains(8007);
			this.cbRegisterSendCoupons.Checked = privileges.Contains(8008);
			this.cbAppDownloadCoupons.Checked = privileges.Contains(8016);
			this.cbRechargeGift.Checked = privileges.Contains(8017);
			this.cbSiteMap.Checked = privileges.Contains(8010);
			this.cbProductPromotion.Checked = privileges.Contains(8002);
			this.cbOrderPromotion.Checked = privileges.Contains(8003);
			this.cbCombinationBuy.Checked = privileges.Contains(8011);
			this.cbPreSale.Checked = privileges.Contains(8015);
			this.cbProductConsultationsManage.Checked = privileges.Contains(7001);
			this.cbProductReviewsManage.Checked = privileges.Contains(7002);
			this.cbReceivedMessages.Checked = privileges.Contains(7003);
			this.cbSendedMessages.Checked = privileges.Contains(7004);
			this.cbSendMessage.Checked = privileges.Contains(7005);
			this.cbFightGroupManage.Checked = privileges.Contains(6013);
			if (this.hasVstoreRight)
			{
				this.cbVServerConfig.Checked = privileges.Contains(6001);
				this.cbVReplyOnKey.Checked = privileges.Contains(6002);
				this.cbVManageMenu.Checked = privileges.Contains(6003);
				this.cbVManageLotteryActivity.Checked = privileges.Contains(6006);
				this.cbVManageActivity.Checked = privileges.Contains(6007);
				this.cbVManageLotteryTicket.Checked = privileges.Contains(6008);
				this.cbVManageRedEnvelope.Checked = privileges.Contains(6009);
			}
			if (this.hasAliohRight)
			{
				this.cbAliohServerConfig.Checked = privileges.Contains(12003);
				this.cbAliohManageMenu.Checked = privileges.Contains(12004);
			}
			if (this.hasAppRight)
			{
				this.cbAppProductSetting.Checked = privileges.Contains(13003);
				this.cbAppHomePageEdit.Checked = privileges.Contains(130016);
				this.cbAppAndroidUpgrade.Checked = privileges.Contains(13005);
				this.cbAPPIosUpgrade.Checked = privileges.Contains(13006);
				this.cbAppAliPaySet.Checked = privileges.Contains(13007);
				this.cbAppWeixinPay.Checked = privileges.Contains(13008);
				this.cbAppShengPaySet.Checked = privileges.Contains(13009);
				this.cbAppBankUnionSet.Checked = privileges.Contains(130011);
				this.cbAppStartPageSet.Checked = privileges.Contains(130012);
				this.cbAppLotteryDrawSet.Checked = privileges.Contains(130013);
				this.cbAppPushSet.Checked = privileges.Contains(130014);
				this.cbAppPushRecords.Checked = privileges.Contains(130015);
			}
			if (this.hasReferralRight)
			{
				this.cbReferralRequest.Checked = privileges.Contains(15001);
				this.cbReferrals.Checked = privileges.Contains(15002);
				this.cbReferralSettings.Checked = privileges.Contains(15004);
				this.cbDeductSettings.Checked = privileges.Contains(15003);
				this.cbSplittinDrawRequest.Checked = privileges.Contains(15005);
				this.cbSplittinDrawRecord.Checked = privileges.Contains(15006);
				this.cbReferralGrades.Checked = privileges.Contains(15007);
				this.cbAddReferralGrade.Checked = privileges.Contains(15008);
				this.cbEditReferralGrade.Checked = privileges.Contains(15009);
				this.cbDeleteReferralGrade.Checked = privileges.Contains(15010);
			}
			if (this.hasAppletRight)
			{
				this.cbAppletTempEdit.Checked = privileges.Contains(17004);
				this.cbAppletMessageTemplate.Checked = privileges.Contains(17003);
				this.cbAppletPayConfig.Checked = privileges.Contains(17002);
				this.cbAppletProductSetting.Checked = privileges.Contains(17001);
			}
			if (this.hasO2OAppletRight)
			{
				this.cbO2OAppletMessageTemplate.Checked = privileges.Contains(17003);
				this.cbO2OAppletPayConfig.Checked = privileges.Contains(17002);
			}
			if (this.hasSupplierRight)
			{
				this.cbSupplierList.Checked = privileges.Contains(16001);
				this.cbAddSupplier.Checked = privileges.Contains(16002);
				this.cbSupplierDetails.Checked = privileges.Contains(16004);
				this.cbEditSupplier.Checked = privileges.Contains(16003);
				this.cbSupplierAuditPdList.Checked = privileges.Contains(16100);
				this.cbSupplierPdList.Checked = privileges.Contains(16101);
				this.cbxSupplierAudit.Checked = privileges.Contains(16102);
				this.cbxSupplierEditPd.Checked = privileges.Contains(16103);
				this.cbSupplierOrderList.Checked = privileges.Contains(16200);
				this.cbSupplierRefund.Checked = privileges.Contains(16201);
				this.cbSupplierReturns.Checked = privileges.Contains(16202);
				this.cbSupplierReplace.Checked = privileges.Contains(16203);
				this.cbSupplierBalance.Checked = privileges.Contains(16300);
				this.cbSupplierDrawList.Checked = privileges.Contains(16301);
				this.cbSupplierBalanceOrder.Checked = privileges.Contains(16302);
				this.cbSupplierBalanceDetail.Checked = privileges.Contains(16303);
			}
		}

		private void PermissionsSet(int roleId, out string strPermissions)
		{
			strPermissions = string.Empty;
			if (this.cbSummary.Checked)
			{
				strPermissions = strPermissions + 1000 + ",";
			}
			if (this.cbSiteContent.Checked)
			{
				strPermissions = strPermissions + 1001 + ",";
			}
			if (this.cbVotes.Checked)
			{
				strPermissions = strPermissions + 8018 + ",";
			}
			if (this.cbFriendlyLinks.Checked)
			{
				strPermissions = strPermissions + 2007 + ",";
			}
			if (this.cbManageThemes.Checked)
			{
				strPermissions = strPermissions + 2001 + ",";
			}
			if (this.cbWapThemeSettings.Checked)
			{
				strPermissions = strPermissions + 2014 + ",";
			}
			if (this.cbWapThemeEdit.Checked)
			{
				strPermissions = strPermissions + 6004 + ",";
			}
			if (this.cbPcThememEdit.Checked)
			{
				strPermissions = strPermissions + 2010 + ",";
			}
			if (this.cbDefineTopics.Checked)
			{
				strPermissions = strPermissions + 2013 + ",";
			}
			if (this.cbSetHeaderMenu.Checked)
			{
				strPermissions = strPermissions + 2011 + ",";
			}
			if (this.cbSetWapCTemplates.Checked)
			{
				strPermissions = strPermissions + 2012 + ",";
			}
			if (this.cbManageHotKeywords.Checked)
			{
				strPermissions = strPermissions + 2008 + ",";
			}
			if (this.cbAfficheList.Checked)
			{
				strPermissions = strPermissions + 2002 + ",";
			}
			if (this.cbHelpCategories.Checked)
			{
				strPermissions = strPermissions + 2003 + ",";
			}
			if (this.cbHelpList.Checked)
			{
				strPermissions = strPermissions + 2004 + ",";
			}
			if (this.cbArticleCategories.Checked)
			{
				strPermissions = strPermissions + 2005 + ",";
			}
			if (this.cbArticleList.Checked)
			{
				strPermissions = strPermissions + 2006 + ",";
			}
			if (this.cbEmailSettings.Checked)
			{
				strPermissions = strPermissions + 1002 + ",";
			}
			if (this.cbSMSSettings.Checked)
			{
				strPermissions = strPermissions + 1003 + ",";
			}
			if (this.cbWeiXinTemplatesSet.Checked)
			{
				strPermissions = strPermissions + 1010 + ",";
			}
			if (this.cbMessageTemplets.Checked)
			{
				strPermissions = strPermissions + 1008 + ",";
			}
			if (this.cbShippingTemplets.Checked)
			{
				strPermissions = strPermissions + 1006 + ",";
			}
			if (this.cbExpressComputerpes.Checked)
			{
				strPermissions = strPermissions + 1007 + ",";
			}
			if (this.cbPictureMange.Checked)
			{
				strPermissions = strPermissions + 1009 + ",";
			}
			if (this.cbMobbilePaySet.Checked)
			{
				strPermissions = strPermissions + 1011 + ",";
			}
			if (this.cbVShopMenu.Checked)
			{
				strPermissions = strPermissions + 6005 + ",";
			}
			if (this.cbRegisterSetting.Checked)
			{
				strPermissions = strPermissions + 1012 + ",";
			}
			if (this.cbAreaManage.Checked)
			{
				strPermissions = strPermissions + 1013 + ",";
			}
			if (this.cbDadaLogistics.Checked)
			{
				strPermissions = strPermissions + 1014 + ",";
			}
			if (this.cbProductTypesView.Checked)
			{
				strPermissions = strPermissions + 3017 + ",";
			}
			if (this.cbProductTypesAdd.Checked)
			{
				strPermissions = strPermissions + 3018 + ",";
			}
			if (this.cbProductTypesEdit.Checked)
			{
				strPermissions = strPermissions + 3019 + ",";
			}
			if (this.cbProductTypesDelete.Checked)
			{
				strPermissions = strPermissions + 3020 + ",";
			}
			if (this.cbManageCategoriesView.Checked)
			{
				strPermissions = strPermissions + 3021 + ",";
			}
			if (this.cbManageCategoriesAdd.Checked)
			{
				strPermissions = strPermissions + 3022 + ",";
			}
			if (this.cbManageCategoriesEdit.Checked)
			{
				strPermissions = strPermissions + 3023 + ",";
			}
			if (this.cbManageCategoriesDelete.Checked)
			{
				strPermissions = strPermissions + 3024 + ",";
			}
			if (this.cbBrandCategories.Checked)
			{
				strPermissions = strPermissions + 3025 + ",";
			}
			if (this.cbManageProductsView.Checked)
			{
				strPermissions = strPermissions + 3001 + ",";
			}
			if (this.cbManageProductsAdd.Checked)
			{
				strPermissions = strPermissions + 3002 + ",";
			}
			if (this.cbManageProductsEdit.Checked)
			{
				strPermissions = strPermissions + 3003 + ",";
			}
			if (this.cbManageProductsDelete.Checked)
			{
				strPermissions = strPermissions + 3004 + ",";
			}
			if (this.cbInStock.Checked)
			{
				strPermissions = strPermissions + 3005 + ",";
			}
			if (this.cbManageProductsUp.Checked)
			{
				strPermissions = strPermissions + 3006 + ",";
			}
			if (this.cbManageProductsDown.Checked)
			{
				strPermissions = strPermissions + 3007 + ",";
			}
			if (this.cbProductUnclassified.Checked)
			{
				strPermissions = strPermissions + 3010 + ",";
			}
			if (this.cbProductBatchUpload.Checked)
			{
				strPermissions = strPermissions + 3012 + ",";
			}
			if (this.cbProductBatchExport.Checked)
			{
				strPermissions = strPermissions + 3026 + ",";
			}
			if (this.cbSubjectProducts.Checked)
			{
				strPermissions = strPermissions + 3011 + ",";
			}
			if (this.cbSyncTaobao.Checked)
			{
				strPermissions = strPermissions + 3031 + ",";
			}
			if (this.cbMemberRanksView.Checked)
			{
				strPermissions = strPermissions + 5004 + ",";
			}
			if (this.cbMemberRanksAdd.Checked)
			{
				strPermissions = strPermissions + 5005 + ",";
			}
			if (this.cbMemberRanksEdit.Checked)
			{
				strPermissions = strPermissions + 5006 + ",";
			}
			if (this.cbMemberRanksDelete.Checked)
			{
				strPermissions = strPermissions + 5007 + ",";
			}
			if (this.cbManageMembersView.Checked)
			{
				strPermissions = strPermissions + 5001 + ",";
			}
			if (this.cbManageMembersEdit.Checked)
			{
				strPermissions = strPermissions + 5002 + ",";
			}
			if (this.cbManageMembersDelete.Checked)
			{
				strPermissions = strPermissions + 5003 + ",";
			}
			if (this.cbBalanceDrawRequest.Checked)
			{
				strPermissions = strPermissions + 9003 + ",";
			}
			if (this.cbAccountSummary.Checked)
			{
				strPermissions = strPermissions + 9001 + ",";
			}
			if (this.cbReCharge.Checked)
			{
				strPermissions = strPermissions + 9002 + ",";
			}
			if (this.cbBalanceDetailsStatistics.Checked)
			{
				strPermissions = strPermissions + 5010 + ",";
			}
			if (this.cbBalanceDrawRequestStatistics.Checked)
			{
				strPermissions = strPermissions + 5011 + ",";
			}
			if (this.cbOpenIdServices.Checked)
			{
				strPermissions = strPermissions + 5008 + ",";
			}
			if (this.cbOpenIdSettings.Checked)
			{
				strPermissions = strPermissions + 5009 + ",";
			}
			if (this.cbUpdateMemberPoint.Checked)
			{
				strPermissions = strPermissions + 5012 + ",";
			}
			if (this.cbMemberChart.Checked)
			{
				strPermissions = strPermissions + 5013 + ",";
			}
			if (this.cbMemberTags.Checked)
			{
				strPermissions = strPermissions + 5014 + ",";
			}
			if (this.cbManageDebitNote.Checked)
			{
				strPermissions = strPermissions + 4016 + ",";
			}
			if (this.cbSetOrderOption.Checked)
			{
				strPermissions = strPermissions + 4022 + ",";
			}
			if (this.cbManageRefundNote.Checked)
			{
				strPermissions = strPermissions + 4017 + ",";
			}
			if (this.cbManageSendNote.Checked)
			{
				strPermissions = strPermissions + 4018 + ",";
			}
			if (this.cbManagerEturnNote.Checked)
			{
				strPermissions = strPermissions + 4019 + ",";
			}
			if (this.cbAddExpressTemplate.Checked)
			{
				strPermissions = strPermissions + 4020 + ",";
			}
			if (this.cbAddShipper.Checked)
			{
				strPermissions = strPermissions + 4021 + ",";
			}
			if (this.cbManageOrderView.Checked)
			{
				strPermissions = strPermissions + 4001 + ",";
			}
			if (this.cbManageOrderDelete.Checked)
			{
				strPermissions = strPermissions + 4002 + ",";
			}
			if (this.cbManageOrderEdit.Checked)
			{
				strPermissions = strPermissions + 4003 + ",";
			}
			if (this.cbManageOrderConfirm.Checked)
			{
				strPermissions = strPermissions + 4004 + ",";
			}
			if (this.cbManageOrderSendedGoods.Checked)
			{
				strPermissions = strPermissions + 4005 + ",";
			}
			if (this.cbExpressPrint.Checked)
			{
				strPermissions = strPermissions + 4006 + ",";
			}
			if (this.cbManageOrderRemark.Checked)
			{
				strPermissions = strPermissions + 4008 + ",";
			}
			if (this.cbExpressTemplates.Checked)
			{
				strPermissions = strPermissions + 4009 + ",";
			}
			if (this.cbShippers.Checked)
			{
				strPermissions = strPermissions + 4010 + ",";
			}
			if (this.cbPaymentModes.Checked)
			{
				strPermissions = strPermissions + 1004 + ",";
			}
			if (this.cbOrderRefundApply.Checked)
			{
				strPermissions = strPermissions + 4012 + ",";
			}
			if (this.cbOrderReplaceApply.Checked)
			{
				strPermissions = strPermissions + 4013 + ",";
			}
			if (this.cbOrderReturnsApply.Checked)
			{
				strPermissions = strPermissions + 4014 + ",";
			}
			if (this.cbTransactionAnalysis.Checked)
			{
				strPermissions = strPermissions + 10010 + ",";
			}
			if (this.cbTrafficStatistics.Checked)
			{
				strPermissions = strPermissions + 10011 + ",";
			}
			if (this.cbProductAnalysis.Checked)
			{
				strPermissions = strPermissions + 10012 + ",";
			}
			if (this.cbMembertAnalysis.Checked)
			{
				strPermissions = strPermissions + 10013 + ",";
			}
			if (this.cbWachaWeChatFanGrowthAnalysis.Checked)
			{
				strPermissions = strPermissions + 10014 + ",";
			}
			if (this.cbWeChatFansInteractiveAnalysis.Checked)
			{
				strPermissions = strPermissions + 10015 + ",";
			}
			if (this.cbCustomPrintItem.Checked)
			{
				strPermissions = strPermissions + 4023 + ",";
			}
			if (this.cbSetSynJDParam.Checked)
			{
				strPermissions = strPermissions + 4024 + ",";
			}
			if (this.cbSetSynJDOrder.Checked)
			{
				strPermissions = strPermissions + 4025 + ",";
			}
			if (this.cbGifts.Checked)
			{
				strPermissions = strPermissions + 8001 + ",";
			}
			if (this.cbGroupBuy.Checked)
			{
				strPermissions = strPermissions + 8005 + ",";
			}
			if (this.cbCountDown.Checked)
			{
				strPermissions = strPermissions + 8006 + ",";
			}
			if (this.cbCoupons.Checked)
			{
				strPermissions = strPermissions + 8007 + ",";
			}
			if (this.cbRegisterSendCoupons.Checked)
			{
				strPermissions = strPermissions + 8008 + ",";
			}
			if (this.cbAppDownloadCoupons.Checked)
			{
				strPermissions = strPermissions + 8016 + ",";
			}
			if (this.cbRechargeGift.Checked)
			{
				strPermissions = strPermissions + 8017 + ",";
			}
			if (this.cbSiteMap.Checked)
			{
				strPermissions = strPermissions + 8010 + ",";
			}
			if (this.cbProductPromotion.Checked)
			{
				strPermissions = strPermissions + 8002 + ",";
			}
			if (this.cbOrderPromotion.Checked)
			{
				strPermissions = strPermissions + 8003 + ",";
			}
			if (this.cbCombinationBuy.Checked)
			{
				strPermissions = strPermissions + 8011 + ",";
			}
			if (this.cbPreSale.Checked)
			{
				strPermissions = strPermissions + 8015 + ",";
			}
			if (this.cbVManageLotteryActivity.Checked)
			{
				strPermissions = strPermissions + 6006 + ",";
			}
			if (this.cbVManageActivity.Checked)
			{
				strPermissions = strPermissions + 6007 + ",";
			}
			if (this.cbVManageLotteryTicket.Checked)
			{
				strPermissions = strPermissions + 6008 + ",";
			}
			if (this.cbVManageRedEnvelope.Checked)
			{
				strPermissions = strPermissions + 6009 + ",";
			}
			if (this.cbFightGroupManage.Checked)
			{
				strPermissions = strPermissions + 6013 + ",";
			}
			if (this.cbProductConsultationsManage.Checked)
			{
				strPermissions = strPermissions + 7001 + ",";
			}
			if (this.cbProductReviewsManage.Checked)
			{
				strPermissions = strPermissions + 7002 + ",";
			}
			if (this.cbReceivedMessages.Checked)
			{
				strPermissions = strPermissions + 7003 + ",";
			}
			if (this.cbSendedMessages.Checked)
			{
				strPermissions = strPermissions + 7004 + ",";
			}
			if (this.cbSendMessage.Checked)
			{
				strPermissions = strPermissions + 7005 + ",";
			}
			if (this.cbStoreSetting.Checked)
			{
				strPermissions = strPermissions + 14001 + ",";
			}
			if (this.cbStoresList.Checked)
			{
				strPermissions = strPermissions + 14002 + ",";
			}
			if (this.cbAddStores.Checked)
			{
				strPermissions = strPermissions + 14003 + ",";
			}
			if (this.cbStoreBalance.Checked)
			{
				strPermissions = strPermissions + 14004 + ",";
			}
			if (this.cbSendGoodOrders.Checked)
			{
				strPermissions = strPermissions + 14005 + ",";
			}
			if (this.cbHIPOSSetting.Checked)
			{
				strPermissions = strPermissions + 14009 + ",";
			}
			if (this.cbHIPOSBind.Checked)
			{
				strPermissions = strPermissions + 14010 + ",";
			}
			if (this.cbHIPOSDeal.Checked)
			{
				strPermissions = strPermissions + 14011 + ",";
			}
			if (this.cbMarketingImageList.Checked)
			{
				strPermissions = strPermissions + 14014 + ",";
			}
			if (this.cbMarktingList.Checked)
			{
				strPermissions = strPermissions + 14015 + ",";
			}
			if (this.cbTagList.Checked)
			{
				strPermissions = strPermissions + 14016 + ",";
			}
			if (this.cbStoreAppPushSet.Checked)
			{
				strPermissions = strPermissions + 14017 + ",";
			}
			if (this.cbStoreAppDownLoad.Checked)
			{
				strPermissions = strPermissions + 14013 + ",";
			}
			if (this.cbReferralRequest.Checked)
			{
				strPermissions = strPermissions + 15001 + ",";
			}
			if (this.cbReferrals.Checked)
			{
				strPermissions = strPermissions + 15002 + ",";
			}
			if (this.cbDeductSettings.Checked)
			{
				strPermissions = strPermissions + 15003 + ",";
			}
			if (this.cbReferralSettings.Checked)
			{
				strPermissions = strPermissions + 15004 + ",";
			}
			if (this.cbSplittinDrawRequest.Checked)
			{
				strPermissions = strPermissions + 15005 + ",";
			}
			if (this.cbSplittinDrawRecord.Checked)
			{
				strPermissions = strPermissions + 15006 + ",";
			}
			if (this.cbReferralGrades.Checked)
			{
				strPermissions += 15007;
			}
			if (this.cbAddReferralGrade.Checked)
			{
				strPermissions += 15008;
			}
			if (this.cbEditReferralGrade.Checked)
			{
				strPermissions += 15009;
			}
			if (this.cbDeleteReferralGrade.Checked)
			{
				strPermissions += 15010;
			}
			if (this.hasVstoreRight)
			{
				if (this.cbVServerConfig.Checked)
				{
					strPermissions = strPermissions + 6001 + ",";
				}
				if (this.cbVReplyOnKey.Checked)
				{
					strPermissions = strPermissions + 6002 + ",";
				}
				if (this.cbVManageMenu.Checked)
				{
					strPermissions = strPermissions + 6003 + ",";
				}
			}
			if (this.hasAliohRight)
			{
				if (this.cbAliohServerConfig.Checked)
				{
					strPermissions = strPermissions + 12003 + ",";
				}
				if (this.cbAliohManageMenu.Checked)
				{
					strPermissions = strPermissions + 12004 + ",";
				}
			}
			if (this.hasAppletRight)
			{
				if (this.cbAppletMessageTemplate.Checked)
				{
					strPermissions = strPermissions + 17003 + ",";
				}
				if (this.cbAppletProductSetting.Checked)
				{
					strPermissions = strPermissions + 17001 + ",";
				}
				if (this.cbAppletTempEdit.Checked)
				{
					strPermissions = strPermissions + 17004 + ",";
				}
				if (this.cbAppletPayConfig.Checked)
				{
					strPermissions = strPermissions + 17002 + ",";
				}
			}
			if (this.hasAppRight)
			{
				if (this.cbAppProductSetting.Checked)
				{
					strPermissions = strPermissions + 13003 + ",";
				}
				if (this.cbAppHomePageEdit.Checked)
				{
					strPermissions = strPermissions + 130016 + ",";
				}
				if (this.cbAppAndroidUpgrade.Checked)
				{
					strPermissions = strPermissions + 13005 + ",";
				}
				if (this.cbAPPIosUpgrade.Checked)
				{
					strPermissions = strPermissions + 13006 + ",";
				}
				if (this.cbAppAliPaySet.Checked)
				{
					strPermissions = strPermissions + 13007 + ",";
				}
				if (this.cbAppWeixinPay.Checked)
				{
					strPermissions = strPermissions + 13008 + ",";
				}
				if (this.cbAppShengPaySet.Checked)
				{
					strPermissions = strPermissions + 13009 + ",";
				}
				if (this.cbAppBankUnionSet.Checked)
				{
					strPermissions = strPermissions + 130011 + ",";
				}
				if (this.cbAppStartPageSet.Checked)
				{
					strPermissions = strPermissions + 130012 + ",";
				}
				if (this.cbAppLotteryDrawSet.Checked)
				{
					strPermissions = strPermissions + 130013 + ",";
				}
				if (this.cbAppPushSet.Checked)
				{
					strPermissions = strPermissions + 130014 + ",";
				}
				if (this.cbAppPushRecords.Checked)
				{
					strPermissions = strPermissions + 130015 + ",";
				}
			}
			if (this.hasSupplierRight)
			{
				if (this.cbSupplierList.Checked)
				{
					strPermissions = strPermissions + 16001 + ",";
				}
				if (this.cbAddSupplier.Checked)
				{
					strPermissions = strPermissions + 16002 + ",";
				}
				if (this.cbSupplierDetails.Checked)
				{
					strPermissions = strPermissions + 16004 + ",";
				}
				if (this.cbEditSupplier.Checked)
				{
					strPermissions = strPermissions + 16003 + ",";
				}
				if (this.cbSupplierAuditPdList.Checked)
				{
					strPermissions = strPermissions + 16100 + ",";
				}
				if (this.cbSupplierPdList.Checked)
				{
					strPermissions = strPermissions + 16101 + ",";
				}
				if (this.cbxSupplierAudit.Checked)
				{
					strPermissions = strPermissions + 16102 + ",";
				}
				if (this.cbxSupplierEditPd.Checked)
				{
					strPermissions = strPermissions + 16103 + ",";
				}
				if (this.cbSupplierOrderList.Checked)
				{
					strPermissions = strPermissions + 16200 + ",";
				}
				if (this.cbSupplierRefund.Checked)
				{
					strPermissions = strPermissions + 16201 + ",";
				}
				if (this.cbSupplierReturns.Checked)
				{
					strPermissions = strPermissions + 16202 + ",";
				}
				if (this.cbSupplierReplace.Checked)
				{
					strPermissions = strPermissions + 16203 + ",";
				}
				if (this.cbSupplierBalance.Checked)
				{
					strPermissions = strPermissions + 16300 + ",";
				}
				if (this.cbSupplierDrawList.Checked)
				{
					strPermissions = strPermissions + 16301 + ",";
				}
				if (this.cbSupplierBalanceOrder.Checked)
				{
					strPermissions = strPermissions + 16302 + ",";
				}
				if (this.cbSupplierBalanceDetail.Checked)
				{
					strPermissions = strPermissions + 16303 + ",";
				}
			}
			if (!string.IsNullOrEmpty(strPermissions))
			{
				strPermissions = strPermissions.Substring(0, strPermissions.LastIndexOf(","));
				ManagerHelper.ClearRolePrivilege(roleId);
				ManagerHelper.DeletePrivilegeByRoleId(roleId);
				ManagerHelper.AddPrivilegeInRoles(roleId, strPermissions);
			}
		}
	}
}
