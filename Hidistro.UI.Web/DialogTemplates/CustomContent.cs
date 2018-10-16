using Hidistro.Context;
using Hidistro.Core;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Web.Admin.Ascx;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Xml;

namespace Hidistro.UI.Web.DialogTemplates
{
	public class CustomContent : Page
	{
		protected HtmlLink cssLink;

		protected HtmlForm form1;

		protected Ueditor editDescription;

		protected ImageList ImageList;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.Page.IsPostBack && !string.IsNullOrEmpty(base.Request.QueryString["id"].ToString()) && !string.IsNullOrEmpty(base.Request.QueryString["type"].ToString()) && !string.IsNullOrEmpty(base.Request.QueryString["name"].ToString()))
			{
				string name = base.Request.QueryString["name"].ToString();
				XmlNode xmlNode = this.BindHtml(base.Request.QueryString["id"].ToString(), base.Request.QueryString["type"].ToString());
				if (xmlNode != null && xmlNode.Attributes[name] != null)
				{
					this.editDescription.Text = Globals.HtmlDecode(Globals.HtmlDecode(xmlNode.Attributes[name].Value));
				}
			}
		}

		protected override void OnInit(EventArgs e)
		{
			if (HiContext.Current.Manager == null)
			{
				this.Page.Response.Redirect(Globals.GetAdminAbsolutePath("login.aspx"), true);
			}
		}

		private XmlNode BindHtml(string controId, string type)
		{
			XmlNode result = null;
			if (controId.Contains("_"))
			{
				string xmlname = controId.Split('_')[0];
				int id = Convert.ToInt32(controId.Split('_')[1].ToString());
				result = this.FindXmlNode(xmlname, id, type);
			}
			return result;
		}

		private XmlNode FindXmlNode(string xmlname, int Id, string type)
		{
			XmlNode result = null;
			if (!(xmlname == "ads"))
			{
				if (xmlname == "products")
				{
					result = TagsHelper.FindProductNode(Id, type);
				}
			}
			else
			{
				result = TagsHelper.FindAdNode(Id, type);
			}
			return result;
		}
	}
}
