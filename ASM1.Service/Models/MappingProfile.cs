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
        }
    }
}
