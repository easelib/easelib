using Ease.DryIoc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Ease.MsTest.DryIoc
{
    public abstract class MsTestDryIocContainerTestBase : DryIocContainerTestBase
	{
		private Action onPerTestSetup;

		public MsTestDryIocContainerTestBase()
		{
			RegisterPerTestSetup(() =>
			{
				ResetLifetime();
			});
		}

		[TestInitialize]
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

