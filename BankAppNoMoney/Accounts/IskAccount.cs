using BankAppNoMoney.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankAppNoMoney.Accounts;

internal class IskAccount : AccountBase
{
    public IskAccount(string accountname, string accountnumber) 
        : base(accountname, accountnumber)
    {
    }

    internal override decimal Balance()
    {
        var t = bankTransactions.Sum(x => x.Amount);

        return t + StartingBalance;
    }
}
