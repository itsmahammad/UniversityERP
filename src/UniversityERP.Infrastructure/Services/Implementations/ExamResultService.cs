using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UniversityERP.Application.Repositories.Abstractions;
using UniversityERP.Domain.Entities;
using UniversityERP.Domain.Enums;
using UniversityERP.Infrastructure.Dtos;
using UniversityERP.Infrastructure.Dtos.ExamResultDtos;
using UniversityERP.Infrastructure.Services.Abstractions;

namespace UniversityERP.Infrastructure.Services.Implementations;

internal class ExamResultService : IExamResultService
{
    private readonly IMapper _mapper;
    private readonly IExamResultRepository _examResults;
    private readonly IEnrollmentCourseRepository _enrollmentCourses;
    private readonly IExamRepository _exams;

    public ExamResultService(
        IMapper mapper,
        IExamResultRepository examResults,
        IEnrollmentCourseRepository enrollmentCourses,
        IExamRepository exams)
    {
        _mapper = mapper;
        _examResults = examResults;
        _enrollmentCourses = enrollmentCourses;
        _exams = exams;
    }

    public async Task<ResultDto> CreateAsync(Guid enrollmentCourseId, ExamResultCreateDto dto)
    {
        var enrollmentCourse = await _enrollmentCourses.GetAll()
            .Include(x => x.CourseOffering)
            .FirstOrDefaultAsync(x => x.Id == enrollmentCourseId);

        if (enrollmentCourse is null)
            return new ResultDto(404, false, "Enrollment course not found.");

        if (enrollmentCourse.Status == EnrollmentCourseStatus.Dropped)
            return new ResultDto(400, false, "Cannot add exam results to a dropped enrollment course.");

        var exam = await _exams.GetAll()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == dto.ExamId);

        if (exam is null)
            return new ResultDto(404, false, "Exam not found.");

        if (!exam.IsActive)
            return new ResultDto(400, false, "Exam is inactive.");

        if (exam.CourseOfferingId != enrollmentCourse.CourseOfferingId)
            return new ResultDto(400, false, "Exam does not belong to the selected enrollment course offering.");

        if (dto.NumericScore > exam.MaxScore)
            return new ResultDto(400, false, "Numeric score cannot exceed exam max score.");

        if (await _examResults.ExistsAsync(enrollmentCourseId, dto.ExamId, ignoreQueryFilter: true))
            return new ResultDto(409, false, "Exam result already exists for this enrollment course and exam.");

        var entity = _mapper.Map<ExamResult>(dto);
        entity.EnrollmentCourseId = enrollmentCourseId;

        await _examResults.AddAsync(entity);
        await _examResults.SaveChangesAsync();

        await RecalculateEnrollmentCourseGradeAsync(enrollmentCourseId);

