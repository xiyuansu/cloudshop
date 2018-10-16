using Hidistro.Core;
using Hidistro.Entities.Sales;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Hidistro.SqlDal.Sales
{
	public class PaymentModeDao : BaseDao
	{
		public bool AddPayment(PaymentModeInfo paymentMode)
		{
			paymentMode.DisplaySequence = this.GetPaymentMaxDisplaySequence(paymentMode.ApplicationType) + 1;
			paymentMode.IsPercent = false;
			paymentMode.Charge = decimal.Zero;
			return this.Add(paymentMode, null) > 0;
		}

		private int GetPaymentMaxDisplaySequence(PayApplicationType applicationType)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("select max(DisplaySequence) from Hishop_PaymentTypes where ApplicationType=@ApplicationType");
			base.database.AddInParameter(sqlStringCommand, "ApplicationType", DbType.Int32, applicationType);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0);
		}

		public PaymentModeInfo GetPaymentMode(string gateway)
		{
			PaymentModeInfo result = null;
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT top 1 * FROM Hishop_PaymentTypes WHERE Gateway = @Gateway");
			base.database.AddInParameter(sqlStringCommand, "Gateway", DbType.String, gateway);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToModel<PaymentModeInfo>(objReader);
			}
			return result;
		}

		public PaymentModeInfo GetAlipayRefundPaymentMode(string gateways)
		{
			PaymentModeInfo result = null;
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT top 1 * FROM Hishop_PaymentTypes WHERE Gateway in(" + gateways + ") AND ModeType <> " + 99);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToModel<PaymentModeInfo>(objReader);
			}
			return result;
		}

		public bool IsSupportPodrequest()
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT count(ModeId) FROM Hishop_PaymentTypes WHERE Gateway = 'hishop.plugins.payment.podrequest'");
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0) > 0;
		}

		public bool IsSupportOfflineRequest()
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT count(ModeId) FROM Hishop_PaymentTypes WHERE Gateway = 'hishop.plugins.payment.bankrequest'");
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0) > 0;
		}

		public IList<PaymentModeInfo> GetPaymentModes(PayApplicationType payApplicationType)
		{
			IList<PaymentModeInfo> result = null;
			string str = "SELECT * FROM Hishop_PaymentTypes WHERE 1 = 1 ";
			if (payApplicationType != 0)
			{
				str += " AND ApplicationType = @ApplicationType";
			}
			str += " AND ModeType = @ModeType";
			str += " Order by DisplaySequence desc";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(str);
			base.database.AddInParameter(sqlStringCommand, "ApplicationType", DbType.Int32, (int)payApplicationType);
			base.database.AddInParameter(sqlStringCommand, "ModeType", DbType.Int32, 0);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<PaymentModeInfo>(objReader);
			}
			return result;
		}

		public IList<PaymentModeInfo> GetPaymentModes()
		{
			IList<PaymentModeInfo> result = null;
			string str = "SELECT * FROM Hishop_PaymentTypes WHERE 1 = 1 ";
			str += " AND ModeType = @ModeType";
			str += " Order by DisplaySequence desc";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(str);
			base.database.AddInParameter(sqlStringCommand, "ModeType", DbType.Int32, 1);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<PaymentModeInfo>(objReader);
			}
			return result;
		}

		public int GetPaymentModeCount(PayApplicationType applicationType)
		{
			string str = "SELECT COUNT(ModeId) FROM Hishop_PaymentTypes WHERE Gateway <> 'hishop.plugins.payment.podrequest' AND Gateway<>'hishop.plugins.payment.bankrequest'";
			if (applicationType != 0)
			{
				str += " AND ApplicationType = @ApplicationType";
			}
			str += " AND ModeType = @ModeType";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(str);
			base.database.AddInParameter(sqlStringCommand, "ApplicationType", DbType.Int32, (int)applicationType);
			base.database.AddInParameter(sqlStringCommand, "ModeType", DbType.Int32, 0);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0);
		}
	}
}
