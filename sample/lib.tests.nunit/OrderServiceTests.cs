using NUnit.Framework;
using Ease.NUnit.DryIoc;
using Moq;
using System;
using lib;
using System.Security.Cryptography.X509Certificates;

namespace lib.tests.nunit
{
    public class OrderServiceTests : NUnitDryIocContainerTestBase
    {
		protected Action<Mock<IOrderRepository>> OnIOrderRepositoryCreated;

        protected override void RegisterTypes()
		{
			OnIOrderRepositoryCreated += (mock) =>
			{
                mock.Setup(or => or.GetOrdersByCustomerId(It.IsAny<int>())).Returns(new Order[]{ new Order() });
			};

			RegisterMockType(() => OnIOrderRepositoryCreated);
            RegisterType<OrderService>();
		}

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetCustomerOrders_ReturnsExpectedResults()
        {
            var os = ResolveType<OrderService>();
            var orders = os.GetCustomerOrders(1);
            Assert.AreEqual(1, orders.Length);
        }

        [Test]
        public void GetCustomerOrders_CallasIOrderRepositryGetOrdersByCustomerId()
        {
            var os = ResolveType<OrderService>();
            var orders = os.GetCustomerOrders(1);
            ValidateMock<IOrderRepository>(mock => {
                mock.Verify(x => x.GetOrdersByCustomerId(It.IsAny<int>()), Times.Once);
            });
        }

        [Test]
        public void GetCustomerOrders_CallasIOrderRepositryGetOrdersByCustomerId_WithExpectedId()
        {
            var os = ResolveType<OrderService>();
            var orders = os.GetCustomerOrders(1);
            ValidateMock<IOrderRepository>(mock => {
                mock.Verify(x => x.GetOrdersByCustomerId(1), Times.Once);
            });
        }
    }
}