using Newtonsoft.Json;
using ShopOnline.Models.DTOs;
using ShopOnline.Web.Services.Contracts;
using System.Net.Http.Json;
using System.Text;

namespace ShopOnline.Web.Services;

public class ShoppingCartService : IShoppingCartService
{
    private readonly HttpClient _httpClient;
    public event Action<int> OnShoppingCartChanged;


    public ShoppingCartService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<CartItemDTO> AddItem(CartItemToAddDTO cartItemToAddDTO)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync<CartItemToAddDTO>("api/ShoppingCart", cartItemToAddDTO);

            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent) 
                { 
                    return default(CartItemDTO);
                }
                return await response.Content.ReadFromJsonAsync<CartItemDTO>();
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Http status code: {response.StatusCode} Message: {message}");
            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<CartItemDTO> DeleteItem(int id)
    {
        try
         {
            var response = await _httpClient.DeleteAsync($"api/ShoppingCart/{id}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CartItemDTO>();
            }
            return default(CartItemDTO);
        }
        catch (Exception)
        {
            //Log exception
            throw;
        }
    }

    public async Task<List<CartItemDTO>> GetItems(int userId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/ShoppingCart/{userId}/GetItems");

            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return Enumerable.Empty<CartItemDTO>().ToList();
                }
                return await response.Content.ReadFromJsonAsync<List<CartItemDTO>>();
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Http status code: {response.StatusCode} Message: {message}");
            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    public void RaiseEventOnShoppingCartChanged(int totalQty)
    {
        if (OnShoppingCartChanged != null)
        {
            OnShoppingCartChanged.Invoke(totalQty);
        }
    }

    public async Task<CartItemDTO> UpdateQty(CartItemQtyUpdateDTO cartItemQtyUpdateDTO)
    {
        try
        {
            var jsonRequest = JsonConvert.SerializeObject(cartItemQtyUpdateDTO);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json-patch+json");

            var response = await _httpClient.PatchAsync($"api/ShoppingCart/{cartItemQtyUpdateDTO.CartItemId}", content);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CartItemDTO>();
            }
            return null;
        }
        catch (Exception)
        {
            // Log exception
            throw;
        }
    }
}
