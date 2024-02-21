using ShopOnline.Models.DTOs;

namespace ShopOnline.Web.Services.Contracts;

public interface IProductService
{
    Task<IEnumerable<ProductDTO>> GetItems();
    Task<ProductDTO> GetItem(int id);
    Task<IEnumerable<ProductCategoryDTO>> GetProductCategories();
    Task<IEnumerable<ProductDTO>> GetItemsByCategory(int categoryId);
}
