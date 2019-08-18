using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ATM_Interface_Core.Models
{
    public class Card
    {
        public Guid Id { get; set; }

        [Display(Name = "Card Number")]
        public string Number { get; set; }

        [Display(Name = "PIN code")]
        public string PIN { get; set; }

        public decimal AvailableMoney { get; set; }
        public bool Status { get; set; }
    }
}
