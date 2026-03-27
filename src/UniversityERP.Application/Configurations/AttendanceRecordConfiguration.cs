using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversityERP.Domain.Entities;

namespace UniversityERP.Application.Configurations;

public class AttendanceRecordConfiguration : IEntityTypeConfiguration<AttendanceRecord>
{
    public void Configure(EntityTypeBuilder<AttendanceRecord> b)
    {
        b.HasKey(x => x.Id);

        b.Property(x => x.Status)
            .IsRequired();

        b.Property(x => x.Note)
            .HasMaxLength(500);

        b.HasQueryFilter(x => !x.IsDeleted);

        b.HasOne(x => x.AttendanceSession)
            .WithMany(x => x.AttendanceRecords)
            .HasForeignKey(x => x.AttendanceSessionId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.EnrollmentCourse)
            .WithMany(x => x.AttendanceRecords)
            .HasForeignKey(x => x.EnrollmentCourseId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(x => new { x.AttendanceSessionId, x.EnrollmentCourseId })
            .IsUnique()
            .HasFilter("\"IsDeleted\" = false");
    }
}
