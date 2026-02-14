using AutoMapper;
using UniversityERP.Domain.Entities;
using UniversityERP.Infrastructure.Dtos.FacultyDtos;

namespace UniversityERP.Infrastructure.Profiles;

internal class FacultyProfile : Profile
{
    public FacultyProfile()
    {
        CreateMap<Faculty, FacultyCreateDto>().ReverseMap();
        CreateMap<Faculty, FacultyUpdateDto>().ReverseMap();
        CreateMap<Faculty, FacultyGetDto>().ReverseMap();
    }
}
