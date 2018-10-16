using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WAPNoStoreTip : WAPTemplatedWebControl
	{
		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-NoStoreTip.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
		}
	}
}
