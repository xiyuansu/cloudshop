using Hidistro.UI.Common.Controls;
using System;
using System.ComponentModel;
using System.Web.UI;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class HomePage : NewWAPTemplateWebControl
	{
		[Bindable(true)]
		public string TempFilePath
		{
			get;
			set;
		}

		protected override void OnInit(EventArgs e)
		{
			this.TempFilePath = "Skin-HomePage.html";
			if (this.SkinName == null)
			{
				this.SkinName = this.TempFilePath;
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
		}
	}
}
