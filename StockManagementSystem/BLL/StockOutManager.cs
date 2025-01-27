using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockManagementSystem.Models;
using StockManagementSystem.Repository;

namespace StockManagementSystem.BLL
{
    public class StockOutManager
    {
        private StockOutRepository _stockOutRepository = new StockOutRepository();

        public DataTable DataBindCompanyToComboBox()
        {
            return _stockOutRepository.DataBindCompanyToComboBox();
        }

        public DataTable DataBindCategoryToComboBox()
        {
            return _stockOutRepository.DataBindCategoryToComboBox();
        }

        public DataTable DataBindCompanyByFilteringToComboBox(int categoryId)
        {
            return _stockOutRepository.DataBindCompanyByFilteringToComboBox(categoryId);
        }

        public DataTable DataBindCategoryByFilteringToComboBox(int companyId)
        {
            return _stockOutRepository.DataBindCategoryByFilteringToComboBox(companyId);
        }

        public DataTable DataBindItemByFilteringToComboBox(int categoryId, int companyId)
        {
            return _stockOutRepository.DataBindItemByFilteringToComboBox(categoryId, companyId);
        }

        public DataTable GetAvailableQuantityAndReorderLevelFromItem(Item item)
        {
            return _stockOutRepository.GetAvailableQuantityAndReorderLevelFromItem(item);
        }

        public int AddStockOut(StockOut stockOut)
        {
            return _stockOutRepository.AddStockOut(stockOut);
        }

        public DataTable GetItemById(Item item)
        {
            return _stockOutRepository.GetItemById(item);
        }

        public void UpdateItem(Item item)
        {
            _stockOutRepository.UpdateItem(item);
        }
    }
}
