using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ATM_Interface_Core.Models;
using ATM_Interface_Core.Data;

namespace ATM_Interface_Core.Controllers
{
    public class HomeController : Controller
    {
        private readonly CardContext _context;

        public Card currentCard = new Card();
        public OperationType operationType = new OperationType();

        public string sessionCurrentCardKey = "CurrentCard";
        public string sessionWrongPINCodeCountKey = "WrongPINCode";
        public string errorMessage = "";

        public HomeController(CardContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult CardNumber()
        {
            HttpContext.Session.Set(sessionCurrentCardKey, "");
            HttpContext.Session.Set(sessionWrongPINCodeCountKey, 0);

            return View();
        }

        [HttpPost]
        public IActionResult CheckCardNumber(string cardNumber)
        {
            currentCard = _context.Cards.Where(card => card.Number == cardNumber).FirstOrDefault();

            if (currentCard == null)
            {
                errorMessage = "Card not found.";
                return View("Error", errorMessage);
            }

            if (currentCard.Status == true)
            {
                HttpContext.Session.Set(sessionCurrentCardKey, currentCard);

                return View("PINCode");
            }
            else
            {
                errorMessage = "Card locked! Unlock your card.";
                return View("Error", errorMessage);
            }
        }

        [HttpGet]
        public IActionResult PINCode()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CheckPINCode(string pin)
        {
            currentCard = HttpContext.Session.Get<Card>(sessionCurrentCardKey);

            int wrongPINCodeCount = HttpContext.Session.Get<int>(sessionWrongPINCodeCountKey);

            if (currentCard.PIN == pin)
            {
                return View("Operations");
            }
            else
            {
                wrongPINCodeCount++;

                if (wrongPINCodeCount < 4)
                {
                    HttpContext.Session.Set(sessionWrongPINCodeCountKey, wrongPINCodeCount);

                    return View("PINCode");
                }
                else
                {
                    currentCard.Status = false;

                    _context.Entry(currentCard).State = EntityState.Modified;

                    _context.SaveChanges();

                    HttpContext.Session.Set(sessionCurrentCardKey, currentCard);

                    return View("Error");
                }
            }
        }

        public IActionResult Operations()
        {
            currentCard = HttpContext.Session.Get<Card>(sessionCurrentCardKey);

            return View();
        }

        public IActionResult Balans()
        {
            currentCard = HttpContext.Session.Get<Card>(sessionCurrentCardKey);

            BalansViewModel balans = new BalansViewModel();

            balans.AvailableMoney = currentCard.AvailableMoney;
            balans.CardNumber = currentCard.Number;
            balans.Date = DateTime.UtcNow;

            operationType = _context.OperationTypes.Where(op => op.Name == "Balans").First();

            AddingOperationWithCard(operationType.Id, balans.Date);

            return View(balans);
        }

        public IActionResult WithdrawMoney()
        {
            currentCard = HttpContext.Session.Get<Card>(sessionCurrentCardKey);

            return View();
        }

        [HttpPost]
        public IActionResult WithdrawMoney(decimal moneyForWithdraw)
        {
            currentCard = HttpContext.Session.Get<Card>(sessionCurrentCardKey);

            if (currentCard.AvailableMoney >= moneyForWithdraw)
            {
                OperationResultViewModel operationResult = new OperationResultViewModel();

                currentCard.AvailableMoney -= moneyForWithdraw;

                _context.Entry(currentCard).State = EntityState.Modified;

                HttpContext.Session.Set(sessionCurrentCardKey, currentCard);

                operationResult.CardNumber = currentCard.Number;
                operationResult.WithdrawnMoney = moneyForWithdraw;
                operationResult.AvailableMoney = currentCard.AvailableMoney;
                operationResult.Date = DateTime.UtcNow;

                _context.SaveChanges();

                operationType = _context.OperationTypes.Where(op => op.Name == "Withdraw money").First();

                AddingOperationWithCard(operationType.Id, operationResult.Date);

                return View("OperationResult", operationResult);
            }
            else
            {
                errorMessage = "Not enought money!";
                return View("Error", errorMessage);
            }
        }

        public IActionResult OperationsWithCards()
        {
            var operationsWithCards = (from OperationsWithCards in _context.OperationsWithCards
                                      join Card
                                        in _context.Cards
                                            on OperationsWithCards.CardId equals Card.Id
                                      join OperationTypes
                                        in _context.OperationTypes
                                            on OperationsWithCards.OperationTypeId equals OperationTypes.Id
                                      orderby OperationsWithCards.OperationTime
                                      select new OperationsWithCardViewModel { CardNumber = Card.Number, OperationType = OperationTypes.Name, Date = OperationsWithCards.OperationTime }).ToList();

            return View(operationsWithCards);
        }

        private void AddingOperationWithCard(Guid operationTypeId, DateTime operationDate)
        {
            OperationsWithCards operationWithCards = new OperationsWithCards();

            operationWithCards.Id = Guid.NewGuid();
            operationWithCards.CardId = currentCard.Id;
            operationWithCards.OperationTypeId = operationTypeId;
            operationWithCards.OperationTime = operationDate;

            _context.OperationsWithCards.Add(operationWithCards);
            _context.SaveChanges();
        }
    }
}
