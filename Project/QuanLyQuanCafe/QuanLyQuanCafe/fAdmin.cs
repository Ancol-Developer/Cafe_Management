using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
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
        BindingSource foodList = new BindingSource();
        public fAdmin()
        {
            InitializeComponent();
            Load();
        }
        #region Method
        void Load()
        {
            dtgvFood.DataSource = foodList;
            LoadDateTimePickerBill();
            LoadListBillBuyDate(dtpkFromDate.Value, dtpkToDate.Value);
            LoadListFood();
            AddFoodBinding();
            LoadCategoryIntoComboBox(cbFoodCetagory);
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
            txbFoodName.DataBindings.Add(new Binding("Text",dtgvFood.DataSource,"name"));
            txbFoodID.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "ID"));
            nmFoodPrice.DataBindings.Add(new Binding("Value", dtgvFood.DataSource, "price"));
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

        #endregion

        private void txbFoodID_TextChanged(object sender, EventArgs e)
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
    }
}
