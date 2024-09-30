using CompanyX.API.DataAccess.Enums;

namespace CompanyX.API.DataAccess.Models
{
    public class EmployeeStatistic
    {
        public string JobTitle { get; set; }
        public int EmployeeCount { get; set; }
        public decimal AverageWage { get; set; }
    }
}
