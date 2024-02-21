using Blazored.LocalStorage;
using ShopOnline.Models.DTOs;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Services
{
    public class MangageProductsLocalStorageService : IMangageProductsLocalStorageService
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly IProductService _productService;

        private const string key = "ProductCollection";

        public MangageProductsLocalStorageService(ILocalStorageService localStorageService, IProductService productService)
        {
            _localStorageService = localStorageService;
            _productService = productService;
        }
        public async Task<IEnumerable<ProductDTO>> GetCollection()
        {
            return await _localStorageService.GetItemAsync<IEnumerable<ProductDTO>>(key) 
                ?? await AddCollection();
        }

        public async Task RemoveCollection()
        {
            await _localStorageService.RemoveItemAsync(key);
        }

        private async Task<IEnumerable<ProductDTO>> AddCollection()
        {
            var productCollection = await _productService.GetItems();

            if (productCollection != null)
            {
                await _localStorageService.SetItemAsync(key, productCollection);
            }
            return productCollection;
        }
    }
}
