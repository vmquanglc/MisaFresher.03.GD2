using AutoMapper;
using MISA.WebFresher2023.Demo.BL.Dto;
using MISA.WebFresher2023.Demo.DL;
using MISA.WebFresher2023.Demo.DL.Entity;
using MISA.WebFresher2023.Demo.DL.Repository;

namespace MISA.WebFresher2023.Demo.BL.Service
{
    public class BookKeepingService : BaseService<BookKeeping, BookKeepingDto, BookKeepingCreateDto, BookKeepingUpdateDto>, IBookKeepingService
    {
        public BookKeepingService(
            IMSDatabase msDatabase,
            IBookKeepingRepository bookKeepingRepository,
            IMapper mapper) : base(msDatabase, bookKeepingRepository, mapper)
        {
        }
    }
}
