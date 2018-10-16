using Hidistro.Context;
using Hidistro.Core;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace demo
{
	public class GetCaptcha : Page
	{
		protected HtmlForm form1;

		protected void Page_Load(object sender, EventArgs e)
		{
			base.Response.ContentType = "application/json";
			base.Response.Write(this.getCaptcha());
			base.Response.End();
		}

		private string getCaptcha()
		{
			GeetestLib geetestLib = new GeetestLib(SettingsManager.GetMasterSettings().GeetestKey, SettingsManager.GetMasterSettings().GeetestId);
			string userID = "mec";
			byte b = geetestLib.preProcess(userID);
			HiCache.Insert("gt_server_status", b);
			return geetestLib.getResponseStr();
		}
	}
}
