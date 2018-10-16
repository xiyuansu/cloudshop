using Hidistro.UI.Common.Controls;
using System;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_CopyShippingAddress : AscxTemplatedWebControl
	{
		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Common_SubmmintOrder/Skin-Common_CopyShippingAddress.ascx";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
		}
	}
}
