using Moq;
using System;
using Unity;
using Unity.Lifetime;

namespace Ease.Unity
{
	public abstract class UnityContainerTestBase : ContainerTestBase
	{
		private UnityContainer Container;
		private LifetimeResetter Resetter { get; set; }

		public UnityContainerTestBase()
		{
			CreateContainer();
			RegisterTypes();
		}

		protected override void CreateContainer()
		{
			Resetter = new LifetimeResetter();
			Container = new UnityContainer();
		}

		protected void ResetLifetime()
        {
			Resetter.Reset();
		}

		private void RegisterResettableType<T>()
		{
			Container.RegisterType<T>(new ResettableLifetimeManager(Resetter));
		}

		private void RegisterResettableType<TInterface, TImplementation>() where TImplementation : TInterface
		{
			Container.RegisterType<TInterface, TImplementation>(new ResettableLifetimeManager(Resetter));
		}

		private void RegisterResettableTypeFactory<T>(Func<T> factory)
		{
			RegisterResettableType<T>(factory);
		}

		private void RegisterResettableType<T>(Func<T> factory)
		{
			Container.RegisterFactory<T>(c => factory(), new ResettableLifetimeManager(Resetter));
		}

		protected override void RegisterMockType<T>(Func<Action<Mock<T>>> onCreatedCallbackFactory)
		{
			RegisterResettableType<T>(() => CreateAndInitializeMockInstance(onCreatedCallbackFactory));
		}

		private static T CreateAndInitializeMockInstance<T>(Func<Action<Mock<T>>> onMockInstanceCreatedFactory) where T : class
		{
			var mock = new Mock<T>();
			var onCreatedCallback = onMockInstanceCreatedFactory();
			onCreatedCallback?.Invoke(mock);
			return mock.Object;
		}

		protected override void RegisterType<T>()
		{
			RegisterResettableType<T>();
		}

		protected override void RegisterType<TInterface, TImplementation>()
		{
			RegisterResettableType<TInterface, TImplementation>();
		}

		protected override void RegisterTypeFactory<T>(Func<T> factory)
		{
			RegisterResettableTypeFactory<T>(factory);
		}

		protected override T ResolveType<T>()
		{
			return Container.Resolve<T>();
		}

		protected class LifetimeResetter
		{
			public Action OnReset;

			public void Reset()
			{
				OnReset?.Invoke();
			}
		}

		protected class ResettableLifetimeManager : 
			LifetimeManager,
			IInstanceLifetimeManager, 
			ITypeLifetimeManager, 
			IFactoryLifetimeManager
		{

			protected object instance = NoValue;

			private LifetimeResetter LifetimeResetter { get; }

			public ResettableLifetimeManager(LifetimeResetter lifetimeResetter)
			{
				lifetimeResetter.OnReset += () => RemoveValue(null);
				LifetimeResetter = lifetimeResetter;
			}

			public override void SetValue(object newValue, ILifetimeContainer container = null)
			{
				instance = newValue;
			}

			public override void RemoveValue(ILifetimeContainer container = null)
			{
				instance = NoValue;
			}

			public override object GetValue(ILifetimeContainer container = null)
			{
				return instance;
			}

			protected override LifetimeManager OnCreateLifetimeManager()
			{
				return new ResettableLifetimeManager(LifetimeResetter);
			}
		}
	}
}
