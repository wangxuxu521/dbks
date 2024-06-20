using System;
using System.Collections.Generic;

namespace dbks.Models;

public partial class Position
{
    public string PositionId { get; set; } = null!;

    public string? PositionName { get; set; }

    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
