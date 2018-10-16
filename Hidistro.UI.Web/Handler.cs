using Newtonsoft.Json;
using System.Web;

public abstract class Handler
{
	public HttpRequest Request
	{
		get;
		private set;
	}

	public HttpResponse Response
	{
		get;
		private set;
	}

	public HttpContext Context
	{
		get;
		private set;
	}

	public HttpServerUtility Server
	{
		get;
		private set;
	}

	public Handler(HttpContext context)
	{
		this.Request = context.Request;
		this.Response = context.Response;
		this.Context = context;
		this.Server = context.Server;
	}

	public abstract void Process();

	protected void WriteJson(object response)
	{
		string text = this.Request["callback"];
		string text2 = JsonConvert.SerializeObject(response);
		if (string.IsNullOrWhiteSpace(text))
		{
			this.Response.AddHeader("Content-Type", "text/plain");
			this.Response.Write(text2);
		}
		else
		{
			this.Response.AddHeader("Content-Type", "application/javascript");
			this.Response.Write($"{text}({text2});");
		}
		this.Response.End();
	}
}
