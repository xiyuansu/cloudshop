using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Promotions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Handler
{
	public class RegionHandler : IHttpHandler
	{
		private static object Couponslock = new object();

		private static object PointCouponslock = new object();

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public void ProcessRequest(HttpContext context)
		{
			try
			{
				string text = context.Request["action"];
				switch (text)
				{
				case "GetRegionsOfProvinceCity":
					this.GetRegionsOfProvinceCity(context);
					break;
				case "GetRegionsOfProvinceCounty":
					this.GetRegionsOfProvinceCounty(context);
					break;
				case "getregions":
					RegionHandler.GetRegions(context);
					break;
				case "getregioninfo":
					this.GetRegionInfo(context);
					break;
				case "getregionid":
					this.GetRegionId(context);
					break;
				case "GetCityByRegionName":
					this.GetCityByRegionName(context);
					break;
				case "openOnlineService":
					this.openOnlineService(context);
					break;
				case "openCnzz":
					this.openCnzz(context);
					break;
				case "ShowCoupons":
					this.ShowCoupons(context);
					break;
				case "SendCoupons":
					this.SendCoupons(context);
					break;
				case "SendPointCoupons":
					this.SendPointCoupons(context);
					break;
				case "AddRegion":
					this.AddRegion(context);
					break;
				case "EditRegion":
					this.EditRegion(context);
					break;
				case "DelRegion":
					this.DelRegion(context);
					break;
				case "HasChild":
					this.HasChild(context);
					break;
				case "ReSetRegionData":
					this.ReSetRegionData(context);
					break;
				case "GetStreets":
					RegionHandler.GetStreets(context);
					break;
				case "BuildJson":
					this.BuildJson(context);
					break;
				}
			}
			catch
			{
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

		private void BuildJson(HttpContext context)
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
							IDictionary<int, string> streets = RegionHelper.GetStreets(item4.Key, false);
							if (streets != null && streets.Count > 0)
							{
								foreach (KeyValuePair<int, string> item5 in streets)
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
									stringBuilder3.Append($"\"id\":\"{item5.Key}\",");
									stringBuilder3.Append($"\"name\":\"{item5.Value}\"");
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
			context.Response.ContentType = "application/json";
			context.Response.Write("{\"Status\":\"true\"}");
		}

		private void AddRegion(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			string szJson = context.Request["DataJson"];
			Hidistro.Entities.Store.RegionInfo regionInfo = JsonHelper.ParseFormJson<Hidistro.Entities.Store.RegionInfo>(szJson);
			int num2;
			if (!string.IsNullOrEmpty(regionInfo.RegionName))
			{
				int num = regionInfo.ParentRegionId;
				if (!string.IsNullOrEmpty(num.ToString()))
				{
					num = regionInfo.Depth;
					if (!string.IsNullOrEmpty(num.ToString()))
					{
						num2 = (string.IsNullOrEmpty(regionInfo.FullRegionPath) ? 1 : 0);
						goto IL_0071;
					}
				}
			}
			num2 = 1;
			goto IL_0071;
			IL_0071:
			if (num2 != 0)
			{
				context.Response.Write("{\"Status\":\"0\"}");
			}
			else if (RegionHelper.IsSameName(regionInfo.RegionName, regionInfo.ParentRegionId, 0))
			{
				context.Response.Write("{\"Status\":\"same\"}");
			}
			else
			{
				bool flag = RegionHelper.AddRegion(regionInfo);
				context.Response.Write("{\"Status\":\"" + flag.ToString() + "\"}");
			}
		}

		private void EditRegion(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			int regionId = 0;
			string text = context.Request["RegionName"];
			if (!int.TryParse(context.Request["RegionId"], out regionId) || string.IsNullOrEmpty(text))
			{
				context.Response.Write("{\"Status\":\"0\"}");
			}
			else
			{
				Hidistro.Entities.Store.RegionInfo regionByRegionId = RegionHelper.GetRegionByRegionId(regionId);
				if (regionByRegionId == null)
				{
					context.Response.Write("{\"Status\":\"-1\"}");
				}
				else if (RegionHelper.IsSameName(text, regionByRegionId.ParentRegionId, regionByRegionId.RegionId))
				{
					context.Response.Write("{\"Status\":\"same\"}");
				}
				else
				{
					bool flag = RegionHelper.UpdateRegionName(regionId, text);
					context.Response.Write("{\"Status\":\"" + flag.ToString() + "\"}");
				}
			}
		}

		private void DelRegion(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			int regionId = 0;
			if (!int.TryParse(context.Request["RegionId"], out regionId))
			{
				context.Response.Write("{\"Status\":\"0\"}");
			}
			else
			{
				bool flag = RegionHelper.DeleteRegions(regionId);
				HiCache.Remove("FileCache-Regions");
				context.Response.Write("{\"Status\":\"" + flag.ToString() + "\"}");
			}
		}

		private void HasChild(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			int regionId = 0;
			if (!int.TryParse(context.Request["RegionId"], out regionId))
			{
				context.Response.Write("{\"Status\":\"0\"}");
			}
			else
			{
				bool flag = RegionHelper.HasChild(regionId);
				context.Response.Write("{\"Status\":\"" + flag.ToString() + "\"}");
			}
		}

		private void ReSetRegionData(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			bool flag = false;
			try
			{
				flag = RegionHelper.ReSetRegionData();
			}
			catch (Exception ex)
			{
				Globals.WriteExceptionLog(ex, null, "ReSetRegionData3");
			}
			context.Response.Write("{\"Status\":\"" + flag.ToString() + "\"}");
		}

		private void GetRegionsOfProvinceCity(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			string text = context.Request["parentRegionId"];
			IList<Hidistro.Entities.Store.RegionInfo> allProvinceLists = RegionHelper.GetAllProvinceLists(false);
			string s = JsonConvert.SerializeObject(new
			{
				province = from p in allProvinceLists
				select new
				{
					id = p.RegionId,
					name = p.RegionName,
					city = from c in RegionHelper.GetRegionChildList(p.RegionId, false)
					select new
					{
						id = c.RegionId,
						name = c.RegionName
					}
				}
			});
			context.Response.Write(s);
		}

		private void GetRegionsOfProvinceCounty(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			IList<Hidistro.Entities.Store.RegionInfo> allProvinceLists = RegionHelper.GetAllProvinceLists(false);
			string s = JsonConvert.SerializeObject(new
			{
				province = from p in allProvinceLists
				select new
				{
					id = p.RegionId,
					name = p.RegionName,
					city = from c in RegionHelper.GetRegionChildList(p.RegionId, false)
					select new
					{
						id = c.RegionId,
						name = c.RegionName,
						county = from d in RegionHelper.GetRegionChildList(c.RegionId, false)
						select new
						{
							id = d.RegionId,
							name = d.RegionName
						}
					}
				}
			});
			context.Response.Write(s);
		}

		private void GetRegionId(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			int num = 0;
			int num2 = 0;
			string text = context.Request["county"];
			string text2 = context.Request["city"];
			string text3 = context.Request["province"];
			if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(text2) || string.IsNullOrEmpty(text3))
			{
				num = 1;
			}
			else
			{
				num2 = RegionHelper.GetRegionId(text, text2, text3);
			}
			string text4 = "{";
			text4 = text4 + "\"Status\":\"" + num + "\",";
			text4 = text4 + "\"RegionId\":\"" + num2.ToString(CultureInfo.InvariantCulture) + "\"";
			text4 += "}";
			context.Response.Write(text4);
		}

		private void GetCityByRegionName(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			bool flag = false;
			string text = context.Request["city"];
			string text2 = context.Request["address"];
			if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(text2) || text2.IndexOf(text) == -1)
			{
				context.Response.Write("{\"Status\":\"0\"}");
			}
			else
			{
				string text3 = "";
				if (text2.IndexOf(text) > -1 && text2.IndexOf(text) + text.Length < text2.Length)
				{
					text3 = text2.Substring(text2.IndexOf(text) + text.Length);
					if (text3.IndexOf("区") > -1 || text3.IndexOf("县") > -1 || text3.IndexOf("市") > -1 || text3.IndexOf("街道") > -1 || text3.IndexOf("镇") > -1 || text3.IndexOf("村") > -1)
					{
						char[] anyOf = new char[5]
						{
							'区',
							'县',
							'市',
							'镇',
							'村'
						};
						int num = text3.LastIndexOfAny(anyOf);
						bool flag2 = false;
						if (num <= -1)
						{
							flag2 = true;
							num = text3.LastIndexOf("街道");
						}
						if (num > -1)
						{
							text3 = text3.Substring(0, num + ((!flag2) ? 1 : 2));
						}
					}
				}
				Hidistro.Entities.Store.RegionInfo regionInfo = RegionHelper.GetCityByRegionName(text, text3);
				if (regionInfo == null)
				{
					int regionIdByRegionName = RegionHelper.GetRegionIdByRegionName(text, 2);
					regionInfo = RegionHelper.GetRegionByRegionId(regionIdByRegionName);
				}
				else
				{
					flag = true;
				}
				if (regionInfo != null)
				{
					string fullRegion = RegionHelper.GetFullRegion(regionInfo.RegionId, " ", true, 0);
					string text4 = text2.Substring(0, text2.IndexOf(regionInfo.RegionName) + regionInfo.RegionName.Length);
					string str = text2.Substring(text2.IndexOf(regionInfo.RegionName) + regionInfo.RegionName.Length);
					string str2 = "{";
					str2 += "\"Status\":\"OK\",";
					str2 = str2 + "\"RegionId\":\"" + regionInfo.RegionId.ToString(CultureInfo.InvariantCulture) + "\",";
					str2 = str2 + "\"County\":\"" + fullRegion + "\",";
					str2 = str2 + "\"Address\":\"" + str + "\",";
					str2 = str2 + "\"IsLocateArea\":\"" + flag.ToString() + "\"";
					str2 += "}";
					context.Response.Write(str2);
				}
				else
				{
					string str3 = "{";
					str3 += "\"Status\":\"OK\",";
					str3 = str3 + "\"RegionId\":\"" + 0 + "\",";
					str3 = str3 + "\"County\":\"" + text + "\",";
					str3 = str3 + "\"Address\":\"" + text2 + "\",";
					str3 = str3 + "\"IsLocateArea\":\"" + flag.ToString() + "\"";
					str3 += "}";
					context.Response.Write(str3);
				}
			}
		}

		private void GetRegionInfo(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			int num = 0;
			int.TryParse(context.Request["regionId"], out num);
			if (num <= 0)
			{
				context.Response.Write("{\"Status\":\"0\"}");
			}
			else
			{
				Hidistro.Entities.Store.RegionInfo region = RegionHelper.GetRegion(num, true);
				if (region == null)
				{
					context.Response.Write("{\"Status\":\"0\"}");
				}
				else
				{
					string fullRegion = RegionHelper.GetFullRegion(num, ",", true, 0);
					int depth = region.Depth;
					string str = (depth > 1) ? RegionHelper.GetFullPath(num, true) : "";
					string str2 = "";
					if (region.Depth != 1)
					{
						str2 = region.ParentRegionId.ToString();
					}
					string str3 = "{";
					str3 += "\"Status\":\"OK\",";
					str3 = str3 + "\"RegionId\":\"" + num.ToString(CultureInfo.InvariantCulture) + "\",";
					str3 = str3 + "\"RegionName\":\"" + fullRegion + "\",";
					str3 = str3 + "\"Depth\":\"" + depth.ToString(CultureInfo.InvariantCulture) + "\",";
					str3 = str3 + "\"Path\":\"" + str + "\",";
					str3 = str3 + "\"ParentId\":\"" + str2 + "\"";
					str3 += "}";
					context.Response.Write(str3);
				}
			}
		}

		private static void GetRegions(HttpContext context)
		{
			try
			{
				context.Response.ContentType = "application/json";
				int num = 0;
				int.TryParse(context.Request["parentId"], out num);
				int num2 = 1;
				IDictionary<int, string> dictionary;
				if (num == 0)
				{
					dictionary = RegionHelper.GetAllProvinces(false);
					num2 = 1;
				}
				else
				{
					Hidistro.Entities.Store.RegionInfo region = RegionHelper.GetRegion(num, true);
					if (region == null)
					{
						context.Response.Write("{\"Status\":\"0\"}");
						goto end_IL_0001;
					}
					num2 = region.Depth + 1;
					dictionary = ((region.Depth != 1) ? ((region.Depth != 2) ? RegionHelper.GetStreets(num, false) : RegionHelper.GetCountys(num, false)) : RegionHelper.GetCitys(num, false));
					if (dictionary == null || dictionary.Count == 0)
					{
						context.Response.Write("{\"Status\":\"0\"}");
						goto end_IL_0001;
					}
				}
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("{");
				stringBuilder.Append("\"Status\":\"OK\",");
				stringBuilder.Append("\"Regions\":[");
				foreach (int key in dictionary.Keys)
				{
					stringBuilder.Append("{");
					stringBuilder.AppendFormat("\"RegionId\":\"{0}\",", key.ToString(CultureInfo.InvariantCulture));
					stringBuilder.AppendFormat("\"RegionName\":\"{0}\"", dictionary[key]);
					stringBuilder.Append("},");
				}
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
				stringBuilder.Append("]}");
				dictionary.Clear();
				context.Response.Write(stringBuilder.ToString());
				end_IL_0001:;
			}
			catch (Exception ex)
			{
				context.Response.Write(ex.Message);
			}
		}

		private static void GetStreets(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			int num = 0;
			int.TryParse(context.Request["parentId"], out num);
			if (num > 0)
			{
				IList<Hidistro.Entities.Store.RegionInfo> regionsByParent = RegionHelper.GetRegionsByParent(num);
				StringBuilder stringBuilder = new StringBuilder();
				if (regionsByParent.Count == 0)
				{
					context.Response.Write("{\"Status\":\"0\"}");
				}
				else
				{
					stringBuilder.Append("{");
					stringBuilder.Append("\"Status\":\"OK\",");
					stringBuilder.Append("\"Regions\":[");
					foreach (Hidistro.Entities.Store.RegionInfo item in regionsByParent)
					{
						stringBuilder.Append("{");
						stringBuilder.AppendFormat("\"RegionId\":\"{0}\",", item.RegionId.ToString(CultureInfo.InvariantCulture));
						stringBuilder.AppendFormat("\"RegionName\":\"{0}\"", item.RegionName);
						stringBuilder.Append("},");
					}
					stringBuilder.Remove(stringBuilder.Length - 1, 1);
					stringBuilder.Append("]}");
					context.Response.Write(stringBuilder.ToString());
				}
			}
			else
			{
				context.Response.Write("{\"Status\":\"0\"}");
			}
		}

		private void openOnlineService(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			bool flag = context.Request["state"] != null && context.Request["state"].ToBool();
			int num = 0;
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			masterSettings.ServiceIsOpen = (flag ? "1" : "0");
			num = masterSettings.ServiceIsOpen.ToInt(0);
			if (masterSettings.ServiceIsOpen == "1")
			{
				if (masterSettings.MeiQiaActivated == "1")
				{
					num = 2;
				}
				masterSettings.MeiQiaActivated = "0";
			}
			SettingsManager.Save(masterSettings);
			context.Response.Write("{\"Status\":\"" + num + "\"}");
			context.Response.End();
		}

		private void openCnzz(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			bool enabledCnzz = context.Request["state"] != null && context.Request["state"].ToBool();
			int num = 0;
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			masterSettings.EnabledCnzz = enabledCnzz;
			num = (masterSettings.EnabledCnzz ? 1 : 0);
			SettingsManager.Save(masterSettings);
			context.Response.Write("{\"Status\":\"" + num + "\"}");
			context.Response.End();
		}

		private void ShowCoupons(HttpContext context)
		{
			int num = context.Request["ProductId"].ToInt(0);
			bool flag = context.Request["IsGroup"].ToBool();
			bool flag2 = context.Request["IsPanicBuying"].ToBool();
			bool useWithFireGroup = context.Request["IsFireGroup"].ToBool();
			StringBuilder stringBuilder = new StringBuilder();
			if (flag)
			{
				GroupBuyInfo groupBuy = PromoteHelper.GetGroupBuy(num);
				num = (groupBuy?.ProductId ?? 0);
			}
			if (flag2)
			{
				CountDownInfo countDownInfo = PromoteHelper.GetCountDownInfo(num, 0);
				num = (countDownInfo?.ProductId ?? 0);
			}
			DataTable couponList = CouponHelper.GetCouponList(num, HiContext.Current.UserId, flag, flag2, useWithFireGroup);
			if (couponList.Rows.Count > 0)
			{
				foreach (DataRow row in couponList.Rows)
				{
					decimal num2 = row["Price"].ToDecimal(0);
					string text = "";
					text = ((!(num2 <= 5m)) ? ((!(num2 <= 50m)) ? "Tag3" : "Tag2") : "Tag1");
					try
					{
						StringBuilder stringBuilder2 = stringBuilder;
						object[] obj = new object[6]
						{
							text,
							row["CouponId"].ToNullString(),
							num2.F2ToString("f2"),
							string.IsNullOrEmpty(row["CanUseProducts"].ToNullString().Trim()) ? "通用" : "部分",
							(row["OrderUseLimit"].ToDecimal(0) == decimal.Zero) ? "无限制" : ("满" + row["OrderUseLimit"].ToDecimal(0).F2ToString("f2") + "元使用"),
							null
						};
						object str;
						DateTime value;
						if (!row["StartTime"].ToDateTime().HasValue)
						{
							str = "";
						}
						else
						{
							value = row["StartTime"].ToDateTime().Value;
							str = value.ToString("yyyy.MM.dd");
						}
						object str2;
						if (!row["ClosingTime"].ToDateTime().HasValue)
						{
							str2 = "";
						}
						else
						{
							value = row["ClosingTime"].ToDateTime().Value;
							str2 = value.ToString("yyyy.MM.dd");
						}
						obj[5] = (string)str + "至" + (string)str2;
						stringBuilder2.AppendFormat("{{ \"LiId\": \"{0}\", \"CId\": \"{1}\", \"Price\": \"{2}\", \"CanUseProducts\": \"{3}\", \"OrderUseLimit\": \"{4}\", \"SCTime\": \"{5}\"}},", obj);
					}
					catch (Exception ex)
					{
						string message = ex.Message;
					}
				}
			}
			context.Response.Write("{\"TotalRecords\":\"" + couponList.Rows.Count + "\",\"Data\":[" + stringBuilder.ToString().TrimEnd(',') + "]}");
			context.Response.End();
		}

		private void SendCoupons(HttpContext context)
		{
			if (HiContext.Current.UserId == 0)
			{
				context.Response.Write("{\"Status\":\"0\"}");
			}
			else
			{
				int num = context.Request["CouponId"].ToInt(0);
				if (num > 0)
				{
					lock (RegionHandler.Couponslock)
					{
						switch (CouponHelper.AddCouponItemInfo(HiContext.Current.User, num))
						{
						case CouponActionStatus.Success:
						{
							CouponInfo eFCoupon2 = CouponHelper.GetEFCoupon(num);
							int couponObtainNum = CouponHelper.GetCouponObtainNum(num, HiContext.Current.UserId);
							if (couponObtainNum < eFCoupon2.UserLimitCount || eFCoupon2.UserLimitCount == 0)
							{
								context.Response.Write("{\"Status\":\"2\"}");
							}
							else
							{
								context.Response.Write("{\"Status\":\"1\"}");
							}
							break;
						}
						case CouponActionStatus.NotExists:
							context.Response.Write("{\"Status\":\"-1\", \"Error\":\"该优惠券已下线\"}");
							break;
						case CouponActionStatus.Overdue:
							context.Response.Write("{\"Status\":\"-1\", \"Error\":\"该优惠券已过期\"}");
							break;
						case CouponActionStatus.InadequateInventory:
							context.Response.Write("{\"Status\":\"-1\", \"Error\":\"该优惠券被领取完了\"}");
							break;
						case CouponActionStatus.CannotReceive:
						{
							CouponInfo eFCoupon = CouponHelper.GetEFCoupon(num);
							context.Response.Write("{\"Status\":\"-1\", \"Error\":\"你好，该优惠券每人只能兑换" + eFCoupon.UserLimitCount + "张\"}");
							break;
						}
						default:
							context.Response.Write("{\"Status\":\"-1\", \"Error\":\"未知错误\"}");
							break;
						}
					}
				}
				else
				{
					context.Response.Write("{\"Status\":\"-1\", \"Error\":\"未知错误\"}");
				}
			}
			context.Response.End();
		}

		private void SendPointCoupons(HttpContext context)
		{
			if (HiContext.Current.UserId == 0)
			{
				context.Response.Write("{\"Status\":\"0\"}");
			}
			else
			{
				int num = context.Request["CouponId"].ToInt(0);
				int num2 = context.Request["NeedPoints"].ToInt(0);
				if (num > 0)
				{
					lock (RegionHandler.PointCouponslock)
					{
						switch (CouponHelper.AddCouponItemInfo(HiContext.Current.User, num))
						{
						case CouponActionStatus.Success:
							context.Response.Write("{\"Status\":\"1\"}");
							break;
						case CouponActionStatus.PointNotEnough:
							context.Response.Write("{\"Status\":\"-1\", \"Error\":\"您的积分不足\"}");
							break;
						case CouponActionStatus.NotExists:
							context.Response.Write("{\"Status\":\"-1\", \"Error\":\"该优惠券已下线\"}");
							break;
						case CouponActionStatus.Overdue:
							context.Response.Write("{\"Status\":\"-1\", \"Error\":\"该优惠券已过期\"}");
							break;
						case CouponActionStatus.InadequateInventory:
							context.Response.Write("{\"Status\":\"-1\", \"Error\":\"该优惠券被领取完了\"}");
							break;
						case CouponActionStatus.CannotReceive:
						{
							CouponInfo eFCoupon = CouponHelper.GetEFCoupon(num);
							context.Response.Write("{\"Status\":\"-1\", \"Error\":\"你好，该优惠券每人只能兑换" + eFCoupon.UserLimitCount + "张\"}");
							break;
						}
						default:
							context.Response.Write("{\"Status\":\"-1\", \"Error\":\"未知错误\"}");
							break;
						}
					}
				}
				else
				{
					context.Response.Write("{\"Status\":\"-1\", \"Error\":\"未知错误\"}");
				}
			}
			context.Response.End();
		}
	}
}
