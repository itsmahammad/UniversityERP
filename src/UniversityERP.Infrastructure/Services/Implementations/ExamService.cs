using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UniversityERP.Application.Repositories.Abstractions;
using UniversityERP.Domain.Entities;
using UniversityERP.Infrastructure.Dtos;
using UniversityERP.Infrastructure.Dtos.ExamDtos;
using UniversityERP.Infrastructure.Services.Abstractions;

namespace UniversityERP.Infrastructure.Services.Implementations;

internal class ExamService : IExamService
{
    private readonly IMapper _mapper;
    private readonly IExamRepository _exams;
    private readonly ICourseOfferingRepository _courseOfferings;

    public ExamService(IMapper mapper, IExamRepository exams, ICourseOfferingRepository courseOfferings)
    {
        _mapper = mapper;
        _exams = exams;
        _courseOfferings = courseOfferings;
    }

    public async Task<ResultDto> CreateAsync(ExamCreateDto dto)
    {
        var offering = await _courseOfferings.GetAll()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == dto.CourseOfferingId);

        if (offering is null)
            return new ResultDto(404, false, "Course offering not found.");

        if (!offering.IsActive)
            return new ResultDto(400, false, "Course offering is inactive.");

        if (await _exams.ExistsAsync(dto.CourseOfferingId, dto.ExamType, ignoreQueryFilter: true))
            return new ResultDto(409, false, "This exam type already exists for the selected course offering.");

        var totalWeight = await _exams.GetWeightSumAsync(dto.CourseOfferingId, ignoreQueryFilter: true);
        if (totalWeight + dto.WeightPercentage > 100)
            return new ResultDto(400, false, "Total exam weight cannot exceed 100% for a course offering.");

        var entity = _mapper.Map<Exam>(dto);

        await _exams.AddAsync(entity);
        await _exams.SaveChangesAsync();

        return new ResultDto(201, true, "Exam created successfully.");
    }

    public async Task<ResultDto> UpdateAsync(ExamUpdateDto dto)
    {
        var entity = await _exams.GetAsync(x => x.Id == dto.Id);
        if (entity is null)
            return new ResultDto(404, false, "Exam not found.");

        var offering = await _courseOfferings.GetAll()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == dto.CourseOfferingId);

        if (offering is null)
            return new ResultDto(404, false, "Course offering not found.");

        if (!offering.IsActive)
            return new ResultDto(400, false, "Course offering is inactive.");

        if (await _exams.ExistsAsync(dto.CourseOfferingId, dto.ExamType, dto.Id, true))
            return new ResultDto(409, false, "This exam type already exists for the selected course offering.");

        var totalWeight = await _exams.GetWeightSumAsync(dto.CourseOfferingId, dto.Id, true);
        if (totalWeight + dto.WeightPercentage > 100)
            return new ResultDto(400, false, "Total exam weight cannot exceed 100% for a course offering.");

        _mapper.Map(dto, entity);
        _exams.Update(entity);
        await _exams.SaveChangesAsync();

        return new ResultDto(200, true, "Exam updated successfully.");
    }

    public async Task<ResultDto> DeleteAsync(Guid id)
    {
        var entity = await _exams.GetAsync(x => x.Id == id);
        if (entity is null)
            return new ResultDto(404, false, "Exam not found.");

        _exams.Delete(entity);
        await _exams.SaveChangesAsync();

        return new ResultDto(200, true, "Exam deleted successfully.");
    }

    public async Task<ResultDto<List<ExamGetDto>>> GetAllAsync()
    {
        var list = await _exams.GetAll()
            .AsNoTracking()
            .Include(x => x.CourseOffering)
                .ThenInclude(x => x.AcademicCourse)
            .Include(x => x.CourseOffering)
                .ThenInclude(x => x.Semester)
                    .ThenInclude(x => x.AcademicYear)
            .Include(x => x.CourseOffering)
                .ThenInclude(x => x.Teacher)
                    .ThenInclude(x => x.User)
            .OrderByDescending(x => x.ExamDate)
            .ThenBy(x => x.CourseOffering.AcademicCourse.Code)
            .ThenBy(x => x.ExamType)
            .ToListAsync();

        return new ResultDto<List<ExamGetDto>>
        {
            StatusCode = 200,
            IsSucced = true,
            Message = "Successfully",
            Data = _mapper.Map<List<ExamGetDto>>(list)
        };
    }

    public async Task<ResultDto<ExamGetDto>> GetByIdAsync(Guid id)
    {
        var entity = await _exams.GetAll()
            .AsNoTracking()
            .Include(x => x.CourseOffering)
                .ThenInclude(x => x.AcademicCourse)
            .Include(x => x.CourseOffering)
                .ThenInclude(x => x.Semester)
                    .ThenInclude(x => x.AcademicYear)
            .Include(x => x.CourseOffering)
                .ThenInclude(x => x.Teacher)
                    .ThenInclude(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (entity is null)
        {
            return new ResultDto<ExamGetDto>
            {
                StatusCode = 404,
                IsSucced = false,
                Message = "Exam not found."
            };
        }

        return new ResultDto<ExamGetDto>
        {
            StatusCode = 200,
            IsSucced = true,
            Message = "Successfully",
            Data = _mapper.Map<ExamGetDto>(entity)
        };
    }
}
