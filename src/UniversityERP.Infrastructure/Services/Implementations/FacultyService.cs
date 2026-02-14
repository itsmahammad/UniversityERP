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
    private readonly IFacultyRepository _repository;

    public FacultyService(IMapper mapper, IFacultyRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<ResultDto> CreateAsync(FacultyCreateDto dto)
    {
        // Validation (FluentValidation also runs, but keep this safe)
        if (string.IsNullOrWhiteSpace(dto.Name))
            return new ResultDto(400, false, "Name is required.");

        var name = dto.Name.Trim();

        // Check uniqueness (query filter applies, so it checks only not-deleted)
        var exists = await _repository.ExistsByNameAsync(name);
        if (exists)
            return new ResultDto(409, false, "Faculty name already exists.");

        var entity = _mapper.Map<Faculty>(dto);
        entity.Name = name;

        await _repository.AddAsync(entity);
        await _repository.SaveChangesAsync();

        return new ResultDto(201, true, "Faculty created successfully.");
    }

    public async Task<ResultDto<List<FacultyGetDto>>> GetAllAsync()
    {
        var entities = await _repository
            .GetAll()
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync();

        var data = _mapper.Map<List<FacultyGetDto>>(entities);

        return new ResultDto<List<FacultyGetDto>>
        {
            Data = data,
            StatusCode = 200,
            IsSucced = true,
            Message = "Successfully"
        };
    }

    public async Task<ResultDto<FacultyGetDto>> GetByIdAsync(Guid id)
    {
        // IMPORTANT: FindAsync can bypass query filters, so prefer GetAsync with filter-aware query
        var entity = await _repository.GetAsync(x => x.Id == id);
        if (entity is null)
            return new ResultDto<FacultyGetDto>
            {
                Data = null,
                StatusCode = 404,
                IsSucced = false,
                Message = "Faculty not found."
            };

        var data = _mapper.Map<FacultyGetDto>(entity);

        return new ResultDto<FacultyGetDto>
        {
            Data = data,
            StatusCode = 200,
            IsSucced = true,
            Message = "Successfully"
        };
    }

    public async Task<ResultDto> UpdateAsync(Guid id, FacultyUpdateDto dto)
    {
        if (id == Guid.Empty)
            return new ResultDto(400, false, "Invalid id.");

        if (string.IsNullOrWhiteSpace(dto.Name))
            return new ResultDto(400, false, "Name is required.");

        var entity = await _repository.GetAsync(x => x.Id == id);
        if (entity is null)
            return new ResultDto(404, false, "Faculty not found.");

        var name = dto.Name.Trim();

        var exists = await _repository.ExistsByNameAsync(name, excludeId: id);
        if (exists)
            return new ResultDto(409, false, "Faculty name already exists.");

        entity.Name = name;

        _repository.Update(entity);
        await _repository.SaveChangesAsync();

        return new ResultDto(200, true, "Faculty updated successfully.");
    }


    public async Task<ResultDto> DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
            return new ResultDto(400, false, "Invalid id.");

        var entity = await _repository.GetAsync(x => x.Id == id);
        if (entity is null)
            return new ResultDto(404, false, "Faculty not found.");

        // This triggers your SaveChangesInterceptor:
        // Deleted -> IsDeleted=true, UpdatedAt, UpdatedBy, state=Modified
        _repository.Delete(entity);
        await _repository.SaveChangesAsync();

        return new ResultDto(200, true, "Faculty deleted successfully.");
    }
}
