using System.Threading.Tasks;

namespace Simple.Hubs
{
	public interface IChatClient
	{
		Task ReceiveMessage(string user, string message);
		Task ReceiveMessage(string message);
	}
}
