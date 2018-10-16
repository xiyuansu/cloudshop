using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Members;
using System;
using System.Web;
using System.Web.UI;

namespace Hidistro.UI.Common.Controls
{
	[PersistChildren(true)]
	[ParseChildren(false)]
	public class WapHeadContainer : Control
	{
		protected override void Render(HtmlTextWriter writer)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			string domainName = Globals.DomainName;
			string text = HttpContext.Current.Request.UserAgent;
			bool flag = false;
			flag = (masterSettings.OpenWap == 1 && true);
			if (string.IsNullOrEmpty(text))
			{
				text = "";
			}
			if (text.ToLower().IndexOf("micromessenger") > -1 && masterSettings.OpenVstore == 1)
			{
				flag = true;
			}
			HiContext current = HiContext.Current;
			bool flag2 = false;
			if (masterSettings.OpenMultStore)
			{
				flag2 = true;
			}
			string imageServerUrl = Globals.GetImageServerUrl();
			writer.Write("<script language=\"javascript\" type=\"text/javascript\"> \r\n                                var HasWapRight = {0};\r\n                                var IsOpenStores = {1};\r\n                                var ClientPath = \"{2}\";\r\n                                var ImageServerUrl = \"{3}\";\r\n                                var ImageUploadPath = \"{4}\";\r\n                                var StoreDefaultPage = \"{5}\";\r\n                                var qqMapAPIKey = \"{6}\";\r\n                            </script>", flag ? "true" : "false", flag2.ToString().ToLower(), HiContext.Current.GetClientPath, imageServerUrl, string.IsNullOrEmpty(imageServerUrl) ? "/admin/UploadHandler.ashx?action=newupload" : "/admin/UploadHandler.ashx?action=remoteupdateimages", masterSettings.Store_PositionRouteTo, string.IsNullOrEmpty(masterSettings.QQMapAPIKey) ? "SYJBZ-DSLR3-IWX3Q-3XNTM-ELURH-23FTP" : masterSettings.QQMapAPIKey);
			string text2 = HttpContext.Current.Request.Url.ToString().ToLower();
			if ((text2.Contains("/groupbuyproductdetails") || text2.Contains("/countdownproductsdetails") || (text2.Contains("/productdetails") && !text2.Contains("/appshop")) || text2.Contains("/membercenter") || text2.Contains("/membergroupdetails") || text2.Contains("/membergroupdetailsstatus") || text2.Contains("/fightgroupactivitydetails") || text2.Contains("/fightgroupactivitydetailssoon") || text2.Contains("/fightgroupdetails") || text2.Contains("/productlist") || text2.Contains("/default") || text2.Contains("/storehome") || text2.Contains("/storelist") || text2.Contains("/storeproductdetails") || text2.Contains("/presaleproductdetails") || text2.Contains("countdownstoreproductsdetails")) && masterSettings.MeiQiaActivated == "1")
			{
				string empty = string.Empty;
				string empty2 = string.Empty;
				int productId = 0;
				string text3 = masterSettings.MeiQiaUnitid.ToNullString();
				string text4 = string.Empty;
				string text5 = string.Empty;
				string empty3 = string.Empty;
				string text6 = string.Empty;
				string empty4 = string.Empty;
				string empty5 = string.Empty;
				string empty6 = string.Empty;
				string empty7 = string.Empty;
				string empty8 = string.Empty;
				string text7 = string.Empty;
				string text8 = string.Empty;
				string text9 = string.Empty;
				string text10 = string.Empty;
				string empty9 = string.Empty;
				string empty10 = string.Empty;
				string text11 = string.Empty;
				string text12 = string.Empty;
				string text13 = string.Empty;
				string text14 = string.Empty;
				string text15 = string.Empty;
				MemberInfo user = HiContext.Current.User;
				if (user != null)
				{
					text4 = user.RealName.ToNullString();
					empty8 = text4;
					empty7 = user.UserName.ToNullString();
					empty5 = ((user.Picture == null) ? "" : (masterSettings.SiteUrl + user.Picture));
					text5 = ((user.Gender != Gender.Female) ? ((user.Gender != Gender.Male) ? "保密" : "男") : "女");
					empty3 = user.BirthDate.ToNullString();
					object obj;
					if (!user.BirthDate.HasValue)
					{
						obj = "";
					}
					else
					{
						DateTime dateTime = DateTime.Now;
						int year = dateTime.Year;
						dateTime = user.BirthDate.Value;
						obj = (year - dateTime.Year).ToString();
					}
					text6 = (string)obj;
					text7 = user.CellPhone.ToNullString();
					text8 = user.Email.ToNullString();
					text9 = RegionHelper.GetFullRegion(user.RegionId, "", true, 0) + user.Address;
					text10 = user.QQ.ToNullString();
					text11 = user.WeChat.ToNullString();
					text12 = user.Wangwang.ToNullString();
					DateTime createDate = user.CreateDate;
					text13 = ((user.CreateDate < new DateTime(1000, 1, 1)) ? "" : user.CreateDate.ToNullString());
					MemberGradeInfo memberGrade = MemberHelper.GetMemberGrade(user.GradeId);
					text14 = ((memberGrade == null) ? "" : memberGrade.Name.ToNullString());
				}
				if (int.TryParse(this.Page.Request.QueryString["productId"], out productId))
				{
					SiteSettings masterSettings2 = SettingsManager.GetMasterSettings();
					ProductInfo productSimpleInfo = ProductBrowser.GetProductSimpleInfo(productId);
					if (productSimpleInfo != null && productSimpleInfo.SaleStatus != 0)
					{
						text15 = ",'商品名称': '{0}'\r\n                                    ,'售价': '{1}'\r\n                                    ,'市场价': '{2}'\r\n                                    ,'品牌': '{3}'\r\n                                    ,'商品编号': '{4}'\r\n                                    ,'商品货号': '{5}'\r\n                                    ,'浏览次数': '{6}'\r\n                                    ,'重量': '{7}'\r\n                                    ,'已经出售': '{8}'";
						string empty11 = string.Empty;
						empty11 = ((!(productSimpleInfo.MinSalePrice == productSimpleInfo.MaxSalePrice)) ? (productSimpleInfo.MinSalePrice.F2ToString("f2") + " - " + productSimpleInfo.MaxSalePrice.F2ToString("f2")) : productSimpleInfo.MinSalePrice.F2ToString("f2"));
						string empty12 = string.Empty;
						empty12 = ((!(productSimpleInfo.Weight > decimal.Zero)) ? "无" : string.Format("{0} g", productSimpleInfo.Weight.F2ToString("f2")));
						string obj2 = string.Empty;
						if (productSimpleInfo.BrandId.HasValue)
						{
							BrandCategoryInfo brandCategory = CatalogHelper.GetBrandCategory(productSimpleInfo.BrandId.Value);
							if (brandCategory != null)
							{
								obj2 = brandCategory.BrandName;
							}
						}
						text15 = string.Format(text15, productSimpleInfo.ProductName.ToNullString(), empty11, productSimpleInfo.MarketPrice.ToNullString(), obj2.ToNullString(), productSimpleInfo.ProductCode.ToNullString(), productSimpleInfo.SKU.ToNullString(), productSimpleInfo.VistiCounts.ToNullString(), empty12, productSimpleInfo.ShowSaleCounts.ToNullString());
					}
				}
				empty = "<script type='text/javascript'>\r\n                                    (function (m, ei, q, i, a, j, s) {\r\n                                        m[a] = m[a] || function () {\r\n                                            (m[a].a = m[a].a || []).push(arguments)\r\n                                        };\r\n                                        j = ei.createElement(q),\r\n                                            s = ei.getElementsByTagName(q)[0];\r\n                                        j.async = true;\r\n                                        j.charset = 'UTF-8';\r\n                                        j.src = i;\r\n                                        s.parentNode.insertBefore(j, s)\r\n                                    })(window, document, 'script', '//eco-api.meiqia.com/dist/meiqia.js', '_MEIQIA');\r\n                                    _MEIQIA('entId', " + text3 + ");\r\n                                    _MEIQIA('metadata', { \r\n                                                address: '" + text9 + "', // 地址\r\n                                                age: '" + text6 + "', // 年龄\r\n                                                comment: '" + empty6 + "', // 备注\r\n                                                email: '" + text8 + "', // 邮箱\r\n                                                gender: '" + text5 + "', // 性别\r\n                                                name: '" + text4 + "', // 名字\r\n                                                qq: '" + text10 + "', // QQ\r\n                                                tel: '" + text7 + "', // 电话\r\n                                                weibo: '" + empty9 + "', // 微博\r\n                                                weixin: '" + empty10 + "', // 微信 \r\n                                                '会员等级': '" + text14 + "',\r\n                                                'MSN': '" + text11 + "',\r\n                                                '旺旺': '" + text12 + "',\r\n                                                '账号创建时间': '" + text13 + "' " + text15 + "\r\n                                    });\r\n                                </script>";
				writer.Write(empty);
			}
			writer.WriteLine();
		}

		private void RenderMetaGenerator(HtmlTextWriter writer)
		{
			writer.WriteLine("<meta name=\"GENERATOR\" content=\"" + HiContext.Current.Config.Version + "\" />");
		}

		private void RenderFavicon(HtmlTextWriter writer)
		{
			string arg = Globals.FullPath("favicon.ico");
			writer.WriteLine("<link rel=\"icon\" type=\"image/x-icon\" href=\"{0}\" media=\"screen\" />", arg);
			writer.WriteLine("<link rel=\"shortcut icon\" type=\"image/x-icon\" href=\"{0}\" media=\"screen\" />", arg);
		}

		private void RenderMetaAuthor(HtmlTextWriter writer)
		{
			writer.WriteLine("<meta name=\"author\" content=\"Hishop development team\" />");
		}

		private void RenderMetaLanguage(HtmlTextWriter writer)
		{
			writer.WriteLine("<meta http-equiv=\"content-language\" content=\"zh-CN\" />");
		}
	}
}
