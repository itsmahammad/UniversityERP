using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversityERP.Domain.Entities;

namespace UniversityERP.Application.Configurations;

public class AttendanceSessionConfiguration : IEntityTypeConfiguration<AttendanceSession>
{
    public void Configure(EntityTypeBuilder<AttendanceSession> b)
    {
        b.HasKey(x => x.Id);

        b.Property(x => x.SessionDate)
            .IsRequired();

        b.Property(x => x.Topic)
            .HasMaxLength(300);

        b.Property(x => x.IsActive)
            .IsRequired();

        b.HasQueryFilter(x => !x.IsDeleted);

        b.HasOne(x => x.CourseOffering)
            .WithMany(x => x.AttendanceSessions)
            .HasForeignKey(x => x.CourseOfferingId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(x => new { x.CourseOfferingId, x.SessionDate })
            .IsUnique()
            .HasFilter("\"IsDeleted\" = false");
    }
}
