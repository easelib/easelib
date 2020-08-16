namespace lib
{
	public interface IOrderService
	{
		Order[] GetCustomerOrders(int customerId);
	}
}
