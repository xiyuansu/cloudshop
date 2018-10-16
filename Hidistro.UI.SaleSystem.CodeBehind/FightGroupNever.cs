using Hidistro.Context;
using Hidistro.Entities;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class FightGroupNever : WAPTemplatedWebControl
	{
		private Literal literrorInfo;

		protected override void OnInit(EventArgs e)
		{
			if (base.ClientType == ClientType.VShop)
			{
				if (this.SkinName == null)
				{
					this.SkinName = "skin-FightGroupNever.html";
				}
				base.OnInit(e);
			}
			else
			{
				HiContext.Current.Context.Response.Redirect("OnlyWXOpenTip");
			}
		}

		protected override void AttachChildControls()
		{
		}
	}
}
