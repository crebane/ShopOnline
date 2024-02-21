using Microsoft.AspNetCore.Components;
using ShopOnline.Models.DTOs;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Layout;

public class ProductCategoriesNavMenuBase : ComponentBase
{
    [Inject]
    public IProductService ProductService { get; set; }
    public IEnumerable<ProductCategoryDTO> ProductCategoryDTO { get; set; }
    public string ErrorMessage { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            ProductCategoryDTO = await ProductService.GetProductCategories();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }
}
