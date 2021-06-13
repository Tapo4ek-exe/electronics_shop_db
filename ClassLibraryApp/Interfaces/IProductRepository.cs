using ClassLibraryApp.Table;

namespace ClassLibraryApp.Interface
{
    public interface IProductRepository : IBaseProductRepository
    {
        Product[] allProductsInCategory(int categoryID);

        Product[] allProductsOfCompany(string companyName);
    }
}
