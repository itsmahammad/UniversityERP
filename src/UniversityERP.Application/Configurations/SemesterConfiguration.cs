using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversityERP.Domain.Entities;

namespace UniversityERP.Application.Configurations;

public class SemesterConfiguration : IEntityTypeConfiguration<Semester>
{
    public void Configure(EntityTypeBuilder<Semester> b)
    {
        b.HasKey(x => x.Id);

        b.Property(x => x.MaxCredits)
            .IsRequired();

        b.Property(x => x.IsActive)
            .IsRequired();

        b.HasQueryFilter(x => !x.IsDeleted);

        b.HasOne(x => x.AcademicYear)
            .WithMany(x => x.Semesters)
            .HasForeignKey(x => x.AcademicYearId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(x => new { x.AcademicYearId, x.Term })
            .IsUnique()
            .HasFilter("\"IsDeleted\" = false");
    }
}