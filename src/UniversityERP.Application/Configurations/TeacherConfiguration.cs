using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversityERP.Domain.Entities;

namespace UniversityERP.Application.Configurations;

public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> b)
    {
        b.HasKey(x => x.Id);

        b.Property(x => x.Title)
            .IsRequired();

        b.Property(x => x.HireDate)
            .IsRequired();

        b.Property(x => x.IsActive)
            .IsRequired();

        b.HasQueryFilter(x => !x.IsDeleted);

        b.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.Department)
            .WithMany(x => x.Teachers)
            .HasForeignKey(x => x.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(x => x.UserId)
            .IsUnique()
            .HasFilter("\"IsDeleted\" = false");
    }
}