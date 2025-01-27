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
    class CategoryManager
    {
        CategoryRepository _categoryRepository;
        public CategoryManager()
        {
            _categoryRepository = new CategoryRepository();
        }
        public int Update(Category category)
        {
            return _categoryRepository.Update(category);
        }
        public DataTable Display()
        {
            return _categoryRepository.Display();
        }
        public int Insert(Category category)
        {
            return _categoryRepository.Insert(category);
        }
    }
}
