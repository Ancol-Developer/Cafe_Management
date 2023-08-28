using QuanLyQuanCafe.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanCafe
{
    public partial class fAdmin : Form
    {
        public fAdmin()
        {
            InitializeComponent();
            LoadAccountList();
            LoadFoodList();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        void LoadFoodList()
        {
            string query = "Select * from dbo.Food";
            dtgvFood.DataSource = DataProvider.Instance.ExcuteQuery(query);
        }
        void LoadAccountList()
        {
            string query = "Exec dbo.USP_GetAccountByUserName @userName";
            dtgvAccount.DataSource= DataProvider.Instance.ExcuteQuery(query,new object[] {"staff"});
        }
    }
}
