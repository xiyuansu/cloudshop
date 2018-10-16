using Hidistro.Core;
using Hidistro.Core.Attributes;
using Hishop.Components.Validation.Validators;
using System;

namespace Hidistro.Context
{
	public class SiteSettings
	{
		public string AppletIndexImages
		{
			get;
			set;
		}

		public bool IsInitWXFansInteractData
		{
			get;
			set;
		}

		public bool IsInitWXFansData
		{
			get;
			set;
		}

		public string AppPromoteCouponList
		{
			get;
			set;
		}

		public bool IsOpenAppPromoteCoupons
		{
			get;
			set;
		}

		public bool EnableAppDownload
		{
			get;
			set;
		}

		public int ConsumeTimesInOneMonth
		{
			get;
			set;
		}

		public int ConsumeTimesInThreeMonth
		{
			get;
			set;
		}

		public int ConsumeTimesInSixMonth
		{
			get;
			set;
		}

		public bool EnableAppShake
		{
			get;
			set;
		}

		public string QQMapAPIKey
		{
			get;
			set;
		}

		public bool IsOpenCertification
		{
			get;
			set;
		}

		public int CertificationModel
		{
			get;
			set;
		}

		public string GaoDeAPIKey
		{
			get;
			set;
		}

		public string AppKey
		{
			get;
			set;
		}

		public string HiPOSMerchantId
		{
			get;
			set;
		}

		public string HiPOSAppId
		{
			get;
			set;
		}

		public string HiPOSAppSecret
		{
			get;
			set;
		}

		public string HiPOSSellerName
		{
			get;
			set;
		}

		public string HiPOSContactName
		{
			get;
			set;
		}

		public string HiPOSContactPhone
		{
			get;
			set;
		}

		public string HiPOSExpireAt
		{
			get;
			set;
		}

		public bool EnableHiPOSZFB
		{
			get;
			set;
		}

		public bool EnableHiPOSWX
		{
			get;
			set;
		}

		public string HiPOSZFBPID
		{
			get;
			set;
		}

		public string HiPOSZFBKey
		{
			get;
			set;
		}

		public string HiPOSWXAppId
		{
			get;
			set;
		}

		public string HiPOSWXMchId
		{
			get;
			set;
		}

		public string HiPOSWXAPIKey
		{
			get;
			set;
		}

		public string HiPOSWXCertPath
		{
			get;
			set;
		}

		public int ServiceStatus
		{
			get;
			set;
		}

		public int OpenTaobao
		{
			get;
			set;
		}

		public int OpenMobbile
		{
			get;
			set;
		}

		public int OpenAliho
		{
			get;
			set;
		}

		public int OpenWap
		{
			get;
			set;
		}

		public int OpenVstore
		{
			get;
			set;
		}

		public int OpenReferral
		{
			get;
			set;
		}

		public bool OpenSupplier
		{
			get;
			set;
		}

		public int VCategoryTemplateStatus
		{
			get;
			set;
		}

		public int AppCategoryTemplateStatus
		{
			get;
			set;
		}

		[StringLengthValidator(0, 128, Ruleset = "ValMasterSettings", MessageTemplate = "店铺域名必须控制在128个字符以内")]
		public string SiteUrl
		{
			get;
			set;
		}

		public string CheckCode
		{
			get;
			set;
		}

		public string LogoUrl
		{
			get;
			set;
		}

		[StringLengthValidator(0, 100, Ruleset = "ValMasterSettings", MessageTemplate = "简单介绍TITLE的长度限制在100字符以内")]
		public string SiteDescription
		{
			get;
			set;
		}

		[StringLengthValidator(1, 60, Ruleset = "ValMasterSettings", MessageTemplate = "店铺名称为必填项，长度限制在60字符以内")]
		public string SiteName
		{
			get;
			set;
		}

		public string Theme
		{
			get;
			set;
		}

		[GlobalCode("自定义页尾", IsHtmlCode = true)]
		public string Footer
		{
			get;
			set;
		}

