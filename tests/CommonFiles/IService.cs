using System.Threading.Tasks;

namespace Ease.TestCommon
{
	public interface IService
	{
		Task LongRunningTask();
	}
}
