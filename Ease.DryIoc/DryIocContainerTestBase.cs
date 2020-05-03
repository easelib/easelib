using DryIoc;
using Moq;
using System;

namespace Ease.DryIoc
{
    public abstract class DryIocContainerTestBase : ContainerTestBase
	{
		protected Container _container;
		protected IResolverContext _scopeContext;

		protected DryIocContainerTestBase()
		{
			CreateContainer();
			RegisterTypes();
		}

		protected override void CreateContainer()
		{
			var rules = Rules.Default
				.WithConcreteTypeDynamicRegistrations()
				.WithTrackingDisposableTransients()
				.WithDefaultReuse(new CurrentScopeReuse());

			_container = new Container(rules);
		}

		protected override void RegisterType<T>()
		{
			_container.Register<T>();
		}

		protected override void RegisterType<TInterface, TImplementation>()
		{
			_container.Register<TInterface, TImplementation>();
		}

		protected override void RegisterTypeFactory<T>(Func<T> factory)
		{
			_container.RegisterDelegate<T>(c => factory());
		}

		protected override void RegisterMockType<T>(Func<Action<Mock<T>>> onMockInstanceCreatedFactory)
		{
			_container.Register<T>(made: Made.Of<T>(() => CreateAndInitializeMockInstance(onMockInstanceCreatedFactory)));
		}

		private static T CreateAndInitializeMockInstance<T>(Func<Action<Mock<T>>> onMockInstanceCreatedFactory) where T : class
		{
			var mock = new Mock<T>();
			var onCreatedCallback = onMockInstanceCreatedFactory();
			onCreatedCallback?.Invoke(mock);
			return mock.Object;
		}

		protected override T ResolveType<T>()
		{
			return _scopeContext.Resolve<T>();
		}
	}
}
