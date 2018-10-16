using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Web.ashxBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace Hidistro.UI.Web.Admin.store.ashx
{
	public class WapTop : AdminBaseHandler
	{
		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			if (string.IsNullOrWhiteSpace(base.action))
			{
				throw new HidistroAshxException("错误的参数");
			}
			switch (base.action)
			{
			case "getlist":
				this.GetList(context);
				break;
			case "delete":
				this.Delete(context);
				break;
			case "setHomePage":
				this.SetHomePage(context);
				break;
			case "cancelHomePage":
				this.CancelHomePage(context);
				break;
			default:
				throw new HidistroAshxException("错误的参数");
			}
		}

		private void GetList(HttpContext context)
		{
			int num = 1;
			int num2 = 10;
			num = base.GetIntParam(context, "page", false).Value;
			if (num < 1)
			{
				num = 1;
			}
			num2 = base.GetIntParam(context, "rows", false).Value;
			if (num2 < 1)
			{
				num2 = 10;
			}
			TopicQuery topicQuery = new TopicQuery();
			topicQuery.Title = context.Request["topicTitle"];
			if (!string.IsNullOrEmpty(context.Request["topicType"]))
			{
				topicQuery.TopicType = base.GetIntParam(context, "topicType", false).Value;
			}
			topicQuery.SortBy = "topicId";
			topicQuery.SortOrder = SortAction.Desc;
			topicQuery.PageIndex = num;
			topicQuery.PageSize = num2;
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList(topicQuery);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetDataList(TopicQuery query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			if (query != null)
			{
				DbQueryResult dbQueryResult = VShopHelper.GettopicList(query);
				dataGridViewModel.rows = DataHelper.DataTableToDictionary(dbQueryResult.Data);
				dataGridViewModel.total = dbQueryResult.TotalRecords;
				foreach (Dictionary<string, object> row in dataGridViewModel.rows)
				{
					TopicInfo topicInfo = row.ToObject<TopicInfo>();
					if (topicInfo.TopicType > 0)
					{
						row.Add("TopicTypeStr", ((Enum)(object)(EnumTopicType)topicInfo.TopicType).ToDescription());
						if (topicInfo.TopicType == 1)
						{
							row.Add("linkUrl", Globals.HostPath(HttpContext.Current.Request.Url) + "/vshop/Topics.aspx?TopicId=" + topicInfo.TopicId);
							row.Add("EidUrl", "../../admin/store/TopicTempEdit.aspx?TopicId=" + topicInfo.TopicId);
						}
						if (topicInfo.TopicType == 2)
						{
							row.Add("linkUrl", Globals.HostPath(HttpContext.Current.Request.Url) + "/appshop/Topics.aspx?TopicId=" + topicInfo.TopicId);
							row.Add("EidUrl", "../../admin/store/AppTopicTempEdit.aspx?TopicId=" + topicInfo.TopicId);
						}
						if (topicInfo.TopicType == 3)
						{
							row.Add("linkUrl", Globals.HostPath(HttpContext.Current.Request.Url) + "/Topics.aspx?TopicId=" + topicInfo.TopicId);
							row.Add("EidUrl", "../../admin/store/PcTopicTempEdit.aspx?TopicId=" + topicInfo.TopicId);
						}
					}
					else
					{
						row.Add("TopicTypeStr", "未知类型");
					}
				}
			}
			return dataGridViewModel;
		}

		private void Delete(HttpContext context)
		{
			int value = base.GetIntParam(context, "TopicId", false).Value;
			if (value <= 0)
			{
				throw new HidistroAshxException("错误的参数");
			}
			TopicInfo topicInfo = VShopHelper.Gettopic(value);
			if (topicInfo != null)
			{
				if (VShopHelper.Deletetopic(value))
				{
					string empty = string.Empty;
					string empty2 = string.Empty;
					if (topicInfo.TopicType == 1)
					{
						empty = context.Server.MapPath("/Templates/topic/waptopic/topic_" + value + ".json");
						empty2 = context.Server.MapPath("/Templates/topic/waptopic/Skin-TopicHomePage_" + value + ".html");
					}
					if (topicInfo.TopicType == 3)
					{
						empty = context.Server.MapPath("/Templates/topic/pctopic/pctopic_" + value + ".json");
						empty2 = context.Server.MapPath("/Templates/topic/pctopic/Skin-PcTopicHomePage_" + value + ".html");
					}
					else
					{
						empty = context.Server.MapPath("/Templates/topic/apptopic/apptopic_" + value + ".json");
						empty2 = context.Server.MapPath("/Templates/topic/apptopic/Skin-ApptopicHomePage_" + value + ".html");
					}
					File.Delete(empty);
					File.Delete(empty2);
					base.ReturnSuccessResult(context, "删除成功！", 0, true);
				}
				return;
			}
			throw new HidistroAshxException("错误的参数");
		}

		private void SetHomePage(HttpContext context)
		{
			int num = context.Request["topicId"].ToInt(0);
			if (num <= 0)
			{
				throw new HidistroAshxException("错误的参数");
			}
			if (VShopHelper.SetHomePage(num))
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				masterSettings.HomePageTopicId = num;
				SettingsManager.Save(masterSettings);
				base.ReturnSuccessResult(context, "设置成功", 0, true);
				return;
			}
			throw new HidistroAshxException("设置失败");
		}

		private void CancelHomePage(HttpContext context)
		{
			int num = context.Request["topicId"].ToInt(0);
			if (num <= 0)
			{
				throw new HidistroAshxException("错误的参数");
			}
			if (VShopHelper.CancelHomePage(num))
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				masterSettings.HomePageTopicId = 0;
				SettingsManager.Save(masterSettings);
				base.ReturnSuccessResult(context, "取消成功", 0, true);
				return;
			}
			throw new HidistroAshxException("取消失败");
		}
	}
}
