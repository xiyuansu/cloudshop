using Hidistro.UI.Common.Controls;
using System;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class RedEnvelopeFinish : WAPTemplatedWebControl
	{
		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-RedEnvelopeFinish.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
		}
	}
}
