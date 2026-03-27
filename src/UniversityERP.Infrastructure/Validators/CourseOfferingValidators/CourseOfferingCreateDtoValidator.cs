using FluentValidation;
using UniversityERP.Infrastructure.Dtos.CourseOfferingDtos;

namespace UniversityERP.Infrastructure.Validators.CourseOfferingValidators;

public class CourseOfferingCreateDtoValidator : AbstractValidator<CourseOfferingCreateDto>
{
    public CourseOfferingCreateDtoValidator()
    {
        RuleFor(x => x.AcademicCourseId).NotEmpty();
        RuleFor(x => x.SemesterId).NotEmpty();
        RuleFor(x => x.TeacherId).NotEmpty();
        RuleFor(x => x.Section)
            .MaximumLength(20);
    }
}
