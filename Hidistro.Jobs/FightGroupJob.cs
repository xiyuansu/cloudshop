using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Jobs;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Messages;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Store;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Xml;

namespace Hidistro.Jobs
{
	public class FightGroupJob : IJob
	{
		public void Execute(XmlNode node)
		{
			try
			{
				Database database = DatabaseFactory.CreateDatabase();
				DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT o.OrderId, o.UserId, o.StoreId, o.FightGroupId, o.Gateway, o.GatewayOrderId,o.OrderStatus,o.BalanceAmount, o.OrderTotal,(SELECT SUM(Quantity) FROM Hishop_OrderItems WHERE OrderId = o.OrderId) AS Quantity,(SELECT Top 1 ItemDescription FROM Hishop_OrderItems WHERE OrderId = o.OrderId) as ItemDescription FROM Hishop_FightGroups fg INNER JOIN Hishop_Orders o ON fg.FightGroupId = o.FightGroupId AND fg.EndTime < getdate () AND fg.Status = @Status");
				database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, 0);
				DataTable dataTable = null;
				using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
				{
					dataTable = DataHelper.ConverDataReaderToDataTable(reader);
				}
				if (dataTable != null && dataTable.Rows.Count > 0)
				{
					foreach (DataRow row in dataTable.Rows)
					{
						int refundType = 3;
						string text = row["Gateway"].ToNullString();
						string orderId = row["OrderId"].ToNullString();
						string refundOrderId = row["GatewayOrderId"].ToNullString();
						decimal refundAmount = row["OrderTotal"].ToDecimal(0);
						decimal d = row["BalanceAmount"].ToDecimal(0);
						if (d > decimal.Zero)
						{
							refundType = 1;
						}
						int value = row["StoreId"].ToInt(0);
						int num = row["Quantity"].ToInt(0);
						int fightGroupFail = row["FightGroupId"].ToInt(0);
						int num2 = VShopHelper.SetFightGroupFail(fightGroupFail);
						switch (row["OrderStatus"].ToInt(0))
						{
						case 1:
							TradeHelper.CloseOrder(orderId, "火拼团订单成团时限内未付款");
							break;
						case 2:
						{
							if (text == "hishop.plugins.payment.advancerequest")
							{
								refundType = 1;
							}
							RefundInfo refundInfo = new RefundInfo();
							refundInfo.OrderId = orderId;
							refundInfo.RefundReason = "拼团失败，自动退款";
							refundInfo.RefundGateWay = (string.IsNullOrEmpty(text) ? "" : text.ToLower().Replace(".payment.", ".refund."));
							refundInfo.RefundType = (RefundTypes)refundType;
							refundInfo.RefundOrderId = refundOrderId;
							refundInfo.StoreId = value;
							refundInfo.AdminRemark = "拼团失败，自动退款";
							refundInfo.ApplyForTime = DateTime.Now;
							refundInfo.UserRemark = "";
							refundInfo.RefundAmount = refundAmount;
							TradeHelper.ApplyForRefund(refundInfo);
							MemberInfo user = Users.GetUser((int)row["UserId"]);
							string productInfo = row["ItemDescription"].ToString();
							string fightGroupInfo = string.Format("{0}人团{1}元", num2, ((decimal)row["OrderTotal"]).F2ToString("f2"));
							Messenger.FightGroupOrderFail(user, productInfo, fightGroupInfo, orderId);
							break;
						}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Globals.WriteExceptionLog(ex, null, "FightGroupJob");
			}
		}
	}
}
