using AutoMapper;
using MISA.WebFresher2023.Demo.BL.Dto;
using MISA.WebFresher2023.Demo.DL;
using MISA.WebFresher2023.Demo.DL.Entity;
using MISA.WebFresher2023.Demo.DL.Repository;

namespace MISA.WebFresher2023.Demo.BL.Service
{
    public class SpecificAddressService : BaseService<SpecificAddress, SpecificAddressDto, SpecificAddressCreateDto, SpecificAddressUpdateDto>, ISpecificAddressService
    {
        public SpecificAddressService(ISpecificAddressRepository specificAddressRepository,
            IMSDatabase msDatabase,
            IMapper mapper) : base(msDatabase, specificAddressRepository, mapper)
        {
        }
    }
}
