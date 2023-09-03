
using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class TableDAO
    {
        private static TableDAO instance;
        public static TableDAO Instance { 
            get { if (instance==null) { instance = new TableDAO(); } return instance; }  
            private set => instance = value; 
        }
        public static int TableWidth = 90;
        public static int TableHeight = 90;
        private TableDAO()
        {

        }
        public List<Table> LoadTableList()
        {
            List<Table> list = new List<Table>();
            DataTable data = DataProvider.Instance.ExcuteQuery("USP_GetTableList");
            foreach (DataRow row in data.Rows)
            {
                Table table = new Table(row);
                list.Add(table);
            }
            return list;
        }
        public void SwitchTable(int idtable1, int idtable2)
        {
            string query = "Exec USP_SwitchTable @idTable1 , @idTable2";
            DataProvider.Instance.ExcuteQuery(query, new object[] { idtable1, idtable2 });
        }
    }
}
