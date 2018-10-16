using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.App
{
	public class ManualPushAgain : AdminPage
	{
		private int pushRecordId;

		private static object objLock = new object();

		protected ScriptManager ScriptManager1;

		protected HtmlAnchor sendTitle;

		protected Label lblPushTypeText;

		protected HtmlGenericControl spanPushTypeText;

		protected Label lblPushTypeContext;

		protected Label lblPushTitle;

		protected Label lblPushContent;

		protected Label lblPushTag;

		protected Label lblSendType;

		protected Label lblState;

		protected Button btnSend;

		protected HtmlInputHidden locationUrl;

		protected HiddenField hfPushTypeLi;

		protected HiddenField hfPushRecordId;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.pushRecordId = this.Page.Request["pushRecordId"].ToInt(0);
			if (!base.IsPostBack)
			{
				this.BindDefaultData();
			}
		}

		private void BindDefaultData()
		{
			AppPushRecordInfo appPushRecordInfo = VShopHelper.GetAppPushRecordInfo(this.pushRecordId);
			if (appPushRecordInfo != null)
			{
				int num = appPushRecordInfo.PushStatus;
				if (num.Equals(EnumPushStatus.NoPush))
				{
					this.sendTitle.InnerText = "立即推送";
				}
				this.lblPushTypeText.Text = ((Enum)(object)(EnumPushType)appPushRecordInfo.PushType).ToDescription();
				this.spanPushTypeText.InnerHtml = $"{this.lblPushTypeText.Text}：";
				this.lblPushContent.Text = appPushRecordInfo.PushContent;
				this.lblPushTag.Text = appPushRecordInfo.PushTagText;
				this.lblPushTitle.Text = appPushRecordInfo.PushTitle;
				this.lblState.Text = ((Enum)(object)(EnumPushStatus)appPushRecordInfo.PushStatus).ToDescription();
				num = appPushRecordInfo.PushSendType;
				if (num.Equals(EnumPushSendType.AtOnce))
				{
					this.lblSendType.Text = ((Enum)(object)EnumPushSendType.AtOnce).ToDescription();
				}
				else
				{
					num = appPushRecordInfo.PushSendType;
					if (num.Equals(EnumPushSendType.Timer))
					{
						this.lblSendType.Text = appPushRecordInfo.PushSendDate.ToString("yyyy-MM-dd HH:mm");
					}
				}
				Dictionary<string, string> extras = this.GetExtras(appPushRecordInfo.Extras);
				switch (appPushRecordInfo.PushType)
				{
				case 3:
					extras.ForEach(delegate(KeyValuePair<string, string> c)
					{
						if (c.Key == "url")
						{
							if (c.Value.IndexOf("CountDownProducts") >= 0)
							{
								this.lblPushTypeContext.Text = ((Enum)(object)EnumActivity.CountDownBuy).ToDescription();
							}
							else
							{
								this.lblPushTypeContext.Text = ((Enum)(object)EnumActivity.GroupBuy).ToDescription();
							}
						}
					});
					break;
				case 1:
					extras.ForEach(delegate(KeyValuePair<string, string> c)
					{
						if (c.Key == "url")
						{
							this.lblPushTypeContext.Text = c.Value;
						}
					});
					break;
				case 2:
					extras.ForEach(delegate(KeyValuePair<string, string> c)
					{
						if (c.Key == "url")
						{
							int topicId = 0;
							string[] array = c.Value.Split('=');
							if (array.Count() > 0)
							{
								topicId = array[1].ToInt(0);
							}
							TopicInfo topicInfo = VShopHelper.Gettopic(topicId);
							this.lblPushTypeContext.Text = ((topicInfo != null) ? topicInfo.Title : string.Empty);
						}
					});
					break;
				case 4:
					extras.ForEach(delegate(KeyValuePair<string, string> c)
					{
						if (c.Key == "productid")
						{
							int num2 = c.Value.ToInt(0);
							IList<int> list = null;
							Dictionary<int, IList<int>> dictionary = default(Dictionary<int, IList<int>>);
							ProductInfo productDetails = ProductHelper.GetProductDetails(num2.ToInt(0), out dictionary, out list);
							if (productDetails != null)
							{
								this.lblPushTypeContext.Text = productDetails.ProductName;
							}
						}
					});
					break;
				}
			}
		}

		protected void btnSend_Click(object sender, EventArgs e)
		{
			lock (ManualPushAgain.objLock)
			{
				AppPushRecordInfo appPushRecordInfo = VShopHelper.SendAgainAppPushRecord(this.pushRecordId);
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

		private Dictionary<string, string> GetExtras(string value)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			string[] array = value.Split(VShopHelper.SEPARATOREVERY);
			string[] array2 = array;
			foreach (string text in array2)
			{
				if (!string.IsNullOrEmpty(text))
				{
					string[] array3 = text.Split(VShopHelper.SEPARATORCONTEXT);
					if (array3.Count() != 0)
					{
						dictionary.Add(array3[0], array3[1]);
					}
				}
			}
			return dictionary;
		}
	}
}
