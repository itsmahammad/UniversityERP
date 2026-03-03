using AutoMapper;
using UniversityERP.Domain.Entities;
using UniversityERP.Infrastructure.Dtos.DepartmentDtos;

namespace UniversityERP.Infrastructure.Profiles;

public class DepartmentProfile : Profile
{
    public DepartmentProfile()
    {
        CreateMap<DepartmentCreateDto, Department>()
            .ForMember(d => d.Name, o => o.MapFrom(s => s.Name.Trim()))
            .ForMember(d => d.Code, o => o.MapFrom(s => s.Code.Trim().ToUpperInvariant()));

        CreateMap<DepartmentUpdateDto, Department>()
            .ForMember(d => d.Name, o => o.MapFrom(s => s.Name.Trim()))
            .ForMember(d => d.Code, o => o.MapFrom(s => s.Code.Trim().ToUpperInvariant()));

        CreateMap<Department, DepartmentGetDto>();
    }
}