using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UniversityERP.Application.Repositories.Abstractions;
using UniversityERP.Domain.Entities;
using UniversityERP.Domain.Enums;
using UniversityERP.Infrastructure.Dtos;
using UniversityERP.Infrastructure.Dtos.TeacherDtos;
using UniversityERP.Infrastructure.Services.Abstractions;

namespace UniversityERP.Infrastructure.Services.Implementations;

internal class TeacherService : ITeacherService
{
    private readonly IMapper _mapper;
    private readonly ITeacherRepository _teachers;
    private readonly IUserRepository _users;
    private readonly IDepartmentRepository _departments;

    public TeacherService(IMapper mapper, ITeacherRepository teachers, IUserRepository users, IDepartmentRepository departments)
    {
        _mapper = mapper;
        _teachers = teachers;
        _users = users;
        _departments = departments;
    }

    public async Task<ResultDto> CreateAsync(TeacherCreateDto dto)
    {
        var user = await _users.GetAsync(x => x.Id == dto.UserId);
        if (user is null)
            return new ResultDto(404, false, "User not found.");

        if (user.Role != UserRole.Teacher)
            return new ResultDto(400, false, "Selected user does not have Teacher role.");

        if (await _teachers.ExistsByUserIdAsync(dto.UserId, ignoreQueryFilter: true))
            return new ResultDto(409, false, "Teacher profile already exists for this user.");

        var departmentExists = await _departments.GetAsync(x => x.Id == dto.DepartmentId) is not null;
        if (!departmentExists)
            return new ResultDto(404, false, "Department not found.");

        var entity = _mapper.Map<Teacher>(dto);

        await _teachers.AddAsync(entity);
        await _teachers.SaveChangesAsync();

        return new ResultDto(201, true, "Teacher created successfully.");
    }

    public async Task<ResultDto> UpdateAsync(TeacherUpdateDto dto)
    {
        var entity = await _teachers.GetAsync(x => x.Id == dto.Id);
        if (entity is null)
            return new ResultDto(404, false, "Teacher not found.");

        var departmentExists = await _departments.GetAsync(x => x.Id == dto.DepartmentId) is not null;
        if (!departmentExists)
            return new ResultDto(404, false, "Department not found.");

        _mapper.Map(dto, entity);
        _teachers.Update(entity);
        await _teachers.SaveChangesAsync();

        return new ResultDto(200, true, "Teacher updated successfully.");
    }

    public async Task<ResultDto> DeleteAsync(Guid id)
    {
        var entity = await _teachers.GetAsync(x => x.Id == id);
        if (entity is null)
            return new ResultDto(404, false, "Teacher not found.");

        _teachers.Delete(entity);
        await _teachers.SaveChangesAsync();

        return new ResultDto(200, true, "Teacher deleted successfully.");
    }

    public async Task<ResultDto<List<TeacherGetDto>>> GetAllAsync()
    {
        var list = await _teachers.GetAll()
            .AsNoTracking()
            .OrderByDescending(x => x.HireDate)
            .ToListAsync();

        return new ResultDto<List<TeacherGetDto>>
        {
            StatusCode = 200,
            IsSucced = true,
            Message = "Successfully",
            Data = _mapper.Map<List<TeacherGetDto>>(list)
        };
    }

    public async Task<ResultDto<TeacherGetDto>> GetByIdAsync(Guid id)
    {
        var entity = await _teachers.GetAsync(x => x.Id == id);
        if (entity is null)
        {
            return new ResultDto<TeacherGetDto>
            {
                StatusCode = 404,
                IsSucced = false,
                Message = "Teacher not found."
            };
        }

        return new ResultDto<TeacherGetDto>
        {
            StatusCode = 200,
            IsSucced = true,
            Message = "Successfully",
            Data = _mapper.Map<TeacherGetDto>(entity)
        };
    }
}