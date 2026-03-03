using AutoMapper;
using UniversityERP.Domain.Entities;
using UniversityERP.Infrastructure.Dtos.AcademicProgramDtos;

namespace UniversityERP.Infrastructure.Profiles;

public class AcademicProgramProfile : Profile
{
    public AcademicProgramProfile()
    {
        CreateMap<AcademicProgramCreateDto, AcademicProgram>()
            .ForMember(d => d.Name, o => o.MapFrom(s => s.Name.Trim()));

        CreateMap<AcademicProgramUpdateDto, AcademicProgram>()
            .ForMember(d => d.Name, o => o.MapFrom(s => s.Name.Trim()));

        CreateMap<AcademicProgram, AcademicProgramGetDto>();
    }
}