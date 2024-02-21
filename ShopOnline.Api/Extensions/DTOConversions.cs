using ShopOnline.Api.Models;
using ShopOnline.Models.DTOs;

namespace ShopOnline.Api.Extensions;

public static class DTOConversions
{
    public static IEnumerable<ProductCategoryDTO> ConvertToDTO(this IEnumerable<ProductCategory> productCategories)
    {
        return (from productCategory in productCategories
                select new ProductCategoryDTO
                {
                    Id = productCategory.Id,
                    Name = productCategory.Name,
                    IconCSS = productCategory.IconCSS
                }).ToList();
    }
    public static IEnumerable<ProductDTO> ConvertToDTO(this IEnumerable<Product> products)
    {
        return (from product in products
                select new ProductDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    ImageURL = product.ImageURL,
                    Price = product.Price,
                    Quantity = product.Quantity,
                    CategoryId = product.ProductCategory.Id,
                    CategoryName = product.ProductCategory.Name,
                }).ToList();
    }

    public static ProductDTO ConvertToDTO(this Product product)
    {
        return new ProductDTO
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            ImageURL = product.ImageURL,
            Price = product.Price,
            Quantity = product.Quantity,
            CategoryId = product.ProductCategory.Id,
            CategoryName = product.ProductCategory.Name
        };
    }
    public static IEnumerable<CartItemDTO> ConvertToDTO(this IEnumerable<CartItem> cartItems, IEnumerable<Product> products)
    {
        return (from cartItem in cartItems
                join product in products
                on cartItem.ProductId equals product.Id
                select new CartItemDTO
                {
                    Id = cartItem.Id,
                    ProductId = cartItem.ProductId,
                    ProductName = product.Name,
                    ProductDescription = product.Description,
                    ProductImageURL = product.ImageURL,
                    Price= product.Price,
                    CartId = cartItem.CartId,
                    Qty = cartItem.Quantity,
                    TotalPrice = product.Price * cartItem.Quantity
                }).ToList();
    }

    public static CartItemDTO ConvertToDTO(this CartItem cartItem, Product product)
    {
        return new CartItemDTO
                {
                    Id = cartItem.Id,
                    ProductId = cartItem.ProductId,
                    ProductName = product.Name,
                    ProductDescription = product.Description,
                    ProductImageURL = product.ImageURL,
                    Price = product.Price,
                    CartId = cartItem.CartId,
                    Qty = cartItem.Quantity,
                    TotalPrice = product.Price * cartItem.Quantity
                };
    }
}
