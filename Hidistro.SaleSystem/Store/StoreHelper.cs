using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Store;
using Hidistro.Entities.VShop;
using Hidistro.SqlDal;
using Hidistro.SqlDal.Store;
using Hidistro.SqlDal.VShop;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Web;
using System.Xml;

namespace Hidistro.SaleSystem.Store
{
	public static class StoreHelper
	{
		public static IList<FriendlyLinksInfo> GetFriendlyLinks()
		{
			return new FriendlyLinkDao().Gets<FriendlyLinksInfo>("DisplaySequence", SortAction.Desc, null);
		}

		public static FriendlyLinksInfo GetFriendlyLink(int linkId)
		{
			return new FriendlyLinkDao().Get<FriendlyLinksInfo>(linkId);
		}

		public static bool UpdateFriendlyLink(FriendlyLinksInfo friendlyLink)
		{
			if (friendlyLink == null)
			{
				return false;
			}
			HiCache.Remove("DataCache-FriendLinks");
			return new FriendlyLinkDao().Update(friendlyLink, null);
		}

		public static bool FriendlyLinkDelete(int linkId)
		{
			HiCache.Remove("DataCache-FriendLinks");
			return new FriendlyLinkDao().Delete<FriendlyLinksInfo>(linkId);
		}

		public static bool CreateFriendlyLink(FriendlyLinksInfo friendlyLink)
		{
			if (friendlyLink == null)
			{
				return false;
			}
			HiCache.Remove("DataCache-FriendLinks");
			FriendlyLinkDao friendlyLinkDao = new FriendlyLinkDao();
			friendlyLink.DisplaySequence = friendlyLinkDao.GetMaxDisplaySequence<FriendlyLinksInfo>();
			return new FriendlyLinkDao().Add(friendlyLink, null) > 0;
		}

		public static bool SwapFriendlyLinkSequence(int linkId, int displaySequence)
		{
			HiCache.Remove("DataCache-FriendLinks");
			return new FriendlyLinkDao().SaveSequence<FriendlyLinksInfo>(linkId, displaySequence, null);
		}

		public static bool SwapHotkeywordSequence(int hid, int displaySequence)
		{
			return new HotkeywordDao().SaveSequence<HotkeywordInfo>(hid, displaySequence, null);
		}

		public static void DeleteHotKeywords(int hid)
		{
			HiCache.Remove("DataCache-Keywords");
			new HotkeywordDao().Delete<HotkeywordInfo>(hid);
		}

		public static void UpdateHotWords(int hid, int categoryId, string hotKeyWords)
		{
			HiCache.Remove("DataCache-Keywords");
			new HotkeywordDao().UpdateHotWords(hid, categoryId, hotKeyWords);
		}

		public static void AddHotkeywords(int categoryId, string keywords)
		{
			HiCache.Remove("DataCache-Keywords");
			new HotkeywordDao().AddHotkeywords(categoryId, keywords);
		}

		public static string GetHotkeyword(int id)
		{
			HotkeywordInfo hotkeywordInfo = new HotkeywordDao().Get<HotkeywordInfo>(id);
			if (hotkeywordInfo != null)
			{
				return hotkeywordInfo.Keywords;
			}
			return "";
		}

		public static string UploadVoteImage(HttpPostedFile postedFile)
		{
			if (!ResourcesHelper.CheckPostedFile(postedFile, "image", null))
			{
				return string.Empty;
			}
			string text = Globals.GetStoragePath() + "/vote/" + ResourcesHelper.GenerateFilename(Path.GetExtension(postedFile.FileName));
			postedFile.SaveAs(HttpContext.Current.Request.MapPath(text));
			return text;
		}

		public static DataTable GetHotKeywords()
		{
			return new HotkeywordDao().GetHotKeywords();
		}

		public static DataSet GetVotes()
		{
			return new VoteDao().GetVotes();
		}

		public static int CreateVote(VoteInfo vote)
		{
			int num = 0;
			VoteDao voteDao = new VoteDao();
			long num3 = vote.VoteId = voteDao.Add(vote, null);
			if (num3 > 0)
			{
				num = 1;
				if (vote.VoteItems != null)
				{
					foreach (VoteItemInfo voteItem in vote.VoteItems)
					{
						voteItem.VoteId = num3;
						voteItem.ItemCount = 0;
						num += (int)voteDao.Add(voteItem, null);
					}
				}
			}
			return num;
		}

