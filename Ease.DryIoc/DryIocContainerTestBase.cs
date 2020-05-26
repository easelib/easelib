using DryIoc;
using Moq;
using System;

namespace Ease.DryIoc
{
    public abstract class DryIocContainerTestBase : ContainerTestBase
	{
		protected Container Container;
		protected IResolverContext ScopeContext;

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

			Container = new Container(rules);
			ScopeContext = Container.OpenScope();
		}

		protected override void RegisterType<T>()
		{
			Container.Register<T>();
		}

		protected override void RegisterType<TInterface, TImplementation>()
		{
			Container.Register<TInterface, TImplementation>();
		}

		protected override void RegisterTypeFactory<T>(Func<T> factory)
		{
			Container.RegisterDelegate<T>(c => factory());
		}

		protected override void RegisterMockType<T>(Func<Action<Mock<T>>> onMockInstanceCreatedFactory)
		{
			Container.Register<T>(made: Made.Of<T>(() => CreateAndInitializeMockInstance(onMockInstanceCreatedFactory)));
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
			return ScopeContext.Resolve<T>();
		}
	}
}
