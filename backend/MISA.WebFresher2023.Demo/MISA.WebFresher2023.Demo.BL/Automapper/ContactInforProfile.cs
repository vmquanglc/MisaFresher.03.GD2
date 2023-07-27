using AutoMapper;
using MISA.WebFresher2023.Demo.BL.Dto;
using MISA.WebFresher2023.Demo.DL.Entity;
using MISA.WebFresher2023.Demo.DL.Model;

namespace MISA.WebFresher2023.Demo.BL.Automapper
{
    /// <summary>
    /// Class ContactInfor dành cho automapper 
    /// </summary>
    /// Author: LeDucTiep (07/06/2023)
    public class ContactInforProfile : Profile
    {
        public ContactInforProfile()
        {
            // Map ContactInfor sang ContactInforDto
            CreateMap<ContactInfor, ContactInforDto>();
            CreateMap<ContactInforDto, ContactInfor>();
            // Map ContactInforCreateDto sang ContactInfor
            CreateMap<ContactInforCreateDto, ContactInfor>();
            // Map ContactInforUpdateDto sang ContactInfor
            CreateMap<ContactInforUpdateDto, ContactInfor>();

            CreateMap<ContactInforUpdateDto, ContactInfor>();

            CreateMap<BasePage<ContactInfor>, BasePage<ContactInforDto>>();
            CreateMap<ContactInforCreateDto, ContactInforDto>();

            CreateMap<ContactInforUpdateDto, ContactInforDto>();
        }
    }
}
