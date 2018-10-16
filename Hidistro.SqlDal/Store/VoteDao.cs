using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Store;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Store
{
	public class VoteDao : BaseDao
	{
		public DataSet GetVotes()
		{
			string query = "SELECT *, (SELECT ISNULL(SUM(ItemCount),0) FROM Hishop_VoteItems WHERE VoteId = Hishop_Votes.VoteId) AS VoteCounts FROM Hishop_Votes";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			return base.database.ExecuteDataSet(sqlStringCommand);
		}

		public DbQueryResult Query(VoteSearch query)
		{
			StringBuilder stringBuilder = new StringBuilder("1=1 ");
			if (query.status != 0)
			{
				DateTime now;
				if (query.status == VoteStatus.In)
				{
					StringBuilder stringBuilder2 = stringBuilder;
					now = DateTime.Now;
					stringBuilder2.AppendFormat("and [EndDate] > '{0}'", now.ToString("yyyy-MM-dd HH:mm:ss"));
				}
				else if (query.status == VoteStatus.End)
				{
					StringBuilder stringBuilder3 = stringBuilder;
					now = DateTime.Now;
					stringBuilder3.AppendFormat("and [EndDate] < '{0}'", now.ToString("yyyy-MM-dd HH:mm:ss"));
				}
				else if (query.status == VoteStatus.unBegin)
				{
					StringBuilder stringBuilder4 = stringBuilder;
					now = DateTime.Now;
					stringBuilder4.AppendFormat("and [StartDate] > '{0}'", now.ToString("yyyy-MM-dd HH:mm:ss"));
				}
			}
			if (!string.IsNullOrEmpty(query.Name))
			{
				stringBuilder.AppendFormat("and VoteName like '%{0}%'  ", query.Name);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_Votes", "VoteId", stringBuilder.ToString(), "*");
		}

		public DataTable GetKeys(string voteIds)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand($"SELECT Keys , ActivityId  FROM vshop_Reply WHERE [ReplyType] = {128} AND ActivityId in ( {voteIds} )");
			return base.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}

		public DataSet GetShowWXVotes()
		{
			string query = string.Format("SELECT * FROM Hishop_Votes WHERE IsBackup = 1 and ((StartDate<='{0}' and EndDate>='{0}') or (StartDate is null and EndDate is null)) and IsDisplayAtWX=1; SELECT * FROM Hishop_VoteItems WHERE voteId IN (SELECT voteId FROM Hishop_Votes WHERE IsBackup = 1 and ((StartDate<='{0}' and EndDate>='{0}') or (StartDate is null and EndDate is null)) and IsDisplayAtWX=1)", DateTime.Parse(DateTime.Now.ToShortDateString()));
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			return base.database.ExecuteDataSet(sqlStringCommand);
		}

		public bool DeleteVoteItem(long voteId, DbTransaction dbTran)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM Hishop_VoteItems WHERE VoteId = @VoteId");
			base.database.AddInParameter(sqlStringCommand, "VoteId", DbType.Int64, voteId);
			return base.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0;
		}

		public VoteInfo GetVoteById(long voteId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT *, (SELECT ISNULL(SUM(ItemCount),0) FROM Hishop_VoteItems WHERE VoteId = @VoteId) AS VoteCounts FROM Hishop_Votes WHERE VoteId = @VoteId");
			base.database.AddInParameter(sqlStringCommand, "VoteId", DbType.Int64, voteId);
			VoteInfo result = null;
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToModel<VoteInfo>(objReader);
			}
			return result;
		}

		public IList<VoteItemInfo> GetVoteItems(long voteId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM Hishop_VoteItems WHERE VoteId = @VoteId");
			base.database.AddInParameter(sqlStringCommand, "VoteId", DbType.Int64, voteId);
			IList<VoteItemInfo> result = null;
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<VoteItemInfo>(objReader);
			}
			return result;
		}

		public int GetVoteCounts(long voteId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT ISNULL(SUM(ItemCount),0) FROM Hishop_VoteItems WHERE VoteId = @VoteId");
			base.database.AddInParameter(sqlStringCommand, "VoteId", DbType.Int64, voteId);
			return (int)base.database.ExecuteScalar(sqlStringCommand);
		}

		public DataTable VishopLoadVote(int voteId, out string voteName, out int checkNum, out int voteNum)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT VoteName, MaxCheck, (SELECT SUM(ItemCount) FROM Hishop_VoteItems WHERE VoteId = @VoteId) AS VoteNum FROM Hishop_Votes WHERE VoteId = @VoteId; SELECT * FROM Hishop_VoteItems WHERE VoteId = @VoteId");
			base.database.AddInParameter(sqlStringCommand, "VoteId", DbType.Int64, voteId);
			voteName = string.Empty;
			checkNum = 1;
			voteNum = 0;
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					voteName = (string)((IDataRecord)dataReader)["VoteName"];
					checkNum = (int)((IDataRecord)dataReader)["MaxCheck"];
					voteNum = (int)((IDataRecord)dataReader)["VoteNum"];
				}
				dataReader.NextResult();
				return DataHelper.ConverDataReaderToDataTable(dataReader);
			}
		}

		public bool Vote(int voteId, string itemIds, int userId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("IF EXISTS (SELECT 1 FROM Hishop_Votes WHERE VoteId=@VoteId AND (GETDATE() < StartDate OR GETDATE() > EndDate) ) return;INSERT INTO Hishop_VoteRecord (UserId, VoteId) VALUES (@UserId, @VoteId);" + $" UPDATE Hishop_VoteItems SET ItemCount = ItemCount + 1 WHERE VoteId = @VoteId AND VoteItemId IN ({itemIds})");
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int64, userId);
			base.database.AddInParameter(sqlStringCommand, "VoteId", DbType.Int64, voteId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool VishopIsVote(int voteId, int userId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT COUNT(*) FROM Hishop_VoteRecord WHERE VoteId = @VoteId AND UserId = @UserId");
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int64, userId);
			base.database.AddInParameter(sqlStringCommand, "VoteId", DbType.Int64, voteId);
			return (int)base.database.ExecuteScalar(sqlStringCommand) > 0;
		}
	}
}
