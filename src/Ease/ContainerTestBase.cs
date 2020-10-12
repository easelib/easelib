using Moq;
using System;

namespace Ease
{
	public abstract class ContainerTestBase
	{
		abstract protected T ResolveType<T>()
			where T : class;
		abstract protected void RegisterType<T>() 
			where T : class;
		abstract protected void RegisterType<TInterface, TImplementation>() 
			where TInterface : class
			where TImplementation : TInterface, new();
		abstract protected void RegisterTypeFactory<T>(Func<T> factory)
			where T : class;
		abstract protected void RegisterMockType<T>()
			where T : class;
		abstract protected void RegisterMockType<T>(Func<Action<Mock<T>>> onCreatedCallbackFactory)
			where T : class;

		protected abstract void CreateContainer();

		protected abstract void RegisterTypes();

		protected Mock<T> GetMock<T>() 
			where T : class
		{
			var instance = ResolveType<T>();
			var mock = Mock.Get<T>(instance);
			return mock;
		}

		protected void ValidateMock<T>(Action<Mock<T>> validationAction) where T : class
		{
			var instance = ResolveType<T>();
			var mock = Mock.Get(instance);
			validationAction(mock);
		}
	}
}
