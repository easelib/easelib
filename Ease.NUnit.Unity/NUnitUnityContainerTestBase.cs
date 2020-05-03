using Ease.Unity;
using NUnit.Framework;
using System;

namespace Ease.NUnit.Unity
{
	public abstract class NUnitUnityContainerTestBase : UnityContainerTestBase
	{
		private Action _onPerTestSetup;

		public NUnitUnityContainerTestBase()
		{
			RegisterPerTestSetup(() => 
			{
				_restter.Reset();
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
