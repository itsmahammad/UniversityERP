using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UniversityERP.Application.Repositories.Abstractions;
using UniversityERP.Domain.Entities;
using UniversityERP.Infrastructure.Dtos;
using UniversityERP.Infrastructure.Dtos.ProgramCourseDtos;
using UniversityERP.Infrastructure.Services.Abstractions;

namespace UniversityERP.Infrastructure.Services.Implementations;

internal class ProgramCourseService : IProgramCourseService
{
    private readonly IMapper _mapper;
    private readonly IProgramCourseRepository _programCourses;
    private readonly IAcademicProgramRepository _programs;
    private readonly IAcademicCourseRepository _courses;

    public ProgramCourseService(
        IMapper mapper,
        IProgramCourseRepository programCourses,
        IAcademicProgramRepository programs,
        IAcademicCourseRepository courses)
    {
        _mapper = mapper;
        _programCourses = programCourses;
        _programs = programs;
        _courses = courses;
    }

    public async Task<ResultDto> CreateAsync(ProgramCourseCreateDto dto)
    {
        var programExists = await _programs.GetAsync(x => x.Id == dto.AcademicProgramId) is not null;
        if (!programExists)
            return new ResultDto(404, false, "Academic program not found.");

        var courseExists = await _courses.GetAsync(x => x.Id == dto.AcademicCourseId) is not null;
        if (!courseExists)
            return new ResultDto(404, false, "Academic course not found.");

        if (await _programCourses.ExistsAsync(dto.AcademicProgramId, dto.AcademicCourseId, ignoreQueryFilter: true))
            return new ResultDto(409, false, "This course already exists in the selected academic program.");

        var entity = _mapper.Map<ProgramCourse>(dto);

        await _programCourses.AddAsync(entity);
        await _programCourses.SaveChangesAsync();

        return new ResultDto(201, true, "Program course created successfully.");
    }

    public async Task<ResultDto> UpdateAsync(ProgramCourseUpdateDto dto)
    {
        var entity = await _programCourses.GetAsync(x => x.Id == dto.Id);
        if (entity is null)
            return new ResultDto(404, false, "Program course not found.");

        var programExists = await _programs.GetAsync(x => x.Id == dto.AcademicProgramId) is not null;
        if (!programExists)
            return new ResultDto(404, false, "Academic program not found.");

        var courseExists = await _courses.GetAsync(x => x.Id == dto.AcademicCourseId) is not null;
        if (!courseExists)
            return new ResultDto(404, false, "Academic course not found.");

        if (await _programCourses.ExistsAsync(dto.AcademicProgramId, dto.AcademicCourseId, dto.Id, true))
            return new ResultDto(409, false, "This course already exists in the selected academic program.");

        _mapper.Map(dto, entity);
        _programCourses.Update(entity);
        await _programCourses.SaveChangesAsync();

        return new ResultDto(200, true, "Program course updated successfully.");
    }

    public async Task<ResultDto> DeleteAsync(Guid id)
    {
        var entity = await _programCourses.GetAsync(x => x.Id == id);
        if (entity is null)
            return new ResultDto(404, false, "Program course not found.");

        _programCourses.Delete(entity);
        await _programCourses.SaveChangesAsync();

        return new ResultDto(200, true, "Program course deleted successfully.");
    }

    public async Task<ResultDto<List<ProgramCourseGetDto>>> GetAllAsync()
    {
        var list = await _programCourses.GetAll()
            .AsNoTracking()
            .Include(x => x.AcademicProgram)
            .Include(x => x.AcademicCourse)
            .OrderBy(x => x.AcademicProgram.Name)
            .ThenBy(x => x.SemesterNumber)
            .ThenBy(x => x.AcademicCourse.Code)
            .ToListAsync();

        return new ResultDto<List<ProgramCourseGetDto>>
        {
            StatusCode = 200,
            IsSucced = true,
            Message = "Successfully",
            Data = _mapper.Map<List<ProgramCourseGetDto>>(list)
        };
    }

    public async Task<ResultDto<ProgramCourseGetDto>> GetByIdAsync(Guid id)
    {
        var entity = await _programCourses.GetAll()
            .AsNoTracking()
            .Include(x => x.AcademicProgram)
            .Include(x => x.AcademicCourse)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (entity is null)
        {
            return new ResultDto<ProgramCourseGetDto>
            {
                StatusCode = 404,
                IsSucced = false,
                Message = "Program course not found."
            };
        }

        return new ResultDto<ProgramCourseGetDto>
        {
            StatusCode = 200,
            IsSucced = true,
            Message = "Successfully",
            Data = _mapper.Map<ProgramCourseGetDto>(entity)
        };
    }
}
