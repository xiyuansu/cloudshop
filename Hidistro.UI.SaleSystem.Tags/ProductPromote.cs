using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Catalog;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class ProductPromote : WebControl
	{
		public const string TagID = "ProductPromote";

		public override string ID
		{
			get
			{
				return base.ID;
			}
			set
			{
			}
		}

		public int ProductId
		{
			get;
			set;
		}

		public bool IsAnonymous
		{
			get;
			set;
		}

		public bool IsShowUrl
		{
			get;
			set;
		}

		public bool IsHasSendGift
		{
			get;
			set;
		}

		public ProductPromote()
		{
			base.ID = "ProductPromote";
		}

		protected override void Render(HtmlTextWriter writer)
		{
			PromotionInfo promotion = ProductBrowser.GetProductPromotionInfo(this.ProductId);
			string promsg = string.Empty;
			if (promotion != null)
			{
				promsg += "<span>";
				string text = "#";
				if (this.IsShowUrl)
				{
					text = base.GetRouteUrl("FavourableDetails", new
					{
						activityId = promotion.ActivityId
					});
				}
				switch (promotion.PromoteType)
				{
				case PromoteType.SentGift:
				{
					IList<GiftInfo> giftDetailsByGiftIds = ProductBrowser.GetGiftDetailsByGiftIds(promotion.GiftIds);
					if (giftDetailsByGiftIds.Count > 0)
					{
						promsg += "<div id=\"divGift\"><i class=\"tag2\">赠</i>";
						giftDetailsByGiftIds.ForEach(delegate(GiftInfo giftinfo)
						{
							promsg += string.Format("<em><a href=\"{0}\"><img src=\"{1}\" title=\"{2}\"></a></em><b> ×1 </b>", base.GetRouteUrl("FavourableDetails", new
							{
								activityId = promotion.ActivityId
							}), giftinfo.ThumbnailUrl40, giftinfo.Name);
						});
						promsg += "</div>";
						this.IsHasSendGift = true;
					}
					break;
				}
				case PromoteType.SentProduct:
					promsg += string.Format("<div id=\"divGift\"><i class=\"tag2\">赠</i><b>{0}</b></div>", (promotion.Name.Length > 40) ? (promotion.Name.Substring(0, 40) + "...") : promotion.Name);
					break;
				}
				promsg += "</span>";
				writer.Write(promsg);
			}
		}
	}
}
