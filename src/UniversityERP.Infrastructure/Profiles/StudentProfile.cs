using AutoMapper;
using UniversityERP.Domain.Entities;
using UniversityERP.Infrastructure.Dtos.StudentDtos;

namespace UniversityERP.Infrastructure.Profiles;

public class StudentProfile : Profile
{
    public StudentProfile()
    {
        CreateMap<StudentCreateDto, Student>();
        CreateMap<StudentUpdateDto, Student>();
        CreateMap<Student, StudentGetDto>();
    }
}