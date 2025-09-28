using AutoMapper;
using ASM1.Repository.Models;

namespace ASM1.Service.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Customer, CustomerViewModel>().ReverseMap();
            CreateMap<CustomerCreateViewModel, Customer>();

            CreateMap<Quotation, QuotationViewModel>().ReverseMap();
            CreateMap<QuotationCreateViewModel, Quotation>();

            CreateMap<Order, OrderViewModel>().ReverseMap();
            CreateMap<OrderCreateViewModel, Order>();

            CreateMap<SalesContract, SalesContractViewModel>().ReverseMap();
            CreateMap<SalesContractCreateViewModel, SalesContract>();

            CreateMap<Payment, PaymentViewModel>().ReverseMap();
            CreateMap<PaymentCreateViewModel, Payment>();
        }
    }
}
