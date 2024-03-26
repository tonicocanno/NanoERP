using MongoDB.Bson;
using NanoERP.API.Data;
using NanoERP.API.Domain.Entities;

namespace NanoERP.API.Services
{
    public class PartnerService(DataContext db) : ServiceBase<Partner>(db)
    {
        public async Task<PartnerAddress> AddAddressAsync(Partner partner, PartnerAddress address)
        {
            address.Id = ObjectId.GenerateNewId();

            db.PartnerAddresses.Add(address);
            partner.AddAddress(address);

            await UpdateAsync(partner);
            return address;
        }

        public async Task RemoveAddressAsync(Partner partner, PartnerAddress address)
        {
            partner.RemoveAddress(address);
            await UpdateAsync(partner);
        }

        public async Task RemoveAddressAsync(Partner partner, string addressId)
        {
            partner.RemoveAddress(addressId);
            await UpdateAsync(partner);
        }
    }
}