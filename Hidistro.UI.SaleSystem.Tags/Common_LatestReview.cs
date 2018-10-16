using Hidistro.SaleSystem.Catalog;
using Hidistro.UI.Common.Controls;
using System;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_LatestReview : ThemedTemplatedRepeater
	{
		private int maxNum = 6;

		public int MaxNum
		{
			get
			{
				return this.maxNum;
			}
			set
			{
				this.maxNum = value;
			}
		}

		protected override void OnInit(EventArgs e)
		{
			base.DataSource = ProductBrowser.GetProductReviews(this.maxNum);
			base.DataBind();
		}
	}
}
