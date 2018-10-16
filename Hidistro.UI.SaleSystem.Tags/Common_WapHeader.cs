using Hidistro.Entities;
using Hidistro.UI.Common.Controls;
using System;
using System.Web;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_WapHeader : WAPTemplatedWebControl
	{
		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "tags/skin-Common_Header.html";
			}
			string text = HttpContext.Current.Request.Url.ToString().ToLower();
			if (text.Contains("/vshop/"))
			{
				base.ClientType = ClientType.VShop;
			}
			else if (text.Contains("/alioh/"))
			{
				base.ClientType = ClientType.AliOH;
			}
			else
			{
				base.ClientType = ClientType.WAP;
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
		}
	}
}
