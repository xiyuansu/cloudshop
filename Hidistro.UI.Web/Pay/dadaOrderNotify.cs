using Hidistro.Core;
using Hidistro.Core.Urls;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Shopping;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Text;
using System.Web.UI;

namespace Hidistro.UI.Web.pay
{
	public class dadaOrderNotify : Page
	{
		public class dadaNotifyInfo
		{
			public string client_id
			{
				get;
				set;
			}

			public string order_id
			{
				get;
				set;
			}

			public int order_status
			{
				get;
				set;
			}

			public string cancel_reason
			{
				get;
				set;
			}

			public int cancel_from
			{
				get;
				set;
			}

			public int update_time
			{
				get;
				set;
			}

			public string signature
			{
				get;
				set;
			}

			public int dm_id
			{
				get;
				set;
			}

			public string dm_name
			{
				get;
				set;
			}

			public string dm_mobile
			{
				get;
				set;
			}
		}

		protected string GetParameter(string name)
		{
			return RouteConfig.GetParameter(this.Page, name, false);
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			string value = this.PostInput();
			JObject jObject = JsonConvert.DeserializeObject(value) as JObject;
			string orderId = jObject["order_id"].ToNullString();
			int num = jObject["order_status"].ToInt(0);
			OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(orderId);
			if (orderInfo != null && num != 0)
			{
				if (num == 5)
				{
					orderInfo.OrderStatus = OrderStatus.BuyerAlreadyPaid;
					orderInfo.CloseReason = jObject["cancel_reason"].ToNullString();
				}
				if (num == 2)
				{
					orderInfo.OrderStatus = OrderStatus.SellerAlreadySent;
					orderInfo.ShipOrderNumber = jObject["client_id"].ToNullString();
				}
				if (num == 4)
				{
					orderInfo.OrderStatus = OrderStatus.Finished;
					orderInfo.FinishDate = DateTime.Now;
				}
				orderInfo.DadaStatus = (DadaStatus)num;
				TradeHelper.UpdateOrderInfo(orderInfo);
			}
		}

		private string PostInput()
		{
			try
			{
				Stream inputStream = base.Request.InputStream;
				int num = 0;
				byte[] array = new byte[1024];
				StringBuilder stringBuilder = new StringBuilder();
				while ((num = inputStream.Read(array, 0, 1024)) > 0)
				{
					stringBuilder.Append(Encoding.UTF8.GetString(array, 0, num));
				}
				inputStream.Flush();
				inputStream.Close();
				inputStream.Dispose();
				return stringBuilder.ToString();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