		[GlobalCode("会员注册协议", IsHtmlCode = true)]
		public string RegisterAgreement
		{
			get;
			set;
		}

		[StringLengthValidator(0, 160, Ruleset = "ValMasterSettings", MessageTemplate = "搜索关键字META_KEYWORDS的长度限制在160字符以内")]
		public string SearchMetaKeywords
		{
			get;
			set;
		}

		[StringLengthValidator(0, 260, Ruleset = "ValMasterSettings", MessageTemplate = "店铺描述META_DESCRIPTION的长度限制在260字符以内")]
		[HtmlCoding]
		public string SearchMetaDescription
		{
			get;
			set;
		}

		public string EmailSender
		{
			get;
			set;
		}

		[GlobalCode("获取或设置邮件发送实例的配置信息", IsEncryption = true)]
		public string EmailSettings
		{
			get;
			set;
		}

		public bool EmailEnabled
		{
			get
			{
				return !string.IsNullOrEmpty(this.EmailSender) && !string.IsNullOrEmpty(this.EmailSettings) && this.EmailSender.Trim().Length > 0 && this.EmailSettings.Trim().Length > 0;
			}
		}

		public string SMSSender
		{
			get;
			set;
		}

		[GlobalCode("获取或设置手机短信发送实例的配置信息", IsEncryption = true)]
		public string SMSSettings
		{
			get;
			set;
		}

		public bool SMSEnabled
		{
			get
			{
				return !string.IsNullOrEmpty(this.SMSSender) && !string.IsNullOrEmpty(this.SMSSettings) && this.SMSSender.Trim().Length > 0 && this.SMSSettings.Trim().Length > 0;
			}
		}

		public int DecimalLength
		{
			get;
			set;
		}

		public string DefaultProductImage
		{
			get;
			set;
		}

		public string DefaultProductThumbnail1
		{
			get;
			set;
		}

		public string DefaultProductThumbnail2
		{
			get;
			set;
		}

		public string DefaultProductThumbnail3
		{
			get;
			set;
		}

		public string DefaultProductThumbnail4
		{
			get;
			set;
		}

		public string DefaultProductThumbnail5
		{
			get;
			set;
		}

		public string DefaultProductThumbnail6
		{
			get;
			set;
		}

		public string DefaultProductThumbnail7
		{
			get;
			set;
		}

		public string DefaultProductThumbnail8
		{
			get;
			set;
		}

		public int IPSMSCount
		{
			get;
			set;
		}

		public int PhoneSMSCount
		{
			get;
			set;
		}

		[RangeValidator(typeof(decimal), "0.1", RangeBoundaryType.Inclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset = "ValMasterSettings", MessageTemplate = "几元一积分必须在0.1-10000000之间")]
		public decimal PointsRate
		{
			get;
			set;
		}

		public int MemberRegistrationPoint
		{
			get;
			set;
		}

		public int SignInPoint
		{
			get;
			set;
		}

		public int ProductCommentPoint
		{
			get;
			set;
		}

		public int ContinuousDays
		{
			get;
			set;
		}

		public int ContinuousPoint
		{
			get;
			set;
		}

		public int ShoppingDeduction
		{
			get;
			set;
		}

		public int ShoppingDeductionRatio
		{
			get;
			set;
		}

		public bool CanPointUseWithCoupon
		{
			get;
			set;
		}

		[RangeValidator(1, RangeBoundaryType.Inclusive, 60, RangeBoundaryType.Inclusive, Ruleset = "ValMasterSettings", MessageTemplate = "限时抢购订单有效时间必须在1-60之间")]
		public int CountDownTime
		{
			get;
			set;
		}

		[RangeValidator(1, RangeBoundaryType.Inclusive, 90, RangeBoundaryType.Inclusive, Ruleset = "ValMasterSettings", MessageTemplate = "过期几天自动关闭订单的天数必须在1-90之间")]
		public int CloseOrderDays
		{
			get;
			set;
		}

