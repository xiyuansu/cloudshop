using System.Data;
using System.Linq;
using System.Text;

namespace Hidistro.Core
{
	public static class DataSetExtends
	{
		public static bool IsNullOrEmpty(this DataSet ds)
		{
			if (ds == null || ds.Tables.Count == 0)
			{
				return true;
			}
			return ds.Tables.Cast<DataTable>().All((DataTable tb) => tb.Rows.Count == 0);
		}

		public static bool IsNullOrEmpty(this DataTable dt)
		{
			return dt == null || dt.Rows.Count == 0;
		}

		public static string ToJson(this DataTable dt)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("[");
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				stringBuilder.Append("{");
				for (int j = 0; j < dt.Columns.Count; j++)
				{
					stringBuilder.Append("\"");
					stringBuilder.Append(dt.Columns[j].ColumnName);
					stringBuilder.Append("\":\"");
					stringBuilder.Append(dt.Rows[i][j]);
					stringBuilder.Append("\",");
				}
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
				stringBuilder.Append("},");
			}
			if (stringBuilder.Length > 1)
			{
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
			}
			stringBuilder.Append("]");
			return stringBuilder.ToString();
		}
	}
}
