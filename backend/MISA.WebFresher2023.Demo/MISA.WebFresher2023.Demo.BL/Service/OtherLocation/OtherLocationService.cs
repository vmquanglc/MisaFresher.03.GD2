using AutoMapper;
using MISA.WebFresher2023.Demo.BL.Dto;
using MISA.WebFresher2023.Demo.DL;
using MISA.WebFresher2023.Demo.DL.Entity;
using MISA.WebFresher2023.Demo.DL.Repository;

namespace MISA.WebFresher2023.Demo.BL.Service
{
    public class OtherLocationService : BaseService<OtherLocation, OtherLocationDto, OtherLocationCreateDto, OtherLocationUpdateDto>, IOtherLocationService
    {
        public OtherLocationService(IOtherLocationRepository otherLocationRepository, IMSDatabase msDatabase, IMapper mapper) : base(msDatabase, otherLocationRepository, mapper)
        {
        }
    }
}
