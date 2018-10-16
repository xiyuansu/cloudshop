using Hidistro.UI.Common.Controls;
using System;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_AppHeader : AppshopTemplatedWebControl
	{
		private bool _CheckLogin = true;

		public bool checkLogin
		{
			get
			{
				return this._CheckLogin;
			}
			set
			{
				this._CheckLogin = value;
			}
		}

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				if (this.checkLogin)
				{
					this.SkinName = "tags/skin-Common_Header.html";
				}
				else
				{
					this.SkinName = "tags/skin-Common_NoLoginHeader.html";
				}
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
		}
	}
}
