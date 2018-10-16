using Hidistro.UI.Common.Controls;
using System;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_Search : AscxTemplatedWebControl
	{
		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/HomeTags/Common_Search/Skin-Common_Search.ascx";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
		}
	}
}
