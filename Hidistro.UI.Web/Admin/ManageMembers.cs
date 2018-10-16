using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Ionic.Zlib;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Members)]
	public class ManageMembers : AdminPage
	{
		private string searchKey;

		private string realName;

		private int? rankId;

		private int RegisteredSource = 0;

		private bool? approved;

		private string orderby = "Expenditure";

		private string tagId;

		private string userGroupType;

		private DateTime? startDate;

		private DateTime? endDate;

		protected HiddenField hidUserGroupType;

		protected HiddenField hidIsOpenApp;

		protected CalendarPanel cldStartDate;

		protected CalendarPanel cldEndDate;

		protected MemberGradeDropDownList rankList;

		protected MemberSourceDropDownList sourceList;

		protected DropDownList ddlMemberTags;

		protected ExportFieldsCheckBoxList exportFieldsCheckBoxList;

		protected ExportFormatRadioButtonList exportFormatRadioButtonList;

		protected DropDownList dropSortBy;

		protected PageSizeDropDownList PageSizeDropDownList;

		protected Literal litsmscount;

		protected HtmlTextArea txtmsgcontent;

		protected HtmlTextArea txtemailcontent;

		protected HtmlTextArea txtsitecontent;

		protected HtmlInputHidden hdenablemsg;

		protected HtmlInputHidden hdenableemail;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.LoadParameters();
			if (!this.Page.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["orderBy"]))
				{
					this.dropSortBy.SelectedValue = this.Page.Request.QueryString["orderBy"];
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["GradeId"]))
				{
					this.rankId = this.Page.Request.QueryString["GradeId"].ToInt(0);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["RegisteredSource"]))
				{
					this.RegisteredSource = this.Page.Request.QueryString["RegisteredSource"].ToInt(0);
				}
				var dataSource = from item in MemberTagHelper.GetAllTags()
				select new
				{
					item.TagId,
					item.TagName
				};
				this.ddlMemberTags.DataSource = dataSource;
				this.ddlMemberTags.DataTextField = "TagName";
				this.ddlMemberTags.DataValueField = "TagId";
				this.ddlMemberTags.DataBind();
				this.ddlMemberTags.Items.Insert(0, new ListItem("请选择标签", ""));
				int num;
				if (this.Page.Request.QueryString["TagsId"].ToInt(0) > 0)
				{
					DropDownList dropDownList = this.ddlMemberTags;
					num = this.Page.Request.QueryString["TagsId"].ToInt(0);
					dropDownList.SelectedValue = num.ToString();
				}
				this.rankList.DataBind();
				this.rankList.SelectedValue = this.rankId;
				this.sourceList.DataBind();
				this.sourceList.SelectedValue = this.RegisteredSource;
				SiteSettings siteSetting = this.GetSiteSetting();
				if (siteSetting.SMSEnabled)
				{
					Literal literal = this.litsmscount;
					num = this.GetAmount(siteSetting);
					literal.Text = num.ToString();
					this.hdenablemsg.Value = "1";
				}
				if (siteSetting.EmailEnabled)
				{
					this.hdenableemail.Value = "1";
				}
				HiddenField hiddenField = this.hidIsOpenApp;
				num = siteSetting.OpenMobbile;
				hiddenField.Value = num.ToString();
				foreach (ListItem item in this.exportFieldsCheckBoxList.Items)
				{
					item.Attributes.Add("data-field", item.Value);
					item.Attributes.Add("data-header", item.Text);
					item.Attributes.Add("data-type", "field");
					item.Selected = true;
				}
			}
		}

		private SiteSettings GetSiteSetting()
		{
			return SettingsManager.GetMasterSettings();
		}

		private void LoadParameters()
		{
			if (!this.Page.IsPostBack)
			{
				int value = 0;
				if (int.TryParse(this.Page.Request.QueryString["rankId"], out value))
				{
					this.rankId = value;
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["searchKey"]))
				{
					this.searchKey = base.Server.UrlDecode(this.Page.Request.QueryString["searchKey"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["realName"]))
				{
					this.realName = base.Server.UrlDecode(this.Page.Request.QueryString["realName"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Approved"]))
				{
					this.approved = Convert.ToBoolean(this.Page.Request.QueryString["Approved"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["RegisteredSource"]))
				{
					int.TryParse(this.Page.Request.QueryString["RegisteredSource"], out this.RegisteredSource);
				}
				this.orderby = this.Page.Request.QueryString["OrderBy"].ToNullString().ToLower();
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["tagId"]))
				{
					this.tagId = base.Server.UrlDecode(this.Page.Request.QueryString["tagId"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["userGroupType"]))
				{
					this.userGroupType = base.Server.UrlDecode(this.Page.Request.QueryString["userGroupType"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StartDate"]))
				{
					this.startDate = base.Server.UrlDecode(this.Page.Request.QueryString["StartDate"]).ToDateTime();
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["EndDate"]))
				{
					this.endDate = base.Server.UrlDecode(this.Page.Request.QueryString["EndDate"]).ToDateTime();
				}
				int num = 0;
				foreach (ListItem item in this.dropSortBy.Items)
				{
					if (this.orderby != item.Value)
					{
						num++;
					}
				}
				if (num >= this.dropSortBy.Items.Count)
				{
					this.orderby = this.dropSortBy.Items[0].Value;
				}
				this.dropSortBy.SelectedValue = this.orderby;
				this.rankList.SelectedValue = this.rankId;
				this.ddlMemberTags.SelectedValue = this.tagId;
				this.hidUserGroupType.Value = this.userGroupType;
				this.cldStartDate.SelectedDate = this.startDate;
				this.cldEndDate.SelectedDate = this.endDate;
			}
			else
			{
				this.rankId = this.rankList.SelectedValue;
				this.RegisteredSource = (this.sourceList.SelectedValue.HasValue ? this.sourceList.SelectedValue.Value : 0);
				this.tagId = this.ddlMemberTags.SelectedValue;
				this.userGroupType = this.hidUserGroupType.Value;
				this.startDate = this.cldStartDate.SelectedDate;
				this.endDate = this.cldEndDate.SelectedDate;
			}
		}

		protected int GetAmount(SiteSettings settings)
		{
			int result = 0;
			if (!string.IsNullOrEmpty(settings.SMSSettings))
			{
				string xml = HiCryptographer.TryDecypt(settings.SMSSettings);
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(xml);
				string innerText = xmlDocument.SelectSingleNode("xml/Appkey").InnerText;
				string postData = "method=getAmount&Appkey=" + innerText;
				string text = this.PostData("http://sms.huz.cn/getAmount.aspx", postData);
				int num = default(int);
				if (int.TryParse(text, out num))
				{
					result = Convert.ToInt32(text);
				}
			}
			return result;
		}

		public new string PostData(string url, string postData)
		{
			string result = string.Empty;
			try
			{
				Uri requestUri = new Uri(url);
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUri);
				Encoding uTF = Encoding.UTF8;
				byte[] bytes = uTF.GetBytes(postData);
				httpWebRequest.Method = "POST";
				httpWebRequest.ContentType = "application/x-www-form-urlencoded";
				httpWebRequest.ContentLength = bytes.Length;
				using (Stream stream = httpWebRequest.GetRequestStream())
				{
					stream.Write(bytes, 0, bytes.Length);
				}
				using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
				{
					using (Stream stream2 = httpWebResponse.GetResponseStream())
					{
						Encoding uTF2 = Encoding.UTF8;
						Stream stream3 = stream2;
						if (httpWebResponse.ContentEncoding.ToLower() == "gzip")
						{
							stream3 = new GZipStream(stream2, CompressionMode.Decompress);
						}
						else if (httpWebResponse.ContentEncoding.ToLower() == "deflate")
						{
							stream3 = new DeflateStream(stream2, CompressionMode.Decompress);
						}
						using (StreamReader streamReader = new StreamReader(stream3, uTF2))
						{
							result = streamReader.ReadToEnd();
						}
					}
				}
			}
			catch (Exception ex)
			{
				result = $"获取信息错误：{ex.Message}";
			}
			return result;
		}
	}
}
