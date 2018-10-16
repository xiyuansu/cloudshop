using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Promotions;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class WAPMyGiftDetailInfo : WAPMemberTemplatedWebControl
	{
		private Literal litMarketPrice;

		private Literal litDescript;

		private Literal litName;

		private Image imgPrize;

		private HtmlAnchor goBuy;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-MyGiftDetailInfo.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			int num = this.Page.Request["RecordId"].ToInt(0);
			if (num <= 0)
			{
				base.GotoResourceNotFound("记录不存在！");
			}
			else
			{
				UserAwardRecordsInfo userAwardRecordsInfo = ActivityHelper.GetUserAwardRecordsInfo(num);
				if (userAwardRecordsInfo == null)
				{
					base.GotoResourceNotFound("记录不存在！");
				}
				else if (userAwardRecordsInfo.PrizeType != 3)
				{
					base.GotoResourceNotFound("记录不存在！");
				}
				else
				{
					this.litMarketPrice = (Literal)this.FindControl("litMarketPrice");
					this.litDescript = (Literal)this.FindControl("litDescript");
					this.imgPrize = (Image)this.FindControl("imgPrize");
					this.goBuy = (HtmlAnchor)this.FindControl("goBuy");
					this.litName = (Literal)this.FindControl("litName");
					if (userAwardRecordsInfo.Status == 2)
					{
						this.goBuy.Visible = false;
					}
					else
					{
						this.goBuy.HRef = "SubmmitOrder.aspx?from=prize&RecordId=" + num;
					}
					GiftInfo giftDetails = GiftHelper.GetGiftDetails(userAwardRecordsInfo.PrizeValue);
					if (giftDetails != null)
					{
						this.litName.Text = giftDetails.Name;
						if (giftDetails.MarketPrice.HasValue)
						{
							this.litMarketPrice.Text = "市场参考价：" + giftDetails.MarketPrice.Value.F2ToString("f2");
						}
						else
						{
							this.litMarketPrice.Visible = false;
						}
						this.litDescript.Text = giftDetails.LongDescription;
						this.imgPrize.ImageUrl = giftDetails.ImageUrl;
						PageTitle.AddSiteNameTitle("奖品详情");
					}
					else
					{
						base.GotoResourceNotFound("礼品已被删除！");
					}
				}
			}
		}
	}
}
