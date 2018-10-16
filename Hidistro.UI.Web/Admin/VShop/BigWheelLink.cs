using Hidistro.Core;
using Hidistro.UI.Common.Controls;
using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.vshop
{
	public class BigWheelLink : AdminPage
	{
		private int ActivityID;

		protected HiddenField hidActivityID;

		protected HiddenField hidTypeId;

		protected HiddenField hidClientName;

		protected Label lblReferralsLink;

		protected Button btnDownLoad;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["ActivityID"], out this.ActivityID))
			{
				base.GotoResourceNotFound();
			}
			else
			{
				int num = 1;
				if (!int.TryParse(this.Page.Request.QueryString["typeid"], out num))
				{
					base.GotoResourceNotFound();
				}
				else
				{
					string text = "";
					if (!this.Page.IsPostBack)
					{
						this.hidActivityID.Value = this.ActivityID.ToString();
						this.hidTypeId.Value = num.ToString();
						this.hidClientName.Value = "vshop";
						switch (num)
						{
						case 1:
							text = Globals.HostPath(HttpContext.Current.Request.Url) + "/" + this.hidClientName.Value + "/BigWheel?ActivityID=" + this.ActivityID;
							break;
						case 2:
							text = Globals.HostPath(HttpContext.Current.Request.Url) + "/" + this.hidClientName.Value + "/Scratch?ActivityID=" + this.ActivityID;
							break;
						case 3:
							text = Globals.HostPath(HttpContext.Current.Request.Url) + "/" + this.hidClientName.Value + "/SmashEgg?ActivityID=" + this.ActivityID;
							break;
						}
						this.lblReferralsLink.Text = text;
					}
				}
			}
		}

		protected void btnDownLoad_Click(object sender, EventArgs e)
		{
			string str = "ActivitypicQRCode_" + this.hidActivityID.Value + ".png";
			string qrCodeUrl = "/Storage/master/QRCode/Activitypic_" + this.hidActivityID.Value + ".png";
			string text = "";
			switch (this.hidTypeId.Value.ToInt(0))
			{
			case 1:
				text = Globals.HostPath(HttpContext.Current.Request.Url) + "/" + this.hidClientName.Value + "/BigWheel?ActivityID=" + this.ActivityID;
				break;
			case 2:
				text = Globals.HostPath(HttpContext.Current.Request.Url) + "/" + this.hidClientName.Value + "/Scratch?ActivityID=" + this.ActivityID;
				break;
			case 3:
				text = Globals.HostPath(HttpContext.Current.Request.Url) + "/" + this.hidClientName.Value + "/SmashEgg?ActivityID=" + this.ActivityID;
				break;
			}
			string path = Globals.CreateQRCode(text.Contains("http://") ? text : ("http://" + text), qrCodeUrl, false, ImageFormats.Png);
			FileStream fileStream = new FileStream(base.Server.MapPath(path), FileMode.Open);
			byte[] array = new byte[(int)fileStream.Length];
			fileStream.Read(array, 0, array.Length);
			fileStream.Close();
			base.Response.ContentType = "application/octet-stream";
			base.Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(str, Encoding.UTF8));
			base.Response.BinaryWrite(array);
			base.Response.Flush();
			base.Response.End();
		}
	}
}
