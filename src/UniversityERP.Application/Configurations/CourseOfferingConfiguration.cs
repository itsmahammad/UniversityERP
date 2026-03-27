using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversityERP.Domain.Entities;

namespace UniversityERP.Application.Configurations;

public class CourseOfferingConfiguration : IEntityTypeConfiguration<CourseOffering>
{
    public void Configure(EntityTypeBuilder<CourseOffering> b)
    {
        b.HasKey(x => x.Id);

        b.Property(x => x.Section)
            .IsRequired()
            .HasMaxLength(20)
            .HasDefaultValue(string.Empty);

        b.Property(x => x.IsActive)
            .IsRequired();

        b.HasQueryFilter(x => !x.IsDeleted);

        b.HasOne(x => x.AcademicCourse)
            .WithMany(x => x.CourseOfferings)
            .HasForeignKey(x => x.AcademicCourseId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.Semester)
            .WithMany(x => x.CourseOfferings)
            .HasForeignKey(x => x.SemesterId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.Teacher)
            .WithMany(x => x.CourseOfferings)
            .HasForeignKey(x => x.TeacherId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(x => new { x.AcademicCourseId, x.SemesterId, x.TeacherId, x.Section })
            .IsUnique()
            .HasFilter("\"IsDeleted\" = false");
    }
}
