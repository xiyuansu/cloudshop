using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

public class ListFileManager : Handler
{
	private enum ResultState
	{
		Success,
		InvalidParam,
		AuthorizError,
		IOError,
		PathNotFound
	}

	private int Start;

	private int Size;

	private int Total;

	private ResultState State;

	private string PathToList;

	private string[] FileList;

	private string[] SearchExtensions;

	public ListFileManager(HttpContext context, string pathToList, string[] searchExtensions)
		: base(context)
	{
		this.SearchExtensions = (from x in searchExtensions
		select x.ToLower()).ToArray();
		this.PathToList = pathToList;
	}

	public override void Process()
	{
		try
		{
			this.Start = ((!string.IsNullOrEmpty(base.Request["start"])) ? Convert.ToInt32(base.Request["start"]) : 0);
			this.Size = (string.IsNullOrEmpty(base.Request["size"]) ? Config.GetInt("imageManagerListSize") : Convert.ToInt32(base.Request["size"]));
		}
		catch (FormatException)
		{
			this.State = ResultState.InvalidParam;
			this.WriteResult();
			return;
		}
		List<string> list = new List<string>();
		try
		{
			string localPath = base.Server.MapPath(this.PathToList);
			list.AddRange(from x in Directory.GetFiles(localPath, "*", SearchOption.AllDirectories)
			where this.SearchExtensions.Contains(Path.GetExtension(x).ToLower())
			select this.PathToList + x.Substring(localPath.Length).Replace("\\", "/"));
			this.Total = list.Count;
			this.FileList = (from x in list
			orderby x
			select x).Skip(this.Start).Take(this.Size).ToArray();
		}
		catch (UnauthorizedAccessException)
		{
			this.State = ResultState.AuthorizError;
		}
		catch (DirectoryNotFoundException)
		{
			this.State = ResultState.PathNotFound;
		}
		catch (IOException)
		{
			this.State = ResultState.IOError;
		}
		finally
		{
			this.WriteResult();
		}
	}

	private void WriteResult()
	{
		base.WriteJson(new
		{
			state = this.GetStateString(),
			list = ((this.FileList == null) ? null : (from x in this.FileList
			select new
			{
				url = x
			})),
			start = this.Start,
			size = this.Size,
			total = this.Total
		});
	}

	private string GetStateString()
	{
		switch (this.State)
		{
		case ResultState.Success:
			return "SUCCESS";
		case ResultState.InvalidParam:
			return "参数不正确";
		case ResultState.PathNotFound:
			return "路径不存在";
		case ResultState.AuthorizError:
			return "文件系统权限不足";
		case ResultState.IOError:
			return "文件系统读取错误";
		default:
			return "未知错误";
		}
	}
}
