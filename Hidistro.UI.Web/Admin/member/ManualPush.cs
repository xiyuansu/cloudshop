using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.member
{
	public class ManualPush : AdminPage
	{
		public class DataForm
		{
			public string PushContent
			{
				get;
				set;
			}

			public string PushTitle
			{
				get;
				set;
			}

			public string PushTag
			{
				get;
				set;
			}

			public string PushSendType
			{
				get;
				set;
			}

			public string PushSendDate
			{
				get;
				set;
			}

			public string PushSendDateHours
			{
				get;
				set;
			}

			public string PushType
			{
				get;
				set;
			}

			public string PushContext
			{
				get;
				set;
			}

			public string PushUrl
			{
				get;
				set;
			}

			public string SendUserNames
			{
				get;
				set;
			}

			public string SendUserIds
			{
				get;
				set;
			}
		}

		private static object objLock = new object();

		private string topicId;

		private string productId;

		private string formData;

		private string userIds;

		private string userNames;

		protected ScriptManager ScriptManager1;

		protected HiddenField hidUserIds;

		protected RadioButtonList rblPushType;

		protected Label lblPushType;

		protected TextBox txtURL;

		protected Label lblShow;

		protected HtmlInputButton btnSelectTopic;

		protected HtmlInputButton btnSelectProduct;

		protected RadioButtonList rblActivity;

		protected TextBox txtPushTitle;

		protected TextBox txtPushContent;

		protected Button btnSend;

		protected HtmlInputHidden locationUrl;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.userIds = this.Page.Request.QueryString["userIds"];
			this.topicId = this.Page.Request["topicId"].ToNullString();
			this.productId = this.Page.Request["productId"].ToNullString();
			this.formData = this.Page.Request["formData"].ToNullString();
			this.SetDefault();
			this.SetDateControl();
			if (!base.IsPostBack)
			{
				this.hidUserIds.Value = this.userIds;
				this.BindPushType();
				this.BindActivity();
				this.BindPushSendType();
				if (!string.IsNullOrEmpty(this.topicId))
				{
					this.BindTopic();
					this.BindFormData();
				}
				if (!string.IsNullOrEmpty(this.productId))
				{
					this.BindProduct();
					this.BindFormData();
				}
			}
			this.SetDefault();
		}

		private void SetDateControl()
		{
		}

		private void SetDefault()
		{
			EnumPushType enumPushType = (EnumPushType)this.rblPushType.SelectedValue.ToInt(0);
			this.lblPushType.Text = $"<em>*</em>{((Enum)(object)enumPushType).ToDescription()}：";
			this.txtURL.Style["display"] = "none";
			this.rblActivity.Style["display"] = "none";
			this.lblShow.Style["display"] = "none";
			this.btnSelectTopic.Style["display"] = "none";
			this.btnSelectProduct.Style["display"] = "none";
			switch (enumPushType)
			{
			case EnumPushType.Activity:
				this.rblActivity.Style["display"] = "block";
				break;
			case EnumPushType.Link:
				this.txtURL.Style["display"] = "block";
				break;
			case EnumPushType.ProductTopic:
				this.lblShow.Style["display"] = "block";
				this.btnSelectTopic.Style["display"] = "block";
				break;
			case EnumPushType.Product:
				this.lblShow.Style["display"] = "block";
				this.btnSelectProduct.Style["display"] = "block";
				break;
			}
		}

		private void BindFormData()
		{
			if (!string.IsNullOrEmpty(this.formData))
			{
				DataForm dataForm = JsonHelper.ParseFormJson<DataForm>(this.formData);
				this.txtPushContent.Text = dataForm.PushContent;
				this.txtURL.Text = dataForm.PushUrl;
				this.txtPushTitle.Text = dataForm.PushTitle;
				this.hidUserIds.Value = dataForm.SendUserIds;
				this.rblPushType.SelectedValue = dataForm.PushType;
			}
		}

		private void BindProduct()
		{
			this.lblPushType.Text = $"<em>*</em>{((Enum)(object)EnumPushType.Product).ToDescription()}：";
			IList<int> list = null;
			Dictionary<int, IList<int>> dictionary = default(Dictionary<int, IList<int>>);
			ProductInfo productDetails = ProductHelper.GetProductDetails(this.productId.ToInt(0), out dictionary, out list);
			if (productDetails != null)
			{
				this.lblShow.Text = productDetails.ProductName;
				this.txtURL.Style["display"] = "none";
			}
		}

		private void BindPushSendType()
		{
		}

		private void BindActivity()
		{
			if (this.rblActivity.Items.Count <= 0)
			{
				foreach (EnumActivity value in Enum.GetValues(typeof(EnumActivity)))
				{
					ListItem obj = new ListItem
					{
						Text = ((Enum)(object)value).ToDescription()
					};
					int num = (int)value;
					obj.Value = num.ToString();
					ListItem item = obj;
					this.rblActivity.Items.Add(item);
				}
				this.rblActivity.Style["display"] = "none";
				this.rblActivity.SelectedIndex = 0;
			}
		}

		private void BindTopic()
		{
			this.lblPushType.Text = $"<em>*</em>{((Enum)(object)EnumPushType.ProductTopic).ToDescription()}：";
			TopicInfo topicInfo = VShopHelper.Gettopic(this.topicId.ToInt(0));
			if (topicInfo != null)
			{
				this.lblShow.Text = topicInfo.Title;
				this.txtURL.Style["display"] = "none";
			}
		}

		private void BindPushType()
		{
			if (this.rblPushType.Items.Count <= 0)
			{
				foreach (EnumPushType value in Enum.GetValues(typeof(EnumPushType)))
				{
					ListItem obj = new ListItem
					{
						Text = ((Enum)(object)value).ToDescription()
					};
					int num = (int)value;
					obj.Value = num.ToString();
					ListItem item = obj;
					this.rblPushType.Items.Add(item);
				}
				this.rblPushType.SelectedIndex = 0;
			}
		}

		protected void btnSend_Click(object sender, EventArgs e)
		{
			lock (ManualPush.objLock)
			{
				string text = this.txtURL.Text.Trim();
				string str = string.Empty;
				string text2 = this.txtPushContent.Text.Trim();
				string text3 = this.txtPushTitle.Text.Trim();
				string value = this.hidUserIds.Value;
				if (string.IsNullOrEmpty(value))
				{
					this.ShowMsg("请选择要推送消息的会员", false);
				}
				else if (string.IsNullOrEmpty(text3))
				{
					this.ShowMsg("请填写推送标题", false);
				}
				else if (string.IsNullOrEmpty(text2))
				{
					this.ShowMsg("请填写推送内容", false);
				}
				else
				{
					EnumPushType enumPushType = (EnumPushType)this.rblPushType.SelectedValue.ToInt(0);
					switch (enumPushType)
					{
					case EnumPushType.Link:
						if (string.IsNullOrEmpty(text))
						{
							this.ShowMsg("请填写链接", false);
							return;
						}
						if (!Regex.IsMatch(text, "(http|ftp|https):\\/\\/[\\w\\-_]+(\\.[\\w\\-_]+)+([\\w\\-\\.,@?^=%&amp;:/~\\+#]*[\\w\\-\\@?^=%&amp;/~\\+#])?"))
						{
							this.ShowMsg("请输入正确的链接,比如 https://www.huz.com.cn", false);
							return;
						}
						str = string.Format("url{1}{0}{2}", this.txtURL.Text.Trim(), VShopHelper.SEPARATORCONTEXT, VShopHelper.SEPARATOREVERY);
						break;
					case EnumPushType.ProductTopic:
						if (string.IsNullOrEmpty(this.topicId))
						{
							this.ShowMsg("请选择专题", false);
							return;
						}
						str = string.Format("url{1}{0}{2}", Globals.HostPath(HttpContext.Current.Request.Url) + "/appshop/Topics?TopicId=" + this.topicId, VShopHelper.SEPARATORCONTEXT, VShopHelper.SEPARATOREVERY);
						break;
					case EnumPushType.Activity:
						switch (this.rblActivity.SelectedValue.ToInt(0))
						{
						case 2:
							str = string.Format("url{1}{0}{2}", Globals.HostPath(HttpContext.Current.Request.Url) + "/appshop/CountDownProducts.aspx", VShopHelper.SEPARATORCONTEXT, VShopHelper.SEPARATOREVERY);
							break;
						case 1:
							str = string.Format("url{1}{0}{2}", Globals.HostPath(HttpContext.Current.Request.Url) + "/appshop/GroupBuyList.aspx", VShopHelper.SEPARATORCONTEXT, VShopHelper.SEPARATOREVERY);
							break;
						}
						break;
					case EnumPushType.Product:
						if (string.IsNullOrEmpty(this.productId))
						{
							this.ShowMsg("请选择商品", false);
							return;
						}
						str = string.Format("productid{1}{0}{2}", this.productId, VShopHelper.SEPARATORCONTEXT, VShopHelper.SEPARATOREVERY);
						break;
					}
					DateTime now = DateTime.Now;
					int num = 14;
					int num2 = 20;
					if (text3.Length > num)
					{
						this.ShowMsg($"推送标题不能超过{num}", false);
					}
					else if (text2.Length > num2)
					{
						this.ShowMsg($"推送内容不能超过{num2}个字", false);
					}
					else
					{
						str += string.Format("type{1}{0}{2}", (int)enumPushType, VShopHelper.SEPARATORCONTEXT, VShopHelper.SEPARATOREVERY);
						AppPushRecordInfo appPushRecordInfo = new AppPushRecordInfo
						{
							PushType = (int)enumPushType,
							PushContent = text2,
							PushTitle = text3,
							SendUserIds = value,
							PushStatus = 1,
							PushSendType = 1,
							PushSendDate = now,
							ToAll = false,
							Extras = str
						};
						if (VShopHelper.CheckAppPushRecordDuplicate(appPushRecordInfo))
						{
							this.ShowMsg("不能在一小时内推送重复的信息", false);
						}
						else
						{
							VShopHelper.AppPushRecordSendAboutAtOnce(appPushRecordInfo);
							if (appPushRecordInfo.PushStatus.Equals(EnumPushStatus.PushFailure))
							{
								this.ShowMsg("推送失败", false);
							}
							else
							{
								this.ShowMsg("推送成功", true);
							}
						}
					}
				}
			}
		}
	}
}
