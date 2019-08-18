using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ATM_Interface_Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ATM_Interface_Core.Data
{
    public class CardContext : DbContext
    {
        public CardContext(DbContextOptions<CardContext> options) : base(options) { }

        public DbSet<Card> Cards { get; set; }
        public DbSet<OperationType> OperationTypes { get; set; }
        public DbSet<OperationsWithCards> OperationsWithCards { get; set; }
    }
}
