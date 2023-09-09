using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace QuanLyQuanCafe
{
    public partial class fAdmin : Form
    {
        BindingSource foodList = new BindingSource();
        BindingSource accountList = new BindingSource();
        public Account logionAccount;
        public fAdmin()
        {
            InitializeComponent();
            Load();
        }
        #region Method
        void Load()
        {
            dtgvFood.DataSource = foodList;
            dtgvAccount.DataSource = accountList;
            LoadDateTimePickerBill();
            LoadListBillBuyDate(dtpkFromDate.Value, dtpkToDate.Value);
            LoadListFood();
            LoadAccount();
            LoadCategoryIntoComboBox(cbFoodCetagory);
            AddFoodBinding();
            AddAccountBinding();
            
        }
        List<Foods> SearchFoodByName(string name)
        {
            List<Foods> foods = FoodDAO.Instance.SearchFoodByName(name);
            return foods;
        }
        void LoadDateTimePickerBill()
        {
            DateTime today = DateTime.Now;
            dtpkFromDate.Value = new DateTime(today.Year, today.Month, 1);
            dtpkToDate.Value = dtpkFromDate.Value.AddMonths(1).AddDays(-1); 
        }
        void LoadListBillBuyDate(DateTime checkIn, DateTime checkOut)
        {
            dtgvBill.DataSource = BillDAO.Instance.GetBillListByDate(checkIn, checkOut);
        }
        void LoadListFood()
        {
            // Su dung binding source de ko loi khi an xem van thay doi -- han che mat ket noi Binding
            foodList.DataSource = FoodDAO.Instance.GetListFood();
        }
        void LoadCategoryIntoComboBox(ComboBox cb)
        {
            cb.DataSource = CategoryDAO.Instance.GetListCategory();
            cb.DisplayMember = "Name";
        }
        void AddFoodBinding() 
        {
            // Binding data - thay doi khi click sang cai khac
            txbFoodName.DataBindings.Add(new Binding("Text",dtgvFood.DataSource,"name",true,DataSourceUpdateMode.Never));
            txbFoodID.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "ID", true, DataSourceUpdateMode.Never));
            nmFoodPrice.DataBindings.Add(new Binding("Value", dtgvFood.DataSource, "price", true, DataSourceUpdateMode.Never));
        }
        void AddAccountBinding()
        {
            // Binding Data
            txbUserName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "UserName", true, DataSourceUpdateMode.Never));
            txbDisplayName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "DisplayName", true, DataSourceUpdateMode.Never));
            nmAccountType.DataBindings.Add(new Binding("Value", dtgvAccount.DataSource, "Type", true, DataSourceUpdateMode.Never));

        }
        void LoadAccount()
        {
            accountList.DataSource =AccountDAO.Instance.GetListAccount();
        }
        void AddAccount(string userName, string displayName, int type)
        {
            if (AccountDAO.Instance.InsertAccount(userName,displayName,type))
            {
                MessageBox.Show("Thêm tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Thêm tài khoản thất bại!");
            }
            LoadAccount();
        }
        void EditAccount(string userName, string displayName, int type)
        {
            if (AccountDAO.Instance.EditAccount(userName, displayName, type))
            {
                MessageBox.Show("Cập nhật tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Cập nhật tài khoản thất bại!");
            }
            LoadAccount();
        }
        void DeleteAccount(string userName)
        {
            if (logionAccount.Equals(userName))
            {
                MessageBox.Show("Không thể xóa chính bạn");
                return;
            }
            if (AccountDAO.Instance.DeleteAccount(userName))
            {
                MessageBox.Show("Xóa tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Xóa tài khoản thất bại!");
            }
            LoadAccount();
        }
        void ResetPassword(string userName)
        {
            if (AccountDAO.Instance.ResetPassword(userName))
            {
                MessageBox.Show("Đặt lại mật khẩu thành công");
            }
            else
            {
                MessageBox.Show("Đặt lại mật khẩu thất bại");
            }
        }
        #endregion
        #region Event
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnViewBill_Click(object sender, EventArgs e)
        {
            LoadListBillBuyDate(dtpkFromDate.Value, dtpkToDate.Value);
        }
        private void btnShowFood_Click(object sender, EventArgs e)
        {
            LoadListFood();
        }
        private void btnAddFood_Click(object sender, EventArgs e)
        {
            string name = txbFoodName.Text;
            int idCategory = (cbFoodCetagory.SelectedItem as Category).ID; 
            float price = (float)nmFoodPrice.Value;
            if (FoodDAO.Instance.InsertFood(name,idCategory,price))
            {
                MessageBox.Show("Thêm món thành công");
                LoadListFood();
                if (insertFood!=null)
                {
                    insertFood(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm thức ăn");
            }
        }
        private void txbFoodID_TextChanged(object sender, EventArgs e)
        {
            try 
            {
                // Lay 1 cell trong dtgv
                if (dtgvFood.SelectedCells.Count > 0)
                {
                    int id = (int)dtgvFood.SelectedCells[0].OwningRow.Cells["idCategory"].Value;
                    Category category = CategoryDAO.Instance.GetCategoryById(id);
                    //cbFoodCetagory.SelectedItem = category;
                    int index = -1;
                    int i = 0;
                    foreach (Category item in cbFoodCetagory.Items)
                    {
                        if (item.ID == category.ID)
                        {
                            index = i;
                            break;
                        }
                        i++;
                    }
                    cbFoodCetagory.SelectedIndex = index;
                }

            } 
            catch { }
            
        }
        private void btnEditFood_Click(object sender, EventArgs e)
        {
            string name = txbFoodName.Text;
            int idCategory = (cbFoodCetagory.SelectedItem as Category).ID;
            float price = (float)nmFoodPrice.Value;
            int idFood = Convert.ToInt32(txbFoodID.Text);
            if (FoodDAO.Instance.UpdateFood(idFood,name, idCategory, price))
            {
                MessageBox.Show("Sửa món thành công");
                LoadListFood();
                if (updateFood!=null)
                {
                    updateFood(this,new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa thức ăn");
            }
        }
        private void btnDeleteFood_Click(object sender, EventArgs e)
        {
            int idFood = Convert.ToInt32(txbFoodID.Text);
            if (FoodDAO.Instance.DeleteFood(idFood))
            {
                MessageBox.Show("Xóa món thành công");
                LoadListFood();
                if (deleteFood != null)
                {
                    deleteFood(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Có lỗi khi xóa thức ăn");
            }
        }

        private event EventHandler insertFood;
        public event EventHandler InsertFood
        {
            add { insertFood += value; } remove
            {
                insertFood -= value;
            }
        }
        private event EventHandler deleteFood;
        public event EventHandler DeleteFood
        {
            add { deleteFood += value; }
            remove { deleteFood -= value; }
        }
        private event EventHandler updateFood;
        public event EventHandler UpdateFood
        {
            add { updateFood += value; }
            remove
            {
                updateFood -= value;
            }
        }
        private void btnSearchFood_Click(object sender, EventArgs e)
        {
            foodList.DataSource = SearchFoodByName(txbSearchFoodName.Text);
        }
        private void btnShowAccount_Click(object sender, EventArgs e)
        {
            LoadAccount();
        }
        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;
            string displayName = txbDisplayName.Text;
            int type = (int)(nmAccountType.Value);
            AddAccount(userName,displayName,type);
        }

        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;
            DeleteAccount(userName);
        }

        private void btnEditAccount_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;
            string displayName = txbDisplayName.Text;
            int type = (int)(nmAccountType.Value);
            EditAccount(userName, displayName, type);
        }
        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;
            ResetPassword(userName);
        }


        #endregion


    }
}
