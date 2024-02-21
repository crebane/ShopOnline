using ShopOnline.Models.DTOs;

namespace ShopOnline.Web.Services.Contracts;

public interface IMangageCartItemsLocalStorageService
{
    Task<List<CartItemDTO>> GetCollection();
    Task SaveCollection(List<CartItemDTO> cartItemDTO);
    Task RemoveCollection();
}
