using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversityERP.Domain.Entities;

namespace UniversityERP.Application.Configurations;

public class ExamResultConfiguration : IEntityTypeConfiguration<ExamResult>
{
    public void Configure(EntityTypeBuilder<ExamResult> b)
    {
        b.HasKey(x => x.Id);

        b.Property(x => x.NumericScore)
            .HasColumnType("numeric(5,2)")
            .IsRequired();

        b.HasQueryFilter(x => !x.IsDeleted);

        b.HasOne(x => x.EnrollmentCourse)
            .WithMany(x => x.ExamResults)
            .HasForeignKey(x => x.EnrollmentCourseId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.Exam)
            .WithMany(x => x.ExamResults)
            .HasForeignKey(x => x.ExamId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(x => new { x.EnrollmentCourseId, x.ExamId })
            .IsUnique()
            .HasFilter("\"IsDeleted\" = false");
    }
}
