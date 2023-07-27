using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WebFresher2023.Demo.DL.Entity
{
    public class BookKeeping: BaseEntity
    {
        public Guid BookKeepingId { get; set; }

        public Guid ReceiptId { get; set; }

        public string Note { get; set; }
        
        public Guid DebitAccountId { get; set; }

        public Guid CollectAccountId { get; set; }

        public double AmountOfMoney { get; set; }
    }
}
