using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryApp.Interface
{
    public interface ICategoryRepository : IBaseCategoryRepository
    {
        public int GetCategoryID(string categoryName);
    }
}
