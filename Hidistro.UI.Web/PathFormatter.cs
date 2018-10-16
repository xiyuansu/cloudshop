using System;
using System.IO;
using System.Text.RegularExpressions;

public static class PathFormatter
{
	public static string Format(string originFileName, string pathFormat)
	{
		if (string.IsNullOrWhiteSpace(pathFormat))
		{
			pathFormat = "{filename}{rand:6}";
		}
		Regex regex = new Regex("[\\\\\\/\\:\\*\\?\\042\\<\\>\\|]");
		originFileName = regex.Replace(originFileName, "");
		string extension = Path.GetExtension(originFileName);
		string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(originFileName);
		pathFormat = pathFormat.Replace("{filename}", fileNameWithoutExtension);
		pathFormat = new Regex("\\{rand(\\:?)(\\d+)\\}", RegexOptions.Compiled).Replace(pathFormat, delegate(Match match)
		{
			int num2 = 6;
			if (match.Groups.Count > 2)
			{
				num2 = Convert.ToInt32(match.Groups[2].Value);
			}
			Random random = new Random();
			return random.Next((int)Math.Pow(10.0, (double)num2), (int)Math.Pow(10.0, (double)(num2 + 1))).ToString();
		});
		string text = pathFormat;
		DateTime now = DateTime.Now;
		pathFormat = text.Replace("{time}", now.Ticks.ToString());
		string text2 = pathFormat;
		now = DateTime.Now;
		int num = now.Year;
		pathFormat = text2.Replace("{yyyy}", num.ToString());
		string text3 = pathFormat;
		now = DateTime.Now;
		num = now.Year % 100;
		pathFormat = text3.Replace("{yy}", num.ToString("D2"));
		string text4 = pathFormat;
		now = DateTime.Now;
		num = now.Month;
		pathFormat = text4.Replace("{mm}", num.ToString("D2"));
		string text5 = pathFormat;
		now = DateTime.Now;
		num = now.Day;
		pathFormat = text5.Replace("{dd}", num.ToString("D2"));
		string text6 = pathFormat;
		now = DateTime.Now;
		num = now.Hour;
		pathFormat = text6.Replace("{hh}", num.ToString("D2"));
		string text7 = pathFormat;
		now = DateTime.Now;
		num = now.Minute;
		pathFormat = text7.Replace("{ii}", num.ToString("D2"));
		string text8 = pathFormat;
		now = DateTime.Now;
		num = now.Second;
		pathFormat = text8.Replace("{ss}", num.ToString("D2"));
		return pathFormat + extension;
	}
}
