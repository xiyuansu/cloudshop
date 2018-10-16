using Hidistro.Entities;
using System.Data;
using System.Data.Common;

namespace Hidistro.SqlDal.Orders
{
	public class ExpressDataDao : BaseDao
	{
		public bool AddExpressData(ExpressDataInfo edata)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("IF NOT EXISTS (SELECT * FROM Hishop_OrderExpressData WHERE CompanyCode=@CompanyCode AND ExpressNumber=@ExpressNumber)  INSERT INTO Hishop_OrderExpressData (CompanyCode,ExpressNumber, DataContent) VALUES(@CompanyCode,@ExpressNumber, @DataContent) ELSE  UPDATE Hishop_OrderExpressData SET DataContent=@DataContent WHERE CompanyCode=@CompanyCode AND ExpressNumber=@ExpressNumber");
			base.database.AddInParameter(sqlStringCommand, "CompanyCode", DbType.String, edata.CompanyCode);
			base.database.AddInParameter(sqlStringCommand, "ExpressNumber", DbType.String, edata.ExpressNumber);
			base.database.AddInParameter(sqlStringCommand, "DataContent", DbType.String, edata.DataContent);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public string GetExpressDataList(string companyCode, string expressNumber)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM Hishop_OrderExpressData WHERE CompanyCode = @CompanyCode AND ExpressNumber=@ExpressNumber;");
			base.database.AddInParameter(sqlStringCommand, "CompanyCode", DbType.String, companyCode);
			base.database.AddInParameter(sqlStringCommand, "ExpressNumber", DbType.String, expressNumber);
			string result = "";
			IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand);
			if (dataReader.Read())
			{
				result = ((IDataRecord)dataReader)["DataContent"].ToString();
			}
			dataReader.Close();
			return result;
		}
	}
}
