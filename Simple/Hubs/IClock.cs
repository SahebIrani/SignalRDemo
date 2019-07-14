using System;
using System.Threading.Tasks;

namespace Simple.Hubs
{
	public interface IClock
	{
		Task ShowTime(DateTime currentTime);
	}
}
