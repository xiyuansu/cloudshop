using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WapCouponsList : WAPTemplatedWebControl
	{
		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-couponslist.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
		}
	}
}
