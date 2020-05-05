using Moq;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Linq.Expressions;

#if IS_MSTEST
namespace Ease.MsTest.PrismForms
#elif (IS_DRYIOC && IS_NUNIT)
namespace Ease.NUnit.DryIoc.PrismForms
#elif (IS_DRYIOC && IS_XUNIT)
namespace Ease.XUnit.DryIoc.PrismForms
#elif (IS_UNITY && IS_NUNIT)
namespace Ease.NUnit.Unity.PrismForms
#elif (IS_UNITY && IS_XUNIT)
namespace Ease.XUnit.Unity.PrismForms
#endif
{
	public class PrismFormsTestBase
#if IS_MSTEST
	: MsTestDryIocContainerTestBase
#elif (IS_DRYIOC && IS_NUNIT)
	: NUnitDryIocContainerTestBase
#elif (IS_DRYIOC && IS_XUNIT)
	: XUnitDryIocContainerTestBase
#elif (IS_UNITY && IS_NUNIT)
	: NUnitUnityContainerTestBase
#elif (IS_UNITY && IS_XUNIT)
	: XUnitUnityContainerTestBase
#endif
	{
		protected Action<Mock<INavigationService>> OnINavigationServiceMockCreated;

		protected Action<Mock<IPageDialogService>> OnIPageDialogServiceMockCreated;

		protected Action<Mock<IEventAggregator>> OnIEventAggregatorMockCreated;

		private bool _baseRegisterTypesCalled;

		public PrismFormsTestBase()
		{
			if ( !_baseRegisterTypesCalled )
				throw new InvalidOperationException("Inherited classes must call base.RegisterTypes() when overriding");
		}

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

			_baseRegisterTypesCalled = true;
		}

		#region INavigationService Validation

		protected void AddNavigationCallback(Action<Uri, NavigationParameters, bool?, bool> callback)
		{
			var navServiceMock = Mock.Get(ResolveType<INavigationService>());
			navServiceMock.Setup(s => s.NavigateAsync(It.IsAny<Uri>(), It.IsAny<NavigationParameters>(), It.IsAny<bool?>(), It.IsAny<bool>()))
				.Callback(callback);
		}

		protected void AddNavigationCallback(Action<String, NavigationParameters, bool?, bool> callback)
		{
			var navServiceMock = Mock.Get(ResolveType<INavigationService>());
			navServiceMock.Setup(s => s.NavigateAsync(It.IsAny<String>(), It.IsAny<NavigationParameters>(), It.IsAny<bool?>(), It.IsAny<bool>()))
				.Callback(callback);
		}

		protected void VerifyNavigation(Uri uri, Func<Times> times)
		{
			var navServiceMock = Mock.Get(ResolveType<INavigationService>());
			navServiceMock.Verify(
				n => n.NavigateAsync(uri), times);
		}

		protected void VerifyNavigation(Uri uri, INavigationParameters parameters, Func<Times> times)
		{
			var navServiceMock = Mock.Get(ResolveType<INavigationService>());
			navServiceMock.Verify(
				n => n.NavigateAsync(uri, parameters), times);
		}

		protected void VerifyNavigation(Uri uri, Expression<Func<INavigationParameters, bool>> parameterValidation, Func<Times> times)
		{
			var navServiceMock = Mock.Get(ResolveType<INavigationService>());
			navServiceMock.Verify(n => n.NavigateAsync(uri, It.Is(parameterValidation)), times);
		}

		protected void VerifyNavigation(string path, Func<Times> times)
		{
			var navServiceMock = Mock.Get(ResolveType<INavigationService>());
			navServiceMock.Verify(
				n => n.NavigateAsync(path), times);
		}

		protected void VerifyNavigation(string path, INavigationParameters parameters, Func<Times> times)
		{
			var navServiceMock = Mock.Get(ResolveType<INavigationService>());
			if (parameters != null)
			{
				navServiceMock.Verify(n => n.NavigateAsync(path, parameters), times);
			}
			else
			{
				navServiceMock.Verify(n => n.NavigateAsync(path, It.IsAny<INavigationParameters>()), times);
			}
		}

		protected void VerifyNavigation(string path, Expression<Func<INavigationParameters, bool>> parameterValidation, Func<Times> times)
		{
			var navServiceMock = Mock.Get(ResolveType<INavigationService>());
			navServiceMock.Verify(n => n.NavigateAsync(path, It.Is(parameterValidation)), times);
		}

		protected void VerifyNavigationGoBack(Func<Times> times)
		{
			var navServiceMock = Mock.Get(ResolveType<INavigationService>());
			navServiceMock.Verify(n => n.GoBackAsync(), times);
		}

		protected void VerifyNavigationGoBack(INavigationParameters parameters, Func<Times> times)
		{
			var navServiceMock = Mock.Get(ResolveType<INavigationService>());
			navServiceMock.Verify(n => n.GoBackAsync(parameters), times);
		}

		protected void VerifyNavigationGoBack(Expression<Func<INavigationParameters, bool>> parameterValidation, Func<Times> times)
		{
			var navServiceMock = Mock.Get(ResolveType<INavigationService>());
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
		protected void VerifyNavigation(Uri uri, INavigationParameters parameters, bool useModalNavigation, bool animated, Func<Times> times)
		{
			var navServiceMock = Mock.Get(ResolveType<INavigationService>());
			var mockPlatformNavigation = navServiceMock.As<IPlatformNavigationService>();

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
		protected void VerifyNavigation(Uri uri, Expression<Func<INavigationParameters, bool>> parameterValidation, bool useModalNavigation, bool animated, Func<Times> times)
		{
			var navServiceMock = Mock.Get(ResolveType<INavigationService>());
			var mockPlatformNavigation = navServiceMock.As<IPlatformNavigationService>();

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
		protected void VerifyNavigation(string path, INavigationParameters parameters, bool useModalNavigation, bool animated, Func<Times> times)
		{
			var navServiceMock = Mock.Get(ResolveType<INavigationService>());
			var mockPlatformNavigation = navServiceMock.As<IPlatformNavigationService>();

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
		protected void VerifyNavigation(string path, Expression<Func<INavigationParameters, bool>> parameterValidation, bool useModalNavigation, bool animated, Func<Times> times)
		{
			var navServiceMock = Mock.Get(ResolveType<INavigationService>());
			var mockPlatformNavigation = navServiceMock.As<IPlatformNavigationService>();

			mockPlatformNavigation.Verify(x => x.NavigateAsync(path, It.Is(parameterValidation), useModalNavigation, animated), times);
		}

		/// <summary>
		/// Verify that IPlatformNavigationService.GoBackAsync extension method was called
		/// </summary>
		/// <param name="parameters">The expected navigation parameters</param>
		/// <param name="useModalNavigation">Whether or not the expected navigation should have been modal</param>
		/// <param name="animated">Whether or not the expected navigation should have been animated</param>
		/// <param name="times">The Moq.Times object that represents the expected number of times IPlatformNavigationService.GoBackAsync should have been called</param>
		protected void VerifyNavigationGoBack(INavigationParameters parameters, bool useModalNavigation, bool animated, Func<Times> times)
		{
			var navServiceMock = Mock.Get(ResolveType<INavigationService>());
			var mockPlatformNavigation = navServiceMock.As<IPlatformNavigationService>();

			mockPlatformNavigation.Verify(x => x.GoBackAsync(parameters, useModalNavigation, animated), times);
		}

		/// <summary>
		/// Verify that IPlatformNavigationService.GoBackAsync extension method was called
		/// </summary>
		/// <param name="parameterValidation">Predicate that is passed to Moq.It.Is to validate the expected navigation parameters</param>
		/// <param name="useModalNavigation">Whether or not the expected navigation should have been modal</param>
		/// <param name="animated">Whether or not the expected navigation should have been animated</param>
		/// <param name="times">The Moq.Times object that represents the expected number of times IPlatformNavigationService.GoBackAsync should have been called</param>
		protected void VerifyNavigationGoBack(Expression<Func<INavigationParameters, bool>> parameterValidation, bool useModalNavigation, bool animated, Func<Times> times)
		{
			//_mockPlatformNavigation.Verify(x => x.GoBackAsync(It.Is(parameterValidation), useModalNavigation, animated), times);
		}

		#endregion IPlatformNavigationService Validation

		protected INavigationParameters CreateNavigationParameters(NavigationMode navigationMode, INavigationParameters parameters)
		{
			if (parameters == null)
				parameters = new NavigationParameters();

			INavigationParametersInternal internalParams = parameters as INavigationParametersInternal;
			internalParams.Add("__NavigationMode", navigationMode);

			return parameters;
		}

		protected T ResolveAndCallOnNavigatedToAsync<T>(NavigationMode navigationMode, INavigationParameters parameters)
			where T : BindableBase, INavigatedAware
		{
			var vm = ResolveType<T>();
			var navParams = CreateNavigationParameters(navigationMode, parameters);

			vm.OnNavigatedTo(navParams);

			return vm;
		}
	}
}