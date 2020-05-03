using System;
using NUnit.Framework;

namespace Ease.NUnit
{
	public abstract class NUnitContainerTestBase : ContainerTestBase
	{
		private Action _onPerTestSetup;

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
