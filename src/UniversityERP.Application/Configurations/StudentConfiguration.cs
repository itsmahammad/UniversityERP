using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversityERP.Domain.Entities;

namespace UniversityERP.Application.Configurations;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> b)
    {
        b.HasKey(x => x.Id);

        b.Property(x => x.EnrollmentYear)
            .IsRequired();

        b.Property(x => x.Status)
            .IsRequired();

        b.HasQueryFilter(x => !x.IsDeleted);

        b.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.AcademicProgram)
            .WithMany(x => x.Students)
            .HasForeignKey(x => x.AcademicProgramId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(x => x.UserId)
            .IsUnique()
            .HasFilter("\"IsDeleted\" = false");
    }
}