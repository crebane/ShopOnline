using ShopOnline.Models.DTOs;

namespace ShopOnline.Web.Services.Contracts;

public interface IShoppingCartService
{
    Task<List<CartItemDTO>> GetItems(int userId);
    Task<CartItemDTO> AddItem(CartItemToAddDTO cartItemToAddDTO);
    Task<CartItemDTO> DeleteItem(int id);
    Task<CartItemDTO> UpdateQty(CartItemQtyUpdateDTO cartItemQtyUpdateDTO);
    
    event Action<int> OnShoppingCartChanged;
    void RaiseEventOnShoppingCartChanged(int totalQty);
}
