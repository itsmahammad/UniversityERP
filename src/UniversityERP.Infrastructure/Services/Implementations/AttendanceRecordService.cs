using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UniversityERP.Application.Repositories.Abstractions;
using UniversityERP.Domain.Enums;
using UniversityERP.Infrastructure.Dtos;
using UniversityERP.Infrastructure.Dtos.AttendanceRecordDtos;
using UniversityERP.Infrastructure.Services.Abstractions;

namespace UniversityERP.Infrastructure.Services.Implementations;

internal class AttendanceRecordService : IAttendanceRecordService
{
    private readonly IMapper _mapper;
    private readonly IAttendanceRecordRepository _attendanceRecords;
    private readonly IAttendanceSessionRepository _attendanceSessions;
    private readonly IEnrollmentCourseRepository _enrollmentCourses;

    public AttendanceRecordService(
        IMapper mapper,
        IAttendanceRecordRepository attendanceRecords,
        IAttendanceSessionRepository attendanceSessions,
        IEnrollmentCourseRepository enrollmentCourses)
    {
        _mapper = mapper;
        _attendanceRecords = attendanceRecords;
        _attendanceSessions = attendanceSessions;
        _enrollmentCourses = enrollmentCourses;
    }

    public async Task<ResultDto> CreateAsync(Guid attendanceSessionId, AttendanceRecordCreateDto dto)
    {
        var session = await _attendanceSessions.GetAll()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == attendanceSessionId);

        if (session is null)
            return new ResultDto(404, false, "Attendance session not found.");

        if (!session.IsActive)
            return new ResultDto(400, false, "Attendance session is inactive.");

        var enrollmentCourse = await _enrollmentCourses.GetAll()
            .AsNoTracking()
            .Include(x => x.StudentSemesterEnrollment)
            .FirstOrDefaultAsync(x => x.Id == dto.EnrollmentCourseId);

        if (enrollmentCourse is null)
            return new ResultDto(404, false, "Enrollment course not found.");

        if (enrollmentCourse.Status == EnrollmentCourseStatus.Dropped)
            return new ResultDto(400, false, "Cannot create attendance record for dropped enrollment course.");

        if (enrollmentCourse.CourseOfferingId != session.CourseOfferingId)
            return new ResultDto(400, false, "Enrollment course does not belong to the attendance session course offering.");

        if (await _attendanceRecords.ExistsAsync(attendanceSessionId, dto.EnrollmentCourseId, ignoreQueryFilter: true))
            return new ResultDto(409, false, "Attendance record already exists for this session and enrollment course.");

        var entity = _mapper.Map<Domain.Entities.AttendanceRecord>(dto);
        entity.AttendanceSessionId = attendanceSessionId;

        await _attendanceRecords.AddAsync(entity);
        await _attendanceRecords.SaveChangesAsync();

        return new ResultDto(201, true, "Attendance record created successfully.");
    }

    public async Task<ResultDto> UpdateAsync(Guid attendanceSessionId, AttendanceRecordUpdateDto dto)
    {
        var session = await _attendanceSessions.GetAll()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == attendanceSessionId);

        if (session is null)
            return new ResultDto(404, false, "Attendance session not found.");

        if (!session.IsActive)
            return new ResultDto(400, false, "Attendance session is inactive.");

        var entity = await _attendanceRecords.GetAsync(x => x.Id == dto.Id && x.AttendanceSessionId == attendanceSessionId);
        if (entity is null)
            return new ResultDto(404, false, "Attendance record not found.");

        var enrollmentCourse = await _enrollmentCourses.GetAll()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == dto.EnrollmentCourseId);

        if (enrollmentCourse is null)
            return new ResultDto(404, false, "Enrollment course not found.");

        if (enrollmentCourse.Status == EnrollmentCourseStatus.Dropped)
            return new ResultDto(400, false, "Cannot update attendance record for dropped enrollment course.");

        if (enrollmentCourse.CourseOfferingId != session.CourseOfferingId)
            return new ResultDto(400, false, "Enrollment course does not belong to the attendance session course offering.");

        if (await _attendanceRecords.ExistsAsync(attendanceSessionId, dto.EnrollmentCourseId, dto.Id, true))
            return new ResultDto(409, false, "Attendance record already exists for this session and enrollment course.");

        _mapper.Map(dto, entity);
        _attendanceRecords.Update(entity);
        await _attendanceRecords.SaveChangesAsync();

        return new ResultDto(200, true, "Attendance record updated successfully.");
    }

    public async Task<ResultDto> DeleteAsync(Guid attendanceSessionId, Guid id)
    {
        var session = await _attendanceSessions.GetAsync(x => x.Id == attendanceSessionId);
        if (session is null)
            return new ResultDto(404, false, "Attendance session not found.");

        var entity = await _attendanceRecords.GetAsync(x => x.Id == id && x.AttendanceSessionId == attendanceSessionId);
        if (entity is null)
            return new ResultDto(404, false, "Attendance record not found.");

        _attendanceRecords.Delete(entity);
        await _attendanceRecords.SaveChangesAsync();

        return new ResultDto(200, true, "Attendance record deleted successfully.");
    }

    public async Task<ResultDto<List<AttendanceRecordGetDto>>> GetAllAsync(Guid attendanceSessionId)
    {
        var sessionExists = await _attendanceSessions.GetAsync(x => x.Id == attendanceSessionId);
        if (sessionExists is null)
        {
            return new ResultDto<List<AttendanceRecordGetDto>>
            {
                StatusCode = 404,
                IsSucced = false,
                Message = "Attendance session not found."
            };
        }

        var list = await _attendanceRecords.GetAll()
            .AsNoTracking()
            .Where(x => x.AttendanceSessionId == attendanceSessionId)
            .Include(x => x.EnrollmentCourse)
                .ThenInclude(x => x.StudentSemesterEnrollment)
                    .ThenInclude(x => x.Student)
                        .ThenInclude(x => x.User)
            .OrderBy(x => x.EnrollmentCourse.StudentSemesterEnrollment.Student.User.FullName)
            .ToListAsync();

        return new ResultDto<List<AttendanceRecordGetDto>>
        {
            StatusCode = 200,
            IsSucced = true,
            Message = "Successfully",
            Data = _mapper.Map<List<AttendanceRecordGetDto>>(list)
        };
    }

    public async Task<ResultDto<AttendanceRecordGetDto>> GetByIdAsync(Guid attendanceSessionId, Guid id)
    {
        var entity = await _attendanceRecords.GetAll()
            .AsNoTracking()
            .Include(x => x.EnrollmentCourse)
                .ThenInclude(x => x.StudentSemesterEnrollment)
                    .ThenInclude(x => x.Student)
                        .ThenInclude(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == id && x.AttendanceSessionId == attendanceSessionId);

        if (entity is null)
        {
            return new ResultDto<AttendanceRecordGetDto>
            {
                StatusCode = 404,
                IsSucced = false,
                Message = "Attendance record not found."
            };
        }

        return new ResultDto<AttendanceRecordGetDto>
        {
            StatusCode = 200,
            IsSucced = true,
            Message = "Successfully",
            Data = _mapper.Map<AttendanceRecordGetDto>(entity)
        };
    }
}
