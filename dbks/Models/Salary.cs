using System;
using System.Collections.Generic;

namespace dbks.Models;

public partial class Salary
{
    public string SalaryId { get; set; } = null!;

    public string? EmployeeId { get; set; }

    public decimal? NetSalary { get; set; }

    public decimal? BasicSalary { get; set; }

    public decimal? PersonalIncome { get; set; }

    public decimal? Bonus { get; set; }

    public DateTime? PayDate { get; set; }
}
