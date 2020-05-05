using Prism.Mvvm;
using Prism.Navigation;
using System.Threading.Tasks;

namespace Ease.TestCommon
{
	public class VM : BindableBase
	{
		private IRepo Repo { get; }
		private INavigationService NavigationService { get; }

		public string MyStringProperty { get; set; }

		public string MyRepoProperty { get => Repo.MyProperty; }

		public VM(INavigationService navigationService, IRepo repo)
		{
			NavigationService = navigationService;
			Repo = repo;
		}

		public void DoSaveData()
		{
			Repo.SaveData();
		}

		public Task DoNavigationAsync(string target)
		{
			return NavigationService.NavigateAsync(target);
		}

		public Task DoNavigationWithParametersAsync(string target)
		{
			return NavigationService.NavigateAsync(target, new NavigationParameters("x=1"));
		}

		public Task GoBackAsync()
		{
			return NavigationService.GoBackAsync();
		}

		public Task GoBackWithParametersAsync()
		{
			return NavigationService.GoBackAsync(new NavigationParameters("x=1"));
		}
	}
}
