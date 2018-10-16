using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.store
{
	[PrivilegeCheck(Privilege.TopicManager)]
	public class WapTopicLink : AdminPage
	{
		private int TopicId;

		protected HiddenField hidTopicId;

		protected HiddenField hidClientName;

		protected Label lblReferralsLink;

		protected Button btnDownLoad;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["TopicId"], out this.TopicId))
			{
				base.GotoResourceNotFound();
			}
			else if (!this.Page.IsPostBack)
			{
				this.hidTopicId.Value = this.TopicId.ToString();
				TopicInfo topicInfo = VShopHelper.Gettopic(this.TopicId);
				if (topicInfo.TopicType == 2)
				{
					this.hidClientName.Value = "AppShop";
				}
				else if (topicInfo.TopicType == 1)
				{
					this.hidClientName.Value = "vshop";
				}
				else
				{
					this.hidClientName.Value = "pc";
				}
				string text = "";
				text = ((topicInfo.TopicType == 3) ? (Globals.HostPath(HttpContext.Current.Request.Url) + "/Topics?TopicId=" + this.TopicId) : (Globals.HostPath(HttpContext.Current.Request.Url) + "/" + this.hidClientName.Value + "/Topics?TopicId=" + this.TopicId));
				this.lblReferralsLink.Text = text;
			}
		}

		protected void btnDownLoad_Click(object sender, EventArgs e)
		{
			if (!(this.hidTopicId.Value == "pc"))
			{
				string str = "TopicQRCode_" + this.hidTopicId.Value + ".png";
				string qrCodeUrl = "/Storage/master/QRCode/Topic_" + this.hidTopicId.Value + ".png";
				string text = Globals.HostPath(HttpContext.Current.Request.Url) + "/" + this.hidClientName.Value + "/Topics?TopicId=" + this.hidTopicId.Value;
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
}
