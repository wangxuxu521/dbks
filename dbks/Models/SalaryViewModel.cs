namespace dbks.Models
{
    public class SalaryViewModel
    {
        public string SalaryId { get; set; }

        public decimal? SalaryM { get; set; }
        public decimal? BasicSalary { get; set; }
        public decimal? PersonalIncome { get; set; }
        public decimal? Bonus { get; set; }
        public DateOnly PayDate { get; set; }
        public decimal? TaxRate { get; set; }
    }
}
