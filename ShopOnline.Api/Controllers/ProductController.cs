using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Api.Extensions;
using ShopOnline.Api.Repositories.Contracts;
using ShopOnline.Models.DTOs;

namespace ShopOnline.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _productRepository;

    public ProductController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> GetItems()
    {
        try
        {
            var products = await _productRepository.GetItems();

            if (products is null)
            {
                return NotFound();
            }
            else
            {
                var productDTOs = products.ConvertToDTO();
                return Ok(productDTOs);
            }
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductDTO>> GetItem(int id)
    {
        try
        {
            var product = await _productRepository.GetItem(id);

            if (product is null)
            {
                return BadRequest();
            }
            else
            {
                var productDTO = product.ConvertToDTO();
                return Ok(productDTO);
            }
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
        }
    }

    [HttpGet]
    [Route(nameof(GetProductCategories))]
    public async Task<ActionResult<IEnumerable<ProductCategoryDTO>>> GetProductCategories()
    {
        try
        {
            var productCategories = await _productRepository.GetCategories();
            var productCategoryDTOs = productCategories.ConvertToDTO();
            return Ok(productCategoryDTOs);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
        }
    }

    [HttpGet]
    [Route("{categoryId}/GetItemsByCategory")]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> GetItemsByCategory(int categoryId)
    {
        try
        {
            var products = await _productRepository.GetItemsByCategory(categoryId);
            var productDTOs = products.ConvertToDTO();

            return Ok(productDTOs);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");

        }
    }

}
