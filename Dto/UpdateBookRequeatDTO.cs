namespace OnlineBookShop.Dto
{
    public class UpdateBookRequeatDTO
    {
        public string? BookCode { get; set; }
        public int? Qty { get; set; }
        public double? CostPrice { get; set; }
        public double? SellingPrice { get; set; }
        public string? BookName { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public string? Supplier { get; set; }

    }
}
