﻿using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class BillDAO
    {
        private static BillDAO instance;

        public static BillDAO Instance {
            get { if (instance == null) instance = new BillDAO();return instance; } 
            private set => instance = value; }
        private BillDAO()
        {
            
        }
        /// <summary>
        /// Thành công : bill ID
        /// Thất bại: -1
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int GetUncheckBillIDByTableID(int id)
        {
            DataTable data= DataProvider.Instance.ExcuteQuery("SELECT * From dbo.Bill where idTable = "+ id + " and status = 0");
            if (data.Rows.Count>0)
            {
                Bill bill = new Bill(data.Rows[0]);
                return bill.ID;

            }
            return -1;

        }
        public void InsertBill(int id)
        {
            DataProvider.Instance.ExcuteNonQuery("Exec USP_InsertBill @idTable", new object[] {id});
        }
        public int GetMaxBill()
        {
            try
            {
                return (int)DataProvider.Instance.ExcuteScalar("Select Max(id) from dbo.Bill");
            }
            catch 
            { 
                return 1; 
            }
            
        }
        public void CheckOut(int id, int discount, float totalPrice)
        {
            string query = "Update dbo.Bill Set DateCheckOut = GetDate() , status = 1, discount = "+ discount+", totalPrice = "+ totalPrice +" where id = " + id;
            DataProvider.Instance.ExcuteNonQuery(query);
        } 
        public DataTable GetBillListByDate(DateTime dateCheckIn, DateTime dateCheckOut)
        {
            return DataProvider.Instance.ExcuteQuery("Exec USP_GetListBillByDate @checkIn , @checkout", new object[] {dateCheckIn,dateCheckOut});
        }
        public DataTable GetBillListByDateAndPage(DateTime dateCheckIn, DateTime dateCheckOut,int pageNum)
        {
            return DataProvider.Instance.ExcuteQuery("Exec USP_GetListBillByDateAndPage @checkIn , @checkout , @page", new object[] { dateCheckIn, dateCheckOut, pageNum });
        }
        public int GetNumBillListByDate(DateTime dateCheckIn, DateTime dateCheckOut)
        {
            return (int)DataProvider.Instance.ExcuteScalar("Exec USP_GetNumBillByDate @checkIn , @checkout", new object[] { dateCheckIn, dateCheckOut });
        }
    }
}
