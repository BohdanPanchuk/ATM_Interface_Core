using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ATM_Interface_Core.Models;

namespace ATM_Interface_Core.Data
{
    public class DbInitializer
    {
        public static void Initialize(CardContext context)
        {
            context.Database.EnsureCreated();

            if (context.Cards.Any())
            {
                return;
            }

            var cards = new Card[]
            {
                new Card { Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), Number = "1111111111111111", Status = true, AvailableMoney = 1000, PIN = "1111" },
                new Card { Id = Guid.Parse("00000000-0000-0000-0000-000000000002"), Number = "2222222222222222", Status = true, AvailableMoney = 2000, PIN = "2222" },
                new Card { Id = Guid.Parse("00000000-0000-0000-0000-000000000003"), Number = "3333333333333333", Status = true, AvailableMoney = 500, PIN = "1111" }
            };

            foreach (Card card in cards)
            {
                context.Cards.Add(card);
            }

            context.SaveChanges();

            var operationTypes = new OperationType[]
            {
                new OperationType { Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), Name = "Balans" },
                new OperationType { Id = Guid.Parse("00000000-0000-0000-0000-000000000002"), Name = "Withdraw Money" }
            };

            foreach (OperationType operationType in operationTypes)
            {
                context.OperationTypes.Add(operationType);
            }

            context.SaveChanges();
        }
    }
}
