using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketManagement.Entities;
using MarketManagement.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace MarketManagement.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductController : ControllerBase
    {
        private readonly IService<Product> _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IService<Product> service,
            ILogger<ProductController> logger)
        {
            _productService = service;
            _logger = logger;
        }

        /// <summary>
        /// Retrieve all products
        /// </summary>
        /// <returns>List of all products</returns>
        /// <response code="200"> Products have been retrieved</response>
        /// <response code="404"> No product was found</response> 
        // GET: api/Products
        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProducts()
        {
            _logger.LogInformation("GET: api/Products called");
            var products = await _productService.GetAll();
            if (!products.Any())
            {
                return NotFound("There is no product yet");
            }
            return Ok(products);
        }

        /// <summary>
        /// Retrieve specific product
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200"> Specified product has been retrieved</response>
        /// <response code="404"> Specified product was not found</response> 
        // GET: api/Products/1
        [Authorize]
        [HttpGet("{id}", Name = "Get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProduct(string id)
        {
            _logger.LogInformation("GET: api/Products/{0} called",id);
            var product = await _productService.Get(id);
            if (product == null)
            {
                return NotFound("No product found");
            }
            return Ok(product);
        }

        /// <summary>
        /// Add a product
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     
        ///     POST /Products
        ///     {
        ///         "id": "8",
        ///         "name": "Table",
        ///         "price": 60,
        ///         "currency": "TRY"
        ///     }
        ///     
        /// </remarks>
        /// <param name="product"></param>
        /// <response code="200"> Specified product has been created successfully</response>
        /// <response code="400"> Bad Request</response> 
        // POST: api/Products
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            _logger.LogInformation("POST: api/Products/ called from Product Controller");
            var result = await _productService.Create(product);
            if (result)
                return StatusCode(StatusCodes.Status201Created);
            else
                return BadRequest();
        }

        /// <summary>
        /// Update existed product
        /// </summary>
        /// <remarks>
        /// Note that given id must be existed.
        /// Sample request:
        /// 
        ///      PUT /Products
        ///      {
        ///         "id": "8",
        ///         "name": "Table",
        ///         "price": 60,
        ///         "currency": "TRY"
        ///      }
        /// 
        /// </remarks>
        /// <param name="product"></param>
        /// <returns>
        /// <response code="200"> Specified product has been updated successfully</response>
        /// <response code="404"> Specified product was not found</response> 
        /// </returns>
        // PUT: api/Products/
        [Authorize]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProduct([FromBody] Product product)
        {
            var success = await _productService.Update(product);
            if (success)
                return Ok("Product has been updated successfully.");
            else
                return NoProductFoundCorrespondingId();
        }

        /// <summary>
        /// Delete specified product.
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200"> Specified product has been deleted successfully</response>
        /// <response code="404"> Specified product was not found</response> 
        // DELETE: api/Products/5
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var success = await _productService.Delete(id);
            if (success)
            {
                return Ok("Product has been deleted");
            }
            return NoProductFoundCorrespondingId();
        }

        private IActionResult NoProductFoundCorrespondingId()
        {
            return NotFound("No product found against this id");
        }
    }
}
