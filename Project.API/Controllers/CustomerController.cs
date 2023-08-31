using Microsoft.AspNetCore.Mvc;
using Project.Core.Entities.Business;
using Project.Core.Interfaces.IServices;

namespace Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerService _customerService;

        public CustomerController(ILogger<CustomerController> logger, ICustomerService customerService)
        {
            _logger = logger;
            _customerService = customerService;
        }


        // GET: api/customer/paginated
        [HttpGet("paginated")]
        public async Task<IActionResult> Get(int? pageNumber, int? pageSize)
        {
            try
            {
                int pageSizeValue = (pageSize ?? 4);
                int pageNumberValue = (pageNumber ?? 1);

                //Get peginated data
                var customers = await _customerService.GetPaginatedCustomers(pageNumberValue, pageSizeValue);

                return Ok(customers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving customers");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        // GET: api/customer
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var customers = await _customerService.GetCustomers();
                return Ok(customers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving customers");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }


        // GET api/customer/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var data = await _customerService.GetCustomer(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                if (ex.Message == "No data found")
                {
                    return StatusCode(StatusCodes.Status404NotFound, ex.Message);
                }
                _logger.LogError(ex, $"An error occurred while retrieving the customer");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        // POST api/customer
        [HttpPost]
        public async Task<IActionResult> Create(CustomerViewModel model)
        {
            if (ModelState.IsValid)
            {
                string message = "";
                if (await _customerService.IsExists("Email", model.Email))
                {
                    message = $"The customer email- '{model.Email}' already exists";
                    return StatusCode(StatusCodes.Status400BadRequest, message);
                }

                try
                {
                    var data = await _customerService.Create(model);
                    return Ok(data);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"An error occurred while adding the customer");
                    message = $"An error occurred while adding the customer- {ex.Message}";

                    return StatusCode(StatusCodes.Status500InternalServerError, message);
                }
            }
            return StatusCode(StatusCodes.Status400BadRequest, "Please input all required data");
        }

        // PUT api/customer/5
        [HttpPut]
        public async Task<IActionResult> Edit(CustomerViewModel model)
        {
            if (ModelState.IsValid)
            {
                string message = "";
                if (await _customerService.IsExistsForUpdate(model.Id, "Email", model.Email))
                {
                    message = $"The customer email- '{model.Email}' already exists";
                    return StatusCode(StatusCodes.Status400BadRequest, message);
                }

                try
                {
                    await _customerService.Update(model);
                    return Ok();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"An error occurred while updating the customer");
                    message = $"An error occurred while updating the customer- {ex.Message}";

                    return StatusCode(StatusCodes.Status500InternalServerError, message);
                }
            }
            return StatusCode(StatusCodes.Status400BadRequest, "Please input all required data");
        }


        // DELETE api/customer/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _customerService.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the customer");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the customer- " + ex.Message);
            }
        }

    }
}
