using ShopOnline.Api.Models;
using ShopOnline.Models.DTOs;

namespace ShopOnline.Api.Repositories.Contracts;

public interface IShoppingCartRepository
{
    Task<CartItem> AddItem(CartItemToAddDTO cartItemToAddDTO);
    Task<CartItem> UpdateQty(int id, CartItemQtyUpdateDTO cartItemQtyUpdateDTO);
    Task<CartItem> DeleteItem(int id);
    Task<CartItem> GetItem(int id);
    Task<IEnumerable<CartItem>> GetItems(int userId);

}
