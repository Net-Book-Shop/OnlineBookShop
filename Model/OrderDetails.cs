using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineBookShop.Model
{
    public class OrderDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string OrderCode { get; set; }
        public string Status {  get; set; }
        public int Qty {  get; set; }
        public double UnitCostPrice {  get; set; }
        public double UnitSellingPrice {  get; set; }
        public double Total {  get; set; }
        public string BookCode {  get; set; }
        public int IsActive { get; set; }

        [ForeignKey("Order")]
        public Guid OrderID { get; set; }
        public Orders Orders { get; set; }
    }
}
