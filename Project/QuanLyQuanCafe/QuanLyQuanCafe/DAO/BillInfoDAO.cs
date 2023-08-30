using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class BillInfoDAO
    {
        private static BillInfoDAO instance;

        public static BillInfoDAO Instance {
            get { if (instance == null) instance = new BillInfoDAO(); return instance; } 
            private set => instance = value; 
        }
        private BillInfoDAO() { }
        /// <summary>
        /// Get List bill Infor by bill ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<BillInfo> GetListInfor(int id)
        {
            List<BillInfo> list = new List<BillInfo>();

            DataTable data = DataProvider.Instance.ExcuteQuery("Select * from dbo.BillInfo where idBill = "+id);
            foreach (DataRow item in data.Rows)
            {
                BillInfo info = new BillInfo(item);
                list.Add(info);
            }
            return list;
        }
        public void InsertBillInfor(int idBill,int idFood,int count)
        {
            DataProvider.Instance.ExcuteNonQuery("USP_InsertBillInfo @idBill , @idFood , @count", new object[] {idBill,idFood,count});
        }
    }
}
