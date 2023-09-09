using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class AccountDAO
    {
        private static AccountDAO instance;
        public static AccountDAO Instance {
            get { if (instance == null) { instance = new AccountDAO(); }; return instance;  }
            private set => instance = value; }
        private AccountDAO()
        {
            
        }
        // using Stor proc han che SQP injection
        public bool Login(string userName,string passWord)
        {
            string query = "Exec USP_Login @userName , @passWord";
            DataTable result = DataProvider.Instance.ExcuteQuery(query, new object[] {userName, passWord});
            return result.Rows.Count>0;
        }
        public Account GetAccountByUserName(string userName)
        {
            DataTable data = DataProvider.Instance.ExcuteQuery("Select * from dbo.Account where userName = '"+ userName + "'" );
            foreach (DataRow row in data.Rows)
            {
                return new Account(row);
            }
            return null;
        }
        public bool UpdateAccount(string userName, string displayName, string pass, string newPass)
        {
            int result = DataProvider.Instance.ExcuteNonQuery("Exec USP_UpdateAccount @userName , @displayName , @password , @newPassword ", new object[] {userName,displayName,pass,newPass});
            return result>0;
        }
        public DataTable GetListAccount()
        {
            return DataProvider.Instance.ExcuteQuery("select Username, DisplayName, Type from dbo.Account");
        }
        public bool InsertAccount(string userName, string displayName, int type)
        {
            string query = string.Format("Insert dbo.Account (Username,DisplayName,Type, Password)values (N'{0}',N'{1}',{2},1)", userName, displayName, type);
            int result = DataProvider.Instance.ExcuteNonQuery(query);
            return result > 0;
        }
        public bool EditAccount(string userName, string displayName, int type)
        {
            string query = string.Format("Update dbo.Account set DisplayName= N'{0}', Type = {1} Where Username = N'{2}'", displayName, type, userName);
            int result = DataProvider.Instance.ExcuteNonQuery(query);
            return result > 0;
        }
        public bool DeleteAccount(string userName)
        {
            string query = string.Format("Delete Account Where Username = N'{0}'", userName);
            int result = DataProvider.Instance.ExcuteNonQuery(query);
            return result > 0;
        }
        public bool ResetPassword(string userName)
        {
            string query = string.Format("Update Account set Password = N'1' Where Username = N'{0}'", userName);
            int result = DataProvider.Instance.ExcuteNonQuery(query);
            return result > 0;
        }
    }
}
