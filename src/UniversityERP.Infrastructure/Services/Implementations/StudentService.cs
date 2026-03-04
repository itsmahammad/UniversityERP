using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UniversityERP.Application.Repositories.Abstractions;
using UniversityERP.Domain.Entities;
using UniversityERP.Domain.Enums;
using UniversityERP.Infrastructure.Dtos;
using UniversityERP.Infrastructure.Dtos.StudentDtos;
using UniversityERP.Infrastructure.Services.Abstractions;

namespace UniversityERP.Infrastructure.Services.Implementations;

internal class StudentService : IStudentService
{
    private readonly IMapper _mapper;
    private readonly IStudentRepository _students;
    private readonly IUserRepository _users;
    private readonly IAcademicProgramRepository _programs;

    public StudentService(IMapper mapper, IStudentRepository students, IUserRepository users, IAcademicProgramRepository programs)
    {
        _mapper = mapper;
        _students = students;
        _users = users;
        _programs = programs;
    }

    public async Task<ResultDto> CreateAsync(StudentCreateDto dto)
    {
        var user = await _users.GetAsync(x => x.Id == dto.UserId);
        if (user is null)
            return new ResultDto(404, false, "User not found.");

        if (user.Role != UserRole.Student)
            return new ResultDto(400, false, "Selected user does not have Student role.");

        if (await _students.ExistsByUserIdAsync(dto.UserId, ignoreQueryFilter: true))
            return new ResultDto(409, false, "Student profile already exists for this user.");

        var programExists = await _programs.GetAsync(x => x.Id == dto.AcademicProgramId) is not null;
        if (!programExists)
            return new ResultDto(404, false, "Academic program not found.");

        var entity = _mapper.Map<Student>(dto);

        await _students.AddAsync(entity);
        await _students.SaveChangesAsync();

        return new ResultDto(201, true, "Student created successfully.");
    }

    public async Task<ResultDto> UpdateAsync(StudentUpdateDto dto)
    {
        var entity = await _students.GetAsync(x => x.Id == dto.Id);
        if (entity is null)
            return new ResultDto(404, false, "Student not found.");

        var programExists = await _programs.GetAsync(x => x.Id == dto.AcademicProgramId) is not null;
        if (!programExists)
            return new ResultDto(404, false, "Academic program not found.");

        _mapper.Map(dto, entity);
        _students.Update(entity);
        await _students.SaveChangesAsync();

        return new ResultDto(200, true, "Student updated successfully.");
    }

    public async Task<ResultDto> DeleteAsync(Guid id)
    {
        var entity = await _students.GetAsync(x => x.Id == id);
        if (entity is null)
            return new ResultDto(404, false, "Student not found.");

        _students.Delete(entity);
        await _students.SaveChangesAsync();

        return new ResultDto(200, true, "Student deleted successfully.");
    }

    public async Task<ResultDto<List<StudentGetDto>>> GetAllAsync()
    {
        var list = await _students.GetAll()
            .AsNoTracking()
            .OrderByDescending(x => x.EnrollmentYear)
            .ToListAsync();

        return new ResultDto<List<StudentGetDto>>
        {
            StatusCode = 200,
            IsSucced = true,
            Message = "Successfully",
            Data = _mapper.Map<List<StudentGetDto>>(list)
        };
    }

    public async Task<ResultDto<StudentGetDto>> GetByIdAsync(Guid id)
    {
        var entity = await _students.GetAsync(x => x.Id == id);
        if (entity is null)
        {
            return new ResultDto<StudentGetDto>
            {
                StatusCode = 404,
                IsSucced = false,
                Message = "Student not found."
            };
        }

        return new ResultDto<StudentGetDto>
        {
            StatusCode = 200,
            IsSucced = true,
            Message = "Successfully",
            Data = _mapper.Map<StudentGetDto>(entity)
        };
    }
}