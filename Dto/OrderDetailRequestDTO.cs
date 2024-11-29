namespace OnlineBookShop.Dto
{
    public class OrderDetailRequestDTO
    {
        public string? BookCode { get; set; }
        public int? Qty { get; set; }
        public double? UnitCostPrice { get; set; }
        public double? UnitSellingPrice { get; set; }
        public double? Total { get; set; }
    }
}
