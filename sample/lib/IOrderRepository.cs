namespace lib
{
	public interface IOrderRepository
	{
		Order[] GetOrdersByCustomerId(int customerId);
	}
}
