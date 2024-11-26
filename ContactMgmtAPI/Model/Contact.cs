using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContactMgmtAPI.Model
{
    public class Contact
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[Column(Order = 1, TypeName = "serial")]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
