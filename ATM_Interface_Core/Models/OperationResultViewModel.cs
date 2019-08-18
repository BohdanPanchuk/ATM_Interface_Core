using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATM_Interface_Core.Models
{
    public class OperationResultViewModel
    {
        public string CardNumber { get; set; }
        public DateTime Date { get; set; }
        public decimal WithdrawnMoney { get; set; }
        public decimal AvailableMoney { get; set; }
    }
}
