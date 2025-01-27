using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StockManagementSystem.Models;
using StockManagementSystem.BLL;


namespace StockManagementSystem
{
    public partial class StockInUi : Form
    {
        private StockInManager _stockInManager;
        private StockIn _stockIn;
        private Item _item;
        private DataTable _dataTable;
        public StockInUi()
        {
            InitializeComponent();
            _stockIn = new StockIn();
            _stockInManager = new StockInManager();
            _item = new Item();
        }

        private void StockInForm_Load(object sender, EventArgs e)
        {
            _dataTable = _stockInManager.CompanyDataBindToCombobox();
            comboBoxCompanyName.DataSource = _dataTable;
            if (_dataTable.Rows.Count == 0)
            {
                comboBoxCompanyName.Text = "";
                MessageBox.Show("Company not available in record!");
            }
            //category      
            _dataTable = _stockInManager.CategoryDataBindToCombobox();
            comboBoxCategoryName.DataSource = _dataTable;
            if (_dataTable.Rows.Count == 0)
            {
                comboBoxCompanyName.Text = "";
                MessageBox.Show("Category not available in record!");
            }
            TextBoxDefaultValue();
            DisplayAllRecords();
        }

        private void TextBoxDefaultValue()
        {
            comboBoxCompanyName.Text = "-Select-";
            comboBoxCategoryName.Text = "-Select-";
            comboBoxItemName.Text = "-Select-";
            textBoxReorderLevel.Text = "<View>";
            textBoxAvailableQuantity.Text = "<View>";
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (buttonSave.Text.Equals("Confirm"))
                {
                    int updateQuantity = Convert.ToInt32(textBoxStockInQuantity.Text);
                    _item.AvailableQuantity += updateQuantity;
                    _stockIn.Quantity = updateQuantity;
                    _stockInManager.UpdateAvailableQuantity(_item);
                    _stockInManager.UpdateStockIn(_stockIn);
                    buttonSave.Text = "Save";
                }
                else
                {
                    
                    if (comboBoxCompanyName.Text.Equals("-Select-"))
                    {
                        MessageBox.Show("Enter a company!");
                        return;
                    }
                    if (comboBoxCategoryName.Text.Equals("-Select-"))
                    {
                        MessageBox.Show("Enter a category");
                        return;
                    }
                    if (comboBoxItemName.Text.Equals("-Select-"))
                    {
                        MessageBox.Show("Enter an Item");
                        return;
                    }
                    _stockIn.Date = dateTimePickerUserDefineDate.Value.ToString("yyyy-MM-dd");
                    _stockIn.ItemID = Convert.ToInt32(comboBoxItemName.SelectedValue);
                    if (String.IsNullOrEmpty(textBoxStockInQuantity.Text))
                    {
                        MessageBox.Show("Enter a quantity!");
                        return;
                    }
                    _stockIn.Quantity = Convert.ToInt32(textBoxStockInQuantity.Text);
                    AddStockIn(_stockIn);

                    //update avaiableQuantity
                    _item.ID = _stockIn.ID;
                    _dataTable = _stockInManager.GetItemById(_item);
                    int currentQuantity = Convert.ToInt32(_dataTable.Rows[0]["AvailableQuantity"]);
                    _item.AvailableQuantity = currentQuantity + _stockIn.Quantity;
                    _stockInManager.UpdateAvailableQuantity(_item);

                }


                TextBoxDefaultValue();
                textBoxStockInQuantity.Text = "";

                DisplayAllRecords();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AddStockIn(StockIn stockIn)
        {
            int isExecuted = 0;
            isExecuted = _stockInManager.AddtStockIn(stockIn);
            if (isExecuted > 0)
            {
                MessageBox.Show("Stock in save successfully.");
            }
            else
            {
                MessageBox.Show("Stock in save failed!");
            }
        }

        private void DisplayAllRecords()
        {
            _dataTable = _stockInManager.DisplayAllRecords();
            dataGridViewStockInAllRecord.DataSource = _dataTable;
            if (_dataTable.Rows.Count == 0)
            {
                MessageBox.Show("No any records");
            }
            foreach (DataGridViewRow row in dataGridViewStockInAllRecord.Rows)
            {
                row.Cells["SL"].Value = (row.Index + 1).ToString();
                row.Cells["Action"].Value = Convert.ToString("Edit");
            }
        }

        private void comboBoxCompanyName_Click(object sender, EventArgs e)
        {
            _item.CompanyID = Convert.ToInt32(comboBoxCompanyName.SelectedValue);
        }

        private void comboBoxCategoryName_Click(object sender, EventArgs e)
        {
            if (comboBoxCompanyName.Text.Equals("--Select--") || comboBoxCompanyName.Text == null)
            {
                MessageBox.Show("Select company!");
                return;
            }
            _item.CategoryID = Convert.ToInt32(comboBoxCategoryName.SelectedValue);
        }

        private void comboBoxItemName_Click(object sender, EventArgs e)
        {
            if (comboBoxCategoryName.Text.Equals("--Select--") || comboBoxCategoryName.Text == null)
            {
                MessageBox.Show("Select category!");
                return;
            }
            _dataTable = _stockInManager.ItemDataBindToComboboxByFiltering(Convert.ToInt32(comboBoxCategoryName.SelectedValue), Convert.ToInt32(comboBoxCompanyName.SelectedValue));
            comboBoxItemName.DataSource = _dataTable;
            if (_dataTable.Rows.Count == 0)
            {
                comboBoxItemName.Text = "";
                MessageBox.Show("Item Not found");
            }
        }

        private void comboBoxItemName_SelectedIndexChanged(object sender, EventArgs e)
        {
            _dataTable = _stockInManager.GetAvailableQuantityAndReorderLevel(Convert.ToInt32(comboBoxCategoryName.SelectedValue), Convert.ToInt32(comboBoxCompanyName.SelectedValue), comboBoxItemName.Text);
            if (_dataTable.Rows.Count > 0)
            {
                textBoxReorderLevel.Text = _dataTable.Rows[0]["ReorderLevel"].ToString();
                textBoxAvailableQuantity.Text = _dataTable.Rows[0]["AvailableQuantity"].ToString();
            }
            //Display on DataGridView
            DisplayAllRecords();
        }

        private void dataGridViewStockInAllRecord_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int currentQuantity = Convert.ToInt32(dataGridViewStockInAllRecord.Rows[e.RowIndex].Cells["dataGridViewTextBoxColumn2"].Value);
            _item.ID = Convert.ToInt32(dataGridViewStockInAllRecord.Rows[e.RowIndex].Cells["dataGridViewTextBoxColumn3"].Value);
            _dataTable = _stockInManager.GetItemById(_item);

            //display into textbox
            if (_dataTable.Rows.Count > 0)
            {
                comboBoxCompanyName.SelectedValue = _dataTable.Rows[0]["CompanyID"];
                comboBoxCategoryName.SelectedValue = _dataTable.Rows[0]["CategoryID"];
                comboBoxItemName.Text = _dataTable.Rows[0]["Name"].ToString();
                textBoxReorderLevel.Text = _dataTable.Rows[0]["ReorderLevel"].ToString();
                textBoxAvailableQuantity.Text = _dataTable.Rows[0]["AvailableQuantity"].ToString();
            }
            textBoxStockInQuantity.Text = currentQuantity.ToString();
            buttonSave.Text = "Confirm";

            //updating for item     
            _item.AvailableQuantity = Convert.ToInt32(textBoxAvailableQuantity.Text);
            _item.AvailableQuantity -= currentQuantity; //reduce currentQuantity

            //updating for StockIn
            _stockIn.ID = Convert.ToInt32(dataGridViewStockInAllRecord.Rows[e.RowIndex].Cells["iDDataGridViewTextBoxColumn"].Value);
        }



    }
}
