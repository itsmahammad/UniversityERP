using FluentValidation;
using UniversityERP.Infrastructure.Dtos.ProgramCourseDtos;

namespace UniversityERP.Infrastructure.Validators.ProgramCourseValidators;

public class ProgramCourseUpdateDtoValidator : AbstractValidator<ProgramCourseUpdateDto>
{
    public ProgramCourseUpdateDtoValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.AcademicProgramId).NotEmpty();
        RuleFor(x => x.AcademicCourseId).NotEmpty();
        RuleFor(x => x.SemesterNumber).InclusiveBetween(1, 20);
    }
}
