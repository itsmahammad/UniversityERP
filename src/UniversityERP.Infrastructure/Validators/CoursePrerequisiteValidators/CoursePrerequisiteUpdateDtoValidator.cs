using FluentValidation;
using UniversityERP.Infrastructure.Dtos.CoursePrerequisiteDtos;

namespace UniversityERP.Infrastructure.Validators.CoursePrerequisiteValidators;

public class CoursePrerequisiteUpdateDtoValidator : AbstractValidator<CoursePrerequisiteUpdateDto>
{
    public CoursePrerequisiteUpdateDtoValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.AcademicCourseId).NotEmpty();
        RuleFor(x => x.PrerequisiteAcademicCourseId).NotEmpty();
        RuleFor(x => x)
            .Must(x => x.AcademicCourseId != x.PrerequisiteAcademicCourseId)
            .WithMessage("A course cannot be its own prerequisite.");
    }
}
