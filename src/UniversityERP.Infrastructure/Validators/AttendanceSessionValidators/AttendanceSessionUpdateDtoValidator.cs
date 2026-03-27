using FluentValidation;
using UniversityERP.Infrastructure.Dtos.AttendanceSessionDtos;

namespace UniversityERP.Infrastructure.Validators.AttendanceSessionValidators;

public class AttendanceSessionUpdateDtoValidator : AbstractValidator<AttendanceSessionUpdateDto>
{
    public AttendanceSessionUpdateDtoValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.CourseOfferingId).NotEmpty();
        RuleFor(x => x.Topic).MaximumLength(300);
    }
}
