using Hidistro.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class CalendarPanel : TextBox
	{
		public Dictionary<string, object> CalendarParameter;

		public bool IsEndDate
		{
			get;
			set;
		}

		public bool NotFloatLeft
		{
			get;
			set;
		}

		public string FunctionNameForChangeDate
		{
			get;
			set;
		}

		public DateTime? SelectedDate
		{
			get
			{
				if (this.IsEndDate && this.Text.ToDateTime().HasValue)
				{
					DateTime dateTime = this.Text.ToDateTime().Value;
					dateTime = dateTime.AddDays(1.0);
					return dateTime.AddSeconds(-1.0);
				}
				return this.Text.ToDateTime();
			}
			set
			{
				if (value.HasValue)
				{
					DateTime value2;
					if (this.CalendarParameter["format"].ToString().ToLower() == "yyyy-mm-dd hh:ii:00" || this.CalendarParameter["format"].ToString().ToLower() == "yyyy-mm-dd hh:ii")
					{
						value2 = value.Value;
						this.Text = value2.ToString("yyyy-MM-dd HH:mm");
					}
					else
					{
						value2 = value.Value;
						this.Text = value2.ToString("yyyy-MM-dd");
					}
				}
			}
		}

		public CalendarPanel()
		{
			this.CalendarParameter = new Dictionary<string, object>();
			this.SetCalendarParameter();
		}

		private void SetCalendarParameter()
		{
			this.CalendarParameter.Add("minView", 2);
			this.CalendarParameter.Add("format", "yyyy-mm-dd");
			this.CalendarParameter.Add("language", "zh-CN");
			this.CalendarParameter.Add("weekStart", 1);
			this.CalendarParameter.Add("todayBtn", 1);
			this.CalendarParameter.Add("autoclose", 1);
			this.CalendarParameter.Add("todayHighlight", 1);
			this.CalendarParameter.Add("startView", 2);
			this.CalendarParameter.Add("forceParse", 0);
			this.CalendarParameter.Add("showMeridian", 1);
		}

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			this.CssClass = (string.IsNullOrEmpty(this.CssClass) ? "forminput form-control" : this.CssClass);
			if (!this.NotFloatLeft)
			{
				base.Style.Add("float", "left");
			}
			base.Attributes.Add("readonly", "readonly");
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("$('#{0}').datetimepicker", this.ClientID);
			stringBuilder.Append("({");
			foreach (KeyValuePair<string, object> item in this.CalendarParameter)
			{
				if (item.Value is int)
				{
					stringBuilder.AppendFormat("{0}:{1},", item.Key, item.Value);
				}
				else if (item.Value is string)
				{
					stringBuilder.AppendFormat("{0}:'{1}',", item.Key, item.Value);
				}
			}
			stringBuilder.Append("  });");
			if (!string.IsNullOrEmpty(this.FunctionNameForChangeDate))
			{
				stringBuilder.AppendFormat("$('#{0}').datetimepicker().on('changeDate',{1});", this.ClientID, this.FunctionNameForChangeDate);
			}
			string script = "$(function () { " + stringBuilder.ToString() + "  });";
			this.Page.ClientScript.RegisterClientScriptBlock(base.GetType(), Guid.NewGuid().ToString(), script, true);
		}
	}
}
