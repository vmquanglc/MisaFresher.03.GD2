using Microsoft.Extensions.Configuration;
using MISA.WebFresher2023.Demo.DL.Entity;

namespace MISA.WebFresher2023.Demo.DL.Repository
{
    public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(IMSDatabase msDatabase) : base(msDatabase)
        {
        }
    }
}
