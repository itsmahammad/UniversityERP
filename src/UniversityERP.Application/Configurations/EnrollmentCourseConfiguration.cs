using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversityERP.Domain.Entities;

namespace UniversityERP.Application.Configurations;

public class EnrollmentCourseConfiguration : IEntityTypeConfiguration<EnrollmentCourse>
{
    public void Configure(EntityTypeBuilder<EnrollmentCourse> b)
    {
        b.HasKey(x => x.Id);

        b.Property(x => x.AttemptNumber)
            .IsRequired();

        b.Property(x => x.CreditsSnapshot)
            .IsRequired();

        b.Property(x => x.Status)
            .IsRequired();

        b.Property(x => x.EnrolledAt)
            .IsRequired();

        b.HasQueryFilter(x => !x.IsDeleted);

        b.HasOne(x => x.StudentSemesterEnrollment)
            .WithMany(x => x.EnrollmentCourses)
            .HasForeignKey(x => x.StudentSemesterEnrollmentId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.CourseOffering)
            .WithMany(x => x.EnrollmentCourses)
            .HasForeignKey(x => x.CourseOfferingId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(x => new { x.StudentSemesterEnrollmentId, x.CourseOfferingId })
            .IsUnique()
            .HasFilter("\"IsDeleted\" = false");
    }
}
