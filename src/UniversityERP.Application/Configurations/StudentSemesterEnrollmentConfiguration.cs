using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversityERP.Domain.Entities;

namespace UniversityERP.Application.Configurations;

public class StudentSemesterEnrollmentConfiguration : IEntityTypeConfiguration<StudentSemesterEnrollment>
{
    public void Configure(EntityTypeBuilder<StudentSemesterEnrollment> b)
    {
        b.HasKey(x => x.Id);

        b.Property(x => x.StudentStatus)
            .IsRequired();

        b.Property(x => x.MaxCredits)
            .IsRequired();

        b.Property(x => x.StartingCgpa)
            .HasColumnType("numeric(4,2)");

        b.Property(x => x.Status)
            .IsRequired();

        b.Property(x => x.Notes)
            .HasMaxLength(1000);

        b.HasQueryFilter(x => !x.IsDeleted);

        b.HasOne(x => x.Student)
            .WithMany(x => x.StudentSemesterEnrollments)
            .HasForeignKey(x => x.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.Semester)
            .WithMany(x => x.StudentSemesterEnrollments)
            .HasForeignKey(x => x.SemesterId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.AcademicProgram)
            .WithMany(x => x.StudentSemesterEnrollments)
            .HasForeignKey(x => x.AcademicProgramId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(x => new { x.StudentId, x.SemesterId })
            .IsUnique()
            .HasFilter("\"IsDeleted\" = false");
    }
}
