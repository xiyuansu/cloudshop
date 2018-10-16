using Hidistro.Core;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.ChoicePage
{
	public class CPGifts : AdminPage
	{
		protected string filterGiftIds;

		protected string giftName;

		protected HiddenField hidFilterGiftIds;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.filterGiftIds = this.Page.Request["giftIds"].ToNullString();
			this.LoadParameters();
		}

		private void LoadParameters()
		{
			this.hidFilterGiftIds.Value = this.filterGiftIds;
		}
	}
}
