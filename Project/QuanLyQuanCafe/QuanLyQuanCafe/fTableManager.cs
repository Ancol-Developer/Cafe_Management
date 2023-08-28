using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanCafe
{
    public partial class fTableManager : Form
    {
        public fTableManager()
        {
            InitializeComponent();
            LoadTable();
        }
        #region Method
        void LoadTable()
        {
            List<Table> tableList= TableDAO.Instance.LoadTableList();
            foreach (Table items in tableList)
            {
                Button button = new Button()
                {
                    Width = TableDAO.TableWidth,
                    Height= TableDAO.TableHeight
                };
                button.Text = items.Name + Environment.NewLine + items.Status;
                switch (items.Status)
                {
                    case "Trong":
                        button.BackColor = Color.Aqua;
                        break;
                    default: button.BackColor = Color.LightPink; 
                        break;
                }
                flpTable.Controls.Add(button);
                
            }
        }

        #endregion
        #region Event

        private void btnAdd_Click(object sender, EventArgs e)
        {

        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAccountProfile f = new fAccountProfile();
            f.ShowDialog();
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAdmin f = new fAdmin();
            f.ShowDialog();
        }
        #endregion
    }
}
