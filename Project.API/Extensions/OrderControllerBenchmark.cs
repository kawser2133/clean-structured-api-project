using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Project.API.Controllers;
using Project.Core.Interfaces.IServices;

namespace Project.API.Extensions
{
    public class OrderControllerBenchmark
    {
        private readonly OrderController _orderController;

        public OrderControllerBenchmark()
        {
            // Initialize any dependencies needed by the controller here
            // For example, you might need to create a mock logger and service
            // to pass to the controller's constructor.
            var logger = new Mock<ILogger<OrderController>>().Object;
            var orderService = new Mock<IOrderService>().Object;

            _orderController = new OrderController(logger, orderService);
        }

        [Benchmark]
        public async Task<IActionResult> GetOrderById()
        {
            return await _orderController.Get(7);
        }

        [Benchmark]
        public async Task<IActionResult> GetOrderList()
        {
            return await _orderController.Get();
        }

        [Benchmark]
        public async Task<IActionResult> GetPaginatedOrderList()
        {
            return await _orderController.Get(2, 50);
        }
    }
}
