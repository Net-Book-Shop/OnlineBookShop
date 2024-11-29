namespace OnlineBookShop.Dto
{
    public class ReviewRequestDTO
    {
        public string? Review { get; set; }
        public int? Rating { get; set; }

        public string? BookCode { get; set; }

        public string? CustomerName { get; set; }
        public string? MobileNumber { get; set; }
    }
}
