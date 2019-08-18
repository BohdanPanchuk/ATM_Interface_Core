using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATM_Interface_Core.Models
{
    public class OperationsWithCardViewModel
    {
        public string CardNumber { get; set; }
        public string OperationType { get; set; }
        public DateTime Date { get; set; }
    }
}
