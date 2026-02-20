using BankAppNoMoney.Accounts;
using BankAppNoMoney.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace BankAppNoMoney;

internal class Bank
{

    

    private List<AccountBase> accounts {  get; set; } = new List<AccountBase>();

    internal void AddAccount (AccountBase account)
    {
        accounts.Add(account); 
    }

    internal void RemoveAccount (Guid accountId)
    {
        var account = accounts.FirstOrDefault(x => x.Id == accountId);
        if (account != null)
        {
            accounts.Remove(account);
        }
    }

    internal List<AccountBase> GetAccounts()
    {
        return accounts;
    }

    internal void ShowBankMenu()
    {
        while (true)
        {

            Console.WriteLine("----------Meny----------");
            Console.WriteLine("1. Skapa konto tryck[S] "); 
            Console.WriteLine("2. Ta bort konto[T]"); 
            Console.WriteLine("3. Visa konton[V]"); 
            Console.WriteLine("4. Hantera konto[H]");

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            Console.Clear();

            switch (char.ToUpper(keyInfo.KeyChar))
            {
                case 'S':
                    Console.WriteLine("Skapa konto:");
                    CreateAccount();
                    break;
                case 'T':
                    Console.WriteLine("Ta bort konto");

                    if (ShowAllAccounts())  // Kolla först om konton finns (bool) innan Ta bort..
                    {
                        RemoveAccountByIndex();
                    }

                    break; 
                case 'V':
                    Console.WriteLine("Visa konto");
                    ShowAllAccounts();
                    break; 
                case 'H':
                    Console.WriteLine("Hantera konto");
                    ManageAccounts();
                    break;
                    default: Console.WriteLine("Fel input!");
                    break;

            }
           
        }


    }


    internal void CreateAccount()
    {
        Console.WriteLine("Vill du skapa:");
        Console.WriteLine("Bankaccount [B]");
        Console.WriteLine("IskAccount [I]");
        Console.WriteLine("UddevallaAccount [U]");

        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
        Console.Clear();

        char choice = char.ToUpper(keyInfo.KeyChar);

        // Kolla så användare väljer rätt bokstav
        if (choice != 'B' && choice != 'I' && choice != 'U')
        {
            Console.WriteLine("Fel val!");
            Console.ReadLine();
            return;
        }

        
        Console.Write("Ange kontonamn: ");
        string name = Console.ReadLine();

        Console.Write("Ange kontonummer (11 siffror): ");
        string number = Console.ReadLine();

        AccountBase newAccount = null;

        try
        {
            switch (choice)
            {
                case 'B':
                    newAccount = new BankAccount(name, number);
                    break;

                case 'I':
                    newAccount = new IskAccount(name, number);
                    break;

                case 'U':
                    newAccount = new UddevallaAccount(name, number);
                    break;
            
            }

        
            AddAccount(newAccount);

            Console.WriteLine("Konto skapat!");
            Console.WriteLine($"Namn: {newAccount.AccountName}");
            Console.WriteLine($"Nummer: {newAccount.AccountNumber}");


        }
        catch (ArgumentException ex) 
        { 
            Console.WriteLine($"Fel: {ex.Message}"); 
        }

        Console.ReadLine();
        Console.Clear();

    }


    internal bool ShowAllAccounts()  //Returnerar boolen Sant/True om konto finns. Eller Falskt/False om inget konto finns
    {

        if (accounts.Count == 0)
        {
            Console.WriteLine("Inga konton finns!");
            return false;
        }

        Console.WriteLine("Alla konton:");

        int index = 1;

        foreach (var item in accounts)
        {
            decimal yearlyInterest = item.CalculateYearlyInterestDaily(2025);

            Console.WriteLine($"[{index}] Kontonamn:{item.AccountName}- Kontonummer: {item.AccountNumber}- Saldo: {item.Balance()} Räntesumma: {yearlyInterest}");

            index++;
        }
        return true;

    }

    internal void RemoveAccountByIndex()
    {
        Console.Write("Välj numret på kontot som ska tas bort: ");
        string input = Console.ReadLine(); 

        if (!int.TryParse(input, out int choice)) 
        { 
            Console.WriteLine("Fel: Du måste skriva ett nummer."); 
            return; 
        }

        int index = choice -1;

        if (index < 0 || index >= accounts.Count)
        {
            Console.WriteLine("Ogiltigt val.");
            return;
        }


        accounts.RemoveAt(index); // Tar bort elementet på den specifika indexplatsen .

        Console.WriteLine("Kontot har tagits bort.");
        Console.ReadLine();
        Console.Clear();


    }

    private void ManageAccounts()
    {

        ShowAllAccounts();
        
        Console.Write("Välj numret på de kontot som du vill hantera. ");
        string input = Console.ReadLine();

        if (!int.TryParse(input, out int choice))
        {
            Console.WriteLine("Fel: Du måste skriva ett nummer.");
            return;

        }
        

        int index = choice - 1;

        if (index < 0 || index >= accounts.Count)
        {
            Console.WriteLine("Ogiltigt val.");
            return;
        }

        AccountBase selectedAccount = accounts[index];


        while (true)
        {
            Console.WriteLine("-------Konto Hanterings Meny-------");
            Console.WriteLine($"Konto: {selectedAccount.AccountName}");
            Console.WriteLine("-----------------------------------");
            Console.WriteLine("1. Insättning tryck[I] "); 
            Console.WriteLine("2. Uttag [U]"); 
            Console.WriteLine("3. Visa Saldo[S]"); 
            Console.WriteLine("4. Avsluta hanterings meny[A]");

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            Console.Clear();

            switch (char.ToUpper(keyInfo.KeyChar))
            {
                case 'I':
                    Console.WriteLine("Ange summa att sätta in:");

                    if (int.TryParse(Console.ReadLine(), out int userDeposit)) 
                    { 
                        selectedAccount.Deposit(userDeposit); 
                    } 
                    else 
                    { 
                        Console.WriteLine("Fel: Du måste skriva en siffra."); 
                    }

                    break;
                case 'U':
                    Console.WriteLine("Ange summa att ta ut");

                    if (int.TryParse(Console.ReadLine(), out int userWithdraw))
                    {
                        selectedAccount.Withdraw(userWithdraw);
                    }
                    else
                    {
                        Console.WriteLine("Fel: Du måste skriva en siffra.");
                    }

                    break;
                case 'S':
                    Console.WriteLine("Saldo:");

                    Console.WriteLine($"{selectedAccount.Balance()}");
                    Console.ReadLine();

                    break;
                case 'A':
                    Console.WriteLine("Avsluta");
                    Console.WriteLine("Åter till huvudmeny...");
                    ShowBankMenu();
                    break;
                default:
                    Console.WriteLine("Fel input!");
                    break;

            }
            
        }

    }




}
