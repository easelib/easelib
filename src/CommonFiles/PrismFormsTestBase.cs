using Moq;
using Prism.AppModel;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

#if (IS_DRYIOC && IS_MSTEST)
namespace Ease.MsTest.DryIoc.PrismForms
#elif (IS_DRYIOC && IS_NUNIT)
namespace Ease.NUnit.DryIoc.PrismForms
#elif (IS_DRYIOC && IS_XUNIT)
namespace Ease.XUnit.DryIoc.PrismForms
#elif (IS_UNITY && IS_MSTEST)
namespace Ease.MsTest.Unity.PrismForms
#elif (IS_UNITY && IS_NUNIT)
namespace Ease.NUnit.Unity.PrismForms
#elif (IS_UNITY && IS_XUNIT)
namespace Ease.XUnit.Unity.PrismForms
#endif
{
	public class PrismFormsTestBase
#if (IS_DRYIOC && IS_MSTEST)
	: MsTestDryIocContainerTestBase
#elif (IS_DRYIOC && IS_NUNIT)
	: NUnitDryIocContainerTestBase
#elif (IS_DRYIOC && IS_XUNIT)
	: XUnitDryIocContainerTestBase
#elif (IS_UNITY && IS_MSTEST)
	: MsTestUnityContainerTestBase
#elif (IS_UNITY && IS_NUNIT)
	: NUnitUnityContainerTestBase
#elif (IS_UNITY && IS_XUNIT)
	: XUnitUnityContainerTestBase
#endif
	{
		protected Action<Mock<INavigationService>> OnINavigationServiceMockCreated;

		protected Action<Mock<IPageDialogService>> OnIPageDialogServiceMockCreated;

		protected Action<Mock<IEventAggregator>> OnIEventAggregatorMockCreated;

		private bool baseRegisterTypesCalled;

		public PrismFormsTestBase()
		{
			if ( !baseRegisterTypesCalled )
				throw new InvalidOperationException("Inherited classes must call base.RegisterTypes() when overriding");
		}

		/// <summary>
		/// <para>Regsiter any types that are required by the test.</para>
		/// <para>You should always call base when overriding as it registers Prism.INavigationService and Prism.IPlatformNavigationService.</para>
		/// </summary>
		protected override void RegisterTypes()
		{
			OnINavigationServiceMockCreated += (mock) =>
			{
				// Get the INavigationService mock as IPlatformNavigationService so that we can verify extension methods
				// This won't need to be done if IPlatformNavigationService is removed in Prism 8.0 as suggested:
				// https://github.com/PrismLibrary/Prism/issues/1990
				mock.As<IPlatformNavigationService>();
			};

			RegisterMockType(() => OnINavigationServiceMockCreated);
			RegisterMockType(() => OnIPageDialogServiceMockCreated);
			RegisterMockType(() => OnIEventAggregatorMockCreated);

			baseRegisterTypesCalled = true;
		}

		/// <summary>
		/// Add a callback that will be invoked whenever NavigateAsync is called
		/// </summary>
		/// <param name="callback">The callback to be invoked</param>
		protected void AddNavigationCallback(Action<Uri, INavigationParameters, bool?, bool> callback)
		{
			var mockPlatformNavigation = GetMock<INavigationService>()
				.As<IPlatformNavigationService>();

			mockPlatformNavigation.Setup(s => s.NavigateAsync(It.IsAny<Uri>(), It.IsAny<NavigationParameters>(), It.IsAny<bool?>(), It.IsAny<bool>()))
				.Callback(callback);
		}

		/// <summary>
		/// Add a callback that will be invoked whenever NavigateAsync is called
		/// </summary>
		/// <param name="callback">The callback to be invoked</param>
		protected void AddNavigationCallback(Action<string, INavigationParameters, bool?, bool> callback)
		{
			var mockPlatformNavigation = GetMock<INavigationService>()
				.As<IPlatformNavigationService>();

			mockPlatformNavigation.Setup(s => s.NavigateAsync(It.IsAny<string>(), It.IsAny<NavigationParameters>(), It.IsAny<bool?>(), It.IsAny<bool>()))
				.Callback(callback);
		}

		#region INavigationService Validation

		/// <summary>
		/// Verify that INavigationService.NavigateAsync was called
		/// </summary>
		/// <param name="uri">The Uri that was expected to be navigated to</param>
		/// <param name="times">The Moq.Times object that represents the expected number of times INavigationService.NavigateAsync should have been called</param>
		protected void VerifyPrismNavigateAsync(Uri uri, Func<Times> times)
		{
			var navServiceMock = GetMock<INavigationService>();
			navServiceMock.Verify(n => n.NavigateAsync(uri), times);
		}

		/// <summary>
		/// Verify that INavigationService.NavigateAsync was called
		/// </summary>
		/// <param name="uri">The Uri that was expected to be navigated to</param>
		/// <param name="parameters">The expected navigation parameters</param>
		/// <param name="times">The Moq.Times object that represents the expected number of times INavigationService.NavigateAsync should have been called</param>
		protected void VerifyPrismNavigateAsync(Uri uri, INavigationParameters parameters, Func<Times> times)
		{
			var navServiceMock = GetMock<INavigationService>();
			navServiceMock.Verify(n => n.NavigateAsync(uri, parameters), times);
		}

		/// <summary>
		/// Verify that INavigationService.NavigateAsync was called
		/// </summary>
		/// <param name="uri">The Uri that was expected to be navigated to</param>
		/// <param name="parameterValidation">Predicate that is passed to Moq.It.Is to validate the expected navigation parameters</param>
		/// <param name="times">The Moq.Times object that represents the expected number of times INavigationService.NavigateAsync should have been called</param>
		protected void VerifyPrismNavigateAsync(Uri uri, Expression<Func<INavigationParameters, bool>> parameterValidation, Func<Times> times)
		{
			var navServiceMock = GetMock<INavigationService>();
			navServiceMock.Verify(n => n.NavigateAsync(uri, It.Is(parameterValidation)), times);
		}

		/// <summary>
		/// Verify that INavigationService.NavigateAsync was called
		/// </summary>
		/// <param name="path">The path that was expected to be navigated to</param>
		/// <param name="times">The Moq.Times object that represents the expected number of times INavigationService.NavigateAsync should have been called</param>
		protected void VerifyPrismNavigateAsync(string path, Func<Times> times)
		{
			var navServiceMock = GetMock<INavigationService>();
			navServiceMock.Verify(n => n.NavigateAsync(path), times);
		}

		/// <summary>
		/// Verify that INavigationService.NavigateAsync was called
		/// </summary>
		/// <param name="path">The path that was expected to be navigated to</param>
		/// <param name="parameters">The expected navigation parameters</param>
		/// <param name="times">The Moq.Times object that represents the expected number of times INavigationService.NavigateAsync should have been called</param>
		protected void VerifyPrismNavigateAsync(string path, INavigationParameters parameters, Func<Times> times)
		{
			var navServiceMock = GetMock<INavigationService>();
			if (parameters != null)
			{
				navServiceMock.Verify(n => n.NavigateAsync(path, parameters), times);
			}
			else
			{
				navServiceMock.Verify(n => n.NavigateAsync(path, It.IsAny<INavigationParameters>()), times);
			}
		}

		/// <summary>
		/// Verify that INavigationService.NavigateAsync was called
		/// </summary>
		/// <param name="path">The path that was expected to be navigated to</param>
		/// <param name="parameterValidation">Predicate that is passed to Moq.It.Is to validate the expected navigation parameters</param>
		/// <param name="times">The Moq.Times object that represents the expected number of times INavigationService.NavigateAsync should have been called</param>
		protected void VerifyPrismNavigateAsync(string path, Expression<Func<INavigationParameters, bool>> parameterValidation, Func<Times> times)
		{
			var navServiceMock = GetMock<INavigationService>();
			navServiceMock.Verify(n => n.NavigateAsync(path, It.Is(parameterValidation)), times);
		}

		/// <summary>
		/// Verify that INavigationService.GoBackAsync was called
		/// </summary>
		/// <param name="times">The Moq.Times object that represents the expected number of times INavigationService.GoBackAsync should have been called</param>
		protected void VerifyPrismGoBackAsync(Func<Times> times)
		{
			var navServiceMock = GetMock<INavigationService>();
			navServiceMock.Verify(n => n.GoBackAsync(), times);
		}

		/// <summary>
		/// Verify that INavigationService.GoBackAsync was called
		/// </summary>
		/// <param name="parameters">The expected navigation parameters</param>
		/// <param name="times">The Moq.Times object that represents the expected number of times INavigationService.GoBackAsync should have been called</param>
		protected void VerifyPrismGoBackAsync(INavigationParameters parameters, Func<Times> times)
		{
			var navServiceMock = GetMock<INavigationService>();
			navServiceMock.Verify(n => n.GoBackAsync(parameters), times);
		}

		/// <summary>
		/// Verify that INavigationService.GoBackAsync was called
		/// </summary>
		/// <param name="parameterValidation">Predicate that is passed to Moq.It.Is to validate the expected navigation parameters</param>
		/// <param name="times">The Moq.Times object that represents the expected number of times INavigationService.GoBackAsync should have been called</param>
		protected void VerifyPrismGoBackAsync(Expression<Func<INavigationParameters, bool>> parameterValidation, Func<Times> times)
		{
			var navServiceMock = GetMock<INavigationService>();
			navServiceMock.Verify(n => n.GoBackAsync(It.Is(parameterValidation)), times);
		}

		#endregion INavigationService Validation

		#region IPlatformNavigationService Validation

		/// <summary>
		/// Verify that IPlatformNavigationService.NavigateAsync extension method was called
		/// </summary>
		/// <param name="uri">The Uri that was expected to be navigated to</param>
		/// <param name="parameters">The expected navigation parameters</param>
		/// <param name="useModalNavigation">Whether or not the expected navigation should have been modal</param>
		/// <param name="animated">Whether or not the expected navigation should have been animated</param>
		/// <param name="times">The Moq.Times object that represents the expected number of times IPlatformNavigationService.NavigateAsync should have been called</param>
		protected void VerifyPrismNavigateAsync(Uri uri, INavigationParameters parameters, bool useModalNavigation, bool animated, Func<Times> times)
		{
			var mockPlatformNavigation = GetMock<INavigationService>()
				.As<IPlatformNavigationService>();

			mockPlatformNavigation.Verify(x => x.NavigateAsync(uri, parameters, useModalNavigation, animated), times);
		}

		/// <summary>
		/// Verify that IPlatformNavigationService.NavigateAsync extension method was called
		/// </summary>
		/// <param name="uri">The Uri that was expected to be navigated to</param>
		/// <param name="parameterValidation">Predicate that is passed to Moq.It.Is to validate the expected navigation parameters</param>
		/// <param name="useModalNavigation">Whether or not the expected navigation should have been modal</param>
		/// <param name="animated">Whether or not the expected navigation should have been animated</param>
		/// <param name="times">The Moq.Times object that represents the expected number of times IPlatformNavigationService.NavigateAsync should have been called</param>
		protected void VerifyPrismNavigateAsync(Uri uri, Expression<Func<INavigationParameters, bool>> parameterValidation, bool useModalNavigation, bool animated, Func<Times> times)
		{
			var mockPlatformNavigation = GetMock<INavigationService>()
				.As<IPlatformNavigationService>();

			mockPlatformNavigation.Verify(x => x.NavigateAsync(uri, It.Is(parameterValidation), useModalNavigation, animated), times);
		}

		/// <summary>
		/// Verify that IPlatformNavigationService.NavigateAsync extension method was called
		/// </summary>
		/// <param name="path">The path that was expected to be navigated to</param>
		/// <param name="parameters">The expected navigation parameters</param>
		/// <param name="useModalNavigation">Whether or not the expected navigation should have been modal</param>
		/// <param name="animated">Whether or not the expected navigation should have been animated</param>
		/// <param name="times">The Moq.Times object that represents the expected number of times IPlatformNavigationService.NavigateAsync should have been called</param>
		protected void VerifyPrismNavigateAsync(string path, INavigationParameters parameters, bool useModalNavigation, bool animated, Func<Times> times)
		{
			var mockPlatformNavigation = GetMock<INavigationService>()
				.As<IPlatformNavigationService>();

			mockPlatformNavigation.Verify(x => x.NavigateAsync(path, parameters, useModalNavigation, animated), times);
		}

		/// <summary>
		/// Verify that IPlatformNavigationService.NavigateAsync extension method was called
		/// </summary>
		/// <param name="path">The path that was expected to be navigated to</param>
		/// <param name="parameterValidation">Predicate that is passed to Moq.It.Is to validate the expected navigation parameters</param>
		/// <param name="useModalNavigation">Whether or not the expected navigation should have been modal</param>
		/// <param name="animated">Whether or not the expected navigation should have been animated</param>
		/// <param name="times">The Moq.Times object that represents the expected number of times IPlatformNavigationService.NavigateAsync should have been called</param>
		protected void VerifyPrismNavigateAsync(string path, Expression<Func<INavigationParameters, bool>> parameterValidation, bool useModalNavigation, bool animated, Func<Times> times)
		{
			var mockPlatformNavigation = GetMock<INavigationService>()
				.As<IPlatformNavigationService>();

			mockPlatformNavigation.Verify(x => x.NavigateAsync(path, It.Is(parameterValidation), useModalNavigation, animated), times);
		}

		/// <summary>
		/// Verify that IPlatformNavigationService.GoBackAsync extension method was called
		/// </summary>
		/// <param name="parameters">The expected navigation parameters</param>
		/// <param name="useModalNavigation">Whether or not the expected navigation should have been modal</param>
		/// <param name="animated">Whether or not the expected navigation should have been animated</param>
		/// <param name="times">The Moq.Times object that represents the expected number of times IPlatformNavigationService.GoBackAsync should have been called</param>
		protected void VerifyPrismGoBackAsync(INavigationParameters parameters, bool useModalNavigation, bool animated, Func<Times> times)
		{
			var mockPlatformNavigation = GetMock<INavigationService>()
				.As<IPlatformNavigationService>();

			mockPlatformNavigation.Verify(x => x.GoBackAsync(parameters, useModalNavigation, animated), times);
		}

		/// <summary>
		/// Verify that IPlatformNavigationService.GoBackAsync extension method was called
		/// </summary>
		/// <param name="parameterValidation">Predicate that is passed to Moq.It.Is to validate the expected navigation parameters</param>
		/// <param name="useModalNavigation">Whether or not the expected navigation should have been modal</param>
		/// <param name="animated">Whether or not the expected navigation should have been animated</param>
		/// <param name="times">The Moq.Times object that represents the expected number of times IPlatformNavigationService.GoBackAsync should have been called</param>
		protected void VerifyPrismGoBackAsync(Expression<Func<INavigationParameters, bool>> parameterValidation, bool useModalNavigation, bool animated, Func<Times> times)
		{
			var mockPlatformNavigation = GetMock<INavigationService>()
				.As<IPlatformNavigationService>();

			mockPlatformNavigation.Verify(x => x.GoBackAsync(It.Is(parameterValidation), useModalNavigation, animated), times);
		}

		#endregion IPlatformNavigationService Validation

		#region IInitialize

		/// <summary>
		/// Resolves the ViewModel and calls Initialize
		/// </summary>
		/// <typeparam name="T">The ViewModel type to resolve</typeparam>
		/// <param name="parameters">The navigation parameters to pass through to Initialize</param>
		/// <returns>The resolved instance of the desired ViewModel</returns>
		protected T ResolveAndCallInitialize<T>(INavigationParameters parameters = null)
			where T : BindableBase, IInitialize
		{
			T vm = ResolveType<T>();
			var navParams = CreateNavigationParameters(NavigationMode.New, parameters);

			vm.Initialize(navParams);

			return vm;
		}

		#endregion IInitialize

		#region IInitializeAsync

		/// <summary>
		/// Resolves the ViewModel and calls IInitializeAsync
		/// </summary>
		/// <typeparam name="T">The ViewModel type to resolve</typeparam>
		/// <param name="parameters">The navigation parameters to pass through to InitializeAsync</param>
		/// <returns>The resolved instance of the desired ViewModel</returns>
		protected async Task<T> ResolveAndCallInitializeAsync<T>(INavigationParameters parameters = null)
			where T : BindableBase, IInitializeAsync
		{
			T vm = ResolveType<T>();
			var navParams = CreateNavigationParameters(NavigationMode.New, parameters);

			await vm.InitializeAsync(navParams);

			return vm;
		}

		#endregion IInitializeAsync

		#region INavigatedAware

		/// <summary>
		/// Resolves the ViewModel and calls OnNavigatedTo
		/// </summary>
		/// <typeparam name="T">The ViewModel type to resolve</typeparam>
		/// <param name="navigationMode">The navigation mode that will be added to the navigation parameters</param>
		/// <param name="parameters">The navigation parameters to pass through to OnNavigatedFrom</param>
		/// <returns>The resolved instance of the desired ViewModel</returns>
		protected T ResolveAndCallOnNavigatedTo<T>(NavigationMode navigationMode = NavigationMode.New, INavigationParameters parameters = null)
			where T : BindableBase, INavigatedAware
		{
			var vm = ResolveType<T>();
			var navParams = CreateNavigationParameters(navigationMode, parameters);

			vm.OnNavigatedTo(navParams);

			return vm;
		}

		/// <summary>
		/// Resolves the ViewModel and calls OnNavigatedFrom
		/// </summary>
		/// <typeparam name="T">The ViewModel type to resolve</typeparam>
		/// <param name="navigationMode">The navigation mode that will be added to the navigation parameters</param>
		/// <param name="parameters">The navigation parameters to pass through to OnNavigatedFrom</param>
		/// <returns>The resolved instance of the desired ViewModel</returns>
		protected T ResolveAndCallOnNavigatedFrom<T>(NavigationMode navigationMode = NavigationMode.New, INavigationParameters parameters = null)
			where T : BindableBase, INavigatedAware
		{
			T vm = ResolveType<T>();
			var navParams = CreateNavigationParameters(navigationMode, parameters);

			vm.OnNavigatedFrom(navParams);

			return vm;
		}

		#endregion INavigatedAware

		#region IPageLifeCycleAware

		/// <summary>
		/// Resolves the ViewModel and calls OnAppearing
		/// </summary>
		/// <typeparam name="T">The ViewModel type to resolve</typeparam>
		/// <returns>The resolved instance of the desired ViewModel</returns>
		protected T ResolveAndCallOnAppearing<T>()
			 where T : BindableBase, IPageLifecycleAware
		{
			T vm = ResolveType<T>();

			vm.OnAppearing();

			return vm;
		}

		/// <summary>
		/// Resolves the ViewModel and calls OnDisappearing
		/// </summary>
		/// <typeparam name="T">The ViewModel type to resolve</typeparam>
		/// <returns>The resolved instance of the desired ViewModel</returns>
		protected T ResolveAndCallOnDisappearing<T>()
			where T : BindableBase, IPageLifecycleAware
		{
			T vm = ResolveType<T>();

			vm.OnDisappearing();

			return vm;
		}

		#endregion IPageLifeCycleAware

		protected INavigationParameters CreateNavigationParameters(NavigationMode navigationMode, INavigationParameters parameters)
		{
			if (parameters == null)
				parameters = new NavigationParameters();

			INavigationParametersInternal internalParams = parameters as INavigationParametersInternal;
			internalParams.Add("__NavigationMode", navigationMode);

			return parameters;
		}
	}
}