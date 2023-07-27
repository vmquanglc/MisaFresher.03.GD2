using Microsoft.Extensions.Configuration;
using MISA.WebFresher2023.Demo.DL.Entity;

namespace MISA.WebFresher2023.Demo.DL.Repository
{
    public class NationRepository : BaseRepository<Nation>, INationRepository
    {
        public NationRepository(IMSDatabase msDatabase) : base(msDatabase)
        {
        }
    }
}
