using Microsoft.AspNetCore.Mvc;
using Project.Core.Entities.Business;
using Project.Core.Interfaces.IServices;

namespace Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;

        public ProductController(ILogger<ProductController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }


        // GET: api/product/paginated
        [HttpGet("paginated")]
        public async Task<IActionResult> Get(int? pageNumber, int? pageSize)
        {
            try
            {
                int pageSizeValue = (pageSize ?? 4);
                int pageNumberValue = (pageNumber ?? 1);

                //Get peginated data
                var products = await _productService.GetPaginatedProducts(pageNumberValue, pageSizeValue);

                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving products");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        // GET: api/product
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var products = await _productService.GetProducts();
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving products");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }


        // GET api/product/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var data = await _productService.GetProduct(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                if (ex.Message == "No data found")
                {
                    return StatusCode(StatusCodes.Status404NotFound, ex.Message);
                }
                _logger.LogError(ex, $"An error occurred while retrieving the product");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        // POST api/product
        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                string message = "";
                if (await _productService.IsExists("Name", model.Name))
                {
                    message = $"The product name- '{model.Name}' already exists";
                    return StatusCode(StatusCodes.Status400BadRequest, message);
                }

                if (await _productService.IsExists("Code", model.Code))
                {
                    message = $"The product code- '{model.Code}' already exists";
                    return StatusCode(StatusCodes.Status400BadRequest, message);
                }

                try
                {
                    var data = await _productService.Create(model);
                    return Ok(data);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"An error occurred while adding the product");
                    message = $"An error occurred while adding the product- " + ex.Message;

                    return StatusCode(StatusCodes.Status500InternalServerError, message);
                }
            }
            return StatusCode(StatusCodes.Status400BadRequest, "Please input all required data");
        }

        // PUT api/product/5
        [HttpPut]
        public async Task<IActionResult> Edit(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                string message = "";
                if (await _productService.IsExistsForUpdate(model.Id, "Name", model.Name))
                {
                    message = "The product name- '{model.Name}' already exists";
                    return StatusCode(StatusCodes.Status400BadRequest, message);
                }

                if (await _productService.IsExistsForUpdate(model.Id, "Code", model.Code))
                {
                    message = $"The product code- '{model.Code}' already exists";
                    return StatusCode(StatusCodes.Status400BadRequest, message);
                }

                try
                {
                    await _productService.Update(model);
                    return Ok();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"An error occurred while updating the product");
                    message = $"An error occurred while updating the product- " + ex.Message;

                    return StatusCode(StatusCodes.Status500InternalServerError, message);
                }
            }
            return StatusCode(StatusCodes.Status400BadRequest, "Please input all required data");
        }


        // DELETE api/product/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _productService.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the product");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the product- " + ex.Message);
            }
        }

        [HttpGet("PriceCheck/{productId}")]
        public async Task<IActionResult> PriceCheck(int productId)
        {
            try
            {
                var price = await _productService.PriceCheck(productId);
                return Ok(price);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while checking product price");
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while checking product price- {ex.Message}");
            }
        }

    }
}
