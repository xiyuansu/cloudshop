using Hidistro.UI.Common.Controls;
using System;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_Footer : AscxTemplatedWebControl
	{
		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/HomeTags/Skin-Common_Footer.ascx";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
		}
	}
}
