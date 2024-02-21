using Microsoft.AspNetCore.Components;
using ShopOnline.Models.DTOs;
using ShopOnline.Web.Services;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages;

public class ProductDetailsBase : ComponentBase
{
    [Parameter]
    public int Id { get; set; }

    [Inject]
    public IProductService ProductService { get; set; }

    [Inject]
    public IShoppingCartService ShoppingCartService { get; set; }

    [Inject]
    public IMangageProductsLocalStorageService MangageProductsLocalStorageService { get; set; }

    [Inject]
    public IMangageCartItemsLocalStorageService MangageCartItemsLocalStorageService { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    public ProductDTO Product { get; set; }
    public string ErrorMessage { get; set; }
    private List<CartItemDTO> ShoppingCartItems { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            ShoppingCartItems = await MangageCartItemsLocalStorageService.GetCollection();
            Product = await GetProductById(Id);
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }
    
    protected async Task AddToCart_Click(CartItemToAddDTO cartItemToAddDTO)
    {
        try
        {
            var cartItemDTO = await ShoppingCartService.AddItem(cartItemToAddDTO);

            if (cartItemDTO != null)
            {
                ShoppingCartItems.Add(cartItemDTO);
                await MangageCartItemsLocalStorageService.SaveCollection(ShoppingCartItems);
            }

            NavigationManager.NavigateTo("/ShoppingCart");
        }
        catch (Exception)
        {
            //Log exeption
            throw;
        }
    }

    private async Task<ProductDTO> GetProductById(int id)
    {
        var productDTOs = await MangageProductsLocalStorageService.GetCollection();

        if (productDTOs != null)
        {
            return productDTOs.SingleOrDefault(p => p.Id == id);
        }
        return null;
    }
}
