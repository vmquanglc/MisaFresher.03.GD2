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
    public class ReceiptProfile : Profile
    {
        public ReceiptProfile()
        {
            // Map Receipt sang ReceiptDto
            CreateMap<Receipt, ReceiptDto>();
            CreateMap<ReceiptDto, Receipt>();
            // Map ReceiptCreateDto sang Receipt
            CreateMap<ReceiptUpdateDto, Receipt>();
            // Map ReceiptUpdateDto sang Receipt
            CreateMap<ReceiptUpdateDto, Receipt>();

            CreateMap<ReceiptUpdateDto, Receipt>();

            CreateMap<ReceiptUpdateDto, ReceiptDto>();

            CreateMap<BasePage<Receipt>, BasePage<ReceiptDto>>();

            CreateMap<ReceiptUpdateDto, ReceiptDto>();
        }
    }
}
