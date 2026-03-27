using FluentValidation;
using UniversityERP.Infrastructure.Dtos.AttendanceSessionDtos;

namespace UniversityERP.Infrastructure.Validators.AttendanceSessionValidators;

public class AttendanceSessionCreateDtoValidator : AbstractValidator<AttendanceSessionCreateDto>
{
    public AttendanceSessionCreateDtoValidator()
    {
        RuleFor(x => x.CourseOfferingId).NotEmpty();
        RuleFor(x => x.Topic).MaximumLength(300);
    }
}
