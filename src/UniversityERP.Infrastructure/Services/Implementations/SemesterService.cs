using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UniversityERP.Application.Repositories.Abstractions;
using UniversityERP.Domain.Entities;
using UniversityERP.Infrastructure.Dtos;
using UniversityERP.Infrastructure.Dtos.SemesterDtos;
using UniversityERP.Infrastructure.Services.Abstractions;

namespace UniversityERP.Infrastructure.Services.Implementations;

internal class SemesterService : ISemesterService
{
    private readonly IMapper _mapper;
    private readonly ISemesterRepository _semesters;
    private readonly IAcademicYearRepository _academicYears;

    public SemesterService(IMapper mapper, ISemesterRepository semesters, IAcademicYearRepository academicYears)
    {
        _mapper = mapper;
        _semesters = semesters;
        _academicYears = academicYears;
    }

    public async Task<ResultDto> CreateAsync(SemesterCreateDto dto)
    {
        var yearExists = await _academicYears.GetAsync(x => x.Id == dto.AcademicYearId) is not null;
        if (!yearExists)
            return new ResultDto(404, false, "Academic year not found.");

        if (await _semesters.ExistsTermAsync(dto.AcademicYearId, dto.Term, ignoreQueryFilter: true))
            return new ResultDto(409, false, "This term already exists for the selected academic year.");

        var entity = _mapper.Map<Semester>(dto);

        await _semesters.AddAsync(entity);
        await _semesters.SaveChangesAsync();

        return new ResultDto(201, true, "Semester created successfully.");
    }

    public async Task<ResultDto> UpdateAsync(SemesterUpdateDto dto)
    {
        var entity = await _semesters.GetAsync(x => x.Id == dto.Id);
        if (entity is null)
            return new ResultDto(404, false, "Semester not found.");

        var yearExists = await _academicYears.GetAsync(x => x.Id == dto.AcademicYearId) is not null;
        if (!yearExists)
            return new ResultDto(404, false, "Academic year not found.");

        if (await _semesters.ExistsTermAsync(dto.AcademicYearId, dto.Term, dto.Id, true))
            return new ResultDto(409, false, "This term already exists for the selected academic year.");

        _mapper.Map(dto, entity);
        _semesters.Update(entity);
        await _semesters.SaveChangesAsync();

        return new ResultDto(200, true, "Semester updated successfully.");
    }

    public async Task<ResultDto> DeleteAsync(Guid id)
    {
        var entity = await _semesters.GetAsync(x => x.Id == id);
        if (entity is null)
            return new ResultDto(404, false, "Semester not found.");

        _semesters.Delete(entity);
        await _semesters.SaveChangesAsync();

        return new ResultDto(200, true, "Semester deleted successfully.");
    }

    public async Task<ResultDto<List<SemesterGetDto>>> GetAllAsync()
    {
        var list = await _semesters.GetAll()
            .AsNoTracking()
            .OrderByDescending(x => x.StartDate)
            .ToListAsync();

        return new ResultDto<List<SemesterGetDto>>
        {
            StatusCode = 200,
            IsSucced = true,
            Message = "Successfully",
            Data = _mapper.Map<List<SemesterGetDto>>(list)
        };
    }

    public async Task<ResultDto<SemesterGetDto>> GetByIdAsync(Guid id)
    {
        var entity = await _semesters.GetAsync(x => x.Id == id);
        if (entity is null)
        {
            return new ResultDto<SemesterGetDto>
            {
                StatusCode = 404,
                IsSucced = false,
                Message = "Semester not found."
            };
        }

        return new ResultDto<SemesterGetDto>
        {
            StatusCode = 200,
            IsSucced = true,
            Message = "Successfully",
            Data = _mapper.Map<SemesterGetDto>(entity)
        };
    }
}