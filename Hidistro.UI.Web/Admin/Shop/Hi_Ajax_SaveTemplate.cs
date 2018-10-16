using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hishop.Components.Validation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Admin.Shop
{
	public class Hi_Ajax_SaveTemplate : IHttpHandler
	{
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
				context.Response.ContentType = "text/plain";
				string text = context.Request.Form["content"];
				string text2 = context.Request.Form["client"];
				JObject jObject = (JObject)JsonConvert.DeserializeObject(text);
				if (HiContext.Current.Manager == null || HiContext.Current.ManagerId <= 0)
				{
					context.Response.Write("{\"status\":0,\"msg\":\"请先登录后台进行操作\"}");
				}
				else
				{
					string text3 = "保存成功";
					string text4 = "1";
					int num = 0;
					string str = "";
					try
					{
						string empty = string.Empty;
						string empty2 = string.Empty;
						string empty3 = string.Empty;
						string text5 = "";
						string str2 = "";
						string text6 = "";
						string str3 = "";
						string str4 = "";
						string text7 = "";
						if (text2.ToLower().Trim() != "appshop" && text2.ToLower().Trim() != "xcxshop")
						{
							text5 = jObject["page"]["title"].ToString();
						}
						if (text2.ToLower().Trim() == "topic" || text2.ToLower().Trim() == "apptopic" || text2.ToLower().Trim() == "pctopic")
						{
							string text8 = context.Request["topicId"];
							if (string.IsNullOrEmpty(text8))
							{
								text8 = "0";
							}
							int.TryParse(text8, out num);
							string text9 = "";
							string text10 = "";
							try
							{
								if (text2.ToLower().Trim() == "pctopic")
								{
									str2 = jObject["page"]["backgroundColor"].ToString();
									text6 = jObject["page"]["backgroundImg"].ToString();
									text7 = this.UploadImage(text6);
									text = text.Replace(text6, text7);
									str3 = jObject["page"]["fillingMethod"].ToString();
									str4 = jObject["page"]["bgAlign"].ToString();
								}
								text9 = jObject["page"]["describe"].ToString();
								text10 = jObject["page"]["sharepic"].ToString();
							}
							catch (Exception)
							{
							}
							text9 = ((!string.IsNullOrWhiteSpace(text9)) ? text9 : "专题分享");
							text10 = (string.IsNullOrEmpty(text10) ? "/Templates/common/images/topicShare.jpg" : this.UploadImage(text10));
							if (num == 0)
							{
								TopicInfo topicInfo = new TopicInfo();
								topicInfo.Title = text5;
								topicInfo.Description = text9;
								topicInfo.AddedDate = DateTime.Now;
								topicInfo.SharePic = text10;
								if (text2.ToLower().Trim() == "topic")
								{
									topicInfo.TopicType = 1;
								}
								else if (text2.ToLower().Trim() == "pctopic")
								{
									topicInfo.TopicType = 3;
								}
								else
								{
									topicInfo.TopicType = 2;
								}
								ValidationResults validationResults = Validation.Validate(topicInfo, "ValTopicInfo");
								string text11 = string.Empty;
								if (!validationResults.IsValid)
								{
									foreach (ValidationResult item in (IEnumerable<ValidationResult>)validationResults)
									{
										text11 += Formatter.FormatErrorMessage(item.Message);
									}
									text3 = text11;
									text4 = "0";
								}
								else if (!VShopHelper.Createtopic(topicInfo, out num))
								{
									text3 = "保存失败";
									text4 = "0";
								}
							}
							else
							{
								TopicInfo topicInfo2 = VShopHelper.Gettopic(num);
								topicInfo2.Title = text5;
								topicInfo2.Description = text9;
								topicInfo2.SharePic = text10;
								if (!VShopHelper.Updatetopic(topicInfo2))
								{
									text3 = "保存失败";
									text4 = "0";
								}
							}
							if (text4 == "0")
							{
								context.Response.Write("{\"status\":" + text4 + ",\"msg\":\"" + text3 + "\"}");
								return;
							}
							if (text2.ToLower().Trim() == "topic")
							{
								text9 = jObject["page"]["describe"].ToString();
								empty = context.Server.MapPath("/Templates/topic/waptopic/topic_" + num + ".json");
								empty2 = "/Templates/topic/waptopic";
								empty3 = context.Server.MapPath(empty2 + "/Skin-TopicHomePage_" + num + ".html");
							}
							else if (text2.ToLower().Trim() == "pctopic")
							{
								empty = context.Server.MapPath("/Templates/topic/pctopic/pctopic_" + num + ".json");
								empty2 = "/Templates/topic/pctopic";
								empty3 = context.Server.MapPath(empty2 + "/Skin-PcTopicHomePage_" + num + ".html");
							}
							else
							{
								empty = context.Server.MapPath("/Templates/topic/apptopic/apptopic_" + num + ".json");
								empty2 = "/Templates/topic/apptopic";
								empty3 = context.Server.MapPath(empty2 + "/Skin-ApptopicHomePage_" + num + ".html");
							}
						}
						else
						{
							if (text2.ToLower().Trim() == "appshop")
							{
								string path = context.Server.MapPath("/Templates/" + text2 + "/data/default.txt");
								FileStream fileStream = new FileStream(path, FileMode.Create);
								StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8);
								try
								{
									streamWriter.Write(text);
									streamWriter.Flush();
									SiteSettings masterSettings = SettingsManager.GetMasterSettings();
									masterSettings.AppHomeTopicVersionCode += 1L;
									SettingsManager.Save(masterSettings);
								}
								catch (Exception ex2)
								{
									text3 = "保存失败";
									text4 = "0";
									Globals.WriteLog("AppHomeTopicError.txt", ex2.Message);
								}
								finally
								{
									streamWriter.Close();
									fileStream.Close();
								}
								context.Response.Write("{\"status\":" + text4 + ",\"msg\":\"" + text3 + "\"}");
								return;
							}
							if (text2.ToLower().Trim() == "xcxshop")
							{
								string path2 = context.Server.MapPath("/Templates/" + text2 + "/data/default.txt");
								FileStream fileStream2 = new FileStream(path2, FileMode.Create);
								StreamWriter streamWriter2 = new StreamWriter(fileStream2, Encoding.UTF8);
								try
								{
									streamWriter2.Write(text);
									streamWriter2.Flush();
									SiteSettings masterSettings2 = SettingsManager.GetMasterSettings();
									masterSettings2.XcxHomeVersionCode += 1L;
									SettingsManager.Save(masterSettings2);
								}
								catch (Exception ex3)
								{
									text3 = "保存失败";
									text4 = "0";
									Globals.WriteLog("AppHomeTopicError.txt", ex3.Message);
								}
								finally
								{
									streamWriter2.Close();
									fileStream2.Close();
								}
								context.Response.Write("{\"status\":" + text4 + ",\"msg\":\"" + text3 + "\"}");
								return;
							}
							string str5 = context.Request.Form["themeName"];
							empty = context.Server.MapPath("/Templates/common/home/" + str5 + "/data/default.json");
							str = "<Hi:WapAppDownloadInfo runat=\"server\" id=\"appdownloadinfo\" />";
							empty2 = "/Templates/common/home/" + str5;
							empty3 = context.Server.MapPath(empty2 + "/Skin-HomePage.html");
						}
						StreamWriter streamWriter3 = new StreamWriter(empty, false, Encoding.UTF8);
						string text12 = text;
						foreach (char value in text12)
						{
							streamWriter3.Write(value);
						}
						streamWriter3.Close();
						string text13 = "";
						if (text2.ToLower().Trim() == "pctopic")
						{
							text13 = "<title>" + text5 + "</title><Hi:Common_Header ID=\"Common_Header1\" runat=\"server\" />";
						}
						else
						{
							text13 = "<%@ Control Language=\"C#\" %><%@ Register TagPrefix=\"Hi\" Namespace=\"Hidistro.UI.Common.Controls\" Assembly=\"Hidistro.UI.Common.Controls\" %><title>" + text5 + "</title><Hi:WapHeadContainer ID=\"WapHeadContainer1\" runat=\"server\" />";
							text13 = ((!(text2.ToLower().Trim() != "apptopic")) ? (text13 + "<script type=\"text/javascript\">function searchs(v) {if(OpenUrl&&typeof(OpenUrl)==\"function\"){ OpenUrl(\"search-result\", \"{\\\"keywords\\\":\\\"\" + v + \"\\\"}\");}else{var type = GetAgentType();if (type == 2){window.HiCmd.webShowSearchProductResult(v);}else{loadIframeURL(\"hishop://webShowSearchProductResult/\" + v);}}}</script>") : (text13 + "<script type=\"text/javascript\">function searchs(v) {var url = \"ProductList?keyWord=\"+v;window.location.href = url;}</script>"));
						}
						SiteSettings masterSettings3 = SettingsManager.GetMasterSettings();
						if (text2.ToLower().Trim() == "vshop")
						{
							text13 = text13 + "<input type=\"hidden\" id=\"hdTitle\" value=\"" + text5 + "\" />";
							text13 = text13 + "<input type=\"hidden\" id=\"hdDesc\" value=\"" + jObject["page"]["describe"].ToString() + "\" />";
							string text14 = jObject["page"]["sharepic"].ToNullString();
							text14 = Globals.GetImageServerUrl("http://", string.IsNullOrEmpty(text14) ? "" : this.UploadImage(text14));
							if (string.IsNullOrEmpty(text14))
							{
								text14 = masterSettings3.LogoUrl;
							}
							text13 = text13 + "<input type=\"hidden\" id=\"hdImgUrl\" value=\"" + text14 + "\" />";
						}
						if (text2.ToLower().Trim() == "pctopic")
						{
							text13 = text13 + "<input type=\"hidden\" id=\"hidBackgroundColor\" runat=\"server\" ClientIDMode=\"Static\" value=\"" + str2 + "\" />";
							text13 = text13 + "<input type=\"hidden\" id=\"hidBackgroundImg\" runat=\"server\" ClientIDMode=\"Static\" value=\"" + text7 + "\" />";
							text13 = text13 + "<input type=\"hidden\" id=\"hidFillingMethod\" runat=\"server\" ClientIDMode=\"Static\" value=\"" + str3 + "\" />";
							text13 = text13 + "<input type=\"hidden\" id=\"hidBgAlign\" runat=\"server\" ClientIDMode=\"Static\" value=\"" + str4 + "\" />";
						}
						text13 += str;
						text13 += this.GetPModulesHtml(context, jObject);
						string lModulesHtml = this.GetLModulesHtml(context, jObject, text2.ToLower().Trim() == "apptopic");
						text13 += lModulesHtml;
						text13 = text13.Replace("src", "data-url");
						if (!Directory.Exists(context.Server.MapPath(empty2)))
						{
							Directory.CreateDirectory(context.Server.MapPath(empty2));
						}
						StreamWriter streamWriter4 = new StreamWriter(empty3, false, Encoding.UTF8);
						string text15 = text13;
						foreach (char value2 in text15)
						{
							streamWriter4.Write(value2);
						}
						streamWriter4.Close();
					}
					catch (Exception ex4)
					{
						NameValueCollection param = new NameValueCollection
						{
							context.Request.QueryString,
							context.Request.Form
						};
						Globals.WriteExceptionLog_Page(ex4, param, "SaveTemplate");
						text3 = ex4.Message;
						text4 = "0";
					}
					if (context.Request.Form["is_preview"] == "1")
					{
						if (text2.ToLower().Trim() == "topic")
						{
							context.Response.Write("{\"status\":" + text4 + ",\"topicid\":" + num + ",\"msg\":\"" + text3 + "\",\"link\":\"/wapshop/topics?topicId=" + num.ToString() + "\"}");
						}
						else if (text2.ToLower().Trim() == "apptopic")
						{
							context.Response.Write("{\"status\":" + text4 + ",\"topicid\":" + num + ",\"msg\":\"" + text3 + "\",\"link\":\"/appshop/topics?topicId=" + num.ToString() + "\"}");
						}
						else if (text2.ToLower().Trim() == "pctopic")
						{
							context.Response.Write("{\"status\":" + text4 + ",\"topicid\":" + num + ",\"msg\":\"" + text3 + "\",\"link\":\"/topics?topicId=" + num.ToString() + "\"}");
						}
						else
						{
							context.Response.Write("{\"status\":" + text4 + ",\"msg\":\"" + text3 + "\",\"link\":\"/wapshop/default.aspx\"}");
						}
					}
					else if (text2.ToLower().Trim() == "topic" || text2.ToLower().Trim() == "apptopic" || text2.ToLower().Trim() == "pctopic")
					{
						context.Response.Write("{\"status\":" + text4 + ",\"topicid\":" + num + ",\"msg\":\"" + text3 + "\"}");
					}
					else
					{
						context.Response.Write("{\"status\":" + text4 + ",\"msg\":\"" + text3 + "\"}");
					}
				}
			}
			catch (Exception ex5)
			{
				NameValueCollection param2 = new NameValueCollection
				{
					context.Request.QueryString,
					context.Request.Form
				};
				Globals.WriteExceptionLog_Page(ex5, param2, "SaveTemplate1");
			}
		}

		public string UploadImage(string imgSrc)
		{
			string str = Globals.GetStoragePath() + "/temp/";
			string text = HttpContext.Current.Server.MapPath(Globals.GetStoragePath() + "/topic/");
			if (!Globals.PathExist(text, false))
			{
				Globals.CreatePath(text);
			}
			if (imgSrc.Trim().Length == 0)
			{
				return string.Empty;
			}
			imgSrc = imgSrc.Replace("//", "/");
			string text2 = (imgSrc.Split('/').Length == 6) ? imgSrc.Split('/')[5] : imgSrc.Split('/')[4];
			if (File.Exists(text + text2))
			{
				return Globals.GetStoragePath() + "/topic/" + text2;
			}
			if (File.Exists(HttpContext.Current.Server.MapPath(imgSrc)))
			{
				File.Copy(HttpContext.Current.Server.MapPath(imgSrc), text + text2);
			}
			string path = HttpContext.Current.Server.MapPath(str + text2);
			if (File.Exists(path))
			{
				File.Delete(path);
			}
			return Globals.GetStoragePath() + "/topic/" + text2;
		}

		public string GetPModulesHtml(HttpContext context, JObject jo)
		{
			string text = "";
			foreach (JToken item in (IEnumerable<JToken>)jo["PModules"])
			{
				text += this.Base64Decode(item["dom_conitem"].ToString());
			}
			return text;
		}

		public string GetLModulesHtml(HttpContext context, JObject jo, bool IsApp = false)
		{
			string text = "";
			foreach (JToken item in (IEnumerable<JToken>)jo["LModules"])
			{
				text = ((!(item["type"].ToString() == "5")) ? ((!(item["type"].ToString() == "4")) ? (text + this.Base64Decode(item["dom_conitem"].ToString())) : (text + this.GetGoodTag(context, item, IsApp))) : (text + this.GetGoodGroupTag(context, this.Base64Decode(item["dom_conitem"].ToString()), item, IsApp)));
			}
			return text;
		}

		public string GetGoodGroupTag(HttpContext context, string path, JToken data, bool IsApp = false)
		{
			try
			{
				return "<Hi:GoodsListModule runat=\"server\" IsApp=\"" + IsApp.ToString() + "\" Type=\"goodGroup\" Layout=\"" + data["content"]["layout"] + "\" ShowName=\"" + data["content"]["showName"] + "\" ShowIco=\"" + data["content"]["showIco"] + "\" ShowPrice=\"" + data["content"]["showPrice"] + "\" DataUrl=\"" + context.Request.Form["getGoodGroupUrl"] + "\" ID=\"list_" + Guid.NewGuid().ToString("N") + "\" TemplateFile=\"" + path + "\"  GoodListSize=\"" + data["content"]["goodsize"] + "\" CategoryId=\"" + data["content"]["categoryId"] + "\" FirstPriority=\"" + data["content"]["firstPriority"] + "\"  SecondPriority=\"" + data["content"]["secondPriority"] + "\"  ThirdPriority=\"" + data["content"]["thirdPriority"] + "\"   />";
			}
			catch
			{
				return "";
			}
		}

		public string GetGoodTag(HttpContext context, JToken data, bool IsApp = false)
		{
			try
			{
				string text = "";
				foreach (JToken item in (IEnumerable<JToken>)data["content"]["goodslist"])
				{
					text = text + item["item_id"] + ",";
				}
				text = text.TrimEnd(',');
				string str = "";
				if (!string.IsNullOrEmpty(text))
				{
					string text2 = "/admin/shop/Modules/GoodGroup" + data["content"]["layout"] + ".cshtml";
					return "<Hi:GoodsMobule runat=\"server\" IsApp=\"" + IsApp.ToString() + "\" Layout=\"" + data["content"]["layout"] + "\" ShowName=\"" + data["content"]["showName"] + "\" IDs=\"" + text + "\" ShowIco=\"" + data["content"]["showIco"] + "\" ShowPrice=\"" + data["content"]["showPrice"] + "\" DataUrl=\"" + context.Request.Form["getGoodUrl"] + "\" ID=\"goods_" + Guid.NewGuid().ToString("N") + "\" TemplateFile=\"" + text2 + "\"    />";
				}
				return str + this.Base64Decode(data["dom_conitem"].ToString());
			}
			catch
			{
				return "";
			}
		}

		public string Base64Code(string message)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(message);
			return Convert.ToBase64String(bytes);
		}

		public string Base64Decode(string message)
		{
			byte[] bytes = Convert.FromBase64String(message);
			return Encoding.UTF8.GetString(bytes);
		}
	}
}
