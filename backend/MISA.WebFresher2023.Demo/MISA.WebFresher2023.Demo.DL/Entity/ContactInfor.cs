using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WebFresher2023.Demo.DL.Entity
{
    public class ContactInfor : BaseEntity
    {
        public Guid ContactInforId { get; set; }
        public int Vocative { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string LandlineNumber { get; set; }
        public string RecipientFullName { get; set; }
        public string RecipientEmail { get; set; }
        public string RecipientPhoneNumber { get; set; }
        public string LegalRepresentative { get; set; }
    }
}
