﻿using Moq;
using System;
using Ease.TestCommon;
using Prism.Navigation;

#if IS_MSTEST
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Ease.MsTest.PrismForms.Tests
#elif IS_NUNIT
using NUnit.Framework;
#if IS_DRYIOC
namespace Ease.NUnit.DryIoc.PrismForms.Tests
#elif IS_UNITY
namespace Ease.NUnit.Unity.PrismForms.Tests
#endif
#elif IS_XUNIT
using Xunit;
#if IS_DRYIOC
namespace Ease.XUnit.DryIoc.PrismForms.Tests
#endif
#endif
{
#if IS_MSTEST
	[TestClass]
#elif IS_NUNIT
	[TestFixture]
#endif
	public class ScopeTest : PrismFormsTestBase
	{

		private static readonly string _iRepoDefaultMyPropertyValue = "DefaultValue";
		private static readonly string _iRepoOverridenMyPropertyValue = "NewValue";

		private static Action<Mock<IRepo>> onIRepoMockCreated;
		private static Action<Mock<IRepo>> configureIRepoMockWithDefaultValue = mock =>
		{
			mock.SetupGet(s => s.MyProperty)
				.Returns(_iRepoDefaultMyPropertyValue);
		};
		private static Action<Mock<IRepo>> configureIRepoWithOverridenValue = mock =>
		{
			mock.SetupGet(s => s.MyProperty)
				.Returns(_iRepoOverridenMyPropertyValue);
		};

#if (IS_MSTEST || IS_XUNIT)
		public ScopeTest()
		{
			onIRepoMockCreated = configureIRepoMockWithDefaultValue;
		}

		protected override void RegisterTypes()
		{
			base.RegisterTypes();
			RegisterMockType(() => onIRepoMockCreated);
		}
#else
		public ScopeTest()
		{
			RegisterMockType(() => onIRepoMockCreated);
#if IS_NUNIT
			RegisterPerTestSetup(() =>
			{
				onIRepoMockCreated = configureIRepoMockWithDefaultValue;
			});
#endif
		}
#endif

#if IS_MSTEST
		[TestMethod]
#elif IS_NUNIT
		[Test]
#elif IS_XUNIT
		[Fact]
#endif
		public void IRepoIsSetupWithDefaultCreatedCallback()
		{
			var vm = ResolveType<VM>();

#if IS_XUNIT
			Assert.Equal(_iRepoDefaultMyPropertyValue, vm.MyRepoProperty);
#else
			Assert.AreEqual(_iRepoDefaultMyPropertyValue, vm.MyRepoProperty);
#endif
		}


#if IS_MSTEST
		[TestMethod]
#elif IS_NUNIT
		[Test]
#elif IS_XUNIT
		[Fact]
#endif
		public void IRepoIsSetupWithOverridenCreatedCallback()
		{
			onIRepoMockCreated += configureIRepoWithOverridenValue;
			var vm = ResolveType<VM>();

#if IS_XUNIT
			Assert.Equal(_iRepoOverridenMyPropertyValue, vm.MyRepoProperty);
			Assert.Null(vm.MyStringProperty);
#else
			Assert.AreEqual(_iRepoOverridenMyPropertyValue, vm.MyRepoProperty);
			Assert.IsNull(vm.MyStringProperty);
#endif
		}


#if IS_MSTEST
		[TestMethod]
#elif IS_NUNIT
		[Test]
#elif IS_XUNIT
		[Fact]
#endif
		public void IRepoIsSetupWithNoCallback()
		{
			onIRepoMockCreated = null;
			var vm = ResolveType<VM>();
#if IS_XUNIT
			Assert.Null(vm.MyRepoProperty);
#else
			Assert.IsNull(vm.MyRepoProperty);
#endif
		}


#if IS_MSTEST
		[TestMethod]
#elif IS_NUNIT
		[Test]
#elif IS_XUNIT
		[Fact]
#endif
		public void VmCallsRepoSaveDataWhenDoSaveData()
		{
			onIRepoMockCreated += configureIRepoWithOverridenValue;
			var vm = ResolveType<VM>();
			vm.DoSaveData();
			ValidateMock<IRepo>(m => m.Verify(i => i.SaveData(), Times.Once));
		}


#if IS_MSTEST
		[DataTestMethod]
		[DataRow(1)]
		[DataRow(2)]
		[DataRow(3)]
		[DataRow(4)]
		[DataRow(5)]
		[DataRow(6)]
		[DataRow(7)]
		[DataRow(8)]
		[DataRow(9)]
		[DataRow(10)]
		public void RepoSaveDataCallHistoryIsResetBetweenCalls(int time)
#elif IS_NUNIT
		[Test]
		public void RepoSaveDataCallHistoryIsResetBetweenCalls([Range(1, 10)]int time)
#elif IS_XUNIT
		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(3)]
		[InlineData(4)]
		[InlineData(5)]
		[InlineData(6)]
		[InlineData(7)]
		[InlineData(8)]
		[InlineData(9)]
		[InlineData(10)]
		public void RepoSaveDataCallHistoryIsResetBetweenCalls(int time)
#endif
		{
			Func<Times> expected = Times.Never;
			var vm = ResolveType<VM>();
			if (time % 2 == 0)
			{
				expected = Times.Once;
				vm.DoSaveData();

			}
			ValidateMock<IRepo>(m => m.Verify(i => i.SaveData(), expected));
		}


#if IS_MSTEST
		[DataTestMethod]
		[DataRow(1)]
		[DataRow(2)]
		[DataRow(3)]
		[DataRow(4)]
		[DataRow(5)]
		[DataRow(6)]
		[DataRow(7)]
		[DataRow(8)]
		[DataRow(9)]
		[DataRow(10)]
		public void ObjectsAreResetBewteenCalls(int time)
#elif IS_NUNIT
		[Test]
		public void ObjectsAreResetBewteenCalls([Range(1, 10)]int time)
#elif IS_XUNIT
		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(3)]
		[InlineData(4)]
		[InlineData(5)]
		[InlineData(6)]
		[InlineData(7)]
		[InlineData(8)]
		[InlineData(9)]
		[InlineData(10)]
		public void ObjectsAreResetBewteenCalls(int time)
