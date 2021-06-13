using ClassLibraryApp.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryApp.Interface
{
    public interface IBaseProductRepository
    {
        Product[] GetAll();
        Product GetProduct(int productID);
        void Add(Product product);
        void Update(Product product);
        void Delete(int productID);
    }
}
