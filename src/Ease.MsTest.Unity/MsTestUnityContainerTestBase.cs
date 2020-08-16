using Ease.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Ease.MsTest.Unity
{
    public abstract class MsTestUnityContainerTestBase : UnityContainerTestBase
	{
		private Action onPerTestSetup;

		public MsTestUnityContainerTestBase()
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

