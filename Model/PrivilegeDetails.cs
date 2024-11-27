using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineBookShop.Model
{
    public class PrivilegeDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [ForeignKey("Role")]
        public Guid RoleId { get; set; }

        [ForeignKey("Privilege")]
        public Guid PrivilegeId { get; set; }

        public int IsActive { get; set; }

        // Navigation properties
        public Roles Role { get; set; }
        public Privilege Privilege { get; set; }
    }
}
