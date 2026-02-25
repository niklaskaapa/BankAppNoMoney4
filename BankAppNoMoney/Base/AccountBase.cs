using System;
using System.Collections.Generic;
using System.Text;

namespace BankAppNoMoney.Base
{
    internal abstract class AccountBase
    {

        internal Guid Id { get; set; } = Guid.NewGuid();   

        protected decimal StartingBalance { get; set; } = 0;

        internal string AccountName { get; set; } = "";

        internal string AccountNumber { get; set; } = "";
        
        internal decimal InterestRate { get; set; } = 0;
        internal decimal InterestAmount { get; set; } = 0;

        protected List<BankTransaction> bankTransactions = new List<BankTransaction>();


        protected AccountBase(string accountname, string accountnumber)
        {

            if (string.IsNullOrWhiteSpace(accountnumber) || accountnumber.Length != 11) 
            { 
                throw new ArgumentException("Kontonummer måste vara exakt 11 tecken!", nameof(accountnumber)); 
            }


            AccountName = accountname;
            AccountNumber = accountnumber;
        }


        internal abstract decimal Balance();
        
        internal virtual void Deposit(decimal amount)
        {

            if (amount <= 0)
            {
                Console.WriteLine("Insättning måste vara större än 0!");
                return;
            }


            if (amount >= 10000)
            {
                Console.WriteLine("För stort belopp att sätta in! Max är: 10 000:-");
                return;
            } 

            var t = new BankTransaction
            {
                Amount = amount,
                TransactionDate = DateTime.Now
            };

            bankTransactions.Add(t);

            Console.WriteLine("Du har satt in " + amount + " kr");

        }
        internal virtual void Withdraw(decimal amount)
        {
            var balance = Balance();

            if (amount <= 0)
            {
                Console.WriteLine("Uttag måste vara större än 0!");
                return;
            }

            if (amount > balance)
            {
                Console.WriteLine("För lite saldo på konto för att göra uttag!");
                return;
            }

            var t = new BankTransaction
            {
                Amount = -amount,
                TransactionDate = DateTime.Now
            };

            bankTransactions.Add(t);

            Console.WriteLine("Du har tagit ut " + amount + " kr");

        }

        internal decimal CalculateYearlyInterestDaily(int year)
        {
            decimal totalInterest = 0m;

            DateTime startDate = new DateTime(year, 1, 1);
            DateTime endDate = new DateTime(year, 12, 31);

            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                decimal balanceForDay = GetBalanceForDate(date);

                decimal dailyInterest = balanceForDay * (InterestRate / 365m);

                totalInterest += dailyInterest;
            }

            return Math.Round (totalInterest, 2);
        }

        private decimal GetBalanceForDate(DateTime date)
        {
            decimal balance = 0m;

            foreach (var transaction in bankTransactions)
            {
                if (transaction.TransactionDate.Date <= date.Date)
                {
                    balance += transaction.Amount;
                }
            }

            return balance;
        }


        internal void DepositWithDate(decimal amount, DateTime date)
        {
            var t = new BankTransaction
            {
                Amount = amount,
                TransactionDate = date
            };

            bankTransactions.Add(t);
        }


    }
}
