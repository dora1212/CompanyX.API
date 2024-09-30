using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace CompanyX.API.DataAccess.Entities
{
    public class MergedCustomerHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [ForeignKey("Customer")]
        public Guid? CustomerId { get; set; }
        public Customer Customer { get; set; }
        public DateTime MergeDate { get; set; }
    }
}
