using CompanyX.API.DataAccess.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CompanyX.API.DataAccess.Entities
{
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public JobTitle Name { get; set; }

        public List<Employee> Employees { get; set; }

    }
}
