using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class Bill
    {
        private int discount;
        private int status;
        public int Status
        {
            get { return status; } 
            set
            {
                status = value;
            }
        }
        private DateTime? dateCheckIn;
        public DateTime? DateCheckIn
        {
            get { return dateCheckIn; }
            set { dateCheckIn = value; }
        }
        private DateTime? dateCheckOut;
        public DateTime? DateCheckOut
        {
            get { return dateCheckOut; }
            set { dateCheckOut = value; }
        }
        private int iD;
        public int ID
        {
            get { return iD; }
            set { iD = value; }
        }

        public int Discount { get => discount; set => discount = value; }

        public Bill(int id, DateTime? dateCheckIn,DateTime? dateCheckOut, int statut, int discount=0)
        {
            this.ID = id;
            this.DateCheckIn = dateCheckIn;
            this.DateCheckOut = dateCheckOut;
            this.Status = status;
            this.Discount = discount;
        }
        public Bill(DataRow row)
        {
            this.ID = (int)row["ID"];
            this.DateCheckIn = (DateTime?)row["dateCheckIn"];

            var dateCheckOutTemp= row["dateCheckOut"];
            if (dateCheckOutTemp.ToString() != string.Empty)
            {
                this.DateCheckOut = (DateTime?)dateCheckOutTemp;
            }
            this.Status = (int)row["status"];
            if (row["discount"].ToString()!= string.Empty)
            {
                this.Discount = (int)row["discount"];
            }
            
        }
    }
}
