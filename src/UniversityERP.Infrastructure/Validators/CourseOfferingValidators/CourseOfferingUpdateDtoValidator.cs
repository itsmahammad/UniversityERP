using FluentValidation;
using UniversityERP.Infrastructure.Dtos.CourseOfferingDtos;

namespace UniversityERP.Infrastructure.Validators.CourseOfferingValidators;

public class CourseOfferingUpdateDtoValidator : AbstractValidator<CourseOfferingUpdateDto>
{
    public CourseOfferingUpdateDtoValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.AcademicCourseId).NotEmpty();
        RuleFor(x => x.SemesterId).NotEmpty();
        RuleFor(x => x.TeacherId).NotEmpty();
        RuleFor(x => x.Section)
            .MaximumLength(20);
    }
}
