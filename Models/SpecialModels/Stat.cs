using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OXG.ServiceCenterWeb.Models
{
    public class Stat
    {
        public DateTime Now { get; set; } = DateTime.Now;

        public int NowReceiptsCreated { get; set; }

        public int NowReceiptsClosed { get; set; }

        public decimal NowReceiptsSum { get; set; }

        public int MonthReceiptsCreated { get; set; }

        public int MonthReceiptsClosed { get; set; }

        public decimal MonthReceiptsSum { get; set; }

        public string DayWorkerName { get; set; }

        public decimal DayWorkerSum { get; set; }

        public int DayWorkerReceipts { get; set; }

        public string MonthWorkerName { get; set; }

        public decimal MonthWorkerSum { get; set; }

        public int MonthWorkerReceipts { get; set; }

    }
}
