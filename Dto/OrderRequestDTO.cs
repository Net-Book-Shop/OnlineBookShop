namespace OnlineBookShop.Dto
{
    public class OrderRequestDTO
    {
        public string? OrderCode { get; set; }
        public int? TotalQty { get; set; }
        public string? CustomerName { get; set; }
        public string? Address { get; set; }
        public string? MobileNumber { get; set; }
        public string? CustomerEmail { get; set; }
        public double? DeliveryFee { get; set; }
        public string? PaymentMethod { get; set; }
        public string? BankTransactionId { get; set; }
        public double? Discount { get; set; }
        public double? OrderAmount { get; set; }
        public double? TotalCostPrice { get; set; }
        public List<OrderDetailRequestDTO>? OrderDetails { get; set; }
        public CustomerRequestDTO? Customer { get; set; }
    }
}
