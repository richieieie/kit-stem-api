using AutoMapper;
using KSH.Api.Constants;
using KSH.Api.Models.Domain;
using KSH.Api.Models.DTO;
using KSH.Api.Models.DTO.Request;
using KSH.Api.Models.DTO.Response;
using KST.Api.Models.DTO.Request;
using KST.Api.Models.DTO.Response;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KSH.Api.Utils
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Using for users
            CreateMap<ApplicationUser, UserProfileDTO>();

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
            CreateMap<Kit, KitInPackageResponseDTO>();
            CreateMap<Package, PackageCartResponseDTO>()
                .ForMember(dest => dest.PackageId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Labs, opt => opt.MapFrom(src => src.PackageLabs)).ReverseMap();

            // Using for getting labs
            CreateMap<Lab, LabResponseDTO>();
            CreateMap<LabUploadDTO, Lab>();

            // Using for getting KitComponents
            CreateMap<KitComponent, KitComponentDTO>().ReverseMap();

            // Using for Order
            CreateMap<UserOrders, OrderResponseDTO>();
            CreateMap<PackageOrder, PackageOrderResponseDTO>();
            CreateMap<VNPaymentRequestDTO, Payment>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.MethodId, opt => opt.MapFrom(src => OrderFulfillmentConstants.PaymentVnPay))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => false));
            CreateMap<Payment, PaymentResponseDTO>();
            CreateMap<UserOrders, OrderCreateDTO>().ReverseMap();

            //Using for getting Carts
            CreateMap<Cart, CartDTO>().ReverseMap();
            CreateMap<Cart, CartResponseDTO>().ReverseMap();

            // Using for Kit

            CreateMap<Kit, KitCreateDTO>().ReverseMap();
            CreateMap<Package, KitResponseDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Kit.Id))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Kit.CategoryId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Kit.Name))
                .ForMember(dest => dest.Brief, opt => opt.MapFrom(src => src.Kit.Brief))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Kit.Description))
                .ForMember(dest => dest.PurchaseCost, opt => opt.MapFrom(src => src.Kit.PurchaseCost))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Kit.Status))
                .ForMember(dest => dest.KitsCategory, opt => opt.MapFrom(src => src.Kit.Category))
                .ForMember(dest => dest.KitImages, opt => opt.MapFrom(src => src.Kit.KitImages));
            CreateMap<KitsCategory, CategoryDTO>();
            CreateMap<KitImage, KitImageDTO>();
            // Using for KitImage
            CreateMap<KitImage, KitImageCreateDTO>().ReverseMap();

            //Using for Component
            CreateMap<Component, ComponentDTO>().ReverseMap();

            //Using for PackageOrder
            CreateMap<PackageOrder, PackageOrderCreateDTO>().ReverseMap();

            //Using for Type
            CreateMap<ComponentsType, ComponentTypeCreateDTO>().ReverseMap();

            //Using for LabSupport
            CreateMap<LabSupport, LabSupportUpdateStaffDTO>().ReverseMap();
            CreateMap<LabSupport, LabSupportResponseDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.OrderSupport.Order.User.UserName));
           

        }
    }
}