using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Promotions;
using System;
using System.Data;
using System.Data.Common;

namespace Hidistro.SqlDal.Promotions
{
	public class WeiXinRedEnvelopeDao : BaseDao
	{
		public PageModel<WeiXinRedEnvelopeInfo> GetWeiXinRedEnvelope(RedEnvelopeGetRecordQuery query)
		{
			return DataHelper.PagingByRownumber<WeiXinRedEnvelopeInfo>(query.PageIndex, query.PageSize, "Id", SortAction.Desc, true, "Hishop_WeiXinRedEnvelope", "Id", "1=1", "*");
		}

		public bool SetRedEnvelopeState(int id, RedEnvelopeState redEnvelopeState)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE [dbo].[Hishop_WeiXinRedEnvelope] SET [State] =@State WHERE Id=@Id");
			base.database.AddInParameter(sqlStringCommand, "Id", DbType.Int32, id);
			base.database.AddInParameter(sqlStringCommand, "State", DbType.Int32, (int)redEnvelopeState);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public WeiXinRedEnvelopeInfo GetOpenedWeiXinRedEnvelope()
		{
			DateTime now = DateTime.Now;
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT TOP 1 * FROM [dbo].[Hishop_WeiXinRedEnvelope] WHERE State=1 AND @NowDateTime>ActiveStartTime AND @NowDateTime<ActiveEndTime ORDER BY CreateTime ASC");
			base.database.AddInParameter(sqlStringCommand, "NowDateTime", DbType.DateTime, now);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				return DataHelper.ReaderToModel<WeiXinRedEnvelopeInfo>(objReader);
			}
		}

		public CouponActionStatus AddWeiXinRedEnvelopeToUser(CouponItemInfo couponItemInfo)
		{
			try
			{
				DbCommand sqlStringCommand = base.database.GetSqlStringCommand("insert into [dbo].[Hishop_CouponItems] values(null\r\n           ,@RedEnvelopeId\r\n           ,@ClaimCode\r\n           ,@UserId\r\n           ,@UserName\r\n           ,@GetDate\r\n           ,@CouponName\r\n           ,@Price\r\n           ,@OrderUseLimit\r\n           ,@StartTime\r\n           ,@ClosingTime\r\n           ,''\r\n           ,0\r\n           ,0\r\n           ,null\r\n           ,null\r\n           ,0)");
				base.database.AddInParameter(sqlStringCommand, "RedEnvelopeId", DbType.Int32, couponItemInfo.RedEnvelopeId);
				base.database.AddInParameter(sqlStringCommand, "ClaimCode", DbType.String, couponItemInfo.ClaimCode);
				base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, couponItemInfo.UserId);
				base.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, couponItemInfo.UserName);
				base.database.AddInParameter(sqlStringCommand, "GetDate", DbType.DateTime, couponItemInfo.GetDate);
				base.database.AddInParameter(sqlStringCommand, "CouponName", DbType.String, couponItemInfo.CouponName);
				base.database.AddInParameter(sqlStringCommand, "Price", DbType.Decimal, couponItemInfo.Price);
				base.database.AddInParameter(sqlStringCommand, "OrderUseLimit", DbType.Decimal, couponItemInfo.OrderUseLimit);
				base.database.AddInParameter(sqlStringCommand, "StartTime", DbType.DateTime, couponItemInfo.StartTime);
				base.database.AddInParameter(sqlStringCommand, "ClosingTime", DbType.DateTime, couponItemInfo.ClosingTime);
				return (base.database.ExecuteNonQuery(sqlStringCommand) <= 0) ? CouponActionStatus.UnknowError : CouponActionStatus.Success;
			}
			catch (Exception ex)
			{
				Globals.WriteExceptionLog(ex, null, "Exception");
				return CouponActionStatus.UnknowError;
			}
		}
	}
}
