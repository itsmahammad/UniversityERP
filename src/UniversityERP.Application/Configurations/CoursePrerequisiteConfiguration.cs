using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversityERP.Domain.Entities;

namespace UniversityERP.Application.Configurations;

public class CoursePrerequisiteConfiguration : IEntityTypeConfiguration<CoursePrerequisite>
{
    public void Configure(EntityTypeBuilder<CoursePrerequisite> b)
    {
        b.HasKey(x => x.Id);

        b.HasQueryFilter(x => !x.IsDeleted);

        b.HasOne(x => x.AcademicCourse)
            .WithMany(x => x.CoursePrerequisites)
            .HasForeignKey(x => x.AcademicCourseId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.PrerequisiteAcademicCourse)
            .WithMany(x => x.RequiredForCourses)
            .HasForeignKey(x => x.PrerequisiteAcademicCourseId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(x => new { x.AcademicCourseId, x.PrerequisiteAcademicCourseId })
            .IsUnique()
            .HasFilter("\"IsDeleted\" = false");
    }
}
