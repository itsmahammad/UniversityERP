using FluentValidation;
using UniversityERP.Infrastructure.Dtos.AttendanceRecordDtos;

namespace UniversityERP.Infrastructure.Validators.AttendanceRecordValidators;

public class AttendanceRecordCreateDtoValidator : AbstractValidator<AttendanceRecordCreateDto>
{
    public AttendanceRecordCreateDtoValidator()
    {
        RuleFor(x => x.EnrollmentCourseId).NotEmpty();
        RuleFor(x => x.Note).MaximumLength(500);
    }
}
