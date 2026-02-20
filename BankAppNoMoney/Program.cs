using BankAppNoMoney.Accounts;
using BankAppNoMoney.Base;

namespace BankAppNoMoney;

internal class Program
{
    static void Main(string[] args)
    {

        Bank bank = new Bank();
        
        TestData testData = new TestData();


        testData.TestCalculateInteresRate(bank);

        bank.ShowBankMenu();
        

        //new Bank().ShowBankMenu();

       



    }
}