#endif
		{
			var testValue = "test";
			var expected = default(string);
			var vm = ResolveType<VM>();
			if (time % 2 == 0)
			{
				vm.MyStringProperty = testValue;
				expected = testValue;
			}
#if IS_XUNIT
			Assert.Equal(expected, vm.MyStringProperty);
#else
			Assert.AreEqual(expected, vm.MyStringProperty);
#endif
		}


#if IS_MSTEST
		[TestMethod]
#elif IS_NUNIT
		[Test]
#elif IS_XUNIT
		[Fact]
#endif
		public void VmCallsNavigationServiceWithTargetWhenDoNavigation()
		{
			var target = "TargetPath";
			var vm = ResolveType<VM>();
			vm.DoNavigation(target).Wait();
			VerifyNavigation(target, Times.Once);
		}

#if IS_MSTEST
		[TestMethod]
#elif IS_NUNIT
		[Test]
#elif IS_XUNIT
		[Fact]
#endif
		public void VmCallsNavigationServiceWithTargetWhenDoNavigationWithParameters()
		{
			var target = "TargetPath";
			var vm = ResolveType<VM>();
			vm.DoNavigationWithParameters(target).Wait();
			VerifyNavigation(target, null as INavigationParameters, Times.Once);
		}

#if IS_MSTEST
		[TestMethod]
#elif IS_NUNIT
		[Test]
#elif IS_XUNIT
		[Fact]
#endif
		public void VmCallsNavigationServiceWithParameterValidationWhenDoNavigationWithParameters()
		{
			var target = "TargetPath";
			var vm = ResolveType<VM>();
			vm.DoNavigationWithParameters(target).Wait();
			VerifyNavigation(target, p => p.ContainsKey("x") && p.GetValue<string>("x").Equals("1"), Times.Once);
		}

#if IS_MSTEST
		[TestMethod]
#elif IS_NUNIT
		[Test]
#elif IS_XUNIT
		[Fact]
#endif
		public void VmCallsNavigationServiceWithTargetWhenDoNavigationWithParametersCheckingSpecificParameters()
		{
			var target = "TargetPath";
			var vm = ResolveType<VM>();
			vm.DoNavigationWithParameters(target).Wait();
			VerifyNavigation(target, new NavigationParameters("x=1"), Times.Once);
		}
	}
}
