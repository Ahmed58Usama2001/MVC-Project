using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.DAL.Data.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.Property(E => E.Salary).HasColumnType("decimal(18,2)");

        builder.Property(E => E.Name)
            .IsRequired(true)
            .HasMaxLength(50);
    }
}
