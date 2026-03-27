using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversityERP.Domain.Entities;

namespace UniversityERP.Application.Configurations;

public class ExamConfiguration : IEntityTypeConfiguration<Exam>
{
    public void Configure(EntityTypeBuilder<Exam> b)
    {
        b.HasKey(x => x.Id);

        b.Property(x => x.ExamType)
            .IsRequired();

        b.Property(x => x.ExamDate)
            .IsRequired();

        b.Property(x => x.MaxScore)
            .HasColumnType("numeric(5,2)")
            .IsRequired();

        b.Property(x => x.WeightPercentage)
            .HasColumnType("numeric(5,2)")
            .IsRequired();

        b.Property(x => x.IsActive)
            .IsRequired();

        b.HasQueryFilter(x => !x.IsDeleted);

        b.HasOne(x => x.CourseOffering)
            .WithMany(x => x.Exams)
            .HasForeignKey(x => x.CourseOfferingId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(x => new { x.CourseOfferingId, x.ExamType })
            .IsUnique()
            .HasFilter("\"IsDeleted\" = false");
    }
}
