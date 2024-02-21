using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.Models.DTOs;

public class ProductDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? ImageURL { get; set; }
    public int Quantity { get; set; }
    public int CategoryId { get; set; }
    public string? CategoryName { get; set; }
}
