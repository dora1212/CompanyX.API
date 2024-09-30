namespace CompanyX.API.DataAccess.Models
{
    public class EmployeeData
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime EmploymentDate { get; set; }
        public Guid BossId { get; set; }
        public decimal CurrentSalary { get; set; }
        public Guid RoleId { get; set; }
        public string StreetName { get; set; }
        public string HouseNumber { get; set; }
        public string City { get; set; }
        public int PostalCode { get; set; }
    }
}
