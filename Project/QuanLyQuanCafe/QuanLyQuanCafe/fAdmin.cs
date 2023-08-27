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
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        void LoadAccountList()
        {
            string query = "Exec dbo.USP_GetAccountByUserName @userName";
            DataProvider dataProvider = new DataProvider();
            dtgvAccount.DataSource= dataProvider.ExcuteQuery(query,"K9");
        }
    }
}
