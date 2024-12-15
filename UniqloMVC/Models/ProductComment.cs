﻿using System.ComponentModel.DataAnnotations;

namespace UniqloMVC.Models
{
    public class ProductComment
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Comment { get; set; }
        public int? ProductId { get; set; }
        public Product? Product { get; set; }
        public string? UserId { get; set; }
        public User? User { get; set; }
    }
}
