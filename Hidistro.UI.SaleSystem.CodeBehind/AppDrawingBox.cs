using Hidistro.Context;
using Hidistro.Entities;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class AppDrawingBox : AppshopMemberTemplatedWebControl
	{
		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-DrawingBox.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			if (HiContext.Current.User != null)
			{
				AppshopTemplatedRepeater appshopTemplatedRepeater = (AppshopTemplatedRepeater)this.FindControl("rptDrawList");
				IList<AppLotteryDraw> list = (IList<AppLotteryDraw>)(appshopTemplatedRepeater.DataSource = APPHelper.GetAppLotteryDraw(HiContext.Current.UserId));
				appshopTemplatedRepeater.DataBind();
				PageTitle.AddSiteNameTitle("中奖记录");
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
		}
	}
}
