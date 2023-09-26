using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Microsoft.AspNetCore.Mvc;
using Project.Core.Entities.Business;
using Project.Core.Interfaces.IServices;

namespace Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [MemoryDiagnoser]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;

        public OrderController(ILogger<OrderController> logger, IOrderService orderService)
        {
            _logger = logger;
            _orderService = orderService;
        }


        // GET: api/order/paginated
        [HttpGet("paginated")]
        public async Task<IActionResult> Get(int? pageNumber, int? pageSize)
        {
            try
            {
                int pageSizeValue = (pageSize ?? 4);
                int pageNumberValue = (pageNumber ?? 1);

                //Get peginated data
                var orders = await _orderService.GetPaginatedOrders(pageNumberValue, pageSizeValue);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving orders");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        // GET: api/order
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var orders = await _orderService.GetOrders();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving orders");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpGet("sync/{orderId}")]
        public IActionResult GetSync(int orderId)
        {
            try
            {
                var orders = _orderService.GetPaginatedOrdersSync(2, 50);
                _logger.LogError($"1Sync-ThreadId: {Thread.CurrentThread.ManagedThreadId.ToString()}");
                Thread.Sleep(1000);
                _logger.LogError($"2Sync-ThreadId: {Thread.CurrentThread.ManagedThreadId.ToString()}");
                var data = _orderService.GetOrderSync(orderId);
                _logger.LogError($"3Sync-ThreadId: {Thread.CurrentThread.ManagedThreadId.ToString()}");

                return Ok(data);
            }
            catch (Exception ex)
            {
                if (ex.Message == "No data found")
                {
                    return StatusCode(StatusCodes.Status404NotFound, ex.Message);
                }
                _logger.LogError(ex, $"An error occurred while retrieving the order");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpGet("{orderId}")]
        public async Task<IActionResult> Get(int orderId)
        {
            try
            {
                var orders = await _orderService.GetPaginatedOrders(2, 50);
                _logger.LogError($"1Async-ThreadId: {Thread.CurrentThread.ManagedThreadId.ToString()}");
                await Task.Delay(1000);
                _logger.LogError($"2Async-ThreadId: {Thread.CurrentThread.ManagedThreadId.ToString()}");
                var data = await _orderService.GetOrder(orderId);
                _logger.LogError($"3Async-ThreadId: {Thread.CurrentThread.ManagedThreadId.ToString()}");

                return Ok(data);
            }
            catch (Exception ex)
            {
                if (ex.Message == "No data found")
                {
                    return StatusCode(StatusCodes.Status404NotFound, ex.Message);
                }
                _logger.LogError(ex, $"An error occurred while retrieving the order");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        // POST api/order
        [HttpPost]
        public async Task<IActionResult> Create(OrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                string message = "";

                try
                {
                    var data = await _orderService.Create(model);
                    return Ok(data);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"An error occurred while adding the order");
                    message = $"An error occurred while adding the order- {ex.Message}";

                    return StatusCode(StatusCodes.Status500InternalServerError, message);
                }
            }
            return StatusCode(StatusCodes.Status400BadRequest, "Please input all required data");
        }

        // PUT api/order/5
        [HttpPut]
        public async Task<IActionResult> Edit(OrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                string message = "";

                try
                {
                    await _orderService.Update(model);
                    return Ok();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"An error occurred while updating the order");
                    message = $"An error occurred while updating the order- {ex.Message}";

                    return StatusCode(StatusCodes.Status500InternalServerError, message);
                }
            }
            return StatusCode(StatusCodes.Status400BadRequest, "Please input all required data");
        }


        // DELETE api/order/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _orderService.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the order");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the order- " + ex.Message);
            }
        }

    }
}
