using NanoERP.API.Data;
using NanoERP.API.Domain.Entities;

namespace NanoERP.API.Services
{
    public class ProductService(DataContext db) : ServiceBase<Product>(db)
    {
        
    }
}