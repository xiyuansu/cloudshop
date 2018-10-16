using Hidistro.Context;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_OnlineServer : AscxTemplatedWebControl
	{
		private Repeater repOnlineService;

		private HtmlInputHidden hidPosition;

		private HtmlInputHidden hidYPostion;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Common_Comment/Skin-Common_OnlineServer.ascx";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (masterSettings.ServiceIsOpen == "1")
			{
				this.repOnlineService = (Repeater)this.FindControl("repOnlineService");
				this.hidPosition = (HtmlInputHidden)this.FindControl("hidPosition");
				this.hidYPostion = (HtmlInputHidden)this.FindControl("hidYPostion");
				if (this.hidPosition != null)
				{
					this.hidPosition.Value = masterSettings.ServicePosition.ToString();
				}
				if (this.hidYPostion != null)
				{
					this.hidYPostion.Value = masterSettings.ServiceCoordinate;
				}
				if (this.repOnlineService != null)
				{
					IList<OnlineServiceInfo> allOnlineService = OnlineServiceHelper.GetAllOnlineService(0, 1);
					IList<OnlineServiceInfo> allOnlineService2 = OnlineServiceHelper.GetAllOnlineService(0, 2);
					if (allOnlineService2 != null)
					{
						foreach (OnlineServiceInfo item in allOnlineService2)
						{
							allOnlineService.Add(item);
						}
					}
					this.repOnlineService.DataSource = allOnlineService;
					this.repOnlineService.DataBind();
				}
				else
				{
					Literal literal = (Literal)this.FindControl("litOnlineServer");
					literal.Text = HiContext.Current.SiteSettings.HtmlOnlineServiceCode;
				}
			}
			else
			{
				this.Visible = false;
			}
		}
	}
}
