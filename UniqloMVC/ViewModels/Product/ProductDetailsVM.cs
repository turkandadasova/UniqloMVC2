using System.ComponentModel.DataAnnotations;

namespace UniqloMVC.ViewModels.Product
{
    public class ProductDetailsVM
    {
        [MaxLength(64)]
        public string Name { get; set; } = null!;
        [MaxLength(512)]
        public string Description { get; set; } = null!;
        public IFormFile CoverFile { get; set; } = null!;
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }
        //[DataType("decimal(18,2)")]
        //public decimal CostPrice { get; set; }
        [DataType("decimal(18,2)")]
        public decimal SellPrice { get; set; }
        [Range(0, 100)]
        public int Discount { get; set; }
        public int? CategoryId { get; set; }
    }
}
