namespace dbks.Models
{
    public class EmployeeSalaryDetailsViewModel
    {
         public string EmployeeID { get; set; }
        public string Name { get; set; }
        public string PositionID { get; set; }
        public string SalaryID { get; set; }
        public decimal? Salary_M { get; set; }
        public decimal? Basic_salary { get; set; }
        public decimal? Personal_income { get; set; }
        public decimal? Bonus { get; set; }
        public DateOnly PayDate { get; set; }
    }
}
