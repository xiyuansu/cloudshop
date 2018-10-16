using Hidistro.Context;
using Hidistro.Core;
using Hidistro.SaleSystem.Members;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web;

namespace Hidistro.UI.Web.Handler
{
	public class MemberStatistics : IHttpHandler
	{
		private HttpContext context;

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "text/plain";
			try
			{
				string text = context.Request["action"];
				if (string.IsNullOrEmpty(text))
				{
					context.Response.Write("参数错误");
				}
				else
				{
					this.context = context;
					string a = text;
					if (!(a == "MemberGroupStatistics"))
					{
						if (a == "SetActiveConsumeTimes")
						{
							this.SetActiveConsumeTimes();
						}
					}
					else
					{
						this.MemberGroupStatistics();
					}
				}
			}
			catch (Exception ex)
			{
				context.Response.Write(ex.Message.ToString());
			}
		}

		public void MemberGroupStatistics()
		{
			try
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				IDictionary<string, int> memberCount = MemberHelper.GetMemberCount(masterSettings.ConsumeTimesInOneMonth, masterSettings.ConsumeTimesInThreeMonth, masterSettings.ConsumeTimesInSixMonth);
				memberCount.Add("ActiveMemberTotal", memberCount["ActiveInOneMonth"] + memberCount["ActiveInThreeMonth"] + memberCount["ActiveInSixMonth"]);
				memberCount.Add("DormancyMemberTotal", memberCount["DormancyInOneMonth"] + memberCount["DormancyInThreeMonth"] + memberCount["DormancyInSixMonth"] + memberCount["DormancyInNineMonth"] + memberCount["DormancyInOneYear"]);
				memberCount.Add("ConsumeTimesInOneMonth", masterSettings.ConsumeTimesInOneMonth);
				memberCount.Add("ConsumeTimesInThreeMonth", masterSettings.ConsumeTimesInThreeMonth);
				memberCount.Add("ConsumeTimesInSixMonth", masterSettings.ConsumeTimesInSixMonth);
				string str = JsonConvert.SerializeObject(memberCount);
				this.context.Response.Write("{\"Status\":\"Success\",\"Data\":" + str + "}");
			}
			catch (Exception)
			{
				this.context.Response.Write("{\"Status\":\"Failure\"}");
			}
		}

		public void SetActiveConsumeTimes()
		{
			int consumeTimesInOneMonth = this.context.Request["ConsumeTimesInOneMonth"].ToInt(0);
			int consumeTimesInThreeMonth = this.context.Request["ConsumeTimesInThreeMonth"].ToInt(0);
			int consumeTimesInSixMonth = this.context.Request["ConsumeTimesInSixMonth"].ToInt(0);
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			masterSettings.ConsumeTimesInOneMonth = consumeTimesInOneMonth;
			masterSettings.ConsumeTimesInThreeMonth = consumeTimesInThreeMonth;
			masterSettings.ConsumeTimesInSixMonth = consumeTimesInSixMonth;
			try
			{
				SettingsManager.Save(masterSettings);
				this.context.Response.Write("{\"Status\":\"Success\"}");
			}
			catch (Exception)
			{
				this.context.Response.Write("{\"Status\":\"Failure\"}");
			}
		}
	}
}
