using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineBookShop.Model
{
    public class Privilege
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string PrivilegeName { get; set; }
        public int IsActive { get; set; }

        public ICollection<PrivilegeDetails> PrivilegeDetails { get; set; }
    }
}
