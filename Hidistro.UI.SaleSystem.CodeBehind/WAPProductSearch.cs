using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Commodities;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WAPProductSearch : WAPTemplatedWebControl
	{
		private SiteSettings siteSettings = SettingsManager.GetMasterSettings();

		private int storeId = 0;

		protected override void OnInit(EventArgs e)
		{
			this.storeId = this.Page.Request.QueryString["storeId"].ToInt(0);
			if (this.SkinName == null)
			{
				if (this.siteSettings.OpenMultStore && this.storeId == 0 && this.siteSettings.Store_PositionRouteTo == 2.ToString())
				{
					this.SkinName = "Categories/skin-vCategoryTemplate0.html";
				}
				else
				{
					switch (this.siteSettings.VCategoryTemplateStatus)
					{
					case 1:
						this.SkinName = "Categories/skin-vCategoryTemplate1.html";
						break;
					case 2:
						this.SkinName = "Categories/skin-vCategoryTemplate2.html";
						break;
					case 3:
						this.SkinName = "Categories/skin-vCategoryTemplate3.html";
						break;
					default:
						this.SkinName = "Categories/skin-vCategoryTemplate0.html";
						break;
					}
				}
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			PageTitle.AddSiteNameTitle("分类搜索页");
			if (this.siteSettings.OpenMultStore && this.storeId == 0 && this.siteSettings.Store_PositionRouteTo == 2.ToString())
			{
				this.setTemplate0();
			}
			else
			{
				switch (this.siteSettings.VCategoryTemplateStatus)
				{
				case 1:
					break;
				case 2:
					break;
				case 3:
					break;
				default:
					this.setTemplate0();
					break;
				}
			}
		}

		private void setTemplate0()
		{
			Literal literal = (Literal)this.FindControl("ltlCategories");
			if (literal != null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				StringBuilder stringBuilder2 = new StringBuilder();
				IList<CategoryInfo> list = CatalogHelper.GetSubCategories(0).ToList();
				string text = (this.siteSettings.OpenMultStore && this.storeId == 0 && this.siteSettings.Store_PositionRouteTo == 2.ToString()) ? "StoreListSearch.aspx" : "ProductList.aspx";
				string text2 = (this.storeId > 0) ? ("&storeId=" + this.storeId) : "";
				foreach (CategoryInfo item in list)
				{
					string text3 = string.IsNullOrEmpty(item.BigImageUrl) ? "/templates/common/images/catedefault.jpg" : Globals.GetImageServerUrl("http://", item.BigImageUrl);
					stringBuilder.AppendFormat("<div class=\"s_group_img\"><div></div><img src=\"{0}\" style=\"width:100%;height:100%;\" onerror=\"javascript:this.src='/templates/common/images/catedefault.jpg'\" /><span><h3>{1}</h3><a href=\"{2}?categoryId={3}{4}\">查看全部</a></span></div>", text3, item.Name, text, item.CategoryId, text2);
					IList<CategoryInfo> list2 = CatalogHelper.GetSubCategories(item.CategoryId).ToList();
					int num = (list2.Count + 2) / 3;
					for (int i = 0; i < num; i++)
					{
						stringBuilder.AppendFormat("<div class=\"s_3\">");
						List<CategoryInfo> list3 = list2.Skip(i * 3).Take(3).ToList();
						foreach (CategoryInfo item2 in list3)
						{
							List<CategoryInfo> list4 = CatalogHelper.GetSubCategories(item2.CategoryId).ToList();
							StringBuilder stringBuilder3 = new StringBuilder();
							if (list4.Count > 0)
							{
								stringBuilder3.AppendFormat("<li><a href='{0}?categoryId={1}{2}'>全部</a></li>", text, item2.CategoryId, text2);
								foreach (CategoryInfo item3 in list4)
								{
									stringBuilder3.AppendFormat("<li><a href='{0}?categoryId={1}{2}'>{3}</a></li>", text, item3.CategoryId, text2, item3.Name);
								}
							}
							stringBuilder.AppendFormat("<span{0} pid=\"" + item2.CategoryId + "\" tid=\"" + item.CategoryId + "\"><h3>{1}{2}</h3>{3}</span>", (list4.Count > 0) ? " class=\"slid_down\"" : string.Format(" onclick=\"window.location.href={0}\"", "'" + text + "?categoryId=" + item2.CategoryId + text2 + "'"), item2.Name, (list4.Count > 0) ? "<i class=\"icon_slipup\" style=\"display:none;\"></i><i class=\"icon_slipdown\"></i>" : string.Empty, (list4.Count > 0) ? "<em></em>" : string.Empty);
							if (list4.Count > 0)
							{
								stringBuilder2.AppendFormat("<input type=\"hidden\" id=\"hidpid_{0}\" value=\"{1}\"/>", item2.CategoryId, stringBuilder3);
							}
						}
						stringBuilder.AppendFormat("<ul id=\"ulc_{0}\"></ul>", item.CategoryId);
						stringBuilder.Append("</div>");
					}
				}
				literal.Text = stringBuilder.ToString() + "\r\n" + stringBuilder2.ToString();
			}
		}
	}
}
