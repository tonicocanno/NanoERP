using NanoERP.API.Domain.interfaces;

namespace NanoERP.API.Domain.Entities
{
    public class Partner : MasterData, IEntity
    {
        public PartnerType Type { get; set; }
        public string? Phone1 { get; set; }
        public string? Phone2 { get; set; }
        public string? Email { get; set; }
        public string? TaxId0 { get; set; }
        public string? TaxId1 { get; set; }
        public string? TaxId2 { get; set; }
        public ICollection<PartnerAddress>? Addresses { get; set; }

        public void AddAddress(PartnerAddress address)
        {
            Addresses ??= [];
            Addresses.Add(address);
        }

        public void RemoveAddress(PartnerAddress address)
        {
            Addresses?.Remove(address);
        }

        public void RemoveAddress(string addressId)
        {
            var address = Addresses?.FirstOrDefault(a => a.Id.ToString() == addressId);
            if (address != null)
            {
                Addresses?.Remove(address);
            }
        }
    }

    public enum PartnerType
    {
        Customer = 1,
        Supplier = 2,
    }
}
