namespace NanoERP.API.Domain.Entities
{
    public class Partner : MasterData
    {
        public PartnerType Type { get; set; }
        public string? Phone1 { get; set; }
        public string? Phone2 { get; set; }
        public string? Email { get; set; }
        public List<PartnerAddress>? Addresses { get; set; }
    }

    public enum PartnerType
    {
        Customer = 1,
        Supplier = 2,
    }
}
