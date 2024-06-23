namespace dbks.Models
{
    public class EmployeeSalaryViewModel
    {
        public string EmployeeId { get; set; }
        public string Name { get; set; }
        public List<SalaryViewModel> Salaries { get; set; }
    }
}
