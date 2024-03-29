﻿using Microsoft.EntityFrameworkCore;
using ShopOnline.Api.Data;
using ShopOnline.Api.Models;
using ShopOnline.Api.Repositories.Contracts;
using ShopOnline.Models.DTOs;

namespace ShopOnline.Api.Repositories;

public class ShoppingCartRepository : IShoppingCartRepository
{
    private readonly ShopOnlineDbContext _shopOnlineDbContext;

    public ShoppingCartRepository(ShopOnlineDbContext shopOnlineDbContext)
    {
        _shopOnlineDbContext = shopOnlineDbContext;
    }

    private async Task<bool> CartItemExists(int cartId, int productId)
    {
        return await _shopOnlineDbContext.CartItems.AnyAsync(c => c.CartId == cartId && c.ProductId == productId);
    }

    public async Task<CartItem> AddItem(CartItemToAddDTO cartItemToAddDTO)
    {
        if (await CartItemExists(cartItemToAddDTO.CartId, cartItemToAddDTO.ProductId) == false)
        {
            var item = await (from product in _shopOnlineDbContext.Products
                              where product.Id == cartItemToAddDTO.ProductId
                              select new CartItem
                              {
                                  CartId = cartItemToAddDTO.CartId,
                                  ProductId = product.Id,
                                  Quantity = cartItemToAddDTO.Qty
                              }).SingleOrDefaultAsync();

            if (item != null) 
            {
                var result = await _shopOnlineDbContext.CartItems.AddAsync(item);
                await _shopOnlineDbContext.SaveChangesAsync();
                return result.Entity;
            }
        } 
        return null;
    }

    public async Task<CartItem> DeleteItem(int id)
    {
        var item = await _shopOnlineDbContext.CartItems.FindAsync(id);

        if(item != null)
        {
            _shopOnlineDbContext.CartItems.Remove(item);
            await _shopOnlineDbContext.SaveChangesAsync();
        }
        return item;
    }

    public async Task<CartItem> GetItem(int id)
    {
        return await (from cart in _shopOnlineDbContext.Carts
                      join cartItem in _shopOnlineDbContext.CartItems
                      on cart.Id equals cartItem.CartId
                      where cartItem.Id == id
                      select new CartItem
                      {
                          Id = cartItem.Id,
                          ProductId = cartItem.ProductId,
                          Quantity = cartItem.Quantity,
                          CartId = cartItem.CartId
                      }).SingleOrDefaultAsync();
    }

    public async Task<IEnumerable<CartItem>> GetItems(int userId)
    {
        return await (from cart in _shopOnlineDbContext.Carts
                      join cartItem in _shopOnlineDbContext.CartItems
                      on cart.Id equals cartItem.Id
                      where cart.UserId == userId
                      select new CartItem
                      {
                          Id = cartItem.Id,
                          ProductId = cartItem.ProductId,
                          Quantity = cartItem.Quantity,
                          CartId = cartItem.CartId,
                      }).ToListAsync();
    }

    public async Task<CartItem> UpdateQty(int id, CartItemQtyUpdateDTO cartItemQtyUpdateDTO)
    {
        var item = await _shopOnlineDbContext.CartItems.FindAsync(id);
        
        if (item != null)
        {
            item.Quantity = cartItemQtyUpdateDTO.Qty;
            await _shopOnlineDbContext.SaveChangesAsync();
            return item;
        }
        return null;
    }
}
