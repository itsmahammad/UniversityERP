using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversityERP.Domain.Entities;

namespace UniversityERP.Application.Configurations;

public class AcademicCourseConfiguration : IEntityTypeConfiguration<AcademicCourse>
{
    public void Configure(EntityTypeBuilder<AcademicCourse> b)
    {
        b.HasKey(x => x.Id);

        b.Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(30);

        b.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        b.Property(x => x.Description)
            .HasMaxLength(2000);

        b.Property(x => x.EctsCredits)
            .IsRequired();

        b.Property(x => x.IsActive)
            .IsRequired();

        b.HasQueryFilter(x => !x.IsDeleted);

        b.HasOne(x => x.OwningDepartment)
            .WithMany(x => x.OwningCourses)
            .HasForeignKey(x => x.OwningDepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(x => x.Code)
            .IsUnique()
            .HasFilter("\"IsDeleted\" = false");
    }
}