namespace OnlineBookShop.Dto
{
    public class BookSearchRequestDTO
    {
        public string? CategoryName { get; set; }
        public string? SubCategoryName { get; set; }
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
        public string? BookCode { get; set; }
    }
}
