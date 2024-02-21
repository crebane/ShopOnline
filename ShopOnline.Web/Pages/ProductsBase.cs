using Microsoft.AspNetCore.Components;
using ShopOnline.Models.DTOs;
using ShopOnline.Web.Services.Contracts;
using System.Reflection;

namespace ShopOnline.Web.Pages;

public class ProductsBase : ComponentBase
{
    [Inject]
    public IProductService? ProductService { get; set; }

    [Inject]
    public IShoppingCartService? ShoppingCartService { get; set; }
    
    [Inject]
    public IMangageProductsLocalStorageService? MangageProductsLocalStorageService { get; set; }

    [Inject]
    public IMangageCartItemsLocalStorageService? MangageCartItemsLocalStorageService { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    public string ErrorMessage { get; set; }
    public IEnumerable<ProductDTO>? Products { get; set; }


    protected override async Task OnInitializedAsync()
    {
        await ClearLocalStorage();

        Products = await MangageProductsLocalStorageService.GetCollection();

        var shoppingCartItems = await MangageCartItemsLocalStorageService.GetCollection();
        var totalQty = shoppingCartItems.Sum(i => i.Qty);

        ShoppingCartService.RaiseEventOnShoppingCartChanged(totalQty);
    }

    protected IOrderedEnumerable<IGrouping<int, ProductDTO>> GetGroupedProductsByCategory()
    {
        return from product in Products
               group product by product.CategoryId into prodByCatGroup
               orderby prodByCatGroup.Key
               select prodByCatGroup;
    }

    protected string GetCategoryName(IGrouping<int, ProductDTO> groupedProductDTOs)
    {
        return groupedProductDTOs.FirstOrDefault(pg => pg.CategoryId == groupedProductDTOs.Key).CategoryName;
    }

    private async Task ClearLocalStorage()
    {
        await MangageProductsLocalStorageService.RemoveCollection();
        await MangageCartItemsLocalStorageService.RemoveCollection();
    }
}
