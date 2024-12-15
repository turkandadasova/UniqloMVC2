using System.ComponentModel.DataAnnotations;
using System.Drawing.Drawing2D;
using UniqloMVC.ViewModels.Product;

namespace UniqloMVC.Models
{
    public class Product:BaseEntity
    {
        [MaxLength(64)]
        public string Name { get; set; } = null!;
        [MaxLength(512)]
        public string Description { get; set; } = null!;
        public string CoverImage { get; set; } = null!;
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }
        [DataType("decimal(18,2)")]
        public decimal CostPrice { get; set; }
        [DataType("decimal(18,2)")]
        public decimal SellPrice { get; set; }
        [Range(0, 100)]

        public int Discount { get; set; }
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
        public ICollection<Tag> Tags { get; set; }
        public ICollection<Comment> Comments { get; set; }

        public ICollection<ProductRating>? Ratings { get; set; }

        public static implicit operator Product(ProductCreateVM vm)
        {
            return new Product
            {
                CategoryId = vm.CategoryId,
                Name = vm.Name,
                Description = vm.Description,
                CostPrice = vm.CostPrice,
                SellPrice = vm.SellPrice,
                Discount = vm.Discount,
                Quantity = vm.Quantity,
            };
        }

        
    }
}
        