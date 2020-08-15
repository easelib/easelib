using Ease.Unity;
using NUnit.Framework;
using System;

namespace Ease.NUnit.Unity
{
	public abstract class NUnitUnityContainerTestBase : UnityContainerTestBase
	{
		private Action onPerTestSetup;

		public NUnitUnityContainerTestBase()
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
