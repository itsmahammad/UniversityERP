using AutoMapper;
using UniversityERP.Domain.Entities;
using UniversityERP.Infrastructure.Dtos.TeacherDtos;

namespace UniversityERP.Infrastructure.Profiles;

public class TeacherProfile : Profile
{
    public TeacherProfile()
    {
        CreateMap<TeacherCreateDto, Teacher>();
        CreateMap<TeacherUpdateDto, Teacher>();
        CreateMap<Teacher, TeacherGetDto>();
    }
}