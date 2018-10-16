using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Messages;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Sales;
using Hidistro.UI.Common.Controls;
using System;
using System.Data;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.sales
{
	public class CheckVerification : AdminPage
	{
		private OrderInfo order;

		private int currentGroup = 1;

		protected HtmlInputHidden hidRemarkImage;

		protected Literal litStoreName;

		protected Literal litOrderId;

		protected CheckBoxList ckbList;

		protected Repeater orderInputItem;

		protected Order_ItemsList itemsList;

		protected Button btnVerification;

		protected HtmlInputHidden hidIsVerification;

		protected void Page_Load(object sender, EventArgs e)
		{
			string text = this.Page.Request["Code"].ToNullString();
			OrderVerificationItemInfo verificationInfoByPassword = OrderHelper.GetVerificationInfoByPassword(text);
			if (verificationInfoByPassword == null)
			{
				this.ShowMsg("该核销码无效，请重新输入", false);
			}
			else if (verificationInfoByPassword.VerificationStatus == 1)
			{
				this.ShowMsg("该核销码 于" + verificationInfoByPassword.VerificationDate.Value.ToString("yyyy-MM-dd HH:mm:ss") + "已核销", false);
			}
			else if (verificationInfoByPassword.VerificationStatus == 3)
			{
				this.ShowMsg("核销码已过期，无法核销", false);
			}
			else if (verificationInfoByPassword.VerificationStatus == 5)
			{
				this.ShowMsg("此核销码已进行售后，无法核销", false);
			}
			else if (verificationInfoByPassword.VerificationStatus == 4)
			{
				this.ShowMsg("此核销码已进行售后，无法核销", false);
			}
			else
			{
				this.order = OrderHelper.GetServiceProductOrderInfo(verificationInfoByPassword.OrderId);
				if (this.order == null)
				{
					this.ShowMsg("订单不存在，或者已被删除。", false);
				}
				else
				{
					this.itemsList.Order = this.order;
					if (!this.Page.IsPostBack)
					{
						this.litStoreName.Text = this.order.StoreName;
						this.litOrderId.Text = this.order.OrderId;
						DataTable dataSource = OrderHelper.GetOrderInputItem(this.order.OrderId);
						this.orderInputItem.DataSource = dataSource;
						this.orderInputItem.DataBind();
						DataTable verificationItem = OrderHelper.GetVerificationItem(this.order.OrderId);
						for (int i = 0; i < verificationItem.Rows.Count; i++)
						{
							if (verificationItem.Rows[i]["VerificationStatus"].ToInt(0) != 0.GetHashCode())
							{
								verificationItem.Rows.Remove(verificationItem.Rows[i]);
								i--;
							}
							else
							{
								verificationItem.Rows[i]["SkuId"] = this.GetVerificationPassword(verificationItem.Rows[i]["VerificationPassword"], text);
							}
						}
						this.ckbList.DataSource = verificationItem;
						this.ckbList.DataTextField = "SkuId";
						this.ckbList.DataValueField = "VerificationPassword";
						this.ckbList.DataBind();
						this.ckbList.SelectedValue = text;
					}
				}
			}
		}

		protected void btnVerification_Click(object sender, EventArgs e)
		{
			this.btnVerification.Enabled = false;
			string text = "";
			for (int i = 0; i < this.ckbList.Items.Count; i++)
			{
				if (this.ckbList.Items[i].Selected)
				{
					text = text + this.ckbList.Items[i].Value + ",";
				}
			}
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("请选择核销码", false);
				this.btnVerification.Enabled = true;
			}
			else
			{
				string[] array = text.TrimEnd(',').Split(',');
				DateTime now = DateTime.Now;
				string orderId = "";
				OrderVerificationItemInfo orderVerificationItemInfo = null;
				int num = 0;
				for (int j = 0; j < array.Length; j++)
				{
					if (!string.IsNullOrEmpty(array[j]))
					{
						OrderVerificationItemInfo verificationInfoByPassword = OrderHelper.GetVerificationInfoByPassword(array[j]);
						if (verificationInfoByPassword != null)
						{
							orderVerificationItemInfo = verificationInfoByPassword;
							verificationInfoByPassword.VerificationStatus = 1;
							verificationInfoByPassword.VerificationDate = now;
							verificationInfoByPassword.ManagerId = HiContext.Current.Manager.ManagerId;
							verificationInfoByPassword.UserName = HiContext.Current.Manager.UserName;
							OrderHelper.UpdateVerificationItem(verificationInfoByPassword);
							orderId = verificationInfoByPassword.OrderId;
							num++;
						}
					}
				}
				string text2 = "平台";
				OrderInfo serviceProductOrderInfo = OrderHelper.GetServiceProductOrderInfo(orderId);
				if (serviceProductOrderInfo != null)
				{
					text2 = serviceProductOrderInfo.StoreName;
					if (orderVerificationItemInfo != null)
					{
						decimal verificationTotal = (decimal)num * (serviceProductOrderInfo.GetTotal(false) / (decimal)serviceProductOrderInfo.LineItems.Values.FirstOrDefault().Quantity);
						MemberInfo user = Users.GetUser(serviceProductOrderInfo.UserId);
						Messenger.ServiceOrderValidSuccess(orderVerificationItemInfo, user, serviceProductOrderInfo, serviceProductOrderInfo.LineItems.Values.FirstOrDefault().ItemDescription, text2, string.Join(",", array), verificationTotal);
					}
					if (OrderHelper.IsVerificationFinished(orderId) && serviceProductOrderInfo.ItemStatus == OrderItemStatus.Nomarl)
					{
						serviceProductOrderInfo.OrderStatus = OrderStatus.Finished;
						serviceProductOrderInfo.FinishDate = DateTime.Now;
						TradeHelper.UpdateOrderInfo(serviceProductOrderInfo);
					}
				}
				this.hidIsVerification.Value = "true";
				this.ShowMsg("核销成功", true);
			}
		}

		public string GetVerificationPassword(object pwd, string code)
		{
			string result = "";
			if (pwd.ToNullString().Length == 12)
			{
				result = pwd.ToNullString();
				string text = result.Substring(0, 4);
				string text2 = result.Substring(4, 4);
				string text3 = result.Substring(8, 4);
				if (result != code)
				{
					text2 = "****";
				}
				result = text + " " + text2 + " " + text3;
			}
			return result;
		}

		public string GetInputField(object inputFieldType, object inputFieldValue)
		{
			int num = inputFieldType.ToInt(0);
			string text = inputFieldValue.ToNullString();
			if (num == 6)
			{
				string[] array = text.Split(',');
				text = "";
				for (int i = 0; i < array.Length; i++)
				{
					if (!string.IsNullOrEmpty(array[i]))
					{
						text = text + "<a href=\"" + array[i] + "\" target=\"_blank\" style=\"margin-right:10px;\"><img src=\"" + array[i] + "\" style=\"width:40px;border-width:0px;\"></a>";
					}
				}
			}
			return text;
		}

		protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				HtmlTableRow htmlTableRow = e.Item.FindControl("trGroup") as HtmlTableRow;
				HtmlInputHidden htmlInputHidden = e.Item.FindControl("hidInputFieldGroup") as HtmlInputHidden;
				if (this.currentGroup != htmlInputHidden.Value.ToInt(0))
				{
					htmlTableRow.Style.Add("border-top", "1px dotted #ccc;");
					this.currentGroup = htmlInputHidden.Value.ToInt(0);
				}
			}
		}
	}
}