		public static DbQueryResult Query(VoteSearch query)
		{
			return new VoteDao().Query(query);
		}

		public static bool UpdateVote(VoteInfo vote)
		{
			Database database = DatabaseFactory.CreateDatabase();
			using (DbConnection dbConnection = database.CreateConnection())
			{
				VoteDao voteDao = new VoteDao();
				dbConnection.Open();
				DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					if (!voteDao.Update(vote, null))
					{
						dbTransaction.Rollback();
						return false;
					}
					if (!voteDao.DeleteVoteItem(vote.VoteId, dbTransaction))
					{
						dbTransaction.Rollback();
						return false;
					}
					int num = 0;
					if (vote.VoteItems != null)
					{
						foreach (VoteItemInfo voteItem in vote.VoteItems)
						{
							voteItem.VoteId = vote.VoteId;
							voteItem.ItemCount = 0;
							num += (int)voteDao.Add(voteItem, null);
						}
						if (num < vote.VoteItems.Count)
						{
							dbTransaction.Rollback();
							return false;
						}
					}
					dbTransaction.Commit();
					return true;
				}
				catch (Exception)
				{
					dbTransaction.Rollback();
					return false;
				}
				finally
				{
					dbConnection.Close();
				}
			}
		}

		public static bool DeleteVote(long voteId)
		{
			return new VoteDao().Delete<VoteInfo>(voteId);
		}

		public static VoteInfo GetVoteById(long voteId)
		{
			return new VoteDao().GetVoteById(voteId);
		}

		public static IList<VoteItemInfo> GetVoteItems(long voteId)
		{
			return new VoteDao().GetVoteItems(voteId);
		}

		public static int GetVoteCounts(long voteId)
		{
			return new VoteDao().GetVoteCounts(voteId);
		}

		public static string BackupData()
		{
			return new BackupRestoreDao().BackupData(HttpContext.Current.Request.MapPath("/Storage/data/Backup/"));
		}

		public static bool InserBackInfo(string fileName, string version, long fileSize)
		{
			string filename = HiContext.Current.Context.Request.MapPath("/config/BackupFiles.config");
			try
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(filename);
				XmlNode xmlNode = xmlDocument.SelectSingleNode("root");
				XmlElement xmlElement = xmlDocument.CreateElement("backupfile");
				xmlElement.SetAttribute("BackupName", fileName);
				xmlElement.SetAttribute("Version", version.ToString());
				xmlElement.SetAttribute("FileSize", fileSize.ToString());
				xmlElement.SetAttribute("BackupTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
				xmlNode.AppendChild(xmlElement);
				xmlDocument.Save(filename);
				return true;
			}
			catch
			{
				return false;
			}
		}

		public static DataTable GetBackupFiles()
		{
			DataTable dataTable = new DataTable();
			dataTable.Columns.Add("BackupName", typeof(string));
			dataTable.Columns.Add("Version", typeof(string));
			dataTable.Columns.Add("FileSize", typeof(string));
			dataTable.Columns.Add("BackupTime", typeof(string));
			string filename = HiContext.Current.Context.Request.MapPath("/config/BackupFiles.config");
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(filename);
			XmlNodeList childNodes = xmlDocument.SelectSingleNode("root").ChildNodes;
			foreach (XmlNode item in childNodes)
			{
				XmlElement xmlElement = (XmlElement)item;
				DataRow dataRow = dataTable.NewRow();
				dataRow["BackupName"] = xmlElement.GetAttribute("BackupName");
				dataRow["Version"] = xmlElement.GetAttribute("Version");
				dataRow["FileSize"] = xmlElement.GetAttribute("FileSize");
				dataRow["BackupTime"] = xmlElement.GetAttribute("BackupTime");
				dataTable.Rows.Add(dataRow);
			}
			return dataTable;
		}

