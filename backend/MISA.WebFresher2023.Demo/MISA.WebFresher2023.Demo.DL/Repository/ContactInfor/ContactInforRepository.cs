using Microsoft.Extensions.Configuration;
using MISA.WebFresher2023.Demo.DL.Entity;

namespace MISA.WebFresher2023.Demo.DL.Repository
{
    public class ContactInforRepository : BaseRepository<ContactInfor>, IContactInforRepository
    {
        public ContactInforRepository(IMSDatabase msDatabase) : base(msDatabase)
        {
        }
    }
}
