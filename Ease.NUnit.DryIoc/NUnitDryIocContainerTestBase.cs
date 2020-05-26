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
				ResetLifetime();
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
