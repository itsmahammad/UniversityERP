using FluentValidation;
using UniversityERP.Infrastructure.Dtos.ProgramCourseDtos;

namespace UniversityERP.Infrastructure.Validators.ProgramCourseValidators;

public class ProgramCourseCreateDtoValidator : AbstractValidator<ProgramCourseCreateDto>
{
    public ProgramCourseCreateDtoValidator()
    {
        RuleFor(x => x.AcademicProgramId).NotEmpty();
        RuleFor(x => x.AcademicCourseId).NotEmpty();
        RuleFor(x => x.SemesterNumber).InclusiveBetween(1, 20);
    }
}
