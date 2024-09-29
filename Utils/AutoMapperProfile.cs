using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using kit_stem_api.Models.Domain;
using kit_stem_api.Models.DTO;
using kit_stem_api.Models.DTO.Request;
using kit_stem_api.Models.DTO.Response;

namespace kit_stem_api.Utils
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Kit, KitInPackageResponseDTO>();

            // Using for packages
            CreateMap<Package, PackageResponseDTO>();
            CreateMap<PackageLab, LabInPackageResponseDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Lab.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Lab.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Lab.Price))
                .ForMember(dest => dest.MaxSupportTimes, opt => opt.MapFrom(src => src.Lab.MaxSupportTimes))
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Lab.Author))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Lab.Status))
                .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Lab.Level));
            CreateMap<Lab, LabInPackageResponseDTO>();
            CreateMap<PackageCreateDTO, Package>();

            // Using for getting labs
            CreateMap<Lab, LabResponseDTO>();

            CreateMap<LabUploadDTO, Lab>();

            // Using for getting KitComponents
            CreateMap<KitComponent, KitComponentDTO>().ReverseMap();
        }
    }
}