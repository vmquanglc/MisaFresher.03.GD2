using AutoMapper;
using MISA.WebFresher2023.Demo.BL.Dto;
using MISA.WebFresher2023.Demo.DL;
using MISA.WebFresher2023.Demo.DL.Entity;
using MISA.WebFresher2023.Demo.DL.Repository;

namespace MISA.WebFresher2023.Demo.BL.Service
{
    public class TermsOfPaymentService : BaseService<TermsOfPayment, TermsOfPaymentDto, TermsOfPaymentCreateDto, TermsOfPaymentUpdateDto>, ITermsOfPaymentService
    {
        public TermsOfPaymentService(
            ITermsOfPaymentRepository termsOfPaymentRepository,
            IMSDatabase msDatabase,
            IMapper mapper) : base(msDatabase,
                termsOfPaymentRepository,
                mapper)
        {
        }
    }
}
