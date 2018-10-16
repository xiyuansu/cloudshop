using System.Text.RegularExpressions;

namespace Hidistro.Core
{
	public static class Filter
	{
		public static string FilterScript(string content)
		{
			string arg = "(?'comment'<!--.*?--[ \\n\\r]*>)";
			string arg2 = "(\\/\\*.*?\\*\\/|\\/\\/.*?[\\n\\r])";
			string arg3 = $"(?'script'<[ \\n\\r]*script[^>]*>(.*?{arg2}?)*<[ \\n\\r]*/script[^>]*>)";
			string pattern = $"(?s)({arg}|{arg3})";
			return Filter.StripScriptAttributesFromTags(Regex.Replace(content, pattern, string.Empty, RegexOptions.IgnoreCase));
		}

		private static string StripScriptAttributesFromTags(string content)
		{
			string arg = "on(blur|c(hange|lick)|dblclick|focus|keypress|(key|mouse)(down|up)|(un)?load\r\n                |mouse(move|o(ut|ver))|reset|s(elect|ubmit))";
			string pattern = $"(?inx)\r\n    \\<(\\w+)\\s+\r\n        (\r\n            (?'attribute'\r\n            (?'attributeName'{arg})\\s*=\\s*\r\n            (?'delim'['\"]?)\r\n            (?'attributeValue'[^'\">]+)\r\n            (\\3)\r\n        )\r\n        |\r\n        (?'attribute'\r\n            (?'attributeName'href)\\s*=\\s*\r\n            (?'delim'['\"]?)\r\n            (?'attributeValue'javascript[^'\">]+)\r\n            (\\3)\r\n        )\r\n        |\r\n        [^>]\r\n    )*\r\n\\>";
			Regex regex = new Regex(pattern);
			return regex.Replace(content, Filter.StripAttributesHandler);
		}

		private static string StripAttributesHandler(Match m)
		{
			if (m.Groups["attribute"].Success)
			{
				return m.Value.Replace(m.Groups["attribute"].Value, "");
			}
			return m.Value;
		}

		public static string FilterAHrefScript(string content)
		{
			string input = Filter.FilterScript(content);
			string pattern = " href[ ^=]*= *[\\s\\S]*script *:";
			return Regex.Replace(input, pattern, string.Empty, RegexOptions.IgnoreCase);
		}

		public static string FilterSrc(string content)
		{
			string input = Filter.FilterScript(content);
			string pattern = " src *= *['\"]?[^\\.]+\\.(js|vbs|asp|aspx|php|jsp)['\"]";
			return Regex.Replace(input, pattern, "", RegexOptions.IgnoreCase);
		}

		public static string FilterHtml(string content)
		{
			string input = Filter.FilterScript(content);
			string pattern = "<[^>]*>";
			return Regex.Replace(input, pattern, string.Empty, RegexOptions.IgnoreCase);
		}

		public static string FilterObject(string content)
		{
			string pattern = "(?i)<Object([^>])*>(\\w|\\W)*</Object([^>])*>";
			return Regex.Replace(content, pattern, string.Empty, RegexOptions.IgnoreCase);
		}

		public static string FilterIframe(string content)
		{
			string pattern = "(?i)<Iframe([^>])*>(\\w|\\W)*</Iframe([^>])*>";
			return Regex.Replace(content, pattern, string.Empty, RegexOptions.IgnoreCase);
		}

		public static string FilterFrameset(string content)
		{
			string pattern = "(?i)<Frameset([^>])*>(\\w|\\W)*</Frameset([^>])*>";
			return Regex.Replace(content, pattern, string.Empty, RegexOptions.IgnoreCase);
		}

		public static string FilterAll(string content)
		{
			content = Filter.FilterHtml(content);
			content = Filter.FilterScript(content);
			content = Filter.FilterAHrefScript(content);
			content = Filter.FilterObject(content);
			content = Filter.FilterIframe(content);
			content = Filter.FilterFrameset(content);
			content = Filter.FilterSrc(content);
			return content;
		}
	}
}
