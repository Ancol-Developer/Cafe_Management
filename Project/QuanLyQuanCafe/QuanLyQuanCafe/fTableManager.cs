using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
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
            LoadCategory();
        }
        #region Method
        void LoadTable()
        {
            // clear controls --- tranh add ban lien tuc
            flpTable.Controls.Clear();
            List<Table> tableList= TableDAO.Instance.LoadTableList();
            foreach (Table items in tableList)
            {
                Button button = new Button()
                {
                    Width = TableDAO.TableWidth,
                    Height= TableDAO.TableHeight
                };
                button.Text = items.Name + Environment.NewLine + items.Status;
                // Add button click cho table
                button.Click += Button_Click;
                button.Tag = items;
                // doi mau ban 'trong' va 'co nguoi'
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
            //  Xoa bill cu tranh add nhieu bill tren cung 1 listView
            lsvBill.Items.Clear();
            List<MenuFood> listBillInfor = MenuDAO.Instance.GetListMenuByTable(id);
            float totalPrice = 0;
            // add field tto lsvItem
            foreach (MenuFood item in listBillInfor)
            {
                ListViewItem lsvItem = new ListViewItem(item.FoodName.ToString());
                lsvItem.SubItems.Add(item.Count.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());
                lsvItem.SubItems.Add(item.TotalPrice.ToString());
                totalPrice += item.TotalPrice;
                lsvBill.Items.Add(lsvItem);
            }
            // chuyen doi vung ve VietNam- vnd
            CultureInfo culture = new CultureInfo("vi-VN");
            //Thread.CurrentThread.CurrentCulture = culture;
            txbTotalPrice.Text = totalPrice.ToString("c",culture);
        }
        void LoadCategory()
        {
            List<Category> listCategory = CategoryDAO.Instance.GetListCategory();
            cbCategory.DataSource = listCategory;
            cbCategory.DisplayMember = "Name";
        }
        void LoadFoodListByCategoryID(int id)
        {
            List<Foods> listFood= FoodDAO.Instance.GetFooByCategoryID(id);
            cbFood.DataSource = listFood;
            cbFood.DisplayMember = "Name";
        }


        #endregion
        #region Event
        private void Button_Click(object sender, EventArgs e)
        {
            int tableID=((sender as Button).Tag as Table).ID;
            lsvBill.Tag = (sender as Button).Tag;
            ShowBill(tableID);
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {   
            Table table = lsvBill.Tag as Table;
            int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
            int idFood = (cbFood.SelectedItem as Foods).ID;
            int count = (int)nmFoodCount.Value;
            if (idBill == -1) // neu ko ton tai bill
            {
                BillDAO.Instance.InsertBill(table.ID);
                BillInfoDAO.Instance.InsertBillInfor(BillDAO.Instance.GetMaxBill(),idFood,count);
            }
            else
            {
                BillInfoDAO.Instance.InsertBillInfor(idBill, idFood, count);
            }
            // reload button
            ShowBill(table.ID);
            // Load lai table sau khi add mon -- hien thi ban 'trong' ban 'co nguoi'
            LoadTable();
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
        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = 0;
            ComboBox cb = sender as ComboBox;
            if (cb.SelectedItem == null)
            {
                return;
            }
            Category selected = cb.SelectedItem as Category;
            id = selected.ID;
            LoadFoodListByCategoryID(id);
        }
        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            Table table= lsvBill.Tag as Table;
            int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
            int discount = (int)nmDiscount.Value;
            double totalPrice = Convert.ToDouble(txbTotalPrice.Text.Split(',')[0]);
            double finalTotalPrice = totalPrice - (totalPrice/100)*discount;
            if (idBill!=-1)
            {
                if(MessageBox.Show(string.Format("Bạn có chắc chắn thanh toán cho bàn {0}\n Tổng tiền - (Tổng tiền / 100)  x Giảm giá \n = {1} - ({1}/100) x {2} = {3}",table.Name,totalPrice,discount,finalTotalPrice),"Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    BillDAO.Instance.CheckOut(idBill,discount);
                    ShowBill(table.ID);
                    // Load lai table sau khi thanh toan -- hien thi ban 'trong' ban 'co nguoi'
                    LoadTable();
                }
            }
        }

        #endregion


    }
}