		[RangeValidator(1, RangeBoundaryType.Inclusive, 90, RangeBoundaryType.Inclusive, Ruleset = "ValMasterSettings", MessageTemplate = "发货几天自动完成订单的天数必须在1-90之间")]
		public int FinishOrderDays
		{
			get;
			set;
		}

		[RangeValidator(1, RangeBoundaryType.Inclusive, 90, RangeBoundaryType.Inclusive, Ruleset = "ValMasterSettings", MessageTemplate = "订单完成几天自动结束交易的天数必须在1-90之间")]
		public int EndOrderDays
		{
			get;
			set;
		}

		[RangeValidator(typeof(decimal), "0", RangeBoundaryType.Inclusive, "100", RangeBoundaryType.Inclusive, Ruleset = "ValMasterSettings", MessageTemplate = "税率必须在0-100之间")]
		public decimal TaxRate
		{
			get;
			set;
		}

		public decimal RegReferralDeduct
		{
			get;
			set;
		}

		public decimal SubMemberDeduct
		{
			get;
			set;
		}

		public decimal SecondLevelDeduct
		{
			get;
			set;
		}

		public decimal ThreeLevelDeduct
		{
			get;
			set;
		}

		public bool SelfBuyDeduct
		{
			get;
			set;
		}

		public bool ShowDeductInProductPage
		{
			get;
			set;
		}

		public bool RegisterBecomePromoter
		{
			get;
			set;
		}

		public bool IsPromoterValidatePhone
		{
			get;
			set;
		}

		public string PromoterNeedInfo
		{
			get;
			set;
		}

		public string ExtendShareTitle
		{
			get;
			set;
		}

		public string ExtendSharePic
		{
			get;
			set;
		}

		public string ExtendShareDetail
		{
			get;
			set;
		}

		[GlobalCode("分销员介绍", IsHtmlCode = true)]
		public string ReferralIntroduction
		{
			get;
			set;
		}

		public decimal DeductMinDraw
		{
			get;
			set;
		}

		public string ReferralFilterIP
		{
			get;
			set;
		}

		public bool OpenReferralLog
		{
			get;
			set;
		}

		public bool IsOpenSecondLevelCommission
		{
			get;
			set;
		}

		public bool IsOpenThirdLevelCommission
		{
			get;
			set;
		}

		public decimal ApplyReferralNeedAmount
		{
			get;
			set;
		}

		public int ApplyReferralCondition
		{
			get;
			set;
		}

		public int AppDepletePoints
		{
			get;
			set;
		}

		public int AppFirstPrizeType
		{
			get;
			set;
		}

		public int AppSecondPrizeType
		{
			get;
			set;
		}

		public int AppThirdPrizeType
		{
			get;
			set;
		}

		public int AppFourPrizeType
		{
			get;
			set;
		}

		public int AppFirstPrizePoints
		{
			get;
			set;
		}

		public int AppSecondPrizePoints
		{
			get;
			set;
		}

		public int AppThirdPrizePoints
		{
			get;
			set;
		}

		public int AppFourPrizePoints
		{
			get;
			set;
		}

		public int AppFirstPrizeCouponId
		{
			get;
			set;
		}

		public int AppSecondPrizeCouponId
		{
			get;
			set;
		}

		public int AppThirdPrizeCouponId
		{
			get;
			set;
		}

		public int AppFourPrizeCouponId
		{
			get;
			set;
		}

		public int AppFirstPrizePercent
		{
			get;
			set;
		}

		public int AppSecondPrizePercent
		{
			get;
			set;
		}

		public int AppThirdPrizePercent
		{
			get;
			set;
		}

		public int AppFourPrizePercent
		{
			get;
			set;
		}

		[GlobalCode("网页在线客服代码", IsHtmlCode = true)]
		public string HtmlOnlineServiceCode
		{
			get;
			set;
		}

		public int ServicePosition
		{
			get;
			set;
		}

		public string ServiceCoordinate
		{
			get;
			set;
		}

		public string ServiceIsOpen
		{
			get;
			set;
		}

		public string ServicePhone
		{
			get;
			set;
		}

