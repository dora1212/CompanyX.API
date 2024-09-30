using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CompanyX.API.DataAccess.Entities
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }

        public CustomerHomeAddress HomeAddress { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string? CardNumber { get; set; }

        [DefaultValue(false)]
        public bool IsUpdated { get; set; }

    }
}
