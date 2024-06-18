using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace dbks.Models;

public partial class DbksContext : DbContext
{
    public DbksContext()
    {
    }

    public DbksContext(DbContextOptions<DbksContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Administrator> Administrators { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<Salary> Salaries { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("persist security info=True;data source=192.168.1.106;port=3306;initial catalog=dbks;user id=test1;password=123456;character set=utf8;allow zero datetime=true;convert zero datetime=true;pooling=true;maximumpoolsize=3000", Microsoft.EntityFrameworkCore.ServerVersion.Parse("5.7.42-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("latin1_swedish_ci")
            .HasCharSet("latin1");

        modelBuilder.Entity<Administrator>(entity =>
        {
            entity.HasKey(e => e.Administratorid).HasName("PRIMARY");

            entity.ToTable("administrator");

            entity.Property(e => e.Administratorid).HasMaxLength(30);
            entity.Property(e => e.Administratorname).HasMaxLength(30);
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DeptId).HasName("PRIMARY");

            entity.ToTable("department");

            entity.Property(e => e.DeptId)
                .HasMaxLength(20)
                .HasColumnName("DeptID");
            entity.Property(e => e.DeptName).HasMaxLength(20);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PRIMARY");

            entity.ToTable("employee");

            entity.Property(e => e.EmployeeId)
                .HasMaxLength(20)
                .HasColumnName("EmployeeID");
            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.Age).HasColumnType("int(3)");
            entity.Property(e => e.DeptId)
                .HasMaxLength(20)
                .HasColumnName("DeptID");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FamilyInfo).HasColumnType("tinytext");
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.Idnumber)
                .HasMaxLength(20)
                .HasColumnName("IDNumber");
            entity.Property(e => e.Name).HasMaxLength(30);
            entity.Property(e => e.OnJob).HasMaxLength(10);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.PositionId)
                .HasMaxLength(20)
                .HasColumnName("PositionID");
            entity.Property(e => e.Salary).HasMaxLength(20);
            entity.Property(e => e.SalaryId)
                .HasMaxLength(20)
                .HasColumnName("SalaryID");
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasKey(e => e.PositionId).HasName("PRIMARY");

            entity.ToTable("position");

            entity.Property(e => e.PositionId)
                .HasMaxLength(20)
                .HasColumnName("PositionID");
            entity.Property(e => e.PositionName).HasMaxLength(20);
        });

        modelBuilder.Entity<Salary>(entity =>
        {
            entity.HasKey(e => e.SalaryId).HasName("PRIMARY");

            entity.ToTable("salary");

            entity.Property(e => e.SalaryId)
                .HasMaxLength(20)
                .HasColumnName("SalaryID");
            entity.Property(e => e.BasicSalary)
                .HasPrecision(10)
                .HasColumnName("Basic salary");
            entity.Property(e => e.Bonus).HasPrecision(10);
            entity.Property(e => e.EmployeeId)
                .HasMaxLength(20)
                .HasColumnName("EmployeeID");
            entity.Property(e => e.NetSalary)
                .HasPrecision(10)
                .HasColumnName("Net Salary");
            entity.Property(e => e.PayDate).HasColumnType("datetime");
            entity.Property(e => e.PersonalIncome)
                .HasPrecision(10)
                .HasColumnName("Personal income");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
