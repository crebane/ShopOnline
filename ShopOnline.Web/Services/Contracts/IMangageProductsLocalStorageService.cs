using ShopOnline.Models.DTOs;

namespace ShopOnline.Web.Services.Contracts;

public interface IMangageProductsLocalStorageService
{
    Task<IEnumerable<ProductDTO>> GetCollection();
    Task RemoveCollection();
}
