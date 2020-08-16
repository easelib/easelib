namespace lib
{

	public class OrderService : IOrderService
    {
        private IOrderRepository OrderRepository { get; }
        public OrderService(IOrderRepository orderRepository)
        {
            OrderRepository = orderRepository;
        }

        public Order[] GetCustomerOrders(int customerId)
        {
            return OrderRepository.GetOrdersByCustomerId(customerId);
        }

    }
}
