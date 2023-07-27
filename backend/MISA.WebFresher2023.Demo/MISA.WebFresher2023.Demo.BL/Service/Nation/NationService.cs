using AutoMapper;
using MISA.WebFresher2023.Demo.BL.Dto;
using MISA.WebFresher2023.Demo.DL;
using MISA.WebFresher2023.Demo.DL.Entity;
using MISA.WebFresher2023.Demo.DL.Repository;

namespace MISA.WebFresher2023.Demo.BL.Service
{
    public class NationService : BaseService<Nation, NationDto, NationCreateDto, NationUpdateDto>, INationService
    {
        public NationService(INationRepository nationRepository, IMSDatabase msDatabase, IMapper mapper) : base(msDatabase, nationRepository, mapper)
        {
        }
    }
}
