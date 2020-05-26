using DryIoc;
using Ease.DryIoc;
using System;
using System.Collections.Generic;

namespace Ease.MsTest
{
	public abstract class MsTestDryIocContainerTestBase : DryIocContainerTestBase
	{
		private static Dictionary<Type, Container> Containers { get; set; } = new Dictionary<Type, Container>();

		public MsTestDryIocContainerTestBase()
		{
			var key = this.GetType();
			if (!Containers.ContainsKey(key))
			{
				CreateContainer();
				RegisterTypes();

				Containers.Add(key, Container);
			}
			else
			{
				Container = Containers[key];
			}

			ScopeContext = Container.OpenScope();
		}
	}
}

