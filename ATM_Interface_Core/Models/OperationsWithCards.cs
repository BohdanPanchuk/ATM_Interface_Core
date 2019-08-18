using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATM_Interface_Core.Models
{
    public class OperationsWithCards
    {
        public Guid Id { get; set; }
        public Guid CardId { get; set; }
        public Guid OperationTypeId { get; set; }
        public DateTime OperationTime { get; set; }
    }
}