		public string MeiQiaAppId
		{
			get;
			set;
		}

		public string MeiQiaAppSecret
		{
			get;
			set;
		}

		public string MeiQiaUnitid
		{
			get;
			set;
		}

		public string MeiQiaUnit
		{
			get;
			set;
		}

		public string MeiQiaPassword
		{
			get;
			set;
		}

		public string MeiQiaUnitname
		{
			get;
			set;
		}

		public string MeiQiaActivated
		{
			get;
			set;
		}

		public bool EnableBulkPaymentAdvance
		{
			get;
			set;
		}

		public bool EnableBulkPaymentAliPay
		{
			get;
			set;
		}

		public bool EnableBulkPaymentWeixin
		{
			get;
			set;
		}

		public decimal MinimumSingleShot
		{
			get;
			set;
		}

		public bool EnabledCnzz
		{
			get;
			set;
		}

		public string CnzzUsername
		{
			get;
			set;
		}

		public string SiteMapTime
		{
			get;
			set;
		}

		public string SiteMapNum
		{
			get;
			set;
		}

		public string CnzzPassword
		{
			get;
			set;
		}

		public bool EnableShopMenu
		{
			get;
			set;
		}

		public string WapTheme
		{
			get;
			set;
		}

		public bool OpenMultStore
		{
			get;
			set;
		}

		public string WeixinAppId
		{
			get;
			set;
		}

		public string WeixinAppSecret
		{
			get;
			set;
		}

		public string WeixinToken
		{
			get;
			set;
		}

		public string WeixinPaySignKey
		{
			get;
			set;
		}

		public string WeixinPartnerID
		{
			get;
			set;
		}

		[GlobalCode("微信商户密钥", IsEncryption = true)]
		public string WeixinPartnerKey
		{
			get;
			set;
		}

		public string WeixinCertPath
		{
			get;
			set;
		}

		public string WeixinCertPassword
		{
			get;
			set;
		}

		public bool IsValidationService
		{
			get;
			set;
		}

		public string WeixinNumber
		{
			get;
			set;
		}

		public string WeixinLoginUrl
		{
			get;
			set;
		}

		public string WeiXinCodeImageUrl
		{
			get;
			set;
		}

		public bool EnableWeiXinRequest
		{
			get;
			set;
		}

		public bool EnableVAliPayCrossBorderSet
		{
			get;
			set;
		}

		public bool EnableWeixinWapAliPay
		{
			get;
			set;
		}

		public bool EnableBankUnionPay
		{
			get;
			set;
		}

		public bool OpenManyService
		{
			get;
			set;
		}

		public bool EnableVshopReferral
		{
			get;
			set;
		}

		public bool WeixinGuideAttention
		{
			get;
			set;
		}

		public bool IsOpenGiftCoupons
		{
			get;
			set;
		}

		public string GiftCouponList
		{
			get;
			set;
		}

		public string AppPushAppId
		{
			get;
			set;
		}

		public string AppPushAppKey
		{
			get;
			set;
		}

		public string AppPushMasterSecret
		{
			get;
			set;
		}

		public bool EnableAppPushSetOrderSend
		{
			get;
			set;
		}

		public bool EnableAppPushSetOrderRefund
		{
			get;
			set;
		}

		public bool EnableAppPushSetOrderReturn
		{
			get;
			set;
		}

		public bool EnableAppAliPay
		{
			get;
			set;
		}

		public bool EnableAppWapAliPay
		{
			get;
			set;
		}

		public bool EnableAppShengPay
		{
			get;
			set;
		}

		public bool EnableAPPBankUnionPay
		{
			get;
			set;
		}

		public string AndroidStartImg
		{
			get;
			set;
		}

		public string IOSStartImg
		{
			get;
			set;
		}

		public string AppWxAppId
		{
			get;
			set;
		}

		public string AppWxMchId
		{
			get;
			set;
		}

		public string AppWxAppSecret
		{
			get;
			set;
		}

