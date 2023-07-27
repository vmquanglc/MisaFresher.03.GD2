using AutoMapper;
using MISA.WebFresher2023.Demo.BL.Dto;
using MISA.WebFresher2023.Demo.DL;
using MISA.WebFresher2023.Demo.DL.Entity;
using MISA.WebFresher2023.Demo.DL.Repository;

namespace MISA.WebFresher2023.Demo.BL.Service
{
    public class ContactInforService : BaseService<ContactInfor, ContactInforDto, ContactInforCreateDto, ContactInforUpdateDto>, IContactInforService
    {
        public ContactInforService(IContactInforRepository contactInforRepository,
            IMSDatabase msDatabase,
            IMapper mapper) : base(msDatabase, contactInforRepository, mapper)
        {
        }
    }
}
