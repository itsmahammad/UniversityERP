using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversityERP.Domain.Entities;

namespace UniversityERP.Application.Configurations;

public class ProgramCourseConfiguration : IEntityTypeConfiguration<ProgramCourse>
{
    public void Configure(EntityTypeBuilder<ProgramCourse> b)
    {
        b.HasKey(x => x.Id);

        b.Property(x => x.SemesterNumber)
            .IsRequired();

        b.Property(x => x.IsCore)
            .IsRequired();

        b.HasQueryFilter(x => !x.IsDeleted);

        b.HasOne(x => x.AcademicProgram)
            .WithMany(x => x.ProgramCourses)
            .HasForeignKey(x => x.AcademicProgramId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.AcademicCourse)
            .WithMany(x => x.ProgramCourses)
            .HasForeignKey(x => x.AcademicCourseId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(x => new { x.AcademicProgramId, x.AcademicCourseId })
            .IsUnique()
            .HasFilter("\"IsDeleted\" = false");
    }
}
