using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class CategoryDAO
    {
        private static CategoryDAO instance;

        public static CategoryDAO Instance 
        {
            get { if (instance == null) instance = new CategoryDAO(); return instance; }
            private set => instance = value; 
        }
        private CategoryDAO()
        {
            
        }
        public List<Category> GetListCategory()
        {
            List<Category> list = new List<Category>();
            string query = "SELECT * FROM dbo.FoodCetagory";
            DataTable data = DataProvider.Instance.ExcuteQuery(query);
            foreach (DataRow row in data.Rows)
            {
                Category category = new Category(row);
                list.Add(category);
            }
            return list;
        }
    }
}
