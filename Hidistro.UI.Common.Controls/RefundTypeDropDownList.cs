using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Member;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class RefundTypeDropDownList : DropDownList
	{
		private bool allowNull = true;

		private string nullToDisplay = "请选择退款方式";

		public bool AllowNull
		{
			get
			{
				return this.allowNull;
			}
			set
			{
				this.allowNull = value;
			}
		}

		public string OrderGateWay
		{
			get;
			set;
		}

		public string NullToDisplay
		{
			get
			{
				return this.nullToDisplay;
			}
			set
			{
				this.nullToDisplay = value;
			}
		}

		public decimal BalanceAmount
		{
			get;
			set;
		}

		public new int? SelectedValue
		{
			get
			{
				if (string.IsNullOrEmpty(base.SelectedValue))
				{
					return null;
				}
				return int.Parse(base.SelectedValue);
			}
			set
			{
				int num;
				int num2;
				if (value.HasValue)
				{
					int? nullable = value;
					num = 0;
					num2 = ((nullable > num) ? 1 : 0);
				}
				else
				{
					num2 = 0;
				}
				if (num2 != 0)
				{
					num = value.Value;
					base.SelectedValue = num.ToString();
				}
				else
				{
					base.SelectedValue = string.Empty;
				}
			}
		}

		public static bool IsBackReturn(string refundGateway)
		{
			return TradeHelper.AllowRefundGateway.Contains(refundGateway);
		}

		public new void DataBind()
		{
			string text = this.OrderGateWay.ToNullString().ToLower();
			base.ClientIDMode = ClientIDMode.Static;
			this.Items.Clear();
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (this.AllowNull)
			{
				base.Items.Add(new ListItem(this.NullToDisplay, string.Empty));
			}
			bool flag = false;
			if (TradeHelper.AllowRefundGateway.Contains(text) && this.BalanceAmount <= decimal.Zero)
			{
				flag = true;
				if ((text == "hishop.plugins.payment.wxqrcode.wxqrcoderequest" || text == "hishop.plugins.payment.weixinrequest") && (string.IsNullOrEmpty(masterSettings.WeixinCertPath) || string.IsNullOrEmpty(masterSettings.WeixinCertPassword)))
				{
					flag = false;
				}
				if (text == "hishop.plugins.payment.wxappletpay" && (string.IsNullOrEmpty(masterSettings.WxApplectPayCert) || string.IsNullOrEmpty(masterSettings.WxApplectPayCertPassword)))
				{
					flag = false;
				}
				if (text == "hishop.plugins.payment.appwxrequest")
				{
					string appWxMchId = masterSettings.AppWxMchId;
					string b = string.IsNullOrEmpty(masterSettings.Main_Mch_ID) ? masterSettings.WeixinPartnerID : masterSettings.Main_Mch_ID;
					if (string.IsNullOrEmpty(masterSettings.AppWxCertPath) || string.IsNullOrEmpty(masterSettings.AppWxCertPass) || (appWxMchId == b && (string.IsNullOrEmpty(masterSettings.WeixinCertPath) || string.IsNullOrEmpty(masterSettings.WeixinCertPassword))))
					{
						flag = false;
					}
				}
			}
			int num;
			if (text.ToLower() == "hishop.plugins.payment.cashreceipts")
			{
				ListItemCollection items = base.Items;
				string enumDescription = EnumDescription.GetEnumDescription((Enum)(object)RefundTypes.ReturnOnStore, 0);
				num = 4;
				items.Add(new ListItem(enumDescription, num.ToString()));
			}
			else
			{
				foreach (RefundTypes value in Enum.GetValues(typeof(RefundTypes)))
				{
					if (value != RefundTypes.BackReturn && value != RefundTypes.ReturnOnStore)
					{
						ListItemCollection items2 = base.Items;
						string enumDescription2 = EnumDescription.GetEnumDescription((Enum)(object)value, 0);
						num = (int)value;
						items2.Add(new ListItem(enumDescription2, num.ToString()));
					}
					else
					{
						int num2;
						switch (value)
						{
						case RefundTypes.BackReturn:
							if (flag)
							{
								ListItemCollection items4 = base.Items;
								string enumDescription4 = EnumDescription.GetEnumDescription((Enum)(object)value, 0);
								num = (int)value;
								items4.Add(new ListItem(enumDescription4, num.ToString()));
							}
							break;
						case RefundTypes.InBankCard:
							num2 = ((this.BalanceAmount <= decimal.Zero) ? 1 : 0);
							goto IL_028b;
						default:
							{
								num2 = 0;
								goto IL_028b;
							}
							IL_028b:
							if (num2 != 0)
							{
								ListItemCollection items3 = base.Items;
								string enumDescription3 = EnumDescription.GetEnumDescription((Enum)(object)value, 0);
								num = (int)value;
								items3.Add(new ListItem(enumDescription3, num.ToString()));
							}
							break;
						}
					}
				}
			}
		}

		protected override void Render(HtmlTextWriter writer)
		{
			base.Render(writer);
		}
	}
}
