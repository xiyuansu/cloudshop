using Newtonsoft.Json;
using System;
using System.Data;

namespace Hidistro.UI.Web
{
	public class ConvertTojson : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return typeof(DataTable).IsAssignableFrom(objectType);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			DataTable dataTable = (DataTable)value;
			writer.WriteStartArray();
			foreach (DataRow row in dataTable.Rows)
			{
				writer.WriteStartObject();
				foreach (DataColumn column in dataTable.Columns)
				{
					writer.WritePropertyName(column.ColumnName);
					writer.WriteValue(row[column].ToString());
				}
				writer.WriteEndObject();
			}
			writer.WriteEndArray();
		}
	}
}
