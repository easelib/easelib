using DryIoc;
using Ease.DryIoc;
using NUnit.Framework;
using System;

namespace Ease.NUnit.DryIoc
{
	public abstract class NUnitDryIocContainerTestBase : DryIocContainerTestBase
	{
		private Action onPerTestSetup;

		protected NUnitDryIocContainerTestBase()
		{
			RegisterPerTestSetup(() =>
			{
				ScopeContext?.Dispose();
				ScopeContext = Container.OpenScope();
			});
		}

		[SetUp]
		public void PerTestSetup()
		{
			onPerTestSetup?.Invoke();
		}

		protected void RegisterPerTestSetup(Action perTestSetup)
		{
			onPerTestSetup += perTestSetup;
		}
	}
}
