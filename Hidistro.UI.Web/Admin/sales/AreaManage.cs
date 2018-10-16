using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web.Admin.sales
{
	[PrivilegeCheck(Privilege.AreaManage)]
	public class AreaManage : AdminPage
	{
		protected HtmlAnchor btnBuildJson;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnBuildJson.Visible = HiContext.Current.SiteSettings.OpenMultStore;
			if (!this.Page.IsPostBack)
			{
				string a = base.GetParameter("action").ToNullString().ToLower();
				if (a == "buildjson")
				{
					this.BuildJson();
				}
			}
		}

		private IDictionary<int, string> GetRegionArea()
		{
			IDictionary<int, string> dictionary = new Dictionary<int, string>();
			dictionary.Add(1, "华东");
			dictionary.Add(2, "华北");
			dictionary.Add(3, "华中");
			dictionary.Add(4, "华南");
			dictionary.Add(5, "东北");
			dictionary.Add(6, "西北");
			dictionary.Add(7, "西南");
			dictionary.Add(8, "港澳台");
			dictionary.Add(9, "海外");
			return dictionary;
		}

		public void BuildJson()
		{
			DateTime now = DateTime.Now;
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			StringBuilder stringBuilder3 = new StringBuilder();
			stringBuilder3.Append("{");
			stringBuilder3.Append("\"province\":[");
			int num = 9;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			IDictionary<int, string> regionArea = this.GetRegionArea();
			foreach (KeyValuePair<int, string> item in regionArea)
			{
				IDictionary<int, string> provinces = RegionHelper.GetProvinces(item.Key, false);
				foreach (KeyValuePair<int, string> item2 in provinces)
				{
					num++;
					num2 = 0;
					if (num > 10)
					{
						stringBuilder3.Append(",{");
					}
					else
					{
						stringBuilder3.Append("{");
					}
					stringBuilder3.Append($"\"id\":\"{item2.Key}\",");
					stringBuilder3.Append($"\"name\":\"{item2.Value}\",");
					stringBuilder3.Append("\"city\":[");
					IDictionary<int, string> citys = RegionHelper.GetCitys(item2.Key, false);
					foreach (KeyValuePair<int, string> item3 in citys)
					{
						num2++;
						if (num2 > 1)
						{
							stringBuilder3.Append(",{");
						}
						else
						{
							stringBuilder3.Append("{");
						}
						stringBuilder3.Append($"\"id\":\"{item3.Key}\",");
						stringBuilder3.Append($"\"name\":\"{item3.Value}\",");
						stringBuilder3.Append("\"county\":[");
						num3 = 0;
						IDictionary<int, string> countys = RegionHelper.GetCountys(item3.Key, false);
						foreach (KeyValuePair<int, string> item4 in countys)
						{
							num3++;
							if (num3 > 1)
							{
								stringBuilder3.Append(",{");
							}
							else
							{
								stringBuilder3.Append("{");
							}
							stringBuilder3.Append($"\"id\":\"{item4.Key}\",");
							stringBuilder3.Append($"\"name\":\"{item4.Value}\",");
							stringBuilder3.Append("\"street\":[");
							num4 = 0;
							IList<RegionInfo> streetsFromDB = RegionHelper.GetStreetsFromDB(item4.Key, false);
							if (streetsFromDB != null && streetsFromDB.Count > 0)
							{
								foreach (RegionInfo item5 in streetsFromDB)
								{
									num4++;
									if (num4 > 1)
									{
										stringBuilder3.Append(",{");
									}
									else
									{
										stringBuilder3.Append("{");
									}
									stringBuilder3.Append($"\"id\":\"{item5.RegionId}\",");
									stringBuilder3.Append($"\"name\":\"{item5.RegionName}\"");
									stringBuilder3.Append("}");
								}
							}
							stringBuilder3.Append("]}");
						}
						stringBuilder3.Append("]}");
					}
					stringBuilder3.Append("]}");
				}
			}
			stringBuilder3.Append("]}");
			using (StreamWriter streamWriter = File.CreateText(HttpContext.Current.Request.MapPath("/config/region.js")))
			{
				streamWriter.WriteLine(stringBuilder3.ToString());
				streamWriter.Flush();
				streamWriter.Close();
			}
			this.ShowMsg("同步数据成功", true, "AreaManage.aspx");
		}
	}
}
