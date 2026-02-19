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

    }
}
