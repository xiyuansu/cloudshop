using System.Xml;

namespace Hidistro.Core.Jobs
{
	public interface IJob
	{
		void Execute(XmlNode node);
	}
}
