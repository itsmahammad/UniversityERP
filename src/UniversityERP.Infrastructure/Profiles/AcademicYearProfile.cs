using AutoMapper;
using UniversityERP.Domain.Entities;
using UniversityERP.Infrastructure.Dtos.AcademicYearDtos;

namespace UniversityERP.Infrastructure.Profiles;

public class AcademicYearProfile : Profile
{
    public AcademicYearProfile()
    {
        CreateMap<AcademicYearCreateDto, AcademicYear>()
            .ForMember(d => d.Name, o => o.MapFrom(s => s.Name.Trim()));

        CreateMap<AcademicYearUpdateDto, AcademicYear>()
            .ForMember(d => d.Name, o => o.MapFrom(s => s.Name.Trim()));

        CreateMap<AcademicYear, AcademicYearGetDto>();
    }
}