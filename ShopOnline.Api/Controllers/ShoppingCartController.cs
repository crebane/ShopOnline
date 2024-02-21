using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using ShopOnline.Api.Extensions;
using ShopOnline.Api.Repositories.Contracts;
using ShopOnline.Models.DTOs;

namespace ShopOnline.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ShoppingCartController : ControllerBase
{
    private readonly IShoppingCartRepository _shoppingCartRepository;
    private readonly IProductRepository _productRepository;

    public ShoppingCartController(IShoppingCartRepository shoppingCartRepository, IProductRepository productRepository)
    {
        _shoppingCartRepository = shoppingCartRepository;
        _productRepository = productRepository;
    }

    [HttpGet]
    [Route("{userId}/GetItems")]
    public async Task<ActionResult<IEnumerable<CartItemDTO>>> GetItems(int userId)
    {
        try
        {
            var cartItems = await _shoppingCartRepository.GetItems(userId);

            if (cartItems == null)
            {
                return NoContent();
            }
            var products = await _productRepository.GetItems();

            if (products == null)
            {
                throw new Exception("No products exist in the system");
            }

            var cartItemsDTO = cartItems.ConvertToDTO(products);
            return Ok(cartItemsDTO);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CartItemDTO>> GetItem(int id)
    {
        try
        {
            var cartItem = await _shoppingCartRepository.GetItem(id);
            
            if (cartItem == null)
            {
                return NotFound();
            }

            var product = await _productRepository.GetItem(cartItem.ProductId);
            
            if (product == null)
            {
                return NotFound();
            }

            var cartItemDTO = cartItem.ConvertToDTO(product);
            return Ok(cartItemDTO);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<CartItemDTO>> PostItem([FromBody] CartItemToAddDTO cartItemToAddDTO)
    {
        try
        {
            var newCartItem = await _shoppingCartRepository.AddItem(cartItemToAddDTO);

            if (newCartItem == null)
            {
                return NoContent();
            }

            var product = await _productRepository.GetItem(newCartItem.ProductId);

            if (product == null)
            {
                throw new Exception($"Something sent wrong when attempting to retrieve product (productId: ({cartItemToAddDTO.ProductId}))"); /// ta bort en ) ?
            }

            var newCartItemDTO = newCartItem.ConvertToDTO(product);
            //standard practice to return the location of the resourse where the newly added item can be found
            return CreatedAtAction(nameof(GetItem), new {id = newCartItemDTO.Id}, newCartItemDTO);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<CartItemDTO>> DeleteItem(int id)
    {
        try
        {
            var cartItem = await _shoppingCartRepository.DeleteItem(id);

            if (cartItem == null)
            {
                return NotFound();
            }

            var product = await _productRepository.GetItem(cartItem.ProductId);

            if (product == null)
            {
                return NotFound();
            }

            var cartItemDTO = cartItem.ConvertToDTO(product);

            return Ok(cartItemDTO);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPatch("{id:int}")]
    public async Task<ActionResult<CartItemDTO>> UpdateQty(int id, CartItemQtyUpdateDTO cartItemQtyUpdateDTO)
    {
        try
        {
            var cartItem = await _shoppingCartRepository.UpdateQty(id, cartItemQtyUpdateDTO);

            if (cartItem == null)
            {
                return NotFound();
            }

            var product = await _productRepository.GetItem(cartItem.ProductId);
            var cartItemDTO = cartItem.ConvertToDTO(product);
            return Ok(cartItemDTO);
        }

        catch (Exception ex)
        {

            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

        }
    }
}
