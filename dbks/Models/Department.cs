﻿using System;
using System.Collections.Generic;

namespace dbks.Models;

public partial class Department
{
    public string DeptId { get; set; } = null!;

    public string? DeptName { get; set; }

    public string PositionId { get; set; } = null!;

    public string? PositionName { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
