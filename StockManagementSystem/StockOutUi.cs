using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StockManagementSystem.BLL;
using StockManagementSystem.Models;

namespace StockManagementSystem
{
    public partial class StockOutUi : Form
    {
        private DataTable _dataTable;
        private StockOutManager _stockOutManager;
        private Item _item;
        private List<StockOut> _listStockOut;
        private StockOut _stockOut;
        public StockOutUi()
        {
            InitializeComponent();
            _stockOutManager = new StockOutManager();
            _item = new Item();
            _listStockOut = new List<StockOut>();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxCompanyName.Text.Equals("-Select-") || comboBoxCompanyName.Text == "")
                {
                    MessageBox.Show("Select company!");
                }
                if (comboBoxCategoryName.Text.Equals("-Select-") || comboBoxCategoryName.Text == "")
                {
                    MessageBox.Show("Select category!");
                }
                if (comboBoxItemName.Text.Equals("-Select-") || comboBoxItemName.Text == "")
                {
                    MessageBox.Show("Select category!");
                }
                _stockOut = new StockOut();

                //Stockout Quantity textfield checking
                if (String.IsNullOrEmpty(textBoxStockOutQuantity.Text))
                {
                    MessageBox.Show("Enter Stock Out Quantity!");
                }
                if (System.Text.RegularExpressions.Regex.IsMatch(textBoxStockOutQuantity.Text, "[^0-9]"))
                {
                    MessageBox.Show("Enter Digits Only!");
                }

                //avaiability checking
                int quantityOut = Convert.ToInt32(textBoxStockOutQuantity.Text);
                int availableQuantity = Convert.ToInt32(textBoxAvailableQuantity.Text);
                int reorderLevel = Convert.ToInt32(textBoxReorderLevel.Text);
                if (quantityOut > availableQuantity)
                {
                    MessageBox.Show("No Product as Your Order!");
                }
                if ((availableQuantity - quantityOut) <= reorderLevel)
                {
                    MessageBox.Show("Item is Under ReorderLevel!");
                }
                _stockOut.ItemName = comboBoxItemName.Text;
                _stockOut.CompanyName = comboBoxCompanyName.Text;
                _stockOut.Quantity = Convert.ToInt32(textBoxStockOutQuantity.Text);
                _item.Name = comboBoxItemName.Text;
                _item.CategoryID = Convert.ToInt32(comboBoxCategoryName.SelectedValue);
                _item.CompanyID = Convert.ToInt32(comboBoxCompanyName.SelectedValue);
                _dataTable = _stockOutManager.GetAvailableQuantityAndReorderLevelFromItem(_item);
                _stockOut.ItemID = Convert.ToInt32(_dataTable.Rows[0]["ID"].ToString());
                _listStockOut.Add(_stockOut);
                dataGridViewStockOutAllRecord.DataSource = null;
                dataGridViewStockOutAllRecord.DataSource = _listStockOut;

                //Adding SI column
                foreach (DataGridViewRow row in dataGridViewStockOutAllRecord.Rows)
                {
                    row.Cells["SL"].Value = (row.Index + 1).ToString();
                }

                //Preview 
                TextBoxDefaultValue();
                textBoxStockOutQuantity.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TextBoxDefaultValue()
        {
            comboBoxCompanyName.Text = "-Select-";
            comboBoxCategoryName.Text = "-Select-";
            comboBoxItemName.Text = "-Select-";
            textBoxReorderLevel.Text = "<View>";
            textBoxAvailableQuantity.Text = "<View>";
        }

        private void comboBoxCompanyName_Click(object sender, EventArgs e)
        {

            _dataTable = _stockOutManager.DataBindCompanyToComboBox();
            comboBoxCompanyName.DataSource = _dataTable;
            if (_dataTable.Rows.Count == 0)
            {
                MessageBox.Show("Company not available in record!");
            }
        }

        private void comboBoxCategoryName_Click(object sender, EventArgs e)
        {
            _dataTable = _stockOutManager.DataBindCategoryToComboBox();
            comboBoxCategoryName.DataSource = _dataTable;
            if (_dataTable.Rows.Count == 0)
            {
                MessageBox.Show("Category not available in record!");
            }
        }

        private void comboBoxItemName_Click(object sender, EventArgs e)
        {
            textBoxReorderLevel.Text = "";
            textBoxAvailableQuantity.Text = "";
            if (comboBoxCompanyName.Text.Equals("-Select-") || comboBoxCompanyName.Text == "")
            {
                MessageBox.Show("Select company!");
            }
            if (comboBoxCategoryName.Text.Equals("-Select-") || comboBoxCategoryName.Text == "")
            {
                MessageBox.Show("Select category!");
            }
            _dataTable = _stockOutManager.DataBindItemByFilteringToComboBox(Convert.ToInt32(comboBoxCategoryName.SelectedValue), Convert.ToInt32(comboBoxCompanyName.SelectedValue));
            comboBoxItemName.DataSource = _dataTable;
            if (_dataTable.Rows.Count == 0)
            {
                MessageBox.Show("Item not available!");
                comboBoxItemName.Text = "";
            }
        }

        private void comboBoxItemName_SelectedIndexChanged(object sender, EventArgs e)
        {
            _item.Name = comboBoxItemName.Text;
            _item.CategoryID = Convert.ToInt32(comboBoxCategoryName.SelectedValue);
            _item.CompanyID = Convert.ToInt32(comboBoxCompanyName.SelectedValue);
            _dataTable = _stockOutManager.GetAvailableQuantityAndReorderLevelFromItem(_item);
            textBoxReorderLevel.Text = _dataTable.Rows[0]["ReorderLevel"].ToString();
            textBoxAvailableQuantity.Text = _dataTable.Rows[0]["AvailableQuantity"].ToString();
        }

        private void StockOutForm_Load(object sender, EventArgs e)
        {
           TextBoxDefaultValue();
        }

        private void buttonSell_Click(object sender, EventArgs e)
        {
            UpdateStockOut("Sell");
        }

        private void UpdateStockOut(string action)
        {
            try
            {
                _stockOut = new StockOut();
                _stockOut.Date = dateTimePickerStockOut.Value.ToString("yyyy-MM-dd");
                foreach (DataGridViewRow row in dataGridViewStockOutAllRecord.Rows)
                {

                    _stockOut.Quantity = Convert.ToInt32(row.Cells["quantityDataGridViewTextBoxColumn"].Value.ToString());
                    _stockOut.ItemID = Convert.ToInt32(row.Cells["itemIdDataGridViewTextBoxColumn"].Value.ToString());
                    _stockOut.Action = action;
                    _item.ID = _stockOut.ItemID;
                    _dataTable = _stockOutManager.GetItemById(_item);
                    int quantity = Convert.ToInt32(_dataTable.Rows[0]["AvailableQuantity"]);
                    quantity -= _stockOut.Quantity;
                    _item.AvailableQuantity = quantity;
                    _stockOutManager.UpdateItem(_item);
                    int isUpdated = 0;
                    isUpdated = _stockOutManager.AddStockOut(_stockOut);
                    if (isUpdated > 0)
                    {
                        MessageBox.Show("Item: " + comboBoxItemName.Text + " Saved");
                    }
                }
                _listStockOut = new List<StockOut>();
                dataGridViewStockOutAllRecord.DataSource = null;
                dataGridViewStockOutAllRecord.DataSource = _listStockOut;

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }
        }

        private void buttonLost_Click(object sender, EventArgs e)
        {
            UpdateStockOut("Lost");
        }

        private void buttonDamage_Click(object sender, EventArgs e)
        {
            UpdateStockOut("Damage");
        }
    }
}