		[GlobalCode("APP微信商户密钥", IsEncryption = true)]
		public string AppWxPartnerKey
		{
			get;
			set;
		}

		public string AppWxCertPath
		{
			get;
			set;
		}

		public string AppWxCertPass
		{
			get;
			set;
		}

		public bool OpenAppWxPay
		{
			get;
			set;
		}

		public string AppIOSDownLoadUrl
		{
			get;
			set;
		}

		public string AppAndroidDownLoadUrl
		{
			get;
			set;
		}

		public bool EnableWapAliPay
		{
			get;
			set;
		}

		public bool EnableWapWeiXinPay
		{
			get;
			set;
		}

		public bool EnableWapShengPay
		{
			get;
			set;
		}

		public bool EnableWapAliPayCrossBorder
		{
			get;
			set;
		}

		public string AliOHAppId
		{
			get;
			set;
		}

		public string AliOHFollowRelay
		{
			get;
			set;
		}

		public string AliOHFollowRelayTitle
		{
			get;
			set;
		}

		public string AliOHServerUrl
		{
			get;
			set;
		}

		public bool IsSurportEmail
		{
			get;
			set;
		}

		public bool IsSurportPhone
		{
			get;
			set;
		}

		public bool IsNeedValidEmail
		{
			get;
			set;
		}

		public string RegistExtendInfo
		{
			get;
			set;
		}

		public int OpenImgCheckPhoneCode
		{
			get;
			set;
		}

		public string GeetestId
		{
			get;
			set;
		}

		public string GeetestKey
		{
			get;
			set;
		}

		public bool IsOpenGeetest
		{
			get;
			set;
		}

		public string Main_Mch_ID
		{
			get;
			set;
		}

		public string Main_AppId
		{
			get;
			set;
		}

		public bool OrderPayToShipper
		{
			get;
			set;
		}

		public bool AutoRedirectClient
		{
			get;
			set;
		}

		public bool AutoAllotOrder
		{
			get;
			set;
		}

		public bool StoreNeedTakeCode
		{
			get;
			set;
		}

		public string StoreAppPushMasterSecret
		{
			get;
			set;
		}

		public string StoreAppPushAppKey
		{
			get;
			set;
		}

		public string StoreAppPushAppId
		{
			get;
			set;
		}

		public string JDAppKey
		{
			get;
			set;
		}

		public string JDAppSecret
		{
			get;
			set;
		}

		public string JDAccessToken
		{
			get;
			set;
		}

		public bool EnableTax
		{
			get;
			set;
		}

		public bool OpenMultReferral
		{
			get;
			set;
		}

		public bool OpenRecruitmentAgreement
		{
			get;
			set;
		}

		[GlobalCode("分销员协议", IsHtmlCode = true)]
		public string RecruitmentAgreement
		{
			get;
			set;
		}

		public long AppHomeTopicVersionCode
		{
			get;
			set;
		}

		public long XcxHomeVersionCode
		{
			get;
			set;
		}

		public bool IsDemoSite
		{
			get;
			set;
		}

		[RangeValidator(1, RangeBoundaryType.Inclusive, 90, RangeBoundaryType.Inclusive, Ruleset = "ValMasterSettings", MessageTemplate = "订单完成几天自动好评的天数必须在1-90之间")]
		public int EndOrderDaysEvaluate
		{
			get;
			set;
		}

		public bool Store_IsMemberVisitBelongStore
		{
			get;
			set;
		}

		public bool Store_IsOrderInClosingTime
		{
			get;
			set;
		}

		public bool Store_IsRecommend
		{
			get;
			set;
		}

		public string Store_PositionRouteTo
		{
			get;
			set;
		}

		public string Store_PositionNoMatchTo
		{
			get;
			set;
		}

		public string Store_BannerInfo
		{
			get;
			set;
		}

		public bool IsOpenPickeupInStore
		{
			get;
			set;
		}

		public string PickeupInStoreRemark
		{
			get;
			set;
		}

		public int HomePageTopicId
		{
			get;
			set;
		}

