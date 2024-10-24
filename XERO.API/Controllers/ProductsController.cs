using Microsoft.AspNetCore.Mvc;
using XERO.API.DTO;
using XERO.API.Models;
using XERO.API.NewFolder.XERO.API.Models;
using XERO.API.Services.Interfaces;


namespace XERO.API.Controllers
{
    [Route("products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<Products>> GetAll()
        {
            var products = await _productService.GetAllProducts();
            if (products == null || !products.Any())
            {
                return NoContent();
            }
            return Ok(products);
        }


        [HttpGet("name")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetByName([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("The 'name' query parameter is required."); 
            }

            var products = await _productService.GetProductsByName(name);
            if (products == null || !products.Any())
            {
                return NotFound($"No products found with the name '{name}'."); 
            }

            return Ok(products); 
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(Guid id)
        {
            var product = await _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound($"Product with ID {id} not found.");
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> Create([FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }
            var dto = new ProductDto()
            {
                Name = product.Name,
                Description = product.Description,
                DeliveryPrice = product.DeliveryPrice,
                Price = product.Price
            };
            var createdProduct = await _productService.CreateProduct(dto);
            return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] ProductDto updateProductDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }

            var isUpdated = await _productService.UpdateProduct(id, updateProductDto);

            if (!isUpdated)
            {
                return NotFound($"Product with ID {id} not found.");
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var isDeleted = await _productService.DeleteProduct(id);

            if (!isDeleted)
            {
                return NotFound($"Product with ID {id} not found.");
            }

            return NoContent();
        }

        [HttpGet("{id}/options")]
        public async Task<ActionResult<IEnumerable<ProductOption>>> GetOptions(Guid id)
        {
            var options = await _productService.GetOptionsByProductId(id);

            if (!options.Any())
            {
                return NotFound($"No options found for product with ID {id}");
            }

            return Ok(options);
        }


        [HttpGet("{id}/options/{optionId}")]
        public async Task<ActionResult<ProductOptionDto>> GetOption(Guid id, Guid optionId)
        {
            var option = await _productService.GetOptionByProductIdAndOptionId(id, optionId);

            if (option == null)
            {
                var productExists = await _productService.DoesProductExist(id);
                if (!productExists)
                {
                    return NotFound($"Product with ID {id} not found.");
                }
                else
                {
                    return NotFound($"Option with ID {optionId} not found for product with ID {id}.");
                }
            }

            return Ok(option);
        }


        [HttpPost("{id}/options")]
        public async Task<IActionResult> CreateOption(Guid id, [FromBody] ProductOptionDto optionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var createdOption = await _productService.AddOptionToProduct(id, optionDto);

                return CreatedAtAction(nameof(GetOption), new { id = id, optionId = createdOption.Id }, createdOption);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }


        [HttpPut("{id}/options/{optionId}")]
        public async Task<IActionResult> UpdateOption(Guid id, Guid optionId, [FromBody] ProductOptionDto optionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isUpdated = await _productService.UpdateProductOption(id, optionId, optionDto);

            if (!isUpdated)
            {
                return NotFound($"Option with ID {optionId} not found for product with ID {id}.");
            }

            return Ok();
        }

        [HttpDelete("{id}/options/{optionId}")]
        public async Task<IActionResult> DeleteOption(Guid id, Guid optionId)
        {
            try
            {
                var isDeleted = await _productService.DeleteProductOption(id, optionId);

                if (!isDeleted)
                {
                    return NotFound($"Option with ID {optionId} not found for product with ID {id}.");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
