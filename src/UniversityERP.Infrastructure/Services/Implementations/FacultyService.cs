using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UniversityERP.Application.Repositories.Abstractions;
using UniversityERP.Domain.Entities;
using UniversityERP.Infrastructure.Dtos;
using UniversityERP.Infrastructure.Dtos.FacultyDtos;
using UniversityERP.Infrastructure.Services.Abstractions;

namespace UniversityERP.Infrastructure.Services.Implementations;

internal class FacultyService : IFacultyService
{
    private readonly IMapper _mapper;
    private readonly IFacultyRepository _faculties;

    public FacultyService(IMapper mapper, IFacultyRepository faculties)
    {
        _mapper = mapper;
        _faculties = faculties;
    }

    public async Task<ResultDto> CreateAsync(FacultyCreateDto dto)
    {
        if (await _faculties.ExistsByNameAsync(dto.Name, ignoreQueryFilter: true))
            return new ResultDto(409, false, "Faculty name already exists.");

        if (await _faculties.ExistsByCodeAsync(dto.Code, ignoreQueryFilter: true))
            return new ResultDto(409, false, "Faculty code already exists.");

        var entity = _mapper.Map<Faculty>(dto);

        await _faculties.AddAsync(entity);
        await _faculties.SaveChangesAsync();

        return new ResultDto(201, true, "Faculty created successfully.");
    }

    public async Task<ResultDto> UpdateAsync(FacultyUpdateDto dto)
    {
        var entity = await _faculties.GetAsync(x => x.Id == dto.Id);
        if (entity is null)
            return new ResultDto(404, false, "Faculty not found.");

        if (await _faculties.ExistsByNameAsync(dto.Name, dto.Id, ignoreQueryFilter: true))
            return new ResultDto(409, false, "Faculty name already exists.");

        if (await _faculties.ExistsByCodeAsync(dto.Code, dto.Id, ignoreQueryFilter: true))
            return new ResultDto(409, false, "Faculty code already exists.");

        _mapper.Map(dto, entity);
        _faculties.Update(entity);
        await _faculties.SaveChangesAsync();

        return new ResultDto(200, true, "Faculty updated successfully.");
    }

    public async Task<ResultDto> DeleteAsync(Guid id)
    {
        var entity = await _faculties.GetAsync(x => x.Id == id);
        if (entity is null)
            return new ResultDto(404, false, "Faculty not found.");

        _faculties.Delete(entity);
        await _faculties.SaveChangesAsync();

        return new ResultDto(200, true, "Faculty deleted successfully.");
    }

    public async Task<ResultDto<List<FacultyGetDto>>> GetAllAsync()
    {
        var list = await _faculties.GetAll()
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync();

        return new ResultDto<List<FacultyGetDto>>
        {
            StatusCode = 200,
            IsSucced = true,
            Message = "Successfully",
            Data = _mapper.Map<List<FacultyGetDto>>(list)
        };
    }

    public async Task<ResultDto<FacultyGetDto>> GetByIdAsync(Guid id)
    {
        var entity = await _faculties.GetAsync(x => x.Id == id);
        if (entity is null)
        {
            return new ResultDto<FacultyGetDto>
            {
                StatusCode = 404,
                IsSucced = false,
                Message = "Faculty not found."
            };
        }

        return new ResultDto<FacultyGetDto>
        {
            StatusCode = 200,
            IsSucced = true,
            Message = "Successfully",
            Data = _mapper.Map<FacultyGetDto>(entity)
        };
    }
}