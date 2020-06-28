using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketManagement.Entities;
using MarketManagement.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace MarketManagement.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductController : ControllerBase
    {
        private readonly IService<Product> _productService;

        public ProductController(IService<Product> service)
        {
            _productService = service;
        }

        /// <summary>
        /// Retrieve all products
        /// </summary>
        /// <returns>List of all products</returns>
        // GET: api/Products
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
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
        /// <returns></returns>
        // GET: api/Products/1
        [Authorize]
        [HttpGet("{id}", Name = "Get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProduct(string id)
        {
            var product = await _productService.Get(id);
            if (product == null)
            {
                return NotFound("No product found");
            }
            return Ok(product);
        }

        /// <summary>
        /// Adding a product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        // POST: api/Products
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            var result = await _productService.Create(product);

            if (result)
                return StatusCode(StatusCodes.Status201Created);
            else
                return BadRequest();
        }

        /// <summary>
        /// Updating existed product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
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
        /// <returns></returns>
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
