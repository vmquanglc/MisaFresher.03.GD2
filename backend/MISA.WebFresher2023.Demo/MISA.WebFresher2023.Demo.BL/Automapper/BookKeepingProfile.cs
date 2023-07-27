using AutoMapper;
using MISA.WebFresher2023.Demo.BL.Dto;
using MISA.WebFresher2023.Demo.DL.Entity;
using MISA.WebFresher2023.Demo.DL.Model;

namespace MISA.WebFresher2023.Demo.BL.Automapper
{
    /// <summary>
    /// Class dành cho automapper 
    /// </summary>
    /// Author: LeDucTiep (07/06/2023)
    public class BookKeepingProfile : Profile
    {
        public BookKeepingProfile()
        {
            // Map BookKeeping sang BookKeepingDto
            CreateMap<BookKeeping, BookKeepingDto>();
            CreateMap<BookKeepingDto, BookKeeping>();
            // Map BookKeepingCreateDto sang BookKeeping
            CreateMap<BookKeepingCreateDto, BookKeeping>();
            // Map BookKeepingUpdateDto sang BookKeeping
            CreateMap<BookKeepingUpdateDto, BookKeeping>();

            CreateMap<BookKeepingUpdateDto, BookKeeping>();

            CreateMap<BookKeepingCreateDto, BookKeepingDto>();

            CreateMap<BasePage<BookKeeping>, BasePage<BookKeepingDto>>();

            CreateMap<BookKeepingUpdateDto, BookKeepingDto>();
        }
    }
}
