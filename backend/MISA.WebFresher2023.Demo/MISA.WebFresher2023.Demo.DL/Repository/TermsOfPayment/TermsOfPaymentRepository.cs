using MISA.WebFresher2023.Demo.DL.Entity;

namespace MISA.WebFresher2023.Demo.DL.Repository
{
    public class TermsOfPaymentRepository : BaseRepository<TermsOfPayment>, ITermsOfPaymentRepository
    {
        public TermsOfPaymentRepository(IMSDatabase msDatabase) : base(msDatabase)
        {
        }
    }
}