		public bool IsOpenRechargeGift
		{
			get;
			set;
		}

		public DateTime LastSendCouponsMessengerTime
		{
			get;
			set;
		}

		public bool OpenWxApplet
		{
			get;
			set;
		}

		public bool OpenWxAppletWxPay
		{
			get;
			set;
		}

		public string WxAppletAppId
		{
			get;
			set;
		}

		public string WxAppletAppSecrect
		{
			get;
			set;
		}

		public string WxApplectMchId
		{
			get;
			set;
		}

		public string WxApplectKey
		{
			get;
			set;
		}

		public string WxApplectPayCert
		{
			get;
			set;
		}

		public string WxApplectPayCertPassword
		{
			get;
			set;
		}

		public bool OpenPcShop
		{
			get;
			set;
		}

		public string StoreAppAndroidUrl
		{
			get;
			set;
		}

		public string StoreAppIOSUrl
		{
			get;
			set;
		}

		public int UserAddressMaxCount
		{
			get;
			set;
		}

		public decimal StoreAppVersion
		{
			get;
			set;
		}

		public string StoreAppDescription
		{
			get;
			set;
		}

		public bool IsAutoDealRefund
		{
			get;
			set;
		}

		public string O2OAppletAppId
		{
			get;
			set;
		}

		public string O2OAppletAppSecrect
		{
			get;
			set;
		}

		public string O2OAppletMchId
		{
			get;
			set;
		}

		public string O2OAppletKey
		{
			get;
			set;
		}

		public string O2OAppletPayCert
		{
			get;
			set;
		}

		public string O2OAppletPayCertPassword
		{
			get;
			set;
		}

		public bool OpenO2OAppletWxPay
		{
			get;
			set;
		}

		public bool OpenDadaLogistics
		{
			get;
			set;
		}

		public string DadaAppKey
		{
			get;
			set;
		}

		public string DadaAppSecret
		{
			get;
			set;
		}

		public string DadaSourceID
		{
			get;
			set;
		}

		public bool IsForceAttention
		{
			get;
			set;
		}

		public bool SplittinDraws_CashToDeposit
		{
			get;
			set;
		}

		public bool SplittinDraws_CashToWeiXin
		{
			get;
			set;
		}

		public bool SplittinDraws_CashToALiPay
		{
			get;
			set;
		}

		public bool SplittinDraws_CashToBankCard
		{
			get;
			set;
		}

		public bool OpenBalancePay
		{
			get;
			set;
		}

		public bool OpenWXO2OApplet
		{
			get;
			set;
		}

		public bool EnableVATInvoice
		{
			get;
			set;
		}

		public bool EnableE_Invoice
		{
			get;
			set;
		}

		[RangeValidator(typeof(decimal), "0", RangeBoundaryType.Inclusive, "100", RangeBoundaryType.Inclusive, Ruleset = "ValMasterSettings", MessageTemplate = "税率必须在0-100之间")]
		public decimal VATTaxRate
		{
			get;
			set;
		}

		public int VATInvoiceDays
		{
			get;
			set;
		}

		public int MemberBirthDaySetting
		{
			get;
			set;
		}

		public bool FightGroupActivitiyAutoAllotOrder
		{
			get;
			set;
		}

		public bool FitGroupIsOpenPickeupInStore
		{
			get;
			set;
		}

		public string InstallDate
		{
			get;
			set;
		}

		public bool UserLoginIsForceBindingMobbile
		{
			get;
			set;
		}

		public bool QuickLoginIsForceBindingMobbile
		{
			get;
			set;
		}

		public string AppAuditAPIUrl
		{
			get;
			set;
		}

		public string AppWX_Main_AppId
		{
			get;
			set;
		}

		public string AppWX_Main_MchID
		{
			get;
			set;
		}

		public string PolyapiAppId
		{
			get;
			set;
		}

		public string PolyapiKey
		{
			get;
			set;
		}

		public bool AppWxPayOpenServiceMode
		{
			get;
			set;
		}

