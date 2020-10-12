using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Threading.Tasks;

namespace Ease.TestCommon
{
	public class VM : BindableBase
	{
		private IRepo Repo { get; }
		private IService Service { get; }
		private INavigationService NavigationService { get; }

		public string MyStringProperty { get; set; }

		public string MyRepoProperty { get => Repo.MyProperty; }

		public VM(INavigationService navigationService, IRepo repo, IService service)
		{
			NavigationService = navigationService;
			Repo = repo;
			Service = service;
		}

		public void DoSaveData()
		{
			Repo.SaveData();
		}

		public Task DoLongRunningTask()
        {
			return Service.LongRunningTask();
        }

		public Task DoNavigationAsync(string target)
		{
			return NavigationService.NavigateAsync(target);
		}

		public Task DoNavigationAsync(string target, INavigationParameters parameters)
		{
			return NavigationService.NavigateAsync(target, parameters);
		}

		public Task DoNavigationAsync(string target, INavigationParameters parameters, bool useModalNavigation, bool animated)
		{
			return NavigationService.NavigateAsync(target, parameters, useModalNavigation, animated);
		}

		public Task DoNavigationAsync(Uri target)
		{
			return NavigationService.NavigateAsync(target);
		}

		public Task DoNavigationAsync(Uri target, INavigationParameters parameters)
		{
			return NavigationService.NavigateAsync(target, parameters);
		}

		public Task DoNavigationAsync(Uri target, INavigationParameters parameters, bool useModalNavigation, bool animated)
		{
			return NavigationService.NavigateAsync(target, parameters, useModalNavigation, animated);
		}

		public Task DoGoBackAsync()
		{
			return NavigationService.GoBackAsync();
		}

		public Task DoGoBackAsync(INavigationParameters parameters)
		{
			return NavigationService.GoBackAsync(parameters);
		}

		public Task DoGoBackAsync(INavigationParameters parameters, bool useModalNavigation, bool animated)
		{
			return NavigationService.GoBackAsync(parameters, useModalNavigation, animated);
		}
	}
}
