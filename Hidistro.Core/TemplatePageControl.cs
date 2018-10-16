namespace Hidistro.Core
{
	public static class TemplatePageControl
	{
		public static int GetPageCount(int totalRecords, int pageSize)
		{
			int num = 1;
			if (totalRecords % pageSize != 0)
			{
				return totalRecords / pageSize + 1;
			}
			return totalRecords / pageSize;
		}

		public static string GetPageHtml(int pageCount, int pageIndex)
		{
			if (pageIndex < 1)
			{
				pageIndex = 1;
			}
			string str = "<a href='javascript:;' class='prev' href='javascript:void(0);' page='" + (pageIndex - 1) + "'></a>";
			if (pageIndex == 1)
			{
				str = "<a href='javascript:;' class='prev disabled' ></a>";
			}
			string str2 = "";
			str = ((pageCount <= 6) ? (str + TemplatePageControl.PageNumHtmlLessThanTen(pageCount, pageIndex)) : (str + TemplatePageControl.PageNumHtmlMoreThanTen(pageCount, pageIndex)));
			string str3 = "<a href='javascript:;' class='next' href='javascript:void(0);' page='" + (pageIndex + 1) + "'></a>";
			if (pageIndex == pageCount)
			{
				str3 = "<a href='javascript:;' class='next disabled' ></a>";
			}
			return str + str2 + str3;
		}

		public static string PageNumHtmlMoreThanTen(int pageCount, int pageIndex)
		{
			string text = "";
			bool flag = true;
			if (pageIndex < 0)
			{
				pageIndex = 1;
			}
			int num = 1;
			if (pageIndex > 6)
			{
				num = pageIndex - 3;
			}
			int num2 = num + 6 + 1;
			if (num2 >= pageCount)
			{
				num2 = pageCount;
				flag = false;
			}
			int num3 = pageCount;
			if (num2 + 2 > pageCount)
			{
				num3 = 0;
			}
			for (int i = num; i < num2; i++)
			{
				text = ((i != pageIndex) ? (text + "<a href='javascript:void(0);' page='" + i + "' >" + i + "</a>") : (text + "<a class='cur' >" + i + "</a>"));
			}
			if (flag)
			{
				text = text + "<a href='javascript:void(0);' page='" + num2 + "' >.....</a>";
			}
			if (num3 != 0)
			{
				text = text + "<a href='javascript:void(0);' page='" + (num3 - 2) + "' >" + (num3 - 2) + "</a>";
				text = text + "<a href='javascript:void(0);' page='" + (num3 - 1) + "' >" + (num3 - 1) + "</a>";
			}
			return text;
		}

		public static string PageNumHtmlLessThanTen(int pageCount, int pageIndex)
		{
			string text = "";
			for (int i = 1; i <= pageCount; i++)
			{
				text = ((i != pageIndex) ? (text + "<a href='javascript:void(0);' page='" + i + "' >" + i + "</a>") : (text + "<a class='cur' >" + i + "</a>"));
			}
			return text;
		}
	}
}
