using AutoMapper;
using MISA.WebFresher2023.Demo.BL.Dto;
using MISA.WebFresher2023.Demo.DL.Entity;
using MISA.WebFresher2023.Demo.DL.Model;
using static Dapper.SqlMapper;

namespace MISA.WebFresher2023.Demo.BL.Automapper
{
    /// <summary>
    /// Class TermsOfPayment dành cho automapper 
    /// </summary>
    /// Author: LeDucTiep (07/06/2023)
    public class TermsOfPaymentProfile : Profile
    {
        public TermsOfPaymentProfile()
        {
            // Map TermsOfPayment sang TermsOfPaymentDto
            CreateMap<TermsOfPayment, TermsOfPaymentDto>();
            CreateMap<TermsOfPaymentDto, TermsOfPayment>();
            // Map TermsOfPaymentCreateDto sang TermsOfPayment
            CreateMap<TermsOfPaymentCreateDto, TermsOfPayment>();
            // Map TermsOfPaymentUpdateDto sang TermsOfPayment
            CreateMap<TermsOfPaymentUpdateDto, TermsOfPayment>();
            CreateMap<BasePage<TermsOfPayment>, BasePage<TermsOfPaymentDto>>();

            CreateMap<TermsOfPaymentUpdateDto, TermsOfPayment>();

            CreateMap<TermsOfPaymentCreateDto, TermsOfPaymentDto>();

            CreateMap<TermsOfPaymentUpdateDto, TermsOfPaymentDto>();
        }
    }
}
