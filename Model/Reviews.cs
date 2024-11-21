using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineBookShop.Model
{
    public class Reviews
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Review { get; set; }
        public int Rating {  get; set; }

        public string BookCode { get; set; }

        public string CustomerName {  get; set; }
        public string MobileNumber {  get; set; }


    }
}
