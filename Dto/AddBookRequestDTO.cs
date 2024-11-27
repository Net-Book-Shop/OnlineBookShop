﻿using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineBookShop.Dto
{
    public class AddBookRequestDTO
    {
        public string? CategoryName { get; set; }
        public string? SubCategoryName { get; set; }
        public string? CategoryCode { get; set; }
        public int? Qty { get; set; }
        public double? CostPrice { get; set; }
        public double? SellingPrice { get; set; }
        public string? BookName { get; set; }
        public string? Description { get; set; }
        public string? Supplier { get; set; }
        public int? IsActive { get; set; }
        public string? ProductImage { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}
