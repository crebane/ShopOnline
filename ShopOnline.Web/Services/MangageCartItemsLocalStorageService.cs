using Blazored.LocalStorage;
using ShopOnline.Models.DTOs;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Services;

public class MangageCartItemsLocalStorageService : IMangageCartItemsLocalStorageService
{
    private readonly ILocalStorageService _localStorageService;
    private readonly IShoppingCartService _shoppingCartService;

    const string key = "CartItemCollection";

    public MangageCartItemsLocalStorageService(ILocalStorageService localStorageService, IShoppingCartService shoppingCartService)
    {
        _localStorageService = localStorageService;
        _shoppingCartService = shoppingCartService;
    }
    public async Task<List<CartItemDTO>> GetCollection()
    {
        return await _localStorageService.GetItemAsync<List<CartItemDTO>>(key)
                ?? await AddCollection();
    }

    public async Task RemoveCollection()
    {
        await _localStorageService.RemoveItemAsync(key);
    }

    public async Task SaveCollection(List<CartItemDTO> cartItemDtos)
    {
        await _localStorageService.SetItemAsync(key, cartItemDtos);
    }

    private async Task<List<CartItemDTO>> AddCollection()
    {
        var shoppingCartCollection = await _shoppingCartService.GetItems(HardCoded.UserId);

        if (shoppingCartCollection != null)
        {
            await _localStorageService.SetItemAsync(key, shoppingCartCollection);
        }

        return shoppingCartCollection;

    }
}
