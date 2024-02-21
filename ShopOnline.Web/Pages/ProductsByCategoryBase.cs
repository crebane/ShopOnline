using Microsoft.AspNetCore.Components;
using ShopOnline.Models.DTOs;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages;

public class ProductsByCategoryBase : ComponentBase
{
    [Parameter]
    public int CategoryId { get; set; }

    [Inject]
    public IProductService? ProductService { get; set; }

    [Inject]
    public IMangageProductsLocalStorageService? MangageProductsLocalStorageService { get; set; }

    public IEnumerable<ProductDTO>? Products { get; set; }
    public string? CategoryName { get; set; }
    public string? ErrorMessage { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        try
        {
            Products = await GetProductCollectionByCategoryId(CategoryId);

            if (Products != null && Products.Count() > 0)
            {
                var productDTO = Products.FirstOrDefault(p => p.CategoryId == CategoryId);
                
                if (productDTO != null)
                {
                    CategoryName = productDTO.CategoryName;
                }
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }

    private async Task<IEnumerable<ProductDTO>> GetProductCollectionByCategoryId(int categoryId)
    {
        var productCollection = await MangageProductsLocalStorageService.GetCollection();

        if (productCollection != null)
        {
            return productCollection.Where(p => p.CategoryId == categoryId);
        }
        else
        {
            return await ProductService.GetItemsByCategory(categoryId);
        }
    }
}