        return new ResultDto(201, true, "Exam result created successfully.");
    }

    public async Task<ResultDto> UpdateAsync(Guid enrollmentCourseId, ExamResultUpdateDto dto)
    {
        var enrollmentCourse = await _enrollmentCourses.GetAll()
            .Include(x => x.CourseOffering)
            .FirstOrDefaultAsync(x => x.Id == enrollmentCourseId);

        if (enrollmentCourse is null)
            return new ResultDto(404, false, "Enrollment course not found.");

        if (enrollmentCourse.Status == EnrollmentCourseStatus.Dropped)
            return new ResultDto(400, false, "Cannot update exam results for a dropped enrollment course.");

        var entity = await _examResults.GetAsync(x => x.Id == dto.Id && x.EnrollmentCourseId == enrollmentCourseId);
        if (entity is null)
            return new ResultDto(404, false, "Exam result not found.");

        var exam = await _exams.GetAll()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == dto.ExamId);

        if (exam is null)
            return new ResultDto(404, false, "Exam not found.");

        if (!exam.IsActive)
            return new ResultDto(400, false, "Exam is inactive.");

        if (exam.CourseOfferingId != enrollmentCourse.CourseOfferingId)
            return new ResultDto(400, false, "Exam does not belong to the selected enrollment course offering.");

        if (dto.NumericScore > exam.MaxScore)
            return new ResultDto(400, false, "Numeric score cannot exceed exam max score.");

        if (await _examResults.ExistsAsync(enrollmentCourseId, dto.ExamId, dto.Id, true))
            return new ResultDto(409, false, "Exam result already exists for this enrollment course and exam.");

        _mapper.Map(dto, entity);
        _examResults.Update(entity);
        await _examResults.SaveChangesAsync();

        await RecalculateEnrollmentCourseGradeAsync(enrollmentCourseId);

        return new ResultDto(200, true, "Exam result updated successfully.");
    }

    public async Task<ResultDto> DeleteAsync(Guid enrollmentCourseId, Guid id)
    {
        var enrollmentCourse = await _enrollmentCourses.GetAsync(x => x.Id == enrollmentCourseId);
        if (enrollmentCourse is null)
            return new ResultDto(404, false, "Enrollment course not found.");

        var entity = await _examResults.GetAsync(x => x.Id == id && x.EnrollmentCourseId == enrollmentCourseId);
        if (entity is null)
            return new ResultDto(404, false, "Exam result not found.");

        _examResults.Delete(entity);
        await _examResults.SaveChangesAsync();

        await RecalculateEnrollmentCourseGradeAsync(enrollmentCourseId);

        return new ResultDto(200, true, "Exam result deleted successfully.");
    }

    public async Task<ResultDto<List<ExamResultGetDto>>> GetAllAsync(Guid enrollmentCourseId)
    {
        var exists = await _enrollmentCourses.GetAsync(x => x.Id == enrollmentCourseId);
        if (exists is null)
        {
            return new ResultDto<List<ExamResultGetDto>>
            {
                StatusCode = 404,
                IsSucced = false,
                Message = "Enrollment course not found."
            };
        }

        var list = await _examResults.GetAll()
            .AsNoTracking()
            .Where(x => x.EnrollmentCourseId == enrollmentCourseId)
            .Include(x => x.Exam)
            .OrderBy(x => x.Exam.ExamDate)
            .ThenBy(x => x.Exam.ExamType)
            .ToListAsync();

        return new ResultDto<List<ExamResultGetDto>>
        {
            StatusCode = 200,
            IsSucced = true,
            Message = "Successfully",
            Data = _mapper.Map<List<ExamResultGetDto>>(list)
        };
    }

    public async Task<ResultDto<ExamResultGetDto>> GetByIdAsync(Guid enrollmentCourseId, Guid id)
    {
        var entity = await _examResults.GetAll()
            .AsNoTracking()
            .Include(x => x.Exam)
            .FirstOrDefaultAsync(x => x.Id == id && x.EnrollmentCourseId == enrollmentCourseId);

        if (entity is null)
        {
            return new ResultDto<ExamResultGetDto>
            {
                StatusCode = 404,
                IsSucced = false,
                Message = "Exam result not found."
            };
        }

        return new ResultDto<ExamResultGetDto>
        {
            StatusCode = 200,
            IsSucced = true,
            Message = "Successfully",
            Data = _mapper.Map<ExamResultGetDto>(entity)
        };
    }

    private async Task RecalculateEnrollmentCourseGradeAsync(Guid enrollmentCourseId)
    {
        var enrollmentCourse = await _enrollmentCourses.GetAll()
            .Include(x => x.ExamResults)
                .ThenInclude(x => x.Exam)
            .FirstOrDefaultAsync(x => x.Id == enrollmentCourseId);

        if (enrollmentCourse is null || enrollmentCourse.Status == EnrollmentCourseStatus.Dropped)
            return;

        var activeExams = enrollmentCourse.ExamResults
            .Where(x => x.Exam.IsActive)
            .Select(x => x.Exam)
            .ToList();

        var offeringExamCount = await _exams.GetAll()
            .AsNoTracking()
            .CountAsync(x => x.CourseOfferingId == enrollmentCourse.CourseOfferingId && x.IsActive);

        var resultCount = enrollmentCourse.ExamResults.Count(x => x.Exam.IsActive);

        if (offeringExamCount == 0 || resultCount != offeringExamCount)
        {
            enrollmentCourse.FinalNumericScore = null;
            enrollmentCourse.LetterGrade = null;
            enrollmentCourse.GradePoint = null;
            if (enrollmentCourse.Status != EnrollmentCourseStatus.Dropped)
                enrollmentCourse.Status = EnrollmentCourseStatus.Enrolled;

            _enrollmentCourses.Update(enrollmentCourse);
            await _enrollmentCourses.SaveChangesAsync();
            return;
        }

        var totalWeight = activeExams.Sum(x => x.WeightPercentage);
        if (totalWeight != 100m)
        {
            enrollmentCourse.FinalNumericScore = null;
            enrollmentCourse.LetterGrade = null;
            enrollmentCourse.GradePoint = null;
            enrollmentCourse.Status = EnrollmentCourseStatus.Enrolled;

            _enrollmentCourses.Update(enrollmentCourse);
            await _enrollmentCourses.SaveChangesAsync();
            return;
        }

        var finalNumericScore = enrollmentCourse.ExamResults
            .Where(x => x.Exam.IsActive)
            .Sum(x => x.Exam.MaxScore == 0 ? 0 : (x.NumericScore / x.Exam.MaxScore) * x.Exam.WeightPercentage);

        finalNumericScore = Math.Round(finalNumericScore, 2, MidpointRounding.AwayFromZero);

        var (letterGrade, gradePoint) = GradeScaleHelper.ToGrade(finalNumericScore);

        enrollmentCourse.FinalNumericScore = finalNumericScore;
        enrollmentCourse.LetterGrade = letterGrade;
        enrollmentCourse.GradePoint = gradePoint;
        enrollmentCourse.Status = EnrollmentCourseStatus.Completed;

        _enrollmentCourses.Update(enrollmentCourse);
        await _enrollmentCourses.SaveChangesAsync();
    }
}