		public SiteSettings()
		{
			this.SiteDescription = "最安全，最专业的网上商店系统";
			this.Theme = "default";
			this.WapTheme = "default";
			this.SiteName = "Hishop";
			this.LogoUrl = "/utility/pics/logo.jpg";
			this.DefaultProductImage = "/utility/pics/none.gif";
			this.DefaultProductThumbnail1 = "/utility/pics/none.gif";
			this.DefaultProductThumbnail2 = "/utility/pics/none.gif";
			this.DefaultProductThumbnail3 = "/utility/pics/none.gif";
			this.DefaultProductThumbnail4 = "/utility/pics/none.gif";
			this.DefaultProductThumbnail5 = "/utility/pics/none.gif";
			this.DefaultProductThumbnail6 = "/utility/pics/none.gif";
			this.DefaultProductThumbnail7 = "/utility/pics/none.gif";
			this.DefaultProductThumbnail8 = "/utility/pics/none.gif";
			this.DecimalLength = 2;
			this.PointsRate = decimal.One;
			this.MemberRegistrationPoint = 0;
			this.SignInPoint = 0;
			this.ProductCommentPoint = 0;
			this.ContinuousDays = 0;
			this.ContinuousPoint = 0;
			this.ShoppingDeduction = 0;
			this.ShoppingDeductionRatio = 0;
			this.CanPointUseWithCoupon = true;
			this.CountDownTime = 40;
			this.CloseOrderDays = 3;
			this.FinishOrderDays = 7;
			this.EndOrderDays = 7;
			this.ServiceStatus = 1;
			this.OpenAliho = 0;
			this.OpenTaobao = 1;
			this.OpenMobbile = 0;
			this.OpenVstore = 0;
			this.OpenWap = 0;
			this.OpenReferral = 0;
			this.OpenSupplier = false;
			this.VCategoryTemplateStatus = 0;
			this.AppCategoryTemplateStatus = 0;
			this.OpenManyService = false;
			this.WeixinGuideAttention = false;
			this.IsOpenGiftCoupons = false;
			this.GiftCouponList = "";
			this.DeductMinDraw = 50m;
			this.ReferralFilterIP = "113.108.80.206";
			this.OpenReferralLog = true;
			this.RegReferralDeduct = decimal.Zero;
			this.SubMemberDeduct = decimal.Zero;
			this.SecondLevelDeduct = decimal.Zero;
			this.ThreeLevelDeduct = decimal.Zero;
			this.SelfBuyDeduct = true;
			this.ShowDeductInProductPage = true;
			this.RegisterBecomePromoter = false;
			this.IsPromoterValidatePhone = true;
			this.PromoterNeedInfo = "";
			this.ExtendShareTitle = "";
			this.ExtendSharePic = "";
			this.ExtendShareDetail = "";
			this.ServiceCoordinate = "120";
			this.ServicePosition = 1;
			this.ServiceIsOpen = "1";
			this.ServicePhone = "";
			this.MeiQiaAppId = string.Empty;
			this.MeiQiaAppSecret = string.Empty;
			this.MeiQiaUnitid = string.Empty;
			this.MeiQiaUnit = string.Empty;
			this.MeiQiaPassword = string.Empty;
			this.MeiQiaUnitname = string.Empty;
			this.MeiQiaActivated = "0";
			this.EnableBulkPaymentAliPay = false;
			this.EnableBulkPaymentWeixin = false;
			this.EnableBulkPaymentAdvance = true;
			this.MinimumSingleShot = decimal.One;
			this.EnableVshopReferral = false;
			this.EnableBankUnionPay = false;
			this.EnableAPPBankUnionPay = false;
			this.WeixinCertPath = "";
			this.WeixinCertPassword = "";
			this.IOSStartImg = "";
			this.AndroidStartImg = "";
			this.OpenMultStore = false;
			this.AppDepletePoints = 0;
			this.AppFirstPrizeType = 1;
			this.AppFirstPrizeCouponId = 0;
			this.AppFirstPrizePoints = 0;
			this.AppFirstPrizePercent = 0;
			this.AppSecondPrizeType = 1;
			this.AppSecondPrizeCouponId = 0;
			this.AppSecondPrizePoints = 0;
			this.AppSecondPrizePercent = 0;
			this.AppThirdPrizeType = 1;
			this.AppThirdPrizeCouponId = 0;
			this.AppThirdPrizePoints = 0;
			this.AppThirdPrizePercent = 0;
			this.AppFourPrizeType = 1;
			this.AppFourPrizeCouponId = 0;
			this.AppFourPrizePoints = 0;
			this.AppFourPrizePercent = 0;
			this.OrderPayToShipper = true;
			this.Main_Mch_ID = "";
			this.Main_AppId = "";
			this.AppWxAppId = "";
			this.AppWxMchId = "";
			this.AppWxAppSecret = "";
			this.OpenAppWxPay = false;
			this.AppWxPartnerKey = "";
			this.EnableShopMenu = false;
			this.AppAndroidDownLoadUrl = "";
			this.AppIOSDownLoadUrl = "";
			this.PhoneSMSCount = 5;
			this.IPSMSCount = 10;
			this.IsSurportEmail = true;
			this.IsSurportPhone = false;
			this.IsNeedValidEmail = false;
			this.RegistExtendInfo = string.Empty;
			this.OpenImgCheckPhoneCode = 1;
			this.AutoRedirectClient = true;
			this.QQMapAPIKey = "MF5BZ-AQV6F-3TQJ5-J6TQJ-QHE2K-MKBK4";
			this.AutoAllotOrder = false;
			this.StoreNeedTakeCode = true;
			this.StoreAppPushMasterSecret = "";
			this.StoreAppPushAppKey = "";
			this.GaoDeAPIKey = "";
			this.EnableAppShake = false;
			this.EnableTax = false;
			this.OpenMultReferral = false;
			this.OpenRecruitmentAgreement = false;
			this.RecruitmentAgreement = "";
			this.AliOHServerUrl = "https://openapi.alipay.com/gateway.do";
			this.EnableShopMenu = true;
			this.AppHomeTopicVersionCode = 1000L;
			this.XcxHomeVersionCode = 1000L;
			this.GeetestId = "d282cc6dc51a9a68fb495fa6d31b820f";
			this.GeetestKey = "a4daf0e1ab70f92b6b79a15745f80cdd ";
			this.IsOpenGeetest = false;
			this.IsDemoSite = false;
			this.EndOrderDaysEvaluate = 7;
			this.ConsumeTimesInOneMonth = 1;
			this.ConsumeTimesInThreeMonth = 1;
			this.ConsumeTimesInSixMonth = 1;
			this.IsOpenAppPromoteCoupons = false;
			this.EnableAppDownload = false;
			this.AppPromoteCouponList = "";
			this.IsInitWXFansData = false;
			this.IsInitWXFansInteractData = false;
			this.HomePageTopicId = 0;
			this.IsOpenRechargeGift = false;
			this.Store_PositionNoMatchTo = "Platform";
			this.Store_PositionRouteTo = "Platform";
			this.Store_IsRecommend = false;
			this.Store_IsOrderInClosingTime = false;
			this.Store_IsMemberVisitBelongStore = false;
			this.IsOpenPickeupInStore = false;
			this.AppletIndexImages = "";
			this.LastSendCouponsMessengerTime = DateTime.Now.AddDays(-1.0);
			this.OpenWxApplet = false;
			this.OpenPcShop = true;
			this.UserAddressMaxCount = 20;
			this.StoreAppVersion = 0.0m;
			this.IsAutoDealRefund = false;
			this.SplittinDraws_CashToDeposit = true;
			this.OpenBalancePay = false;
			this.OpenO2OAppletWxPay = false;
			this.EnableVATInvoice = false;
			this.EnableE_Invoice = false;
			this.VATTaxRate = decimal.Zero;
			this.VATInvoiceDays = 0;
			this.InstallDate = "";
		}
	}
}
