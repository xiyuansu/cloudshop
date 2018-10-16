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

namespace Hidistro.UI.Web.Admin.App
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
		}

		private static object objLock = new object();

		private string topicId;

		private string productId;

		private string formData;

		protected ScriptManager ScriptManager1;

		protected RadioButtonList rblPushType;

		protected Label lblPushType;

		protected TextBox txtURL;

		protected Label lblShow;

		protected HtmlInputButton btnSelectTopic;

		protected HtmlInputButton btnSelectProduct;

		protected RadioButtonList rblActivity;

		protected TextBox txtPushTitle;

		protected TextBox txtPushContent;

		protected MemberGradeDropDownList ddlPushTag;

		protected RadioButtonList rblPushSendType;

		protected CalendarPanel calendarSendDate;

		protected Button btnSend;

		protected HtmlInputHidden locationUrl;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.topicId = this.Page.Request["topicId"].ToNullString();
			this.productId = this.Page.Request["productId"].ToNullString();
			this.formData = this.Page.Request["formData"].ToNullString();
			this.SetDefault();
			this.SetDateControl();
			if (!base.IsPostBack)
			{
				this.ddlPushTag.DataBind();
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
			this.calendarSendDate.FunctionNameForChangeDate = "fuChangeStartDate";
			this.calendarSendDate.CalendarParameter["format"] = "yyyy-mm-dd hh:ii:00";
			this.calendarSendDate.CalendarParameter["minView"] = "0";
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
				this.ddlPushTag.SelectedValue = dataForm.PushTag.ToInt(0);
				this.rblPushSendType.SelectedValue = dataForm.PushSendType;
				if (this.rblPushSendType.SelectedValue.ToInt(0).Equals(EnumPushSendType.Timer))
				{
					this.calendarSendDate.SelectedDate = dataForm.PushSendDate.ToDateTime();
				}
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
			if (this.rblPushSendType.Items.Count <= 0)
			{
				int num;
				foreach (EnumPushSendType value in Enum.GetValues(typeof(EnumPushSendType)))
				{
					ListItem obj = new ListItem
					{
						Text = ((Enum)(object)value).ToDescription()
					};
					num = (int)value;
					obj.Value = num.ToString();
					ListItem item = obj;
					this.rblPushSendType.Items.Add(item);
				}
				num = 1;
				string selectedValue = num.ToString();
				this.rblPushSendType.SelectedValue = selectedValue;
				this.calendarSendDate.Style["display"] = "none";
			}
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
				string value = this.ddlPushTag.SelectedItem.Value;
				string pushTagText = this.ddlPushTag.SelectedItem.Text.Trim();
				EnumPushSendType pushSendType = (EnumPushSendType)this.rblPushSendType.SelectedValue.ToInt(0);
				if (string.IsNullOrEmpty(text3))
				{
					this.ShowMsg("请填写推送标题", false);
				}
				else if (string.IsNullOrEmpty(text2))
				{
					this.ShowMsg("请填写推送内容", false);
				}
				else if (pushSendType.Equals(EnumPushSendType.Timer) && !this.calendarSendDate.SelectedDate.HasValue)
				{
					this.ShowMsg("请填写定时的时间", false);
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
					DateTime pushSendDate = DateTime.Now;
					if (pushSendType.Equals(EnumPushSendType.Timer))
					{
						if (!this.calendarSendDate.SelectedDate.HasValue)
						{
							this.ShowMsg("请选择推送时间", false);
							goto end_IL_0009;
						}
						DateTime value2 = this.calendarSendDate.SelectedDate.Value;
						if (value2 <= DateTime.Now)
						{
							this.ShowMsg("请选择大于当前的时间", false);
							goto end_IL_0009;
						}
						pushSendDate = this.calendarSendDate.SelectedDate.Value;
					}
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
							PushTag = value,
							PushTagText = pushTagText,
							PushStatus = 1,
							PushSendType = (int)pushSendType,
							PushSendDate = pushSendDate,
							ToAll = (this.ddlPushTag.SelectedIndex == 0),
							Extras = str
						};
						if (VShopHelper.CheckAppPushRecordDuplicate(appPushRecordInfo))
						{
							this.ShowMsg("不能在一小时内推送重复的信息", false);
						}
						else
						{
							if (pushSendType.Equals(EnumPushSendType.AtOnce))
							{
								VShopHelper.AppPushRecordSendAboutAtOnce(appPushRecordInfo);
							}
							VShopHelper.AddAppPushRecord(appPushRecordInfo);
							if (appPushRecordInfo.PushStatus.Equals(EnumPushStatus.PushFailure))
							{
								this.ShowMsg("推送失败", false);
							}
							else
							{
								this.ShowMsg("推送成功", true, "PushRecords.aspx");
							}
						}
					}
				}
				end_IL_0009:;
			}
		}
	}
}
