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
    public class StockInManager
    {
        private StockInRepository _stockInRepository = new StockInRepository();

        public DataTable CompanyDataBindToCombobox()
        {
            return _stockInRepository.CompanyDataBindToCombobox();
        }

        public DataTable CategoryDataBindToCombobox()
        {
            return _stockInRepository.CategoryDataBindToCombobox();
        }

        public DataTable ItemDataBindToComboboxByFiltering(int categoryId, int companyId)
        {
            return _stockInRepository.ItemDataBindToComboboxByFiltering(categoryId, companyId);
        }

        public int AddtStockIn(StockIn stockIn)
        {
            return _stockInRepository.AddtStockIn(stockIn);
        }

        public void UpdateStockIn(StockIn stockIn)
        {
            _stockInRepository.UpdateStockIn(stockIn);
        }

        public DataTable DisplayAllRecords()
        {
            return _stockInRepository.DisplayAllRecords();
        }

        public DataTable GetAvailableQuantityAndReorderLevel(int categoryId, int companyId, string itemName)
        {
            return _stockInRepository.GetAvailableQuantityAndReorderLevel(categoryId, companyId, itemName);
        }

        public void UpdateAvailableQuantity(Item item)
        {
            _stockInRepository.UpdateAvailableQuantity(item);
        }

        public DataTable GetItemById(Item item)
        {
            return _stockInRepository.GetItemById(item);
        }
    }
}
