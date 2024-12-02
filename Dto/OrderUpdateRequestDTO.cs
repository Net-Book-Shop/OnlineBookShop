namespace OnlineBookShop.Dto
{
    public class OrderUpdateRequestDTO
    {
        public string? OrderCode { get; set; }
        public string? Status { get; set; }
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
    }
}
