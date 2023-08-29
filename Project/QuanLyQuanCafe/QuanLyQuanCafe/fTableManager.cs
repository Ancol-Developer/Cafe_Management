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
                button.Click += Button_Click;
                button.Tag = items;
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
        void ShowBill(int id)
        {
            lsvBill.Items.Clear();
            List<MenuFood> listBillInfor = MenuDAO.Instance.GetListMenuByTable(id);
            foreach (MenuFood item in listBillInfor)
            {
                ListViewItem lsvItem = new ListViewItem(item.FoodName.ToString());
                lsvItem.SubItems.Add(item.Count.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());
                lsvItem.SubItems.Add(item.TotalPrice.ToString());
                lsvBill.Items.Add(lsvItem);
            }

        }


        #endregion
        #region Event
        private void Button_Click(object sender, EventArgs e)
        {
            int tableID=((sender as Button).Tag as Table).ID;
            ShowBill(tableID);
        }
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
