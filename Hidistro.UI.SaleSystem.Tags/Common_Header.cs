using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Members;
using Hidistro.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_Header : AscxTemplatedWebControl
	{
		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/HomeTags/Skin-Common_Header.ascx";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (masterSettings.MeiQiaActivated == "1")
			{
				string text = HttpContext.Current.Request.Url.ToString().ToLower();
				if (!text.Contains("/userdefault") && !text.Contains("/browse/category") && !text.Contains("/ss/category") && !text.Contains("/product_detail") && !text.Contains("/default") && !text.Contains("/productdetails") && !text.Contains("/groupbuyproductdetails") && !text.Contains("/countdownproductsdetails") && !text.Contains("/presaleproductdetails") && !text.Contains("/fightgroupactivitydetails"))
				{
					return;
				}
				string empty = string.Empty;
				int productId = 0;
				string text2 = masterSettings.MeiQiaUnitid.ToNullString();
				string text3 = string.Empty;
				string text4 = string.Empty;
				string empty2 = string.Empty;
				string text5 = string.Empty;
				string empty3 = string.Empty;
				string empty4 = string.Empty;
				string empty5 = string.Empty;
				string empty6 = string.Empty;
				string empty7 = string.Empty;
				string text6 = string.Empty;
				string text7 = string.Empty;
				string text8 = string.Empty;
				string text9 = string.Empty;
				string empty8 = string.Empty;
				string empty9 = string.Empty;
				string text10 = string.Empty;
				string text11 = string.Empty;
				string text12 = string.Empty;
				string text13 = string.Empty;
				string text14 = string.Empty;
				MemberInfo user = HiContext.Current.User;
				if (user != null)
				{
					text3 = user.UserName.ToNullString();
					empty7 = text3;
					empty6 = user.UserName.ToNullString();
					empty4 = ((user.Picture == null) ? "" : (masterSettings.SiteUrl + user.Picture));
					text4 = ((user.Gender != Gender.Female) ? ((user.Gender != Gender.Male) ? "保密" : "男") : "女");
					empty2 = user.BirthDate.ToNullString();
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
						obj = (year - dateTime.Year).ToNullString();
					}
					text5 = (string)obj;
					text6 = user.CellPhone.ToNullString();
					text7 = user.Email.ToNullString();
					text8 = RegionHelper.GetFullRegion(user.RegionId, "", true, 0).ToNullString() + user.Address.ToNullString();
					text9 = user.QQ.ToNullString();
					text10 = user.WeChat.ToNullString();
					text11 = user.Wangwang.ToNullString();
					DateTime createDate = user.CreateDate;
					text12 = ((user.CreateDate == DateTime.MinValue) ? "" : user.CreateDate.ToNullString());
					MemberGradeInfo memberGrade = MemberHelper.GetMemberGrade(user.GradeId);
					text13 = ((memberGrade == null) ? "" : memberGrade.Name.ToNullString());
				}
				if (int.TryParse(this.Page.Request.QueryString["productId"], out productId))
				{
					SiteSettings masterSettings2 = SettingsManager.GetMasterSettings();
					ProductInfo productSimpleInfo = ProductBrowser.GetProductSimpleInfo(productId);
					if (productSimpleInfo != null && productSimpleInfo.SaleStatus != 0)
					{
						text14 = ",'商品名称': '{0}'\r\n                                    ,'售价': '{1}'\r\n                                    ,'市场价': '{2}'\r\n                                    ,'品牌': '{3}'\r\n                                    ,'商品编号': '{4}'\r\n                                    ,'商品货号': '{5}'\r\n                                    ,'浏览次数': '{6}'\r\n                                    ,'重量': '{7}'\r\n                                    ,'已经出售': '{8}'";
						string empty10 = string.Empty;
						empty10 = ((!(productSimpleInfo.MinSalePrice == productSimpleInfo.MaxSalePrice)) ? (productSimpleInfo.MinSalePrice.F2ToString("f2") + " - " + productSimpleInfo.MaxSalePrice.F2ToString("f2")) : productSimpleInfo.MinSalePrice.F2ToString("f2"));
						string empty11 = string.Empty;
						empty11 = ((!(productSimpleInfo.Weight > decimal.Zero)) ? "无" : $"{productSimpleInfo.Weight} g");
						string obj2 = string.Empty;
						if (productSimpleInfo.BrandId.HasValue)
						{
							BrandCategoryInfo brandCategory = CatalogHelper.GetBrandCategory(productSimpleInfo.BrandId.Value);
							if (brandCategory != null)
							{
								obj2 = brandCategory.BrandName;
							}
						}
						text14 = string.Format(text14, productSimpleInfo.ProductName.ToNullString(), empty10, productSimpleInfo.MarketPrice.ToNullString(), obj2.ToNullString(), productSimpleInfo.ProductCode.ToNullString(), productSimpleInfo.SKU.ToNullString(), productSimpleInfo.VistiCounts.ToNullString(), empty11, productSimpleInfo.ShowSaleCounts.ToNullString());
					}
				}
				empty = "<script type='text/javascript'>\r\n                                    var homePageTopicId = " + masterSettings.HomePageTopicId + ";\r\n                                    (function (m, ei, q, i, a, j, s) {\r\n                                        m[a] = m[a] || function () {\r\n                                            (m[a].a = m[a].a || []).push(arguments)\r\n                                        };\r\n                                        j = ei.createElement(q),\r\n                                            s = ei.getElementsByTagName(q)[0];\r\n                                        j.async = true;\r\n                                        j.charset = 'UTF-8';\r\n                                        j.src = i;\r\n                                        s.parentNode.insertBefore(j, s)\r\n                                    })(window, document, 'script', '//eco-api.meiqia.com/dist/meiqia.js', '_MEIQIA');\r\n                                    _MEIQIA('entId', " + text2 + ");\r\n                                    _MEIQIA('metadata', { \r\n                                                address: '" + text8 + "', // 地址\r\n                                                age: '" + text5 + "', // 年龄\r\n                                                comment: '" + empty5 + "', // 备注\r\n                                                email: '" + text7 + "', // 邮箱\r\n                                                gender: '" + text4 + "', // 性别\r\n                                                name: '" + text3 + "', // 名字\r\n                                                qq: '" + text9 + "', // QQ\r\n                                                tel: '" + text6 + "', // 电话\r\n                                                weibo: '" + empty8 + "', // 微博\r\n                                                weixin: '" + empty9 + "', // 微信 \r\n                                                '会员等级': '" + text13 + "',\r\n                                                'MSN': '" + text10 + "',\r\n                                                '旺旺': '" + text11 + "',\r\n                                                '账号创建时间': '" + text12 + "' " + text14 + "\r\n                                    });\r\n                                </script>";
				Literal literal = (Literal)this.FindControl("MeiQia_OnlineServer");
				if (literal != null)
				{
					literal.Text = empty;
				}
			}
			else
			{
				string text15 = "<script type='text/javascript'>\r\n                                    var homePageTopicId = " + masterSettings.HomePageTopicId + ";</script>";
				Literal literal2 = (Literal)this.FindControl("MeiQia_OnlineServer");
				if (literal2 != null)
				{
					literal2.Text = text15;
				}
			}
		}
	}
}
