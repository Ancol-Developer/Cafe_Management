using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class FoodDAO
    {
        private static FoodDAO instance;
        public static FoodDAO Instance {
            get { if (instance == null) instance = new FoodDAO(); return instance; }
            private set => instance = value; 
        }
        private FoodDAO()
        {
            
        }
        public List<Foods> GetFooByCategoryID(int id)
        {
            List<Foods> list = new List<Foods>();
            string query = "Select * FROM dbo.Food Where idCategory = "+id;
            DataTable data = DataProvider.Instance.ExcuteQuery(query);
            foreach (DataRow row in data.Rows)
            {
                Foods foods = new Foods(row);
                list.Add(foods);
            }
            return list;
        }
        public List<Foods> GetListFood()
        {
            List<Foods> list = new List<Foods>();
            string query = " Select * From dbo.Food";
            DataTable data = DataProvider.Instance.ExcuteQuery(query);
            foreach(DataRow row in data.Rows)
            {
                Foods food = new Foods(row);
                list.Add(food);
            }
            return list;
        }
    }
}
