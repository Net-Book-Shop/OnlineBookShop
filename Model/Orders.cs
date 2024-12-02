using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineBookShop.Model
{
    public class Orders
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string OrderCode { get; set; }
        public int TotalQty { get; set; }
        public string CustomerName {  get; set; }
        public string Address { get; set; }
        public string MobileNumber {  get; set; }
        public string CustomerEmail {  get; set; }
        public double DeliveryFee {  get; set; }
        public string PaymentMethod { get; set; }
        public string BankTransactionId { get; set; }
        public double Discount {  get; set; }
        public double OrderAmount {  get; set; }
        public double TotalCostPrice {  get; set; }
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int IsActive { get; set; }

        public ICollection<OrderDetails> OrderDetails { get; set; }

    }
}
