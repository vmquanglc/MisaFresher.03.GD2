using Microsoft.Extensions.Configuration;
using MISA.WebFresher2023.Demo.DL.Entity;

namespace MISA.WebFresher2023.Demo.DL.Repository
{
    public class PositionRepository : BaseRepository<Position>, IPositionRepository
    {
        public PositionRepository(IMSDatabase msDatabase) : base(msDatabase)
        {
        }
    }
}
