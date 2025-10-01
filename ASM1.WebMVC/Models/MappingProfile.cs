using AutoMapper;
using ASM1.Repository.Models;

namespace ASM1.WebMVC.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Customer, CustomerViewModel>()
                .ForMember(dest => dest.DealerName, opt => opt.MapFrom(src => src.Dealer != null ? src.Dealer.FullName : ""))
                .ReverseMap();
            CreateMap<CustomerCreateViewModel, Customer>();

            CreateMap<Quotation, QuotationViewModel>()
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer != null ? src.Customer.FullName : ""))
                .ForMember(dest => dest.VehicleInfo, opt => opt.MapFrom(src => src.Variant != null && src.Variant.VehicleModel != null ? 
                    $"{src.Variant.VehicleModel.Manufacturer.Name} {src.Variant.VehicleModel.Name} {src.Variant.Version}" : ""))
                .ForMember(dest => dest.DealerName, opt => opt.MapFrom(src => src.Dealer != null ? src.Dealer.FullName : ""))
                .ReverseMap();
            CreateMap<QuotationCreateViewModel, Quotation>()
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.FinalPrice));

            CreateMap<Order, OrderViewModel>()
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer != null ? src.Customer.FullName : ""))
                .ForMember(dest => dest.DealerName, opt => opt.MapFrom(src => src.Dealer != null ? src.Dealer.FullName : ""))
                .ForMember(dest => dest.VehicleInfo, opt => opt.MapFrom(src => src.Variant != null && src.Variant.VehicleModel != null ? 
                    $"{src.Variant.VehicleModel.Manufacturer.Name} {src.Variant.VehicleModel.Name} {src.Variant.Version}" : ""))
                .ReverseMap();
            CreateMap<OrderCreateViewModel, Order>();

            CreateMap<SalesContract, SalesContractViewModel>().ReverseMap();
            CreateMap<SalesContractCreateViewModel, SalesContract>();

            CreateMap<Payment, PaymentViewModel>().ReverseMap();
            CreateMap<PaymentCreateViewModel, Payment>();

            CreateMap<Manufacturer, ManufacturerViewModel>()
                .ForMember(dest => dest.VehicleModelCount, opt => opt.MapFrom(src => src.VehicleModels.Count))
                .ReverseMap();
            CreateMap<ManufacturerCreateViewModel, Manufacturer>();
            CreateMap<Manufacturer, ManufacturerDetailViewModel>().ReverseMap();

            CreateMap<VehicleModel, VehicleModelViewModel>()
                .ForMember(dest => dest.ManufacturerName, opt => opt.MapFrom(src => src.Manufacturer != null ? src.Manufacturer.Name : ""))
                .ForMember(dest => dest.VariantCount, opt => opt.MapFrom(src => src.VehicleVariants.Count))
                .ReverseMap();
            CreateMap<VehicleModelCreateViewModel, VehicleModel>();
            CreateMap<VehicleModel, VehicleModelDetailViewModel>()
                .ForMember(dest => dest.ManufacturerName, opt => opt.MapFrom(src => src.Manufacturer != null ? src.Manufacturer.Name : ""))
                .ReverseMap();

            CreateMap<VehicleVariant, VehicleVariantViewModel>()
                .ForMember(dest => dest.ModelName, opt => opt.MapFrom(src => src.VehicleModel != null ? src.VehicleModel.Name : ""))
                .ForMember(dest => dest.ManufacturerName, opt => opt.MapFrom(src => src.VehicleModel != null && src.VehicleModel.Manufacturer != null ? src.VehicleModel.Manufacturer.Name : ""))
                .ReverseMap();
            CreateMap<VehicleVariantCreateViewModel, VehicleVariant>();
            CreateMap<VehicleVariant, VehicleVariantDetailViewModel>().ReverseMap();
        }
    }
}
