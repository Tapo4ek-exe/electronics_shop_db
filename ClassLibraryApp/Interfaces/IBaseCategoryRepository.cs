using ClassLibraryApp.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryApp.Interface
{
    public interface IBaseCategoryRepository
    {
        Category[] GetAll();
        Category GetCategory(int categoryID);
        void Add(Category category);
        void Update(Category category);
        void Delete(int categoryID);
    }
}
