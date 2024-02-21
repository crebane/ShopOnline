using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ShopOnline.Models.DTOs;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages;

public class CheckoutBase : ComponentBase
{
    [Inject]
    public IJSRuntime Js {  get; set; }

    [Inject]
    public IShoppingCartService? ShoppingCartService { get; set; }

    [Inject]
    public IMangageCartItemsLocalStorageService MangageCartItemsLocalStorageService { get; set; }
    protected IEnumerable<CartItemDTO>? ShoppingCartItems { get; set; }
    protected int TotalQty { get; set; }
    protected string? PaymentDescription { get; set; }
    protected decimal PaymentAmount { get; set; }

    protected string DisplayButtons { get; set; } = "block";


    protected override async Task OnInitializedAsync()
    {
        try
        {
            ShoppingCartItems = await MangageCartItemsLocalStorageService.GetCollection();

            if (ShoppingCartItems != null && ShoppingCartItems.Count() > 0)
            {
                Guid orderGuid = Guid.NewGuid();

                PaymentAmount = ShoppingCartItems.Sum(p => p.TotalPrice);
                TotalQty = ShoppingCartItems.Sum(p => p.Qty);
                PaymentDescription = $"O_{HardCoded.UserId}_{orderGuid}";
            }
            else
            {
                DisplayButtons = "none";
            }
        }
        catch (Exception)
        {
            //Log exception
            throw;
        }
    }

    protected override async void OnAfterRender(bool firstRender)
    {
        try
        {
            if (firstRender)
            {
                await Js.InvokeVoidAsync("initPayPalButton");
            }
        }
        catch (Exception)
        {

            throw;
        }
    }
}
