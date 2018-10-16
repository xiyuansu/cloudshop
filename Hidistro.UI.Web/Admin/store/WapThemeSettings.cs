using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.Web.Admin.store
{
	[PrivilegeCheck(Privilege.WapThemeSettings)]
	public class WapThemeSettings : AdminPage
	{
		protected HtmlGenericControl divwaptem1;

		protected HtmlImage imgCurrentImg;

		protected HtmlGenericControl div1;

		protected HtmlImage imgWapCode;

		protected HtmlAnchor apreview;

		protected Literal litWapUrl;

		protected Literal litThemeName;

		protected HtmlAnchor acurrentedit;

		protected Repeater repThemes;

		protected void Page_Load(object sender, EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (masterSettings.OpenVstore != 1 && masterSettings.OpenWap != 1 && masterSettings.OpenAliho != 1)
			{
				this.Page.Response.Redirect(Globals.GetAdminAbsolutePath("/AccessDenied?errormsg=抱歉，您暂未开通此服务！"), true);
			}
			if (!base.IsPostBack)
			{
				string wapTheme = masterSettings.WapTheme;
				IList<ManageThemeInfo> dataSource = this.LoadThemes(wapTheme);
				this.repThemes.DataSource = dataSource;
				this.repThemes.DataBind();
				string qrCodeUrl = "/Storage/master/QRCode/" + masterSettings.SiteUrl.ToLower().Replace("http://", "").Replace("https://", "") + "_wapshop.png";
				string text = masterSettings.SiteUrl.ToLower();
				string src = Globals.CreateQRCode((text.StartsWith("http://") || text.StartsWith("https://")) ? (text + "/wapshop") : ("http://" + text + "/wapshop"), qrCodeUrl, false, ImageFormats.Png);
				this.imgWapCode.Src = src;
				this.litWapUrl.Text = ((text.StartsWith("http://") || text.StartsWith("https://")) ? (text + "/wapshop") : ("http://" + text + "/wapshop"));
				this.apreview.HRef = ((text.StartsWith("http://") || text.StartsWith("https://")) ? (text + "/wapshop") : ("http://" + text + "/wapshop?fromSource=1"));
				this.acurrentedit.HRef = "ShopTempEdit?theme=" + wapTheme;
			}
		}

		protected IList<ManageThemeInfo> LoadThemes(string currentThemeName)
		{
			HttpContext context = HiContext.Current.Context;
			XmlDocument xmlDocument = new XmlDocument();
			IList<ManageThemeInfo> list = new List<ManageThemeInfo>();
			string path = context.Request.PhysicalApplicationPath + "\\Templates\\common\\home";
			string[] array = Directory.Exists(path) ? Directory.GetDirectories(path) : null;
			ManageThemeInfo manageThemeInfo = null;
			string[] array2 = array;
			foreach (string path2 in array2)
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(path2);
				string text = directoryInfo.Name.ToLower(CultureInfo.InvariantCulture);
				if (text.Length > 0 && !text.StartsWith("_"))
				{
					FileInfo[] files = directoryInfo.GetFiles("template.xml");
					FileInfo[] array3 = files;
					foreach (FileInfo fileInfo in array3)
					{
						manageThemeInfo = new ManageThemeInfo();
						FileStream fileStream = fileInfo.OpenRead();
						xmlDocument.Load(fileStream);
						fileStream.Close();
						manageThemeInfo.Name = xmlDocument.SelectSingleNode("root/Name").InnerText;
						manageThemeInfo.ThemeImgUrl = "/Templates/common/home/" + text + "/" + xmlDocument.SelectSingleNode("root/ImageUrl").InnerText;
						manageThemeInfo.ThemeName = text;
						if (string.Compare(manageThemeInfo.ThemeName, currentThemeName) == 0)
						{
							this.litThemeName.Text = manageThemeInfo.Name;
							this.imgCurrentImg.Src = "/Templates/common/home/" + text + "/" + xmlDocument.SelectSingleNode("root/ImageUrl").InnerText;
						}
						list.Add(manageThemeInfo);
					}
				}
			}
			return list;
		}

		protected void repThemes_ItemCommand(object source, RepeaterCommandEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				HiddenField hiddenField = e.Item.FindControl("hidThemeImgUrl") as HiddenField;
				HiddenField hiddenField2 = e.Item.FindControl("hidThemeName") as HiddenField;
				HiddenField hiddenField3 = e.Item.FindControl("hidDirName") as HiddenField;
				if (e.CommandName == "btnUse")
				{
					this.UserThems(hiddenField2.Value, hiddenField.Value, hiddenField3.Value);
					this.ShowMsg("成功修改了模板", true);
				}
			}
		}

		protected void UserThems(string name, string imgUrl, string dirName)
		{
			this.litThemeName.Text = name;
			this.imgCurrentImg.Src = imgUrl;
			this.acurrentedit.HRef = "ShopTempEdit?theme=" + dirName;
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			masterSettings.WapTheme = dirName;
			SettingsManager.Save(masterSettings);
		}
	}
}