		public static bool DeleteBackupFile(string backupName)
		{
			string filename = HiContext.Current.Context.Request.MapPath("/config/BackupFiles.config");
			try
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(filename);
				XmlNodeList childNodes = xmlDocument.SelectSingleNode("root").ChildNodes;
				foreach (XmlNode item in childNodes)
				{
					XmlElement xmlElement = (XmlElement)item;
					if (xmlElement.GetAttribute("BackupName") == backupName)
					{
						xmlDocument.SelectSingleNode("root").RemoveChild(item);
					}
				}
				xmlDocument.Save(filename);
				return true;
			}
			catch
			{
				return false;
			}
		}

		public static bool RestoreData(string bakFullName)
		{
			bool result = new BackupRestoreDao().RestoreData(bakFullName);
			new BackupRestoreDao().Restor();
			return result;
		}

		public static IList<MenuInfo> GetMenus(ClientType clientType)
		{
			IList<MenuInfo> list = new List<MenuInfo>();
			MenuDao menuDao = new MenuDao();
			IList<MenuInfo> topMenus = menuDao.GetTopMenus(clientType);
			if (topMenus == null)
			{
				return list;
			}
			foreach (MenuInfo item in topMenus)
			{
				list.Add(item);
				IList<MenuInfo> menusByParentId = menuDao.GetMenusByParentId(item.MenuId, clientType);
				if (menusByParentId != null)
				{
					foreach (MenuInfo item2 in menusByParentId)
					{
						list.Add(item2);
					}
				}
			}
			return list;
		}

		public static IList<MenuInfo> GetMenusByParentId(int parentId, ClientType clientType)
		{
			return new MenuDao().GetMenusByParentId(parentId, clientType);
		}

		public static MenuInfo GetMenu(int menuId)
		{
			return new MenuDao().Get<MenuInfo>(menuId);
		}

		public static IList<MenuInfo> GetTopMenus(ClientType clientType)
		{
			return new MenuDao().GetTopMenus(clientType);
		}

		public static bool CanAddMenu(int parentId, ClientType clientType)
		{
			IList<MenuInfo> menusByParentId = new MenuDao().GetMenusByParentId(parentId, clientType);
			if (menusByParentId == null || menusByParentId.Count == 0)
			{
				return true;
			}
			if (parentId == 0)
			{
				return menusByParentId.Count < 3;
			}
			return menusByParentId.Count < 5;
		}

		public static bool UpdateMenu(MenuInfo menu)
		{
			return new MenuDao().Update(menu, null);
		}

		public static bool SaveMenu(MenuInfo menu)
		{
			return new MenuDao().Add(menu, null) > 0;
		}

		public static bool DeleteMenu(int menuId)
		{
			return new MenuDao().Delete<MenuInfo>(menuId);
		}

		public static IList<MenuInfo> GetInitMenus(ClientType clientType)
		{
			MenuDao menuDao = new MenuDao();
			IList<MenuInfo> topMenus = menuDao.GetTopMenus(clientType);
			foreach (MenuInfo item in topMenus)
			{
				item.Chilren = menuDao.GetMenusByParentId(item.MenuId, clientType);
				if (item.Chilren == null)
				{
					item.Chilren = new List<MenuInfo>();
				}
			}
			return topMenus;
		}

		public static string UploadLinkImage(HttpPostedFile postedFile)
		{
			if (!ResourcesHelper.CheckPostedFile(postedFile, "image", null))
			{
				return string.Empty;
			}
			string text = HiContext.Current.GetStoragePath() + "/link/" + ResourcesHelper.GenerateFilename(Path.GetExtension(postedFile.FileName));
			postedFile.SaveAs(HiContext.Current.Context.Request.MapPath(text));
			return text;
		}

		public static string UploadLogo(HttpPostedFile postedFile)
		{
			if (!ResourcesHelper.CheckPostedFile(postedFile, "image", null))
			{
				return string.Empty;
			}
			string text = HiContext.Current.GetStoragePath() + ResourcesHelper.GenerateFilename(Path.GetExtension(postedFile.FileName));
			postedFile.SaveAs(HiContext.Current.Context.Request.MapPath(text));
			return text;
		}

		public static string UploadAppStartIMG(HttpPostedFile postedFile)
		{
			if (!ResourcesHelper.CheckPostedFile(postedFile, "image", null))
			{
				return string.Empty;
			}
			string text = HiContext.Current.GetStoragePath() + "App_" + ResourcesHelper.GenerateFilename(Path.GetExtension(postedFile.FileName));
			postedFile.SaveAs(HiContext.Current.Context.Request.MapPath(text));
			return text;
		}

		public static void DeleteImage(string imageUrl)
		{
			if (!string.IsNullOrEmpty(imageUrl))
			{
				try
				{
					string path = HiContext.Current.Context.Request.MapPath(imageUrl);
					if (File.Exists(path))
					{
						File.Delete(path);
					}
				}
				catch
				{
				}
			}
		}
	}
}
