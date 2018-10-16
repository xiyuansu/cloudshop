using System;
using System.IO;
using System.Net;
using System.Web;

public class Crawler
{
	public string SourceUrl
	{
		get;
		set;
	}

	public string ServerUrl
	{
		get;
		set;
	}

	public string State
	{
		get;
		set;
	}

	private HttpServerUtility Server
	{
		get;
		set;
	}

	public Crawler(string sourceUrl, HttpServerUtility server)
	{
		this.SourceUrl = sourceUrl;
		this.Server = server;
	}

	public Crawler Fetch()
	{
		HttpWebRequest httpWebRequest = WebRequest.Create(this.SourceUrl) as HttpWebRequest;
		using (HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse)
		{
			if (httpWebResponse.StatusCode != HttpStatusCode.OK)
			{
				this.State = "Url returns " + httpWebResponse.StatusCode + ", " + httpWebResponse.StatusDescription;
				return this;
			}
			if (httpWebResponse.ContentType.IndexOf("image") == -1)
			{
				this.State = "Url is not an image";
				return this;
			}
			this.ServerUrl = PathFormatter.Format(Path.GetFileName(this.SourceUrl), Config.GetString("catcherPathFormat"));
			string path = this.Server.MapPath(this.ServerUrl);
			if (!Directory.Exists(Path.GetDirectoryName(path)))
			{
				Directory.CreateDirectory(Path.GetDirectoryName(path));
			}
			try
			{
				Stream responseStream = httpWebResponse.GetResponseStream();
				BinaryReader binaryReader = new BinaryReader(responseStream);
				byte[] bytes = default(byte[]);
				using (MemoryStream memoryStream = new MemoryStream())
				{
					byte[] array = new byte[4096];
					int count;
					while ((count = binaryReader.Read(array, 0, array.Length)) != 0)
					{
						memoryStream.Write(array, 0, count);
					}
					bytes = memoryStream.ToArray();
				}
				File.WriteAllBytes(path, bytes);
				this.State = "SUCCESS";
			}
			catch (Exception ex)
			{
				this.State = "抓取错误：" + ex.Message;
			}
			return this;
		}
	}
}
