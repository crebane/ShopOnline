using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ShopOnline.Models.DTOs;
using ShopOnline.Web.Services.Contracts;


namespace ShopOnline.Web.Pages;

public class ShoppingCartBase : ComponentBase
{
	[Inject]
	public IJSRuntime Js {  get; set; }

    [Inject]
    public IShoppingCartService ShoppingCartService { get; set; }

    [Inject]
    public IMangageCartItemsLocalStorageService MangageCartItemsLocalStorageService { get; set; }


    public List<CartItemDTO> ShoppingCartItems { get; set; }
	public string ErrorMessage {  get; set; }

	protected string TotalPrice { get; set; }
	protected int TotalQuantity { get; set; }

    protected override async Task OnInitializedAsync()
    {
		try
		{
			ShoppingCartItems = await MangageCartItemsLocalStorageService.GetCollection();
            CartChanged();
        }
        catch (Exception ex)
		{
			ErrorMessage = ex.Message;
		}
    }

	protected async Task DeleteCartItem_Click(int id)
	{
		var cartItemDTO = await ShoppingCartService.DeleteItem(id);
		await RemoveCartItem(id);
        CartChanged();
    }

    protected async Task UpdateQtyCartItem_Click(int id, int qty)
	{
		try
		{
			if (qty > 0)
			{
				var updateItemDTO = new CartItemQtyUpdateDTO
				{
					CartItemId = id,
					Qty = qty
				};

				var returnedUpdateItemDTO = await ShoppingCartService.UpdateQty(updateItemDTO);
				await UpdateItemTotalPrice(returnedUpdateItemDTO);
                CartChanged();
                await MakeUpdateQtyButtonVisible(id, false);

            }
            else
			{
				var item = ShoppingCartItems.FirstOrDefault(i => i.Id == id);

				if (item != null)
				{
					item.Qty = 1;
					item.TotalPrice = item.Price;
				}
			}
		}
		catch (Exception)
		{

			throw;
		}
	}

	protected async Task UpdateQty_Input(int id)
	{
		await MakeUpdateQtyButtonVisible(id, true);
	}

	private async Task MakeUpdateQtyButtonVisible(int id, bool visible)
	{
        await Js.InvokeVoidAsync("MakeUpdateQtyButtonVisible", id, visible);
    }


    private async Task UpdateItemTotalPrice(CartItemDTO cartItemDTO)
	{
		var item = GetCartItem(cartItemDTO.Id);

		if (item != null)
		{
			item.TotalPrice = cartItemDTO.Price * cartItemDTO.Qty;
		}
		await MangageCartItemsLocalStorageService.SaveCollection(ShoppingCartItems);
	}

	private void CalculateCartSummaryTotals()
	{
		SetTotalPrice();
		SetTotalQuatity();
	}

	private void SetTotalPrice()
	{
		TotalPrice = ShoppingCartItems.Sum(p => p.TotalPrice).ToString("C");
	}

	private void SetTotalQuatity()
	{
		TotalQuantity = ShoppingCartItems.Sum(p => p.Qty);
	}

	private CartItemDTO GetCartItem(int id)
	{
		return ShoppingCartItems.FirstOrDefault(i => i.Id == id);
	}

	private async Task RemoveCartItem(int id)
	{
		var cartItemDTO = GetCartItem(id);
		ShoppingCartItems.Remove(cartItemDTO);

		await MangageCartItemsLocalStorageService.SaveCollection(ShoppingCartItems);
	}

	private void CartChanged()
	{
		CalculateCartSummaryTotals();
		ShoppingCartService.RaiseEventOnShoppingCartChanged(TotalQuantity);
	}
}
