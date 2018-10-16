using Hidistro.Core.Enums;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Vshop;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_PrizeUsers : WebControl
	{
		public LotteryActivityInfo Activity
		{
			get;
			set;
		}

		public int ActivityId
		{
			get;
			set;
		}

		protected override void Render(HtmlTextWriter writer)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.Activity != null)
			{
				PrizeQuery prizeQuery = new PrizeQuery();
				prizeQuery.ActivityId = this.Activity.ActivityId;
				prizeQuery.SortOrder = SortAction.Desc;
				prizeQuery.SortBy = "PrizeTime";
				IOrderedEnumerable<PrizeRecordInfo> orderedEnumerable = from a in VshopBrowser.GetPrizeList(prizeQuery)
				orderby a.PrizeTime descending
				select a;
				if (orderedEnumerable != null && orderedEnumerable.Count() > 0)
				{
					foreach (PrizeRecordInfo item in orderedEnumerable)
					{
						if (!string.IsNullOrEmpty(item.CellPhone) && !string.IsNullOrEmpty(item.RealName))
						{
							stringBuilder.AppendFormat("<p>{0}&nbsp;&nbsp;{1} &nbsp;&nbsp;{2}</p>", item.Prizelevel, this.ShowCellPhone(item.CellPhone), item.RealName);
						}
					}
				}
				else
				{
					stringBuilder.AppendFormat("<p>暂无获奖名单！</p>");
				}
			}
			else if (this.ActivityId > 0)
			{
				PrizeQuery prizeQuery2 = new PrizeQuery();
				prizeQuery2.ActivityId = this.ActivityId;
				prizeQuery2.PageIndex = 1;
				prizeQuery2.PageSize = 20;
				prizeQuery2.SortBy = "CreateDate";
				prizeQuery2.SortOrder = SortAction.Desc;
				prizeQuery2.IsDel = true;
				IEnumerable<ViewUserAwardRecordsInfo> models = ActivityHelper.GetAllAwardRecordsByActityId(prizeQuery2).Models;
				if (models != null && models.Count() > 0)
				{
					foreach (ViewUserAwardRecordsInfo item2 in models)
					{
						if (!string.IsNullOrEmpty(item2.UserName))
						{
							stringBuilder.AppendFormat("<p>{0}&nbsp;&nbsp;{1}</p>", this.ShowUserName(item2.UserName), this.CapitalLetters(item2.AwardGrade) + "等奖");
						}
					}
				}
				else
				{
					stringBuilder.AppendFormat("<p>暂无获奖名单！</p>");
				}
			}
			writer.Write(stringBuilder.ToString());
		}

		private string ShowCellPhone(string phone)
		{
			if (!string.IsNullOrEmpty(phone))
			{
				return Regex.Replace(phone, "(?im)(\\d{3})(\\d{4})(\\d{4})", "$1****$3");
			}
			return "";
		}

		private string ShowUserName(string UserName)
		{
			if (!string.IsNullOrEmpty(UserName) && UserName.Length >= 2)
			{
				return UserName.Substring(0, 2) + "******";
			}
			return UserName + "*******";
		}

		public string CapitalLetters(int Num)
		{
			switch (Num)
			{
			case 1:
				return "一";
			case 2:
				return "二";
			case 3:
				return "三";
			case 4:
				return "四";
			case 5:
				return "五";
			default:
				return "六";
			}
		}
	}
}
