using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UniversityERP.Application.Repositories.Abstractions;
using UniversityERP.Infrastructure.Dtos;
using UniversityERP.Infrastructure.Dtos.AttendanceSessionDtos;
using UniversityERP.Infrastructure.Services.Abstractions;

namespace UniversityERP.Infrastructure.Services.Implementations;

internal class AttendanceSessionService : IAttendanceSessionService
{
    private readonly IMapper _mapper;
    private readonly IAttendanceSessionRepository _attendanceSessions;
    private readonly ICourseOfferingRepository _courseOfferings;

    public AttendanceSessionService(
        IMapper mapper,
        IAttendanceSessionRepository attendanceSessions,
        ICourseOfferingRepository courseOfferings)
    {
        _mapper = mapper;
        _attendanceSessions = attendanceSessions;
        _courseOfferings = courseOfferings;
    }

    public async Task<ResultDto> CreateAsync(AttendanceSessionCreateDto dto)
    {
        var offering = await _courseOfferings.GetAll()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == dto.CourseOfferingId);

        if (offering is null)
            return new ResultDto(404, false, "Course offering not found.");

        if (!offering.IsActive)
            return new ResultDto(400, false, "Course offering is inactive.");

        if (await _attendanceSessions.ExistsAsync(dto.CourseOfferingId, dto.SessionDate, ignoreQueryFilter: true))
            return new ResultDto(409, false, "Attendance session already exists for this course offering and date.");

        var entity = _mapper.Map<Domain.Entities.AttendanceSession>(dto);

        await _attendanceSessions.AddAsync(entity);
        await _attendanceSessions.SaveChangesAsync();

        return new ResultDto(201, true, "Attendance session created successfully.");
    }

    public async Task<ResultDto> UpdateAsync(AttendanceSessionUpdateDto dto)
    {
        var entity = await _attendanceSessions.GetAsync(x => x.Id == dto.Id);
        if (entity is null)
            return new ResultDto(404, false, "Attendance session not found.");

        var offering = await _courseOfferings.GetAll()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == dto.CourseOfferingId);

        if (offering is null)
            return new ResultDto(404, false, "Course offering not found.");

        if (!offering.IsActive)
            return new ResultDto(400, false, "Course offering is inactive.");

        if (await _attendanceSessions.ExistsAsync(dto.CourseOfferingId, dto.SessionDate, dto.Id, true))
            return new ResultDto(409, false, "Attendance session already exists for this course offering and date.");

        _mapper.Map(dto, entity);
        _attendanceSessions.Update(entity);
        await _attendanceSessions.SaveChangesAsync();

        return new ResultDto(200, true, "Attendance session updated successfully.");
    }

    public async Task<ResultDto> DeleteAsync(Guid id)
    {
        var entity = await _attendanceSessions.GetAsync(x => x.Id == id);
        if (entity is null)
            return new ResultDto(404, false, "Attendance session not found.");

        _attendanceSessions.Delete(entity);
        await _attendanceSessions.SaveChangesAsync();

        return new ResultDto(200, true, "Attendance session deleted successfully.");
    }

    public async Task<ResultDto<List<AttendanceSessionGetDto>>> GetAllAsync()
    {
        var list = await _attendanceSessions.GetAll()
            .AsNoTracking()
            .Include(x => x.CourseOffering)
                .ThenInclude(x => x.AcademicCourse)
            .Include(x => x.CourseOffering)
                .ThenInclude(x => x.Teacher)
                    .ThenInclude(x => x.User)
            .Include(x => x.AttendanceRecords)
                .ThenInclude(x => x.EnrollmentCourse)
                    .ThenInclude(x => x.StudentSemesterEnrollment)
                        .ThenInclude(x => x.Student)
                            .ThenInclude(x => x.User)
            .OrderByDescending(x => x.SessionDate)
            .ThenBy(x => x.CourseOffering.AcademicCourse.Code)
            .ToListAsync();

        return new ResultDto<List<AttendanceSessionGetDto>>
        {
            StatusCode = 200,
            IsSucced = true,
            Message = "Successfully",
            Data = _mapper.Map<List<AttendanceSessionGetDto>>(list)
        };
    }

    public async Task<ResultDto<AttendanceSessionGetDto>> GetByIdAsync(Guid id)
    {
        var entity = await _attendanceSessions.GetAll()
            .AsNoTracking()
            .Include(x => x.CourseOffering)
                .ThenInclude(x => x.AcademicCourse)
            .Include(x => x.CourseOffering)
                .ThenInclude(x => x.Teacher)
                    .ThenInclude(x => x.User)
            .Include(x => x.AttendanceRecords)
                .ThenInclude(x => x.EnrollmentCourse)
                    .ThenInclude(x => x.StudentSemesterEnrollment)
                        .ThenInclude(x => x.Student)
                            .ThenInclude(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (entity is null)
        {
            return new ResultDto<AttendanceSessionGetDto>
            {
                StatusCode = 404,
                IsSucced = false,
                Message = "Attendance session not found."
            };
        }

        return new ResultDto<AttendanceSessionGetDto>
        {
            StatusCode = 200,
            IsSucced = true,
            Message = "Successfully",
            Data = _mapper.Map<AttendanceSessionGetDto>(entity)
        };
    }
}
