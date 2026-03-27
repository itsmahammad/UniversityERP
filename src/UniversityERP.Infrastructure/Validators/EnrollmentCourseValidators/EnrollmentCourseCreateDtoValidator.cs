using FluentValidation;
using UniversityERP.Infrastructure.Dtos.EnrollmentCourseDtos;

namespace UniversityERP.Infrastructure.Validators.EnrollmentCourseValidators;

public class EnrollmentCourseCreateDtoValidator : AbstractValidator<EnrollmentCourseCreateDto>
{
    public EnrollmentCourseCreateDtoValidator()
    {
        RuleFor(x => x.CourseOfferingId).NotEmpty();
    }
}
