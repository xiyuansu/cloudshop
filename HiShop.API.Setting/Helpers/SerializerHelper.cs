using System.Globalization;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace HiShop.API.Setting.Helpers
{
	public class SerializerHelper
	{
		public static string DecodeUnicode(Match match)
		{
			if (!match.Success)
			{
				return null;
			}
			char c = (char)int.Parse(match.Value.Remove(0, 2), NumberStyles.HexNumber);
			return new string(c, 1);
		}

		public string GetJsonString(object data)
		{
			JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
			string input = javaScriptSerializer.Serialize(data);
			MatchEvaluator evaluator = SerializerHelper.DecodeUnicode;
			return Regex.Replace(input, "\\\\u[0123456789abcdef]{4}", evaluator);
		}
	}
}
