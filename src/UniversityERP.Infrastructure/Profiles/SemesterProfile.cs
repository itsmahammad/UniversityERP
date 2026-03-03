using AutoMapper;
using UniversityERP.Domain.Entities;
using UniversityERP.Infrastructure.Dtos.SemesterDtos;

namespace UniversityERP.Infrastructure.Profiles;

public class SemesterProfile : Profile
{
    public SemesterProfile()
    {
        CreateMap<SemesterCreateDto, Semester>();
        CreateMap<SemesterUpdateDto, Semester>();
        CreateMap<Semester, SemesterGetDto>();
    }
}