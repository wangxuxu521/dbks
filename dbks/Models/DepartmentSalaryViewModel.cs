namespace dbks.Models
{
    public class DepartmentSalaryViewModel
    {
        public string DeptID { get; set; }
        public string DeptName { get; set; }
        public List<EmployeeSalaryDetailsViewModel> Employees { get; set; }
    }
}
