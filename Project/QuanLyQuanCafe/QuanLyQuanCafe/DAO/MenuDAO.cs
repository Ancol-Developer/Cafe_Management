using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanCafe.DAO
{
    public class MenuDAO
    {
        private static MenuDAO instance;

        public static MenuDAO Instance {
            get { if (instance == null) instance = new MenuDAO(); return instance; }
            private set => instance = value; }
        private MenuDAO() { }
        public List<MenuFood> GetListMenuByTable(int id)
        {
            List<MenuFood> list = new List<MenuFood>();
            DataTable data = new DataTable();
            string query = "Select f.name,bi.count,f.price,f.price*bi.count as totalPrice from dbo.BillInfo as bi, dbo.Bill as b,dbo.Food as f\r\nWhere bi.idBill = b.id and bi.idFood=f.id and b.status = 0 and b.idTable =" +id;
            data = DataProvider.Instance.ExcuteQuery(query);
            foreach (DataRow item in data.Rows)
            {
                MenuFood menu = new MenuFood(item);
                list.Add(menu);

            }
            return list;
        }
    }
}
