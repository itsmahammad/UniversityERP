using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversityERP.Domain.Entities;

namespace UniversityERP.Application.Configurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> b)
    {
        b.HasKey(x => x.Id);

        b.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        b.Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(30);

        b.Property(x => x.IsActive)
            .IsRequired();

        b.HasQueryFilter(x => !x.IsDeleted);

        b.HasMany(x => x.AcademicPrograms)
            .WithOne(x => x.Department)
            .HasForeignKey(x => x.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(x => new { x.FacultyId, x.Name })
            .IsUnique()
            .HasFilter("\"IsDeleted\" = false");

        b.HasIndex(x => new { x.FacultyId, x.Code })
            .IsUnique()
            .HasFilter("\"IsDeleted\" = false");
    }
}