using System;
using System.Collections.Generic;

namespace dbks.Models;

public partial class Salary
{
    public string SalaryId { get; set; } = null!;

    public decimal? Salary1 { get; set; }

    public decimal? BasicSalary { get; set; }

    public decimal? PersonalIncome { get; set; }

    public decimal? Bonus { get; set; }

    public DateTime? PayDate { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
