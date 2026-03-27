using FluentValidation;
using UniversityERP.Infrastructure.Dtos.AttendanceRecordDtos;

namespace UniversityERP.Infrastructure.Validators.AttendanceRecordValidators;

public class AttendanceRecordUpdateDtoValidator : AbstractValidator<AttendanceRecordUpdateDto>
{
    public AttendanceRecordUpdateDtoValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.EnrollmentCourseId).NotEmpty();
        RuleFor(x => x.Note).MaximumLength(500);
    }
}
