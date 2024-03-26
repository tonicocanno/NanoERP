using MongoDB.Bson;

namespace NanoERP.API.Domain.Entities
{
    public class PartnerAddress : MasterData
    {
        public string Street { get; set; } = null!;
        public string Number { get; set; } = null!;
        public string City { get; set; } = null!;
        public string State { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
    }
}
