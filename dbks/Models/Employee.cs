using System;
using System.Collections.Generic;

namespace dbks.Models;

public partial class Employee
{
    public string EmployeeId { get; set; } = null!;

    public string? Idnumber { get; set; }

    public string? Name { get; set; }

    public int? Age { get; set; }

    public string? Gender { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public string? FamilyInfo { get; set; }

    public string? DeptId { get; set; }

    public string? PositionId { get; set; }

    public string? SalaryId { get; set; }

    public string? OnJob { get; set; }

    public virtual Department? Dept { get; set; }

    public virtual Position? Position { get; set; }

    public virtual Salary? Salary { get; set; }
}
