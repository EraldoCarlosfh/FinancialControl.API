using AutoMapper;
using FinancialControl.API.ViewModels;
using FinancialControl.Domain.Dtos;
using FinancialControl.Domain.Entities;

namespace FinancialControl.Api.Mappings
{
    public class FinancialControlProfile : Profile
    {
        public FinancialControlProfile()
        {
            CreateMap<Transaction, TransactionViewModel>()
                .ForMember(c => c.Id, opt => opt.MapFrom(c => c.Id.ToString()))
                .ForMember(c => c.Date, opt => opt.MapFrom(c => c.Date.ToString("dd/MM/yyyy HH:mm")))
                .ReverseMap();
            CreateMap<DailyBalanceResultDto, DailyBalanceViewModel>()
                .ForMember(c => c.Date, opt => opt.MapFrom(c => c.Date.ToString("dd/MM/yyyy HH:mm")))
                .ReverseMap();
        }        
    }
}
