namespace Project.API.Extensions
{
    using BenchmarkDotNet.Attributes;
    using Project.Core.Entities.Business;
    using Project.Core.Services;

    [MemoryDiagnoser]
    public class OrderServiceBenchmark
    {
        private readonly OrderService _orderService;
        private readonly int _orderId;

        public OrderServiceBenchmark(OrderService orderService, int orderId)
        {
            _orderService = orderService;
            _orderId = orderId;
        }

        [Benchmark]
        public async Task<OrderViewModel> BenchmarkGetOrder()
        {
            return await _orderService.GetOrder(_orderId);
        }
    }

}
