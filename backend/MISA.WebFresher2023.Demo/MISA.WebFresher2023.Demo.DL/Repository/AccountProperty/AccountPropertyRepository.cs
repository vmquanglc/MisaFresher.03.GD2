using Microsoft.Extensions.Configuration;
using MISA.WebFresher2023.Demo.DL.Entity;

namespace MISA.WebFresher2023.Demo.DL.Repository
{
    public class AccountPropertyRepository : BaseRepository<AccountProperty>, IAccountPropertyRepository
    {
        public AccountPropertyRepository(IMSDatabase msDatabase) : base(msDatabase)
        {
        }
    }
}
