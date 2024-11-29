using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineBookShop.Model
{
    public class Books
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string BookCode { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public string CategoryCode { get; set; }
        public int Qty { get; set; }
        public double CostPrice { get; set; }
        public double SellingPrice { get; set; }
        public string BookName { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Supplier { get; set; }
        public int rating {  get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public string? ProductImage { get; set; }
        public int IsActive { get; set; }


    }
}
