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
        public bool InsertFood(string name, int id, float price)
        {
            string query = string.Format("Insert dbo.Food (name,idCategory,price)values (N'{0}',{1},{2})",name,id,price);
            int result = DataProvider.Instance.ExcuteNonQuery(query);
            return result > 0;
        }
        public bool UpdateFood(int idFood, string name, int id, float price)
        {
            string query = string.Format("Update dbo.Food set name = N'{0}', idCategory= {1}, price = {2} Where id = {3}", name, id, price,idFood);
            int result = DataProvider.Instance.ExcuteNonQuery(query);
            return result > 0;
        }
        public bool DeleteFood(int idFood)
        {
            BillInfoDAO.Instance.DeleteBillInFoByFoodID(idFood);
            string query = string.Format("Delete Food Where id = {0}",idFood);
            int result = DataProvider.Instance.ExcuteNonQuery(query);
            return result > 0;
        }
    }
}
