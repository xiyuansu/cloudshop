using Hidistro.Core;
using Hidistro.Core.Helper;
using Hidistro.Entities;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WAPStoreListSearch : WAPTemplatedWebControl
	{
		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-StoreListSearch.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			base.CheckOpenMultStore();
			StoreEntityQuery storeEntityQuery = null;
			string cookie = WebHelper.GetCookie("UserCoordinateCookie");
			if (!string.IsNullOrEmpty(cookie))
			{
				storeEntityQuery = new StoreEntityQuery();
				storeEntityQuery.RegionId = WebHelper.GetCookie("UserCoordinateCookie", "CityRegionId").ToInt(0);
				storeEntityQuery.AreaId = WebHelper.GetCookie("UserCoordinateCookie", "RegionId").ToInt(0);
				if (storeEntityQuery.RegionId == 0 || storeEntityQuery.AreaId == 0)
				{
					this.Page.Response.Redirect("StoreList?from");
				}
			}
			else
			{
				this.Page.Response.Redirect("StoreList?from");
			}
		}
	}
}
