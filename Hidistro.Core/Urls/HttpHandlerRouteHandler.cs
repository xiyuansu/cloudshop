using System.Web;
using System.Web.Routing;

namespace Hidistro.Core.Urls
{
	public class HttpHandlerRouteHandler : IRouteHandler
	{
		private IHttpHandler _handler;

		public HttpHandlerRouteHandler(IHttpHandler handler)
		{
			this._handler = handler;
		}

		public IHttpHandler GetHttpHandler(RequestContext requestContext)
		{
			return this._handler;
		}
	}
}
