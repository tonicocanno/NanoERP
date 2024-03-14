using NanoERP.API.Data;
using NanoERP.API.Domain.Entities;

namespace NanoERP.API.Services
{
    public class PartnerService(DataContext db) : ServiceBase<Partner>(db)
    {
        
    }
}