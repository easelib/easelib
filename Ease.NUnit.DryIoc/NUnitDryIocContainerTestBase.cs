using DryIoc;
using Ease.DryIoc;
using NUnit.Framework;
using System;

namespace Ease.NUnit.DryIoc
{
	public abstract class NUnitDryIocContainerTestBase : DryIocContainerTestBase
	{
		private Action _onPerTestSetup;

		protected NUnitDryIocContainerTestBase()
		{
			RegisterPerTestSetup(() =>
			{
				_scopeContext?.Dispose();
				_scopeContext = _container.OpenScope();
			});
		}

		[SetUp]
		public void PerTestSetup()
		{
			_onPerTestSetup?.Invoke();
		}

		protected void RegisterPerTestSetup(Action perTestSetup)
		{
			_onPerTestSetup += perTestSetup;
		}
	}
}
