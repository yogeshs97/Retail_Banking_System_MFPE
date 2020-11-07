using Account_Microservice.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account_Microservice.Repository
{
   

    public class AccountRepository : IAccountRepository
    {
       public static DateTime d;
       public static int checkno= 78;
        readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(AccountRepository));

        public static  List<Account> accounts = new List<Account>() { 
            new Account(){AccountId=1,CustomerId=1,Balance=1000,AccountType="Savings",minBalance=1000 },
            new Account(){AccountId=2,CustomerId=1,Balance=1000,AccountType="Current",minBalance=0 },
            new Account(){AccountId=3,CustomerId=2,Balance=1000,AccountType="Savings",minBalance=1000 },
            new Account(){AccountId=4,CustomerId=2,Balance=1000,AccountType="Current",minBalance=0 }
        };

         public static List<Statement> statements = new List<Statement>() { 
       new Statement(){ StatementId=1,AccountId=1,date=Convert.ToDateTime("2020-10-3T13:00:15"),refno="Ref75",ValueDate=Convert.ToDateTime("2020-10-3T13:00:15"),Withdrawal=0,Deposit=200,ClosingBalance=1200}, 
       new Statement(){ StatementId=2,AccountId=1,date=Convert.ToDateTime("2020-10-4T13:00:15"),refno="Ref76",ValueDate=Convert.ToDateTime("2020-10-4T13:00:15"),Withdrawal=100,Deposit=0,ClosingBalance=1100}, 
       new Statement(){ StatementId=3,AccountId=2,date=Convert.ToDateTime("2020-10-5T13:00:15"),refno="Ref77",ValueDate=Convert.ToDateTime("2020-10-5T13:00:15"),Withdrawal=0,Deposit=600,ClosingBalance=1600}, 
       new Statement(){ StatementId=4,AccountId=2,date=Convert.ToDateTime("2020-10-6T13:00:15"),refno="Ref78",ValueDate=Convert.ToDateTime("2020-10-6T13:00:15"),Withdrawal=200,Deposit=0,ClosingBalance=1400}
         };
      



        /// <summary>
        /// this method takes customer id and account type and creates account
        /// </summary>
        /// <param name="CId"></param>
        /// <param name="AType"></param>
        /// <returns></returns>
     public AccountCreationStatus AddAccount(int CId, string AType)
        {
            Account a = new Account();
           
                
                a.AccountId = accounts.Count + 1;
                a.CustomerId = CId;
                a.Balance = 1000;
                a.AccountType = AType;
                if (AType == "Savings")
                {
                    a.minBalance = 1000;
                }
                else
                {
                    a.minBalance = 0;
                }
           
            accounts.Add(a);

            return new AccountCreationStatus() { Message = "Account has been successfully created", AccountId = a.AccountId };
        }


        /// <summary>
        /// this method takes account id and amount and add amount to given account id
        /// </summary>
        /// <param name="AId"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public TransactionStatus depositAccount(int AId, int amount)
        {
            int c = 0;
            int sbalance=0, dbalance = 0;
            try
            {
                foreach (var item in accounts)
                {
                    if (item.AccountId == AId)
                    {
                        c = 1;
                        sbalance = item.Balance;
                        item.Balance = item.Balance + amount;
                        dbalance = item.Balance;

                        Statement s = new Statement();

                        s.StatementId = statements.Count + 1;
                        s.AccountId = AId;
                        d = DateTime.Now;
                        s.date = d;

                        checkno = checkno+1;
                        s.refno = "Ref" + Convert.ToString(checkno);
                        s.ValueDate = d;
                        s.Withdrawal = 0;
                        s.Deposit = amount;
                        s.ClosingBalance = dbalance;
                        statements.Add(s);

                        d = s.date;
                        break;
                    }
                }

                if (c == 1)
                {
                    return new TransactionStatus() { Message = "Your account has been credited", source_balance = sbalance, destination_balance = dbalance };

                }
                else
                {
                    throw new System.ArgumentNullException("Account id is invalid "+AId);
                  
                }
            }
            catch(Exception e)
            {
                _log4net.Error(e.Message);
                throw e;
            }
                

        }




        /// <summary>
        /// this method takes cutomerid and returns the all accounts of him
        /// </summary>
        /// <param name="CId"></param>
        /// <returns></returns>
        public IEnumerable<Account> getAllAccounts(int CId)
        {
           
            List<Account> li = new List<Account>();
            try
            {
                foreach (var item in accounts)
                {
                    if (item.CustomerId == CId)
                    {
                        li.Add(item);
                    }
                }
                if (li.Count == 0)
                {
                    throw new System.ArgumentNullException("nothing in the list for this customer id  "+CId);
                }
            }
            catch(Exception e)
            {
                _log4net.Error(e.Message);
                throw e;
            }
            
            return li;
        }

        /// <summary>
        /// this account account id and returns the account
        /// </summary>
        /// <param name="AId"></param>
        /// <returns></returns>
        public Account getAccount(int AId)
        {
           
            Account a = new Account();
            try
            {
                foreach (var item in accounts)
                {
                    if (item.AccountId == AId)
                    {
                      
                        a = item;
                    }
                }

                if (a.AccountId == 0)
                {
                    throw new System.ArgumentNullException("No such accunt is stored");
                }
            }
            catch(Exception e)
            {
                _log4net.Error(e.Message);
                throw e;
            }
           
            return a;
        }


        /// <summary>
        /// this method gets account id and from date and to date and returns the statements of given account id
        /// </summary>
        /// <param name="AId"></param>
        /// <param name="from_date"></param>
        /// <param name="to_date"></param>
        /// <returns></returns>
        public IEnumerable<Statement> getStatement(int AId, DateTime from_date, DateTime to_date)
        {
            List<Statement> li = new List<Statement>();
            try
            {
                DateTime tempDate = Convert.ToDateTime("1/1/0001 12:00:00 AM");
               
                if (from_date == tempDate)
                {


                    DateTime f = DateTime.Now, t = DateTime.Now;
                    int month = f.Month, year = f.Year;
                    string ds = "1/", ms = Convert.ToString(month) + "/", ys = Convert.ToString(year), ts = " 12:00:00 AM", fs = ds + ms + ys + ts;
                    f = Convert.ToDateTime(fs);
                    foreach (var item in statements)
                    {
                        if (item.AccountId == AId && item.date >= f && item.date <= t)
                        {
                            li.Add(item);
                        }
                    }
                }
                else
                {
                    foreach (var item in statements)
                    {
                        if (item.AccountId == AId && item.date >= from_date && item.date <= to_date)
                        {
                            li.Add(item);
                        }
                    }

                }
                if (li.Count == 0)
                {
                    throw new System.ArgumentNullException("No statement for account id: "+AId);
                }
            }
            catch(Exception e)
            {
                _log4net.Info(e.Message);
                throw e;
            }

            return li;
        }


        /// <summary>
        /// this method takes the account id and amount and withdraw the money
        /// </summary>
        /// <param name="AId"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public TransactionStatus withdrawAccount(int AId, int amount)
        {
            
            int c = 0;
            int sbalance = 0, dbalance = 0;
            try
            {
                foreach (var item in accounts)
                {
                    if (item.AccountId == AId)
                    {
                        c = 1;
                        sbalance = item.Balance;
                        item.Balance = item.Balance - amount;
                        dbalance = item.Balance;

                        Statement s = new Statement();

                        s.StatementId = statements.Count + 1;
                        s.AccountId = AId;
                        d = DateTime.Now;
                        s.date = d;
                        checkno = checkno + 1;
                        s.refno = "Ref" + Convert.ToString(checkno);
                        s.ValueDate = d;
                        s.Withdrawal = amount;
                        s.Deposit = 0;
                        s.ClosingBalance = dbalance;
                        statements.Add(s);

                        d = s.date;
                        break;
                    }
                }

                if (c == 1)
                {
                    return new TransactionStatus() { Message = "Your account has been debited", source_balance = sbalance, destination_balance = dbalance };

                }
                else
                {

                    throw new System.ArgumentNullException("Account id is invalid " + AId);
                }
            }
           
            catch (Exception e)
            {
                _log4net.Error(e.Message);
                throw e;
            }
        }


        /// <summary>
        /// this method returns the all accounts
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Account>getCustomersAllAccounts()
        {
            return accounts;
        }

    }
}
